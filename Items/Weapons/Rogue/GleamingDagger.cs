﻿using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class GleamingDagger : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 26;
            Item.damage = 14;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<GleamingDaggerProj>();
            Item.shootSpeed = 15f;
            Item.DamageType = RogueDamageClass.Instance;
        }

        public override float StealthDamageMultiplier => 2.8f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, 1f);
                if (p.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[p].Calamity().stealthStrike = true;
                    Main.projectile[p].penetrate = 4;
                }
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PlatinumBar, 12).
                AddIngredient(ItemID.ThrowingKnife, 250).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
