﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor.FathomSwarmer
{
    [AutoloadEquip(EquipType.Legs)]
    public class FathomSwarmerBoots : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.Hardmode";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 13;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<SummonDamageClass>() += 0.08f;
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.moveSpeed += 0.4f;
            }
            player.ignoreWater = true;
            if (player.wingTime <= 0) //ignore flippers while the player can fly
                player.accFlipper = true;
        }




        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SeaRemains>(9).
                AddIngredient<PlantyMush>(8).
                AddIngredient<DepthCells>(4).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
