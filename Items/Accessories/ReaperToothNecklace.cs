﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    public class ReaperToothNecklace : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 50;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<GenericDamageClass>() += 0.15f;
            player.GetArmorPenetration<GenericDamageClass>() += 15;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SandSharkToothNecklace>().
                AddIngredient<ReaperTooth>(6).
                AddIngredient<DepthCells>(15).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }
    }
}
