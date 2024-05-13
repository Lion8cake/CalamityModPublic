﻿using System;
using System.Collections.Generic;
using CalamityMod.Buffs.Summon;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Summon.Umbrella
{
    public class MagicHat : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Summon";
        public const float Range = 1500.0001f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 5f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();
            CalamityGlobalProjectile modProj = Projectile.Calamity();

            // Set up minion buffs and bools
            bool hatExists = Projectile.type == ModContent.ProjectileType<MagicHat>();
            player.AddBuff(ModContent.BuffType<MagicHatBuff>(), 3600);
            if (hatExists)
            {
                if (player.dead)
                {
                    modPlayer.magicHat = false;
                }
                if (modPlayer.magicHat)
                {
                    Projectile.timeLeft = 2;
                }
            }

            // On frame 2, spawn the tools
            if (Projectile.ai[0] == 1f)
            {
                List<Tuple<int, float>> Projectiles = new List<Tuple<int, float>>()
                {
                    new Tuple<int, float>(ModContent.ProjectileType<MagicArrow>(), 2f),
                    new Tuple<int, float>(ModContent.ProjectileType<MagicHammer>(), 3f),
                    new Tuple<int, float>(ModContent.ProjectileType<MagicAxe>(), 1f),
                    new Tuple<int, float>(ModContent.ProjectileType<MagicUmbrella>(), 1f),
                    new Tuple<int, float>(ModContent.ProjectileType<MagicRifle>(), 1f),
                };
                for (int i = 0; i < Projectiles.Count; i++)
                {
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, Projectiles[i].Item1, (int)(Projectile.damage * Projectiles[i].Item2),
                                                     Projectile.knockBack * Projectiles[i].Item2, Projectile.owner, Projectile.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = (int)(Projectile.originalDamage * Projectiles[i].Item2);
                }
            }
            Projectile.ai[0]++;

            // Stick to the player's head, account for shifts in gravity
            Projectile.Center = player.MountedCenter + Vector2.UnitY * (player.gfxOffY - 30f);
            if (player.gravDir == -1f)
            {
                Projectile.Center = player.MountedCenter - Vector2.UnitY * (player.gfxOffY - 30f);
                Projectile.rotation = MathHelper.Pi;
            }
            else
            {
                Projectile.rotation = 0f;
            }
            Projectile.position.X = (int)Projectile.position.X;
            Projectile.position.Y = (int)Projectile.position.Y;

            // Change the summons scale size a little bit to make it pulse in and out
            float scalar = (float)Main.mouseTextColor / 200f - 0.35f;
            scalar *= 0.2f;
            Projectile.scale = scalar + 0.95f;

            // On summon dust
            if (Projectile.localAI[0] == 0f)
            {
                int dustAmt = 50;
                for (int dustIndex = 0; dustIndex < dustAmt; dustIndex++)
                {
                    int dustEffects = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height - 16, DustID.BoneTorch, 0f, 0f, 0, default, 1f);
                    Main.dust[dustEffects].velocity *= 2f;
                    Main.dust[dustEffects].scale *= 1.15f;
                }
                Projectile.localAI[0] += 1f;
            }
        }

        // Glowmask effect
        public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, 200);

        // No contact damage
        public override bool? CanDamage() => false;

        // Draw over players
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => overPlayers.Add(index);

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            // Use a different texture if the player is invisible or has at least 50% stealth
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            float stealthPercent = player.Calamity().rogueStealthMax != 0 ? (player.Calamity().rogueStealth / player.Calamity().rogueStealthMax) : 0f; //0 to 1
            bool hasStealth = player.Calamity().rogueStealth > 0f && stealthPercent > 0.5f && player.townNPCs < 3f && CalamityConfig.Instance.StealthInvisibility;
            if (player.ShouldNotDraw || hasStealth)
                texture = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/Umbrella/MagicHatInvis").Value;

            Rectangle frame = texture.Frame(1, Main.projFrames[Type], 0, Projectile.frame);
            Vector2 origin = frame.Size() * 0.5f;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            SpriteEffects direction = Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            // Draw the hat.
            if (player.dye[0].dye > 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                var dyeShader = GameShaders.Armor.GetShaderFromItemId(player.dye[0].type);
                dyeShader.Apply();
                Main.spriteBatch.Draw(texture, drawPosition, frame, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, direction, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            }
            else
            {
                Main.spriteBatch.Draw(texture, drawPosition, frame, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, direction, 0);
            }

            return false;
        }
    }
}
