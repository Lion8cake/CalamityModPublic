using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;

namespace CalamityMod.Systems
{
    public abstract class ModLavaStyle //Do not use yet
    {
        public virtual void DrawColor(int x, int y, ref VertexColors liquidColor)
        {
        }

        public virtual void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
        }
    }

    internal static class LavaStylesLoader //not fully sketched out, here as a sub for implementing ModWaterStyle.DrawColor
    {
        internal static void ModifyLightSetup(int i, int j, int type, ref float r, ref float g, ref float b)
        {
        }

        internal static void DrawColorSetup(int x, int y, int type, ref VertexColors liquidColor)
        {
        }
    }
}
