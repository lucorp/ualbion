﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using UAlbion.Api;

#pragma warning disable CA2227 // Collection properties should be read only
namespace UAlbion.Config
{
    public class AssetInfo
    {
        public string Id { get; set; } // Id of this asset in the mapped enum type.
        [JsonInclude, JsonExtensionData] public Dictionary<string, object> Properties { get; private set; } // Supported: string, bool, int
        [JsonIgnore] public AssetId AssetId { get; set; }
        [JsonIgnore] public int Index { get; set; } // Sub-asset offset in the container file (or 0 if not inside a container)
        [JsonIgnore] public AssetFileInfo File { get; set; }

        public int Width // For sprites only
        {
            get => Get(AssetProperty.Width, File?.Width ?? 0);
            set => Set(AssetProperty.Width, value == 0 ? (object)null : value);
        }

        public int Height // For sprites only
        {
            get => Get(AssetProperty.Height, File?.Height ?? 0);
            set => Set(AssetProperty.Height, value == 0 ? (object)null : value);
        }

        public AssetInfo() { }
        public AssetInfo(Dictionary<string, object> properties)
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));
            foreach(var property in properties)
                Set(property.Key, property.Value);
        }

        public override string ToString() => $"I:{AssetId} ({File.Filename}.{Index})";

        public T Get<T>(string property, T defaultValue)
        {
            if (Properties == null || !Properties.TryGetValue(property, out var token))
                return File != null ? File.Get(property, defaultValue) : defaultValue;

            if (token is JsonElement elem)
            {
                if (typeof(T) == typeof(string)) return (T)(object)elem.GetString();
                if (typeof(T) == typeof(int)) return (T)(object)elem.GetInt32();
                if (typeof(T) == typeof(long)) return (T)(object)elem.GetInt64();
                if (typeof(T) == typeof(bool)) return (T)(object)elem.GetBoolean();

                if (typeof(T).IsAssignableTo(typeof(IAssetId)))
                {
                    var id = elem.GetString();
                    return CastHelper<AssetId, T>.Cast(File.ResolveId(id));
                }

                if (typeof(T).IsEnum)
                    return (T)Enum.Parse(typeof(T), elem.GetString() ?? "");
            }
            //else if (token is double asDouble)
            //{
            //    if (typeof(T) == typeof(int)) return (T)(object)Convert.ToInt32(asDouble);
            //    if (typeof(T) == typeof(int?)) return (T)(object)Convert.ToInt32(asDouble);
            //}

            return (T)token;
        }

        public void Set<T>(string property, T value)
        {
            if (value == null)
            {
                if (Properties == null)
                    return;

                Properties.Remove(property);
                if (Properties.Count == 0)
                    Properties = null;
            }
            else
            {
                Properties ??= new Dictionary<string, object>();
                Properties[property] = value;
            }
        }

        public string BuildFilename(string pattern, int frameNum, string overrideName = null)
            => string.Format(CultureInfo.InvariantCulture,
                    pattern,
                    Index,    // 0 = Index in container
                    frameNum, // 1 = frame/sub-asset number
                    string.IsNullOrEmpty(overrideName) ? ConfigUtil.AssetName(AssetId) : overrideName, // 2 = asset name
                    Get(AssetProperty.PaletteId, 0)); // 3 = palette id

        static readonly Dictionary<string, Regex> RegexCache = new();
        static readonly Regex ParameterRegex = new(@"\\{(\d+)(:[^}]+)?}", RegexOptions.Compiled);
        public static (int, int, int?, string) ParseFilename(string pattern, string filename) // Return index and sub-asset number, may set palette id.
        {
            Regex regex;
            lock (RegexCache)
            {
                if (!RegexCache.TryGetValue(pattern, out regex))
                {
                    var replaced = ParameterRegex.Replace(
                        Regex.Escape(pattern),
                        x => x.Groups[1].Value switch
                        {
                            "0" => @"(?<Index>\d+)",
                            "1" => @"(?<SubAsset>\d+)",
                            "2" => @"(?<Name>\w+)",
                            "3" => @"(?<Palette>\d+)",
                            _ => x.Groups[1].Value
                        });
                    regex = new Regex(replaced);
                    RegexCache[pattern] = regex;
                }
            }

            var m = regex.Match(filename);
            if (!m.Success)
                return (-1, -1, null, null);

            var indexGroup = m.Groups["Index"];
            var subAssetGroup = m.Groups["SubAsset"];
            var paletteGroup = m.Groups["Palette"];
            int? paletteId = paletteGroup.Success ? (int?)int.Parse(paletteGroup.Value, CultureInfo.InvariantCulture) : null;

            int index = indexGroup.Success ? int.Parse(indexGroup.Value, CultureInfo.InvariantCulture) : -1;
            int subAsset = subAssetGroup.Success ? int.Parse(subAssetGroup.Value, CultureInfo.InvariantCulture) : 0;

            return (index, subAsset, paletteId, m.Groups["Name"].Value);
        }
    }
}
#pragma warning restore CA2227 // Collection properties should be read only
