﻿using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Placeables.FurnitureSilva
{
    public class SilvaChandelier : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placeables";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurnitureSilva.SilvaChandelier>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SilvaCrystal>(4).
                AddIngredient(ItemID.Torch, 4).
                AddIngredient(ItemID.Chain).
                AddTile<SilvaBasin>().
                Register();
        }
    }
}
