﻿using System.IO;
using CalamityMod.BiomeManagers;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Banners;
using CalamityMod.Items.Weapons.Magic;
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
    public class MirageJelly : ModNPC
    {
        private bool teleporting = false;
        private bool rephasing = false;
        private bool hasBeenHit = false;
        public static Asset<Texture2D> GlowTexture;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 7;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitPositionYOverride = 20
            };
            value.Position.Y += 30f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
            if (!Main.dedServ)
            {
                GlowTexture = ModContent.Request<Texture2D>(Texture + "Glow", AssetRequestMode.AsyncLoad);
            }
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.damage = 100;
            NPC.width = 78;
            NPC.height = 170;
            NPC.defense = 10;
            NPC.lifeMax = 6000;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 25, 0);
            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<MirageJellyBanner>();
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToWater = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<AbyssLayer3Biome>().Type };

            // Scale stats in Expert and Master
            CalamityGlobalNPC.AdjustExpertModeStatScaling(NPC);
            CalamityGlobalNPC.AdjustMasterModeStatScaling(NPC);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("Mods.CalamityMod.Bestiary.MirageJelly")
            });
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(hasBeenHit);
            writer.Write(teleporting);
            writer.Write(rephasing);
            writer.Write(NPC.chaseable);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            hasBeenHit = reader.ReadBoolean();
            teleporting = reader.ReadBoolean();
            rephasing = reader.ReadBoolean();
            NPC.chaseable = reader.ReadBoolean();
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            NPC.velocity *= 0.985f;
            if (NPC.velocity.Y > -0.3f)
            {
                NPC.velocity.Y = -3f;
            }
            if (NPC.justHit)
            {
                if (Main.rand.NextBool(10))
                {
                    teleporting = true;
                }
                hasBeenHit = true;
            }
            if (NPC.ai[0] == 0f)
            {
                NPC.chaseable = true;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (teleporting)
                    {
                        teleporting = false;
                        NPC.TargetClosest(true);
                        int teleportTries = 0;
                        int teleportTileX;
                        int teleportTileY;
                        while (true)
                        {
                            teleportTries++;
                            teleportTileX = (int)player.Center.X / 16;
                            teleportTileY = (int)player.Center.Y / 16;

                            int min = 6;
                            int max = 9;

                            if (Main.rand.NextBool())
                                teleportTileX += Main.rand.Next(min, max);
                            else
                                teleportTileX -= Main.rand.Next(min, max);

                            min = 11;
                            max = 26;

                            teleportTileY += Main.rand.Next(min, max);

                            if (!WorldGen.SolidTile(teleportTileX, teleportTileY) && Collision.CanHit(new Vector2((float)(teleportTileX * 16), (float)(teleportTileY * 16)), 1, 1, player.position, player.width, player.height) &&
                                Main.tile[teleportTileX, teleportTileY].LiquidAmount > 204)
                            {
                                break;
                            }
                            if (teleportTries > 100)
                            {
                                goto Block;
                            }
                        }
                        NPC.ai[0] = 1f;
                        NPC.ai[1] = (float)teleportTileX;
                        NPC.ai[2] = (float)teleportTileY;
                        NPC.netUpdate = true;
Block:
                        ;
                    }
                }
            }
            else if (NPC.ai[0] == 1f)
            {
                NPC.damage = 0;
                NPC.chaseable = false;
                NPC.alpha += 5;
                if (NPC.alpha >= 255)
                {
                    NPC.alpha = 255;
                    NPC.position.X = NPC.ai[1] * 16f - (float)(NPC.width / 2);
                    NPC.position.Y = NPC.ai[2] * 16f - (float)(NPC.height / 2);
                    NPC.ai[0] = 2f;
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 2f)
            {
                NPC.alpha -= 5;
                if (NPC.alpha <= 0)
                {
                    NPC.damage = Main.expertMode ? 200 : 100;
                    NPC.chaseable = true;
                    NPC.alpha = 0;
                    NPC.ai[0] = 0f;
                    NPC.netUpdate = true;
                }
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

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!NPC.IsABestiaryIconDummy)
            {
                var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                Main.EntitySpriteDraw(GlowTexture.Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY + 4),
                NPC.frame, Color.White * 0.5f, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, effects, 0);
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += hasBeenHit ? 0.15f : 0.1f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.Calamity().ZoneAbyssLayer3 && spawnInfo.Water)
            {
                return Main.remixWorld ? 5.4f : SpawnCondition.CaveJellyfish.Chance * 0.6f;
            }
            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.AddIf(() => NPC.downedBoss3, ModContent.ItemType<AbyssShocker>(), 10);

            var postLevi = npcLoot.DefineConditionalDropSet(DropHelper.PostLevi());
            postLevi.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<DepthCells>(), 2, 5, 7, 10, 14));

            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<LifeJelly>(), 7, 5));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<CleansingJelly>(), 7, 5));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<VitalJelly>(), 7, 5));
            npcLoot.Add(ItemID.JellyfishNecklace, 10);
        }

        // Can only hit the target if they're touching the tentacles
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            Vector2 npcCenter = NPC.Center;
            Rectangle tentacleHitbox = new Rectangle((int)(npcCenter.X - (NPC.width / 4f)), (int)npcCenter.Y, NPC.width / 2, NPC.height / 2);

            Rectangle targetHitbox = target.Hitbox;
            bool insideTentacleHitbox = targetHitbox.Intersects(tentacleHitbox);

            return insideTentacleHitbox;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (hurtInfo.Damage > 0)
            {
                target.AddBuff(BuffID.Venom, 240, true);
                target.AddBuff(BuffID.Electrified, 120, true);
            }
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
            }
        }
    }
}
