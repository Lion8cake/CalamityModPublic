﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class AstrealDefeat : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 78;
            Item.damage = 153;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 4;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = CalamityGlobalItem.RarityPurpleBuyPrice;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item102;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AstrealArrow>();
            Item.shootSpeed = 4f;
            Item.useAmmo = AmmoID.Arrow;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            float speed = velocity.Length();
            if (speed > 8f)
                velocity *= 8f / speed;

            // Always fires Astreal Arrows, regardless of ammo chosen.
            // Normally we like to allow bows to fire normal arrows but this weapon is incredibly overpowered when that is allowed.
            type = Item.shoot;
            float aiVar = Main.rand.Next(4);

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, aiVar);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SpiritFlame).
                AddIngredient(ItemID.ShadowFlameBow).
                AddIngredient(ItemID.LunarBar, 5).
                AddIngredient<AshesofCalamity>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
