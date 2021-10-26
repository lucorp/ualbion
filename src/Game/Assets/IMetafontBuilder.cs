﻿using UAlbion.Api.Visual;
using UAlbion.Formats.Assets;

namespace UAlbion.Game.Assets
{
    public interface IMetafontBuilder
    {
        ITexture Build(MetaFontId id);
    }
}