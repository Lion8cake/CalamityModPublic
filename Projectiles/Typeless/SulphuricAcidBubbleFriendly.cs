﻿using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Typeless
{
    public class SulphuricAcidBubbleFriendly : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Typeless";
        public override string Texture => "CalamityMod/Projectiles/Enemy/SulphuricAcidBubble";

        private bool fromArmour = false;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.scale = 0.1f;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.timeLeft = 360;
            Projectile.penetrate = 1;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 1f)
            {
                Projectile.ai[0] = 0f;
                Projectile.scale = 1f;
                fromArmour = true;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 6)
            {
                Projectile.frame = 0;
            }
            if (Projectile.localAI[1] < 1f)
            {
                Projectile.localAI[1] += 0.01f;
                if (Projectile.scale < 1f || (fromArmour && Projectile.scale < 1.8f))
                    Projectile.scale += 0.02f;
                Projectile.width = (int)(30f * Projectile.scale);
                Projectile.height = (int)(30f * Projectile.scale);
            }
            else
            {
                Projectile.width = fromArmour ? Projectile.width : 30;
                Projectile.height = fromArmour ? Projectile.height : 30;
                Projectile.tileCollide = true;
            }
            if (Projectile.localAI[0] > 2f)
            {
                Projectile.alpha -= 20;
                if (Projectile.alpha < 100)
                {
                    Projectile.alpha = 100;
                }
            }
            else
            {
                Projectile.localAI[0] += 1f;
            }
            if (Projectile.ai[1] > 30f)
            {
                if (Projectile.velocity.Y > -2f)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y - 0.05f;
                }
            }
            else
            {
                Projectile.ai[1] += 1f;
            }
            if (Projectile.wet)
            {
                if (Projectile.velocity.Y > 0f)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y * 0.98f;
                }
                if (Projectile.velocity.Y > -1f)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y - 0.2f;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int framing = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int y6 = framing * Projectile.frame;
            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, texture2D13.Width, framing)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2((float)texture2D13.Width / 2f, (float)framing / 2f), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.localAI[1] < 1f)
            {
                return;
            }
            target.AddBuff(ModContent.BuffType<Irradiated>(), fromArmour ? 150 : 120);
            Projectile.Kill();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Projectile.localAI[1] < 1f)
            {
                return;
            }
            target.AddBuff(ModContent.BuffType<Irradiated>(), fromArmour ? 150 : 120);
            Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item54, Projectile.position);
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 60;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            int inc;
            for (int i = 0; i < 25; i = inc + 1)
            {
                int toxicDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 0, default, 1f);
                Main.dust[toxicDust].position = (Main.dust[toxicDust].position + Projectile.position) / 2f;
                Main.dust[toxicDust].velocity = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                Main.dust[toxicDust].velocity.Normalize();
                Dust dust = Main.dust[toxicDust];
                dust.velocity *= (float)Main.rand.Next(1, 30) * 0.1f;
                Main.dust[toxicDust].alpha = Projectile.alpha;
                inc = i;
            }
        }
    }
}
