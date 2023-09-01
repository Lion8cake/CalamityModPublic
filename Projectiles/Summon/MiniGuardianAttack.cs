﻿using System;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Summon
{
    public class MiniGuardianAttack : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Summon";
        public Player Owner => Main.player[Projectile.owner];

        public bool SpawnedFromPSC => Projectile.ai[0] == 1f;
        public bool ForcedVanity => SpawnedFromPSC && !Owner.Calamity().profanedCrystalBuffs;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.tileCollide = false;
            Projectile.width = 60;
            Projectile.height = 88;
            Projectile.minion = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {  
            // Despawn properly
            if (Owner.Calamity().pSoulGuardians)
                Projectile.timeLeft = 2;
            if (!Owner.Calamity().pSoulArtifact || Owner.dead || !Owner.active)
            {
                Owner.Calamity().pSoulGuardians = false;
                Projectile.active = false;
                return;
            }
            
            // Dynamically update stats here, originalDamage can be found in MiscEffects
            Projectile.damage = (int)Owner.GetTotalDamage<SummonDamageClass>().ApplyTo(Projectile.originalDamage);
            Projectile.localNPCHitCooldown = (SpawnedFromPSC ? 6 : 9);

            // Find minion and charge if possible, make sure vanity minions are not chasing
            NPC potentialTarget = Projectile.Center.MinionHoming(3000f, Owner);
            if (potentialTarget != null && !ForcedVanity)
            {
                Vector2 targetDestination = potentialTarget.Center - Projectile.Center;
                float targetDist = targetDestination.Length();
                // Moves faster if from PSC
                float baseSpeed = (targetDist < 100f ? 28f : 24f) * (SpawnedFromPSC ? 2f : 0.95f);
                float inertia = SpawnedFromPSC ? 20f : 12f;

                targetDist = baseSpeed / targetDist;
                targetDestination *= targetDist;
                Projectile.velocity = (Projectile.velocity * inertia + targetDestination) / (inertia + 1f);
            }
            else // Idle movement
            {
                Vector2 playerDestination = Owner.Center - Projectile.Center;
                playerDestination.X += Main.rand.NextFloat(-10f, 20f) - (60f * Owner.direction);
                playerDestination.Y += Main.rand.NextFloat(-10f, 20f) - 60f;
                float playerDist = playerDestination.Length();
                float acceleration = 0.5f;
                float returnSpeed = 18f;

                // Teleport if too far
                if (playerDist > 2000f)
                {
                    Projectile.position = Owner.position;
                    Projectile.netUpdate = true;
                }
                // Slow down a lot when close
                else if (playerDist < 50f)
                {
                    acceleration = 0.01f;
                    if (Math.Abs(Projectile.velocity.X) > 2f || Math.Abs(Projectile.velocity.Y) > 2f)
                        Projectile.velocity *= 0.9f;
                }
                else
                {
                    if (playerDist < 100f)
                        acceleration = 0.1f;
                    
                    if (playerDist > 300f)
                        acceleration = 1f;

                    playerDist = returnSpeed / playerDist;
                    playerDestination *= playerDist;
                }

                // Turning (wtf is this)
                if (Projectile.velocity.X < playerDestination.X)
                {
                    Projectile.velocity.X += acceleration;
                    if (acceleration > 0.05f && Projectile.velocity.X < 0f)
                        Projectile.velocity.X += acceleration;
                }
                if (Projectile.velocity.X > playerDestination.X)
                {
                    Projectile.velocity.X -= acceleration;
                    if (acceleration > 0.05f && Projectile.velocity.X > 0f)
                        Projectile.velocity.X -= acceleration;
                }
                if (Projectile.velocity.Y < playerDestination.Y)
                {
                    Projectile.velocity.Y += acceleration;
                    if (acceleration > 0.05f && Projectile.velocity.Y < 0f)
                        Projectile.velocity.Y += acceleration * 2f;
                }
                if (Projectile.velocity.Y > playerDestination.Y)
                {
                    Projectile.velocity.Y -= acceleration;
                    if (acceleration > 0.05f && Projectile.velocity.Y > 0f)
                        Projectile.velocity.Y -= acceleration * 2f;
                }
            }

            // Direction and frames
            if (Math.Abs(Projectile.velocity.X) > 0.2f)
                Projectile.direction = Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);

            Projectile.frameCounter++;
            Projectile.frame = Projectile.frameCounter / 6 % Main.projFrames[Projectile.type];
        }

        // Vanity stuff can't damage
        public override bool? CanDamage() => !ForcedVanity;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Owner.Calamity().angelicAlliance)
                target.AddBuff(ModContent.BuffType<BanishingFire>(), 300);
            if (!Owner.Calamity().profanedCrystal)
            {
                if (Projectile.ai[1] == 0f)
                    Owner.Calamity().rollBabSpears(1, target.chaseable);
                Projectile.ai[1] -= 1f;
                if (Projectile.ai[1] < 0f)
                    Projectile.ai[1] = 15;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Owner.Calamity().angelicAlliance)
                target.AddBuff(ModContent.BuffType<BanishingFire>(), 300);
            if (!Owner.Calamity().profanedCrystal)
            {
                if (Projectile.ai[1] == 0f)
                    Owner.Calamity().rollBabSpears(1, true);
                Projectile.ai[1] -= 1f;
                if (Projectile.ai[1] < 0f)
                    Projectile.ai[1] = 15;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Has afterimages if maximum empowerment
            if (!ForcedVanity && SpawnedFromPSC)
            {
                CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
                return false;
            }
            return true;
        }
    }
}
