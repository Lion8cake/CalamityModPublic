﻿using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class PartisanExplosion : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.DamageType = RogueDamageClass.Instance;
            Projectile.friendly = true;
            Projectile.timeLeft = 10;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
        }

        public override void AI()
        {
            for (int i = 0; i < 7; i++)
            {
                float speedx = Main.rand.NextFloat(-0.9f, 0.9f);
                float speedy = Main.rand.NextFloat(-0.9f, 0.9f);
                int d = Dust.NewDust(Projectile.position, 33, 33, DustID.CopperCoin, speedx, speedy, 120, default(Color), 2.6f);
                Main.dust[d].position = Projectile.Center;
            }
        }
    }
}
