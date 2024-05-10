﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Placeables.Furniture.CraftingStations
{
    public class CosmicAnvilItem : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placeables";
        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<CosmicAnvil>();

            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.value = Item.sellPrice(platinum: 2, gold: 50);
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.CraftingObjects;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("HardmodeAnvil").
                AddIngredient<CosmiliteBar>(10).
                AddIngredient(ItemID.LunarBar, 10).
                AddIngredient<GalacticaSingularity>(12).
                AddIngredient<ExodiumCluster>(20).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
