﻿using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class RelicofRuin : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 40;
            Item.damage = 21;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 16;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4.25f;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item84;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ForbiddenAxeBlade>();
            Item.shootSpeed = 5f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int totalProjectiles = 12;
            float radians = MathHelper.TwoPi / totalProjectiles;
            for (int i = 0; i < totalProjectiles; i++)
            {
                Vector2 vector = new Vector2(0f, -Item.shootSpeed).RotatedBy(radians * i);
                Projectile.NewProjectile(source, position, vector, type, damage, knockback, Main.myPlayer);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SpellTome).
                AddRecipeGroup("AnyAdamantiteBar", 5).
                AddIngredient(ItemID.AncientBattleArmorMaterial, 2).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
