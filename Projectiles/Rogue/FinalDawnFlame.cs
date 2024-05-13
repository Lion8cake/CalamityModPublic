﻿using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class FinalDawnFlame : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        internal struct Flame
        {
            public Vector2 Position;

            public int FrameCounter;

            public int Frame;

            public float Alpha;

            public float Scale;

            public int Direction;
        }
        private Flame[] Flames;
        public const int TotalFlames = 120;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }
        public override void SetDefaults()
        {
            Projectile.scale = 1f;
            Projectile.width = 1000;
            Projectile.height = 100;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = RogueDamageClass.Instance;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;

            Flames = new Flame[TotalFlames];
            for (int i = 0; i < Flames.Length; i++)
            {
                Vector2 flamePosition = Main.rand.NextVector2Circular(Projectile.width * 0.36f, Projectile.height / 2);

                Flames[i].Position = flamePosition;
                Flames[i].Frame = Main.rand.Next(8);
                Flames[i].FrameCounter = Main.rand.Next(5);
                Flames[i].Alpha = 1f;
                Flames[i].Scale = 0.8f + 0.4f * Main.rand.NextFloat();
                Flames[i].Direction = Main.rand.Next(2);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];


            if (Flames.Length > 0)
            {
                for (int i = 0; i < Flames.Length; i++)
                {
                    int frameHeight2 = frameHeight * Flames[i].Frame;
                    Main.EntitySpriteDraw(texture,
                                     Projectile.Center + Flames[i].Position - Main.screenPosition,
                                     new Rectangle?(new Rectangle(0, frameHeight2, texture.Width, frameHeight)),
                                     Projectile.GetAlpha(Color.White) * Flames[i].Alpha,
                                     Projectile.rotation,
                                     new Vector2(texture.Width / 2f, frameHeight / 2f),
                                     Flames[i].Scale,
                                     Flames[i].Direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                                     0);
                }
            }
            return false;
        }

        public override void AI()
        {
            if (Flames.Length > 0)
            {
                for (int i = 0; i < Flames.Length; i++)
                {
                    AdjustFlameValues(ref Flames[i]);
                }
            }
            AlphaAdjustments();
        }
        // The internal is to match consistency with the Flame struct. If you want to make this class public, you must also make the struct public.
        internal void AdjustFlameValues(ref Flame flame)
        {
            flame.FrameCounter++;
            if (flame.FrameCounter >= 5)
            {
                flame.Frame++;
                flame.FrameCounter = 0;

                if (flame.Frame >= 8)
                {
                    if (Projectile.ai[0] < 570)
                    {
                        Vector2 pos = new Vector2(Main.rand.NextFloat(Projectile.width * 0.36f)).RotatedByRandom(MathHelper.TwoPi);
                        float widthHeightRatio = (float)Projectile.height / Projectile.width;
                        pos.Y *= widthHeightRatio;

                        flame.Position = pos;
                        flame.Scale = 0.8f + 0.4f * Main.rand.NextFloat();
                        flame.Direction = Main.rand.Next(2);
                        flame.Frame = 0;
                    }
                    else
                    {
                        flame.Alpha = 0f;
                    }
                }
            }
        }
        public void AlphaAdjustments()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 570)
            {
                Projectile.alpha -= 10;
                if (Projectile.alpha < 0) Projectile.alpha = 0;
            }
            else
            {
                Projectile.friendly = false;
                Projectile.alpha += 5;
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                }
            }
            if (Projectile.ai[0] >= 600 && Projectile.alpha >= 255)
            {
                Projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 180);
        }
    }
}
