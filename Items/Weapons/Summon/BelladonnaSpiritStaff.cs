﻿using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Summon
{
    public class BelladonnaSpiritStaff : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Summon";
        #region Other stats for easy modification

        public const float EnemyDistanceDetection = 1200f; // In pixels.

        public const float FireRate = 75f;  // In frames. 60 frames = 1 second.

        public const float PetalTimeBeforeTargetting = 60f;  // In frames. 60 frames = 1 second.

        public const float PetalVelocity = 20f;

        public const float PetalGravityStrenght = 0.2f;

        #endregion

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 42;
            Item.damage = 22;
            Item.knockBack = 1f;
            Item.mana = 10;

            Item.shoot = ModContent.ProjectileType<BelladonnaSpirit>();
            Item.useTime = Item.useAnimation = 35;

            Item.DamageType = DamageClass.Summon;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item44;
            Item.rare = ItemRarityID.Blue;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.noMelee = true;
            Item.autoReuse = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int belladonna = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
            if (Main.projectile.IndexInRange(belladonna))
                Main.projectile[belladonna].originalDamage = Item.damage;
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Vine, 4).
                AddIngredient(ItemID.JungleSpores, 5).
                AddIngredient(ItemID.Stinger, 8).
                AddIngredient(ItemID.RichMahogany, 25).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
