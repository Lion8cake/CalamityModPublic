﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Projectiles.Environment
{
    public class SmokeTelegraph : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Misc";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        private int dustType = 31;
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.trap = true;
        }

        public override void AI()
        {
            int isActive = Math.Sign(Projectile.velocity.Y);
            int dustCustomData = isActive == -1 ? 0 : 1;
            if (Projectile.ai[0] == 0f)
            {
                if (!Collision.SolidCollision(Projectile.position + new Vector2(0f, isActive == -1 ? (float)(Projectile.height - 48) : 0f), Projectile.width, 48) && !Collision.WetCollision(Projectile.position + new Vector2(0f, isActive == -1 ? (float)(Projectile.height - 20) : 0f), Projectile.width, 20))
                {
                    Projectile.velocity = new Vector2(0f, (float)Math.Sign(Projectile.velocity.Y) * (1f / 1000f));
                    Projectile.ai[0] = 1f;
                    Projectile.ai[1] = 0f;
                    Projectile.timeLeft = 60;
                }

                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] >= 60f)
                    Projectile.Kill();

                for (int i = 0; i < 3; ++i)
                {
                    int smoky = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, new Color(), 1f);
                    Main.dust[smoky].scale = 0.1f + Main.rand.Next(5) * 0.1f;
                    Main.dust[smoky].fadeIn = 1.5f + Main.rand.Next(5) * 0.1f;
                    Main.dust[smoky].noGravity = true;
                    Main.dust[smoky].position = Projectile.Center + new Vector2(0f, (float)(-Projectile.height / 2)).RotatedBy((double)Projectile.rotation, new Vector2()) * 1.1f;
                }
            }

            if (Projectile.ai[0] != 1f)
                return;

            Projectile.velocity = new Vector2(0f, (float)Math.Sign(Projectile.velocity.Y) * (1f / 1000f));

            if (isActive != 0)
            {
                int heightIncrease = 16;
                int maxHeight = 320;
                while (heightIncrease < maxHeight && !Collision.SolidCollision(Projectile.position + new Vector2(0f, isActive == -1 ? (float)(Projectile.height - heightIncrease - 16) : 0f), Projectile.width, heightIncrease + 16))
                    heightIncrease += 16;

                if (isActive == -1)
                {
                    Projectile.position.Y += (float)Projectile.height;
                    Projectile.height = heightIncrease;
                    Projectile.position.Y -= (float)heightIncrease;
                }
                else
                    Projectile.height = heightIncrease;
            }

            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] >= 60f)
                Projectile.Kill();

            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                for (int i = 0; i < 60; ++i)
                {
                    int smoky = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, -2.5f * (float)-isActive, 0, new Color(), 1f);
                    Dust dust = Main.dust[smoky];
                    dust.alpha = 200;
                    dust.velocity *= new Vector2(0.3f, 2f);
                    dust.velocity.Y += (float)(2 * isActive);
                    dust.scale += Main.rand.NextFloat();
                    dust.position = new Vector2(Projectile.Center.X, Projectile.Center.Y + (float)Projectile.height * 0.5f * (float)-isActive);
                    dust.customData = (object)dustCustomData;
                    if (isActive == -1 && !Main.rand.NextBool(4))
                    {
                        dust.velocity.Y -= 0.2f;
                    }
                }
                SoundEngine.PlaySound(SoundID.Item34, Projectile.Center);
            }
            if (isActive == 1)
            {
                for (int i = 0; i < 9; ++i)
                {
                    int smoky = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, -2.5f * (float)-isActive, 0, new Color(), 1f);
                    Dust dust = Main.dust[smoky];
                    dust.alpha = 200;
                    dust.velocity *= new Vector2(0.3f, 2f);
                    dust.velocity.Y += (float)(2 * isActive);
                    dust.scale += Main.rand.NextFloat();
                    dust.position = new Vector2(Projectile.Center.X, Projectile.Center.Y + (float)Projectile.height * 0.5f * (float)-isActive);
                    dust.customData = (object)dustCustomData;
                    if (isActive == -1 && !Main.rand.NextBool(4))
                    {
                        Main.dust[smoky].velocity.Y -= 0.2f;
                    }
                }
            }
            int Height = (int)(Projectile.ai[1] / 60f * (float)Projectile.height) * 3;
            if (Height > Projectile.height)
                Height = Projectile.height;
            Vector2 Position = Projectile.position + (isActive == -1 ? new Vector2(0f, (float)(Projectile.height - Height)) : Vector2.Zero);
            Vector2 vector2 = Projectile.position + (isActive == -1 ? new Vector2(0f, (float)Projectile.height) : Vector2.Zero);
            for (int i = 0; i < 6; ++i)
            {
                if (Main.rand.Next(3) < 2)
                {
                    int smoky = Dust.NewDust(Position, Projectile.width, Height, dustType, 0f, 0f, 90, new Color(), 2.5f);
                    Dust dust = Main.dust[smoky];
                    dust.noGravity = true;
                    dust.fadeIn = 1f;
                    if (dust.velocity.Y > 0f)
                    {
                        dust.velocity.Y *= -1f;
                    }
                    if (Main.rand.Next(6) < 3)
                    {
                        dust.position.Y = MathHelper.Lerp(dust.position.Y, vector2.Y, 0.5f);
                        dust.velocity *= 5f;
                        dust.velocity.Y -= 3f;
                        dust.position.X = Projectile.Center.X;
                        dust.noGravity = false;
                        dust.noLight = true;
                        dust.fadeIn = 0.4f;
                        dust.scale *= 0.3f;
                    }
                    else
                        Main.dust[smoky].velocity = Projectile.DirectionFrom(Main.dust[smoky].position) * Main.dust[smoky].velocity.Length() * 0.25f;
                    Main.dust[smoky].velocity.Y *= (float)-isActive;
                    Main.dust[smoky].customData = (object)dustCustomData;
                }
            }
            for (int i = 0; i < 6; ++i)
            {
                if (Main.rand.NextFloat() >= 0.5f)
                {
                    int smoky = Dust.NewDust(Position, Projectile.width, Height, dustType, 0f, -2.5f * (float)-isActive, 0, new Color(), 1f);
                    Dust dust = Main.dust[smoky];
                    dust.alpha = 200;
                    dust.velocity *= new Vector2(0.6f, 1.5f);
                    dust.scale += Main.rand.NextFloat();
                    if (isActive == -1 && !Main.rand.NextBool(4))
                    {
                        dust.velocity.Y -= 0.2f;
                    }
                    dust.customData = (object)dustCustomData;
                }
            }
        }

        public override bool? CanDamage() => false;
    }
}
