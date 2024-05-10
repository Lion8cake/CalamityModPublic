﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    [LegacyName("StellarContemptMelee", "StellarContemptRogue")]
    public class StellarContempt : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public static float Speed = 25f;

        public override void SetDefaults()
        {
            Item.width = 74;
            Item.height = 74;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = 650;
            Item.knockBack = 28f;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.value = CalamityGlobalItem.RarityPurpleBuyPrice;
            Item.rare = ItemRarityID.Purple;

            Item.shoot = ModContent.ProjectileType<StellarContemptHammer>();
            Item.shootSpeed = Speed;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FallenPaladinsHammer>().
                AddIngredient(ItemID.LunarBar, 5).
                AddIngredient<GalacticaSingularity>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
