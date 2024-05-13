﻿using System;
using System.IO;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Events;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Sounds;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.NPCs.AstrumAureus
{
    public class AureusSpawn : ModNPC
    {
        public static Asset<Texture2D> GlowTexture;
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            if (!Main.dedServ)
            {
                GlowTexture = ModContent.Request<Texture2D>(Texture + "Glow", AssetRequestMode.AsyncLoad);
            }
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.GetNPCDamage();
            NPC.width = 90;
            NPC.height = 60;
            NPC.Opacity = 0f;
            NPC.defense = 10;
            NPC.lifeMax = 5000;
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.dontTakeDamage);
            for (int i = 0; i < 4; i++)
                writer.Write(NPC.Calamity().newAI[i]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.dontTakeDamage = reader.ReadBoolean();
            for (int i = 0; i < 4; i++)
                NPC.Calamity().newAI[i] = reader.ReadSingle();
        }

        public override void AI()
        {
            // Force despawn if Astrum Aureus isn't active
            if (CalamityGlobalNPC.astrumAureus < 0 || !Main.npc[CalamityGlobalNPC.astrumAureus].active)
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.checkDead();
                NPC.active = false;
                NPC.netUpdate = true;
                return;
            }

            Lighting.AddLight((int)((NPC.position.X + (NPC.width / 2)) / 16f), (int)((NPC.position.Y + (NPC.height / 2)) / 16f), 0.6f * NPC.Opacity * NPC.scale, 0.25f * NPC.Opacity * NPC.scale, 0f);

            NPC.rotation = Math.Abs(NPC.velocity.X) * NPC.direction * 0.04f;

            NPC.spriteDirection = NPC.direction;

            if (NPC.Opacity < 1f)
            {
                NPC.Opacity += 0.01f;

                if (NPC.Opacity > 0.33f)
                    NPC.velocity *= 0.95f;

                if (NPC.Opacity >= 1f)
                {
                    NPC.Opacity = 1f;
                    NPC.dontTakeDamage = false;
                }

                for (int i = 0; i < 8; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<AstralOrange>(), NPC.velocity.X, NPC.velocity.Y, 255, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0.5f;
                }

                return;
            }

            NPC.TargetClosest();

            // Push away from each other
            float pushVelocity = 0.5f;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active)
                {
                    if (i != NPC.whoAmI && Main.npc[i].type == NPC.type)
                    {
                        if (Vector2.Distance(NPC.Center, Main.npc[i].Center) < (160f + 30f * (NPC.scale - 1f)))
                        {
                            if (NPC.position.X < Main.npc[i].position.X)
                                NPC.velocity.X = NPC.velocity.X - pushVelocity;
                            else
                                NPC.velocity.X = NPC.velocity.X + pushVelocity;

                            if (NPC.position.Y < Main.npc[i].position.Y)
                                NPC.velocity.Y = NPC.velocity.Y - pushVelocity;
                            else
                                NPC.velocity.Y = NPC.velocity.Y + pushVelocity;
                        }
                    }
                }
            }

            // Percent life remaining
            float lifeRatio = NPC.life / (float)NPC.lifeMax;

            // Phase 2 check
            bool phase2 = lifeRatio <= 0.5f || Main.IsItDay();

            // Charge towards the target and explode after some time
            int inertia = 30;
            Vector2 vector = Main.player[NPC.target].Center - NPC.Center;
            Vector2 vector2 = Main.npc[CalamityGlobalNPC.astrumAureus].Center - NPC.Center;
            float distanceFromAureus = vector2.Length();
            float distanceFromTarget = vector.Length();
            float chargeVelocity = 8f;
            if (phase2)
            {
                inertia = 50;
                chargeVelocity = 16f;
                float engagePhase2GateValue = 60f;
                float enlargeGateValue = engagePhase2GateValue + 300f;

                // Pause when phase 2 is triggered
                NPC.ai[0] += 1f;
                if (NPC.ai[0] < engagePhase2GateValue)
                {
                    NPC.velocity *= 0.95f;
                    return;
                }

                // Emit dust and sound when phase 2 charging begins
                if (NPC.ai[0] == engagePhase2GateValue)
                {
                    SoundEngine.PlaySound(CommonCalamitySounds.ExoPlasmaExplosionSound, NPC.Center);

                    for (int j = 0; j < 10; j++)
                    {
                        int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<AstralOrange>(), 0f, 0f, 100, default, 1f);
                        Main.dust[dust].velocity *= 1.66f;
                        if (Main.rand.NextBool())
                        {
                            Main.dust[dust].scale = 0.5f;
                            Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                        }
                        Main.dust[dust].noGravity = true;
                    }

                    for (int k = 0; k < 20; k++)
                    {
                        int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 2f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity *= 2f;
                        dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<AstralOrange>(), 0f, 0f, 100, default, 1f);
                        Main.dust[dust].velocity *= 1.33f;
                        Main.dust[dust].noGravity = true;
                    }
                }

                // Grow in size
                float enlargeDuration = 180f;
                if (NPC.ai[0] >= enlargeGateValue)
                {
                    NPC.Calamity().newAI[0] += 1f;
                    NPC.scale = MathHelper.Lerp(1f, 2f, NPC.Calamity().newAI[0] / enlargeDuration);
                    NPC.width = (int)(90f * NPC.scale);
                    NPC.height = (int)(60f * NPC.scale);
                }

                // Check if it can explode
                Point point = NPC.Center.ToTileCoordinates();
                Tile tileSafely = Framing.GetTileSafely(point);
                bool explodeOnCollision = tileSafely.HasUnactuatedTile && Main.tileSolid[tileSafely.TileType] && !Main.tileSolidTop[tileSafely.TileType] && !TileID.Sets.Platforms[tileSafely.TileType];
                bool explodeOnAureus = distanceFromAureus < (180f + 30f * (NPC.scale - 1f)) && !Main.IsItDay();
                if (vector.Length() < (60f + 30f * (NPC.scale - 1f)) || explodeOnCollision || explodeOnAureus || NPC.Calamity().newAI[0] >= enlargeDuration)
                {
                    NPC.life = 0;
                    NPC.HitEffect();
                    NPC.checkDead();
                    return;
                }
            }

            // Move towards the target
            chargeVelocity += distanceFromTarget / 200f;
            if (distanceFromTarget > chargeVelocity)
            {
                vector.Normalize();
                vector *= chargeVelocity;
            }
            NPC.velocity = (NPC.velocity * (inertia - 1) + vector) / inertia;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.5f * balance);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.Calamity().newAI[0] >= 180f)
                return false;

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            if (NPC.ai[0] >= 60f)
                NPC.DrawBackglow(Color.Lerp(Color.Cyan, Color.Orange, (float)Math.Sin(Main.GlobalTimeWrappedHourly) / 2f + 0.5f) with { A = 0 }, 2f + 8f * ((float)Math.Sin(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi) + 1f), spriteEffects, NPC.frame, screenPos);

            Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
            Vector2 originalDrawSize = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
            Color whiteColor = Color.White;
            int afterimageAmt = 10;

            if (CalamityConfig.Instance.Afterimages)
            {
                for (int i = 1; i < afterimageAmt; i += 2)
                {
                    Color afterimageColor = drawColor;
                    afterimageColor = Color.Lerp(afterimageColor, whiteColor, 0.5f);
                    afterimageColor = NPC.GetAlpha(afterimageColor);
                    afterimageColor *= (afterimageAmt - i) / 15f;
                    Vector2 afterimagePos = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
                    afterimagePos -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    afterimagePos += originalDrawSize * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(texture2D15, afterimagePos, NPC.frame, afterimageColor, NPC.rotation, originalDrawSize, NPC.scale, spriteEffects, 0f);
                }
            }

            Vector2 drawLocation = NPC.Center - screenPos;
            drawLocation -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
            drawLocation += originalDrawSize * NPC.scale + new Vector2(0f, NPC.gfxOffY);
            spriteBatch.Draw(texture2D15, drawLocation, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, originalDrawSize, NPC.scale, spriteEffects, 0f);

            texture2D15 = GlowTexture.Value;
            Color afterimageColorLerp = Color.Lerp(Color.White, Color.Orange, 0.5f) * NPC.Opacity;

            if (CalamityConfig.Instance.Afterimages)
            {
                for (int j = 1; j < afterimageAmt; j++)
                {
                    Color secondAfterimageColor = afterimageColorLerp;
                    secondAfterimageColor = Color.Lerp(secondAfterimageColor, whiteColor, 0.5f);
                    secondAfterimageColor *= (afterimageAmt - j) / 15f;
                    Vector2 secondAfterimagePos = NPC.oldPos[j] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
                    secondAfterimagePos -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    secondAfterimagePos += originalDrawSize * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(texture2D15, secondAfterimagePos, NPC.frame, secondAfterimageColor, NPC.rotation, originalDrawSize, NPC.scale, spriteEffects, 0f);
                }
            }

            spriteBatch.Draw(texture2D15, drawLocation, NPC.frame, afterimageColorLerp, NPC.rotation, originalDrawSize, NPC.scale, spriteEffects, 0f);

            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.IsABestiaryIconDummy)
                NPC.Opacity = 1f;

            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void OnKill()
        {
            int closestPlayer = Player.FindClosest(NPC.Center, 1, 1);
            if (Main.rand.NextBool(8) && Main.player[closestPlayer].statLife < Main.player[closestPlayer].statLifeMax2)
                Item.NewItem(NPC.GetSource_Loot(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Heart);

            // Split into a halo of projectiles in death mode
            if (CalamityWorld.death || BossRushEvent.BossRushActive)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int totalProjectiles = (CalamityWorld.LegendaryMode && CalamityWorld.revenge) ? 36 : (3 + (int)((NPC.scale - 1f) * 3));
                    double radians = MathHelper.TwoPi / totalProjectiles;
                    int type = ModContent.ProjectileType<AstralLaser>();
                    int damage2 = NPC.GetProjectileDamage(type);
                    float velocity = (CalamityWorld.LegendaryMode && CalamityWorld.revenge) ? 10f : 6f;
                    double angleA = radians * 0.5;
                    double angleB = MathHelper.ToRadians(90f) - angleA;
                    float velocityX = (float)(velocity * Math.Sin(angleA) / Math.Sin(angleB));
                    Vector2 spinningPoint = Main.rand.NextBool() ? new Vector2(0f, -velocity) : new Vector2(-velocityX, -velocity);
                    for (int k = 0; k < totalProjectiles; k++)
                    {
                        Vector2 vector255 = spinningPoint.RotatedBy(radians * k);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vector255, type, damage2, 0f, Main.myPlayer);
                    }
                }
            }

            // Return if Astrum Aureus isn't active
            if (CalamityGlobalNPC.astrumAureus < 0 || !Main.npc[CalamityGlobalNPC.astrumAureus].active)
                return;

            // Damage Aureus for a percentage of its HP if the spawn explodes on or near it
            if (Main.netMode != NetmodeID.MultiplayerClient && (Main.npc[CalamityGlobalNPC.astrumAureus].Center - NPC.Center).Length() < (200f + 30f * (NPC.scale - 1f)) && !Main.IsItDay())
                Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), Main.npc[CalamityGlobalNPC.astrumAureus].Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), (int)(Main.npc[CalamityGlobalNPC.astrumAureus].lifeMax / 200 * NPC.scale), 0f, Main.myPlayer, Main.npc[CalamityGlobalNPC.astrumAureus].whoAmI);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ShadowbeamStaff, hit.HitDirection, -1f, 0, default, 1f);

            if (NPC.life <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item14, NPC.Center);

                NPC.position.X = NPC.position.X + NPC.width / 2;
                NPC.position.Y = NPC.position.Y + NPC.height / 2;
                NPC.damage = (int)Math.Round(NPC.defDamage * (double)NPC.scale);
                NPC.width = NPC.height = (int)(216 * NPC.scale);
                NPC.position.X = NPC.position.X - NPC.width / 2;
                NPC.position.Y = NPC.position.Y - NPC.height / 2;

                for (int r = 0; r < 30; r++)
                {
                    int astralDust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<AstralOrange>(), 0f, 0f, 100, default, 1f);
                    Main.dust[astralDust].velocity *= 3f;
                    if (Main.rand.NextBool())
                    {
                        Main.dust[astralDust].scale = 0.5f;
                        Main.dust[astralDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                    Main.dust[astralDust].noGravity = true;
                }

                for (int s = 0; s < 60; s++)
                {
                    int astralDust2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 2f);
                    Main.dust[astralDust2].noGravity = true;
                    Main.dust[astralDust2].velocity *= 5f;
                    astralDust2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<AstralOrange>(), 0f, 0f, 100, default, 1f);
                    Main.dust[astralDust2].velocity *= 2f;
                    Main.dust[astralDust2].noGravity = true;
                }
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot) => NPC.Opacity == 1f;

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            int debuffType = Main.zenithWorld ? ModContent.BuffType<GodSlayerInferno>() : ModContent.BuffType<AstralInfectionDebuff>();
            target.AddBuff(debuffType, (int)(75 * NPC.scale), true);
        }
    }
}
