﻿using Microsoft.Xna.Framework;
using Terraria.ModLoader;
namespace CalamityMod.Projectiles.Magic
{
    public class MelterNote1 : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Magic";
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.985f;

            if (Projectile.localAI[0] == 0f)
            {
                Projectile.scale += 0.02f;
                if (Projectile.scale >= 1.25f)
                    Projectile.localAI[0] = 1f;
            }
            else if (Projectile.localAI[0] == 1f)
            {
                Projectile.scale -= 0.02f;
                if (Projectile.scale <= 0.75f)
                    Projectile.localAI[0] = 0f;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255);
        }
    }
}
