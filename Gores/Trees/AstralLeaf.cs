﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalamityMod.Gores.Trees
{
    public class AstralLeaf : ModGore
    {
        public override void SetStaticDefaults()
        {
            ChildSafety.SafeGore[Type] = true;
        }

        public override void OnSpawn(Gore gore, IEntitySource source)
        {
            ChildSafety.SafeGore[gore.type] = true;
            gore.velocity = new Vector2(Main.rand.NextFloat() - 0.5f, Main.rand.NextFloat() * MathHelper.TwoPi);
            gore.numFrames = 8;
            gore.frame = (byte)Main.rand.Next(8);
            gore.frameCounter = (byte)Main.rand.Next(8);
            UpdateType = 910;
        }
    }
}
