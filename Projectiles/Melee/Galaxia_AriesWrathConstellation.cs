﻿using System.Collections.Generic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Melee
{
    public class AriesWrathConstellation : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public Player Owner => Main.player[Projectile.owner];
        public ref float Timer => ref Projectile.ai[0];

        public List<Particle> Particles;

        const float ConstellationSwapTime = 15;

        Vector2 PreviousEnd = Vector2.Zero;

        Vector2 AnchorStart => Owner.Center;
        Vector2 AnchorEnd => Owner.Calamity().mouseWorld;
        public Vector2 SizeVector => Utils.SafeNormalize(AnchorEnd - AnchorStart, Vector2.Zero) * MathHelper.Clamp((AnchorEnd - AnchorStart).Length(), 0, FourSeasonsGalaxia.AriesAttunement_Reach);

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = Projectile.height = 8;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.timeLeft = 20;
        }


        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + SizeVector, 30f, ref collisionPoint);
        }

        public void BootlegSpawnParticle(Particle particle)
        {
            if (!Main.dedServ)
            {
                Particles.Add(particle);
                particle.Type = GeneralParticleHandler.particleTypes[particle.GetType()];
            }
        }

        public override void AI()
        {
            if (Particles == null)
                Particles = new List<Particle>();

            Projectile.Center = Owner.Center;

            if (Owner.channel)
                Projectile.timeLeft = 20;

            if (!Owner.active)
            {
                Projectile.Kill();
                return;
            }

            if (Timer % ConstellationSwapTime == 0 && Projectile.timeLeft >= 20)
            {
                Particles.Clear();

                PreviousEnd = Owner.Calamity().mouseWorld - Owner.Center;
                Projectile.ai[0] = 1;
                Vector2 previousStar = Projectile.Center;
                Vector2 offset;
                Particle Line;
                Particle Star = new GenericSparkle(previousStar, Vector2.Zero, Color.White, Color.Plum, Main.rand.NextFloat(1f, 1.5f), 20, 0f, 3f);
                BootlegSpawnParticle(Star);

                for (float i = 0 + Main.rand.NextFloat(0.2f, 0.5f); i < 1; i += Main.rand.NextFloat(0.2f, 0.5f))
                {
                    offset = Main.rand.NextFloat(-50f, 50f) * Utils.SafeNormalize(SizeVector.RotatedBy(MathHelper.PiOver2), Vector2.Zero);
                    Star = new GenericSparkle(Projectile.Center + SizeVector * i + offset, Vector2.Zero, Color.White, Color.Plum, Main.rand.NextFloat(1f, 1.5f), 20, 0f, 3f);
                    BootlegSpawnParticle(Star);

                    Line = new BloomLineVFX(previousStar, Projectile.Center + SizeVector * i + offset - previousStar, 0.8f, Color.MediumVioletRed * 0.75f, 20, true, true);
                    BootlegSpawnParticle(Line);

                    if (Main.rand.NextBool(3))
                    {
                        offset = Main.rand.NextFloat(-50f, 50f) * Utils.SafeNormalize(SizeVector.RotatedBy(MathHelper.PiOver2), Vector2.Zero);
                        Star = new GenericSparkle(Projectile.Center + SizeVector * i + offset, Vector2.Zero, Color.White, Color.Plum, Main.rand.NextFloat(1f, 1.5f), 20, 0f, 3f);
                        BootlegSpawnParticle(Star);

                        Line = new BloomLineVFX(previousStar, Projectile.Center + SizeVector * i + offset - previousStar, 0.8f, Color.MediumVioletRed * 0.75f, 20, true, true);
                        BootlegSpawnParticle(Line);
                    }

                    previousStar = Projectile.Center + SizeVector * i + offset;
                }

                Star = new GenericSparkle(Projectile.Center + SizeVector, Vector2.Zero, Color.White, Color.Plum, Main.rand.NextFloat(1f, 1.5f), 20, 0f, 3f);
                BootlegSpawnParticle(Star);

                Line = new BloomLineVFX(previousStar, Projectile.Center + SizeVector - previousStar, 0.8f, Color.MediumVioletRed * 0.75f, 20, true);
                BootlegSpawnParticle(Line);

            }

            Vector2 moveDirection = Vector2.Zero;
            if (Timer > Projectile.oldPos.Length)
                moveDirection = Projectile.position - Projectile.oldPos[0];

            foreach (Particle particle in Particles)
            {
                if (particle == null)
                    continue;
                particle.Position += particle.Velocity + moveDirection;
                particle.Time++;
                particle.Update();
            }

            Particles.RemoveAll(particle => (particle.Time >= particle.Lifetime && particle.SetLifetime));
            Timer++;

            //Reset the constellation if the mouse goes too far
            if ((Owner.Calamity().mouseWorld - Owner.Center - PreviousEnd).Length() > 120)
                Timer = ConstellationSwapTime;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Particles != null)
            {
                Main.spriteBatch.EnterShaderRegion(BlendState.Additive);

                foreach (Particle particle in Particles)
                {
                    particle.CustomDraw(Main.spriteBatch);
                }

                Main.spriteBatch.ExitShaderRegion();
            }
            return false;
        }
    }
}
