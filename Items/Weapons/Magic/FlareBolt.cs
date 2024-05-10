﻿using CalamityMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class FlareBolt : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 38;
            Item.damage = 36;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 12;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FlareBoltProjectile>();
            Item.shootSpeed = 7.5f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.HellstoneBar, 6).
                AddIngredient(ItemID.Obsidian, 9).
                AddIngredient(ItemID.Fireblossom, 2).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
