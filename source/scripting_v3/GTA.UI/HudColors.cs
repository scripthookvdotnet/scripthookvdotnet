using System.Drawing;
using GTA.Native;

namespace GTA.UI
{
    /// <summary>
    /// Provides methods to replace HUD colors at runtime.
    /// </summary>
    public static class HudColors
    {
        /// <summary>
        /// Gets the current value of a <see cref="HudColor"/>.
        /// </summary>
        public static Color Get(HudColor hudColor)
        {
            int r, g, b, a;

            unsafe
            {
                Function.Call(Hash.GET_HUD_COLOUR, (int)hudColor, &r, &g, &b, &a);
            }

            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Replaces a <see cref="HudColor"/> with another existing <see cref="HudColor"/>.
        /// </summary>
        /// <param name="destination">The HUD color slot to replace.</param>
        /// <param name="source">The source HUD color to copy from.</param>
        public static void Set(HudColor destination, HudColor source)
        {
            Function.Call(Hash.REPLACE_HUD_COLOUR, (int)destination, (int)source);
        }

        /// <summary>
        /// Replaces a <see cref="HudColor"/> with a custom color.
        /// </summary>
        /// <param name="destination">The <see cref="HudColor"/> slot to replace.</param>
        /// <param name="color">The new color.</param>
        public static void Set(HudColor destination, Color color)
        {
            Function.Call(Hash.REPLACE_HUD_COLOUR_WITH_RGBA, (int)destination, color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Replaces a <see cref="HudColor"/> with a custom color specified by RGBA components.
        /// </summary>
        /// <param name="destination">The <see cref="HudColor"/> slot to replace.</param>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <param name="a">Alpha component (0 = transparent, 255 = fully opaque).</param>
        public static void Set(HudColor destination, byte r, byte g, byte b, byte a)
        {
            Function.Call(Hash.REPLACE_HUD_COLOUR_WITH_RGBA, (int)destination, (int)r, (int)g, (int)b, (int)a);
        }
    }
}
