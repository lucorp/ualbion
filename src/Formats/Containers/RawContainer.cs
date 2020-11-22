﻿using System;
using System.IO;
using SerdesNet;
using UAlbion.Api;
using UAlbion.Config;

namespace UAlbion.Formats.Containers
{
    [ContainerLoader(ContainerFormat.None)]
    public class RawContainerLoader : IContainerLoader
    {
        public ISerializer Open(string file, AssetInfo info)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));
            ApiUtil.Assert(info.SubAssetId == 0, "SubItem should always be 0 when accessing a non-container file");
            var stream = File.OpenRead(file);
            var br = new BinaryReader(stream);
            return new AlbionReader(br);
        }
    }
}
