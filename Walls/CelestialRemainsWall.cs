﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Walls
{
    [LegacyName("AstralFossilWall")]
    public class CelestialRemainsWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            DustType = ModContent.DustType<Dusts.AstralBasic>();
            Main.wallHouse[Type] = true;

            AddMapEntry(new Color(29, 38, 49));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}
