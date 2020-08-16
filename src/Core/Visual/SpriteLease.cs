﻿using System;

namespace UAlbion.Core.Visual
{
    public sealed class SpriteLease : IComparable<SpriteLease>, IDisposable
    {
        readonly MultiSprite _sprite;
        public SpriteKey Key => _sprite.Key;
        public int Length => To - From;
        internal int From { get; set; } // [from..to)
        internal int To { get; set; }
        public bool Disposed { get; private set; }
#if DEBUG
        internal object Owner { get; set; }
        public override string ToString() => $"LEASE [{From}-{To}) {_sprite} for {Owner}";
#else
        public override string ToString() => $"LEASE [{From}-{To}) {_sprite}";
#endif

        public void Dispose()
        {
            if (Disposed) return;
            _sprite.Shrink(this);
            Disposed = true;
        }

        public Span<SpriteInstanceData> Access()
        {
            if (Disposed)
                throw new InvalidOperationException("SpriteLease used after return");
            return _sprite.GetSpan(this);
        }

        // Should only be created by MultiSprite
        internal SpriteLease(MultiSprite sprite, int @from, int to)
        {
            _sprite = sprite;
            From = @from;
            To = to;
        }

        public int CompareTo(SpriteLease other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var fromComparison = From.CompareTo(other.From);
            if (fromComparison != 0) return fromComparison;
            return To.CompareTo(other.To);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(obj, null)) return false;
            return obj is SpriteLease lease && Key == lease.Key && To == lease.To;
        }

        public override int GetHashCode() => HashCode.Combine(Key, From, To);
        public static bool operator ==(SpriteLease left, SpriteLease right) => left?.Equals(right) ?? ReferenceEquals(right, null);
        public static bool operator !=(SpriteLease left, SpriteLease right) => !(left == right);
        public static bool operator <(SpriteLease left, SpriteLease right) => ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
        public static bool operator <=(SpriteLease left, SpriteLease right) => ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
        public static bool operator >(SpriteLease left, SpriteLease right) => !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
        public static bool operator >=(SpriteLease left, SpriteLease right) => ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
    }
}
