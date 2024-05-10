﻿
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Tiles.FurnitureVoid
{
    public class SmoothVoidstone : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;

            CalamityUtils.MergeWithGeneral(Type);
            CalamityUtils.MergeSmoothTiles(Type);
            CalamityUtils.MergeDecorativeTiles(Type);
            CalamityUtils.MergeWithAbyss(Type);

            HitSound = SoundID.Tink;
            MineResist = 2.1f;
            AddMapEntry(new Color(27, 24, 31));
        }
        int animationFrameWidth = 288;

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, DustID.DungeonSpirit, 0f, 0f, 1, new Color(255, 255, 255), 1f);
            return false;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            int uniqueAnimationFrameX = 0;
            int xPos = i % 4;
            int yPos = j % 4;
            switch (xPos)
            {
                case 0:
                    switch (yPos)
                    {
                        case 0:
                            uniqueAnimationFrameX = 0;
                            break;
                        case 1:
                            uniqueAnimationFrameX = 0;
                            break;
                        case 2:
                            uniqueAnimationFrameX = 1;
                            break;
                        case 3:
                            uniqueAnimationFrameX = 1;
                            break;
                        default:
                            uniqueAnimationFrameX = 0;
                            break;
                    }
                    break;
                case 1:
                    switch (yPos)
                    {
                        case 0:
                            uniqueAnimationFrameX = 1;
                            break;
                        case 1:
                            uniqueAnimationFrameX = 0;
                            break;
                        case 2:
                            uniqueAnimationFrameX = 1;
                            break;
                        case 3:
                            uniqueAnimationFrameX = 1;
                            break;
                        default:
                            uniqueAnimationFrameX = 0;
                            break;
                    }
                    break;
                case 2:
                    switch (yPos)
                    {
                        case 0:
                            uniqueAnimationFrameX = 1;
                            break;
                        case 1:
                            uniqueAnimationFrameX = 0;
                            break;
                        case 2:
                            uniqueAnimationFrameX = 0;
                            break;
                        case 3:
                            uniqueAnimationFrameX = 1;
                            break;
                        default:
                            uniqueAnimationFrameX = 0;
                            break;
                    }
                    break;
                case 3:
                    switch (yPos)
                    {
                        case 0:
                            uniqueAnimationFrameX = 0;
                            break;
                        case 1:
                            uniqueAnimationFrameX = 1;
                            break;
                        case 2:
                            uniqueAnimationFrameX = 0;
                            break;
                        case 3:
                            uniqueAnimationFrameX = 1;
                            break;
                        default:
                            uniqueAnimationFrameX = 0;
                            break;
                    }
                    break;
            }
            frameXOffset = uniqueAnimationFrameX * animationFrameWidth;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int xPos = Main.tile[i, j].TileFrameX;
            int yPos = Main.tile[i, j].TileFrameY;
            int xOffset = 0;
            int relativeXPos = i % 4;
            int relativeYPos = j % 4;
            switch (relativeXPos)
            {
                case 0:
                    switch (relativeYPos)
                    {
                        case 0:
                            xOffset = 0;
                            break;
                        case 1:
                            xOffset = 0;
                            break;
                        case 2:
                            xOffset = 1;
                            break;
                        case 3:
                            xOffset = 1;
                            break;
                        default:
                            xOffset = 0;
                            break;
                    }
                    break;
                case 1:
                    switch (relativeYPos)
                    {
                        case 0:
                            xOffset = 1;
                            break;
                        case 1:
                            xOffset = 0;
                            break;
                        case 2:
                            xOffset = 1;
                            break;
                        case 3:
                            xOffset = 1;
                            break;
                        default:
                            xOffset = 0;
                            break;
                    }
                    break;
                case 2:
                    switch (relativeYPos)
                    {
                        case 0:
                            xOffset = 1;
                            break;
                        case 1:
                            xOffset = 0;
                            break;
                        case 2:
                            xOffset = 0;
                            break;
                        case 3:
                            xOffset = 1;
                            break;
                        default:
                            xOffset = 0;
                            break;
                    }
                    break;
                case 3:
                    switch (relativeYPos)
                    {
                        case 0:
                            xOffset = 0;
                            break;
                        case 1:
                            xOffset = 1;
                            break;
                        case 2:
                            xOffset = 0;
                            break;
                        case 3:
                            xOffset = 1;
                            break;
                        default:
                            xOffset = 0;
                            break;
                    }
                    break;
            }
            xOffset *= 288;
            xPos += xOffset;
            Texture2D glowmask = ModContent.Request<Texture2D>("CalamityMod/Tiles/FurnitureVoid/SmoothVoidstoneGlow").Value;
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawOffset = new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y) + zero;
            Color drawColour = GetDrawColour(i, j, new Color(255, 255, 255, 255));
            Tile trackTile = Main.tile[i, j];
            float brightness = 1f;
            float declareThisHereToPreventRunningTheSameCalculationMultipleTimes = Main.GameUpdateCount * 0.007f;
            brightness *= (float)MathF.Sin(i / 18f + declareThisHereToPreventRunningTheSameCalculationMultipleTimes);
            brightness *= (float)MathF.Sin(j / 18f + declareThisHereToPreventRunningTheSameCalculationMultipleTimes);
            brightness *= (float)MathF.Sin(i * 18f + declareThisHereToPreventRunningTheSameCalculationMultipleTimes);
            brightness *= (float)MathF.Sin(j * 18f + declareThisHereToPreventRunningTheSameCalculationMultipleTimes);
            drawColour *= brightness;

            TileFraming.SlopedGlowmask(i, j, 0, glowmask, drawOffset, null, GetDrawColour(i, j, drawColour), default);
        }

        private Color GetDrawColour(int i, int j, Color colour)
        {
            int colType = Main.tile[i, j].TileColor;
            Color paintCol = WorldGen.paintColor(colType);
            if (colType >= 13 && colType <= 24)
            {
                colour.R = (byte)(paintCol.R / 255f * colour.R);
                colour.G = (byte)(paintCol.G / 255f * colour.G);
                colour.B = (byte)(paintCol.B / 255f * colour.B);
            }
            return colour;
        }
    }
}
