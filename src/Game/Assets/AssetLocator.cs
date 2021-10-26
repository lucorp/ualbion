﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using SerdesNet;
using UAlbion.Api;
using UAlbion.Config;
using UAlbion.Core;
using UAlbion.Formats;

namespace UAlbion.Game.Assets
{
    public sealed class AssetLocator : ServiceComponent<IAssetLocator>, IAssetLocator
    {
        readonly IDictionary<string, string> _hashCache = new Dictionary<string, string>();
        IAssetLoaderRegistry _assetLoaderRegistry;
        IContainerRegistry _containerRegistry;

        protected override void Subscribed()
        {
            _assetLoaderRegistry = Resolve<IAssetLoaderRegistry>();
            _containerRegistry = Resolve<IContainerRegistry>();
            base.Subscribed();
        }

        public object LoadAsset(AssetId id, AssetMapping mapping, AssetInfo info, IDictionary<string, string> extraPaths)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));
            if (extraPaths == null) throw new ArgumentNullException(nameof(extraPaths));
            var generalConfig = Resolve<IGeneralConfig>();
            var jsonUtil = Resolve<IJsonUtil>();

            using ISerializer s = Search(generalConfig, info, extraPaths);
            if (s == null)
                return null;

            if (s.BytesRemaining == 0) // Happens all the time when dumping, just return rather than throw to preserve perf.
                return new AssetNotFoundException($"Asset for {info.AssetId} found but size was 0 bytes.", info.AssetId);

            var loader = _assetLoaderRegistry.GetLoader(info.File.Loader);
            if (loader == null)
                throw new InvalidOperationException($"Could not instantiate loader \"{info.File.Loader}\" required by asset {info.AssetId}");

            return loader.Serdes(null, info, mapping, s, jsonUtil);
        }

        public List<(int,int)> GetSubItemRangesForFile(AssetFileInfo info, IDictionary<string, string> extraPaths)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            var generalConfig = Resolve<IGeneralConfig>();
            var disk = Resolve<IFileSystem>();
            var jsonUtil = Resolve<IJsonUtil>();
            var resolved = generalConfig.ResolvePath(info.Filename, extraPaths);
            var container = _containerRegistry.GetContainer(resolved, info.Container, disk);
            return container?.GetSubItemRanges(resolved, info, disk, jsonUtil) ?? new List<(int, int)> { (0, 1) };
        }

        ISerializer Search(IGeneralConfig generalConfig, AssetInfo info, IDictionary<string, string> extraPaths)
        {
            var path = generalConfig.ResolvePath(info.File.Filename, extraPaths);
            var disk = Resolve<IFileSystem>();
            var jsonUtil = Resolve<IJsonUtil>();
            if (info.File.Sha256Hash != null && !info.File.Sha256Hash.Equals(GetHash(path, disk), StringComparison.OrdinalIgnoreCase))
                return null;

            var container = _containerRegistry.GetContainer(path, info.File.Container, disk);
            return container?.Read(path, info, disk, jsonUtil);
        }

        string GetHash(string filename, IFileSystem disk)
        {
            if (_hashCache.TryGetValue(filename, out var hash))
                return hash;

            using var sha256 = SHA256.Create();
            using var stream = disk.OpenRead(filename);
            var hashBytes = sha256.ComputeHash(stream);
            hash = FormatUtil.BytesToHexString(hashBytes);

            _hashCache[filename] = hash;
            return hash;
        }
    }
}
