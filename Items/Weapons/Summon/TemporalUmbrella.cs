﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Summon.Umbrella;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Summon
{
    [LegacyName("BensUmbrella")]
    public class TemporalUmbrella : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Summon";
        public override void SetDefaults()
        {
            Item.width = 74;
            Item.height = 72;
            Item.damage = 193;
            Item.knockBack = 1f;
            Item.mana = 99;
            Item.useTime = Item.useAnimation = 10;
            Item.DamageType = DamageClass.Summon;
            Item.shootSpeed = 0f;
            Item.shoot = ModContent.ProjectileType<MagicHat>();

            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item68;
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().devItem = true;
        }

        public override bool CanUseItem(Player player) => player.maxMinions >= 5;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityUtils.KillShootProjectileMany(player, type, ModContent.ProjectileType<MagicArrow>(), ModContent.ProjectileType<MagicHammer>(), ModContent.ProjectileType<MagicAxe>(), ModContent.ProjectileType<MagicUmbrella>(), ModContent.ProjectileType<MagicRifle>());
            int p = Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
            if (Main.projectile.IndexInRange(p))
                Main.projectile[p].originalDamage = Item.damage;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ViridVanguard>().
                AddIngredient<SarosPossession>().
                AddIngredient(ItemID.Umbrella).
                AddIngredient(ItemID.TopHat).
                AddIngredient<ShadowspecBar>(5).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
