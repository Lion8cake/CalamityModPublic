﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class ShadowboltStaff : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 56;
            Item.damage = 280;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 8f;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.UseSound = SoundID.Item72;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Shadowbolt>();
            Item.shootSpeed = 6f;
            Item.rare = ModContent.RarityType<PureGreen>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ShadowbeamStaff).
                AddIngredient<ArmoredShell>(3).
                AddIngredient<RuinousSoul>(2).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
