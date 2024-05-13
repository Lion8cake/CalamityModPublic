﻿using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Magic
{
    public class ArcticBearPawProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Magic";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            AIType = ProjectileID.Bullet;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.coldDamage = true;
            Projectile.penetrate = 5;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.coldDamage = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            //make pretty dust
            int index2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemSapphire);
            Main.dust[index2].noGravity = true;

            if (Projectile.velocity.X > -0.05f && Projectile.velocity.X < 0.05f &
                Projectile.velocity.Y > -0.05f && Projectile.velocity.Y < 0.05f)
            {
                Projectile.Kill();
            }
            else
            {
                Projectile.velocity *= 0.968f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 180);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 30);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(200, 200, 200, Projectile.alpha);
        }
    }
}
