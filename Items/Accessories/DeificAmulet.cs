﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    public class DeificAmulet : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";

        public static readonly int MaxBonusIFrames = 30;

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Cyan;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            player.longInvince = true;
            modPlayer.dAmulet = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.StarVeil).
                AddIngredient<AstralBar>(10).
                AddIngredient<SeaPrism>(15).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
