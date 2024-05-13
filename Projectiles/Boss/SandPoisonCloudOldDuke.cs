﻿using System;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Boss
{
    public class SandPoisonCloudOldDuke : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Boss";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 10;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 45;
            Projectile.height = 45;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 1800;
            Projectile.alpha = 80;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.1f, 0.7f, 0f);

            Projectile.ai[0] += 1f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.ai[0] < 1620f)
            {
                if (Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            if (Projectile.ai[0] > 1620f)
            {
                Projectile.damage = 0;
            }
            else if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.Kill();
            }

            Projectile.velocity *= 0.995f;

            if (Math.Abs(Projectile.velocity.X) > 0f)
            {
                Projectile.spriteDirection = -Projectile.direction;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor.R = (byte)(255 * Projectile.Opacity);
            lightColor.G = (byte)(255 * Projectile.Opacity);
            lightColor.B = (byte)(255 * Projectile.Opacity);
            CalamityUtils.DrawProjectileWithBackglow(Projectile, new Color(66, 236, 113), lightColor, 3.75f);
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => CalamityUtils.CircularHitboxCollision(Projectile.Center, 20f, targetHitbox);

        public override bool CanHitPlayer(Player target) => Projectile.ai[0] < 1620f;

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage <= 0)
                return;

            target.AddBuff(ModContent.BuffType<Irradiated>(), 240, true);
        }
    }
}
