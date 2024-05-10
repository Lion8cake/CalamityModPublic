﻿using CalamityMod.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    [AutoloadEquip(new EquipType[] { EquipType.HandsOn, EquipType.HandsOff })]
    public class ElectriciansGlove : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 40;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.electricianGlove = true;
            modPlayer.bloodyGlove = true;
            modPlayer.filthyGlove = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FilthyGlove>().
                AddIngredient(ItemID.Wire, 100).
                AddRecipeGroup("AnyMythrilBar", 5).
                AddTile(TileID.MythrilAnvil).
                Register();

            CreateRecipe().
                AddIngredient<BloodstainedGlove>().
                AddIngredient(ItemID.Wire, 100).
                AddRecipeGroup("AnyMythrilBar", 5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
