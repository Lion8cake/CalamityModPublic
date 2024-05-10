﻿using CalamityMod.Items.Potions;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.TreasureBags.MiscGrabBags
{
    public class SulphuricTreasure : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.TreasureBags";
        internal static readonly int[] SulphuricTreasurePotions = new int[]
        {
            ItemID.SpelunkerPotion,
            ItemID.MagicPowerPotion,
            ItemID.ShinePotion,
            ItemID.WaterWalkingPotion,
            ItemID.ObsidianSkinPotion,
            ItemID.WaterWalkingPotion,
            ItemID.GravitationPotion,
            ItemID.RegenerationPotion,
            ModContent.ItemType<AnechoicCoating>(),
            ItemID.GillsPotion,
            ItemID.EndurancePotion,
            ItemID.HeartreachPotion,
            ItemID.FlipperPotion,
            ItemID.LifeforcePotion,
            ItemID.InfernoPotion
        };

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Green; //Green for thematics
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.GoodieBags;
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // 1/15 chance for potions
            var oneInFifteenPotions = itemLoot.Add(new OneFromOptionsNotScaledWithLuckDropRule(15, 1, SulphuricTreasurePotions));

            // 1/30 chance for a Wormhole Potion in multiplayer only
            IItemDropRule wormholePotion = ItemDropRule.ByCondition(DropHelper.If(() => Main.netMode == NetmodeID.MultiplayerClient), ItemID.WormholePotion, 30);
            LeadingConditionRule singlePlayer = new LeadingConditionRule(DropHelper.If(() => Main.netMode != NetmodeID.MultiplayerClient));

            // IF YOU DONT GET POTIONS
            // 10% chance for 4-8 Sticky Glowsticks
            // 10% chance for 10-20 Jester Arrows
            // 10% chance for 1 Hadal Stew
            // 10% chance for 5-7 BOMBS?!
            // 0% chance for Lamp Oil (no item in game)
            // 60% chance for 40-60 Silver Coins

            // 4-8 Glowsticks
            CommonDrop stickyGlowsticks = new CommonDrop(ItemID.StickyGlowstick, 1, 4, 8);
            // 10-20 Jester Arrows
            CommonDrop jesterArrows = new CommonDrop(ItemID.JestersArrow, 1, 10, 20);
            // 1 Hadal Stew
            CommonDrop hadalStew = new CommonDrop(ModContent.ItemType<HadalStew>(), 1);
            // 5-7 Sticky Bombs
            CommonDrop stickyBombs = new CommonDrop(ItemID.StickyBomb, 1, 5, 7);
            // 40-60 Silver Coin
            CommonDrop silver = new CommonDrop(ItemID.SilverCoin, 1, 40, 60);

            OneFromRulesRule otherDrops = new OneFromRulesRule(1, new IItemDropRule[] { stickyGlowsticks, jesterArrows, hadalStew, stickyBombs, silver, silver, silver, silver, silver, silver });

            oneInFifteenPotions.OnFailedRoll(wormholePotion).OnFailedRoll(otherDrops);
            oneInFifteenPotions.OnFailedRoll(singlePlayer).OnSuccess(otherDrops);
        }
    }
}
