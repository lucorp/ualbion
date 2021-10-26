﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UAlbion.Api;
using UAlbion.Config;
using UAlbion.Formats.Assets;
using UAlbion.Formats.Assets.Labyrinth;
using UAlbion.Formats.Assets.Maps;
using UAlbion.Game;

namespace UAlbion
{
    static class DumpJson
    {
        public static void Dump(string baseDir, IAssetManager assets, ISet<AssetType> types, AssetId[] dumpIds)
        {
            var disposeList = new List<IDisposable>();

            TextWriter Writer(string name)
            {
                var filename = Path.Combine(baseDir, "data", "exported", "json", name);
                var directory = Path.GetDirectoryName(filename);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                var stream = File.Open(filename, FileMode.Create);
                var writer = new StreamWriter(stream);
                disposeList.Add(writer);
                disposeList.Add(stream);
                return writer;
            }

            void Flush()
            {
                foreach (var d in disposeList)
                    d.Dispose();
                disposeList.Clear();
            }

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            };

            var s = JsonSerializer.Create(settings);
            TextWriter tw;
            if (types.Contains(AssetType.TilesetData))
            {
                foreach (var id in DumpUtil.All(AssetType.TilesetData, dumpIds))
                {
                    TilesetData asset = assets.LoadTileData(id);
                    if (asset == null) continue;
                    tw = Writer($"tilesets/tileset{id.Id}.json");
                    s.Serialize(tw, asset);
                }

                Flush();
            }

            if (types.Contains(AssetType.Labyrinth))
            {
                foreach (var id in DumpUtil.All(AssetType.Labyrinth, dumpIds))
                {
                    LabyrinthData asset = assets.LoadLabyrinthData(id);
                    if (asset == null) continue;
                    tw = Writer($"labdata/labyrinth{id.Id}.json");
                    s.Serialize(tw, asset);
                }

                Flush();
            }

            // string str = assets.LoadString(StringId id, GameLanguage language);

            if (types.Contains(AssetType.Map))
            {
                foreach (var id in DumpUtil.All(AssetType.Map, dumpIds))
                {
                    IMapData asset = assets.LoadMap(id);
                    if (asset == null) continue;
                    tw = Writer($"maps/map{id.Id}_{id}.json");
                    s.Serialize(tw, asset);
                }

                Flush();
            }

            if (types.Contains(AssetType.Item))
            {
                tw = Writer("items.json");
                s.Serialize(tw, DumpUtil.AllAssets(AssetType.Item, dumpIds, x => assets.LoadItem(x)));
                Flush();
            }

            if (types.Contains(AssetType.Party))
            {
                tw = Writer("party_members.json");
                s.Serialize(tw, DumpUtil.AllAssets(AssetType.Party, dumpIds, x => assets.LoadSheet(x)));
                Flush();

            }

            if (types.Contains(AssetType.Npc))
            {
                tw = Writer("npcs.json");
                s.Serialize(tw, DumpUtil.AllAssets(AssetType.Npc, dumpIds, x => assets.LoadSheet(x)));
                Flush();
            }

            if (types.Contains(AssetType.Monster))
            {

                tw = Writer("monsters.json");
                s.Serialize(tw, DumpUtil.AllAssets(AssetType.Monster, dumpIds, x => assets.LoadSheet(x)));
                Flush();
            }

            if (types.Contains(AssetType.Chest))
            {

                tw = Writer("chests.json");
                s.Serialize(tw, DumpUtil.AllAssets(AssetType.Chest, dumpIds, assets.LoadInventory));
                Flush();
            }

            if (types.Contains(AssetType.Merchant))
            {

                tw = Writer("merchants.json");
                s.Serialize(tw, DumpUtil.AllAssets(AssetType.Merchant, dumpIds, assets.LoadInventory));
                Flush();
            }

            if (types.Contains(AssetType.BlockList))
            {
                foreach (var id in DumpUtil.All(AssetType.BlockList, dumpIds))
                {
                    IList<Block> asset = assets.LoadBlockList(id);
                    if (asset == null) continue;
                    tw = Writer($"blocks/blocklist{id.Id}.json");
                    s.Serialize(tw, asset);
                }
                Flush();
            }

            if (types.Contains(AssetType.EventSet))
            {
                tw = Writer("event_sets.json");
                s.Serialize(tw, DumpUtil.AllAssets(AssetType.EventSet, dumpIds, x => assets.LoadEventSet(x)));
                Flush();
            }

            if (types.Contains(AssetType.Script))
            {
                foreach (var id in DumpUtil.All(AssetType.Script, dumpIds))
                {
                    IList<IEvent> asset = assets.LoadScript(id);
                    if (asset == null) continue;
                    tw = Writer($"scripts/script{id.Id}.json");
                    s.Serialize(tw, asset.Select(x => x.ToString()).ToArray());
                }
                Flush();
            }

            if (types.Contains(AssetType.Spell))
            {
                tw = Writer("spells.json");
                s.Serialize(tw, DumpUtil.AllAssets(AssetType.Spell, dumpIds, x => assets.LoadSpell(x)));
                Flush();
            }

            if (types.Contains(AssetType.MonsterGroup))
            {
                tw = Writer("monster_groups.json");
                s.Serialize(tw, DumpUtil.AllAssets(AssetType.MonsterGroup, dumpIds, x => assets.LoadMonsterGroup(x)));
                Flush();
            }

            if (types.Contains(AssetType.Palette))
            {
                foreach (var id in DumpUtil.All(AssetType.Palette, dumpIds))
                {
                    tw = Writer($"palettes/palette{id.Id}_{id}.json");
                    var palette = assets.LoadPalette(id);
                    s.Serialize(tw, palette);
                }
                Flush();
            }
        }
    }
}
