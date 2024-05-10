﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class ThePrince : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";
        public const int FlameSplitCount = 6;
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 102;
            Item.height = 112;
            Item.damage = 166;
            Item.knockBack = 4.25f;
            Item.shootSpeed = 23.5f;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.mana = 12;
            Item.useTime = Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = SoundID.DD2_FlameburstTowerShot;
            Item.shoot = ModContent.ProjectileType<PrinceFlameLarge>();
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.Calamity().donorItem = true;
            Item.rare = ModContent.RarityType<Turquoise>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 flameSpawnPosition = player.RotatedRelativePoint(player.MountedCenter, true);
            flameSpawnPosition += velocity.SafeNormalize(Vector2.Zero) * 105f;
            Projectile.NewProjectile(source, flameSpawnPosition, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ArchAmaryllis>().
                AddIngredient<DivineGeode>(15).
                AddIngredient<UnholyEssence>(10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
