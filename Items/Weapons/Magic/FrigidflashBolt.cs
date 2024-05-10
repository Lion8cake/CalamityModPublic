﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Magic;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class FrigidflashBolt : ModItem, ILocalizedModType
    {
        public static readonly SoundStyle UseSound = new("CalamityMod/Sounds/Item/FrigidflashUse");
        public static readonly SoundStyle ProjDeathSound = new("CalamityMod/Sounds/Item/FrigidflashDeath");

        public new string LocalizationCategory => "Items.Weapons.Magic";
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 42;
            Item.damage = 80;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 13;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = UseSound;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FrigidflashBoltProjectile>();
            Item.shootSpeed = 9f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FrostBolt>().
                AddIngredient<FlareBolt>().
                AddIngredient<EssenceofEleum>(2).
                AddIngredient<EssenceofHavoc>(2).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
