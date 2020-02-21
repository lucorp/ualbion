﻿using UAlbion.Formats.Parsers;

namespace UAlbion.Formats.MapEvents
{
    public class ChangePartyGoldEvent : ModifyEvent
    {
        public static ChangePartyGoldEvent Translate(ChangePartyGoldEvent e, ISerializer s)
        {
            e ??= new ChangePartyGoldEvent();
            s.EnumU8(nameof(Operation), () => e.Operation, x => e.Operation = x, x => ((byte)x, x.ToString()));
            s.Dynamic(e, nameof(Unk3));
            s.Dynamic(e, nameof(Unk4));
            s.Dynamic(e, nameof(Unk5));
            s.Dynamic(e, nameof(Amount));
            s.Dynamic(e, nameof(Unk8));
            return e;
        }

        public QuantityChangeOperation Operation { get; private set; }
        public byte Unk3 { get; set; }
        public byte Unk4 { get; set; }
        public byte Unk5 { get; set; }
        public ushort Amount { get; private set; }
        public ushort Unk8 { get; set; }
        public override string ToString() => $"change_party_gold {Operation} {Amount} ({Unk3} {Unk4} {Unk5} {Unk8})";
        public override ModifyType SubType => ModifyType.ChangePartyGold;
    }
}
