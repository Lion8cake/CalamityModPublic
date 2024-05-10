﻿using CalamityMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Melee.Yoyos
{
    public class AzathothYoyo : ModProjectile
    {
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<Azathoth>();
        public const int MaxUpdates = 3;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 880f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 60f / MaxUpdates;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = ProjAIStyleID.Yoyo;
            Projectile.width = Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = MaxUpdates;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 2 * MaxUpdates;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(MaxUpdates))
            {
                if (Projectile.owner == Main.myPlayer)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.2f, 0.55f), ModContent.ProjectileType<CosmicOrb>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner, 0f, 0f);
            }
            if ((Projectile.position - Main.player[Projectile.owner].position).Length() > 3200f) //200 blocks
                Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
