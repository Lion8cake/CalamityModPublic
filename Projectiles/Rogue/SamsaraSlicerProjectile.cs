﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class SamsaraSlicerProjectile : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/SamsaraSlicer";

        private bool initialized = false;
        private int Lifetime = 180;
        private int ReboundTime = 45;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 46;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.penetrate = -1;
            Projectile.DamageType = RogueDamageClass.Instance;
        }

        public override void AI()
        {
            Initialize();
            BoomerangAI();
            StealthStrikeAI();
            SpawnProjectilesNearEnemies();
            LightingandDust();
        }

        private void Initialize()
        {
            if (initialized)
                return;

            Lifetime = Projectile.Calamity().stealthStrike ? 360 : 180;
            ReboundTime = Projectile.Calamity().stealthStrike ? 90 : 45;
            Projectile.timeLeft = Lifetime;
            initialized = true;
        }

        private void BoomerangAI()
        {
            // Boomerang rotation
            Projectile.rotation += 0.4f * Projectile.direction;

            // Boomerang sound
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }

            // Returns after some number of frames in the air
            if (Projectile.timeLeft < Lifetime - ReboundTime)
                Projectile.ai[0] = 1f;

            if (Projectile.ai[0] == 1f)
            {
                //Ignore tiles and fly faster
                Projectile.tileCollide = false;
                Projectile.extraUpdates = 1;

                Player player = Main.player[Projectile.owner];
                float returnSpeed = 16f;
                float acceleration = 1.4f;
                Vector2 playerVec = player.Center - Projectile.Center;
                float dist = playerVec.Length();

                // Delete the projectile if it's excessively far away.
                if (dist > 3000f)
                    Projectile.Kill();

                playerVec.Normalize();
                playerVec *= returnSpeed;

                // Home back in on the player.
                if (Projectile.velocity.X < playerVec.X)
                {
                    Projectile.velocity.X += acceleration;
                    if (Projectile.velocity.X < 0f && playerVec.X > 0f)
                        Projectile.velocity.X += acceleration;
                }
                else if (Projectile.velocity.X > playerVec.X)
                {
                    Projectile.velocity.X -= acceleration;
                    if (Projectile.velocity.X > 0f && playerVec.X < 0f)
                        Projectile.velocity.X -= acceleration;
                }
                if (Projectile.velocity.Y < playerVec.Y)
                {
                    Projectile.velocity.Y += acceleration;
                    if (Projectile.velocity.Y < 0f && playerVec.Y > 0f)
                        Projectile.velocity.Y += acceleration;
                }
                else if (Projectile.velocity.Y > playerVec.Y)
                {
                    Projectile.velocity.Y -= acceleration;
                    if (Projectile.velocity.Y > 0f && playerVec.Y < 0f)
                        Projectile.velocity.Y -= acceleration;
                }

                // Delete the projectile if it touches its owner.
                if (Main.myPlayer == Projectile.owner)
                    if (Projectile.Hitbox.Intersects(player.Hitbox))
                        Projectile.Kill();
            }
        }

        private void StealthStrikeAI()
        {
            if (!Projectile.Calamity().stealthStrike)
                return;

            if (Projectile.timeLeft % 8f == 0f && Main.myPlayer == Projectile.owner)
            {
                int disk = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SamsaraSlicerSmallDisk>(), Projectile.damage / 4, Projectile.knockBack / 4f, Projectile.owner, Projectile.identity, Main.rand.NextFloat(0.02f, 0.1f));
                Main.projectile[disk].timeLeft *= 2;
                Main.projectile[disk].aiStyle = -1;
            }
        }

        private void SpawnProjectilesNearEnemies()
        {
            if (!Projectile.friendly)
                return;

            const float maxDistance = 300f;
            bool homeIn = false;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy(Projectile, false))
                {
                    float extraDistance = npc.width / 2 + npc.height / 2;

                    bool canHit = true;
                    if (extraDistance < maxDistance)
                        canHit = Collision.CanHit(Projectile.Center, 1, 1, npc.Center, 1, 1);

                    if (Vector2.Distance(npc.Center, Projectile.Center) < maxDistance + extraDistance && canHit)
                    {
                        homeIn = true;
                        break;
                    }
                }
            }

            if (homeIn)
            {
                if (Main.player[Projectile.owner].miscCounter % 50 == 0)
                {
                    int splitProj = ModContent.ProjectileType<SamsaraSlicerSmallDisk>();
                    if (Projectile.owner == Main.myPlayer)
                    {
                        float spread = 60f * 0.0174f;
                        double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                        double deltaAngle = spread / 6f;
                        for (int i = 0; i < 6; i++)
                        {
                            Vector2 velocity = ((MathHelper.TwoPi * i / 6f) - MathHelper.PiOver2).ToRotationVector2() * 6f;
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<SamsaraSlicerSmallDisk>(), Projectile.damage / 2, Projectile.knockBack * 0.5f, Projectile.owner);
                        }
                    }
                }
            }
        }

        private void LightingandDust()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0.75f, 0f);
            if (!Main.rand.NextBool(5))
                return;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.TerraBlade, Projectile.velocity.X, Projectile.velocity.Y);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Return to the player after striking an enemy. Stealth strikes don't return immediately.
            if (!Projectile.Calamity().stealthStrike || Projectile.ai[1] > 3)
                Projectile.ai[0] = 1f;
            Projectile.ai[1]++;
        }

        // Make it bounce on tiles.
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Impacts the terrain even though it bounces off.
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);

            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            Projectile.ai[0] = 1f;
            return false;
        }
    }
}
