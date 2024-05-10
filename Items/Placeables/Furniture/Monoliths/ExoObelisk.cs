﻿using CalamityMod.Items.Placeables.FurnitureExo;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Tiles.Furniture.Monoliths;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Items.Placeables.Furniture.Monoliths
{
    public class ExoObelisk : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placeables";
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<ExoObeliskTile>();
            Item.rare = ModContent.RarityType<Violet>();
            Item.accessory = true;
            Item.vanity = true;
        }

        public override void UpdateEquip(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                player.Calamity().monolithExoShader = 30;
            }
        }
        public override void UpdateVanity(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                player.Calamity().monolithExoShader = 30;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ExoPlating>(15).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
