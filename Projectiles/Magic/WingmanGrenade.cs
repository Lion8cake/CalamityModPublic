﻿using CalamityMod.Particles;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Projectiles.Magic
{
    public class WingmanGrenade : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Magic";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public int BounceHits = 0;
        public Color mainColor = Color.White;
        public bool exploding = false;
        public float sizeBonus = 1;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 480;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 4;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            if (mainColor == Color.White)
            {
                Projectile.scale = 0.4f;
                if (Projectile.ai[1] == 0)
                    mainColor = Color.HotPink;

                if (Projectile.ai[1] == 2)
                {
                    mainColor = Color.MediumVioletRed;
                    Projectile.scale = 0.5f;
                    sizeBonus = 1.5f;
                }
            }

            if (Projectile.timeLeft % 2 == 0)
                Projectile.scale = Main.rand.NextFloat(0.35f, 0.5f);

            if (Projectile.timeLeft <= 65)
                exploding = true;

            if (exploding)
            {
                Projectile.velocity = Vector2.Zero;
                if (Projectile.timeLeft > 65)
                    Projectile.timeLeft = 65;
                if (Projectile.timeLeft == 65)
                {
                    Particle blastRing2 = new CustomPulse(Projectile.Center, Vector2.Zero, mainColor, "CalamityMod/Particles/HighResHollowCircleHardEdge", Vector2.One, Main.rand.NextFloat(-10, 10), 0f, 0.12f * sizeBonus, 15);
                    GeneralParticleHandler.SpawnParticle(blastRing2);
                    Particle blastRing = new CustomPulse(Projectile.Center, Vector2.Zero, Color.Lerp(mainColor, Color.White, 0.5f), "CalamityMod/Particles/BloomCircle", Vector2.One, Main.rand.NextFloat(-10, 10), 3f * sizeBonus, 0f, 25);
                    GeneralParticleHandler.SpawnParticle(blastRing);
                    SoundStyle fire = new("CalamityMod/Sounds/Item/ArcNovaDiffuserChargeImpact");
                    SoundEngine.PlaySound(fire with { Volume = 1.25f, Pitch = -0.2f, PitchVariance = 0.15f }, Projectile.Center);
                }
                if (Projectile.timeLeft == 65)
                {
                    Particle blastRing2 = new CustomPulse(Projectile.Center, Vector2.Zero, mainColor, "CalamityMod/Particles/HighResHollowCircleHardEdge", Vector2.One, Main.rand.NextFloat(-10, 10), 0.12f * sizeBonus, 0.135f * sizeBonus, 50);
                    GeneralParticleHandler.SpawnParticle(blastRing2);
                }

                if (Projectile.timeLeft % 4 == 0)
                {
                    Particle blastRing2 = new CustomPulse(Projectile.Center + new Vector2(95, 95).RotatedByRandom(100) * sizeBonus * Main.rand.NextFloat(0.7f, 1.1f), Vector2.Zero, mainColor, "CalamityMod/Particles/HighResHollowCircleHardEdge", Vector2.One, Main.rand.NextFloat(-10, 10), 0f, Main.rand.NextFloat(0.04f, 0.07f) * sizeBonus, 13);
                    GeneralParticleHandler.SpawnParticle(blastRing2);
                }
            }
            Lighting.AddLight(Projectile.Center, mainColor.ToVector3() * 0.7f);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            
            Projectile.velocity *= 0.988f;
            if (Projectile.timeLeft % 2 == 0)
            {
                if (!exploding && Main.rand.NextBool() || exploding)
                {
                    Vector2 dustVel = new Vector2(4, 4).RotatedByRandom(100) * Main.rand.NextFloat(0.1f, 0.8f) * (exploding ? sizeBonus : 1);
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + dustVel * (exploding ? 5 : 1), Main.rand.NextBool(4) ? 264 : 66, dustVel * (exploding ? 5 : 1), 0, default, Main.rand.NextFloat(0.9f, 1.2f) * (exploding ? 1.5f : 1));
                    dust.noGravity = true;
                    dust.color = Main.rand.NextBool() ? Color.Lerp(mainColor, Color.White, 0.5f) : mainColor;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CalamityMod/Particles/Light").Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Color drawColor = Projectile.GetAlpha(lightColor);
            float drawRotation = Projectile.rotation;
            Vector2 rotationPoint = texture.Size() * 0.5f;

            if (!exploding)
            {
                CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], Color.Lerp(mainColor, Color.White, 0.5f) * 0.6f, 1);
                Main.EntitySpriteDraw(texture, drawPosition, null, mainColor with { A = 0 }, drawRotation, rotationPoint, Projectile.scale, SpriteEffects.None);
                Main.EntitySpriteDraw(texture, drawPosition, null, Color.White with { A = 0 }, drawRotation, rotationPoint, Projectile.scale, SpriteEffects.None);

            }
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!exploding)
            {
                exploding = true;
            }
            if (Projectile.numHits > 0)
                Projectile.damage = (int)(Projectile.damage * 0.95f);
            if (Projectile.damage < 1)
                Projectile.damage = 1;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 5; i++)
            {
                //Dust dust = Dust.NewDustPerfect(Projectile.Center, Main.rand.NextBool(4) ? 264 : 66, (Projectile.velocity.SafeNormalize(Vector2.UnitY) * 15f).RotatedByRandom(MathHelper.ToRadians(15f)) * Main.rand.NextFloat(0.1f, 0.8f), 0, default, Main.rand.NextFloat(1.2f, 1.6f));
                //dust.noGravity = true;
                //dust.color = Main.rand.NextBool() ? Color.Lerp(mainColor, Color.White, 0.5f) : mainColor;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.tileCollide = false;
            exploding = true;
            return false;
        }
        public override bool? CanDamage() => null;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => CalamityUtils.CircularHitboxCollision(Projectile.Center, exploding ? 120 * sizeBonus : 20 * sizeBonus, targetHitbox);
    }
}
