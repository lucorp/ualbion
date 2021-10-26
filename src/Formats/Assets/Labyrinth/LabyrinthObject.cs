﻿using System;
using System.ComponentModel;
using SerdesNet;
using UAlbion.Config;

namespace UAlbion.Formats.Assets.Labyrinth
{
    public class LabyrinthObject
    {
        public LabyrinthObjectFlags Properties { get; set; } // 0
        public uint Collision { get; set; } // 1, len = 3 bytes
        public SpriteId SpriteId { get; set; } // 4, ushort
        [DefaultValue(1)]
        public byte AnimationFrames { get; set; } // 6
        public byte Unk7 { get; set; } // 7
        public ushort Width { get; set; } // 8
        public ushort Height { get; set; } // A
        public ushort MapWidth { get; set; } // C
        public ushort MapHeight { get; set; } // E

        public override string ToString() =>
            $"EO.{SpriteId}:{AnimationFrames} {Width}x{Height} [{MapWidth}x{MapHeight}] {Properties}";

        public static LabyrinthObject Serdes(int _, LabyrinthObject o, AssetMapping mapping, ISerializer s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            o ??= new LabyrinthObject();
            o.Properties = s.EnumU8(nameof(o.Properties), o.Properties);

            // Either a 24 bit int or a 3 byte array, annoying to serialize either way.
            o.Collision = (o.Collision & 0xffff00) | s.UInt8(nameof(o.Collision), (byte)(o.Collision & 0xff));
            o.Collision = (o.Collision & 0xff00ff) | (uint)s.UInt8(nameof(o.Collision), (byte)((o.Collision >> 8) & 0xff)) << 8;
            o.Collision = (o.Collision & 0x00ffff) | (uint)s.UInt8(nameof(o.Collision), (byte)((o.Collision >> 16) & 0xff)) << 16;

            o.SpriteId = SpriteId.SerdesU16(nameof(SpriteId), o.SpriteId, AssetType.Object3D, mapping, s);
            o.AnimationFrames = s.UInt8(nameof(o.AnimationFrames), o.AnimationFrames);
            o.Unk7 = s.UInt8(nameof(o.Unk7), o.Unk7);
            o.Width = s.UInt16(nameof(o.Width), o.Width);
            o.Height = s.UInt16(nameof(o.Height), o.Height);
            o.MapWidth = s.UInt16(nameof(o.MapWidth), o.MapWidth);
            o.MapHeight = s.UInt16(nameof(o.MapHeight), o.MapHeight);
            return o;
        }
    }
}
