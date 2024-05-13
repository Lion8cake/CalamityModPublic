﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Summon
{
    public class WarloksMoonFist : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Summon";
        public const int SlotCount = 4;

        public const int PunchCooldownTime = 36;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 450;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ItemRarityID.Purple;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.UseSound = SoundID.Item104;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<MoonFist>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
            Item.Calamity().donorItem = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                int existingFists = player.ownedProjectileCounts[type];
                int p = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
                if (Main.projectile.IndexInRange(p))
                {
                    Main.projectile[p].originalDamage = Item.damage;
                    Main.projectile[p].ai[0] = existingFists;
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.GolemFist).
                AddIngredient<Lumenyl>(10).
                AddIngredient<RuinousSoul>(5).
                AddIngredient<ExodiumCluster>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
