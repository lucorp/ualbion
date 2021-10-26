﻿using System;
using System.Globalization;
using System.Xml.Serialization;

namespace UAlbion.Formats.Exporters.Tiled
{
    public class TiledProperty
    {
        public TiledProperty() { }
        public TiledProperty(string key, string value)
        {
            Name = key;
            if (value != null && (value.Contains('\n', StringComparison.Ordinal) || value.Contains('\r', StringComparison.Ordinal)))
                MultiLine = value;
            else
                Value = value;
        }

        public TiledProperty(string key, int value)
        {
            Name = key;
            Value = value.ToString(CultureInfo.InvariantCulture);
            Type = TiledPropertyType.Int;
        }

        public TiledProperty(string key, float value)
        {
            Name = key;
            Value = value.ToString(CultureInfo.InvariantCulture);
            Type = TiledPropertyType.Float;
        }

        public TiledProperty(string key, bool value)
        {
            Name = key;
            Value = value.ToString(CultureInfo.InvariantCulture);
            Type = TiledPropertyType.Bool;
        }

#pragma warning disable CA1720 // Identifier contains type name
        public static TiledProperty Object(string key, int objectId) => new()
        {
            Name = key,
            Type = TiledPropertyType.Object,
            Value = objectId.ToString(CultureInfo.InvariantCulture)
        };
#pragma warning restore CA1720 // Identifier contains type name

        [XmlAttribute("name")] public string Name { get; set; }
        [XmlAttribute("value")] public string Value { get; set; }
        [XmlAttribute("type")] public TiledPropertyType Type { get; set; }
        [XmlIgnore] public bool TypeSpecified => Type != TiledPropertyType.String;
        [XmlText] public string MultiLine { get; set; }
    }
}