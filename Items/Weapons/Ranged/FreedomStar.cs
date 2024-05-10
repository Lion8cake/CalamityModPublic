﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class FreedomStar : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        // This is the amount of charge consumed every time the holdout projectile fires various projectiles.
        public const float HoldoutChargeUse_Orb = 0.004f;
        public const float HoldoutChargeUse_OrbLarge = 0.005f;
        public const float HoldoutChargeUse_Laser = 0.006f;
        public const float HoldoutChargeUse_LaserLarge = 0.01f;

        public override void SetDefaults()
        {
            CalamityGlobalItem modItem = Item.Calamity();
            Item.width = 54;
            Item.height = 28;
            Item.damage = 100;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.knockBack = 3f;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            modItem.donorItem = true;
            Item.UseSound = SoundID.Item75;
            Item.shoot = ModContent.ProjectileType<FreedomStarHoldout>();
            Item.shootSpeed = 12f;
            modItem.UsesCharge = true;
            modItem.MaxCharge = 150f;
            modItem.ChargePerUse = 0f; // This weapon is a holdout. Charge is consumed by the holdout projectile.
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 shootVelocity = velocity;
            Vector2 shootDirection = shootVelocity.SafeNormalize(Vector2.UnitX * player.direction);
            // Charge-up. Done via a holdout projectile.
            Projectile.NewProjectile(source, position, shootDirection, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LunarBar, 4).
                AddIngredient<UelibloomBar>(8).
                AddIngredient<MysteriousCircuitry>(12).
                AddIngredient<DubiousPlating>(18).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
