﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Placeables.Plates
{
    public class Elumplate : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placeables";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<Onyxplate>();
        }

        public override void SetDefaults()
        {
            Item.width = 13;
            Item.height = 10;
            Item.createTile = ModContent.TileType<Tiles.Plates.Elumplate>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 3);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            CreateRecipe(25).
                AddIngredient(ItemID.Obsidian, 25).
                AddIngredient<EssenceofEleum>().
                AddTile(TileID.Hellforge).
                Register();
            CreateRecipe().
                AddIngredient<ElumplateWall>(4).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
