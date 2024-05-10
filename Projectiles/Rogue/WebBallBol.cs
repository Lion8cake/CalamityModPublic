﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class WebBallBol : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/WebBall";

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
            Projectile.DamageType = RogueDamageClass.Instance;
            Projectile.aiStyle = ProjAIStyleID.GroundProjectile;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(12))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Web, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.Calamity().stealthStrike)
            {
                target.AddBuff(BuffID.Webbed, 60);
            }
            else
            {
                target.AddBuff(BuffID.Webbed, 30);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Projectile.Calamity().stealthStrike)
            {
                target.AddBuff(BuffID.Webbed, 60);
            }
            else
            {
                target.AddBuff(BuffID.Webbed, 30);
            }
        }
    }
}

