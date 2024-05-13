﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class SamsaraSlicerSmallDisk : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        private double rotation = 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.alpha = 75;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.aiStyle = ProjAIStyleID.Boomerang;
            Projectile.timeLeft = 60;
            AIType = ProjectileID.WoodenBoomerang;
            Projectile.DamageType = RogueDamageClass.Instance;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 6;
        }

        public override void AI()
        {
            StealthStrikeAI();
            LightingandDust();
        }

        private void StealthStrikeAI()
        {
            if (Projectile.aiStyle == 3)
                return;

            Projectile.rotation += 0.4f * Projectile.direction;

            Projectile parent = Main.projectile[0];
            bool active = false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.identity == Projectile.ai[0] && p.active && p.type == ModContent.ProjectileType<SamsaraSlicerProjectile>())
                {
                    parent = p;
                    active = true;
                    break;
                }
            }

            if (active)
            {
                Vector2 vector = parent.Center - Projectile.Center;
                Projectile.Center = parent.Center + new Vector2(80, 0).RotatedBy(rotation);
                double rotateAmt = (double)Projectile.ai[1];
                rotation += rotateAmt;
                if (rotation >= 360)
                {
                    rotation = 0;
                }
                Projectile.velocity.X = (vector.X > 0f) ? -0.000001f : 0f;
            }
            else
            {
                Projectile.Kill();
            }

            if (!parent.active)
            {
                Projectile.Kill();
            }
        }

        private void LightingandDust()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0.75f, 0f);
            if (!Main.rand.NextBool(5))
                return;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.TerraBlade, Projectile.velocity.X, Projectile.velocity.Y);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
    }
}
