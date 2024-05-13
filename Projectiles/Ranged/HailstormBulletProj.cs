﻿using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Ranged
{
    public class HailstormBulletProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Ranged";
        public float rotIncrease = 0;
        public bool rotDirection = false;
        public Vector2 startVelocity;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            AIType = ProjectileID.Bullet;
            Projectile.timeLeft = 400;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.coldDamage = true;
            Projectile.penetrate = 2;
            Projectile.extraUpdates = 15;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.coldDamage = true;
            Projectile.Calamity().pointBlankShotDuration = CalamityGlobalProjectile.DefaultPointBlankDuration;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                startVelocity = Projectile.velocity;
                rotDirection = Main.rand.NextBool();
                Projectile.velocity *= Main.rand.NextFloat(0.97f, 1.03f);
            }
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 150f && Projectile.localAI[0] % 15 == 0)
            {
                SparkParticle tip = new SparkParticle(Projectile.Center + Projectile.velocity * 18.5f, Projectile.velocity, false, 15, MathHelper.Clamp(-5 + (Projectile.localAI[0] * 0.03f), 0, 0.53f), Color.SkyBlue);
                GeneralParticleHandler.SpawnParticle(tip);
            }

            Dust dust = Dust.NewDustPerfect(Projectile.Center + Projectile.velocity * 5.5f, Main.rand.NextBool(3) ? 135 : 279, (-Projectile.velocity * 0.4f).RotatedBy(rotIncrease));
            dust.scale = 0.75f;
            dust.noGravity = true;

            rotIncrease += 0.1f * (rotDirection ? -1 : 1);

            Projectile.velocity *= 0.982f;

            if (Projectile.localAI[0] > 100f && Projectile.localAI[0] < 300 && Projectile.localAI[0] % 9 == 0)
            {
                LineParticle subTrail = new LineParticle(Projectile.Center, Projectile.velocity * 0.01f, false, 17, 0.6f, Color.SkyBlue);
                GeneralParticleHandler.SpawnParticle(subTrail);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player Owner = Main.player[Projectile.owner];
            target.AddBuff(BuffID.Frostburn, 120);
            if (hit.Crit)
            {
                SoundStyle crit = new("CalamityMod/Sounds/NPCHit/CryogenPhaseTransitionCrack");
                SoundEngine.PlaySound(crit with { Volume = 0.35f, Pitch = 1f }, Projectile.Center);
                target.AddBuff(BuffID.Frostburn2, 300);
                int points = 6;
                float radians = MathHelper.TwoPi / points;
                Vector2 spinningPoint = Vector2.Normalize(new Vector2(-1f, -1f));
                Vector2 addedPlacement = startVelocity;
                float rotRando = Main.rand.NextFloat(0.1f, 2.5f);
                for (int k = 0; k < points; k++)
                {
                    Vector2 velocity = spinningPoint.RotatedBy(radians * k).RotatedBy(-0.45f * rotRando);
                    WaterFlavoredParticle subTrail = new WaterFlavoredParticle(Projectile.Center + velocity * 4.5f + addedPlacement, velocity * 7, false, 6, 0.65f, Color.SkyBlue);
                    GeneralParticleHandler.SpawnParticle(subTrail);
                }
                int onHitDamage = Owner.CalcIntDamage<RangedDamageClass>(0.48f * Projectile.damage);
                Owner.ApplyDamageToNPC(target, onHitDamage, 0f, 0, false);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.numHits > 0)
                Projectile.damage = (int)(Projectile.damage * 0.5f);
            if (Projectile.damage < 1)
                Projectile.damage = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.localAI[0] > 15f)
                CalamityUtils.DrawAfterimagesFromEdge(Projectile, 0, lightColor);
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(200, 200, 200, Projectile.alpha);
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.localAI[0] < 399)
            {
                SoundEngine.PlaySound(SoundID.Item27 with { Volume = 0.3f, Pitch = 0.8f }, Projectile.Center);

                for (int k = 0; k < 11; k++)
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, Main.rand.NextBool(3) ? 135 : 279, new Vector2(4, 4).RotatedByRandom(100) * Main.rand.NextFloat(0.1f, 1.2f));
                    dust.scale = Main.rand.NextFloat(0.9f, 1.5f);
                    dust.noGravity = true;
                }
            }
        }
    }
}
