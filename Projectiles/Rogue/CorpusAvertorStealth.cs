﻿using System;
using CalamityMod.Balancing;
using CalamityMod.Buffs.StatBuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class CorpusAvertorStealth : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/CorpusAvertor";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = RogueDamageClass.Instance;
        }

        public override void AI()
        {
            Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.02f;

            if (Projectile.ai[0] < 120f)
                Projectile.ai[0] += 1f;

            Projectile.velocity.X *= 1.01f;
            Projectile.velocity.Y *= 1.01f;

            int scale = (int)((Projectile.ai[0] - 60f) * 4.25f);
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, new Color(scale, 0, 0, 50), 2f);
            Main.dust[dust].velocity *= 0f;
            Main.dust[dust].noGravity = true;
        }

        public override Color? GetAlpha(Color lightColor) => new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, Projectile.alpha);

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.lifeMax > 5)
                OnHitEffects(hit.Damage);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            OnHitEffects(info.Damage);
        }

        private void OnHitEffects(int damage)
        {
            Player player = Main.player[Projectile.owner];
            if (Main.rand.NextBool(7))
            {
                int lifeLossAmt = (int)Math.Ceiling(player.statLife * 0.5);
                player.statLife -= lifeLossAmt;
                if (Main.myPlayer == player.whoAmI)
                    player.HealEffect(-lifeLossAmt, true);
                if (player.statLife <= 0)
                    player.KillMe(PlayerDeathReason.ByCustomReason(CalamityUtils.GetText("Status.Death.CorpusAvertor").Format(player.name)), 1000.0, 0, false);
            }
            else if (Main.LocalPlayer.team == player.team && player.team != 0)
            {
                Main.LocalPlayer.AddBuff(ModContent.BuffType<AvertorBonus>(), CalamityUtils.SecondsToFrames(20f), true);
                player.AddBuff(ModContent.BuffType<AvertorBonus>(), CalamityUtils.SecondsToFrames(20f), true);

                int heal = (int)Math.Round(damage * 0.025);
                if (heal > BalancingConstants.LifeStealCap)
                    heal = BalancingConstants.LifeStealCap;

                if (Main.player[Main.myPlayer].lifeSteal <= 0f || heal <= 0)
                    return;

                CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, player, heal, ProjectileID.VampireHeal, BalancingConstants.LifeStealRange);
            }
        }
    }
}
