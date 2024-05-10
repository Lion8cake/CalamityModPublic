﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Typeless;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Tools
{
    [LegacyName("WulfrumPickaxe")]
    public class WulfrumDrill : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Tools";
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 38;
            Item.damage = 5;
            Item.knockBack = 0f;
            Item.useTime = 5;
            Item.useAnimation = 16;
            Item.pick = 35;
            Item.tileBoost += 1;

            Item.DamageType = DamageClass.Melee;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item23;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<WulfrumDrillProj>();

            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Shoot;
        }

        public override void HoldItem(Player player)
        {
            player.Calamity().mouseWorldListener = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<WulfrumMetalScrap>(8).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
