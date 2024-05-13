﻿using System;
using CalamityMod.NPCs.AstrumDeus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Boss
{
    public class DeusRitualDrama : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Boss";
        public static readonly SoundStyle PulseSound = new("CalamityMod/Sounds/Custom/AstralBeaconOrbPulse");

        public float Time
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public bool CreatedWithStarcore => Projectile.ai[1] == 1f;
        public const int TotalSinePeriods = 6;
        public const int PulseTime = 45;
        public const int TotalRitualTime = 420;
        public const float MaxUpwardRise = 540f;
        public static readonly Point PulseSize = new(300, 300);
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 2;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = TotalRitualTime;
        }

        public override void AI()
        {
            Projectile.extraUpdates = CreatedWithStarcore.ToInt();

            Time++;
            if (Time == TotalRitualTime - PulseTime)
            {
                int idx = NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y - (int)MaxUpwardRise, ModContent.NPCType<AstrumDeusHead>(), 1);
                if (idx != -1)
                {
                    SoundEngine.PlaySound(AstrumDeusHead.SpawnSound, Projectile.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        CalamityUtils.BossAwakenMessage(idx);
                    }
                    else
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, idx);
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            float upwardnessRatio = Utils.GetLerpValue(60f, TotalRitualTime, Time, true);
            float upwardness = MathHelper.Lerp(0f, MaxUpwardRise, upwardnessRatio);
            if (Time >= TotalRitualTime - PulseTime)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                float pulseCompletionRatio = Utils.GetLerpValue(TotalRitualTime - PulseTime, TotalRitualTime, Time, true);
                Vector2 scale = Projectile.scale * (3f + pulseCompletionRatio * 5f) * new Vector2(1.5f, 1f);
                DrawData drawData = new(ModContent.Request<Texture2D>("Terraria/Images/Misc/Perlin").Value,
                    Projectile.Center - Main.screenPosition + PulseSize.ToVector2() * scale * 0.5f - Vector2.UnitY * upwardness,
                    new Rectangle(0, 0, PulseSize.X, PulseSize.Y),
                    new Color(new Vector4(1f - (float)Math.Sqrt(pulseCompletionRatio))) * 0.66f,
                    Projectile.rotation,
                    PulseSize.ToVector2(),
                    scale,
                    SpriteEffects.None, 0);

                Color pulseColor = Color.Lerp(Color.Cyan * 1.5f, Color.OrangeRed, MathHelper.Clamp(pulseCompletionRatio * 1.5f, 0f, 1f));
                GameShaders.Misc["ForceField"].UseColor(pulseColor);
                GameShaders.Misc["ForceField"].Apply(drawData);
                drawData.Draw(Main.spriteBatch);
                return false;
            }
            float outwardnessRatio = Utils.GetLerpValue(60f, 220f, Time, true);
            if (Time > 250f)
                outwardnessRatio = 1f - Utils.GetLerpValue(250f, TotalRitualTime - PulseTime, Time, true);
            float outwardness = MathHelper.Lerp(0f, 140f, outwardnessRatio);

            Vector2 offset = new((float)Math.Sin(Time / (TotalRitualTime - PulseTime) * MathHelper.TwoPi * TotalSinePeriods) * outwardness, -upwardness);

            // If the stars "collide", generate some small explosion dust.
            if (!Main.dedServ && Math.Abs(offset.X) < 6f && Time > 60f)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + Vector2.UnitY * offset.Y, 261);
                    dust.color = Utils.SelectRandom(Main.rand, Color.Cyan, Color.OrangeRed);
                    dust.scale = 1.15f;
                    dust.velocity = Main.rand.NextVector2CircularEdge(3f, 3f) * Main.rand.NextFloat(0.7f, 1.4f);
                    dust.noGravity = true;

                    float angle = MathHelper.TwoPi * i / 20f;
                    dust = Dust.NewDustPerfect(Projectile.Center + Vector2.UnitY * offset.Y, 261);
                    dust.color = Utils.SelectRandom(Main.rand, Color.Cyan, Color.OrangeRed);
                    dust.scale = 1.15f;
                    dust.velocity = angle.ToRotationVector2() * 7f;
                    dust.noGravity = true;
                }
                SoundEngine.PlaySound(PulseSound, Projectile.Center);
            }

            DrawStars(Main.spriteBatch, offset);

            return false;
        }

        public void DrawStars(SpriteBatch spriteBatch, Vector2 offset)
        {
            Texture2D starTexture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            for (int i = 0; i < 6; i++)
            {
                float angle = MathHelper.TwoPi * i / 6f + Time / 15f;
                Vector2 angularOffset = angle.ToRotationVector2() * 4f;
                Main.EntitySpriteDraw(starTexture,
                                 Projectile.Center + angularOffset + offset - Main.screenPosition,
                                 null,
                                 Color.Cyan * 0.5f,
                                 0f,
                                 starTexture.Size() * 0.5f,
                                 0.6f,
                                 SpriteEffects.None,
                                 0);
                Main.EntitySpriteDraw(starTexture,
                                 Projectile.Center + angularOffset + offset * new Vector2(-1f, 1f) - Main.screenPosition,
                                 null,
                                 Color.OrangeRed * 0.5f,
                                 0f,
                                 starTexture.Size() * 0.5f,
                                 0.6f,
                                 SpriteEffects.None,
                                 0);
            }
            Main.EntitySpriteDraw(starTexture,
                             Projectile.Center + offset - Main.screenPosition,
                             null,
                             Color.Cyan * 1.4f,
                             0f,
                             starTexture.Size() * 0.5f,
                             0.6f,
                             SpriteEffects.None,
                             0);
            Main.EntitySpriteDraw(starTexture,
                             Projectile.Center + offset * new Vector2(-1f, 1f) - Main.screenPosition,
                             null,
                             Color.OrangeRed * 1.1f,
                             0f,
                             starTexture.Size() * 0.5f,
                             0.6f,
                             SpriteEffects.None,
                             0);

            // Generate dust at the star position. This gives them a trail effect.
            if (!Main.dedServ)
            {
                Dust dust2 = Dust.NewDustPerfect(Projectile.Center + offset, 261);
                dust2.color = Color.Cyan;
                dust2.scale = 1.15f;
                dust2.velocity = Vector2.Zero;
                dust2.noGravity = true;

                dust2 = Dust.NewDustPerfect(Projectile.Center + offset * new Vector2(-1f, 1f), 261);
                dust2.color = Color.OrangeRed;
                dust2.scale = 1.15f;
                dust2.velocity = Vector2.Zero;
                dust2.noGravity = true;
            }
        }
    }
}
