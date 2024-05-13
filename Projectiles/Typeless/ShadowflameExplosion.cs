﻿using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Typeless
{
    public class ShadowflameExplosion : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Typeless";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 75;
            Projectile.height = 75;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.ai[1]++;
            if (Projectile.ai[1] >= 3f)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 Dspeed = new Vector2(2.3f, 2.3f).RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
                    float Dscale = Main.rand.NextFloat(1f, 1.3f);
                    Dust.NewDust(Projectile.Center, 1, 1, DustID.Shadowflame, Dspeed.X, Dspeed.Y, 0, default, Dscale);
                    Dust.NewDust(Projectile.Center, 1, 1, DustID.Shadowflame, -Dspeed.X, -Dspeed.Y, 0, default, Dscale);
                    Projectile.ai[0] += 19f;
                }
                Projectile.ai[1] = 0f;
            }
            if (Projectile.timeLeft < 25)
            {
                Projectile.damage = 0;
            }

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.ShadowFlame, 180);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(ModContent.BuffType<Shadowflame>(), 180);
    }
}
