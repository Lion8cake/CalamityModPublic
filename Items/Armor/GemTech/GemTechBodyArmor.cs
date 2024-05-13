﻿using System.Collections.Generic;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor.GemTech
{
    [AutoloadEquip(EquipType.Body)]
    public class GemTechBodyArmor : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.PostMoonLord";
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 32;
            Item.defense = 31;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.Calamity().donorItem = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) => GemTechHeadgear.ModifySetTooltips(this, tooltips);
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ExoPrism>(16)
                .AddIngredient<GalacticaSingularity>(5)
                .AddIngredient<CoreofCalamity>(2)
                .AddTile<DraedonsForge>()
                .Register();
        }
    }
}
