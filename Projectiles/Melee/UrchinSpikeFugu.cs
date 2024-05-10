﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Melee
{
    public class UrchinSpikeFugu : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 90;
            Projectile.noEnchantments = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.ai[0] == 0f)
            {
                float maxRange = Projectile.ai[1] == 1f ? 256f : 100f; // Increased homing range for Lionfish
                int npcIndex = -1;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                    {
                        float targetDist = (npc.Center - Projectile.Center).Length();
                        if (targetDist < maxRange)
                        {
                            npcIndex = i;
                            maxRange = targetDist;
                        }
                    }
                }
                Projectile.ai[0] = (float)(npcIndex + 1);
                if (Projectile.ai[0] == 0f)
                {
                    Projectile.ai[0] = -15f;
                }
                if (Projectile.ai[0] > 0f)
                {
                    float scaleFactor5 = (float)Main.rand.Next(35, 75) / 30f;
                    Projectile.velocity = (Projectile.velocity * 20f + Vector2.Normalize(Main.npc[(int)Projectile.ai[0] - 1].Center - Projectile.Center + new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101))) * scaleFactor5) / 21f;
                    Projectile.netUpdate = true;
                }
            }
            else if (Projectile.ai[0] > 0f)
            {
                Vector2 value16 = Vector2.Normalize(Main.npc[(int)Projectile.ai[0] - 1].Center - Projectile.Center);
                Projectile.velocity = (Projectile.velocity * 40f + value16 * 12f) / 41f;
            }
            else
            {
                Projectile.ai[0] += 1f;
                Projectile.alpha -= 25;
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
                Projectile.velocity.Y = Projectile.velocity.Y + 0.015f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 120);
        }
    }
}
