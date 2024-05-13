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
    public class ArkoftheCosmosConstellation : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public Player Owner => Main.player[Projectile.owner];
        public float Timer => Projectile.ai[0] - Projectile.timeLeft;
        const float ConstellationSwapTime = 15;

        public List<Particle> Particles;

        Vector2 AnchorStart => Owner.Center;
        Vector2 AnchorEnd => Owner.Calamity().mouseWorld;
        public Vector2 SizeVector => Utils.SafeNormalize(AnchorEnd - AnchorStart, Vector2.Zero) * MathHelper.Clamp((AnchorEnd - AnchorStart).Length(), 0, ArkoftheCosmos.MaxThrowReach);

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
            Projectile.localNPCHitCooldown = (int)ConstellationSwapTime;
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

            if (!Owner.channel && Projectile.timeLeft > 20)
                Projectile.timeLeft = 20;

            if (!Owner.active)
            {
                Projectile.Kill();
                return;
            }

            if (Timer % ConstellationSwapTime == 0 && Projectile.timeLeft >= 20)
            {
                Particles.Clear();

                float constellationColorHue = Main.rand.NextFloat();
                Color constellationColor = Main.hslToRgb(constellationColorHue, 1, 0.8f);
                Vector2 previousStar = AnchorStart;
                Vector2 offset;
                Particle Line;
                Particle Star = new GenericSparkle(previousStar, Vector2.Zero, Color.White, Color.Plum, Main.rand.NextFloat(1f, 1.5f), 20, 0f, 3f);
                BootlegSpawnParticle(Star);

                for (float i = 0 + Main.rand.NextFloat(0.2f, 0.5f); i < 1; i += Main.rand.NextFloat(0.2f, 0.5f))
                {
                    constellationColorHue = (constellationColorHue + 0.16f) % 1;
                    constellationColor = Main.hslToRgb(constellationColorHue, 1, 0.8f);

                    offset = Main.rand.NextFloat(-50f, 50f) * Utils.SafeNormalize(SizeVector.RotatedBy(MathHelper.PiOver2), Vector2.Zero);
                    Star = new GenericSparkle(AnchorStart + SizeVector * i + offset, Vector2.Zero, Color.White, constellationColor, Main.rand.NextFloat(1f, 1.5f), 20, 0f, 3f);
                    BootlegSpawnParticle(Star);

                    Line = new BloomLineVFX(previousStar, AnchorStart + SizeVector * i + offset - previousStar, 0.8f, constellationColor * 0.75f, 20, true, true);
                    BootlegSpawnParticle(Line);

                    if (Main.rand.NextBool(3))
                    {
                        constellationColorHue = (constellationColorHue + 0.16f) % 1;
                        constellationColor = Main.hslToRgb(constellationColorHue, 1, 0.8f);

                        offset = Main.rand.NextFloat(-50f, 50f) * Utils.SafeNormalize(SizeVector.RotatedBy(MathHelper.PiOver2), Vector2.Zero);
                        Star = new GenericSparkle(AnchorStart + SizeVector * i + offset, Vector2.Zero, Color.White, constellationColor, Main.rand.NextFloat(1f, 1.5f), 20, 0f, 3f);
                        BootlegSpawnParticle(Star);

                        Line = new BloomLineVFX(previousStar, AnchorStart + SizeVector * i + offset - previousStar, 0.8f, constellationColor, 20, true, true);
                        BootlegSpawnParticle(Line);
                    }

                    previousStar = AnchorStart + SizeVector * i + offset;
                }

                constellationColorHue = (constellationColorHue + 0.16f) % 1;
                constellationColor = Main.hslToRgb(constellationColorHue, 1, 0.8f);

                Star = new GenericSparkle(AnchorStart + SizeVector, Vector2.Zero, Color.White, constellationColor, Main.rand.NextFloat(1f, 1.5f), 20, 0f, 3f);
                BootlegSpawnParticle(Star);

                Line = new BloomLineVFX(previousStar, AnchorStart + SizeVector - previousStar, 0.8f, constellationColor * 0.75f, 20, true);
                BootlegSpawnParticle(Line);
            }

            //Run the particles manually to be sure it doesnt get fucked over by the particle cap
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

                particle.Color = Main.hslToRgb(Main.rgbToHsl(particle.Color).X + 0.02f, Main.rgbToHsl(particle.Color).Y, Main.rgbToHsl(particle.Color).Z);
            }

            Particles.RemoveAll(particle => (particle.Time >= particle.Lifetime && particle.SetLifetime));
        }


        public override bool PreDraw(ref Color lightColor)
        {
            if (Particles != null)
            {
                Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
                foreach (Particle particle in Particles)
                    particle.CustomDraw(Main.spriteBatch);

                Main.spriteBatch.ExitShaderRegion();
            }
            return false;
        }
    }
}
