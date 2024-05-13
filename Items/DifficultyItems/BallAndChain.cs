﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.DifficultyItems
{
    public class BallAndChain : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Misc";
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 50;
            Item.rare = ItemRarityID.Blue;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = (ContentSamples.CreativeHelper.ItemGroup)CalamityResearchSorting.ToolsOther;
        }

        public override bool CanUseItem(Player player) => false;

        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
                player.Calamity().blockAllDashes = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("IronBar", 10).
                AddIngredient(ItemID.Chain).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
