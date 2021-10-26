﻿using UAlbion.Api;

namespace UAlbion.Game.Veldrid.Assets
{
    [Event("iso_pal")]
    public class IsoPaletteEvent : Event, IVerboseEvent
    {
        public IsoPaletteEvent(int delta) => Delta = delta;
        [EventPart("delta")] public int Delta { get; }
    }
}