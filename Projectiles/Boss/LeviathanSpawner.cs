﻿using System;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using LeviathanNPC = CalamityMod.NPCs.Leviathan.Leviathan;

namespace CalamityMod.Projectiles.Boss
{
    public class LeviathanSpawner : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Boss";
        internal ref float Time => ref Projectile.ai[0];
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public static readonly SoundStyle RumbleSound = new("CalamityMod/Sounds/Custom/LeviathanRumble");
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 10000;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
            Projectile.timeLeft = 450;
        }

        public override void AI()
        {
            Projectile.Opacity = CalamityUtils.Convert01To010(Projectile.timeLeft / 120f) * 4f;
            if (Projectile.Opacity > 1f)
                Projectile.Opacity = 1f;

            Main.LocalPlayer.Calamity().GeneralScreenShakePower = (float)Math.Pow(Utils.GetLerpValue(180f, 290f, Time, true), 0.3D) * 6f;
            Main.LocalPlayer.Calamity().GeneralScreenShakePower += CalamityUtils.Convert01To010((float)Math.Pow(Utils.GetLerpValue(300f, 440f, Time, true), 0.5D)) * 10f;

            if (Projectile.timeLeft % 180 == 0)
            {
                SoundEngine.PlaySound(RumbleSound, Projectile.Center);
            }

            if (Projectile.timeLeft == 45)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    WaterShaderData ripple = (WaterShaderData)Filters.Scene["WaterDistortion"].GetShader();
                    Vector2 ripplePos = Projectile.Center;

                    for (int i = 0; i < 3; i++)
                        ripple.QueueRipple(ripplePos, Color.White, Vector2.One * 1000f, RippleShape.Square, Main.rand.NextFloat(MathHelper.TwoPi));
                }

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    return;

                int leviathan = NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, ModContent.NPCType<LeviathanNPC>());
                if (Main.npc.IndexInRange(leviathan))
                    Main.npc[leviathan].velocity = Vector2.UnitY * -7f;
            }

            CreateVisuals();
            Time++;
        }

        public void CreateVisuals()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            WorldUtils.Find((Projectile.Center - Vector2.UnitY * 1200f).ToTileCoordinates(), Searches.Chain(new Searches.Down(150), new CustomConditions.IsWater()), out Point waterTop);

            // Create bubbling water.
            if (Time % 4f == 3f && Time > 90f)
            {
                float xArea = MathHelper.Lerp(400f, 1150f, Time / 300f);
                Vector2 dustSpawnPosition = waterTop.ToWorldCoordinates() + Vector2.UnitY * 25f;
                dustSpawnPosition.X += Main.rand.NextFloatDirection() * xArea * 0.35f;
                Dust bubble = Dust.NewDustPerfect(dustSpawnPosition, 267, Vector2.UnitY * -12f);
                bubble.noGravity = true;
                bubble.scale = 1.9f;
                bubble.color = Color.CornflowerBlue;

                // As well as liquid disruption.
                for (float x = -xArea; x <= xArea; x += 110f)
                {
                    float ripplePower = MathHelper.Lerp(4f, 10f, (float)Math.Sin(Main.GlobalTimeWrappedHourly + x / xArea * MathHelper.TwoPi) * 0.5f + 0.5f);
                    ripplePower *= MathHelper.Lerp(0.5f, 1f, Time / 300f);

                    WaterShaderData ripple = (WaterShaderData)Filters.Scene["WaterDistortion"].GetShader();
                    Vector2 ripplePos = waterTop.ToWorldCoordinates() + new Vector2(x, 32f) + Main.rand.NextVector2CircularEdge(50f, 50f);
                    ripple.QueueRipple(ripplePos, Color.White, Vector2.One * ripplePower, RippleShape.Square, Main.rand.NextFloat(-0.7f, 0.7f) + MathHelper.PiOver2);
                }
            }
        }
    }
}
