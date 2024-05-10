﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor.GodSlayer
{
    [AutoloadEquip(EquipType.Body)]
    public class GodSlayerChestplate : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.PostMoonLord";
        public const int DashIFrames = 12;

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.defense = 41;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.Calamity();
            player.thorns += 0.5f;
            player.statLifeMax2 += 60;
            player.GetDamage<GenericDamageClass>() += 0.11f;
            player.GetCritChance<GenericDamageClass>() += 6;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CosmiliteBar>(15).
                AddIngredient<AscendantSpiritEssence>(3).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
