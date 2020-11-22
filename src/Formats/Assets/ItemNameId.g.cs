// Note: This file was automatically generated using Tools/GenerateEnums.
// No changes should be made to this file by hand. Instead, the relevant json
// files should be modified and then GenerateEnums should be used to regenerate
// the various types.
using System;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;
using SerdesNet;
using UAlbion.Api;
using UAlbion.Config;

namespace UAlbion.Formats.Assets
{
    [JsonConverter(typeof(ToStringJsonConverter))]
    [TypeConverter(typeof(ItemNameIdConverter))]
    public struct ItemNameId : IEquatable<ItemNameId>, IEquatable<AssetId>, ITextureId
    {
        readonly uint _value;
        public ItemNameId(AssetType type, int id = 0)
        {
            if (!(type == AssetType.None || type == AssetType.ItemName))
                throw new ArgumentOutOfRangeException($"Tried to construct a ItemNameId with a type of {type}");
#if DEBUG
            if (id < 0 || id > 0xffffff)
                throw new ArgumentOutOfRangeException($"Tried to construct a ItemNameId with out of range id {id}");
#endif
            _value = (uint)type << 24 | (uint)id;
        }

        ItemNameId(uint id) 
        {
            _value = id;
            if (!(Type == AssetType.None || Type == AssetType.ItemName))
                throw new ArgumentOutOfRangeException($"Tried to construct a ItemNameId with a type of {Type}");
        }

        public static ItemNameId From<T>(T id) where T : unmanaged, Enum => (ItemNameId)AssetMapping.Global.EnumToId(id);

        public int ToDisk(AssetMapping mapping)
        {
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));
            var (enumType, enumValue) = AssetMapping.Global.IdToEnum(this);
            return mapping.EnumToId(enumType, enumValue).Id;
        }

        public static ItemNameId FromDisk(int disk, AssetMapping mapping)
        {
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));
            

            var (enumType, enumValue) = mapping.IdToEnum(new ItemNameId(AssetType.ItemName, disk));
            return (ItemNameId)AssetMapping.Global.EnumToId(enumType, enumValue);
        }

        public static ItemNameId SerdesU8(string name, ItemNameId id, AssetMapping mapping, ISerializer s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));

            byte diskValue = (byte)id.ToDisk(mapping);
            diskValue = s.UInt8(name, diskValue);
            return FromDisk(diskValue, mapping);
        }

        public static ItemNameId SerdesU16(string name, ItemNameId id, AssetMapping mapping, ISerializer s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));

            ushort diskValue = (ushort)id.ToDisk(mapping);
            diskValue = s.UInt16(name, diskValue);
            return FromDisk(diskValue, mapping);
        }

        public readonly AssetType Type => (AssetType)((_value & 0xff00_0000) >> 24);
        public readonly int Id => (int)(_value & 0xffffff);
        public static ItemNameId None => new ItemNameId(AssetType.None);
        public bool IsNone => Type == AssetType.None;

        public override string ToString() => AssetMapping.Global.IdToName(this);
        static AssetType[] _validTypes = { AssetType.ItemName };
        public static ItemNameId Parse(string s) => AssetMapping.Global.Parse(s, _validTypes);

        public static implicit operator AssetId(ItemNameId id) => AssetId.FromUInt32(id._value);
        public static implicit operator ItemNameId(AssetId id) => new ItemNameId(id.ToUInt32());
        public static implicit operator TextId(ItemNameId id) => TextId.FromUInt32(id._value);
        public static explicit operator ItemNameId(TextId id) => new ItemNameId(id.ToUInt32());
        public static implicit operator ItemNameId(UAlbion.Base.ItemName id) => ItemNameId.From(id);

        public readonly int ToInt32() => unchecked((int)_value);
        public readonly uint ToUInt32() => _value;
        public static ItemNameId FromInt32(int id) => new ItemNameId(unchecked((uint)id));
        public static ItemNameId FromUInt32(uint id) => new ItemNameId(id);
        public static bool operator ==(ItemNameId x, ItemNameId y) => x.Equals(y);
        public static bool operator !=(ItemNameId x, ItemNameId y) => !(x == y);
        public static bool operator ==(ItemNameId x, AssetId y) => x.Equals(y);
        public static bool operator !=(ItemNameId x, AssetId y) => !(x == y);
        public bool Equals(ItemNameId other) => _value == other._value;
        public bool Equals(AssetId other) => _value == other.ToUInt32();
        public override bool Equals(object obj) => obj is ITextureId other && Equals(other);
        public override int GetHashCode() => unchecked((int)_value);
    }

    public class ItemNameIdConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) 
            => sourceType == typeof(string) ? true : base.CanConvertFrom(context, sourceType);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) 
            => value is string s ? ItemNameId.Parse(s) : base.ConvertFrom(context, culture, value);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) =>
            destinationType == typeof(string) ? value.ToString() : base.ConvertTo(context, culture, value, destinationType);
    }
}