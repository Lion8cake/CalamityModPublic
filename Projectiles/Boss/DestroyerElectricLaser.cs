﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Projectiles.Boss
{
    public class DestroyerElectricLaser : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Boss";
        public override void SetDefaults()
        {
            Projectile.ignoreWater = true;
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.scale = 1.1f;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
            Lighting.AddLight(Projectile.Center, 0f, (255 - Projectile.alpha) * 0.35f / 255f, (255 - Projectile.alpha) * 0.35f / 255f);
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 125;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            if (Projectile.localAI[1] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item33, Projectile.Center);
                Projectile.localAI[1] = 1f;
            }
            if (Projectile.velocity.Length() < 12f)
            {
                Projectile.velocity *= 1.0025f;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.alpha < 200)
            {
                return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
            }
            return Color.Transparent;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage <= 0)
                return;

            target.AddBuff(BuffID.Electrified, 60);
        }
    }
}
