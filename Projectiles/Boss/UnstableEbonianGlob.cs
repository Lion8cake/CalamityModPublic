﻿using System;
using CalamityMod.Events;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Boss
{
    public class UnstableEbonianGlob : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Boss";
        public override void SetDefaults()
        {
            Projectile.Calamity().DealsDefenseDamage = true;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.Opacity = 0.8f;
            Projectile.tileCollide = false;
            Projectile.timeLeft = BossRushEvent.BossRushActive ? 780 : CalamityWorld.death ? 600 : CalamityWorld.revenge ? 540 : Main.expertMode ? 480 : 300;

            if (Main.getGoodWorld)
                Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (Projectile.velocity.Length() < 12f && (Main.expertMode || BossRushEvent.BossRushActive))
            {
                float velocityMult = BossRushEvent.BossRushActive ? 1.025f : CalamityWorld.death ? 1.015f : CalamityWorld.revenge ? 1.0125f : Main.expertMode ? 1.01f : 1.005f;
                Projectile.velocity *= velocityMult;
            }

            if (Projectile.timeLeft < 60)
                Projectile.Opacity = MathHelper.Lerp(0f, 0.8f, Projectile.timeLeft / 60f);

            if (Main.rand.NextBool())
            {
                Color dustColor = Color.Lavender;
                dustColor.A = 150;
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.TintableDust, 0f, 0f, Projectile.alpha, dustColor);
                Main.dust[dust].noGravity = true;
            }

            Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.05f;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => CalamityUtils.CircularHitboxCollision(Projectile.Center, 12f, targetHitbox);

        public override bool CanHitPlayer(Player target) => Projectile.timeLeft >= 60;

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage <= 0 || Projectile.timeLeft < 60)
                return;

            target.AddBuff(BuffID.Weak, 180);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor.R = (byte)(255 * Projectile.Opacity);
            lightColor.G = (byte)(255 * Projectile.Opacity);
            lightColor.B = (byte)(255 * Projectile.Opacity);
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
