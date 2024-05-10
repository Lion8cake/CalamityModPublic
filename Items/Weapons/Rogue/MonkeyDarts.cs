﻿using System;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class MonkeyDarts : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 27;
            Item.height = 27;
            Item.damage = 150;
            Item.knockBack = 4;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.value = Item.buyPrice(0, 0, 4, 0);
            Item.rare = ItemRarityID.Lime;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<MonkeyDart>();
            Item.autoReuse = true;
            Item.DamageType = RogueDamageClass.Instance;
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 18;

        public override float StealthDamageMultiplier => 0.8f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //Checks if stealth is avalaible to shoot a spread of 3 darts
            if (player.Calamity().StealthStrikeAvailable())
            {
                float spread = 7;
                for (int i = 0; i < 3; i++)
                {
                    Vector2 perturbedspeed = new Vector2(velocity.X * 1.25f, velocity.Y * 1.25f).RotatedBy(MathHelper.ToRadians(spread));
                    int p = Projectile.NewProjectile(source, position, perturbedspeed, ModContent.ProjectileType<MonkeyDart>(), damage, knockback, player.whoAmI, 1);
                    if (p.WithinBounds(Main.maxProjectiles))
                        Main.projectile[p].Calamity().stealthStrike = true;
                    spread -= 7;
                }
                return false;
            }

            else
            {
                return true;
            }
        }

    }
}
