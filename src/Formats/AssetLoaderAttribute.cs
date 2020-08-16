﻿using System;
using UAlbion.Formats.Config;

namespace UAlbion.Formats
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AssetLoaderAttribute : Attribute
    {
        public FileFormat[] SupportedFormats { get; }
        public AssetLoaderAttribute(params FileFormat[] supportedFormats)
        {
            SupportedFormats = supportedFormats;
        }
    }
}
