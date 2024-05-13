﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor.Fearmonger
{
    [AutoloadEquip(EquipType.Head)]
    public class FearmongerGreathelm : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.PostMoonLord";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.defense = 38; // 132 total
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 60;
            player.GetDamage<SummonDamageClass>() += 0.1f;
            player.manaCost *= 0.9f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<FearmongerPlateMail>() && legs.type == ModContent.ItemType<FearmongerGreaves>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus");

            // This bool encompasses cross-class nerf immunity, colossal life regen on minion attack, and the holiday moon DR
            // TODO -- Fearmonger life regen from minion attacks needs some sort of cool visual effect
            player.Calamity().fearmongerSet = true;

            // All-class armors count as rogue sets, but don't grant stealth bonuses
            player.Calamity().wearingRogueArmor = true;
            player.Calamity().WearingPostMLSummonerSet = true;
            player.GetDamage<SummonDamageClass>() += 0.2f;
            player.maxMinions += 2;

            int[] immuneDebuffs = {
                BuffID.OnFire,
                BuffID.Frostburn,
                BuffID.CursedInferno,
                BuffID.ShadowFlame, //doesn't do anything
                BuffID.Daybreak, //doesn't do anything
                BuffID.Burning,
                ModContent.BuffType<Shadowflame>(),
                ModContent.BuffType<BrimstoneFlames>(),
                ModContent.BuffType<HolyFlames>(),
                ModContent.BuffType<GodSlayerInferno>(),
                BuffID.Chilled,
                BuffID.Frozen,
                ModContent.BuffType<GlacialState>(),
            };
            for (var i = 0; i < immuneDebuffs.Length; ++i)
                player.buffImmune[immuneDebuffs[i]] = true;

            // Constantly emit dim orange light
            Lighting.AddLight(player.Center, 0.3f, 0.18f, 0f);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SpookyHelmet).
                AddIngredient<CosmiliteBar>(8).
                AddIngredient(ItemID.SoulofFright, 8).
                AddIngredient<AscendantSpiritEssence>(2).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
