﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class Toxibow : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 54;
            Item.damage = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.Arrow;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CalamityUtils.CheckWoodenAmmo(type, player))
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ToxicArrow>(), damage, 0f, player.whoAmI);
            else
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Acidwood>(20).
                AddIngredient<SulphuricScale>(15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
