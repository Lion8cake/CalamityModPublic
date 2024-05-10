﻿using CalamityMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class FrostBolt : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 38;
            Item.damage = 14;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FrostBoltProjectile>();
            Item.shootSpeed = 6f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("AnyIceBlock", 20).
                AddRecipeGroup("AnySnowBlock", 10).
                AddIngredient(ItemID.Shiverthorn, 2).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
