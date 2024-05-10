﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Ammo
{
    [LegacyName("AcidBullet", "AcidRound")]
    public class BubonicRound : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Ammo";
        public override void SetStaticDefaults() => Item.ResearchUnlockCount = 99;

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 8;
            Item.damage = 16;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 1.5f;
            Item.value = Item.sellPrice(copper: 16);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<BubonicRoundProj>();
            Item.shootSpeed = 10f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            CreateRecipe(150).
                AddIngredient(ItemID.MusketBall, 150).
                AddIngredient<PlagueCellCanister>().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
