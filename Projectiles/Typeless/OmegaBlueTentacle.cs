﻿using CalamityMod.Balancing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Typeless
{
    public class OmegaBlueTentacle : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Typeless";
        public bool initSegments = false;
        public Vector2[] segment = new Vector2[6];

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.timeLeft = 8;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override bool PreAI()
        {
            if (!initSegments)
            {
                initSegments = true;
                for (int i = 0; i < 6; i++)
                {
                    segment[i] = Projectile.Center;
                }
            }
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            bool hentai = player.Calamity().omegaBlueHentai;
            if (player.active && player.Calamity().omegaBlueSet)
                Projectile.timeLeft = 8;

            //tentacle head movement (homing)
            Vector2 playerVel = player.position - player.oldPosition;
            Projectile.position += playerVel;
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 0f)
            {
                Vector2 home = player.Center + new Vector2(50, 0).RotatedBy(MathHelper.ToRadians(60) * Projectile.ai[1]);
                Vector2 distance = home - Projectile.Center;
                float range = distance.Length();
                distance.Normalize();
                if (Projectile.ai[0] == 0f)
                {
                    if (range > 13f)
                    {
                        Projectile.ai[0] = -1f; //if in fast mode, stay fast until back in range
                        if (range > 1300f)
                        {
                            Projectile.Kill();
                            return;
                        }
                    }
                    else
                    {
                        if (hentai)
                            Projectile.ai[0] = 120f;//45f + Main.rand.Next(45);
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 3f + Main.rand.NextFloat(3f);
                        Projectile.netUpdate = true;
                    }
                }
                else
                {
                    distance /= 8f;
                }
                if (range > 120f) //switch to fast return mode
                {
                    Projectile.ai[0] = -1f;
                    Projectile.netUpdate = true;
                }
                Projectile.velocity += distance;
                if (range > 30f)
                    Projectile.velocity *= 0.96f;

                if (Projectile.ai[0] > 120f) //attack nearby enemy
                {
                    Projectile.ai[0] = 10 + Main.rand.Next(10);
                    float maxDistance = hentai ? 900f : 600f;
                    int target = -1;
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.CanBeChasedBy(Projectile))
                        {
                            float npcDistance = Projectile.Distance(npc.Center);
                            if (npcDistance < maxDistance)
                            {
                                maxDistance = npcDistance;
                                target = i;
                            }
                        }
                    }
                    if (target != -1)
                    {
                        Projectile.velocity = Main.npc[target].Center - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 13f;
                        Projectile.velocity += Main.npc[target].velocity / 2f;
                        Projectile.velocity -= playerVel / 2f;
                        Projectile.ai[0] *= -1f;
                    }
                    Projectile.netUpdate = true;
                }
            }

            //tentacle segment updates
            segment[0] = player.Center;
            for (int i = 1; i < 5; i++)
            {
                MoveSegment(segment[i - 1], ref segment[i], segment[i + 1]);
            }
            MoveSegment(segment[4], ref segment[5], Projectile.Center + Projectile.velocity);

            if (hentai)
            {
                if (Projectile.ai[0] != -1f)
                    Projectile.ai[0]++;
                Projectile.position += Projectile.velocity;
                //SMOOTH ASS DUST
                Vector2 dustPos = Projectile.position + Projectile.velocity;
                Vector2 tickVel = dustPos - Projectile.oldPosition; //playerVel + projectile.velocity * 2f;
                dustPos += new Vector2(Projectile.width / 2, 0).RotatedBy(Projectile.rotation);
                dustPos.X -= 4;
                dustPos.X += Projectile.width / 2;
                dustPos.Y -= 4;
                dustPos.Y += Projectile.height / 2;
                const float factor = 3f;
                int limit = (int)(tickVel.Length() / factor);
                if (limit == 0)
                {
                    int d = Dust.NewDust(dustPos, 0, 0, DustID.PurificationPowder, 0, 0, 100, Color.Transparent, 0.9f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].noLight = true;
                    Main.dust[d].fadeIn = 1f;
                    Main.dust[d].velocity = Vector2.Zero;
                }
                else
                {
                    tickVel.Normalize();
                    tickVel *= factor;
                    for (int i = 0; i <= limit; i++)
                    {
                        int d = Dust.NewDust(dustPos, 0, 0, DustID.PurificationPowder, 0, 0, 100, Color.Transparent, 0.9f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].noLight = true;
                        Main.dust[d].fadeIn = 1f;
                        Main.dust[d].position -= tickVel * i;
                        Main.dust[d].velocity = Vector2.Zero;
                    }
                }
            }
        }

        private static void MoveSegment(Vector2 previous, ref Vector2 current, Vector2 next)
        {
            current = previous + next;
            current /= 2;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.player[Projectile.owner].Calamity().omegaBlueHentai)
                modifiers.SetCrit();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.owner == Main.myPlayer && Main.player[Projectile.owner].lifeSteal > 0f && !Main.player[Projectile.owner].moonLeech && target.lifeMax > 5)
            {
                int healAmount = 10 * damageDone / Projectile.damage; //should always be around max, less if enemy has defense/DR
                if (healAmount > BalancingConstants.LifeStealCap)
                    healAmount = BalancingConstants.LifeStealCap;

                if (healAmount > 0)
                {
                    Main.player[Projectile.owner].lifeSteal -= healAmount;
                    if (Main.player[Projectile.owner].Calamity().omegaBlueHentai) //hentai always crits, this makes it have same lifesteal delay
                        Main.player[Projectile.owner].lifeSteal += healAmount / 2;

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileID.SpiritHeal, 0, 0f, Projectile.owner, Projectile.owner, healAmount);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            GameShaders.Armor.ApplySecondary(Main.player[Projectile.owner].cBody, Main.player[Projectile.owner], new DrawData?());
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Texture2D segmentSprite = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Typeless/OmegaBlueTentacleSegment1").Value;
            for (int i = 0; i < 5; i++)
            {
                Projectile.rotation = (Projectile.Center - segment[i]).ToRotation();
                switch (i)
                {
                    case 0:
                        break;
                    case 1:
                        segmentSprite = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Typeless/OmegaBlueTentacleSegment2").Value;
                        break;
                    case 2:
                        segmentSprite = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Typeless/OmegaBlueTentacleSegment3").Value;
                        break;
                    case 3:
                        segmentSprite = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Typeless/OmegaBlueTentacleSegment4").Value;
                        break;
                    case 4:
                        segmentSprite = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Typeless/OmegaBlueTentacleSegment5").Value;
                        break;
                    default:
                        break;

                }
                Main.spriteBatch.Draw(segmentSprite, segment[i] - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), segmentSprite.Bounds, Projectile.GetAlpha(lightColor), Projectile.rotation, segmentSprite.Bounds.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            }
            Projectile.rotation = (Projectile.Center - segment[5]).ToRotation();
            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), texture2D13.Bounds, Projectile.GetAlpha(lightColor), Projectile.rotation, texture2D13.Bounds.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
