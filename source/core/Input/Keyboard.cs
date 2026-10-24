//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Windows.Forms;

namespace SHVDN
{
    internal static class Keyboard
    {
        private static bool[] s_keyboardStatePool = new bool[256];
        private static ModifierKeyState s_modKeyState = ModifierKeyState.None;

        public static void OnMessage(ulong keycode, bool keydown, bool ctrl, bool shift, bool alt)
        {
            UpdatePrimaryKeyboardStateCache((ushort)keycode, keydown);
            UpdateKeyboardModifierStateCache(shift, ctrl, alt);

            // Convert message into a key event
            var keys = (Keys)keycode;
            if (ctrl)
            {
                keys = keys | Keys.Control;
            }

            if (shift)
            {
                keys = keys | Keys.Shift;
            }

            if (alt)
            {
                keys = keys | Keys.Alt;
            }

            Console console = ScriptHookVDotNet.Console;
            if (console != null)
            {
                if (keydown && AreAllKeysPressed(Ini.ReloadKeyBinding))
                {
                    // Force a reload
                    InbuildCommands.Reload();
                    return;
                }

                if (keydown && AreAllKeysPressed(Ini.ConsoleKeyBinding))
                {
                    // Toggle open state
                    console.IsOpen = !console.IsOpen;
                    return;
                }

                // Send key events to console
                console.DoKeyEvent(keys, keydown);

                // Do not send keyboard events to other running scripts when console is open
                if (console.IsOpen)
                {
                    return;
                }
            }

            // Send key events to all scripts
            ScriptHookVDotNet.Domain.DoKeyEvent(keys, keydown);
        }

        public static bool AreAllKeysPressed(Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                switch (key)
                {
                    case Keys.Shift:
                        if ((s_modKeyState & (ModifierKeyState)Keys.Shift) == ModifierKeyState.None)
                        {
                            return false;
                        }
                        break;
                    case Keys.Control:
                        if ((s_modKeyState & (ModifierKeyState)Keys.Control) == ModifierKeyState.None)
                        {
                            return false;
                        }
                        break;
                    case Keys.Alt:
                        if ((s_modKeyState & (ModifierKeyState)Keys.Alt) == ModifierKeyState.None)
                        {
                            return false;
                        }
                        break;

                    default:
                        if (!s_keyboardStatePool[(int)key])
                        {
                            return false;
                        }
                        break;
                }
            }

            return true;
        }

        private static void UpdatePrimaryKeyboardStateCache(ushort keyCode, bool down)
        {
            s_keyboardStatePool[keyCode] = down;
        }
        private static void UpdateKeyboardModifierStateCache(bool shift, bool ctrl, bool alt)
        {
            ModifierKeyState newState = (shift ? ModifierKeyState.Shift : ModifierKeyState.None);
            newState = (ctrl ? newState | ModifierKeyState.Control : newState);
            newState = (alt ? newState | ModifierKeyState.Alt : newState);

            s_modKeyState = newState;
        }
    }
}
