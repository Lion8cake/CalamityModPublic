﻿using System;
using System.Collections.Generic;
using System.IO;
using CalamityMod.Dusts;
using CalamityMod.Events;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Vanity;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Furniture.BossRelics;
using CalamityMod.Items.Placeables.Furniture.DevPaintings;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Items.Potions;
using CalamityMod.Items.TreasureBags;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Boss;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.NPCs.Polterghast
{
    public class Polterghast : ModNPC
    {
        public static int phase1IconIndex;
        public static int phase3IconIndex;

        internal static void LoadHeadIcons()
        {
            string phase1IconPath = "CalamityMod/NPCs/Polterghast/Polterghast_Head_Boss";
            string phase3IconPath = "CalamityMod/NPCs/Polterghast/Necroplasm_Head_Boss";

            CalamityMod.Instance.AddBossHeadTexture(phase1IconPath, -1);
            phase1IconIndex = ModContent.GetModBossHeadSlot(phase1IconPath);

            CalamityMod.Instance.AddBossHeadTexture(phase3IconPath, -1);
            phase3IconIndex = ModContent.GetModBossHeadSlot(phase3IconPath);
        }

        private const int DespawnTimerMax = 900;
        private int despawnTimer = DespawnTimerMax;
        private int soundTimer = 0;
        private bool reachedChargingPoint = false;
        private bool threeAM = false;
        public static readonly SoundStyle HitSound = new("CalamityMod/Sounds/NPCHit/PolterghastHit");
        public static readonly SoundStyle P2Sound = new("CalamityMod/Sounds/Custom/Polterghast/PolterghastP2Transition");
        public static readonly SoundStyle P3Sound = new("CalamityMod/Sounds/Custom/Polterghast/PolterghastP3Transition");
        public static readonly SoundStyle SpawnSound = new("CalamityMod/Sounds/Custom/Polterghast/PolterghastSpawn");
        public static readonly SoundStyle PhantomSound = new("CalamityMod/Sounds/Custom/Polterghast/PolterghastPhantomSpawn");

        public static Asset<Texture2D> Texture_Glow;
        public static Asset<Texture2D> Texture_Glow2;

        public static List<SoundStyle> creepySounds = new List<SoundStyle>
        {
            NPCs.DevourerofGods.DevourerofGodsHead.AttackSound,
            NPCs.Providence.Providence.HolyRaySound,
            NPCs.ExoMechs.Ares.AresBody.EnragedSound,
            NPCs.ExoMechs.Ares.AresBody.LaserStartSound,
            NPCs.ExoMechs.Thanatos.ThanatosHead.VentSound,
            NPCs.SupremeCalamitas.SupremeCalamitas.SepulcherSummonSound,
            NPCs.SupremeCalamitas.SupremeCalamitas.SpawnSound,
            NPCs.Ravager.RavagerBody.LimbLossSound,
            NPCs.HiveMind.HiveMind.RoarSound,
            NPCs.Yharon.Yharon.RoarSound,
            NPCs.DesertScourge.DesertScourgeHead.RoarSound,
            NPCs.OldDuke.OldDuke.RoarSound,
            NPCs.Abyss.ReaperShark.SearchRoarSound,
            NPCs.Abyss.ReaperShark.EnragedRoarSound,
            NPCs.Abyss.LuminousCorvina.ScreamSound,
            NPCs.Abyss.DevilFish.MaskBreakSound,
            NPCs.PrimordialWyrm.PrimordialWyrmHead.ChargeSound,
            NPCs.GreatSandShark.GreatSandShark.RoarSound,
            NPCs.AcidRain.Mauler.RoarSound,
            NPCs.AstrumDeus.AstrumDeusHead.DeathSound,
            NPCs.AstrumAureus.AstrumAureus.HitSound,
            SoundID.ScaryScream,
            SoundID.DD2_KoboldFlyerHurt
        };

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 12;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            if (!Main.dedServ)
            {
                Texture_Glow = ModContent.Request<Texture2D>(Texture + "Glow", AssetRequestMode.AsyncLoad);
                Texture_Glow2 = ModContent.Request<Texture2D>(Texture + "Glow2", AssetRequestMode.AsyncLoad);
            }
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.npcSlots = 50f;
            NPC.GetNPCDamage();
            NPC.width = 90;
            NPC.height = 120;
            NPC.defense = 90;
            NPC.DR_NERD(0.2f);
            NPC.LifeMaxNERB(350000, 420000, 325000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.value = Item.buyPrice(3, 50, 0, 0);
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.HitSound = HitSound;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.Calamity().VulnerableToSickness = false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
                new FlavorTextBestiaryInfoElement("Mods.CalamityMod.Bestiary.Polterghast")
            });
        }

        public override void BossHeadSlot(ref int index)
        {
            bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
            bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;

            bool phase3 = NPC.life / (float)NPC.lifeMax < (death ? 0.6f : revenge ? 0.5f : expertMode ? 0.35f : 0.2f);
            if (phase3)
                index = phase3IconIndex;
            else
                index = phase1IconIndex;
        }

        public override void BossHeadRotation(ref float rotation)
        {
            if (NPC.HasValidTarget && NPC.Calamity().newAI[3] == 0f)
                rotation = (Main.player[NPC.TranslatedTargetIndex].Center - NPC.Center).ToRotation() + MathHelper.PiOver2;
            else
                rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(despawnTimer);
            writer.Write(reachedChargingPoint);
            writer.Write(threeAM);
            CalamityGlobalNPC cgn = NPC.Calamity();
            writer.Write(cgn.newAI[0]);
            writer.Write(cgn.newAI[1]);
            writer.Write(cgn.newAI[2]);
            writer.Write(cgn.newAI[3]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            despawnTimer = reader.ReadInt32();
            reachedChargingPoint = reader.ReadBoolean();
            threeAM = reader.ReadBoolean();
            CalamityGlobalNPC cgn = NPC.Calamity();
            cgn.newAI[0] = reader.ReadSingle();
            cgn.newAI[1] = reader.ReadSingle();
            cgn.newAI[2] = reader.ReadSingle();
            cgn.newAI[3] = reader.ReadSingle();
        }

        public override void AI()
        {
            CalamityGlobalNPC calamityGlobalNPC = NPC.Calamity();

            // Emit light
            Lighting.AddLight((int)((NPC.position.X + (NPC.width / 2)) / 16f), (int)((NPC.position.Y + (NPC.height / 2)) / 16f), 0.1f, 0.5f, 0.5f);

            // whoAmI variable
            CalamityGlobalNPC.ghostBoss = NPC.whoAmI;

            // Detect clone
            bool cloneAlive = false;
            if (CalamityGlobalNPC.ghostBossClone != -1)
                cloneAlive = Main.npc[CalamityGlobalNPC.ghostBossClone].active;

            // Percent life remaining
            float lifeRatio = NPC.life / (float)NPC.lifeMax;

            // Variables
            Vector2 vector = NPC.Center;
            bool bossRush = BossRushEvent.BossRushActive;
            bool speedBoost = false;
            bool despawnBoost = false;
            bool death = CalamityWorld.death || bossRush;
            bool revenge = CalamityWorld.revenge || bossRush;
            bool expertMode = Main.expertMode || bossRush;

            // Phases
            bool phase2 = lifeRatio < (death ? 0.9f : revenge ? 0.8f : expertMode ? 0.65f : 0.5f);
            bool phase3 = lifeRatio < (death ? 0.6f : revenge ? 0.5f : expertMode ? 0.35f : 0.2f);
            bool phase4 = lifeRatio < (death ? 0.45f : revenge ? 0.35f : expertMode ? 0.2f : 0.1f);
            bool phase5 = lifeRatio < (death ? 0.2f : revenge ? 0.15f : expertMode ? 0.1f : 0.05f);

            // Get angry if the clone is dead and in phase 3
            bool getPissed = !cloneAlive && phase3;

            // Velocity and acceleration
            calamityGlobalNPC.newAI[0] += 1f;
            float chargePhaseGateValue = 480f;
            if (Main.getGoodWorld)
                chargePhaseGateValue *= 0.5f;

            bool chargePhase = calamityGlobalNPC.newAI[0] >= chargePhaseGateValue;
            int chargeAmt = getPissed ? 4 : phase3 ? 3 : phase2 ? 2 : 1;
            if (Main.zenithWorld)
                chargeAmt = phase4 ? int.MaxValue : getPissed ? 6 : phase3 ? 4 : phase2 ? 3 : 2;

            float chargeVelocity = getPissed ? 28f : phase3 ? 24f : phase2 ? 22f : 20f;
            float chargeAcceleration = getPissed ? 0.7f : phase3 ? 0.6f : phase2 ? 0.55f : 0.5f;
            float chargeDistance = 480f;
            bool charging = NPC.ai[2] >= chargePhaseGateValue - 180f;
            bool reset = NPC.ai[2] >= chargePhaseGateValue + 120f;

            if ((Main.time >= 27000 && Main.time < 30600 && Main.dayTime == false && Main.zenithWorld) || threeAM)
            {
                threeAM = true;
                chargeVelocity *= 2;
                chargeAcceleration *= 2;
                chargeDistance *= 3;

                if (!phase4)
                    chargeAmt *= 2;
            }

            // Only get a new target while not charging
            if (!chargePhase)
            {
                // Get a target
                if (NPC.target < 0 || NPC.target == Main.maxPlayers || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                    NPC.TargetClosest();

                // Despawn safety, make sure to target another player if the current player target is too far away
                if (Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) > CalamityGlobalNPC.CatchUpDistance200Tiles)
                    NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];
            float velocity = 15f;
            float acceleration = 0.075f;

            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    speedBoost = true;
                    despawnBoost = true;
                    reachedChargingPoint = false;
                    NPC.ai[1] = 0f;
                    calamityGlobalNPC.newAI[0] = 0f;
                    calamityGlobalNPC.newAI[1] = 0f;
                    calamityGlobalNPC.newAI[2] = 0f;
                    calamityGlobalNPC.newAI[3] = 0f;

                    if (cloneAlive)
                    {
                        Main.npc[CalamityGlobalNPC.ghostBossClone].ai[0] = 0f;
                        Main.npc[CalamityGlobalNPC.ghostBossClone].ai[1] = 0f;
                        Main.npc[CalamityGlobalNPC.ghostBossClone].Calamity().newAI[0] = 0f;
                        Main.npc[CalamityGlobalNPC.ghostBossClone].Calamity().newAI[1] = 0f;
                        Main.npc[CalamityGlobalNPC.ghostBossClone].Calamity().newAI[2] = 0f;
                        Main.npc[CalamityGlobalNPC.ghostBossClone].Calamity().newAI[3] = 0f;
                    }
                }
            }

            // Play a random creepy sound every once in a while in the zenith seed
            if (Main.zenithWorld)
            {
                soundTimer++;
                int gate = threeAM ? 60 : phase4 ? 300 : phase3 ? 420 : phase2 ? 540 : 600;
                if (soundTimer % gate == 0)
                {
                    SoundStyle[] creepyArray = creepySounds.ToArray();
                    SoundStyle selectedSound = creepyArray[Main.rand.Next(0, creepyArray.Length - 1)];
                    SoundEngine.PlaySound(selectedSound with { Pitch = selectedSound.Pitch - 0.8f, Volume = selectedSound.Volume - 0.2f }, NPC.Center);
                }
            }

            // Stop rain
            if (CalamityConfig.Instance.BossesStopWeather)
                CalamityMod.StopRain();

            // Set time left
            if (NPC.timeLeft < 1800)
                NPC.timeLeft = 1800;

            // Spawn hooks
            if (NPC.localAI[0] == 0f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.localAI[0] = 1f;
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)vector.X, (int)vector.Y, ModContent.NPCType<PolterghastHook>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)vector.X, (int)vector.Y, ModContent.NPCType<PolterghastHook>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)vector.X, (int)vector.Y, ModContent.NPCType<PolterghastHook>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)vector.X, (int)vector.Y, ModContent.NPCType<PolterghastHook>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);

                if (Main.zenithWorld)
                {
                    for (int I = 0; I < 3; I++)
                    {
                        int spawn = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(vector.X + (Math.Sin(I * 120) * 500)), (int)(vector.Y + (Math.Cos(I * 120) * 500)), ModContent.NPCType<PhantomFuckYou>(), NPC.whoAmI, 0, 0, 0, -1);
                        NPC npc2 = Main.npc[spawn];
                        npc2.ai[0] = I * 120;
                    }
                }
            }

            bool despawn = !player.ZoneDungeon && !bossRush && player.position.Y < Main.worldSurface * 16.0;
            if (despawn)
            {
                despawnTimer--;
                if (despawnTimer <= 0)
                {
                    despawnBoost = true;
                    NPC.ai[1] = 0f;
                    calamityGlobalNPC.newAI[0] = 0f;
                    calamityGlobalNPC.newAI[1] = 0f;
                    calamityGlobalNPC.newAI[2] = 0f;
                    calamityGlobalNPC.newAI[3] = 0f;
                }

                speedBoost = true;
                velocity += 5f;
                acceleration += 0.05f;
            }
            else
                despawnTimer++;

            // Despawn
            if (Vector2.Distance(player.Center, vector) > (despawnBoost ? 1500f : 6000f))
            {
                NPC.active = false;
                NPC.netUpdate = true;
                return;
            }

            if (phase2)
            {
                velocity += 2.5f;
                acceleration += 0.02f;
            }

            if (!phase3)
            {
                if (charging)
                {
                    velocity += phase2 ? 4.5f : 3.5f;
                    acceleration += phase2 ? 0.03f : 0.025f;
                }
            }
            else
            {
                if (charging)
                {
                    velocity += phase5 ? 8.5f : 4.5f;
                    acceleration += phase5 ? 0.06f : 0.03f;
                }
                else
                {
                    if (phase5)
                    {
                        velocity += 1.5f;
                        acceleration += 0.015f;
                    }
                    else if (phase4)
                    {
                        velocity += 1f;
                        acceleration += 0.01f;
                    }
                    else
                    {
                        velocity += 0.5f;
                        acceleration += 0.005f;
                    }
                }
            }

            if (expertMode)
            {
                chargeVelocity += revenge ? 4f : 2f;
                velocity += revenge ? 5f : 3.5f;
                acceleration += revenge ? 0.035f : 0.025f;
            }

            NPC.Calamity().CurrentlyEnraged = despawn;

            // Used to inform the clone and hooks about how aggressive they should be.
            NPC.ai[3] = 1.5f;

            float baseProjectileVelocity = speedBoost ? 9.375f : 7.5f;

            // Predictiveness
            Vector2 predictionVector = chargePhase && bossRush ? player.velocity * 20f : Vector2.Zero;
            Vector2 lookAt = player.Center + predictionVector;
            Vector2 rotationVector = lookAt - vector;

            // Rotation
            if (calamityGlobalNPC.newAI[3] == 0f)
            {
                float playerXDestination = player.Center.X + predictionVector.X - vector.X;
                float playerYDestination = player.Center.Y + predictionVector.Y - vector.Y;
                NPC.rotation = (float)Math.Atan2(playerYDestination, playerXDestination) + MathHelper.PiOver2;
            }
            else
                NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;

            int phase1ReducedSetDamage = (int)Math.Round(NPC.defDamage * 0.5);
            int phase2Damage = (int)Math.Round(NPC.defDamage * 1.2);
            int phase2ReducedSetDamage = (int)Math.Round(phase2Damage * 0.5);
            int phase3Damage = (int)Math.Round(NPC.defDamage * 1.4);
            int phase3ReducedSetDamage = (int)Math.Round(phase3Damage * 0.5);

            if (!chargePhase)
            {
                NPC.ai[2] += 1f;
                if (reset)
                {
                    NPC.ai[2] = 0f;
                    NPC.netUpdate = true;
                }

                float movementLimitX = 0f;
                float movementLimitY = 0f;
                int numHooks = 4;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<PolterghastHook>())
                    {
                        movementLimitX += Main.npc[i].Center.X;
                        movementLimitY += Main.npc[i].Center.Y;
                    }
                }
                movementLimitX /= numHooks;
                movementLimitY /= numHooks;

                Vector2 movementLimitVector = new Vector2(movementLimitX, movementLimitY);
                float movementLimitedXDist = player.Center.X - movementLimitVector.X;
                float movementLimitedYDist = player.Center.Y - movementLimitVector.Y;

                if (despawnBoost)
                {
                    movementLimitedYDist *= -1f;
                    movementLimitedXDist *= -1f;
                    velocity += 10f;
                }

                float movementLimitedDistance = (float)Math.Sqrt(movementLimitedXDist * movementLimitedXDist + movementLimitedYDist * movementLimitedYDist);
                float maxDistanceFromHooks = expertMode ? 650f : 500f;
                if (speedBoost || bossRush)
                    maxDistanceFromHooks += 250f;
                if (death)
                    maxDistanceFromHooks += maxDistanceFromHooks * 0.1f * (1f - lifeRatio);

                if (death)
                {
                    velocity += velocity * 0.15f * (1f - lifeRatio);
                    acceleration += acceleration * 0.15f * (1f - lifeRatio);
                }

                if (movementLimitedDistance >= maxDistanceFromHooks)
                {
                    movementLimitedDistance = maxDistanceFromHooks / movementLimitedDistance;
                    movementLimitedXDist *= movementLimitedDistance;
                    movementLimitedYDist *= movementLimitedDistance;
                }

                movementLimitX += movementLimitedXDist;
                movementLimitY += movementLimitedYDist;
                movementLimitedXDist = movementLimitX - vector.X;
                movementLimitedYDist = movementLimitY - vector.Y;
                movementLimitedDistance = (float)Math.Sqrt(movementLimitedXDist * movementLimitedXDist + movementLimitedYDist * movementLimitedYDist);

                if (movementLimitedDistance < velocity)
                {
                    movementLimitedXDist = NPC.velocity.X;
                    movementLimitedYDist = NPC.velocity.Y;
                }
                else
                {
                    movementLimitedDistance = velocity / movementLimitedDistance;
                    movementLimitedXDist *= movementLimitedDistance;
                    movementLimitedYDist *= movementLimitedDistance;
                }

                if (NPC.velocity.X < movementLimitedXDist)
                {
                    NPC.velocity.X += acceleration;
                    if (NPC.velocity.X < 0f && movementLimitedXDist > 0f)
                        NPC.velocity.X += acceleration * 2f;
                }
                else if (NPC.velocity.X > movementLimitedXDist)
                {
                    NPC.velocity.X -= acceleration;
                    if (NPC.velocity.X > 0f && movementLimitedXDist < 0f)
                        NPC.velocity.X -= acceleration * 2f;
                }
                if (NPC.velocity.Y < movementLimitedYDist)
                {
                    NPC.velocity.Y += acceleration;
                    if (NPC.velocity.Y < 0f && movementLimitedYDist > 0f)
                        NPC.velocity.Y += acceleration * 2f;
                }
                else if (NPC.velocity.Y > movementLimitedYDist)
                {
                    NPC.velocity.Y -= acceleration;
                    if (NPC.velocity.Y > 0f && movementLimitedYDist < 0f)
                        NPC.velocity.Y -= acceleration * 2f;
                }
            }
            else
            {
                // Charge
                if (calamityGlobalNPC.newAI[3] == 1f)
                {
                    reachedChargingPoint = false;

                    if (calamityGlobalNPC.newAI[1] == 0f)
                    {
                        NPC.damage = phase3 ? phase3Damage : phase2 ? phase2Damage : NPC.defDamage;

                        NPC.velocity = Vector2.Normalize(rotationVector) * chargeVelocity;
                        calamityGlobalNPC.newAI[1] = 1f;
                    }
                    else
                    {
                        NPC.damage = phase3 ? phase3Damage : phase2 ? phase2Damage : NPC.defDamage;

                        calamityGlobalNPC.newAI[2] += 1f;

                        // Slow down for a few frames
                        float totalChargeTime = chargeDistance * 4f / chargeVelocity;
                        float slowDownTime = chargeVelocity;
                        if (calamityGlobalNPC.newAI[2] >= totalChargeTime - slowDownTime)
                        {
                            NPC.damage = phase3 ? phase3ReducedSetDamage : phase2 ? phase2ReducedSetDamage : phase1ReducedSetDamage;

                            NPC.velocity *= 0.9f;
                        }

                        // Reset and either go back to normal or charge again
                        if (calamityGlobalNPC.newAI[2] >= totalChargeTime)
                        {
                            calamityGlobalNPC.newAI[1] = 0f;
                            calamityGlobalNPC.newAI[2] = 0f;
                            calamityGlobalNPC.newAI[3] = 0f;
                            NPC.ai[1] += 1f;

                            if (NPC.ai[1] >= chargeAmt)
                            {
                                // Reset and return to normal movement
                                calamityGlobalNPC.newAI[0] = 0f;
                                NPC.ai[1] = 0f;
                            }
                            else
                            {
                                // Get a new target and charge again
                                NPC.TargetClosest();
                            }
                        }
                    }
                }
                else
                {
                    NPC.damage = phase3 ? phase3ReducedSetDamage : phase2 ? phase2ReducedSetDamage : phase1ReducedSetDamage;

                    // Pick a charging location
                    // Set charge locations X
                    if (vector.X >= player.Center.X)
                        calamityGlobalNPC.newAI[1] = player.Center.X + chargeDistance;
                    else
                        calamityGlobalNPC.newAI[1] = player.Center.X - chargeDistance;

                    // Set charge locations Y
                    if (vector.Y >= player.Center.Y)
                        calamityGlobalNPC.newAI[2] = player.Center.Y + chargeDistance;
                    else
                        calamityGlobalNPC.newAI[2] = player.Center.Y - chargeDistance;

                    Vector2 chargeVector = new Vector2(calamityGlobalNPC.newAI[1], calamityGlobalNPC.newAI[2]);
                    Vector2 chargeLocationVelocity = Vector2.Normalize(chargeVector - vector) * chargeVelocity;
                    Vector2 cloneChargeVector = cloneAlive ? new Vector2(Main.npc[CalamityGlobalNPC.ghostBossClone].Calamity().newAI[1], Main.npc[CalamityGlobalNPC.ghostBossClone].Calamity().newAI[2]) : default;

                    // If clone is alive and not at proper location then keep trying to line up until it gets into position
                    float chargeDistanceGateValue = 40f;
                    bool clonePositionCheck = cloneAlive ? Vector2.Distance(Main.npc[CalamityGlobalNPC.ghostBossClone].Center, cloneChargeVector) <= chargeDistanceGateValue : true;

                    // Line up a charge
                    if (Vector2.Distance(vector, chargeVector) <= chargeDistanceGateValue || reachedChargingPoint)
                    {
                        // Emit dust
                        if (!reachedChargingPoint)
                        {
                            SoundEngine.PlaySound(SoundID.Item125, NPC.Center);
                            for (int i = 0; i < 30; i++)
                            {
                                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Ectoplasm, 0f, 0f, 100, default, 3f);
                                Main.dust[dust].noGravity = true;
                                Main.dust[dust].velocity *= 5f;
                            }
                        }

                        reachedChargingPoint = true;
                        NPC.velocity = Vector2.Zero;
                        NPC.Center = chargeVector;

                        if (clonePositionCheck)
                        {
                            // Initiate charge
                            calamityGlobalNPC.newAI[1] = 0f;
                            calamityGlobalNPC.newAI[2] = 0f;
                            calamityGlobalNPC.newAI[3] = 1f;

                            // Tell clone to charge
                            if (cloneAlive)
                            {
                                Main.npc[CalamityGlobalNPC.ghostBossClone].ai[0] = 0f;
                                Main.npc[CalamityGlobalNPC.ghostBossClone].Calamity().newAI[1] = 0f;
                                Main.npc[CalamityGlobalNPC.ghostBossClone].Calamity().newAI[2] = 0f;
                                Main.npc[CalamityGlobalNPC.ghostBossClone].Calamity().newAI[3] = 1f;

                                //
                                // CODE TWEAKED BY: OZZATRON
                                // September 21st, 2020
                                // reason: fixing Polter charge MP desync bug
                                //
                                // removed Polter syncing the clone's newAI array. The clone now auto syncs its own newAI every frame.
                            }
                        }
                    }
                    else
                        NPC.SimpleFlyMovement(chargeLocationVelocity, chargeAcceleration);
                }

                NPC.netUpdate = true;

                if (NPC.netSpam > 10)
                    NPC.netSpam = 10;

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI, 0f, 0f, 0f, 0, 0, 0);
            }

            bool isInChargePhase = charging && chargePhase;

            // Phase 1: "Polterghast"
            if (!phase2 && !phase3)
            {
                if (!isInChargePhase)
                    NPC.damage = phase1ReducedSetDamage;

                NPC.defense = NPC.defDefense;

                if (Main.netMode != NetmodeID.MultiplayerClient && !isInChargePhase)
                {
                    NPC.localAI[1] += expertMode ? 1.5f : 1f;
                    if (speedBoost)
                        NPC.localAI[1] += 2f;

                    if (NPC.localAI[1] >= 120f)
                    {
                        NPC.localAI[1] = 0f;

                        bool notLiningUpCharge = Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height);
                        if (NPC.localAI[3] > 0f)
                        {
                            notLiningUpCharge = true;
                            NPC.localAI[3] = 0f;
                        }

                        if (notLiningUpCharge)
                        {
                            int type = ModContent.ProjectileType<PhantomShot>();
                            if (Main.rand.NextBool(3))
                            {
                                NPC.localAI[1] = -30f;
                                type = ModContent.ProjectileType<PhantomBlast>();
                            }

                            int damage = NPC.GetProjectileDamage(type);

                            Vector2 firingPosition = vector;
                            float playerXDist = player.Center.X - firingPosition.X;
                            float playerYDist = player.Center.Y - firingPosition.Y;
                            float playerDistance = (float)Math.Sqrt(playerXDist * playerXDist + playerYDist * playerYDist);

                            playerDistance = baseProjectileVelocity / playerDistance;
                            playerXDist *= playerDistance;
                            playerYDist *= playerDistance;
                            firingPosition.X += playerXDist * 3f;
                            firingPosition.Y += playerYDist * 3f;

                            float rotation = MathHelper.ToRadians(65);
                            float baseSpeed = (float)Math.Sqrt(playerXDist * playerXDist + playerYDist * playerYDist);
                            double startAngle = Math.Atan2(playerXDist, playerYDist) - rotation / 2;
                            double deltaAngle = rotation / 6;
                            double offsetAngle;
                            for (int i = 0; i < 6; i++)
                            {
                                offsetAngle = startAngle + deltaAngle * i;
                                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), firingPosition.X, firingPosition.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), type, damage, 0f, Main.myPlayer, 0f, 0f);
                                Main.projectile[proj].timeLeft = type == ModContent.ProjectileType<PhantomBlast>() ? 450 : 1800;
                            }
                        }
                        else
                        {
                            int type = ModContent.ProjectileType<PhantomBlast>();
                            int damage = NPC.GetProjectileDamage(type);

                            Vector2 firingPosition = vector;
                            float playerXDist = player.Center.X - firingPosition.X;
                            float playerYDist = player.Center.Y - firingPosition.Y;
                            float playerDistance = (float)Math.Sqrt(playerXDist * playerXDist + playerYDist * playerYDist);

                            playerDistance = (baseProjectileVelocity + 5f) / playerDistance;
                            playerXDist *= playerDistance;
                            playerYDist *= playerDistance;
                            firingPosition.X += playerXDist * 3f;
                            firingPosition.Y += playerYDist * 3f;

                            float rotation = MathHelper.ToRadians(80);
                            float baseSpeed = (float)Math.Sqrt(playerXDist * playerXDist + playerYDist * playerYDist);
                            double startAngle = Math.Atan2(playerXDist, playerYDist) - rotation / 2;
                            double deltaAngle = rotation / 6;
                            double offsetAngle;
                            for (int i = 0; i < 6; i++)
                            {
                                offsetAngle = startAngle + deltaAngle * i;
                                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), firingPosition.X, firingPosition.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), type, damage, 0f, Main.myPlayer, 0f, 0f);
                                Main.projectile[proj].timeLeft = 450;
                            }
                        }
                    }
                }
            }

            // Phase 2: "Necroghast"
            else if (!phase3)
            {
                if (NPC.ai[0] == 0f)
                {
                    NPC.ai[0] = 1f;

                    // Reset charge attack arrays to prevent problems
                    NPC.ai[1] = 0f;
                    calamityGlobalNPC.newAI[0] = 0f;
                    calamityGlobalNPC.newAI[1] = 0f;
                    calamityGlobalNPC.newAI[2] = 0f;
                    calamityGlobalNPC.newAI[3] = 0f;

                    SoundEngine.PlaySound(P2Sound, NPC.Center);

                    if (Main.netMode != NetmodeID.Server)
                    {
                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Polt").Type, 1f);
                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Polt2").Type, 1f);
                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Polt3").Type, 1f);
                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Polt4").Type, 1f);
                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Polt5").Type, 1f);
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        int ghostDust = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Necroplasm, 0f, 0f, 100, default, 2f);
                        Main.dust[ghostDust].velocity *= 3f;
                        Main.dust[ghostDust].noGravity = true;
                        if (Main.rand.NextBool())
                        {
                            Main.dust[ghostDust].scale = 0.5f;
                            Main.dust[ghostDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                        }
                    }
                    for (int j = 0; j < 30; j++)
                    {
                        int ghostDust2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Ectoplasm, 0f, 0f, 100, default, 3f);
                        Main.dust[ghostDust2].noGravity = true;
                        Main.dust[ghostDust2].velocity *= 5f;
                        ghostDust2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Ectoplasm, 0f, 0f, 100, default, 2f);
                        Main.dust[ghostDust2].velocity *= 2f;
                    }
                }

                if (!isInChargePhase)
                    NPC.damage = phase2ReducedSetDamage;

                NPC.defense = (int)Math.Round(NPC.defDefense * 0.8);

                if (Main.netMode != NetmodeID.MultiplayerClient && !isInChargePhase)
                {
                    NPC.localAI[1] += expertMode ? 1.5f : 1f;
                    if (speedBoost)
                        NPC.localAI[1] += 2f;

                    if (NPC.localAI[1] >= 200f)
                    {
                        NPC.localAI[1] = 0f;

                        bool notLiningUpCharge = Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height);
                        if (NPC.localAI[3] > 0f)
                        {
                            notLiningUpCharge = true;
                            NPC.localAI[3] = 0f;
                        }

                        if (notLiningUpCharge)
                        {
                            int type = ModContent.ProjectileType<PhantomShot2>();
                            if (Main.rand.NextBool(3))
                            {
                                NPC.localAI[1] = -30f;
                                type = ModContent.ProjectileType<PhantomBlast2>();
                            }

                            int damage = NPC.GetProjectileDamage(type);

                            Vector2 firingPosition = vector;
                            float playerXDist = player.Center.X - firingPosition.X;
                            float playerYDist = player.Center.Y - firingPosition.Y;
                            float playerDistance = (float)Math.Sqrt(playerXDist * playerXDist + playerYDist * playerYDist);

                            playerDistance = (baseProjectileVelocity + 1f) / playerDistance;
                            playerXDist *= playerDistance;
                            playerYDist *= playerDistance;
                            firingPosition.X += playerXDist * 3f;
                            firingPosition.Y += playerYDist * 3f;

                            int numProj = 7;
                            float rotation = MathHelper.ToRadians(80);
                            float baseSpeed = (float)Math.Sqrt(playerXDist * playerXDist + playerYDist * playerYDist);
                            double startAngle = Math.Atan2(playerXDist, playerYDist) - rotation / 2;
                            double deltaAngle = rotation / numProj;
                            double offsetAngle;
                            for (int i = 0; i < numProj; i++)
                            {
                                offsetAngle = startAngle + deltaAngle * i;
                                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), firingPosition.X, firingPosition.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), type, damage, 0f, Main.myPlayer, 0f, 0f);
                                Main.projectile[proj].timeLeft = type == ModContent.ProjectileType<PhantomBlast2>() ? 450 : 1800;
                            }
                        }
                        else
                        {
                            int type = ModContent.ProjectileType<PhantomBlast2>();
                            int damage = NPC.GetProjectileDamage(type);

                            Vector2 firingPosition = vector;
                            float playerXDist = player.Center.X - firingPosition.X;
                            float playerYDist = player.Center.Y - firingPosition.Y;
                            float playerDistance = (float)Math.Sqrt(playerXDist * playerXDist + playerYDist * playerYDist);

                            playerDistance = (baseProjectileVelocity + 5f) / playerDistance;
                            playerXDist *= playerDistance;
                            playerYDist *= playerDistance;
                            firingPosition.X += playerXDist * 3f;
                            firingPosition.Y += playerYDist * 3f;

                            int numProj = 7;
                            float rotation = MathHelper.ToRadians(100);
                            float baseSpeed = (float)Math.Sqrt(playerXDist * playerXDist + playerYDist * playerYDist);
                            double startAngle = Math.Atan2(playerXDist, playerYDist) - rotation / 2;
                            double deltaAngle = rotation / numProj;
                            double offsetAngle;
                            for (int i = 0; i < numProj; i++)
                            {
                                offsetAngle = startAngle + deltaAngle * i;
                                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), firingPosition.X, firingPosition.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), type, damage, 0f, Main.myPlayer, 0f, 0f);
                                Main.projectile[proj].timeLeft = 450;
                            }
                        }
                    }
                }
            }

            // Phase 3: "Necroplasm"
            else
            {
                NPC.HitSound = SoundID.NPCHit36;

                if (NPC.ai[0] == 1f)
                {
                    NPC.ai[0] = 2f;

                    // Reset charge attack arrays to prevent problems
                    NPC.ai[1] = 0f;
                    calamityGlobalNPC.newAI[0] = 0f;
                    calamityGlobalNPC.newAI[1] = 0f;
                    calamityGlobalNPC.newAI[2] = 0f;
                    calamityGlobalNPC.newAI[3] = 0f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.NewNPC(NPC.GetSource_FromAI(), (int)vector.X, (int)vector.Y, ModContent.NPCType<PolterPhantom>());

                        if (expertMode && !Main.zenithWorld)
                        {
                            for (int I = 0; I < 3; I++)
                            {
                                int spawn = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(vector.X + (Math.Sin(I * 120) * 500)), (int)(vector.Y + (Math.Cos(I * 120) * 500)), ModContent.NPCType<PhantomFuckYou>(), NPC.whoAmI, 0, 0, 0, -1);
                                NPC npc2 = Main.npc[spawn];
                                npc2.ai[0] = I * 120;
                            }
                        }
                    }

                    SoundEngine.PlaySound(P3Sound, NPC.Center);

                    if (Main.netMode != NetmodeID.Server)
                    {
                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Polt").Type, 1f);
                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Polt2").Type, 1f);
                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Polt3").Type, 1f);
                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Polt4").Type, 1f);
                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Polt5").Type, 1f);
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        int ghostDust = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Necroplasm, 0f, 0f, 100, default, 2f);
                        Main.dust[ghostDust].velocity *= 3f;
                        Main.dust[ghostDust].noGravity = true;
                        if (Main.rand.NextBool())
                        {
                            Main.dust[ghostDust].scale = 0.5f;
                            Main.dust[ghostDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                        }
                    }
                    for (int j = 0; j < 30; j++)
                    {
                        int ghostDust2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Ectoplasm, 0f, 0f, 100, default, 3f);
                        Main.dust[ghostDust2].noGravity = true;
                        Main.dust[ghostDust2].velocity *= 5f;
                        ghostDust2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Ectoplasm, 0f, 0f, 100, default, 2f);
                        Main.dust[ghostDust2].velocity *= 2f;
                    }
                }

                if (!isInChargePhase)
                    NPC.damage = phase3ReducedSetDamage;

                NPC.defense = (int)Math.Round(NPC.defDefense * 0.5);

                NPC.localAI[1] += 1f;
                if (NPC.localAI[1] >= (getPissed ? 200f : 280f) && Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                {
                    NPC.localAI[1] = 0f;
                    if (Main.netMode != NetmodeID.MultiplayerClient && !isInChargePhase)
                    {
                        Vector2 firingPosition = vector;
                        float playerXDist = player.Center.X - firingPosition.X;
                        float playerYDist = player.Center.Y - firingPosition.Y;
                        float playerDistance = (float)Math.Sqrt(playerXDist * playerXDist + playerYDist * playerYDist);

                        playerDistance = baseProjectileVelocity / playerDistance;
                        playerXDist *= playerDistance;
                        playerYDist *= playerDistance;
                        firingPosition.X += playerXDist * 3f;
                        firingPosition.Y += playerYDist * 3f;

                        int numProj = 6 + (getPissed ? 4 : 2);
                        float rotation = MathHelper.ToRadians(110 + (getPissed ? 15 : 0));
                        float baseSpeed = (float)Math.Sqrt(playerXDist * playerXDist + playerYDist * playerYDist);
                        double startAngle = Math.Atan2(playerXDist, playerYDist) - rotation / 2;
                        double deltaAngle = rotation / numProj;
                        double offsetAngle;

                        int type = Main.rand.NextBool() ? ModContent.ProjectileType<PhantomShot2>() : ModContent.ProjectileType<PhantomShot>();
                        int damage = NPC.GetProjectileDamage(type);

                        for (int i = 0; i < numProj; i++)
                        {
                            offsetAngle = startAngle + deltaAngle * i;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), firingPosition.X, firingPosition.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), type, damage, 0f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }

                if (phase4)
                {
                    NPC.localAI[2] += 1f;
                    if (NPC.localAI[2] >= (getPissed ? 300f : 420f))
                    {
                        NPC.localAI[2] = 0f;

                        Vector2 spiritSpawn = vector;
                        float spiritXDist = player.Center.X - spiritSpawn.X;
                        float spiritYDist = player.Center.Y - spiritSpawn.Y;
                        float spiritDistance = (float)Math.Sqrt(spiritXDist * spiritXDist + spiritYDist * spiritYDist);
                        spiritDistance = 6f / spiritDistance;
                        spiritXDist *= spiritDistance;
                        spiritYDist *= spiritDistance;
                        spiritSpawn.X += spiritXDist * 3f;
                        spiritSpawn.Y += spiritYDist * 3f;

                        if (NPC.CountNPCS(ModContent.NPCType<PhantomSpiritL>()) < 2 && Main.netMode != NetmodeID.MultiplayerClient && !isInChargePhase)
                        {
                            SoundEngine.PlaySound(PhantomSound, NPC.Center);
                            int phantomSpirit = NPC.NewNPC(NPC.GetSource_FromAI(), (int)vector.X, (int)vector.Y, ModContent.NPCType<PhantomSpiritL>());
                            Main.npc[phantomSpirit].velocity.X = spiritXDist;
                            Main.npc[phantomSpirit].velocity.Y = spiritYDist;
                            Main.npc[phantomSpirit].netUpdate = true;
                        }
                    }
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ModContent.ItemType<SupremeHealingPotion>();
        }

        public override void OnKill()
        {
            CalamityGlobalNPC.SetNewBossJustDowned(NPC);

            CalamityGlobalNPC.SetNewShopVariable(new int[] { NPCID.Cyborg }, DownedBossSystem.downedPolterghast);

            // If Polterghast has not been killed, notify players about the Abyss minibosses now dropping items
            if (!DownedBossSystem.downedPolterghast)
            {
                if (!Main.player[Main.myPlayer].dead && Main.player[Main.myPlayer].active)
                    SoundEngine.PlaySound(ReaperShark.SearchRoarSound, Main.player[Main.myPlayer].Center);

                string key = "Mods.CalamityMod.Status.Progression.GhostBossText";
                Color messageColor = Color.RoyalBlue;
                string sulfSeaBoostMessage = "Mods.CalamityMod.Status.Progression.GhostBossText4";
                Color sulfSeaBoostColor = AcidRainEvent.TextColor;

                if ((Main.rand.NextBool(20) && DateTime.Now.Month == 4 && DateTime.Now.Day == 1) || Main.zenithWorld)
                {
                    sulfSeaBoostMessage = "Mods.CalamityMod.Status.Progression.AprilFools2"; // Goddamn boomer duke moments
                }

                CalamityUtils.DisplayLocalizedText(key, messageColor);
                CalamityUtils.DisplayLocalizedText(sulfSeaBoostMessage, sulfSeaBoostColor);
            }

            // Mark Polterghast as dead
            DownedBossSystem.downedPolterghast = true;
            CalamityNetcode.SyncWorld();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<PolterghastBag>()));

            // Normal drops: Everything that would otherwise be in the bag
            var normalOnly = npcLoot.DefineNormalOnlyDropSet();
            {
                // Weapons
                int[] weapons = new int[]
                {
                    ModContent.ItemType<TerrorBlade>(),
                    ModContent.ItemType<BansheeHook>(),
                    ModContent.ItemType<DaemonsFlame>(),
                    ModContent.ItemType<FatesReveal>(),
                    ModContent.ItemType<GhastlyVisage>(),
                    ModContent.ItemType<EtherealSubjugator>(),
                    ModContent.ItemType<GhoulishGouger>(),
                };
                normalOnly.Add(DropHelper.CalamityStyle(DropHelper.NormalWeaponDropRateFraction, weapons));

                // Equipment
                normalOnly.Add(DropHelper.PerPlayer(ModContent.ItemType<Affliction>()));

                // Materials
                normalOnly.Add(DropHelper.PerPlayer(ModContent.ItemType<RuinousSoul>(), 1, 7, 15));
                normalOnly.Add(ModContent.ItemType<Necroplasm>(), 1, 30, 40);

                // Vanity
                normalOnly.Add(ModContent.ItemType<PolterghastMask>(), 7);
                normalOnly.Add(ModContent.ItemType<ThankYouPainting>(), ThankYouPainting.DropInt);
            }

            npcLoot.Add(ModContent.ItemType<PolterghastTrophy>(), 10);

            // Relic
            npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<PolterghastRelic>());

            // GFB Cell Phone drop
            var GFBOnly = npcLoot.DefineConditionalDropSet(DropHelper.GFB);
            {
                GFBOnly.Add(ItemID.CellPhone, hideLootReport: true);
            }

            // Lore
            npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedPolterghast, ModContent.ItemType<LorePolterghast>(), desc: DropHelper.FirstKillText);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (threeAM)
            {
                int bloodBase = 120 - NPC.life / NPC.lifeMax;
                float roughBloodCount = (float)Math.Sqrt(0.8f * bloodBase);
                int exactBloodCount = (int)roughBloodCount;
                // Chance for the final blood particle
                if (Main.rand.NextFloat() < roughBloodCount - exactBloodCount)
                    ++exactBloodCount;

                // Velocity of the spurting blood also slightly increases with stacks.
                float velStackMult = 1f + (float)Math.Log(bloodBase);

                // Code copied from Shred which was copied from Violence.
                for (int i = 0; i < exactBloodCount; ++i)
                {
                    int bloodLifetime = Main.rand.Next(22, 36);
                    float bloodScale = Main.rand.NextFloat(0.6f, 0.8f);
                    Color bloodColor = Color.Lerp(Color.Red, Color.DarkRed, Main.rand.NextFloat());
                    bloodColor = Color.Lerp(bloodColor, new Color(51, 22, 94), Main.rand.NextFloat(0.65f));

                    if (Main.rand.NextBool(20))
                        bloodScale *= 2f;

                    float randomSpeedMultiplier = Main.rand.NextFloat(1.25f, 2.25f);
                    Vector2 bloodVelocity = Main.rand.NextVector2Unit() * velStackMult * randomSpeedMultiplier;
                    bloodVelocity.Y -= 5f;
                    BloodParticle blood = new BloodParticle(NPC.Center, bloodVelocity, bloodLifetime, bloodScale, bloodColor);
                    GeneralParticleHandler.SpawnParticle(blood);
                }
                for (int i = 0; i < exactBloodCount / 3; ++i)
                {
                    float bloodScale = Main.rand.NextFloat(0.2f, 0.33f);
                    Color bloodColor = Color.Lerp(Color.Red, Color.DarkRed, Main.rand.NextFloat(0.5f, 1f));
                    Vector2 bloodVelocity = Main.rand.NextVector2Unit() * velStackMult * Main.rand.NextFloat(1f, 2f);
                    bloodVelocity.Y -= 2.3f;
                    BloodParticle2 blood = new BloodParticle2(NPC.Center, bloodVelocity, 20, bloodScale, bloodColor);
                    GeneralParticleHandler.SpawnParticle(blood);
                }
            }

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
            Texture2D texture2D16 = Texture_Glow2.Value;
            Vector2 halfSizeTexture = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
            int afterimageAmt = 7;

            if (CalamityConfig.Instance.Afterimages)
            {
                for (int i = 1; i < afterimageAmt; i += 2)
                {
                    Color afterimageColor = drawColor;
                    afterimageColor = Color.Lerp(afterimageColor, Color.White, 0.5f);
                    afterimageColor = NPC.GetAlpha(afterimageColor);
                    afterimageColor *= (afterimageAmt - i) / 15f;
                    Vector2 afterimagePos = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
                    afterimagePos -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    afterimagePos += halfSizeTexture * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(texture2D15, afterimagePos, NPC.frame, afterimageColor, NPC.rotation, halfSizeTexture, NPC.scale, spriteEffects, 0f);
                }
            }

            Vector2 vector43 = NPC.Center - screenPos;
            vector43 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
            vector43 += halfSizeTexture * NPC.scale + new Vector2(0f, NPC.gfxOffY);
            Color c = NPC.IsABestiaryIconDummy ? Color.White : NPC.GetAlpha(drawColor);
            spriteBatch.Draw(texture2D15, vector43, NPC.frame, c, NPC.rotation, halfSizeTexture, NPC.scale, spriteEffects, 0f);

            texture2D15 = Texture_Glow.Value;

            Color secondColorLerp = Color.Lerp(Color.White, Color.Cyan, 0.5f);
            Color lightRed = new Color(255, 100, 100, 255);

            if (threeAM)
            {
                secondColorLerp = Color.Red;
                lightRed = Color.DarkRed;
            }

            float chargePhaseGateValue = 480f;
            if (Main.getGoodWorld)
                chargePhaseGateValue *= 0.5f;

            float timeToReachFullColor = 120f;
            float colorChangeTime = 180f;
            float changeColorGateValue = chargePhaseGateValue - colorChangeTime;
            if (NPC.Calamity().newAI[0] > changeColorGateValue)
                secondColorLerp = Color.Lerp(secondColorLerp, lightRed, MathHelper.Clamp((NPC.Calamity().newAI[0] - changeColorGateValue) / timeToReachFullColor, 0f, 1f));

            Color veinColor = Color.Lerp(Color.White, (NPC.ai[2] >= changeColorGateValue || NPC.Calamity().newAI[0] > changeColorGateValue) ? Color.Red : Color.Black, 0.5f);

            if (CalamityConfig.Instance.Afterimages)
            {
                for (int j = 1; j < afterimageAmt; j++)
                {
                    Color otherAfterimageColor = secondColorLerp;
                    otherAfterimageColor = Color.Lerp(otherAfterimageColor, Color.White, 0.5f);
                    otherAfterimageColor = NPC.GetAlpha(otherAfterimageColor);
                    otherAfterimageColor *= (afterimageAmt - j) / 15f;
                    Vector2 otherAfterimagePos = NPC.oldPos[j] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
                    otherAfterimagePos -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    otherAfterimagePos += halfSizeTexture * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(texture2D15, otherAfterimagePos, NPC.frame, otherAfterimageColor, NPC.rotation, halfSizeTexture, NPC.scale, spriteEffects, 0f);

                    Color veinAfterimageColor = veinColor;
                    veinAfterimageColor = Color.Lerp(veinAfterimageColor, Color.White, 0.5f);
                    veinAfterimageColor = NPC.GetAlpha(veinAfterimageColor);
                    veinAfterimageColor *= (afterimageAmt - j) / 15f;
                    spriteBatch.Draw(texture2D16, otherAfterimagePos, NPC.frame, veinAfterimageColor, NPC.rotation, halfSizeTexture, NPC.scale, spriteEffects, 0f);
                }
            }

            spriteBatch.Draw(texture2D15, vector43, NPC.frame, secondColorLerp, NPC.rotation, halfSizeTexture, NPC.scale, spriteEffects, 0f);

            spriteBatch.Draw(texture2D16, vector43, NPC.frame, veinColor, NPC.rotation, halfSizeTexture, NPC.scale, spriteEffects, 0f);

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.IsABestiaryIconDummy)
                NPC.Opacity = 1f;

            // Percent life remaining
            float lifeRatio = NPC.life / (float)NPC.lifeMax;

            bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
            bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            bool death = CalamityWorld.death || BossRushEvent.BossRushActive;

            bool phase2 = lifeRatio < (death ? 0.9f : revenge ? 0.8f : expertMode ? 0.65f : 0.5f);
            bool phase3 = lifeRatio < (death ? 0.6f : revenge ? 0.5f : expertMode ? 0.35f : 0.2f);

            NPC.frameCounter += 1D;
            if (NPC.frameCounter > 6D)
            {
                NPC.frameCounter = 0D;
                NPC.frame.Y += frameHeight;
            }
            if (phase3)
            {
                if (NPC.frame.Y < frameHeight * 8)
                {
                    NPC.frame.Y = frameHeight * 8;
                }
                if (NPC.frame.Y > frameHeight * 11)
                {
                    NPC.frame.Y = frameHeight * 8;
                }
            }
            else if (phase2)
            {
                if (NPC.frame.Y < frameHeight * 4)
                {
                    NPC.frame.Y = frameHeight * 4;
                }
                if (NPC.frame.Y > frameHeight * 7)
                {
                    NPC.frame.Y = frameHeight * 4;
                }
            }
            else
            {
                if (NPC.frame.Y > frameHeight * 3)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (hurtInfo.Damage > 0)
                target.AddBuff(BuffID.MoonLeech, 900, true);
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance * bossAdjustment);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Ectoplasm, hit.HitDirection, -1f, 0, default, 1f);
            if (NPC.life <= 0)
            {
                NPC.position.X = NPC.position.X + (NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (NPC.height / 2);
                NPC.width = 90;
                NPC.height = 90;
                NPC.position.X = NPC.position.X - (NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (NPC.height / 2);
                for (int i = 0; i < 10; i++)
                {
                    int ghostDust = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Necroplasm, 0f, 0f, 100, default, 2f);
                    Main.dust[ghostDust].velocity *= 3f;
                    if (Main.rand.NextBool())
                    {
                        Main.dust[ghostDust].scale = 0.5f;
                        Main.dust[ghostDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int j = 0; j < 60; j++)
                {
                    int ghostDust2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Ectoplasm, 0f, 0f, 100, default, 3f);
                    Main.dust[ghostDust2].noGravity = true;
                    Main.dust[ghostDust2].velocity *= 5f;
                    ghostDust2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Ectoplasm, 0f, 0f, 100, default, 2f);
                    Main.dust[ghostDust2].velocity *= 2f;
                }
            }
        }
    }
}
