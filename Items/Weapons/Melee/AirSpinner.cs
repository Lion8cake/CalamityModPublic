﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee.Yoyos;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class AirSpinner : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = 26;
            Item.knockBack = 4f;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<AirSpinnerYoyo>();
            Item.shootSpeed = 14f;

            Item.rare = ItemRarityID.Orange;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AerialiteBar>(6).
                AddIngredient(ItemID.SunplateBlock, 3).
                AddTile(TileID.SkyMill).
                Register();
        }
    }
}
