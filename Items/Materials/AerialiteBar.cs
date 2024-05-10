﻿using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Materials
{
    public class AerialiteBar : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Materials";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.SortingPriorityMaterials[Type] = 69; // Hellstone
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.createTile = ModContent.TileType<AerialiteBarTile>();
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 30);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AerialiteOre>(4).
                AddTile(TileID.Furnaces).
                Register();
        }
    }
}
