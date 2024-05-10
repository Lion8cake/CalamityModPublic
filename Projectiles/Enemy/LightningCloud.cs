﻿using CalamityMod.Sounds;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Enemy
{
    public class LightningCloud : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Enemy";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 28;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hostile = true;
            Projectile.timeLeft = 180;
            Projectile.Opacity = 0f;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                int maxFrame = Projectile.timeLeft < 60 ? 6 : 3;
                if (Projectile.frame >= maxFrame)
                    Projectile.frame = 0;

                if (Projectile.frame == 5 && Main.myPlayer == Projectile.owner)
                {
                    SoundEngine.PlaySound(CommonCalamitySounds.LightningSound, Projectile.Center);
                    float ai = Main.rand.Next(100);
                    Vector2 velocity = Vector2.UnitY * 7f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Bottom, velocity, ProjectileID.CultistBossLightningOrbArc, Projectile.damage, 0f, Projectile.owner, MathHelper.PiOver2, ai);
                }
            }

            if (Projectile.timeLeft < 30)
                Projectile.Opacity = MathHelper.Lerp(Projectile.Opacity, 0f, 0.14f);
            else
                Projectile.Opacity = MathHelper.Lerp(Projectile.Opacity, 1f, 0.33f);
        }
    }
}
