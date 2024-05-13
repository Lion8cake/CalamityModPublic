﻿using System.IO;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using CalamityMod.Items.Weapons.Summon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Summon.AndromedaUI
{
    public class AndromedaUI_Background : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Summon";
        public int FadeoutTime = -1;
        public Vector2 PlayerOffset = Vector2.Zero;
        public static readonly int FadeoutTimeMax = 40;
        public static readonly Vector2 LeftBracketOffset = new(-8f, -6f);
        public static readonly Vector2 RightBracketOffset = new(8f, -6f);
        public static readonly Vector2 TopBracketOffset = new(2f, 27f);
        public GiantIbanRobotOfDoom AttachedRobot => Main.projectile[(int)Projectile.localAI[0]].ModProjectile<GiantIbanRobotOfDoom>();

        // These properties are here to mitigate the need for typing out long and cumbersome slews of the same code to access the robot's variables.

        #region Robot Properties
        public bool LeftBracketActive
        {
            get => AttachedRobot.LeftBracketActive;
            set => AttachedRobot.LeftBracketActive = value;
        }
        public bool RightBracketActive
        {
            get => AttachedRobot.RightBracketActive;
            set => AttachedRobot.RightBracketActive = value;
        }
        public bool BottomBracketActive
        {
            get => AttachedRobot.BottomBracketActive;
            set => AttachedRobot.BottomBracketActive = value;
        }

        public bool LeftIconActive
        {
            get => AttachedRobot.LeftIconActive;
            set => AttachedRobot.LeftIconActive = value;
        }
        public int RightIconCooldown
        {
            get => AttachedRobot.RightIconCooldown;
            set => AttachedRobot.RightIconCooldown = value;
        }
        public bool TopIconActive
        {
            get => AttachedRobot.TopIconActive;
            set => AttachedRobot.TopIconActive = value;
        }
        #endregion

        // Mouse calculations are done primarily in a World context, not Screen, in this file.
        public static Rectangle MouseRectangle => new((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 2, 2);

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 58;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 36000;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(FadeoutTime);
            writer.Write(Projectile.localAI[0]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            FadeoutTime = reader.ReadInt32();
            Projectile.localAI[0] = reader.ReadSingle();
        }

        public override void AI()
        {
            // Death fade-out effect
            if (FadeoutTime > 0)
            {
                Projectile.alpha = (int)MathHelper.Lerp(0f, 255f, 1f - FadeoutTime / (float)FadeoutTimeMax);
                FadeoutTime--;
            }
            else if (FadeoutTime == 0)
            {
                Projectile.Kill();
            }

            // Kill the UI immediately if there is no robot, and halt the AI
            if (Main.projectile[(int)Projectile.localAI[0]].type != ModContent.ProjectileType<GiantIbanRobotOfDoom>() ||
                !Main.projectile[(int)Projectile.localAI[0]].active)
            {
                Projectile.Kill();
                return;
            }

            // Fade-in effect
            Projectile.localAI[1]++;
            if (Projectile.localAI[1] < 40f)
            {
                Projectile.alpha = (int)MathHelper.Lerp(255f, 0f, Projectile.localAI[1] / 40f);
            }

            // Adjust the position of the UI.
            if (Main.myPlayer == Projectile.owner)
            {
                if (PlayerOffset == Vector2.Zero)
                {
                    PlayerOffset = Main.player[Projectile.owner].Center - Projectile.Center;
                }
                Projectile.Center = Main.player[Projectile.owner].Center - PlayerOffset;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Should be invisible for players that do not control the UI
            if (Main.myPlayer == Projectile.owner)
            {
                Matrix perspective = Main.GameViewMatrix.ZoomMatrix;

                // Undo the gravity effect on the perspective matrix. (The math on this is probably a bit wonky but it seems to work fine enough in practice so lol)
                if (Main.LocalPlayer.gravDir == -1f)
                    perspective *= Matrix.Invert(Matrix.CreateScale(1f, -1f, 1f) * Matrix.CreateTranslation(0f, Main.instance.GraphicsDevice.Viewport.Height, 0f));

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, Main.Rasterizer, null, perspective);

                // Draw the background UI
                Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value,
                                 Projectile.Center - Main.screenPosition,
                                 null,
                                 Color.White * Projectile.Opacity,
                                 0f,
                                 Projectile.Size * 0.5f,
                                 Projectile.scale,
                                 SpriteEffects.None,
                                 0f);
                DrawBrackets(Main.spriteBatch);
                DrawIcons(Main.spriteBatch);
                Main.spriteBatch.ExitShaderRegion();

                // Kills the projectile if the player clicks something that isn't the UI.
                // This is done in here and not AI to ensure that the above checks are done immediately before this check.
                if (!Main.blockMouse && Main.mouseLeft && FadeoutTime == -1)
                {
                    FadeoutTime = FadeoutTimeMax;
                }
            }

            return false;
        }

        public void DrawBrackets(SpriteBatch spriteBatch)
        {
            Texture2D leftBracketTexture = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/LeftBracket" + (LeftBracketActive ? "Lit" : "")).Value;
            Texture2D leftBracketTextureHovered = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/LeftBracketHovered").Value;

            Texture2D rightBracketTexture = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/RightBracket" + (RightBracketActive ? "Lit" : "")).Value;
            Texture2D rightBracketTextureHovered = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/RightBracketHovered").Value;

            Texture2D topBracketTexture = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/TopBracket" + (BottomBracketActive ? "Lit" : "")).Value;
            Texture2D topBracketTextureHovered = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/TopBracketHovered").Value;

            // Top bracket
            Vector2 topBracketPosition = Projectile.Bottom + TopBracketOffset;
            Rectangle topBracketFrame = new Rectangle((int)topBracketPosition.X - topBracketTexture.Width / 2,
                                                         (int)topBracketPosition.Y - 24,
                                                         topBracketTexture.Width - 18, topBracketTexture.Height - 12);
            bool topBracketSelect = MouseRectangle.Intersects(topBracketFrame) && !BottomBracketActive;

            Main.EntitySpriteDraw(topBracketSelect ? topBracketTextureHovered : topBracketTexture,
                             topBracketPosition - Main.screenPosition,
                             null,
                             Color.White * Projectile.Opacity,
                             0f,
                             Projectile.Size * 0.5f,
                             Projectile.scale,
                             SpriteEffects.None,
                             0);

            // Left bracket
            Vector2 leftBracketPosition = Projectile.Center + LeftBracketOffset;
            Rectangle leftBracketFrame = new Rectangle((int)leftBracketPosition.X - leftBracketTexture.Width / 2,
                                                         (int)leftBracketPosition.Y - 24,
                                                         leftBracketTexture.Width - 12, leftBracketTexture.Height - 18);
            bool leftBracketSelect = MouseRectangle.Intersects(leftBracketFrame) && !LeftBracketActive;

            Main.EntitySpriteDraw(leftBracketSelect ? leftBracketTextureHovered : leftBracketTexture,
                             leftBracketPosition - Main.screenPosition,
                             null,
                             Color.White * Projectile.Opacity,
                             0f,
                             Projectile.Size * 0.5f,
                             Projectile.scale,
                             SpriteEffects.None,
                             0);

            // Right bracket
            Vector2 rightBracketPosition = Projectile.Center + RightBracketOffset + (Projectile.width / 2 - 1) * Vector2.UnitX;
            Rectangle rightBracketFrame = new Rectangle((int)rightBracketPosition.X - rightBracketTexture.Width / 2,
                                                         (int)rightBracketPosition.Y - 24,
                                                         rightBracketTexture.Width - 12, rightBracketTexture.Height - 18);
            bool rightBracketSelect = MouseRectangle.Intersects(rightBracketFrame) && !RightBracketActive;

            Main.EntitySpriteDraw(rightBracketSelect ? rightBracketTextureHovered : rightBracketTexture,
                             rightBracketPosition - Main.screenPosition,
                             null,
                             Color.White * Projectile.Opacity,
                             0f,
                             Projectile.Size * 0.5f,
                             Projectile.scale,
                             SpriteEffects.None,
                             0);

            if (leftBracketSelect || rightBracketSelect || topBracketSelect)
            {
                Main.blockMouse = true;
                if (Main.mouseLeft && Main.projectile[(int)Projectile.localAI[0]].ai[0] <= 0f)
                {
                    if (leftBracketSelect)
                    {
                        LeftBracketActive = true;
                        RightBracketActive = BottomBracketActive = false;
                    }
                    if (rightBracketSelect)
                    {
                        if (FlamsteedRing.SpaceForLargeMech(Main.LocalPlayer))
                        {
                            RightBracketActive = true;
                            LeftBracketActive = BottomBracketActive = false;
                            LeftIconActive = false;
                        }
                    }
                    if (topBracketSelect)
                    {
                        BottomBracketActive = true;
                        LeftBracketActive = RightBracketActive = false;
                        TopIconActive = false;
                    }
                    Main.projectile[(int)Projectile.localAI[0]].ai[0] = 30f;
                }
            }
        }

        public void DrawIcons(SpriteBatch spriteBatch)
        {
            // Left Icon (Big/Small indicator)
            DrawLeftIcon(spriteBatch);

            // Right Icon (Lightning charge + cooldown)
            DrawRightIcon(spriteBatch);

            // Top Icon (Alternating between melee swing and ranged bursts)
            DrawTopIcon(spriteBatch);
        }

        public void DrawLeftIcon(SpriteBatch spriteBatch)
        {
            // Define icons to draw
            Texture2D smallIndicator = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/SmallIcon").Value;
            Texture2D smallIndicatorLocked = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/Lock").Value;
            Texture2D textureToDraw = smallIndicator;

            // An offset is required for said icons, as they have difference sizes. Don't touch them unless the sprites are modified.
            // Bless whoever is tasked's soul if they are indeed changed
            Vector2 iconOffset = new Vector2(-14f, 11f);
            Vector2 drawPosition = Projectile.Center + iconOffset;

            bool eitherBracketActive = LeftBracketActive || BottomBracketActive;

            // Lock the icon if the active bracket does not activate the icon
            if (!eitherBracketActive || RightBracketActive)
            {
                textureToDraw = smallIndicatorLocked;
                drawPosition = Projectile.Center + iconOffset;
            }
            bool wasActive = LeftIconActive;
            LeftIconActive = textureToDraw == smallIndicator; // In simpler terms, LeftIconActive = Whether the robot is small vs if it's big.
            // If the player's robot state changed, make a scene
            if (LeftIconActive != wasActive)
            {
                if (!Main.dedServ)
                {
                    Player player = Main.player[Projectile.owner];
                    for (int i = 0; i < 45; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(player.Center + Utils.NextVector2Circular(Main.rand, 60f, 90f), 26);
                        dust.velocity = Utils.NextVector2Circular(Main.rand, 4f, 4f);
                        dust.noGravity = true;
                        dust.scale = Main.rand.NextFloat(1.2f, 1.35f);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        Utils.PoofOfSmoke(player.Center + Utils.NextVector2Circular(Main.rand, 20f, 30f));
                    }
                }
            }
            // And finally draw
            Main.EntitySpriteDraw(textureToDraw,
                             drawPosition - Main.screenPosition,
                             null,
                             Color.White * Projectile.Opacity,
                             0f,
                             textureToDraw.Size() * 0.5f,
                             Projectile.scale,
                             SpriteEffects.None,
                             0);
        }

        public void DrawRightIcon(SpriteBatch spriteBatch)
        {
            // Define icons to draw
            Texture2D thunderIndicator = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/ThunderIcon").Value;
            Texture2D thunderIndicatorHovered = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/ThunderIconHovered").Value;
            Texture2D thunderIndicatorCharge = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/ThunderIconCharge").Value;
            Texture2D thunderIndicatorLocked = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/Lock").Value;
            Texture2D textureToDraw = thunderIndicatorLocked;

            // An offset is required for said icons, as they have difference sizes. Don't touch them unless the sprites are modified.
            // Bless whoever is tasked's soul if they are indeed changed
            Vector2 iconOffset = new Vector2(14f, 11f);
            Vector2 drawPosition;

            bool eitherBracketActive = RightBracketActive || BottomBracketActive;

            // Lock the icon if the active bracket does not activate the icon
            if (eitherBracketActive && !LeftBracketActive)
            {
                textureToDraw = thunderIndicator;
            }

            drawPosition = Projectile.Center + iconOffset;

            // Otherwise do mouse click/hover checks, and, assuming the cooldown is at 0, perform the special attack and restart the cooldown.
            if (textureToDraw != thunderIndicatorLocked && RightIconCooldown <= 0)
            {
                Rectangle iconFrame = Utils.CenteredRectangle(drawPosition, textureToDraw.Size());
                textureToDraw = thunderIndicator;
                if (MouseRectangle.Intersects(iconFrame))
                {
                    textureToDraw = thunderIndicatorHovered;
                    // Prevent clicking affecting other things. For example, disallow weapon firing.
                    Main.blockMouse = true;
                    if (Main.mouseLeft && Main.projectile[(int)Projectile.localAI[0]].ai[0] <= 0f)
                    {
                        FadeoutTime = FadeoutTimeMax;
                        RightIconCooldown = GiantIbanRobotOfDoom.RightIconCooldownMax;
                        // A cooldown value. This ensures that the game will not register another click
                        // one frame after the first one.
                        Main.projectile[(int)Projectile.localAI[0]].ai[0] = 30f;

                        // Explosion effect
                        if (!Main.dedServ)
                        {
                            for (int i = 0; i < 80; i++)
                            {
                                Dust dust = Dust.NewDustPerfect(Main.projectile[(int)Projectile.localAI[0]].Center, Utils.SelectRandom(Main.rand, 226, 263));
                                dust.velocity = Main.rand.NextVector2Circular(14f, 14f);
                                dust.fadeIn = 1.1f;
                                dust.noGravity = true;
                            }
                        }
                        SoundEngine.PlaySound(GaussRifle.FireSound, Projectile.Center);
                    }
                }
            }
            // And finally draw the cooldown texture + the icon
            float chargeCompletionRatio = 1f - (RightIconCooldown / (float)GiantIbanRobotOfDoom.RightIconCooldownMax);
            Main.EntitySpriteDraw(thunderIndicatorCharge,
                             drawPosition + Vector2.UnitY - Main.screenPosition,
                             new Rectangle(0, 0, thunderIndicatorCharge.Width, (int)(chargeCompletionRatio * thunderIndicatorCharge.Height)), // Fill up vertically
                             Color.White * Projectile.Opacity,
                             0f,
                             thunderIndicatorCharge.Size() * 0.5f,
                             Projectile.scale,
                             SpriteEffects.None,
                             0);
            Main.EntitySpriteDraw(textureToDraw,
                             drawPosition - Main.screenPosition,
                             null,
                             Color.White * Projectile.Opacity,
                             0f,
                             textureToDraw.Size() * 0.5f,
                             Projectile.scale,
                             SpriteEffects.None,
                             1);
        }

        public void DrawTopIcon(SpriteBatch spriteBatch)
        {
            // Define icons to draw
            Texture2D meleeIndicator = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/MeleeIcon").Value;
            Texture2D rangedIndicator = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/RangedIcon").Value;
            Texture2D attackIndicatorLocked = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/Lock").Value;
            Texture2D meleeIndicatorHovered = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/MeleeIconHovered").Value;
            Texture2D rangedIndicatorHovered = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/AndromedaUI/RangedIconHovered").Value;
            Texture2D textureToDraw = TopIconActive ? meleeIndicator : rangedIndicator;

            // An offset is required for said icons, as they have difference sizes. Don't touch them unless the sprites are modified.
            // Bless whoever is tasked's soul if they are indeed changed
            Vector2 iconOffset = new Vector2(0f, -12f);
            Vector2 drawPosition = Projectile.Center + iconOffset;

            bool eitherBracketActive = LeftBracketActive || RightBracketActive;

            // Lock the icon if the active bracket does not activate the icon
            if (!eitherBracketActive || BottomBracketActive)
            {
                textureToDraw = attackIndicatorLocked;
                drawPosition = Projectile.Center + iconOffset;
            }
            // Otherwise do mouse click/hover checks, and, assuming the cooldown is at 0, perform the special attack and restart the cooldown.
            else if (eitherBracketActive)
            {
                Rectangle iconFrame = Utils.CenteredRectangle(drawPosition, textureToDraw.Size());
                if (MouseRectangle.Intersects(iconFrame))
                {
                    textureToDraw = TopIconActive ? meleeIndicatorHovered : rangedIndicatorHovered;
                    // Prevent clicking affecting other things. For example, disallow weapon firing.
                    Main.blockMouse = true;
                    if (Main.mouseLeft && Main.projectile[(int)Projectile.localAI[0]].ai[0] <= 0f)
                    {
                        TopIconActive = !TopIconActive;
                        // A cooldown value. This ensures that the game will not register another click
                        // one frame after the first one.
                        Main.projectile[(int)Projectile.localAI[0]].ai[0] = 30f;
                    }
                }
            }
            // And finally draw
            Main.EntitySpriteDraw(textureToDraw,
                             drawPosition - Main.screenPosition,
                             null,
                             Color.White * Projectile.Opacity,
                             0f,
                             textureToDraw.Size() * 0.5f,
                             Projectile.scale,
                             SpriteEffects.None,
                             0);
        }

        public override bool? CanDamage() => false; // This is a UI. It should not do damage.
    }
}
