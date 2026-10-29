//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Collections.Generic;
using System.Text;

namespace SHVDN
{
    public static class TextFormatting
    {
        public static string EscapeTokens(string str)
        {
            StringBuilder sb = new();

            int start = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '~')
                {
                    sb.Append(str.Substring(start, i - start));

                    sb.Append('\\');
                    sb.Append(str[i]);

                    start = i + 1;
                }
            }

            sb.Append(str.Substring(start));

            return sb.ToString();
        }

        public static unsafe float GetTextLength(string text)
        {
            NativeFunc.Invoke(0x66E0276CC5F6B9DA /* SET_TEXT_FONT */, 0);
            NativeFunc.Invoke(0x07C837F9A01C34C9 /* SET_TEXT_SCALE */, 0.35f, 0.35f);
            NativeFunc.Invoke(0x54CE8AC98E120CAB /* BEGIN_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT */, NativeMemory.CellEmailBcon);
            NativeFunc.PushLongString(text, 98); // 99 byte string chunks don't process properly in END_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT
            return *(float*)NativeFunc.Invoke(0x85F061DA64ED2F67 /* END_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT */, true);
        }

        public static float GetMarginLength()
        {
            float len1 = GetTextLength("A");
            float len2 = GetTextLength("AA");
            return len1 - (len2 - len1); // [Margin][A] - [A] = [Margin]
        }

        public static string LongestCommonPrefix(List<string> strs, string input)
        {
            if (strs == null || strs.Count == 0)
            {
                return input;
            }

            string prefix = strs[0];
            for (int i = 1; i < strs.Count; i++)
            {
                int j = 0;
                while (j < prefix.Length && j < strs[i].Length && prefix[j] == strs[i][j])
                {
                    j++;
                }

                prefix = prefix.Substring(0, j);
                if (prefix.Length == input.Length)
                {
                    break;
                }
            }

            return prefix;
        }
    }
}
