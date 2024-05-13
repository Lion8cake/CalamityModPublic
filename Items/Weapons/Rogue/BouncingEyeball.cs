﻿using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class BouncingEyeball : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.damage = 16;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3.5f;
            Item.rare = ItemRarityID.Green;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.DamageType = RogueDamageClass.Instance;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<BouncingEyeballProjectile>();
            Item.shootSpeed = 10f;
            Item.autoReuse = true;
        }

        public override float StealthVelocityMultiplier => 2f;

        public override void ModifyStatsExtra(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (Main.bloodMoon)
            {
                knockback *= 3f;
            }
            if (!player.Calamity().StealthStrikeAvailable())
            {
                velocity *= Main.rand.NextFloat(0.85f, 1.3f);
                velocity = velocity.RotatedByRandom(MathHelper.ToRadians(10f)); //random spread
            }
            else
            {
                type = ModContent.ProjectileType<BouncingEyeballProjectileStealthStrike>();
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (p.WithinBounds(Main.maxProjectiles) && player.Calamity().StealthStrikeAvailable())
                Main.projectile[p].Calamity().stealthStrike = true;
            return false;
        }
    }
}
