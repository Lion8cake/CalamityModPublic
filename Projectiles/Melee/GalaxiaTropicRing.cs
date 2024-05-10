﻿using System;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityMod.Projectiles.Melee
{
    public class GalaxiaTropicRing : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public Player Owner => Main.player[Projectile.owner];
        public ref float Mode => ref Projectile.ai[0];
        public ref float Fade => ref Projectile.ai[1];
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Mode == 0f && Main.rand.NextFloat() < FourSeasonsGalaxia.CancerPassiveLifeStealProc)
            {
                Owner.statLife += FourSeasonsGalaxia.CancerPassiveLifeSteal;
                Owner.HealEffect(FourSeasonsGalaxia.CancerPassiveLifeSteal);
            }
            else
            {
                target.AddBuff(BuffType<ArmorCrunch>(), FourSeasonsGalaxia.CapricornPassiveDebuffTime);
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Collision.CheckAABBvAABBCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - Vector2.One * 95 * Projectile.scale, Vector2.One * 190 * Projectile.scale);
        }

        public override void AI()
        {
            Projectile.velocity *= 0.95f;

            if (Mode == 0f)
                Projectile.Center = Vector2.Lerp(Projectile.Center, Owner.Center, 0.4f * MathHelper.Clamp((Projectile.timeLeft - 150) / 150f, 0, 1));
            else
                Projectile.velocity = Utils.SafeNormalize(Projectile.Center - Owner.Center, Vector2.Zero) * MathHelper.Clamp((Projectile.timeLeft - 150) / 150f, 0, 1) * 3f * (5f - MathHelper.Clamp((Projectile.Center - Owner.Center).Length() / 150f, 0, 4f));

            Fade = Projectile.timeLeft > 250 ? (float)Math.Sin((300 - Projectile.timeLeft) / 50f * MathHelper.PiOver2) * 0.6f + 0.4f : Projectile.timeLeft > 50 ? 1f : (float)Math.Sin((Projectile.timeLeft) / 50f * MathHelper.PiOver2);
            Lighting.AddLight(Projectile.Center, 0.75f, 1f, 0.24f);

        }

        public Vector2 starPosition(int MaxStars, int StarIndex, float diameter)
        {
            float starHeight = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3 + StarIndex * MathHelper.TwoPi / (float)MaxStars);
            float starWidth = (float)Math.Cos(Main.GlobalTimeWrappedHourly * 3 + StarIndex * MathHelper.TwoPi / (float)MaxStars);

            return Projectile.Center + Projectile.rotation.ToRotationVector2() * starWidth * (Projectile.scale * diameter * 0.4f) + (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2() * starHeight * (Projectile.scale * diameter * 0.4f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sigil = Request<Texture2D>(Mode == 0 ? "CalamityMod/Projectiles/Melee/GalaxiaCancer" : "CalamityMod/Projectiles/Melee/GalaxiaCapricorn").Value; //OMG Karkat and Gamzee from homestuck i am a big fan
            Texture2D ring = Request<Texture2D>("CalamityMod/Particles/BloomRing").Value;
            Texture2D starTexture = Request<Texture2D>("CalamityMod/Particles/Sparkle").Value;
            Texture2D lineTexture = Request<Texture2D>("CalamityMod/Particles/BloomLine").Value;
            Texture2D bloomTexture = Request<Texture2D>("CalamityMod/Particles/BloomCircle").Value;

            Color ringColor = Mode == 0 ? new Color(255, 0, 0) * Fade : new Color(66, 0, 176) * Fade;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(bloomTexture, Projectile.Center - Main.screenPosition, null, ringColor * 0.5f, 0, bloomTexture.Size() / 2f, 2.5f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(ring, Projectile.Center - Main.screenPosition, null, ringColor * 0.8f, 0f, ring.Size() / 2f, Projectile.scale, 0f, 0);

            Main.EntitySpriteDraw(sigil, Projectile.Center - Main.screenPosition, null, ringColor * MathHelper.Lerp(0.7f, 0f, ((Main.GlobalTimeWrappedHourly * 5f) % 10) / 10f), 0f, sigil.Size() / 2f, Projectile.scale + ((Main.GlobalTimeWrappedHourly * 5f) % 10) / 10f * 2f, 0f, 0);
            Main.EntitySpriteDraw(sigil, Projectile.Center - Main.screenPosition, null, Color.White * Fade, 0f, sigil.Size() / 2f, Projectile.scale, 0f, 0);

            for (int i = 0; i < 5; i++)
            {

                Vector2 starPos = starPosition(5, i, ring.Width);

                Vector2 LineVector = starPosition(5, i + 1, ring.Width) - starPos;
                float rot = LineVector.ToRotation() + MathHelper.PiOver2;
                Vector2 origin = new Vector2(lineTexture.Width / 2f, lineTexture.Height);
                Vector2 scale = new Vector2(0.3f, LineVector.Length() / lineTexture.Height);

                Main.EntitySpriteDraw(lineTexture, starPos - Main.screenPosition, null, Color.White * Fade * 0.7f, rot, origin, scale, SpriteEffects.None, 0);

                Main.EntitySpriteDraw(bloomTexture, starPos - Main.screenPosition, null, ringColor * 0.5f, 0, bloomTexture.Size() / 2f, 0.1f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(starTexture, starPos - Main.screenPosition, null, ringColor * 0.5f, Main.GlobalTimeWrappedHourly * 5 + MathHelper.PiOver4 * i, starTexture.Size() / 2f, 1f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(starTexture, starPos - Main.screenPosition, null, Color.White * Fade, Main.GlobalTimeWrappedHourly * 5, starTexture.Size() / 2f, 1.4f, SpriteEffects.None, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}
