﻿using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class TerrorTalons : RogueWeapon
    {
        private float sign = 1f;

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 24;
            Item.damage = 52;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = Item.useTime = 7;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item39;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityLightPurpleBuyPrice;
            Item.rare = ItemRarityID.LightPurple;
            Item.shoot = ModContent.ProjectileType<TalonSmallProj>();
            Item.shootSpeed = 10.5f;
            Item.DamageType = RogueDamageClass.Instance;
        }

        public override float StealthDamageMultiplier => 9f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<TalonLargeProj>(), damage, knockback, player.whoAmI);
                if (stealth.WithinBounds(Main.maxProjectiles))
                    Main.projectile[stealth].Calamity().stealthStrike = true;
            }
            else
            {
                // flip flop every standard projectile
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<TalonSmallProj>(), damage, knockback, player.whoAmI, 0f, sign);
                sign = -sign;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ChlorophyteBar, 5).
                AddIngredient(ItemID.Leather, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
