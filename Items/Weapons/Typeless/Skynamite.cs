﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Typeless;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Typeless
{
    [LegacyName("AeroDynamite")]
    public class Skynamite : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Typeless";
        public const int Damage = 250;
        public const float Knockback = 10f;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
            ItemID.Sets.ItemsThatCountAsBombsForDemolitionistToSpawn[Item.type] = true;
            ItemID.Sets.CanBePlacedOnWeaponRacks[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 28;
            Item.useTime = Item.useAnimation = 40;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.shootSpeed = 5f;
            Item.shoot = ModContent.ProjectileType<AeroExplosive>();

            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice / 10; // Crafted 10 at a time
            Item.rare = ItemRarityID.Orange;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Bombs;
        }

        // 1 dynamite at Sky Mill = cheap, available early game as an alt option to sticky dynamite, just requires floating islands
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Dynamite).
                AddTile(TileID.SkyMill).
                Register();
        }
    }
}
