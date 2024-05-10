﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Magic
{
    public class RedirectingLostSoul : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Magic";
        public ref float BurstIntensity => ref Projectile.ai[0];
        public ref float Time => ref Projectile.ai[1];
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
        }

        public override void AI() => RedirectingVengefulSoul.DoSoulAI(Projectile, ref Time, 1f);

        public override Color? GetAlpha(Color lightColor)
        {
            Color baseColor = Color.Lerp(Color.DarkRed, Color.DarkViolet, Projectile.identity % 5f / 5f * 0.5f);
            Color color = Color.Lerp(baseColor, Color.White, BurstIntensity * 0.5f + (float)Math.Cos(Main.GlobalTimeWrappedHourly * 2.7f) * 0.04f);
            color.A = 0;
            return color * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects direction = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);

            for (int j = 0; j < Projectile.oldPos.Length / 2; j++)
            {
                float fade = (float)Math.Pow(1f - Utils.GetLerpValue(0f, Projectile.oldPos.Length / 2, j, true), 2D);
                Color drawColor = Projectile.GetAlpha(lightColor) * fade;
                Vector2 drawPosition = Projectile.oldPos[j] + Projectile.Size * 0.5f - Main.screenPosition;
                float rotation = Projectile.oldRot[j];

                Main.EntitySpriteDraw(texture, drawPosition, frame, drawColor, rotation, frame.Size() * 0.5f, Projectile.scale, direction, 0);
            }

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            // Play a wraith death sound at max intensity and a dungeon spirit hit sound otherwise.
            SoundEngine.PlaySound(BurstIntensity >= 1f ? SoundID.NPCDeath52 : SoundID.NPCHit35, Projectile.Center);

            // Make a ghost sound and explode into ectoplasmic energy.
            if (Main.dedServ)
                return;

            for (int i = 0; i < 45; i++)
            {
                Dust ectoplasm = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(50f, 50f) * BurstIntensity, 264);
                ectoplasm.velocity = Main.rand.NextVector2Circular(2f, 2f);
                ectoplasm.color = Projectile.GetAlpha(Color.White);
                ectoplasm.scale = MathHelper.Lerp(1f, 1.6f, BurstIntensity);
                ectoplasm.noGravity = true;
                ectoplasm.noLight = true;
            }
        }
    }
}
