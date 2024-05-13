﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Typeless
{
    public class HydrothermicFlareRogue : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Typeless";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 285 && target.CanBeChasedBy(Projectile);

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.5f, 0.25f, 0f);

            int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Flare, 0f, 0f, 100, default, 2f);
            Main.dust[d].noGravity = true;
            Main.dust[d].velocity *= 0.5f;
            Main.dust[d].velocity += Projectile.velocity * 0.1f;

            if (Projectile.timeLeft < 285)
                CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 250f, 11f, 20f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.OnFire3, 180);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffID.OnFire3, 180);
    }
}
