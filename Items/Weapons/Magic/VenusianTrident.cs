﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class VenusianTrident : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 70;
            Item.height = 68;
            Item.damage = 256;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 9f;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.UseSound = SoundID.Item45;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<VenusianBolt>();
            Item.shootSpeed = 19f;
            Item.rare = ModContent.RarityType<PureGreen>();
        }

        public override Vector2? HoldoutOrigin() => new Vector2(15, 15);

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.InfernoFork).
                AddIngredient<RuinousSoul>(2).
                AddIngredient<TwistingNether>().
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
