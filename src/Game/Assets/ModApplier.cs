﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAlbion.Api;
using UAlbion.Config;
using UAlbion.Core;
using UAlbion.Formats;
using UAlbion.Formats.Assets.Save;
using UAlbion.Game.Events;
using UAlbion.Game.Settings;

namespace UAlbion.Game.Assets
{
    public class ModApplier : Component, IModApplier
    {
        readonly IDictionary<Type, IAssetPostProcessor> _postProcessors = new Dictionary<Type, IAssetPostProcessor>();
        readonly Dictionary<string, ModInfo> _mods = new Dictionary<string, ModInfo>();
        readonly List<ModInfo> _modsInReverseDependencyOrder = new List<ModInfo>();
        readonly AssetCache _assetCache = new AssetCache();
        IAssetLocator _assetLocator;
        GameLanguage _language = GameLanguage.English;

        public ModApplier()
        {
            AttachChild(_assetCache);
            On<SetLanguageEvent>(e =>
            {
                if (_language == e.Language)
                    return;

                _language = e.Language;
                Raise(new ReloadAssetsEvent());
                Raise(new LanguageChangedEvent());
            });
        }

        public IModApplier AddAssetPostProcessor(IAssetPostProcessor postProcessor)
        {
            if (postProcessor == null) throw new ArgumentNullException(nameof(postProcessor));
            foreach (var type in postProcessor.SupportedTypes)
            {
                if(_postProcessors.ContainsKey(type))
                    throw new InvalidOperationException($"A post-processor is already defined for {type}");
                _postProcessors[type] = postProcessor;
            }

            if (postProcessor is IComponent component)
                AttachChild(component);

            return this;
        }

        protected override void Subscribed()
        {
            _assetLocator = Resolve<IAssetLocator>();
            var config = Resolve<IGeneralConfig>();
            Exchange.Register<IModApplier>(this);

            LoadMod(config.ResolvePath("$(MODS)"), "Base");

            foreach (var mod in Resolve<IGameplaySettings>().Mods)
                LoadMod(config.ResolvePath("$(MODS)"), mod);

            _modsInReverseDependencyOrder.Reverse();
        }

        void LoadMod(string dataDir, string modName)
        {
            if (string.IsNullOrEmpty(modName))
                return;

            if (modName.Any(c => c == '\\' || c == '/' || c == Path.DirectorySeparatorChar))
            {
                Raise(new LogEvent(LogEvent.Level.Error, $"Mod {modName} is not a simple directory name"));
                return;
            }

            string path = Path.Combine(dataDir, modName);

            var assetConfigPath = Path.Combine(path, "assets.json");
            if (!File.Exists(assetConfigPath))
            {
                Raise(new LogEvent(LogEvent.Level.Error, $"Mod {modName} does not contain an asset.config file"));
                return;
            }

            var modConfigPath = Path.Combine(path, "modinfo.json");
            if (!File.Exists(modConfigPath))
            {
                Raise(new LogEvent(LogEvent.Level.Error, $"Mod {modName} does not contain an modinfo.config file"));
                return;
            }

            var assetConfig = AssetConfig.Load(assetConfigPath);
            var modConfig = ModConfig.Load(modConfigPath);
            var modInfo = new ModInfo(modName, assetConfig, modConfig, path);

            // Load dependencies
            foreach (var dependency in modConfig.Dependencies)
            {
                LoadMod(dataDir, dependency);
                if (!_mods.TryGetValue(dependency, out var dependencyInfo))
                {
                    Raise(new LogEvent(LogEvent.Level.Error, $"Dependency {dependency} of mod {modName} could not be loaded, skipping load of {modName}"));
                    return;
                }

                modInfo.Mapping.MergeFrom(dependencyInfo.Mapping);
            }

            MergeTypesToMapping(modInfo.Mapping, assetConfig, assetConfigPath);
            AssetMapping.Global.MergeFrom(modInfo.Mapping);
            assetConfig.PopulateAssetIds(AssetMapping.Global);
            _mods.Add(modName, modInfo);
            _modsInReverseDependencyOrder.Add(modInfo);
        }

        static void MergeTypesToMapping(AssetMapping mapping, AssetConfig config, string assetConfigPath)
        {
            foreach (var assetType in config.Types)
            {
                var enumType = Type.GetType(assetType.Key);
                if (enumType == null)
                    throw new InvalidOperationException($"Could not load enum type \"{assetType.Key}\" defined in \"{assetConfigPath}\"");

                mapping.RegisterAssetType(assetType.Key, assetType.Value.AssetType);
            }
        }

        public AssetInfo GetAssetInfo(AssetId id)
        {
            var (typeName, enumId) = AssetMapping.Global.IdToEnumString(id);
            return _modsInReverseDependencyOrder
                .Select(x => x.AssetConfig.GetAsset(typeName, enumId))
                .FirstOrDefault(x => x != null);
        }

        public object LoadAsset(AssetId id)
        {
            try { return LoadAssetInternal(id); }
            catch (Exception e)
            {
                Raise(new LogEvent(LogEvent.Level.Error, $"Could not load asset {id}: {e}"));
                return null;
            }
        }

        public object LoadAssetCached(AssetId id)
        {
            object asset = _assetCache.Get(id);
            if (asset is Exception) // If it failed to load once then stop trying (at least until an asset:reload / cycle)
                return null;

            if (asset != null)
                return asset;

            try
            {
                asset = LoadAssetInternal(id);
            }
            catch (Exception e)
            {
                Raise(new LogEvent(LogEvent.Level.Error, $"Could not load asset {id}: {e}"));
                asset = e;
            }

            _assetCache.Add(asset, id);
            return asset is Exception ? null : asset;
        }

        object LoadAssetInternal(AssetId id)
        {
            var (typeName, enumId) = AssetMapping.Global.IdToEnumString(id);
            object asset = null;
            Stack<IPatch> patches = null; // Create the stack lazily, as most assets won't have any patches.
            foreach (var mod in _modsInReverseDependencyOrder)
            {
                var info = mod.AssetConfig.GetAsset(typeName, enumId);
                if (info == null && typeName != AssetMapping.UnknownType)
                    continue;

                var context = new SerializationContext(mod.Mapping, _language, mod.Path);
                var modAsset = _assetLocator.LoadAsset(id, context, info);
                if (modAsset is IPatch patch)
                {
                    patches ??= new Stack<IPatch>();
                    patches.Push(patch);
                }
                else if (modAsset != null)
                {
                    asset = modAsset;
                    break;
                }
            }

            if (asset == null)
                throw new InvalidOperationException($"Could not load asset for {id}");

            while (patches != null && patches.Count > 0)
                asset = patches.Pop().Apply(asset);

            ICoreFactory factory = Resolve<ICoreFactory>();
            if (_postProcessors.TryGetValue(asset.GetType(), out var processor))
                asset = processor.Process(factory, id, asset);

            return asset;
        }

        public SavedGame LoadSavedGame(string path)
        {
            throw new NotImplementedException();
        }
    }
}