﻿using System;
using CalamityMod.BiomeManagers;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Placeables.Banners;
using CalamityMod.Items.Potions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace CalamityMod.NPCs.Abyss
{
    public class Cuttlefish : ModNPC
    {
        public static Asset<Texture2D> GlowTexture;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
            if (!Main.dedServ)
            {
                GlowTexture = ModContent.Request<Texture2D>(Texture + "Glow", AssetRequestMode.AsyncLoad);
            }
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.chaseable = false;
            NPC.damage = 34;
            NPC.width = 58;
            NPC.height = 30;
            NPC.defense = 8;
            NPC.lifeMax = 110;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.alpha = 150;
            NPC.value = Item.buyPrice(0, 0, 1, 0);
            NPC.HitSound = SoundID.NPCHit33;
            NPC.DeathSound = SoundID.NPCDeath28;
            NPC.knockBackResist = 0.3f;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CuttlefishBanner>();
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = true;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToWater = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<AbyssLayer2Biome>().Type };

            // Scale stats in Expert and Master
            CalamityGlobalNPC.AdjustExpertModeStatScaling(NPC);
            CalamityGlobalNPC.AdjustMasterModeStatScaling(NPC);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("Mods.CalamityMod.Bestiary.Cuttlefish")
            });
        }

        public override void AI()
        {
            NPC.spriteDirection = (NPC.direction > 0) ? 1 : -1;
            int alphaControl = 150;
            if (NPC.ai[2] == 0f)
            {
                NPC.alpha = alphaControl;
                NPC.TargetClosest(true);
                if (!Main.player[NPC.target].dead && (Main.player[NPC.target].Center - NPC.Center).Length() < Main.player[NPC.target].Calamity().GetAbyssAggro(160f) &&
                    Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
                {
                    NPC.ai[2] = -16f;
                }
                if (NPC.justHit)
                {
                    NPC.ai[2] = -16f;
                }
                if (NPC.collideX)
                {
                    NPC.velocity.X = NPC.velocity.X * -1f;
                    NPC.direction *= -1;
                }
                if (NPC.collideY)
                {
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
                NPC.velocity.X = NPC.velocity.X + NPC.direction * 0.02f;
                NPC.rotation = NPC.velocity.X * 0.4f;
                if (NPC.velocity.X < -1f || NPC.velocity.X > 1f)
                {
                    NPC.velocity.X = NPC.velocity.X * 0.95f;
                }
                if (NPC.ai[0] == -1f)
                {
                    NPC.velocity.Y = NPC.velocity.Y - 0.01f;
                    if (NPC.velocity.Y < -1f)
                    {
                        NPC.ai[0] = 1f;
                    }
                }
                else
                {
                    NPC.velocity.Y = NPC.velocity.Y + 0.01f;
                    if (NPC.velocity.Y > 1f)
                    {
                        NPC.ai[0] = -1f;
                    }
                }
                int npcTileX = (int)(NPC.position.X + (NPC.width / 2)) / 16;
                int npcTileY = (int)(NPC.position.Y + (NPC.height / 2)) / 16;
                if (Main.tile[npcTileX, npcTileY - 1].LiquidAmount > 128)
                {
                    if (Main.tile[npcTileX, npcTileY + 1].HasTile)
                    {
                        NPC.ai[0] = -1f;
                    }
                    else if (Main.tile[npcTileX, npcTileY + 2].HasTile)
                    {
                        NPC.ai[0] = -1f;
                    }
                }
                else
                {
                    NPC.ai[0] = 1f;
                }
                if (NPC.velocity.Y > 1.2 || NPC.velocity.Y < -1.2)
                {
                    NPC.velocity.Y = NPC.velocity.Y * 0.99f;
                }
                return;
            }
            if (NPC.ai[2] < 0f)
            {
                if (NPC.alpha > 0)
                {
                    NPC.alpha -= alphaControl / 16;
                    if (NPC.alpha < 0)
                    {
                        NPC.alpha = 0;
                    }
                }
                NPC.ai[2] += 1f;
                if (NPC.ai[2] == 0f)
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[2] = 1f;
                    NPC.velocity.X = NPC.direction * 2;
                }
                return;
            }
            if (NPC.ai[2] == 1f)
            {
                NPC.chaseable = true;
                if (NPC.direction == 0)
                {
                    NPC.TargetClosest(true);
                }
                if (NPC.wet || NPC.noTileCollide)
                {
                    bool canAttack = false;
                    NPC.TargetClosest(false);
                    if (Main.player[NPC.target].wet && !Main.player[NPC.target].dead)
                    {
                        canAttack = true;
                    }
                    if (!canAttack)
                    {
                        if (!Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                        {
                            NPC.noTileCollide = false;
                        }
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
                        if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
                        {
                            if (NPC.ai[3] > 0f && !Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                            {
                                NPC.ai[3] = 0f;
                                NPC.ai[1] = 0f;
                                NPC.netUpdate = true;
                            }
                        }
                        else if (NPC.ai[3] == 0f)
                        {
                            NPC.ai[1] += 1f;
                        }
                        if (NPC.ai[1] >= 150f)
                        {
                            NPC.ai[3] = 1f;
                            NPC.ai[1] = 0f;
                            NPC.netUpdate = true;
                        }
                        if (NPC.ai[3] == 0f)
                        {
                            NPC.alpha = 0;
                            NPC.noTileCollide = false;
                        }
                        else
                        {
                            NPC.alpha = 150;
                            NPC.noTileCollide = true;
                        }
                        NPC.TargetClosest(true);
                        NPC.velocity.X = NPC.velocity.X + (float)NPC.direction * 0.2f;
                        NPC.velocity.Y = NPC.velocity.Y + (float)NPC.directionY * 0.2f;
                        if (NPC.velocity.X > 9f)
                        {
                            NPC.velocity.X = 9f;
                        }
                        if (NPC.velocity.X < -9f)
                        {
                            NPC.velocity.X = -9f;
                        }
                        if (NPC.velocity.Y > 7f)
                        {
                            NPC.velocity.Y = 7f;
                        }
                        if (NPC.velocity.Y < -7f)
                        {
                            NPC.velocity.Y = -7f;
                        }
                    }
                    else
                    {
                        if (!Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                        {
                            NPC.noTileCollide = false;
                        }
                        NPC.velocity.X = NPC.velocity.X + (float)NPC.direction * 0.1f;
                        if (NPC.velocity.X < -1f || NPC.velocity.X > 1f)
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
                    int npcTileXAgain = (int)(NPC.position.X + (float)(NPC.width / 2)) / 16;
                    int npcTileYAgain = (int)(NPC.position.Y + (float)(NPC.height / 2)) / 16;
                    if (Main.tile[npcTileXAgain, npcTileYAgain - 1].LiquidAmount > 128)
                    {
                        if (Main.tile[npcTileXAgain, npcTileYAgain + 1].HasTile)
                        {
                            NPC.ai[0] = -1f;
                        }
                        else if (Main.tile[npcTileXAgain, npcTileYAgain + 2].HasTile)
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
                    NPC.velocity.Y = NPC.velocity.Y + 0.25f;
                    if (NPC.velocity.Y > 7f)
                    {
                        NPC.velocity.Y = 7f;
                    }
                    NPC.ai[0] = 1f;
                }
                NPC.rotation = NPC.velocity.Y * (float)NPC.direction * 0.1f;
                if ((double)NPC.rotation < -0.2)
                {
                    NPC.rotation = -0.2f;
                }
                if ((double)NPC.rotation > 0.2)
                {
                    NPC.rotation = 0.2f;
                    return;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (!NPC.wet && !NPC.noTileCollide && !NPC.IsABestiaryIconDummy)
            {
                NPC.frameCounter = 0.0;
                return;
            }
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!NPC.IsABestiaryIconDummy)
            {
                var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                Main.EntitySpriteDraw(GlowTexture.Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY + 4),
                NPC.frame, Color.White * 0.5f, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, effects, 0);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (hurtInfo.Damage > 0)
                target.AddBuff(BuffID.Darkness, 120, true);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.Calamity().ZoneAbyssLayer2 && spawnInfo.Water)
            {
                return SpawnCondition.CaveJellyfish.Chance * 0.6f;
            }
            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<AnechoicCoating>(), 2);
            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<InkBomb>(), 20, 10));
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 15; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Cuttlefish").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Cuttlefish2").Type, 1f);
                }
            }
        }
    }
}
