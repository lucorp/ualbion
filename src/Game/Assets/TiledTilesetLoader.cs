﻿using System;
using System.IO;
using SerdesNet;
using UAlbion.Api;
using UAlbion.Config;
using UAlbion.Core;
using UAlbion.Formats;
using UAlbion.Formats.Assets;
using UAlbion.Formats.Assets.Maps;
using UAlbion.Formats.Exporters.Tiled;

namespace UAlbion.Game.Assets
{
    public class TiledTilesetLoader : Component, IAssetLoader<TilesetData>
    {
        public object Serdes(object existing, AssetInfo info, AssetMapping mapping, ISerializer s, IJsonUtil jsonUtil)
            => Serdes((TilesetData)existing, info, mapping, s, jsonUtil);

        public TilesetData Serdes(TilesetData existing, AssetInfo info, AssetMapping mapping, ISerializer s, IJsonUtil jsonUtil)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));
            if (s == null) throw new ArgumentNullException(nameof(s));

            var graphicsId = ((TilesetId) info.AssetId).ToTilesetGraphics();
            var graphicsTemplate = info.Get(AssetProperty.GraphicsPattern, "{0}/{0}_{1}.png");
            var blankTilePath = info.Get(AssetProperty.BlankTilePath, "Blank.png");

            var properties = new Tilemap2DProperties
            {
                GraphicsTemplate = graphicsTemplate,
                BlankTilePath = blankTilePath,
                TilesetId = graphicsId.Id,
                TileWidth = 16,
                TileHeight = 16
            };

            return s.IsWriting() ? Save(existing, properties, s) : Load(info, properties, s);
        }

        static TilesetData Load(AssetInfo info, Tilemap2DProperties properties, ISerializer serializer)
        {
            var xmlBytes = serializer.Bytes(null, null, (int)serializer.BytesRemaining);
            using var ms = new MemoryStream(xmlBytes);
            var tileset = Tileset.Parse(ms);
            return tileset.ToAlbion(info.AssetId, properties);
        }

        static TilesetData Save(TilesetData tileset, Tilemap2DProperties properties, ISerializer s)
        {
            if (tileset == null) throw new ArgumentNullException(nameof(tileset));
            var tiledTileset = Tileset.FromAlbion(tileset, properties);
            var bytes = FormatUtil.BytesFromTextWriter(tiledTileset.Serialize);
            s.Bytes(null, bytes, bytes.Length);
            return tileset;
        }
    }
}