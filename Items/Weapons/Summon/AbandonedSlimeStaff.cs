﻿using System;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Summon
{
    public class AbandonedSlimeStaff : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Summon";
        int slimeSlots;
        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 62;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item44;

            Item.DamageType = DamageClass.Summon;
            Item.mana = 40;
            Item.damage = 56;
            Item.knockBack = 3f;
            Item.useTime = Item.useAnimation = 20;
            Item.shoot = ModContent.ProjectileType<AstrageldonSummon>();
            Item.shootSpeed = 10f;

            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.Calamity().donorItem = true;
        }

        public override void HoldItem(Player player)
        {
            player.jumpSpeedBoost += 0.5f;

            double minionCount = 0;
            for (int j = 0; j < Main.projectile.Length; j++)
            {
                Projectile projectile = Main.projectile[j];
                if (projectile.active && projectile.owner == player.whoAmI && projectile.minion && projectile.type != ModContent.ProjectileType<AstrageldonSummon>())
                {
                    minionCount += projectile.minionSlots;
                }
            }
            slimeSlots = (int)(player.maxMinions - minionCount);
        }

        public override bool CanUseItem(Player player)
        {
            return slimeSlots >= 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityUtils.KillShootProjectiles(true, type, player);
            float damageMult = ((float)Math.Log(slimeSlots, 8f)) + 1f;
            position = Main.MouseWorld;
            velocity.X = 0;
            velocity.Y = 0;
            int slime = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, (int)(damage * damageMult), knockback, player.whoAmI);
            Main.projectile[slime].originalDamage = (int)(Item.damage * damageMult);
            Main.projectile[slime].minionSlots = slimeSlots;
            return false;
        }
    }
}
