using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Capture;

namespace CalamityMod.Tiles.FurnitureAshen
{
	public class SmoothBrimstoneSlag : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			soundType = 21;
			mineResist = 5f;
			minPick = 180;
			drop = mod.ItemType("SmoothBrimstoneSlag");
			AddMapEntry(new Color(61, 40, 61));
		}

		public override bool CreateDust(int i, int j, ref int type)
		{
			Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, 60, 0f, 0f, 1, new Color(255, 255, 255), 1f);
			Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, 1, 0f, 0f, 1, new Color(100, 100, 100), 1f);
			return false;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (!Lighting.NotRetro)
			{
				return;
			}
			int xPos = Main.tile[i, j].frameX;
			int yPos = Main.tile[i, j].frameY;
			Texture2D glowmask = mod.GetTexture("Tiles/FurnitureAshen/SmoothBrimstoneSlag_Glowmask");
			//Initialize the default draw offset of the post drawn sections, then update it to not have the 4 tile offset if camera mode is enabled
			Vector2 drawOffset = new Vector2(i * 16 - Main.screenPosition.X + GetDrawOffset(), j * 16 - Main.screenPosition.Y + GetDrawOffset());
			if (CaptureManager.Instance.IsCapturing)
			{
				drawOffset = new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y);
			}
			spriteBatch.Draw
							 (
							 glowmask,
							 drawOffset,
							 new Rectangle(xPos, yPos, 18, 18),
							 new Color(25, 25, 25, 25),
							 0,
							 new Vector2(0f, 0f),
							 1,
							 SpriteEffects.None,
							 0f
							 );
		}

		/// <summary>
		/// Gets the offset in both axes that should be used for drawing the additions to the tile
		/// </summary>
		/// <returns>The pixel draw offset of the postdrawn sprite in both axes</returns>
		private int GetDrawOffset()
		{
			int drawOffset = 0;
			if (Main.screenWidth < 1664f)
			{
				drawOffset = 193;
			}
			else
			{
				drawOffset = (int)(-0.5f * (float)Main.screenWidth + 1025f);
			}
			return (drawOffset - 1);
		}
	}
}
