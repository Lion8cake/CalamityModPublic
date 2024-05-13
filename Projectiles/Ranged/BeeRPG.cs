﻿using System;
using System.Collections.Generic;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Ranged
{
    public class BeeRPG : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Ranged";
        private ref float RocketType => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 95;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            if (Math.Abs(Projectile.velocity.X) >= 8f || Math.Abs(Projectile.velocity.Y) >= 8f)
            {
                for (int i = 0; i < 2; i++)
                {
                    float halfX = 0f;
                    float halfY = 0f;
                    if (i == 1)
                    {
                        halfX = Projectile.velocity.X * 0.5f;
                        halfY = Projectile.velocity.Y * 0.5f;
                    }
                    int dusty = Dust.NewDust(new Vector2(Projectile.position.X + 3f + halfX, Projectile.position.Y + 3f + halfY) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, DustID.Torch, 0f, 0f, 100, default, 1f);
                    Main.dust[dusty].scale *= 2f + (float)Main.rand.Next(10) * 0.1f;
                    Main.dust[dusty].velocity *= 0.2f;
                    Main.dust[dusty].noGravity = true;
                    dusty = Dust.NewDust(new Vector2(Projectile.position.X + 3f + halfX, Projectile.position.Y + 3f + halfY) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, DustID.Smoke, 0f, 0f, 100, default, 0.5f);
                    Main.dust[dusty].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dusty].velocity *= 0.05f;
                }
            }
            if (Math.Abs(Projectile.velocity.X) < 15f && Math.Abs(Projectile.velocity.Y) < 15f)
            {
                Projectile.velocity *= 2f;
            }
            else if (Main.rand.NextBool())
            {
                int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
                Main.dust[dust2].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dust2].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].position = Projectile.Center + new Vector2(0f, (float)(-(float)Projectile.height / 2)).RotatedBy((double)Projectile.rotation, default) * 1.1f;
                Main.rand.Next(2);
                dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1f);
                Main.dust[dust2].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].position = Projectile.Center + new Vector2(0f, (float)(-(float)Projectile.height / 2 - 6)).RotatedBy((double)Projectile.rotation, default) * 1.1f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);

            if (RocketType == ItemID.DryRocket || RocketType == ItemID.WetRocket || RocketType == ItemID.LavaRocket || RocketType == ItemID.HoneyRocket)
            {
                if (Projectile.wet)
                    Projectile.timeLeft = 1;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            var info = new CalamityUtils.RocketBehaviorInfo((int)RocketType);
            int blastRadius = Projectile.RocketBehavior(info);
            Projectile.ExpandHitboxBy(32);
            Projectile.Damage();

            if (Projectile.owner == Main.myPlayer)
            {
                for (int j = 0; j < 12; j++)
                {
                    if (j % 2 != 1 || Main.rand.NextBool(3))
                    {
                        Vector2 projPos = Projectile.position;
                        Vector2 projVel = Projectile.oldVelocity;
                        projVel.Normalize();
                        projVel *= 8f;
                        float beeVelX = (float)Main.rand.Next(-35, 36) * 0.01f;
                        float beeVelY = (float)Main.rand.Next(-35, 36) * 0.01f;
                        projPos -= projVel * (float)j;
                        beeVelX += Projectile.oldVelocity.X / 6f;
                        beeVelY += Projectile.oldVelocity.Y / 6f;
                        int bee = Projectile.NewProjectile(Projectile.GetSource_FromThis(), projPos.X, projPos.Y, beeVelX, beeVelY, Main.player[Projectile.owner].beeType(), Main.player[Projectile.owner].beeDamage(Projectile.damage / 4), Main.player[Projectile.owner].beeKB(0f), Main.myPlayer);
                        if (bee.WithinBounds(Main.maxProjectiles))
                        {
                            Main.projectile[bee].penetrate = 2;
                            Main.projectile[bee].DamageType = DamageClass.Ranged;
                        }
                    }
                }
            }

            if (Main.dedServ)
                return;

            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            for (int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
                Main.dust[dust].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[dust].scale = 0.5f;
                    Main.dust[dust].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 10; j++)
            {
                int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3f);
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].velocity *= 5f;
                dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                Main.dust[dust2].velocity *= 2f;
            }

            if (Main.netMode != NetmodeID.Server)
            {
                Vector2 goreSource = Projectile.Center;
                int goreAmt = 3;
                Vector2 source = new Vector2(goreSource.X - 24f, goreSource.Y - 24f);
                for (int goreIndex = 0; goreIndex < goreAmt; goreIndex++)
                {
                    float velocityMult = 0.33f;
                    if (goreIndex < (goreAmt / 3))
                    {
                        velocityMult = 0.66f;
                    }
                    if (goreIndex >= (2 * goreAmt / 3))
                    {
                        velocityMult = 1f;
                    }
                    Mod mod = ModContent.GetInstance<CalamityMod>();
                    int type = Main.rand.Next(61, 64);
                    int smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    Gore gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X += 1f;
                    gore.velocity.Y += 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X -= 1f;
                    gore.velocity.Y += 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X += 1f;
                    gore.velocity.Y -= 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X -= 1f;
                    gore.velocity.Y -= 1f;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 180);
        }
    }
}
