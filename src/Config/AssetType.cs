﻿using System.Text.Json.Serialization;

namespace UAlbion.Config
{
    /// <summary>
    /// An asset type corresponds to a particular type of asset object intended for a particular purpose.
    /// e.g. a character sheet asset intended for use as a party member has a different AssetType to an NPC's character sheet.
    /// If this enum is modified, then CodeGeneration must be re-run.
    /// Any AssetTypes in a family should have their own AssetId type, in additional to any more general id types they may be a part of.
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter<AssetType>))]
    public enum AssetType : byte
    {
        [Unmapped] None = 0, // Must be 0 so default(AssetId) will equate to AssetId.None

        AutomapGraphics,
        BackgroundGraphics,
        CombatBackground,
        CombatGraphics,
        CoreGraphics,
        Floor,
        Font,
        [IsomorphicTo(Party)] FullBodyPicture,
        ItemGraphics,
        LargeNpcGraphics,
        LargePartyGraphics,
        MonsterGraphics,
        Object3D,
        Picture,
        Slab,
        SmallNpcGraphics,
        SmallPartyGraphics,
        Portrait,
        TacticalIcon,
        [IsomorphicTo(TilesetData)] TilesetGraphics,
        Wall,
        WallOverlay,

        Chest,
        Merchant,
        Door,

        EventSet,
        Script,

        [Unmapped] Gold,
        [Unmapped] Rations,
        Item,

        Labyrinth,

        Map,
        [IsomorphicTo(Map)] Automap,

        Monster,
        Npc,
        Party,

        [Unmapped] MetaFont,
        MonsterGroup,
        [Unmapped] ObjectGroup,
        Palette,
        Sample,
        Song,
        [IsomorphicTo(Song)] WaveLibrary,
        Spell,
        Switch,
        Ticker,
        TilesetData,
        [IsomorphicTo(TilesetData)] BlockList,
        [Localised] Video,

        [Localised] [IsomorphicTo(EventSet)] EventText,
        [Localised] [IsomorphicTo(Map)] MapText,
        [Localised] [IsomorphicTo(Item)] ItemName,
        [Localised] Text,
        [Localised] Word,

        Special = 254, // For various types that only have a single value
        [Unmapped] Unknown = 255
    }
}
