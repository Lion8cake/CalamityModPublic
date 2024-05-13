﻿using CalamityMod.Items.Armor.Victide;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    public class ShieldoftheOcean : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.rare = ItemRarityID.Green;
            Item.defense = 2;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.statDefense += 5;
            }
            // Accessory update comes before set bonus for some reason, so this has to be done
            if ((player.armor[0].type == ModContent.ItemType<VictideHeadMagic>() || player.armor[0].type == ModContent.ItemType<VictideHeadSummon>() ||
                player.armor[0].type == ModContent.ItemType<VictideHeadMelee>() || player.armor[0].type == ModContent.ItemType<VictideHeadRanged>() ||
                player.armor[0].type == ModContent.ItemType<VictideHeadRogue>()) &&
                player.armor[1].type == ModContent.ItemType<VictideBreastplate>() && player.armor[2].type == ModContent.ItemType<VictideGreaves>())
            {
                player.moveSpeed += 0.1f;
                player.lifeRegen += 2;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SeaRemains>(5).
                AddIngredient(ItemID.Starfish, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
