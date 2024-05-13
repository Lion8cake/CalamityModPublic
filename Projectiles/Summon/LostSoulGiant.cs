﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Summon
{
    public class LostSoulGiant : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Summon";
        public ref float Time => ref Projectile.ai[0];
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.scale = 0.8f;
            Projectile.width = Projectile.height = 52;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
        }

        public override void AI() => DoSoulAI(Projectile, ref Time);

        public static void DoSoulAI(Projectile projectile, ref float time)
        {
            if (time >= 15f)
            {
                NPC potentialTarget = projectile.Center.MinionHoming(1800f, Main.player[projectile.owner], false);
                if (potentialTarget != null)
                    HomeInOnTarget(projectile, potentialTarget);
            }

            projectile.Opacity = Utils.GetLerpValue(0f, 15f, time, true);
            projectile.frameCounter++;
            if (projectile.frameCounter % 5f == 4f)
                projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];

            projectile.rotation = projectile.velocity.ToRotation();
            projectile.spriteDirection = (projectile.velocity.X > 0f).ToDirectionInt();
            if (projectile.spriteDirection == -1)
                projectile.rotation += MathHelper.Pi;

            time++;
        }

        public static void HomeInOnTarget(Projectile projectile, NPC target)
        {
            float speed = MathHelper.Lerp(21f, 31f, (float)Math.Sin(projectile.identity % 7f / 7f * MathHelper.TwoPi) * 0.5f + 0.5f);
            projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.SafeDirectionTo(target.Center) * speed, 0.075f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            Color baseColor = Color.Lerp(Color.IndianRed, Color.DarkViolet, Projectile.identity % 5f / 5f * 0.5f);
            Color color = Color.Lerp(baseColor * 1.5f, baseColor, (float)Math.Cos(Main.GlobalTimeWrappedHourly * 2.7f) * 0.04f + 0.45f);
            color.A = 0;
            return color * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects direction = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < Projectile.oldPos.Length / 2; j++)
                {
                    float fade = (float)Math.Pow(1f - Utils.GetLerpValue(0f, Projectile.oldPos.Length / 2, j, true), 2D);
                    Color drawColor = Color.Lerp(Projectile.GetAlpha(lightColor), Color.White * Projectile.Opacity, j / Projectile.oldPos.Length) * fade;
                    Vector2 drawPosition = Projectile.oldPos[j] + Projectile.Size * 0.5f + (MathHelper.TwoPi * i / 4f).ToRotationVector2() * 1.5f - Main.screenPosition;
                    float rotation = Projectile.oldRot[j];

                    Main.EntitySpriteDraw(texture, drawPosition, frame, drawColor, rotation, frame.Size() * 0.5f, Projectile.scale, direction, 0);
                }
            }

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            // Play a wraith death sound.
            SoundEngine.PlaySound(SoundID.NPCDeath52, Projectile.Center);

            if (Main.dedServ)
                return;

            for (int i = 0; i < 45; i++)
            {
                Dust ectoplasm = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(60f, 60f), 264);
                ectoplasm.velocity = Main.rand.NextVector2Circular(2f, 2f);
                ectoplasm.color = Projectile.GetAlpha(Color.White);
                ectoplasm.scale = 1.45f;
                ectoplasm.noGravity = true;
                ectoplasm.noLight = true;
            }
        }
    }
}
