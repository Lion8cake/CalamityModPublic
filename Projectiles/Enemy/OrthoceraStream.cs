﻿using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalamityMod.Projectiles.Enemy
{
    public class OrthoceraStream : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Enemy";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.ai[1]++;
            if (Projectile.ai[1] >= 4f)
            {
                Projectile.tileCollide = true;
            }
            Projectile.scale -= 0.002f;
            if (Projectile.scale <= 0f)
            {
                Projectile.Kill();
            }
            if (Projectile.ai[0] <= 3f)
            {
                Projectile.ai[0] += 1f;
                return;
            }
            Projectile.velocity.Y = Projectile.velocity.Y + 0.075f;
            for (int i = 0; i < 3; i++)
            {
                Vector2 positionDelta = Projectile.velocity / 3f * i;
                int spawnDelta = 14;
                int dustIdx = Dust.NewDust(new Vector2(Projectile.position.X + spawnDelta, Projectile.position.Y + spawnDelta), Projectile.width - spawnDelta * 2, Projectile.height - spawnDelta * 2, (int)CalamityDusts.SulphurousSeaAcid, 0f, 0f, 100, default, 1f);
                Dust dust = Main.dust[dustIdx];
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.5f;
                dust.position -= positionDelta;
            }
            if (Main.rand.NextBool(8))
            {
                int spawnDelta = 16;
                int dustIdx = Dust.NewDust(new Vector2(Projectile.position.X + spawnDelta, Projectile.position.Y + spawnDelta), Projectile.width - spawnDelta * 2, Projectile.height - spawnDelta * 2, (int)CalamityDusts.SulphurousSeaAcid, 0f, 0f, 100, default, 0.5f);
                Main.dust[dustIdx].velocity *= 0.25f;
                Main.dust[dustIdx].velocity += Projectile.velocity * 0.5f;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage <= 0)
                return;

            target.AddBuff(ModContent.BuffType<Irradiated>(), 60);
        }
    }
}
