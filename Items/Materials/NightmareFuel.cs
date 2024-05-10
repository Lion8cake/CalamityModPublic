﻿using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Materials
{
    public class NightmareFuel : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Materials";
        public int frameCounter = 0;
        public int frame = 0;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.SortingPriorityMaterials[Type] = 115;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 36;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            spriteBatch.Draw(texture, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, 6, 6), lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CalamityMod/Items/Materials/NightmareFuelGlow").Value;
            spriteBatch.Draw(texture, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, 6, 6, false), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float brightness = Main.essScale * Main.rand.NextFloat(0.9f, 1.1f);
            Lighting.AddLight(Item.Center, 0.7f * brightness, 0.7f * brightness, 0f);
        }
    }
}
