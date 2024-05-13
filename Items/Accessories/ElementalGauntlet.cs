﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    [AutoloadEquip(new EquipType[] { EquipType.HandsOn, EquipType.HandsOff })]
    public class ElementalGauntlet : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 38;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.eGauntlet = true;
            player.kbGlove = true;
            player.autoReuseGlove = true;
            player.meleeScaleGlove = true;
            modPlayer.gloveLevel = 5;
            player.GetDamage<TrueMeleeDamageClass>() += 0.10f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.FireGauntlet).
                AddIngredient(ItemID.LunarBar, 8).
                AddIngredient<GalacticaSingularity>(4).
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
