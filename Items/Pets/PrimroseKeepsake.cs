﻿using System.Collections.Generic;
using CalamityMod.Buffs.Pets;
using CalamityMod.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Pets
{
    public class PrimroseKeepsake : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Pets";
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.damage = 0;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<PrimroseKeepsakeDisplay>();
            Item.buffType = ModContent.BuffType<FurtasticDuoBuff>();
            Item.UseSound = SoundID.Item44;

            Item.value = Item.sellPrice(platinum: 1);
            Item.rare = ItemRarityID.Purple;
            Item.Calamity().devItem = true;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 15, true);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            List<int> pets = new List<int> { ModContent.ProjectileType<Bear>(), ModContent.ProjectileType<KendraPet>() };
            foreach (int petProjID in pets)
                Projectile.NewProjectile(source, position, velocity, petProjID, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BearsEye>().
                AddIngredient<RomajedaOrchid>().
                AddIngredient(ItemID.LovePotion).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }
    }
}
