﻿using System;
using System.Collections.ObjectModel;
using UAlbion.Core.Visual;

namespace UAlbion.Game.Debugging
{
    public class SpriteInstanceDataDebugBehaviour : IDebugBehaviour
    {
        public ReadOnlyCollection<Type> HandledTypes { get; } = new ReadOnlyCollection<Type>(new[] { typeof(SpriteInstanceData) });
        public object Handle(DebugInspectorAction action, ReflectedObject reflected)
        {
            if (reflected?.Parent == null) return null;

            if (!(reflected.Parent.Target is SpriteInstanceData[] array))
                return null;

            switch (action)
            {
                case DebugInspectorAction.Hover: array[reflected.CollectionIndex].Flags |= SpriteFlags.RedTint; break;
                case DebugInspectorAction.Blur: array[reflected.CollectionIndex].Flags &= ~SpriteFlags.RedTint; break;
            }

            return null;
        }
    }
}
