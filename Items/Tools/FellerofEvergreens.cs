﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Tools
{
    public class FellerofEvergreens : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Tools";
        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 66;
            Item.damage = 18;
            Item.knockBack = 5f;
            Item.useTime = 17;
            Item.useAnimation = 25;
            Item.axe = 100 / 5;

            Item.DamageType = DamageClass.Melee;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("AnySilverBar", 18).
                AddIngredient(ItemID.Wood, 18).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
