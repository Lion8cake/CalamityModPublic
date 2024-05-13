﻿using CalamityMod.Items.Placeables.FurnitureOtherworldly;
using Terraria.ID;
using Terraria.ModLoader;
using WallTiles = CalamityMod.Walls;
namespace CalamityMod.Items.Placeables.Walls
{
    [LegacyName("OccultStoneWall")]
    public class OtherworldlyStoneWall : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placeables";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 7;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createWall = ModContent.WallType<WallTiles.OtherworldlyStoneWall>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(4).
                AddIngredient<OtherworldlyStone>().
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
