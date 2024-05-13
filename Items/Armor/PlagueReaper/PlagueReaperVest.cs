﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor.PlagueReaper
{
    [AutoloadEquip(EquipType.Body)]
    public class PlagueReaperVest : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.Hardmode";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<RangedDamageClass>() += 0.15f;
            player.GetCritChance<RangedDamageClass>() += 5;
            player.buffImmune[ModContent.BuffType<Plague>()] = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.NecroBreastplate).
                AddIngredient<PlagueCellCanister>(29).
                AddIngredient(ItemID.Nanites, 19).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
