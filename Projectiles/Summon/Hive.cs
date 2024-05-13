﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Projectiles.Summon
{
    public class Hive : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Summon";
        public override string Texture => "CalamityMod/NPCs/Astral/HiveEnemy";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 60;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.sentry = true;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 5)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 5)
            {
                Projectile.frame = 0;
            }
            Projectile.velocity.Y += 0.5f;

            if (Projectile.velocity.Y > 10f)
            {
                Projectile.velocity.Y = 10f;
            }

            int target = 0;
            float attackDist = 800f;
            Vector2 projPos = Projectile.position;
            bool canAttack = false;
            if (Main.player[Projectile.owner].HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[Main.player[Projectile.owner].MinionAttackTargetNPC];
                if (npc.CanBeChasedBy(Projectile, false))
                {
                    float targetDist = Vector2.Distance(npc.Center, Projectile.Center);
                    if (!canAttack && targetDist < attackDist)
                    {
                        projPos = npc.Center;
                        canAttack = true;
                        target = npc.whoAmI;
                    }
                }
            }
            if (!canAttack)
            {
                for (int j = 0; j < Main.maxNPCs; j++)
                {
                    NPC nPC2 = Main.npc[j];
                    if (nPC2.CanBeChasedBy(Projectile, false))
                    {
                        float targetDist = Vector2.Distance(nPC2.Center, Projectile.Center);
                        if (!canAttack && targetDist < attackDist)
                        {
                            attackDist = targetDist;
                            projPos = nPC2.Center;
                            canAttack = true;
                            target = j;
                        }
                    }
                }
            }
            if (Projectile.owner == Main.myPlayer && canAttack)
            {
                if (Projectile.ai[0] != 0f)
                {
                    Projectile.ai[0] -= 1f;
                    return;
                }
                Projectile.ai[1] += 1f;
                if ((Projectile.ai[1] % 30f) == 0f)
                {
                    float velocityX = Main.rand.NextFloat(-0.4f, 0.4f);
                    float velocityY = Main.rand.NextFloat(-0.3f, -0.5f);
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, velocityX, velocityY, ModContent.ProjectileType<Hiveling>(), Projectile.damage, Projectile.knockBack, Projectile.owner, (float)target, 0f);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = Projectile.originalDamage;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }

        public override bool? CanDamage() => false;
    }
}
