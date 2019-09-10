﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace UAlbion.Core.Visual
{
    public class MultiSprite : IRenderable
    {
        public MultiSprite(SpriteKey key)
        {
            Key = key;
        }

        public MultiSprite(SpriteKey key, int bufferId, IEnumerable<SpriteInstanceData> sprites)
        {
            Key = key;
            BufferId = bufferId;

            if (sprites is SpriteInstanceData[] array)
                Instances = array;
            else
                Instances = sprites.ToArray();
        }

        public void RotateSprites(Vector3 cameraPosition)
        {
            for(int i = 0; i < Instances.Length; i++)
            {
                if ((Instances[i].Flags & SpriteFlags.Billboard) == 0)
                    continue;

                var delta = Instances[i].Offset - cameraPosition;
                Instances[i].Rotation = -(float)Math.Atan2(delta.X, delta.Z);
            }
        }

        public string Name => Key.Texture.Name;
        public int RenderOrder => Key.RenderOrder;
        public Type Renderer => typeof(SpriteRenderer);
        public SpriteKey Key { get; }
        public int BufferId { get; set; }
        public SpriteInstanceData[] Instances { get; set; }
    }
}