﻿using System;
using System.Numerics;
using UAlbion.Formats.AssetIds;
using UAlbion.Formats.Assets;

namespace UAlbion.Game.Entities
{
    public class LargeNpcSprite : LargeCharacterSprite<LargeNpcId>
    {
        readonly MapNpc.Waypoint[] _waypoints;
        public override string ToString() => $"LNpcSprite {Id} {Animation}";

        public LargeNpcSprite(LargeNpcId id, MapNpc.Waypoint[] waypoints) : base(id, new Vector2(waypoints[0].X, waypoints[0].Y))
        {
            _waypoints = waypoints;
            Animation = (LargeSpriteAnimation)new Random().Next((int)LargeSpriteAnimation.UpperBody);
        }
    }
}