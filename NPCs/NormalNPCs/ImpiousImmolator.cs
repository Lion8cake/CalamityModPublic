﻿using System;
using System.IO;
using CalamityMod.Dusts;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Banners;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Enemy;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalamityMod.NPCs.NormalNPCs
{
    public class ImpiousImmolator : ModNPC
    {
        public bool hasBeenHit = false;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.damage = 50;
            NPC.width = 60;
            NPC.height = 60;
            NPC.defense = 30;
            NPC.lifeMax = 3000;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.value = Item.buyPrice(0, 0, 50, 0);
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.knockBackResist = 0.2f;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<ImpiousImmolatorBanner>();
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToWater = true;

            // Scale stats in Expert and Master
            CalamityGlobalNPC.AdjustExpertModeStatScaling(NPC);
            CalamityGlobalNPC.AdjustMasterModeStatScaling(NPC);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("Mods.CalamityMod.Bestiary.ImpiousImmolator")
            });
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(hasBeenHit);
            writer.Write(NPC.chaseable);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            hasBeenHit = reader.ReadBoolean();
            NPC.chaseable = reader.ReadBoolean();
        }

        public override void AI()
        {
            // Setting this in SetDefaults will disable expert mode scaling, so put it here instead
            NPC.damage = 0;

            NPC.spriteDirection = (NPC.direction > 0) ? 1 : -1;
            NPC.noGravity = true;
            if (NPC.direction == 0)
            {
                NPC.TargetClosest(true);
            }
            if (NPC.justHit)
            {
                hasBeenHit = true;
            }
            NPC.chaseable = hasBeenHit;
            if (!NPC.wet)
            {
                bool canAttack = hasBeenHit;
                NPC.TargetClosest(false);
                if ((Main.player[NPC.target].wet || Main.player[NPC.target].dead) && canAttack)
                {
                    canAttack = false;
                }
                if (!canAttack)
                {
                    if (NPC.collideX)
                    {
                        NPC.velocity.X = NPC.velocity.X * -1f;
                        NPC.direction *= -1;
                        NPC.netUpdate = true;
                    }
                    if (NPC.collideY)
                    {
                        NPC.netUpdate = true;
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y = Math.Abs(NPC.velocity.Y) * -1f;
                            NPC.directionY = -1;
                            NPC.ai[0] = -1f;
                        }
                        else if (NPC.velocity.Y < 0f)
                        {
                            NPC.velocity.Y = Math.Abs(NPC.velocity.Y);
                            NPC.directionY = 1;
                            NPC.ai[0] = 1f;
                        }
                    }
                }
                if (canAttack)
                {
                    NPC.TargetClosest(true);
                    NPC.velocity.X = NPC.velocity.X + (float)NPC.direction * 0.2f;
                    NPC.velocity.Y = NPC.velocity.Y + (float)NPC.directionY * 0.2f;
                    float velocityMax = CalamityWorld.death ? 16f : CalamityWorld.revenge ? 14f : 12f;
                    if (NPC.velocity.X > velocityMax)
                    {
                        NPC.velocity.X = velocityMax;
                    }
                    if (NPC.velocity.X < -velocityMax)
                    {
                        NPC.velocity.X = -velocityMax;
                    }
                    if (NPC.velocity.Y > velocityMax)
                    {
                        NPC.velocity.Y = velocityMax;
                    }
                    if (NPC.velocity.Y < -velocityMax)
                    {
                        NPC.velocity.Y = -velocityMax;
                    }

                    if (NPC.justHit)
                        NPC.localAI[0] = 0f;

                    NPC.localAI[0] += 1f;
                    if (Main.netMode != NetmodeID.MultiplayerClient && NPC.localAI[0] >= (CalamityWorld.death ? 50f : CalamityWorld.revenge ? 70f : 90f))
                    {
                        NPC.localAI[0] = 0f;
                        if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
                        {
                            float speed = 12f;
                            Vector2 beamPosition = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)(NPC.height / 2));
                            float targetXDist = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - beamPosition.X + (float)Main.rand.Next(-20, 21);
                            float targetYDist = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - beamPosition.Y + (float)Main.rand.Next(-20, 21);
                            float targetDistance = (float)Math.Sqrt((double)(targetXDist * targetXDist + targetYDist * targetYDist));
                            targetDistance = speed / targetDistance;
                            targetXDist *= targetDistance;
                            targetYDist *= targetDistance;
                            int damage = Main.masterMode ? 35 : Main.expertMode ? 42 : 55;
                            int beam = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, targetXDist, targetYDist, ModContent.ProjectileType<FlameBurstHostile>(), damage, 0f, Main.myPlayer, 0f, 0f);
                            Main.projectile[beam].tileCollide = true;
                        }
                    }
                }
                else
                {
                    NPC.velocity.X = NPC.velocity.X + (float)NPC.direction * 0.1f;
                    if (NPC.velocity.X < -3f || NPC.velocity.X > 3f)
                    {
                        NPC.velocity.X = NPC.velocity.X * 0.95f;
                    }
                    if (NPC.ai[0] == -1f)
                    {
                        NPC.velocity.Y = NPC.velocity.Y - 0.01f;
                        if ((double)NPC.velocity.Y < -0.3)
                        {
                            NPC.ai[0] = 1f;
                        }
                    }
                    else
                    {
                        NPC.velocity.Y = NPC.velocity.Y + 0.01f;
                        if ((double)NPC.velocity.Y > 0.3)
                        {
                            NPC.ai[0] = -1f;
                        }
                    }
                }
                int tileCheckX = (int)(NPC.position.X + (float)(NPC.width / 2)) / 16;
                int tileCheckY = (int)(NPC.position.Y + (float)(NPC.height / 2)) / 16;
                if (Main.tile[tileCheckX, tileCheckY - 1].LiquidAmount < 128) //problem?
                {
                    if (Main.tile[tileCheckX, tileCheckY + 1].HasTile)
                    {
                        NPC.ai[0] = -1f;
                    }
                    else if (Main.tile[tileCheckX, tileCheckY + 2].HasTile)
                    {
                        NPC.ai[0] = -1f;
                    }
                }
                if ((double)NPC.velocity.Y > 0.4 || (double)NPC.velocity.Y < -0.4)
                {
                    NPC.velocity.Y = NPC.velocity.Y * 0.95f;
                }
            }
            else
            {
                if (NPC.velocity.Y == 0f)
                {
                    NPC.velocity.X = NPC.velocity.X * 0.94f;
                    if ((double)NPC.velocity.X > -0.2 && (double)NPC.velocity.X < 0.2)
                    {
                        NPC.velocity.X = 0f;
                    }
                }
                NPC.velocity.Y = NPC.velocity.Y + 0.3f;
                if (NPC.velocity.Y > 5f)
                {
                    NPC.velocity.Y = 5f;
                }
                NPC.ai[0] = 1f;
            }
            NPC.rotation = NPC.velocity.Y * (float)NPC.direction * 0.1f;
            if ((double)NPC.rotation < -0.1)
            {
                NPC.rotation = -0.1f;
            }
            if ((double)NPC.rotation > 0.1)
            {
                NPC.rotation = 0.1f;
            }
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.minion && !projectile.Calamity().overridesMinionDamagePrevention)
            {
                return hasBeenHit;
            }
            return null;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += hasBeenHit ? 0.25f : 0.125f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || !NPC.downedMoonlord || spawnInfo.Player.Calamity().ZoneCalamity)
            {
                return 0f;
            }
            if (SpawnCondition.Underworld.Chance > 0f)
            {
                return SpawnCondition.Underworld.Chance / 4f;
            }
            return SpawnCondition.OverworldHallow.Chance / 4f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<UnholyEssence>(), 1, 2, 4);
            npcLoot.Add(ModContent.ItemType<SanctifiedSpark>(), 15);
            npcLoot.Add(ModContent.ItemType<BlasphemousDonut>(), 20);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.ProfanedFire, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 25; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.ProfanedFire, hit.HitDirection, -1f, 0, default, 1f);
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ImpiousImmolator").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ImpiousImmolator2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ImpiousImmolator3").Type, 1f);
                }
            }
        }
    }
}
