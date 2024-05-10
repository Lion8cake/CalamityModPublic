﻿using CalamityMod.Buffs.Pets;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Pets
{
    [LegacyName("RotomRemote")]
    public class TheEtomer : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Pets";
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.damage = 0;
            Item.useTime = Item.useAnimation = 20;
            Item.shoot = ModContent.ProjectileType<RotomPet>();
            Item.buffType = ModContent.BuffType<ElectricTroublemaker>();

            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item113;

            Item.value = Item.buyPrice(gold: 4);
            Item.rare = ItemRarityID.Orange;
            Item.Calamity().devItem = true;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityMod/Items/Pets/TheEtomerGlow").Value);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PrismShard>(5).
                AddRecipeGroup("AnyGoldBar", 8).
                AddIngredient<DemonicBoneAsh>().
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
