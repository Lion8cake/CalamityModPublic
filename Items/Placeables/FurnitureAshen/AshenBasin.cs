﻿using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Placeables.FurnitureAshen
{
    public class AshenBasin : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placeables";
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 0;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurnitureAshen.AshenBasin>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SmoothBrimstoneSlag>(10).
                AddIngredient<UnholyCore>(5).
                AddTile<AshenAltar>().
                AddDecraftCondition(Condition.DownedMechBossAny).
                Register();
        }
    }
}
