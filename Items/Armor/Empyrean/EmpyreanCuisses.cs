﻿using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor.Empyrean
{
    [AutoloadEquip(EquipType.Legs)]
    [LegacyName("XerocCuisses")]
    public class EmpyreanCuisses : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.PostMoonLord";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.defense = 24;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<ThrowingDamageClass>() += 5;
            player.GetDamage<ThrowingDamageClass>() += 0.05f;
            player.moveSpeed += 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MeldConstruct>(15).
                AddIngredient(ItemID.LunarBar, 12).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
