﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace UAlbion.Config
{
    public class GeneralConfig : IGeneralConfig
    {
        static readonly Regex Pattern = new Regex(@"(\$\([A-Z]+\))");
        [JsonIgnore] public string BasePath { get; set; }
        public IDictionary<string, string> Paths { get; } = new Dictionary<string, string>();
        public IList<string> SearchPaths { get; } = new List<string>();

        public static GeneralConfig Load(string configPath, string baseDir)
        {
            var config = File.Exists(configPath) 
                ? JsonConvert.DeserializeObject<GeneralConfig>(File.ReadAllText(configPath)) 
                : new GeneralConfig();

            config.BasePath = baseDir;
            return config;
        }

        public string ResolvePath(string relative)
        {
            if (string.IsNullOrEmpty(relative))
                throw new ArgumentNullException(nameof(relative));

            if (relative.Contains(".."))
                throw new ArgumentOutOfRangeException($"Paths containing .. are not allowed ({relative})");

            if (relative.Contains(":") && !relative.StartsWith(BasePath, StringComparison.InvariantCulture))
                throw new ArgumentOutOfRangeException($"Paths containing : are not allowed ({relative})");

            relative = Pattern.Replace(relative, x =>
            {
                var name = x.Groups[0].Value.Substring(2).TrimEnd(')').ToUpperInvariant();
                if(!Paths.TryGetValue(name, out var value))
                    throw new InvalidOperationException($"Could not find path substitution for {name} in path {relative}");
                return value;
            });

            return Path.Combine(BasePath, relative);
        }
    }
}
