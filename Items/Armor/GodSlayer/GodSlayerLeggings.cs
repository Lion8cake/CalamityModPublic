﻿using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor.GodSlayer
{
    [AutoloadEquip(EquipType.Legs)]
    public class GodSlayerLeggings : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.PostMoonLord";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.defense = 35;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.05f;
            player.GetDamage<GenericDamageClass>() += 0.1f;
            player.GetCritChance<GenericDamageClass>() += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CosmiliteBar>(10).
                AddIngredient<AscendantSpiritEssence>(2).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
