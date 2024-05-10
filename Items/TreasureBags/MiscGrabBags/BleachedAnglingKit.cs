﻿using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.TreasureBags.MiscGrabBags
{
    public class BleachedAnglingKit : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.TreasureBags";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<SandyAnglingKit>();
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Pink;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.GoodieBags;
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Different drop rates on Normal and Expert, so define normal first, then expert
            var normalOnly = itemLoot.DefineNormalOnlyDropSet();
            normalOnly.Add(ItemID.FishermansGuide, 18);
            normalOnly.Add(ItemID.WeatherRadio, 18);
            normalOnly.Add(ItemID.Sextant, 18);
            normalOnly.Add(ItemID.FishingPotion, 3, 2, 3);
            normalOnly.Add(ItemID.SonarPotion, 3, 2, 3);
            normalOnly.Add(ItemID.CratePotion, 3, 2, 3);
            normalOnly.Add(ItemID.GoldCoin, 1, 3, 4);

            var expertPlus = itemLoot.DefineConditionalDropSet(new Conditions.IsExpert());
            expertPlus.Add(ItemID.FishermansGuide, 14);
            expertPlus.Add(ItemID.WeatherRadio, 14);
            expertPlus.Add(ItemID.Sextant, 14);
            expertPlus.Add(ItemID.FishingPotion, 2, 2, 3);
            expertPlus.Add(ItemID.SonarPotion, 2, 2, 3);
            expertPlus.Add(ItemID.CratePotion, 2, 2, 3);
            expertPlus.Add(ItemID.GoldCoin, 1, 4, 5);
        }
    }
}
