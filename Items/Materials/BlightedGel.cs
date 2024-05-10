﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Materials
{
    [LegacyName("EbonianGel")]
    public class BlightedGel : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Materials";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 18;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 6);
            Item.rare = ItemRarityID.Blue;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (Item.notAmmo)
            {
                Texture2D texture = ModContent.Request<Texture2D>("CalamityMod/Items/Materials/BlightedGelRed").Value;
                spriteBatch.Draw(texture, position, frame, Color.White, 0f, origin, scale, SpriteEffects.None, 0);
            }
            return !Item.notAmmo;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (Item.notAmmo)
            {
                Texture2D texture = ModContent.Request<Texture2D>("CalamityMod/Items/Materials/BlightedGelRed").Value;
                spriteBatch.Draw(texture, Item.position - Main.screenPosition, new Rectangle(0, 0, Item.width, Item.height), lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            return !Item.notAmmo;
        }
    }
}
