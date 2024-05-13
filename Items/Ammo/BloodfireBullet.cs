﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Ammo
{
    public class BloodfireBullet : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Ammo";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 30;
            Item.damage = 27;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 4.5f;
            Item.value = Item.sellPrice(copper: 24);
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.shoot = ModContent.ProjectileType<BloodfireBulletProj>();
            Item.shootSpeed = 0.1f;
            Item.ammo = ItemID.MusketBall;
        }

        public override void AddRecipes()
        {
            CreateRecipe(333).
                AddIngredient<BloodstoneCore>().
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
