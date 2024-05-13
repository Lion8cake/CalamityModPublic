﻿using CalamityMod.Dusts;
using CalamityMod.NPCs;
using CalamityMod.NPCs.CeaselessVoid;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Boss
{
    public class DarkEnergyBall2 : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Boss";
        public override string Texture => "CalamityMod/NPCs/CeaselessVoid/DarkEnergy";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = DarkEnergy.FrameCount;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.Calamity().DealsDefenseDamage = true;
            Projectile.width = Projectile.height = DarkEnergy.HitboxSize;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.Opacity = 0f;

            if (CalamityWorld.LegendaryMode && CalamityWorld.revenge)
                Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (CalamityGlobalNPC.voidBoss < 0 || !Main.npc[CalamityGlobalNPC.voidBoss].active)
            {
                Projectile.active = false;
                Projectile.netUpdate = true;
                return;
            }

            if (Vector2.Distance(Projectile.Center, Main.npc[CalamityGlobalNPC.voidBoss].Center) < 80f)
                Projectile.Kill();

            if (Projectile.velocity.Length() < 10f)
                Projectile.velocity *= 1.05f;

            if (Projectile.timeLeft < 30)
                Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 30f, 0f, 1f);
            else
                Projectile.Opacity = MathHelper.Clamp(1f - ((Projectile.timeLeft - 570) / 30f), 0f, 1f);

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 5)
                Projectile.frame = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int height = texture.Height / Main.projFrames[Projectile.type];
            int drawStart = height * Projectile.frame;
            Vector2 origin = Projectile.Size / 2;
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>($"{Texture}Glow").Value, Projectile.Center - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, drawStart, texture.Width, height)), Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => CalamityUtils.CircularHitboxCollision(Projectile.Center, 35f, targetHitbox);

        public override bool CanHitPlayer(Player target) => Projectile.Opacity == 1f;

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage <= 0)
                return;

            if (Projectile.Opacity == 1f)
            {
                int debufftype = Main.zenithWorld ? BuffID.Obstructed : BuffID.VortexDebuff;
                int duration = Main.zenithWorld ? 30 : 60;
                if (info.Damage > 0)
                    target.AddBuff(debufftype, duration, true);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                int cosmiliteDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, (int)CalamityDusts.PurpleCosmilite, 0f, 0f, 100, default, 1.2f);
                Main.dust[cosmiliteDust].velocity *= 3f;
                Main.dust[cosmiliteDust].noGravity = true;
                if (Main.rand.NextBool())
                {
                    Main.dust[cosmiliteDust].scale = 0.5f;
                    Main.dust[cosmiliteDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 5; j++)
            {
                int cosmiliteDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, (int)CalamityDusts.PurpleCosmilite, 0f, 0f, 100, default, 1.7f);
                Main.dust[cosmiliteDust2].noGravity = true;
                Main.dust[cosmiliteDust2].velocity *= 5f;
                cosmiliteDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, (int)CalamityDusts.PurpleCosmilite, 0f, 0f, 100, default, 1f);
                Main.dust[cosmiliteDust2].noGravity = true;
                Main.dust[cosmiliteDust2].velocity *= 2f;
            }
        }
    }
}
