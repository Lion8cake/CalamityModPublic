﻿using CalamityMod.Projectiles.Typeless;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using static CalamityMod.CalamityUtils;
using Terraria.Audio;

namespace CalamityMod.Projectiles.Rogue
{
    public class FishboneBoomerangProjectile : ModProjectile
    {
        internal PrimitiveTrail TrailRenderer;
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/FishboneBoomerang";

        public static int ChargeupTime = 20;
        public static int Lifetime = 240;
        public float OverallProgress => 1 - Projectile.timeLeft / (float)Lifetime;
        public float ThrowProgress => 1 - Projectile.timeLeft / (float)(Lifetime);
        public float ChargeProgress => 1 - (Projectile.timeLeft - Lifetime) / (float)(ChargeupTime);

        public Player Owner => Main.player[Projectile.owner];
        public ref float Returning => ref Projectile.ai[0];
        public ref float Bouncing => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fishbone Boomerang");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = Lifetime + ChargeupTime;
            Projectile.DamageType = RogueDamageClass.Instance;
        }

        public override bool ShouldUpdatePosition()
        {
            return ChargeProgress >= 1;
        }

        public override bool? CanDamage()
        {
            //We don't want the anticipation to deal damage.
            if (ChargeProgress < 1)
                return false;

            return base.CanDamage();
        }

        //Swing animation keys
        public CurveSegment pullback = new CurveSegment(EasingType.PolyOut, 0f, 0f, MathHelper.PiOver4 * -1.2f, 2);
        public CurveSegment throwout = new CurveSegment(EasingType.PolyOut, 0.7f, MathHelper.PiOver4 * -1.2f, MathHelper.PiOver4 * 1.2f + MathHelper.PiOver2, 3);
        public CurveSegment stay = new CurveSegment(EasingType.Linear, 0.95f, MathHelper.PiOver2, 0f);
        internal float ArmAnticipationMovement() => PiecewiseAnimation(ChargeProgress, new CurveSegment[] { pullback, throwout });

        public override void AI()
        {
            //if (Projectile.Calamity().stealthStrike)
            //    CalamityGlobalProjectile.MagnetSphereHitscan(Projectile, 300f, 10f, 20f, 5, ModContent.ProjectileType<Seashell>());

            //Anticipation animation. Make the player look like theyre holding the fish skeleton
            if (ChargeProgress < 1)
            {
                float armRotation = ArmAnticipationMovement() * Owner.direction;

                Owner.heldProj = Projectile.whoAmI;

                Projectile.Center = Owner.Center + Vector2.UnitY.RotatedBy(armRotation) * -40f;
                Projectile.rotation = -MathHelper.PiOver2 + armRotation;

                Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.Pi + armRotation);

                return;
            }

            //Play the throw sound when the throw ACTUALLY BEGINS.
            //Additionally, make the projectile collide and set its speed and velocity
            if (Projectile.timeLeft == Lifetime)
            {
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                Projectile.Center = Owner.Center +Projectile.velocity * 12f;
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 17.5f;
                Projectile.tileCollide = true;
            }

            //Boomerang spinny sounds
            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item7, Projectile.Center);
                Projectile.soundDelay = 8;
            }

            Projectile.rotation += (MathHelper.PiOver4 / 4f + MathHelper.PiOver4 / 2f * Math.Clamp(ThrowProgress * 2f, 0, 1)) * Math.Sign(Projectile.velocity.X);

            if (Projectile.velocity.Length() < 2f && Bouncing == 0f)
            {
                Returning = 1f;
            }

            if (Returning == 0f && Bouncing == 0f && Projectile.velocity.Length() > 2f)
            {
                Projectile.velocity *= 0.97f;
            }

            if (Returning == 1f && Projectile.velocity.Length() < 17f)
            {
                Projectile.velocity *= 1.1f;
            }

            for (int i = 0; i < 2; i++)
            {
                Vector2 dustPos = Projectile.Center + (i * MathHelper.Pi + Projectile.rotation + MathHelper.PiOver2).ToRotationVector2() * 14f;
                Dust dust = Dust.NewDustPerfect(dustPos, 176, (i * MathHelper.Pi + Projectile.rotation * Math.Sign(Projectile.velocity.X)).ToRotationVector2() * 3f);
                dust.noGravity = true;
            }


            if (Returning == 1f)
            {
                Projectile.tileCollide = false;
                //Aim back at the player
                Projectile.velocity = Projectile.velocity.Length() * (Owner.Center - Projectile.Center).SafeNormalize(Vector2.One);

                if ((Projectile.Center - Owner.Center).Length() < 24f)
                {
                    Projectile.Kill();
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            ImpactEffects();

            float streakRotation;
            for (int i = 0; i < 4; i++)
            {
                streakRotation = Main.rand.NextFloat(MathHelper.TwoPi);

                for (int j = 0; j < 4; j++)
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + streakRotation.ToRotationVector2() * (4f + 4f * j), 34, streakRotation.ToRotationVector2() * (4f * j + 4f));
                    dust.noGravity = true;
                }
            }

            if (((Projectile.Calamity().stealthStrike && Projectile.numHits > 2) || !Projectile.Calamity().stealthStrike) && Returning != 1f)
            {
                Projectile.velocity *= 0.3f;
                Returning = 1f;
            }

            else
            {
                //Retarget
                NPC newTarget = null;
                float closestNPCDistance = 10000f;
                float targettingDistance = 400f;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (i == target.whoAmI)
                        continue;

                    if (Main.npc[i].CanBeChasedBy(Projectile))
                    {
                        float potentialNewDistance = (Projectile.Center - Main.npc[i].Center).Length();
                        if (potentialNewDistance < targettingDistance && potentialNewDistance < closestNPCDistance)
                        {
                            closestNPCDistance = potentialNewDistance;
                            newTarget = Main.npc[i];
                        }
                    }
                }

                if (newTarget == null)
                {
                    Projectile.velocity *= 0.3f;
                    Returning = 1f;
                    return;
                }

                Bouncing = 1f;
                Projectile.velocity = 15f * (newTarget.Center - Projectile.Center).SafeNormalize(Vector2.One);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ImpactEffects();
            Projectile.velocity = Projectile.oldVelocity.Length() * 0.3f * (Owner.Center - Projectile.Center).SafeNormalize(Vector2.One);
            Returning = 1f;
            return false;
        }

        public void ImpactEffects()
        {
            SoundEngine.PlaySound(SoundID.DD2_SkeletonHurt with { Volume = SoundID.DD2_SkeletonHurt.Volume * 0.8f }, Projectile.Center);
            int goreNumber = Main.rand.Next(4);

            for (int i = 0; i < goreNumber; i++)
            {
                int goreID = Main.rand.NextBool() ? 266 : Main.rand.NextBool() ? 971 : 972;
                Gore bone = Gore.NewGorePerfect(Projectile.GetSource_FromAI(), Projectile.position, Projectile.velocity * 0.2f + Main.rand.NextVector2Circular(5f, 5f), goreID);
                bone.scale = Main.rand.NextFloat(0.6f, 1f) * (goreID == 972 ? 0.7f : 1f); //Shrink the larger bones
                bone.type = goreID; //Gotta do that or else itll spawn gores from the general pool :(
            }
        }
    }
}
