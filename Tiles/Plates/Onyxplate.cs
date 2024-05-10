﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Tiles.Plates
{
    public class Onyxplate : ModTile
    {
        public static readonly SoundStyle MinePlatingSound = new("CalamityMod/Sounds/Custom/PlatingMine", 3);
        internal static Texture2D GlowTexture;
        internal static Texture2D PulseTexture;
        internal static Color[] PulseColors;
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                PulseTexture = ModContent.Request<Texture2D>("CalamityMod/Tiles/Plates/OnyxplatePulse", AssetRequestMode.ImmediateLoad).Value;
                PulseColors = new Color[PulseTexture.Width];
                Main.QueueMainThreadAction(() => PulseTexture.GetData(PulseColors));
                GlowTexture = ModContent.Request<Texture2D>("CalamityMod/Tiles/Plates/OnyxplateGlow", AssetRequestMode.ImmediateLoad).Value;
            }
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;

            CalamityUtils.MergeWithGeneral(Type);

            HitSound = MinePlatingSound;
            MineResist = 1f;
            DustType = 173;
            AddMapEntry(new Color(182, 28, 232));
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            // If the cached textures don't exist for some reason, don't bother using them.
            if (GlowTexture is null || PulseTexture is null)
                return;

            int xPos = Main.tile[i, j].TileFrameX;
            int yPos = Main.tile[i, j].TileFrameY;
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawOffset = new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y) + zero;

            // Glowmask 'pulse' effect
            int factor = (int)Main.GameUpdateCount % PulseTexture.Width;
            float brightness = PulseColors[factor].R / 255f;
            int drawBrightness = (int)(80 * brightness) + 10;
            Color drawColour = GetDrawColour(i, j, new Color(drawBrightness, drawBrightness, drawBrightness, drawBrightness));

            // If these tiles cause lag, comment out the pulse effect code and uncomment this:
            //Color drawColour = GetDrawColour(i, j, new Color(50, 50, 50, 50));

            Tile trackTile = Main.tile[i, j];
            TileFraming.SlopedGlowmask(i, j, 0, GlowTexture, drawOffset, null, GetDrawColour(i, j, drawColour), default);
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
