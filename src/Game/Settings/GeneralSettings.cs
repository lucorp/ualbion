﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAlbion.Api;
using UAlbion.Config;
using UAlbion.Core;

namespace UAlbion.Game.Settings
{
    public class GeneralSettings : IDebugSettings, IAudioSettings, IGameplaySettings, IEngineSettings
    {
        const string UserPath = "$(CONFIG)/settings.json";
        const string DefaultsPath = "$(DATA)/settings.defaults.json";

        // Debug
        public DebugFlags DebugFlags { get; set; }

        // Audio
        public int MusicVolume { get; set; } = 32;
        public int FxVolume { get; set; } = 48;

        // Graphics

        // Gameplay
        public string Language { get; set; } = Base.Language.English;
        public int CombatDelay { get; set; } = 3;
        public IList<string> ActiveMods { get; } = new List<string>();

        // Engine
        public float Special1 { get; set; }
        public float Special2 { get; set; }
        public EngineFlags Flags { get; set; } = EngineFlags.VSync; 

        public static GeneralSettings Load(IGeneralConfig config, IFileSystem disk, IJsonUtil jsonUtil)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (disk == null) throw new ArgumentNullException(nameof(disk));
            if (jsonUtil == null) throw new ArgumentNullException(nameof(jsonUtil));

            var path = config.ResolvePath(UserPath);
            if (!disk.FileExists(path))
                path = config.ResolvePath(DefaultsPath);

            if (!disk.FileExists(path))
                throw new FileNotFoundException($"Could not find default settings file (expected at {path})");

            var settings = disk.FileExists(path) 
                ? jsonUtil.Deserialize<GeneralSettings>(disk.ReadAllBytes(path)) 
                : new GeneralSettings();

            if (!settings.ActiveMods.Any())
                settings.ActiveMods.Add("Base");
            return settings;
        }

        public void Save(IGeneralConfig config, IFileSystem disk, IJsonUtil jsonUtil)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (disk == null) throw new ArgumentNullException(nameof(disk));
            if (jsonUtil == null) throw new ArgumentNullException(nameof(jsonUtil));

            var path = config.ResolvePath(UserPath);
            var dir = Path.GetDirectoryName(path);
            if (!disk.DirectoryExists(dir))
                disk.CreateDirectory(dir);

            var json = jsonUtil.Serialize(this);
            disk.WriteAllText(path, json);
        }
    }
}
