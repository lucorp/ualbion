﻿using System;
using SerdesNet;
using UAlbion.Api;
using UAlbion.Config;
using UAlbion.Formats.Assets;

namespace UAlbion.Formats.MapEvents
{
    public abstract class QueryEvent : MapEvent, IBranchingEvent
    {
        public override MapEventType EventType => MapEventType.Query;
        public abstract QueryType QueryType { get; }
        public static QueryEvent Serdes(QueryEvent e, AssetMapping mapping, ISerializer s, TextId textSourceId)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            if (s.IsWriting() && e == null) throw new ArgumentNullException(nameof(e));

            var queryType = s.EnumU8(nameof(QueryType), e?.QueryType ?? 0);
            return queryType switch
            {
                QueryType.Switch => QuerySwitchEvent.Serdes((QuerySwitchEvent)e, mapping, s),
                QueryType.Unk1 => QueryUnk1Event.Serdes((QueryUnk1Event)e, s),
                QueryType.Unk4 => QueryUnk4Event.Serdes((QueryUnk4Event)e, s),
                QueryType.HasPartyMember => QueryHasPartyMemberEvent.Serdes((QueryHasPartyMemberEvent)e, mapping, s),
                QueryType.HasItem => QueryHasItemEvent.Serdes((QueryHasItemEvent)e, mapping, s),
                QueryType.UsedItem => QueryUsedItemEvent.Serdes((QueryUsedItemEvent)e, mapping, s),
                QueryType.PreviousActionResult => QueryPreviousActionResultEvent.Serdes((QueryPreviousActionResultEvent)e, s),
                QueryType.ScriptDebugMode => QueryScriptDebugModeEvent.Serdes((QueryScriptDebugModeEvent)e, s),
                QueryType.UnkC => QueryUnkCEvent.Serdes((QueryUnkCEvent)e, s),
                QueryType.NpcActive => QueryNpcActiveEvent.Serdes((QueryNpcActiveEvent)e, s),
                QueryType.Gold => QueryGoldEvent.Serdes((QueryGoldEvent)e, s),
                QueryType.RandomChance => QueryRandomChanceEvent.Serdes((QueryRandomChanceEvent)e, s),
                QueryType.Unk12 => QueryUnk12Event.Serdes((QueryUnk12Event)e, s),
                QueryType.ChosenVerb => QueryChosenVerbEvent.Serdes((QueryChosenVerbEvent)e, s),
                QueryType.Conscious => QueryConsciousEvent.Serdes((QueryConsciousEvent)e, mapping, s),
                QueryType.Leader => QueryLeaderEvent.Serdes((QueryLeaderEvent)e, mapping, s),
                QueryType.Ticker => QueryTickerEvent.Serdes((QueryTickerEvent)e, mapping, s),
                QueryType.Map => QueryMapEvent.Serdes((QueryMapEvent)e, mapping, s),
                QueryType.Unk1E => QueryUnk1EEvent.Serdes((QueryUnk1EEvent)e, s),
                QueryType.PromptPlayer => PromptPlayerEvent.Serdes((PromptPlayerEvent)e, textSourceId, s),
                QueryType.Unk19 => QueryUnk19Event.Serdes((QueryUnk19Event)e, s),
                QueryType.TriggerType => QueryTriggerTypeEvent.Serdes((QueryTriggerTypeEvent)e, s),
                QueryType.Unk21 => QueryUnk21Event.Serdes((QueryUnk21Event)e, s),
                QueryType.EventUsed => QueryEventUsedEvent.Serdes((QueryEventUsedEvent)e, s),
                QueryType.DemoVersion => QueryDemoVersionEvent.Serdes((QueryDemoVersionEvent)e, s),
                QueryType.Unk29 => QueryUnk29Event.Serdes((QueryUnk29Event)e, s),
                QueryType.Unk2A => QueryUnk2AEvent.Serdes((QueryUnk2AEvent)e, s),
                QueryType.PromptPlayerNumeric => PromptPlayerNumericEvent.Serdes((PromptPlayerNumericEvent)e, textSourceId, s),
                _ => throw new FormatException($"Unexpected query type \"queryType\"")
            };
        }
    }
}
