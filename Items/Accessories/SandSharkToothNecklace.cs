﻿using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    public class SandSharkToothNecklace : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 44;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<GenericDamageClass>() += 0.06f;
            player.GetArmorPenetration<GenericDamageClass>() += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SharkToothNecklace).
                AddIngredient(ItemID.AvengerEmblem).
                AddIngredient<GrandScale>().
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }
    }
}
