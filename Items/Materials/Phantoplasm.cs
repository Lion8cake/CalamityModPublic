﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityMod.Items.Materials
{
    public class Phantoplasm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantoplasm");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 5));
        }

        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
            item.rare = 10;
            item.value = Item.sellPrice(gold: 1);
            item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(200, 200, 200, 0);
        }
    }
}
