﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class Tumbleweed : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 36;
            Item.damage = 125;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 8f;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<TumbleweedFlail>();
            Item.shootSpeed = 12f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Sunfury).
                AddIngredient<GrandScale>().
                AddIngredient(ItemID.SoulofMight, 5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
