﻿using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor.Aerospec
{
    [AutoloadEquip(EquipType.Legs)]
    public class AerospecLeggings : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.PreHardmode";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.12f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AerialiteBar>(7).
                AddIngredient(ItemID.SunplateBlock, 4).
                AddIngredient(ItemID.Feather, 2).
                AddTile(TileID.SkyMill).
                Register();
        }
    }
}
