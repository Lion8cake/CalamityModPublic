﻿using System;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class MalachiteBolt : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override string Texture => "CalamityMod/Projectiles/Rogue/MalachiteProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 2;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = RogueDamageClass.Instance;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.alpha -= 3;
            if (Projectile.alpha < 100)
            {
                Projectile.alpha = 100;
            }
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] > 4f)
            {
                for (int i = 0; i < 3; i++)
                {
                    int dusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 0.75f);
                    Main.dust[dusty].noGravity = true;
                    Main.dust[dusty].velocity *= 0f;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override Color? GetAlpha(Color lightColor) => new Color(Main.DiscoR, 203, 103, Projectile.alpha);

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 120);
            if (Projectile.penetrate <= 1)
            {
                Projectile.position = Projectile.Center;
                Projectile.width = Projectile.height = 160;
                Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
                Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 10;
                Projectile.Damage();
                for (int j = 0; j < 70; j++)
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1.2f);
                    Main.dust[dust].velocity *= 3f;
                    if (Main.rand.NextBool())
                    {
                        Main.dust[dust].scale = 0.5f;
                        Main.dust[dust].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int k = 0; k < 40; k++)
                {
                    int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1.7f);
                    Main.dust[dust2].noGravity = true;
                    Main.dust[dust2].velocity *= 5f;
                    dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1f);
                    Main.dust[dust2].velocity *= 2f;
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Projectile.penetrate <= 1)
            {
                Projectile.position = Projectile.Center;
                Projectile.width = Projectile.height = 160;
                Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
                Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 10;
                Projectile.Damage();
                for (int j = 0; j < 70; j++)
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1.2f);
                    Main.dust[dust].velocity *= 3f;
                    if (Main.rand.NextBool())
                    {
                        Main.dust[dust].scale = 0.5f;
                        Main.dust[dust].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int k = 0; k < 40; k++)
                {
                    int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1.7f);
                    Main.dust[dust2].noGravity = true;
                    Main.dust[dust2].velocity *= 5f;
                    dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1f);
                    Main.dust[dust2].velocity *= 2f;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 16;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            for (int j = 0; j < 7; j++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1.2f);
                Main.dust[dust].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[dust].scale = 0.5f;
                    Main.dust[dust].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int k = 0; k < 3; k++)
            {
                int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1.7f);
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].velocity *= 5f;
                dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1f);
                Main.dust[dust2].velocity *= 2f;
            }
        }
    }
}
