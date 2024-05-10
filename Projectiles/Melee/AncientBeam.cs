﻿using System;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityMod.CalamityUtils;


namespace CalamityMod.Projectiles.Melee
{
    public class AncientBeam : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override string Texture => "CalamityMod/Items/Weapons/Melee/FracturedArkGlow";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        const float MaxTime = 40;
        public float Timer => MaxTime - Projectile.timeLeft;

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = (int)MaxTime;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;
            float bladeLength = 34f * Projectile.scale;
            Vector2 start = -Utils.SafeNormalize(Projectile.velocity, Vector2.Zero) * 8.5f;

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + start, Projectile.Center + start + Utils.SafeNormalize(Projectile.velocity, Vector2.Zero) * bladeLength, 24, ref collisionPoint);
        }

        public override void AI()
        {
            if (Projectile.timeLeft < MaxTime - 5)
                Projectile.tileCollide = true;


            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.scale = 2.4f;
            Projectile.Opacity = 0.6f;
            Lighting.AddLight(Projectile.Center, 0.75f, 1f, 0.24f);

            Projectile.velocity *= (1 - (float)Math.Pow(Timer / MaxTime, 3));

            if (Main.rand.NextBool(3))
            {
                int dustTrail = Dust.NewDust(Projectile.Center, 14, 14, DustID.RainbowTorch, Projectile.velocity.X * 0.05f, Projectile.velocity.Y * 0.05f, 150, new Color(Main.DiscoR, 100, 255), 1.2f);
                Main.dust[dustTrail].noGravity = true;
            }

            if (Main.rand.NextBool(3))
            {
                int dustType = Main.rand.Next(3);
                dustType = dustType == 0 ? 15 : dustType == 1 ? 57 : 58;

                Dust.NewDust(Projectile.Center, 14, 14, dustType, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default, 1.3f);
            }

            if (Projectile.velocity.Length() < 1.0f)
                Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > 35)
                return false;

            DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_WitherBeastDeath, Projectile.Center);
            for (int i = 0; i < 10; i++)
            {
                Vector2 particleSpeed = Main.rand.NextVector2CircularEdge(1, 1) * Main.rand.NextFloat(1.2f, 2.3f);
                Particle energyLeak = new SquishyLightParticle(Projectile.Center + Utils.SafeNormalize(Projectile.velocity, Vector2.Zero) * 40f, particleSpeed, Main.rand.NextFloat(0.3f, 0.6f), Color.Cyan, 60, 1, 1.5f, hueShift: 0.02f);
                GeneralParticleHandler.SpawnParticle(energyLeak);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 particleOrigin = target.Hitbox.Size().Length() < 140 ? target.Center : Projectile.Center + Projectile.rotation.ToRotationVector2() * 60f;
            for (int i = 0; i < 10; i++)
            {
                Vector2 particleSpeed = Main.rand.NextVector2CircularEdge(1, 1) * Main.rand.NextFloat(2.6f, 4f);
                Particle energyLeak = new SquishyLightParticle(particleOrigin, particleSpeed, Main.rand.NextFloat(0.3f, 0.6f), Color.Cyan, 60, 1, 1.5f, hueShift: 0.02f);
                GeneralParticleHandler.SpawnParticle(energyLeak);
            }
        }
    }
}
