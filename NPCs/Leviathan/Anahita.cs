﻿using System;
using System.IO;
using CalamityMod.Events;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Projectiles.Boss;
using CalamityMod.UI.VanillaBossBars;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.NPCs.Leviathan
{
    [AutoloadBossHead]
    public class Anahita : ModNPC
    {
        public static readonly SoundStyle HitSound = new("CalamityMod/Sounds/NPCHit/AnahitaHit", 3);
        public static readonly SoundStyle DeathSound = new("CalamityMod/Sounds/NPCKilled/AnahitaDeath");

        private int biomeEnrageTimer = CalamityGlobalNPC.biomeEnrageTimerMax;
        private bool spawnedLevi = false;
        private bool forceChargeFrames = false;
        private int frameUsed = 0;
        public bool HasBegunSummoningLeviathan = false;
        public static Asset<Texture2D> ChargeTexture;
        public bool WaitingForLeviathan
        {
            get
            {
                if (Main.npc.IndexInRange(CalamityGlobalNPC.leviathan) && Main.npc[CalamityGlobalNPC.leviathan].life / (float)Main.npc[CalamityGlobalNPC.leviathan].lifeMax >= ((CalamityWorld.death || BossRushEvent.BossRushActive) ? 0.7f : 0.4f))
                    return true;

                return CalamityUtils.FindFirstProjectile(ModContent.ProjectileType<LeviathanSpawner>()) != -1;
            }
        }

        //IMPORTANT: Do NOT remove the empty space on the sprites.  This is intentional for framing.  The sprite is centered and hitbox is fine already.

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Scale = 0.5f
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
            if (!Main.dedServ)
            {
                ChargeTexture = ModContent.Request<Texture2D>("CalamityMod/NPCs/Leviathan/AnahitaStabbing", AssetRequestMode.AsyncLoad);
            }
        }

        public override void SetDefaults()
        {
            NPC.GetNPCDamage();
            NPC.npcSlots = 16f;
            NPC.width = 100;
            NPC.height = 100;
            NPC.defense = 20;
            NPC.DR_NERD(0.2f);
            NPC.LifeMaxNERB(35000, 42000, 260000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.boss = true;
            NPC.BossBar = ModContent.GetInstance<LeviathanAnahitaBossBar>();
            NPC.value = Item.buyPrice(0, 60, 0, 0);
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = HitSound;
            NPC.DeathSound = DeathSound;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToWater = false;

            if (Main.getGoodWorld)
                NPC.scale *= 0.8f;

            if (Main.zenithWorld)
                NPC.scale *= 4f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement("Mods.CalamityMod.Bestiary.Anahita")
            });
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(biomeEnrageTimer);
            writer.Write(spawnedLevi);
            writer.Write(forceChargeFrames);
            writer.Write(NPC.localAI[0]);
            writer.Write(NPC.localAI[2]);
            writer.Write(NPC.localAI[3]);
            writer.Write(frameUsed);
            writer.Write(NPC.dontTakeDamage);
            writer.Write(NPC.Calamity().newAI[0]);
            writer.Write(HasBegunSummoningLeviathan);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            biomeEnrageTimer = reader.ReadInt32();
            spawnedLevi = reader.ReadBoolean();
            forceChargeFrames = reader.ReadBoolean();
            NPC.localAI[0] = reader.ReadSingle();
            NPC.localAI[2] = reader.ReadSingle();
            NPC.localAI[3] = reader.ReadSingle();
            frameUsed = reader.ReadInt32();
            NPC.dontTakeDamage = reader.ReadBoolean();
            NPC.Calamity().newAI[0] = reader.ReadSingle();
            HasBegunSummoningLeviathan = reader.ReadBoolean();
        }

        public override void AI()
        {
            // whoAmI variable
            CalamityGlobalNPC.siren = NPC.whoAmI;

            // This dictates the Leviathan music scene
            if (HasBegunSummoningLeviathan && CalamityGlobalNPC.LeviAndAna == -1)
                CalamityGlobalNPC.LeviAndAna = NPC.whoAmI;

            // Set to false so she doesn't do it constantly
            NPC.Calamity().canBreakPlayerDefense = false;

            // Light
            Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0f, 0.5f, 0.3f);

            // Get a target
            if (NPC.target < 0 || NPC.target == Main.maxPlayers || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest();

            // Despawn safety, make sure to target another player if the current player target is too far away
            if (Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) > CalamityGlobalNPC.CatchUpDistance200Tiles)
                NPC.TargetClosest();

            // Check for Leviathan
            bool leviAlive = false;
            if (CalamityGlobalNPC.leviathan != -1)
                leviAlive = Main.npc[CalamityGlobalNPC.leviathan].active;

            // Variables
            Player player = Main.player[NPC.target];
            bool bossRush = BossRushEvent.BossRushActive;
            bool death = CalamityWorld.death || bossRush;
            bool revenge = CalamityWorld.revenge || bossRush;
            bool expertMode = Main.expertMode || bossRush;
            bool notOcean = player.position.Y < 800f || player.position.Y > Main.worldSurface * 16.0 || (player.position.X > 6400f && player.position.X < (Main.maxTilesX * 16 - 6400));

            // Enrage
            if (notOcean && !bossRush)
            {
                if (biomeEnrageTimer > 0)
                    biomeEnrageTimer--;
            }
            else
                biomeEnrageTimer = CalamityGlobalNPC.biomeEnrageTimerMax;

            bool biomeEnraged = biomeEnrageTimer <= 0 || bossRush;

            float enrageScale = bossRush ? 0.5f : 0f;
            if (biomeEnraged)
            {
                NPC.Calamity().CurrentlyEnraged = !bossRush;
                enrageScale += 1.5f;
            }

            float lifeRatio = NPC.life / (float)NPC.lifeMax;

            float bubbleVelocity = death ? 9f : revenge ? 7f : expertMode ? 6f : 5f;
            bubbleVelocity += 4f * enrageScale;
            if (!leviAlive)
                bubbleVelocity += 2f * (1f - lifeRatio);
            if (Main.getGoodWorld)
                bubbleVelocity *= 1.15f;
            if (CalamityWorld.LegendaryMode && CalamityWorld.revenge)
                bubbleVelocity *= 2f;

            // Phases
            bool phase2 = lifeRatio < 0.7f;
            bool phase3 = lifeRatio < 0.4f;
            bool phase4 = lifeRatio < (death ? 0.4f : 0.2f);

            // Spawn Leviathan and change music
            if (phase3 || death)
            {
                if (!HasBegunSummoningLeviathan && !Main.zenithWorld)
                {
                    // Avoid cheap bullshit
                    NPC.damage = 0;

                    // Use charging frames.
                    NPC.ai[0] = 3f;

                    // Look towards the ocean.
                    NPC.direction = (NPC.Center.X < Main.maxTilesX * 8f).ToDirectionInt();

                    // Prevent Anahita from leaving the world when doing her dive.
                    // If she leaves the world entity summons will fail and Levi will simply not spawn.
                    NPC.position.X = MathHelper.Clamp(NPC.position.X, 150f, Main.maxTilesX * 16f - 150f);

                    if (NPC.alpha <= 0)
                    {
                        float moveDirection = 1f;
                        if (Math.Abs(NPC.Center.X - Main.maxTilesX * 16f) > Math.Abs(NPC.Center.X))
                            moveDirection = -1f;

                        NPC.velocity.X = moveDirection * 6f;
                        NPC.spriteDirection = (int)-moveDirection;
                        NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y + 0.2f, -3f, 16f);
                    }

                    float idealRotation = NPC.velocity.ToRotation();
                    if (NPC.spriteDirection == 1)
                        idealRotation += MathHelper.Pi;

                    NPC.rotation = NPC.rotation.AngleTowards(idealRotation, 0.08f);

                    if (bossRush || Collision.WetCollision(NPC.position, NPC.width, NPC.height) || NPC.position.Y > (Main.worldSurface - 125f) * 16f)
                    {
                        int oldAlpha = NPC.alpha;
                        NPC.alpha = Utils.Clamp(NPC.alpha + 9, 0, 255);
                        if (NPC.alpha >= 255 && oldAlpha < 255)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<LeviathanSpawner>(), 0, 0f);

                            HasBegunSummoningLeviathan = true;
                            NPC.netUpdate = true;
                        }

                        NPC.velocity *= 0.9f;
                    }
                    else
                    {
                        NPC.alpha -= 5;
                        if (NPC.alpha < 0)
                            NPC.alpha = 0;
                    }

                    NPC.dontTakeDamage = true;
                    return;
                }
            }

            // Ice Shield
            if (NPC.localAI[2] < 3f)
            {
                if (NPC.ai[3] == 0f && NPC.localAI[1] == 0f && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int iceShieldSpawn = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AnahitasIceShield>(), NPC.whoAmI);
                    NPC.ai[3] = iceShieldSpawn + 1;
                    NPC.localAI[1] = -1f;
                    NPC.localAI[2] += 1f;
                    NPC.netUpdate = true;
                    Main.npc[iceShieldSpawn].ai[0] = NPC.whoAmI;
                    Main.npc[iceShieldSpawn].netUpdate = true;
                }

                int iceShieldCheck = (int)NPC.ai[3] - 1;
                if (iceShieldCheck != -1 && Main.npc[iceShieldCheck].active && Main.npc[iceShieldCheck].type == ModContent.NPCType<AnahitasIceShield>())
                    NPC.dontTakeDamage = true;
                else
                {
                    NPC.dontTakeDamage = false;
                    NPC.ai[3] = 0f;

                    if (NPC.localAI[1] == -1f)
                        NPC.localAI[1] = 1f;
                    else
                    {
                        switch ((int)NPC.localAI[2])
                        {
                            case 1:
                                if (phase2)
                                    NPC.localAI[1] = 0f;
                                break;
                            case 2:
                                if (phase3)
                                    NPC.localAI[1] = 0f;
                                break;
                        }
                    }
                }
            }
            else
            {
                NPC.dontTakeDamage = false;

                int iceShieldCheck = (int)NPC.ai[3] - 1;
                if (iceShieldCheck != -1 && Main.npc[iceShieldCheck].active && Main.npc[iceShieldCheck].type == ModContent.NPCType<AnahitasIceShield>())
                    NPC.dontTakeDamage = true;
            }

            // Hover near the target, invisible if the Leviathan is present and not sufficiently injured.
            if ((phase3 || death) && WaitingForLeviathan && !Main.zenithWorld)
            {
                // Avoid cheap bullshit
                NPC.damage = 0;

                ChargeRotation(player);
                ChargeLocation(player, false, true);

                if (NPC.alpha < 255)
                    NPC.alpha += 3;

                if (NPC.alpha > 255)
                {
                    NPC.alpha = 255;
                }
                else if (NPC.alpha < 255)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DungeonWater, 0f, 0f, 100, default, 2f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].noLight = true;
                    }
                }

                NPC.dontTakeDamage = true;

                if (NPC.ai[0] != -1f)
                {
                    NPC.ai[0] = -1f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.localAI[0] = 0f;
                    NPC.netUpdate = true;
                }

                return;
            }

            NPC.alpha -= 5;
            if (NPC.alpha < 0)
                NPC.alpha = 0;

            // Play sound
            float extrapitch = Main.zenithWorld ? -0.5f : 0;
            if (Main.rand.NextBool(300))
                SoundEngine.PlaySound(SoundID.Zombie35 with { Pitch = SoundID.Zombie35.Pitch + extrapitch }, NPC.Center);

            // Time left
            if (NPC.timeLeft < 1800)
                NPC.timeLeft = 1800;

            // Despawn
            if (!player.active || player.dead || Vector2.Distance(player.Center, NPC.Center) > 5600f)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead || Vector2.Distance(player.Center, NPC.Center) > 5600f)
                {
                    NPC.rotation = NPC.velocity.X * 0.02f;

                    if (NPC.velocity.Y < -3f)
                        NPC.velocity.Y = -3f;
                    NPC.velocity.Y += 0.2f;
                    if (NPC.velocity.Y > 16f)
                        NPC.velocity.Y = 16f;

                    if (NPC.position.Y > Main.worldSurface * 16.0)
                    {
                        for (int x = 0; x < Main.maxNPCs; x++)
                        {
                            if (Main.npc[x].type == ModContent.NPCType<Leviathan>())
                            {
                                Main.npc[x].active = false;
                                Main.npc[x].netUpdate = true;
                            }
                        }
                        NPC.active = false;
                        NPC.netUpdate = true;
                    }

                    if (NPC.ai[0] != -1f)
                    {
                        NPC.ai[0] = -1f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                        NPC.localAI[0] = 0f;
                        NPC.netUpdate = true;
                    }

                    return;
                }
            }

            // Rotation when charging
            if (NPC.ai[0] > 2f)
                ChargeRotation(player);

            // Phase switch
            if (NPC.ai[0] == -1f)
            {
                int random = ((phase2 && expertMode && !leviAlive) || phase4 || death) ? 4 : 3;
                int nextAttack;
                do nextAttack = Main.rand.Next(random);
                while (nextAttack == NPC.ai[1] || nextAttack == 1);

                NPC.ai[0] = nextAttack;

                if (NPC.ai[0] != 3f)
                {
                    forceChargeFrames = false;
                    float playerLocation = NPC.Center.X - player.Center.X;
                    NPC.direction = playerLocation < 0f ? 1 : -1;
                    NPC.spriteDirection = NPC.direction;
                }
                else
                    ChargeRotation(player);

                NPC.TargetClosest();
                NPC.ai[1] = 0f;
                NPC.ai[2] = 0f;
            }

            // Get in position for bubble spawn
            else if (NPC.ai[0] == 0f)
            {
                // Avoid cheap bullshit
                NPC.damage = 0;

                NPC.rotation = NPC.velocity.X * 0.02f;
                NPC.spriteDirection = NPC.direction;

                Vector2 anahitaPos = NPC.Center;
                float playerXDist = player.position.X + (player.width / 2) - anahitaPos.X;
                float playerYDist = player.position.Y + (player.height / 2) - 200f * NPC.scale - anahitaPos.Y;
                float playerDistance = (float)Math.Sqrt(playerXDist * playerXDist + playerYDist * playerYDist);

                NPC.Calamity().newAI[0] += 1f;
                if (playerDistance < 600f * NPC.scale || NPC.Calamity().newAI[0] >= 180f)
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[1] = 0f;
                    NPC.Calamity().newAI[0] = 0f;
                    NPC.netUpdate = true;
                    return;
                }

                float maxVelocityY = death ? 3f : 4f;
                float maxVelocityX = death ? 7f : 8f;
                maxVelocityY -= enrageScale;
                maxVelocityX -= 2f * enrageScale;

                if (NPC.position.Y > player.position.Y - 350f * NPC.scale)
                {
                    if (NPC.velocity.Y > 0f)
                        NPC.velocity.Y *= 0.98f;
                    NPC.velocity.Y -= death ? 0.12f : 0.1f;
                    if (NPC.velocity.Y > maxVelocityY)
                        NPC.velocity.Y = maxVelocityY;
                }
                else if (NPC.position.Y < player.position.Y - 450f * NPC.scale)
                {
                    if (NPC.velocity.Y < 0f)
                        NPC.velocity.Y *= 0.98f;
                    NPC.velocity.Y += death ? 0.12f : 0.1f;
                    if (NPC.velocity.Y < -maxVelocityY)
                        NPC.velocity.Y = -maxVelocityY;
                }

                if (NPC.position.X + (NPC.width / 2) > player.position.X + (player.width / 2) + 100f * NPC.scale)
                {
                    if (NPC.velocity.X > 0f)
                        NPC.velocity.X *= 0.98f;
                    NPC.velocity.X -= death ? 0.12f : 0.1f;
                    if (NPC.velocity.X > maxVelocityX)
                        NPC.velocity.X = maxVelocityX;
                }

                if (NPC.position.X + (NPC.width / 2) < player.position.X + (player.width / 2) - 100f * NPC.scale)
                {
                    if (NPC.velocity.X < 0f)
                        NPC.velocity.X *= 0.98f;
                    NPC.velocity.X += death ? 0.12f : 0.1f;
                    if (NPC.velocity.X < -maxVelocityX)
                        NPC.velocity.X = -maxVelocityX;
                }
            }

            // Bubble spawn
            else if (NPC.ai[0] == 1f)
            {
                // Avoid cheap bullshit
                NPC.damage = 0;

                NPC.rotation = NPC.velocity.X * 0.02f;
                Vector2 bubbleSpawnPos = new Vector2(NPC.position.X + (NPC.width / 2) + (15 * NPC.direction * NPC.scale), NPC.position.Y + 30 * NPC.scale);
                Vector2 restingPos = NPC.Center;
                float restingPlayerXDist = player.position.X + (player.width / 2) - restingPos.X;
                float restingPlayerYDist = player.position.Y + (player.height / 2) - restingPos.Y;
                float restingPlayerDistance = (float)Math.Sqrt(restingPlayerXDist * restingPlayerXDist + restingPlayerYDist * restingPlayerYDist);

                NPC.ai[1] += 1f;
                int activePlayerAmt = 0;
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && !Main.player[i].dead && (NPC.Center - Main.player[i].Center).Length() < 1000f)
                        activePlayerAmt++;
                }
                NPC.ai[1] += activePlayerAmt / 2;

                bool spawnedBubble = false;
                float bubbleDelay = 20f;
                if (!leviAlive || phase4)
                    bubbleDelay -= death ? 15f * (1f - lifeRatio) : 12f * (1f - lifeRatio);

                if (NPC.ai[1] > bubbleDelay)
                {
                    NPC.ai[1] = 0f;
                    NPC.ai[2] += 1f;
                    spawnedBubble = true;
                }

                if (Collision.CanHit(bubbleSpawnPos, 1, 1, player.position, player.width, player.height) && spawnedBubble)
                {
                    SoundEngine.PlaySound(SoundID.Item85, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int spawn = NPC.NewNPC(NPC.GetSource_FromAI(), (int)bubbleSpawnPos.X, (int)bubbleSpawnPos.Y, NPCID.DetonatingBubble);
                        Main.npc[spawn].target = NPC.target;
                        Main.npc[spawn].velocity = player.Center - bubbleSpawnPos;
                        Main.npc[spawn].velocity.Normalize();
                        Main.npc[spawn].velocity *= bubbleVelocity;
                        Main.npc[spawn].netUpdate = true;
                        Main.npc[spawn].ai[3] = Main.rand.Next(80, 121) / 100f;
                    }
                }

                if (restingPlayerDistance > 600f * NPC.scale)
                {
                    float maxVelocityY = death ? 3f : 4f;
                    float maxVelocityX = death ? 7f : 8f;
                    maxVelocityY -= enrageScale;
                    maxVelocityX -= 2f * enrageScale;

                    if (NPC.position.Y > player.position.Y - 350f * NPC.scale)
                    {
                        if (NPC.velocity.Y > 0f)
                            NPC.velocity.Y *= 0.98f;
                        NPC.velocity.Y -= death ? 0.12f : 0.1f;
                        if (NPC.velocity.Y > maxVelocityY)
                            NPC.velocity.Y = maxVelocityY;
                    }
                    else if (NPC.position.Y < player.position.Y - 450f * NPC.scale)
                    {
                        if (NPC.velocity.Y < 0f)
                            NPC.velocity.Y *= 0.98f;
                        NPC.velocity.Y += death ? 0.12f : 0.1f;
                        if (NPC.velocity.Y < -maxVelocityY)
                            NPC.velocity.Y = -maxVelocityY;
                    }

                    if (NPC.position.X + (NPC.width / 2) > player.position.X + (player.width / 2) + 100f * NPC.scale)
                    {
                        if (NPC.velocity.X > 0f)
                            NPC.velocity.X *= 0.98f;
                        NPC.velocity.X -= death ? 0.12f : 0.1f;
                        if (NPC.velocity.X > maxVelocityX)
                            NPC.velocity.X = maxVelocityX;
                    }

                    if (NPC.position.X + (NPC.width / 2) < player.position.X + (player.width / 2) - 100f * NPC.scale)
                    {
                        if (NPC.velocity.X < 0f)
                            NPC.velocity.X *= 0.98f;
                        NPC.velocity.X += death ? 0.12f : 0.1f;
                        if (NPC.velocity.X < -maxVelocityX)
                            NPC.velocity.X = -maxVelocityX;
                    }
                }
                else
                    NPC.velocity *= 0.9f;

                NPC.spriteDirection = NPC.direction;

                float maxBubbleSpawn = death ? 3f : 4f;
                if (NPC.ai[2] > maxBubbleSpawn)
                {
                    NPC.ai[0] = -1f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.netUpdate = true;
                }
            }

            // Fly near target and summon projectiles
            else if (NPC.ai[0] == 2f)
            {
                // Avoid cheap bullshit
                NPC.damage = 0;

                NPC.rotation = NPC.velocity.X * 0.02f;

                float basey = Main.zenithWorld ? -100 : -350;
                Vector2 targetVector = player.Center + new Vector2(0f, basey) * NPC.scale;
                float velocity = death ? 13.5f : 12f;
                velocity += 6f * enrageScale;

                if (Main.getGoodWorld)
                    velocity *= 1.15f;

                Vector2 chargeSetupLocation = Vector2.Normalize(targetVector - NPC.Center - NPC.velocity) * velocity;
                float acceleration = death ? 0.28f : 0.25f;
                acceleration += 0.2f * enrageScale;

                if (Main.getGoodWorld)
                    acceleration *= 1.15f;

                if (Math.Abs(NPC.Center.Y - targetVector.Y) > 50f * NPC.scale || Math.Abs(NPC.Center.X - player.Center.X) > 350f * NPC.scale)
                    NPC.SimpleFlyMovement(chargeSetupLocation, acceleration);

                NPC.ai[1] += 1f;

                float divisor = 140f;
                divisor -= (int)(30f * enrageScale);
                if (!leviAlive || phase4)
                    divisor -= (float)Math.Ceiling(50f * (1f - lifeRatio));

                bool shootProjectiles = NPC.ai[1] % divisor == 0f;
                if (Main.netMode != NetmodeID.MultiplayerClient && shootProjectiles)
                {
                    float projectileVelocity = expertMode ? 3f : 2f;
                    projectileVelocity += enrageScale;
                    if (!leviAlive || phase4)
                        projectileVelocity += death ? 3f * (1f - lifeRatio) : 2f * (1f - lifeRatio);

                    int totalProjectiles = 8;
                    int projectileDistance = 600;
                    int type = ModContent.ProjectileType<WaterSpear>();
                    if (CalamityWorld.LegendaryMode && CalamityWorld.revenge)
                    {
                        float radians = MathHelper.TwoPi / totalProjectiles;
                        int damage = NPC.GetProjectileDamage(type);
                        for (int i = 0; i < totalProjectiles; i++)
                        {
                            switch (Main.rand.Next(3))
                            {
                                case 0:
                                    SoundEngine.PlaySound(SoundID.Item21, player.Center);
                                    break;

                                case 1:
                                    type = ModContent.ProjectileType<FrostMist>();
                                    SoundEngine.PlaySound(SoundID.Item30, player.Center);
                                    break;

                                case 2:
                                    type = ModContent.ProjectileType<SirenSong>();
                                    float soundPitch = (Main.rand.NextFloat() - 0.5f) * 0.5f;
                                    Main.musicPitch = soundPitch;
                                    SoundEngine.PlaySound(SoundID.Item26, player.Center);
                                    break;
                            }

                            Vector2 spawnVector = player.Center + Vector2.Normalize(new Vector2(0f, -projectileVelocity).RotatedBy(radians * i)) * projectileDistance;
                            Vector2 projVelocity = Vector2.Normalize(player.Center - spawnVector) * projectileVelocity;
                            int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnVector, projVelocity, type, damage, 0f, Main.myPlayer);
                            Main.projectile[proj].netUpdate = true;
                        }
                    }
                    else
                    {
                        switch ((int)NPC.localAI[3])
                        {
                            case 0:
                                SoundEngine.PlaySound(SoundID.Item21, player.Center);
                                break;

                            case 1:
                                totalProjectiles = 3;
                                type = ModContent.ProjectileType<FrostMist>();
                                SoundEngine.PlaySound(SoundID.Item30, player.Center);
                                break;

                            case 2:
                                totalProjectiles = 6;
                                type = ModContent.ProjectileType<SirenSong>();
                                float soundPitch = (Main.rand.NextFloat() - 0.5f) * 0.5f;
                                Main.musicPitch = soundPitch;
                                SoundEngine.PlaySound(SoundID.Item26, player.Center);
                                break;
                        }
                        NPC.localAI[3] += 1f;
                        if (NPC.localAI[3] > 2f)
                            NPC.localAI[3] = 0f;

                        if ((phase2 && !leviAlive) || phase4)
                            totalProjectiles += totalProjectiles / 2;

                        float radians = MathHelper.TwoPi / totalProjectiles;
                        int damage = NPC.GetProjectileDamage(type);
                        for (int i = 0; i < totalProjectiles; i++)
                        {
                            Vector2 spawnVector = player.Center + Vector2.Normalize(new Vector2(0f, -projectileVelocity).RotatedBy(radians * i)) * projectileDistance;
                            Vector2 projVelocity = Vector2.Normalize(player.Center - spawnVector) * projectileVelocity;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnVector, projVelocity, type, damage, 0f, Main.myPlayer);
                        }
                    }
                }

                if (Math.Abs(NPC.Center.X - player.Center.X) > 10f)
                {
                    float playerLocation = NPC.Center.X - player.Center.X;
                    NPC.direction = playerLocation < 0f ? 1 : -1;
                    NPC.spriteDirection = NPC.direction;
                }

                float phaseTimer = 300f;
                phaseTimer -= 50f * enrageScale;
                if (!leviAlive || phase4)
                    phaseTimer -= 150f * (1f - lifeRatio);

                if (NPC.ai[1] >= phaseTimer)
                {
                    NPC.ai[0] = -1f;
                    NPC.ai[1] = 2f;
                    NPC.netUpdate = true;
                }
            }

            // Set up charge
            else if (NPC.ai[0] == 3f)
            {
                // Avoid cheap bullshit
                NPC.damage = 0;

                ChargeLocation(player, leviAlive && !phase4, revenge);

                NPC.ai[1] += 1f;

                if (NPC.ai[1] >= (revenge ? 45f : 60f))
                {
                    forceChargeFrames = true;
                    float aiInterval = (leviAlive && !phase4) ? 2f : 3f;
                    NPC.ai[0] = (NPC.ai[2] >= aiInterval) ? -1f : 4f;
                    NPC.ai[1] = (NPC.ai[2] >= aiInterval) ? 3f : 0f;
                    NPC.localAI[0] = 0f;

                    // Velocity and rotation
                    float chargeVelocity = (leviAlive && !phase4) ? 21f : 26f;
                    chargeVelocity += 8f * enrageScale;

                    if (revenge)
                        chargeVelocity += 2f + (death ? 6f * (1f - lifeRatio) : 4f * (1f - lifeRatio));

                    if (Main.getGoodWorld)
                        chargeVelocity *= 1.15f;

                    NPC.velocity = Vector2.Normalize(player.Center - NPC.Center) * chargeVelocity;
                    NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);

                    // Direction
                    int chargeDirection = Math.Sign(player.Center.X - NPC.Center.X);
                    if (chargeDirection != 0)
                    {
                        NPC.direction = chargeDirection;

                        if (NPC.spriteDirection == 1)
                            NPC.rotation += MathHelper.Pi;

                        NPC.spriteDirection = -NPC.direction;
                    }
                }
            }

            // Charge
            else if (NPC.ai[0] == 4f)
            {
                NPC.Calamity().canBreakPlayerDefense = true;
                NPC.damage = (int)Math.Round(NPC.defDamage * 1.5);

                if (CalamityWorld.LegendaryMode && CalamityWorld.revenge && NPC.ai[1] % 5f == 0f)
                {
                    SoundEngine.PlaySound(SoundID.Item85, NPC.Center);
                    Vector2 bubbleSpawnPos = new Vector2(NPC.position.X + (NPC.width / 2) + (15 * NPC.direction * NPC.scale), NPC.position.Y + 30 * NPC.scale);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int spawn = NPC.NewNPC(NPC.GetSource_FromAI(), (int)bubbleSpawnPos.X, (int)bubbleSpawnPos.Y, NPCID.DetonatingBubble);
                        Main.npc[spawn].target = NPC.target;
                        Main.npc[spawn].velocity = player.Center - bubbleSpawnPos;
                        Main.npc[spawn].velocity.Normalize();
                        Main.npc[spawn].velocity *= bubbleVelocity;
                        Main.npc[spawn].netUpdate = true;
                        Main.npc[spawn].ai[3] = Main.rand.Next(80, 121) / 100f;
                    }
                }

                // Spawn dust
                int dustAmt = 7;
                for (int j = 0; j < dustAmt; j++)
                {
                    Vector2 arg_E1C_0 = (Vector2.Normalize(NPC.velocity) * new Vector2((NPC.width + 50) / 2f, NPC.height) * 0.75f).RotatedBy((j - (dustAmt / 2 - 1)) * MathHelper.Pi / dustAmt) + NPC.Center;
                    Vector2 vector4 = ((float)(Main.rand.NextDouble() * MathHelper.Pi) - MathHelper.PiOver2).ToRotationVector2() * Main.rand.Next(3, 8);
                    int waterDust = Dust.NewDust(arg_E1C_0 + vector4, 0, 0, DustID.DungeonWater, vector4.X * 2f, vector4.Y * 2f, 100, default, 1.4f);
                    Main.dust[waterDust].noGravity = true;
                    Main.dust[waterDust].noLight = true;
                    Main.dust[waterDust].velocity /= 4f;
                    Main.dust[waterDust].velocity -= NPC.velocity;
                }

                NPC.ai[1] += 1f;
                if (NPC.ai[1] >= ((leviAlive && !phase4) ? 50f : 35f))
                {
                    NPC.ai[0] = 3f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] += 1f;
                    NPC.netUpdate = true;
                }
            }
        }

        // Rotation when charging
        private void ChargeRotation(Player player)
        {
            float playerDirection = (float)Math.Atan2(player.Center.Y - NPC.Center.Y, player.Center.X - NPC.Center.X);
            if (NPC.spriteDirection == 1)
                playerDirection += MathHelper.Pi;
            if (playerDirection < 0f)
                playerDirection += MathHelper.TwoPi;
            if (playerDirection > MathHelper.TwoPi)
                playerDirection -= MathHelper.TwoPi;

            float rotationSpeed = 0.04f;
            if (NPC.ai[0] == 4f)
                rotationSpeed = 0f;

            if (rotationSpeed != 0f)
                NPC.rotation = NPC.rotation.AngleTowards(playerDirection, rotationSpeed);
        }

        // Move to charge location
        private void ChargeLocation(Player player, bool leviAlive, bool revenge)
        {
            float distance = (leviAlive ? 600f : 500f) * NPC.scale;

            // Velocity
            if (NPC.localAI[0] == 0f)
                NPC.localAI[0] = (int)distance * Math.Sign((NPC.Center - player.Center).X);

            Vector2 chargeSetupLocation = Vector2.Normalize(player.Center + new Vector2(NPC.localAI[0], -distance) - NPC.Center - NPC.velocity) * 12f;
            float acceleration = revenge ? 0.75f : 0.5f;
            if (Main.getGoodWorld)
                acceleration *= 1.15f;

            NPC.SimpleFlyMovement(chargeSetupLocation, acceleration);

            // Rotation
            int chargeDirection = Math.Sign(player.Center.X - NPC.Center.X);
            if (chargeDirection != 0)
            {
                if (NPC.ai[1] == 0f && chargeDirection != NPC.direction)
                    NPC.rotation += MathHelper.Pi;

                NPC.direction = chargeDirection;

                if (NPC.spriteDirection != -NPC.direction)
                    NPC.rotation += MathHelper.Pi;

                NPC.spriteDirection = -NPC.direction;
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Flare_Blue, hit.HitDirection, -1f, 0, default, 1f);

            if (NPC.life <= 0)
            {
                for (int k = 0; k < 50; k++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Flare_Blue, hit.HitDirection, -1f, 0, default, 1f);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            switch (frameUsed)
            {
                case 0:
                    texture = TextureAssets.Npc[NPC.type].Value;
                    break;
                case 1:
                    texture = ChargeTexture.Value;
                    break;
            }

            bool charging = NPC.ai[0] > 2f || forceChargeFrames;
            int height = texture.Height / Main.npcFrameCount[NPC.type];
            int width = texture.Width;
            SpriteEffects spriteEffects = charging ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            if (NPC.spriteDirection == -1)
                spriteEffects = charging ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY), NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2((float)width / 2f, (float)height / 2f), NPC.scale, spriteEffects, 0f);
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[0] > 2f || forceChargeFrames)
                frameUsed = 1;
            else
                frameUsed = 0;

            int timeBetweenFrames = 8;
            NPC.frameCounter++;
            if (NPC.frameCounter > timeBetweenFrames * Main.npcFrameCount[NPC.type])
                NPC.frameCounter = 0;

            NPC.frame.Y = frameHeight * (int)(NPC.frameCounter / timeBetweenFrames);
            if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
                NPC.frame.Y = 0;

            //100x1140
            //200x636
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void OnKill()
        {
            if (Leviathan.LastAnLStanding())
                Leviathan.RealOnKill(NPC);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            Leviathan.DefineAnahitaLeviathanLoot(npcLoot);

            // Trophy (always directly from boss, never in bag)
            npcLoot.Add(ModContent.ItemType<AnahitaTrophy>(), 10);
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance * bossAdjustment);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }
    }
}
