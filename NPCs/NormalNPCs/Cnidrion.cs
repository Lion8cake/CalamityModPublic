﻿using System;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Placeables.Banners;
using CalamityMod.Projectiles.Enemy;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.NPCs.NormalNPCs
{
    public class Cnidrion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 10;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Scale = 0.8f,
            };
            value.Position.X += 48f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 3f;
            NPC.aiStyle = -1;
            NPC.damage = 20;
            NPC.width = 160;
            NPC.height = 80;
            NPC.defense = 6;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.DR_NERD(0.05f);
            NPC.lifeMax = 280;
            NPC.knockBackResist = 0.05f;
            AIType = -1;
            NPC.value = Item.buyPrice(0, 0, 25, 0);
            NPC.HitSound = SoundID.NPCHit12;
            NPC.DeathSound = SoundID.NPCDeath18;
            NPC.rarity = 2;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CnidrionBanner>();
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToWater = true;

            // Scale stats in Expert and Master
            CalamityGlobalNPC.AdjustExpertModeStatScaling(NPC);
            CalamityGlobalNPC.AdjustMasterModeStatScaling(NPC);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                new FlavorTextBestiaryInfoElement("Mods.CalamityMod.Bestiary.Cnidrion")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.PillarZone() ||
                spawnInfo.Player.InAstral() ||
                spawnInfo.Player.ZoneCorrupt ||
                spawnInfo.Player.ZoneCrimson ||
                spawnInfo.Player.ZoneOldOneArmy ||
                spawnInfo.Player.ZoneSkyHeight ||
                spawnInfo.PlayerSafe ||
                !spawnInfo.Player.ZoneDesert ||
                !spawnInfo.Player.ZoneOverworldHeight ||
                Main.eclipse ||
                Main.snowMoon ||
                Main.pumpkinMoon ||
                Main.invasionType != InvasionID.None)
                return 0f;

            // Keep this as a separate if check, because it's a loop and we don't want to be checking it constantly.
            if (NPC.AnyNPCs(NPC.type))
                return 0f;

            return 0.1f;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.1f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            // Setting this in SetDefaults will disable expert mode scaling, so put it here instead
            NPC.damage = 0;

            Player player = Main.player[NPC.target];
            bool expertMode = Main.expertMode;
            bool masterMode = Main.masterMode;
            NPC.spriteDirection = (NPC.direction > 0) ? 1 : -1;
            float movementSpeed = 1f;
            NPC.TargetClosest(true);
            bool stopMoving = false;
            int offsetX = 80;
            int projectileDamage = masterMode ? 8 : expertMode ? 9 : 12;
            if (NPC.life < NPC.lifeMax * 0.33 && CalamityWorld.death)
            {
                movementSpeed = 2f;
            }
            if (NPC.life < NPC.lifeMax * 0.1 && CalamityWorld.death)
            {
                movementSpeed = 4f;
            }
            if (Main.zenithWorld)
            {
                movementSpeed = 8f;
            }
            if (NPC.ai[0] == 0f)
            {
                NPC.ai[1] += 1f;
                if (NPC.life < NPC.lifeMax * 0.33 && CalamityWorld.death)
                {
                    NPC.ai[1] += 1f;
                }
                if (NPC.life < NPC.lifeMax * 0.1 && CalamityWorld.death)
                {
                    NPC.ai[1] += 1f;
                }
                if (NPC.ai[1] >= 300f && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.ai[1] = 0f;
                    if (NPC.life < NPC.lifeMax * 0.25)
                    {
                        NPC.ai[0] = Main.rand.Next(3, 5);
                    }
                    else if (CalamityWorld.death && NPC.life < NPC.lifeMax * 0.6)
                    {
                        NPC.ai[0] = Main.rand.Next(1, 5);
                    }
                    else
                    {
                        NPC.ai[0] = Main.rand.Next(1, 3);
                    }
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 1f)
            {
                stopMoving = true;
                NPC.ai[1] += 1f;
                if (NPC.ai[1] % 30f == 0f)
                {
                    Vector2 npcPosition = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + 20f);
                    npcPosition.X += offsetX * NPC.direction;
                    float targetXDist = Main.player[NPC.target].position.X + Main.player[NPC.target].width * 0.5f - npcPosition.X;
                    float targetYDist = Main.player[NPC.target].position.Y - npcPosition.Y;
                    float targetDistance = (float)Math.Sqrt(targetXDist * targetXDist + targetYDist * targetYDist);
                    float waterSpeed = 6f;
                    targetDistance = waterSpeed / targetDistance;
                    targetXDist *= targetDistance;
                    targetYDist *= targetDistance;
                    targetXDist *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                    targetYDist *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), npcPosition.X, npcPosition.Y, targetXDist, targetYDist, ModContent.ProjectileType<HorsWaterBlast>(), projectileDamage, 0f, Main.myPlayer);
                }
                if (NPC.ai[1] >= 120f)
                {
                    NPC.ai[1] = 0f;
                    NPC.ai[0] = 0f;
                }
            }
            else if (NPC.ai[0] == 2f)
            {
                stopMoving = true;
                NPC.ai[1] += 1f;
                if (NPC.ai[1] > 60f && NPC.ai[1] < 240f && NPC.ai[1] % 16f == 0f)
                {
                    Vector2 npcPosition = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + 20f);
                    npcPosition.X += offsetX * NPC.direction;
                    float targetXDist = Main.player[NPC.target].position.X + Main.player[NPC.target].width * 0.5f - npcPosition.X;
                    float targetYDist = Main.player[NPC.target].position.Y - npcPosition.Y;
                    float targetDistance = (float)Math.Sqrt(targetXDist * targetXDist + targetYDist * targetYDist);
                    float waterSpeed = 8f;
                    targetDistance = waterSpeed / targetDistance;
                    targetXDist *= targetDistance;
                    targetYDist *= targetDistance;
                    targetXDist *= 1f + Main.rand.Next(-15, 16) * 0.01f;
                    targetYDist *= 1f + Main.rand.Next(-15, 16) * 0.01f;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), npcPosition.X, npcPosition.Y, targetXDist, targetYDist, ModContent.ProjectileType<HorsWaterBlast>(), projectileDamage, 0f, Main.myPlayer);
                }
                if (NPC.ai[1] >= 300f)
                {
                    NPC.ai[1] = 0f;
                    NPC.ai[0] = 0f;
                }
            }
            else if (NPC.ai[0] == 3f)
            {
                movementSpeed = 4f;
                NPC.ai[1] += 1f;
                if (NPC.ai[1] % 30f == 0f)
                {
                    Vector2 npcPosition = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + 20f);
                    npcPosition.X += offsetX * NPC.direction;
                    float fastTargetXDist = Main.player[NPC.target].position.X + Main.player[NPC.target].width * 0.5f - npcPosition.X;
                    float fastTargetYDist = Main.player[NPC.target].position.Y - npcPosition.Y;
                    float fastTargetDistance = (float)Math.Sqrt(fastTargetXDist * fastTargetXDist + fastTargetYDist * fastTargetYDist);
                    float fastWaterSpeed = 10f;
                    fastTargetDistance = fastWaterSpeed / fastTargetDistance;
                    fastTargetXDist *= fastTargetDistance;
                    fastTargetYDist *= fastTargetDistance;
                    fastTargetXDist *= 1f + Main.rand.Next(-10, 11) * 0.001f;
                    fastTargetYDist *= 1f + Main.rand.Next(-10, 11) * 0.001f;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), npcPosition.X, npcPosition.Y, fastTargetXDist, fastTargetYDist, ModContent.ProjectileType<HorsWaterBlast>(), projectileDamage, 0f, Main.myPlayer);
                }
                if (NPC.ai[1] >= 120f)
                {
                    NPC.ai[1] = 0f;
                    NPC.ai[0] = 0f;
                }
            }
            else if (NPC.ai[0] == 4f)
            {
                movementSpeed = 4f;
                NPC.ai[1] += 1f;
                if (NPC.ai[1] % 20f == 0f)
                {
                    Vector2 npcPosition = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + 20f);
                    npcPosition.X += offsetX * NPC.direction;
                    float targetXDist = Main.player[NPC.target].position.X + Main.player[NPC.target].width * 0.5f - npcPosition.X;
                    float targetYDist = Main.player[NPC.target].position.Y - npcPosition.Y;
                    float targetDistance = (float)Math.Sqrt(targetXDist * targetXDist + targetYDist * targetYDist);
                    float waterSpeed = 11f;
                    targetDistance = waterSpeed / targetDistance;
                    targetXDist *= targetDistance;
                    targetYDist *= targetDistance;
                    targetXDist *= 1f + Main.rand.Next(-5, 6) * 0.01f;
                    targetYDist *= 1f + Main.rand.Next(-5, 6) * 0.01f;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), npcPosition.X, npcPosition.Y, targetXDist, targetYDist, ModContent.ProjectileType<HorsWaterBlast>(), projectileDamage, 0f, Main.myPlayer);
                }
                if (NPC.ai[1] >= 240f)
                {
                    NPC.ai[1] = 0f;
                    NPC.ai[0] = 0f;
                }
            }

            if (Math.Abs(NPC.Center.X - player.Center.X) < 50f)
                stopMoving = true;

            if (stopMoving)
            {
                NPC.velocity.X *= 0.9f;
                if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                    NPC.velocity.X = 0f;
            }
            else
            {
                float playerLocation = NPC.Center.X - player.Center.X;
                NPC.direction = playerLocation < 0 ? 1 : -1;

                if (NPC.direction > 0)
                    NPC.velocity.X = (NPC.velocity.X * 20f + movementSpeed) / 21f;
                if (NPC.direction < 0)
                    NPC.velocity.X = (NPC.velocity.X * 20f - movementSpeed) / 21f;
            }

            if (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.Center, 1, 1) && !Collision.SolidCollision(NPC.position, NPC.width, NPC.height) && player.position.Y <= NPC.position.Y + NPC.height && !NPC.collideX)
            {
                NPC.noGravity = false;
                NPC.noTileCollide = false;
            }
            else
            {
                NPC.noGravity = true;
                NPC.noTileCollide = true;

                int collisionWidth = 80;
                int collisionHeight = 20;
                Vector2 collisionSize = new Vector2(NPC.Center.X - (collisionWidth / 2), NPC.position.Y + NPC.height - collisionHeight);
                if (Collision.SolidCollision(collisionSize, collisionWidth, collisionHeight))
                {
                    if (NPC.velocity.Y > 0f)
                        NPC.velocity.Y = 0f;

                    if (NPC.velocity.Y > -0.2f)
                        NPC.velocity.Y -= 0.025f;
                    else
                        NPC.velocity.Y -= 0.2f;

                    if (NPC.velocity.Y < -2f)
                        NPC.velocity.Y = -2f;
                }
                else
                {
                    if (NPC.velocity.Y < 0f)
                        NPC.velocity.Y = 0f;

                    if (NPC.velocity.Y < 0.5f)
                        NPC.velocity.Y += 0.025f;
                    else
                        NPC.velocity.Y += 0.25f;
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 40; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 2f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<AmidiasSpark>(), 4);
            npcLoot.Add(ItemID.FossilOre, 1, 4, 5);
        }
    }
}
