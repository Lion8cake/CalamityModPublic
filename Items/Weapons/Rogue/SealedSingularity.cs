﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class SealedSingularity : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = Item.height = 34;
            Item.damage = 260;
            Item.knockBack = 5f;
            Item.useAnimation = Item.useTime = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = RogueDamageClass.Instance;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SealedSingularityProj>();
            Item.shootSpeed = 14f;

            Item.noMelee = Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item106;

            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.Calamity().donorItem = true;
        }

        public override float StealthDamageMultiplier => 0.72f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (stealth.WithinBounds(Main.maxProjectiles))
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DuststormInABottle>().
                AddIngredient<DarkPlasma>(3).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
