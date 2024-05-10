﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalamityMod.Walls.DraedonStructures
{
    public class LaboratoryPanelWall : ModWall
    {

        public override void SetStaticDefaults()
        {
            DustType = 1;
            Main.wallHouse[Type] = true;

            AddMapEntry(new Color(63, 57, 56));
        }

        public override bool CanExplode(int i, int j) => false;

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}
