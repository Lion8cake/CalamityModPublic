﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor.Silva
{
    [AutoloadEquip(EquipType.Legs)]
    public class SilvaLeggings : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.PostMoonLord";
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.defense = 39;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.1f;
            player.GetDamage<GenericDamageClass>() += 0.12f;
            player.GetCritChance<GenericDamageClass>() += 12;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PlantyMush>(9).
                AddIngredient<EffulgentFeather>(7).
                AddIngredient<AscendantSpiritEssence>(2).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
