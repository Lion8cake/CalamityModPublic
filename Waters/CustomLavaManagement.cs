using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Waters
{
    public static class CustomLavaManagement
    {
        internal static List<CustomLavaStyle> CustomLavaStyles;
        internal static Texture2D LavaBlockTexture;
        internal static Texture2D LavaTexture;

        // The IL Edits are loaded separately.
        internal static void Load()
        {
            CustomLavaStyles = new List<CustomLavaStyle>();

            foreach (Type type in typeof(CalamityMod).Assembly.GetTypes())
            {
                // Ignore abstract types; they cannot have instances.
                // Also ignore types which do not derive from CustomLavaStyle.
                if (!type.IsSubclassOf(typeof(CustomLavaStyle)) || type.IsAbstract)
                    continue;

                CustomLavaStyles.Add(Activator.CreateInstance(type) as CustomLavaStyle);
                CustomLavaStyles.Last().Load();
            }

            if (Main.netMode != NetmodeID.Server)
            {
                LavaBlockTexture = ModContent.GetTexture("Terraria/Liquid_1");
                LavaTexture = ModContent.GetTexture("Terraria/Misc/water_1");
            }
        }

        internal static void Unload()
        {
            foreach (CustomLavaStyle lavaStyle in CustomLavaStyles)
                lavaStyle?.Unload();

            CustomLavaStyles = null;
            LavaBlockTexture = null;
            LavaTexture = null;
        }

        internal static int SelectLavafallStyle(int initialLavafallStyle)
        {
            // Lava waterfall.
            if (initialLavafallStyle != 1)
                return initialLavafallStyle;

            foreach (CustomLavaStyle lavaStyle in CustomLavaStyles)
            {
                int waterfallStyle = lavaStyle.ChooseWaterfallStyle();
                if (lavaStyle.ChooseLavaStyle() && waterfallStyle >= 0)
                    return waterfallStyle;
            }

            return initialLavafallStyle;
        }

        internal static Color SelectLavafallColor(int initialLavafallStyle, Color initialLavafallColor)
        {
            // Lava waterfall.
            if (initialLavafallStyle != 1)
                return initialLavafallColor;

            foreach (CustomLavaStyle lavaStyle in CustomLavaStyles)
            {
                if (lavaStyle.ChooseLavaStyle())
                {
                    lavaStyle.SelectLightColor(ref initialLavafallColor);
                    return initialLavafallColor;
                }
            }

            return initialLavafallColor;
        }
    }
}
