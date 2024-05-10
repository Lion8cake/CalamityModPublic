﻿using CalamityMod.Buffs.Alcohol;
using CalamityMod.Items.Accessories.Vanity;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Potions.Alcohol
{
    public class FabsolsVodka : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Potions";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<CrystalHeartVodka>();
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.LightRed;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<FabsolVodkaBuff>();
            Item.buffTime = CalamityUtils.SecondsToFrames(900f);
            Item.value = Item.buyPrice(0, 2, 60, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Ale).
                AddIngredient(ItemID.PixieDust, 10).
                AddIngredient(ItemID.CrystalShard, 5).
                AddIngredient(ItemID.UnicornHorn).
                AddTile(TileID.Kegs).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.Ale).
                AddIngredient<BloodOrb>(40).
                AddIngredient(ItemID.CrystalShard).
                AddTile(TileID.AlchemyTable).
                Register()
                .DisableDecraft();
        }
    }
}
