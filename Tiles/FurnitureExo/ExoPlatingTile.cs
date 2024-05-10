﻿using CalamityMod.Items.Placeables.FurnitureExo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Tiles.FurnitureExo
{
    public class ExoPlatingTile : ModTile
    {
        internal static Texture2D GlowTexture;

        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GlowTexture = ModContent.Request<Texture2D>("CalamityMod/Tiles/FurnitureExo/ExoPlatingTileGlow", AssetRequestMode.ImmediateLoad).Value;
            }

            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            CalamityUtils.MergeWithGeneral(Type);
            CalamityUtils.MergeDecorativeTiles(Type);

            MineResist = 3f;
            HitSound = SoundID.Tink;
            AddMapEntry(new Color(52, 67, 78));
            AnimationFrameHeight = 90;
        }

        public override bool CanExplode(int i, int j) => false;

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, DustID.TerraBlade, 0f, 0f, 1, new Color(255, 255, 255), 1f);
            return false;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            int yPos = j % 2;
            frameYOffset = yPos * AnimationFrameHeight;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            // If the cached textures don't exist for some reason, don't bother using them.
            if (GlowTexture is null)
                return;

            Tile tile = CalamityUtils.ParanoidTileRetrieval(i, j);
            int xPos = tile.TileFrameX;
            int frameOffset = j % 2 * AnimationFrameHeight;
            int yPos = tile.TileFrameY + frameOffset;
            Color drawColour = GetDrawColour(i, j, Color.White);
            Vector2 drawOffset = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawPosition = new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y) + drawOffset;

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
