//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SHVDN
{
    public class KeyManager
    {
        public class KeyBinding
        {
            public Keys Combined { get; }
            public Action Action { get; }

            public KeyBinding(Keys combined, Action action)
            {
                Combined = combined;
                Action = action;
            }
        }

        private readonly Dictionary<Keys, List<KeyBinding>> _bindings = new();

        private const Keys KeyCodeMask = (Keys)0xFF;
        private const Keys ModMask = Keys.Control | Keys.Shift | Keys.Alt;

        public void Register(Keys combined, Action action)
        {
            var key = combined & KeyCodeMask;

            if (!_bindings.TryGetValue(key, out var list))
            {
                list = new List<KeyBinding>();
                _bindings[key] = list;
            }

            list.Add(new KeyBinding(combined, action));
        }

        public bool TryHandle(Keys input)
        {
            var key = input & KeyCodeMask;
            var mods = input & ModMask;

            if (!_bindings.TryGetValue(key, out var list))
            {
                return false;
            }

            KeyBinding best = null;
            int bestScore = -1;

            foreach (var binding in list)
            {
                var requiredMods = binding.Combined & ModMask;

                if ((mods & requiredMods) != requiredMods)
                {
                    continue;
                }

                int score = CountBits((uint)requiredMods);

                if (score > bestScore)
                {
                    bestScore = score;
                    best = binding;
                }
            }

            if (best == null)
            {
                return false;
            }

            best.Action();
            return true;
        }

        private static int CountBits(uint value)
        {
            int count = 0;
            while (value != 0)
            {
                value &= value - 1; // clear lowest set bit
                count++;
            }

            return count;
        }
    }
}
