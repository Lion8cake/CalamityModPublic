﻿using System;
using CalamityMod.Buffs.StatDebuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class ScourgeVenomCloud : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 10;
        }

        public override void SetDefaults()
        {
            Projectile.width = 45;
            Projectile.height = 45;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 3600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 45;
            Projectile.DamageType = RogueDamageClass.Instance;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.ai[0] < 219f) //255 - frameCounter tick * number of disippation frames
            {
                if (Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            else if (Projectile.owner == Main.myPlayer && Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.Kill();
            }
            Projectile.velocity *= 0.98f;
            if (Projectile.alpha > 110)
            {
                Projectile.alpha -= 30;
                if (Projectile.alpha < 110)
                {
                    Projectile.alpha = 110;
                }
            }
            if (Math.Abs(Projectile.velocity.X) > 0.1f)
            {
                Projectile.spriteDirection = -Projectile.direction;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), Projectile.Calamity().stealthStrike ? 120 : 60);
            if (Projectile.ai[1] == 1f && Projectile.owner == Main.myPlayer) //stealth strike attack
            {
                target.AddBuff(BuffID.Venom, 120);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), Projectile.Calamity().stealthStrike ? 120 : 60);
            if (Projectile.ai[1] == 1f && Projectile.owner == Main.myPlayer) //stealth strike attack
            {
                target.AddBuff(BuffID.Venom, 120);
            }
        }
    }
}
