﻿using CalamityMod.Projectiles.Typeless;
using CalamityMod.Sounds;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Typeless
{
    public class EyeofMagnus : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Typeless";
        public override void SetDefaults()
        {
            Item.width = 80;
            Item.height = 50;
            Item.DamageType = AverageDamageClass.Instance;
            Item.damage = 32;
            Item.rare = ItemRarityID.Red;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5f;
            Item.UseSound = CommonCalamitySounds.LaserCannonSound;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.shoot = ModContent.ProjectileType<MagnusBeam>();
            Item.shootSpeed = 12f;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = (ContentSamples.CreativeHelper.ItemGroup)CalamityResearchSorting.ClasslessWeapon;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<LunicEye>().
                AddIngredient(ItemID.FragmentNebula, 12).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
