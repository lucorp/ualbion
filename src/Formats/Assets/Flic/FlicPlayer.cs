﻿using System;
using System.Collections.Generic;
using System.Linq;
using UAlbion.Api;

namespace UAlbion.Formats.Assets.Flic
{
    public class FlicPlayer
    {
        readonly FlicFile _flic;
        readonly FlicFrame[] _frames;

        public FlicPlayer(FlicFile flic, byte[] pixelData)
        {
            if (flic == null) throw new ArgumentNullException(nameof(flic));
            if (pixelData == null) throw new ArgumentNullException(nameof(pixelData));

            if (pixelData.Length != flic.Width * flic.Height)
            {
                throw new ArgumentOutOfRangeException(
                    "FlicFile.Player: Expected a pixel buffer " +
                    $"of size {flic.Width * flic.Height} ({flic.Width}x{flic.Height})," +
                    $" but was given a buffer of size {pixelData.Length}");
            }

            PixelData = pixelData;
            _flic = flic;
            _frames = flic.Chunks.OfType<FlicFrame>().ToArray();
            ApplyFrame(_frames[0]);
        }

        public uint[] Palette { get; } = new uint[0x100];
        public byte[] PixelData { get; }
        public int Frame { get; private set; }
        public ushort Delay => _frames[Frame].Delay;
        public int FrameCount => _frames.Length;

        void ApplyFrame(FlicFrame frame)
        {
            ApiUtil.Assert(frame.Width == 0, "Frame width overrides are not currently handled");
            ApiUtil.Assert(frame.Height == 0, "Frame height overrides are not currently handled");

            foreach (var subChunk in frame.SubChunks)
            {
                switch (subChunk)
                {
                    case Palette8Chunk paletteChunk:
                        paletteChunk.GetEffectivePalette(Palette).CopyTo(Palette, 0);
                        break;
                    case CopyChunk copy:
                        copy.PixelData.CopyTo(PixelData, 0);
                        break;
                    case DeltaFlcChunk delta:
                        delta.Apply(PixelData, _flic.Width);
                        break;
                    case FullByteOrientedRleChunk rle:
                        rle.PixelData.CopyTo(PixelData, 0);
                        break;
                }
            }
        }

        public void NextFrame()
        {
            Frame++;
            if (Frame >= _flic.Frames)
            {
                Frame %= _flic.Frames;
                // if we have a ring frame, use it.
                if (Frame == 0 && _frames.Length > _flic.Frames)
                    Frame = _flic.Frames;
            }

            var frame = _frames[Frame];
            ApplyFrame(frame);
        }

        public IEnumerable<(uint[], ushort)> AllFrames32() // pixels, delay
        {
            uint[] buffer32 = new uint[_flic.Width * _flic.Height];
            while (Frame < _flic.Frames)
            {
                // Inefficient, could be optimised by rendering with an 8-bit shader or
                // applying the deltas directly to the 32-bit buffer.
                for (int y = 0; y < _flic.Height; y++)
                for (int x = 0; x < _flic.Width; x++)
                    buffer32[(_flic.Height - y - 1) * _flic.Width + x] = Palette[PixelData[y * _flic.Width + x]];

                yield return (buffer32, Delay);
                NextFrame();
            }
        }
    }
}