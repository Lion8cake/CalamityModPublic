﻿using CalamityMod.Items.Materials;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class DragonPow : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public static float Speed = 13f;
        public static float ReturnSpeed = 20f;
        public static float SparkSpeed = 0.6f;
        public static float MinPetalSpeed = 24f;
        public static float MaxPetalSpeed = 30f;
        public static float MinWaterfallSpeed = 12f;
        public static float MaxWaterfallSpeed = 15.5f;

        public override void SetDefaults()
        {
            Item.width = 76;
            Item.height = 82;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = 660;
            Item.knockBack = 9f;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = Yharon.ShortRoarSound;
            Item.channel = true;

            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.Calamity().donorItem = true;

            Item.shoot = ModContent.ProjectileType<DragonPowFlail>();
            Item.shootSpeed = Speed;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Mourningstar>().
                AddIngredient(ItemID.DaoofPow).
                AddIngredient(ItemID.FlowerPow).
                AddIngredient(ItemID.Flairon).
                AddIngredient<BallOFugu>().
                AddIngredient<Tumbleweed>().
                AddIngredient<UrchinFlail>().
                AddIngredient<YharonSoulFragment>(4).
                AddIngredient<AuricBar>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
