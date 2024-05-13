﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor.Hydrothermic
{
    [AutoloadEquip(EquipType.Head)]
    [LegacyName("AtaxiaHeadgear")]
    public class HydrothermicHeadRanged : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.Hardmode";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 15; //53
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HydrothermicArmor>() && legs.type == ModContent.ItemType<HydrothermicSubligar>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
            player.Calamity().hydrothermalSmoke = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus") + "\n" + CalamityUtils.GetTextValueFromModItem<HydrothermicArmor>("CommonSetBonus");
            var modPlayer = player.Calamity();
            modPlayer.ataxiaBlaze = true;
            modPlayer.ataxiaBolt = true;
            player.GetDamage<RangedDamageClass>() += 0.05f;
        }

        public override void UpdateEquip(Player player)
        {
            player.ammoCost75 = true;
            player.GetDamage<RangedDamageClass>() += 0.12f;
            player.GetCritChance<RangedDamageClass>() += 10;
            player.lavaImmune = true;
            player.buffImmune[BuffID.OnFire] = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ScoriaBar>(7).
                AddIngredient<CoreofHavoc>().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
