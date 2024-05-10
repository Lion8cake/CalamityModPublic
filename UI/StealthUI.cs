﻿using System;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using static Microsoft.Xna.Framework.Input.Keys;

namespace CalamityMod.UI
{
    // TODO -- This can be made into a ModSystem with simple OnModLoad and Unload hooks.
    public class StealthUI
    {
        // These values were handpicked on a 1080p screen by Ozzatron. Please disregard the bizarre precision.
        internal const float DefaultStealthPosX = 50.104603f;
        internal const float DefaultStealthPosY = 55.765408f;
        private const float MouseDragEpsilon = 0.05f; // 0.05%

        private static Vector2? dragOffset = null;
        private static Texture2D edgeTexture, indicatorTexture, barTexture, fullBarTexture;

        internal static void Load()
        {
            edgeTexture = ModContent.Request<Texture2D>("CalamityMod/UI/MiscTextures/StealthMeter", AssetRequestMode.ImmediateLoad).Value;
            indicatorTexture = ModContent.Request<Texture2D>("CalamityMod/UI/MiscTextures/StealthMeterStrikeIndicator", AssetRequestMode.ImmediateLoad).Value;
            barTexture = ModContent.Request<Texture2D>("CalamityMod/UI/MiscTextures/StealthMeterBar", AssetRequestMode.ImmediateLoad).Value;
            fullBarTexture = ModContent.Request<Texture2D>("CalamityMod/UI/MiscTextures/StealthMeterBarFull", AssetRequestMode.ImmediateLoad).Value;
            Reset();
        }

        internal static void Unload()
        {
            Reset();
            edgeTexture = indicatorTexture = barTexture = fullBarTexture = null;
        }

        private static void Reset() => dragOffset = null;

        public static void Draw(SpriteBatch spriteBatch, Player player)
        {
            // Sanity check the planned position before drawing
            Vector2 screenRatioPosition = new Vector2(CalamityConfig.Instance.StealthMeterPosX, CalamityConfig.Instance.StealthMeterPosY);
            if (screenRatioPosition.X < 0f || screenRatioPosition.X > 100f)
                screenRatioPosition.X = DefaultStealthPosX;
            if (screenRatioPosition.Y < 0f || screenRatioPosition.Y > 100f)
                screenRatioPosition.Y = DefaultStealthPosY;

            // Convert the screen ratio position to an absolute position in pixels
            // Cast to integer to prevent blurriness which results from decimal pixel positions
            float uiScale = Main.UIScale;
            Vector2 screenPos = screenRatioPosition;
            screenPos.X = (int)(screenPos.X * 0.01f * Main.screenWidth);
            screenPos.Y = (int)(screenPos.Y * 0.01f * Main.screenHeight);

            CalamityPlayer modPlayer = player.Calamity();

            // If not drawing the stealth meter, save its latest position to config and leave.
            if (modPlayer.stealthUIAlpha > 0f && CalamityConfig.Instance.StealthMeter && modPlayer.rogueStealthMax > 0f && modPlayer.wearingRogueArmor)
            {
                DrawStealthBar(spriteBatch, modPlayer, screenPos);
            }
            else
            {
                bool changed = false;
                if (CalamityConfig.Instance.StealthMeterPosX != screenRatioPosition.X)
                {
                    CalamityConfig.Instance.StealthMeterPosX = screenRatioPosition.X;
                    changed = true;
                }
                if (CalamityConfig.Instance.StealthMeterPosY != screenRatioPosition.Y)
                {
                    CalamityConfig.Instance.StealthMeterPosY = screenRatioPosition.Y;
                    changed = true;
                }

                if (changed)
                    CalamityMod.SaveConfig(CalamityConfig.Instance);
            }

            Rectangle mouseHitbox = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 8, 8);
            Rectangle stealthBar = Utils.CenteredRectangle(screenPos, edgeTexture.Size() * uiScale);

            MouseState ms = Mouse.GetState();
            Vector2 mousePos = Main.MouseScreen;

            // Handle mouse dragging
            if (stealthBar.Intersects(mouseHitbox))
            {
                if (!CalamityConfig.Instance.MeterPosLock)
                    Main.LocalPlayer.mouseInterface = true;

                // If the mouse is on top of the meter, show the player's exact numeric stealth.
                if (modPlayer.rogueStealthMax > 0f && modPlayer.stealthUIAlpha >= 0.5f)
                {
                    string stealthStr = (100f * modPlayer.rogueStealth).ToString("n2");
                    string maxStealthStr = (100f * modPlayer.rogueStealthMax).ToString("n2");
                    string textToDisplay = $"{CalamityUtils.GetTextValue("UI.Stealth")}: {stealthStr}/{maxStealthStr}\n";

                    if (!Main.keyState.IsKeyDown(LeftShift))
                    {
                        textToDisplay += CalamityUtils.GetTextValue("UI.StealthShiftText");
                    }
                    else
                    {
                        textToDisplay += CalamityUtils.GetTextValue("UI.StealthInfoText");
                    }

                    Main.instance.MouseText(textToDisplay, 0, 0, -1, -1, -1, -1);
                    modPlayer.stealthUIAlpha = MathHelper.Lerp(modPlayer.stealthUIAlpha, 0.25f, 0.035f);
                }

                Vector2 newScreenRatioPosition = screenRatioPosition;
                // As long as the mouse button is held down, drag the meter along with an offset.
                if (!CalamityConfig.Instance.MeterPosLock && ms.LeftButton == ButtonState.Pressed)
                {
                    // If the drag offset doesn't exist yet, create it.
                    if (!dragOffset.HasValue)
                        dragOffset = mousePos - screenPos;

                    // Given the mouse's absolute current position, compute where the corner of the stealth bar should be based on the original drag offset.
                    Vector2 newCorner = mousePos - dragOffset.GetValueOrDefault(Vector2.Zero);

                    // Convert the new corner position into a screen ratio position.
                    newScreenRatioPosition.X = (100f * newCorner.X) / Main.screenWidth;
                    newScreenRatioPosition.Y = (100f * newCorner.Y) / Main.screenHeight;
                }

                // Compute the change in position. If it is large enough, actually move the meter
                Vector2 delta = newScreenRatioPosition - screenRatioPosition;
                if (Math.Abs(delta.X) >= MouseDragEpsilon || Math.Abs(delta.Y) >= MouseDragEpsilon)
                {
                    CalamityConfig.Instance.StealthMeterPosX = newScreenRatioPosition.X;
                    CalamityConfig.Instance.StealthMeterPosY = newScreenRatioPosition.Y;
                }

                // When the mouse is released, save the config and destroy the drag offset.
                if (ms.LeftButton == ButtonState.Released)
                {
                    dragOffset = null;
                    CalamityMod.SaveConfig(CalamityConfig.Instance);
                }
            }
        }

        private static void DrawStealthBar(SpriteBatch spriteBatch, CalamityPlayer modPlayer, Vector2 screenPos)
        {
            float uiScale = Main.UIScale;
            float offset = (edgeTexture.Width - barTexture.Width) * 0.5f;
            spriteBatch.Draw(edgeTexture, screenPos, null, Color.White * modPlayer.stealthUIAlpha, 0f, edgeTexture.Size() * 0.5f, uiScale, SpriteEffects.None, 0);

            // If SS is available, display the explicit indication thereof
            if (modPlayer.StealthStrikeAvailable())
                spriteBatch.Draw(indicatorTexture, screenPos, null, Color.White * modPlayer.stealthUIAlpha, 0f, indicatorTexture.Size() * 0.5f, uiScale, SpriteEffects.None, 0);

            float completionRatio = modPlayer.rogueStealthMax <= 0f ? 0f : modPlayer.rogueStealth / modPlayer.rogueStealthMax;
            Rectangle barRectangle = new Rectangle(0, 0, (int)(barTexture.Width * completionRatio), barTexture.Width);
            bool full = (modPlayer.rogueStealthMax > 0f) && (modPlayer.rogueStealth >= modPlayer.rogueStealthMax);
            spriteBatch.Draw(full ? fullBarTexture : barTexture, screenPos + new Vector2(offset * uiScale, 0), barRectangle, Color.White * modPlayer.stealthUIAlpha, 0f, indicatorTexture.Size() * 0.5f, uiScale, SpriteEffects.None, 0);
        }
    }
}
