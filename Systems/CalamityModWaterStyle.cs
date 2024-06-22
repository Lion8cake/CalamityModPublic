using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;

namespace CalamityMod.Systems
{
    public abstract class CalamityModWaterStyle : ModWaterStyle
    {
        public virtual void DrawColor(int x, int y, VertexColors liquidColor)
        {
        }

        public virtual void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
        }
    }

    internal static class CalamityWaterLoader
    {
        internal static readonly IList<ModWaterStyle> Waters = new List<ModWaterStyle>();

        internal static ModWaterStyle GetWater(int id)
        {
            if (id < LoaderManager.Get<WaterStylesLoader>().VanillaCount || id >= (int)typeof(Loader).GetField("TotalCount", BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).GetValue(null))
            {
                return null;
            }
            return Waters[id - LoaderManager.Get<WaterStylesLoader>().VanillaCount];
        }

        internal static void ModifyLightSetup(int i, int j, int type, ref float r, ref float g, ref float b)
        {
            CalamityModWaterStyle styles = (CalamityModWaterStyle)LoaderManager.Get<WaterStylesLoader>().Get(type);//GetWater(type);
            if (styles != null)
            {
                styles?.ModifyLight(i, j, ref r, ref g, ref b);
            }
        }
    }
}
