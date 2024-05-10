﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Placeables.Plates;
using CalamityMod.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    [LegacyName("GodlySoulArtifact")]
    public class AuricSoulArtifact : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<Violet>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.auricSArtifact = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ExodiumCluster>(25).
                AddIngredient<Plagueplate>(25).
                AddIngredient<YharonSoulFragment>(5).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
