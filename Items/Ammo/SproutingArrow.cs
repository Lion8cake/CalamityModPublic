﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Ammo
{
    [LegacyName("TerraArrow")]
    public class SproutingArrow : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Ammo";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 36;
            Item.damage = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 1.5f;
            Item.value = Item.sellPrice(copper: 20);
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ModContent.ProjectileType<SproutingArrowMain>();
            Item.shootSpeed = 15f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
            CreateRecipe(250).
                AddIngredient(ItemID.WoodenArrow, 250).
                AddIngredient<LivingShard>().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
