﻿using System;
using CalamityMod.BiomeManagers;
using CalamityMod.Items.Placeables.Banners;
using CalamityMod.Items.Weapons.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalamityMod.NPCs.SunkenSea
{
    public class SeaSerpent1 : ModNPC
    {
        public const int maxLength = 9;
        public float speed = 3f;
        public float turnSpeed = 0.0625f;
        bool TailSpawned = false;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "CalamityMod/ExtraTextures/Bestiary/SeaSerpent_Bestiary",
                PortraitPositionXOverride = 40,
                PortraitPositionYOverride = 20
            };
            value.Position.Y += 20;
            value.Position.X += 40;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }

        public override void SetDefaults()
        {
            NPC.damage = 50;
            NPC.width = 50; //42
            NPC.height = 24; //32
            NPC.defense = 10;
            NPC.lifeMax = 3000;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 20, 0);
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<SeaSerpentBanner>();
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToWater = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SunkenSeaBiome>().Type };

            // Scale stats in Expert and Master
            CalamityGlobalNPC.AdjustExpertModeStatScaling(NPC);
            CalamityGlobalNPC.AdjustMasterModeStatScaling(NPC);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("Mods.CalamityMod.Bestiary.SeaSerpent")
            });
        }

        public override void AI()
        {
            Point point = NPC.Center.ToTileCoordinates();
            Tile tileSafely = Framing.GetTileSafely(point);
            bool createDust = tileSafely.HasUnactuatedTile && NPC.Distance(Main.player[NPC.target].Center) < 800f;
            if (createDust)
            {
                if (Main.rand.NextBool())
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.TreasureSparkle, 0f, 0f, 150, default(Color), 0.3f);
                    dust.fadeIn = 0.75f;
                    dust.velocity *= 0.1f;
                    dust.noLight = true;
                }
            }

            Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0f / 255f, (255 - NPC.alpha) * 0.30f / 255f, (255 - NPC.alpha) * 0.30f / 255f);
            if (NPC.ai[2] > 0f)
            {
                NPC.realLife = (int)NPC.ai[2];
            }
            if (NPC.target < 0 || NPC.target == Main.maxPlayers || Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(true);
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!TailSpawned && NPC.ai[0] == 0f)
                {
                    int Previous = NPC.whoAmI;
                    for (int segment = 0; segment < maxLength; segment++)
                    {
                        int lol = 0;
                        if (segment == 0 || segment == 1 || segment == 4 || segment == 5)
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<SeaSerpent2>(), NPC.whoAmI);
                        }
                        else if (segment == 2 || segment == 3 || segment == 6)
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<SeaSerpent3>(), NPC.whoAmI);
                        }
                        else if (segment == 7)
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<SeaSerpent4>(), NPC.whoAmI);
                        }
                        else if (segment == 8)
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<SeaSerpent5>(), NPC.whoAmI);
                        }
                        Main.npc[lol].realLife = NPC.whoAmI;
                        Main.npc[lol].ai[2] = (float)NPC.whoAmI;
                        Main.npc[lol].ai[1] = (float)Previous;
                        Main.npc[Previous].ai[0] = (float)lol;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, lol, 0f, 0f, 0f, 0);
                        Previous = lol;
                    }
                    TailSpawned = true;
                }
            }
            if (NPC.velocity.X < 0f)
            {
                NPC.spriteDirection = -1;
            }
            else if (NPC.velocity.X > 0f)
            {
                NPC.spriteDirection = 1;
            }
            if (Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(false);
            }
            NPC.alpha -= 42;
            if (NPC.alpha < 0)
            {
                NPC.alpha = 0;
            }
            if (Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) > 5600f)
            {
                NPC.active = false;
            }
            float currentSpeed = speed;
            float currentTurnSpeed = turnSpeed;
            Vector2 segmentPosition = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
            float targetXDist = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2);
            float targetYDist = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2);
            if (NPC.life > NPC.lifeMax * 0.99)
            {
                targetYDist += 300;
                if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) < 250f)
                {
                    if (NPC.velocity.X > 0f)
                    {
                        targetXDist = Main.player[NPC.target].Center.X + 300f;
                    }
                    else
                    {
                        targetXDist = Main.player[NPC.target].Center.X - 300f;
                    }
                }
            }
            else
            {
                currentSpeed *= 1.5f;
                currentTurnSpeed *= 1.5f;
            }
            float maxCurrentSpeed = currentSpeed * 1.3f;
            float minCurrentSpeed = currentSpeed * 0.7f;
            float speedCompare = NPC.velocity.Length();
            if (speedCompare > 0f)
            {
                if (speedCompare > maxCurrentSpeed)
                {
                    NPC.velocity.Normalize();
                    NPC.velocity *= maxCurrentSpeed;
                }
                else if (speedCompare < minCurrentSpeed)
                {
                    NPC.velocity.Normalize();
                    NPC.velocity *= minCurrentSpeed;
                }
            }
            targetXDist = (float)((int)(targetXDist / 16f) * 16);
            targetYDist = (float)((int)(targetYDist / 16f) * 16);
            segmentPosition.X = (float)((int)(segmentPosition.X / 16f) * 16);
            segmentPosition.Y = (float)((int)(segmentPosition.Y / 16f) * 16);
            targetXDist -= segmentPosition.X;
            targetYDist -= segmentPosition.Y;
            float targetDistance = (float)System.Math.Sqrt((double)(targetXDist * targetXDist + targetYDist * targetYDist));
            float absoluteTargetX = System.Math.Abs(targetXDist);
            float absoluteTargetY = System.Math.Abs(targetYDist);
            float timeToReachTarget = currentSpeed / targetDistance;
            targetXDist *= timeToReachTarget;
            targetYDist *= timeToReachTarget;
            if ((NPC.velocity.X > 0f && targetXDist > 0f) || (NPC.velocity.X < 0f && targetXDist < 0f) || (NPC.velocity.Y > 0f && targetYDist > 0f) || (NPC.velocity.Y < 0f && targetYDist < 0f))
            {
                if (NPC.velocity.X < targetXDist)
                {
                    NPC.velocity.X = NPC.velocity.X + currentTurnSpeed;
                }
                else
                {
                    if (NPC.velocity.X > targetXDist)
                    {
                        NPC.velocity.X = NPC.velocity.X - currentTurnSpeed;
                    }
                }
                if (NPC.velocity.Y < targetYDist)
                {
                    NPC.velocity.Y = NPC.velocity.Y + currentTurnSpeed;
                }
                else
                {
                    if (NPC.velocity.Y > targetYDist)
                    {
                        NPC.velocity.Y = NPC.velocity.Y - currentTurnSpeed;
                    }
                }
                if ((double)System.Math.Abs(targetYDist) < (double)currentSpeed * 0.2 && ((NPC.velocity.X > 0f && targetXDist < 0f) || (NPC.velocity.X < 0f && targetXDist > 0f)))
                {
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y = NPC.velocity.Y + currentTurnSpeed * 2f;
                    }
                    else
                    {
                        NPC.velocity.Y = NPC.velocity.Y - currentTurnSpeed * 2f;
                    }
                }
                if ((double)System.Math.Abs(targetXDist) < (double)currentSpeed * 0.2 && ((NPC.velocity.Y > 0f && targetYDist < 0f) || (NPC.velocity.Y < 0f && targetYDist > 0f)))
                {
                    if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X + currentTurnSpeed * 2f; //changed from 2
                    }
                    else
                    {
                        NPC.velocity.X = NPC.velocity.X - currentTurnSpeed * 2f; //changed from 2
                    }
                }
            }
            else
            {
                if (absoluteTargetX > absoluteTargetY)
                {
                    if (NPC.velocity.X < targetXDist)
                    {
                        NPC.velocity.X = NPC.velocity.X + currentTurnSpeed * 1.1f; //changed from 1.1
                    }
                    else if (NPC.velocity.X > targetXDist)
                    {
                        NPC.velocity.X = NPC.velocity.X - currentTurnSpeed * 1.1f; //changed from 1.1
                    }
                    if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)currentSpeed * 0.5)
                    {
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y = NPC.velocity.Y + currentTurnSpeed;
                        }
                        else
                        {
                            NPC.velocity.Y = NPC.velocity.Y - currentTurnSpeed;
                        }
                    }
                }
                else
                {
                    if (NPC.velocity.Y < targetYDist)
                    {
                        NPC.velocity.Y = NPC.velocity.Y + currentTurnSpeed * 1.1f;
                    }
                    else if (NPC.velocity.Y > targetYDist)
                    {
                        NPC.velocity.Y = NPC.velocity.Y - currentTurnSpeed * 1.1f;
                    }
                    if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)currentSpeed * 0.5)
                    {
                        if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X + currentTurnSpeed;
                        }
                        else
                        {
                            NPC.velocity.X = NPC.velocity.X - currentTurnSpeed;
                        }
                    }
                }
            }
            NPC.rotation = (float)System.Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + 1.57f;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode && spawnInfo.Player.Calamity().ZoneSunkenSea && spawnInfo.Water &&
                !NPC.AnyNPCs(ModContent.NPCType<SeaSerpent1>()) && !spawnInfo.Player.Calamity().clamity && !spawnInfo.PlayerSafe)
                return SpawnCondition.CaveJellyfish.Chance * 0.3f;

            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.Add(ModContent.ItemType<Serpentine>(), 4);

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Obsidian, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 10; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Obsidian, hit.HitDirection, -1f, 0, default, 1f);
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SeaSerpentGore1").Type, 1f);
                }
            }
        }
    }
}
