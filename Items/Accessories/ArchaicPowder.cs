﻿using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    public class ArchaicPowder : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.pickSpeed -= 0.15f;
            if (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneUnderworldHeight)
            {
                player.statDefense += 10;
                player.endurance += 0.05f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AncientFossil>().
                AddIngredient<DemonicBoneAsh>().
                AddIngredient<AncientBoneDust>(3).
                AddIngredient(ItemID.Bone, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
