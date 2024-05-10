﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class StatisBlessing : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.holyMinions = true;
            player.GetKnockback<SummonDamageClass>() += 2.5f;
            player.GetDamage<SummonDamageClass>() += 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PapyrusScarab).
                AddIngredient(ItemID.PygmyNecklace).
                AddIngredient(ItemID.SummonerEmblem).
                AddIngredient(ItemID.HolyWater, 30).
                AddIngredient<CoreofSunlight>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
