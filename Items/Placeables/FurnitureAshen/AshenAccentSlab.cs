﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Walls;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Items.Placeables.FurnitureAshen
{
    public class AshenAccentSlab : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placeables";
        public override string Texture => "CalamityMod/Items/Placeables/FurnitureAshen/AshenSlab";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurnitureAshen.AshenAccentSlab>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(50).
                AddIngredient<SmoothBrimstoneSlag>(50).
                AddIngredient<UnholyCore>().
                AddTile<AshenAltar>().
                AddCondition(Condition.InGraveyard).
                Register();
        }
    }
}
