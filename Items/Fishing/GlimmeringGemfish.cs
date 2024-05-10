﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Fishing
{
    public class GlimmeringGemfish : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Fishing";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
            ItemID.Sets.CanBePlacedOnWeaponRacks[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 30;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 50);
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.GoodieBags;
        }

        public override bool CanRightClick() => true;
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            int gemMin = 1;
            int gemMax = 3;
            itemLoot.Add(ItemID.Amethyst, 2, gemMin, gemMax);
            itemLoot.Add(ItemID.Topaz, 2, gemMin, gemMax);
            itemLoot.Add(ItemID.Sapphire, 4, gemMin, gemMax);
            itemLoot.Add(ItemID.Emerald, 4, gemMin, gemMax);
            itemLoot.Add(ItemID.Ruby, 8, gemMin, gemMax);
            itemLoot.Add(ItemID.Diamond, 10, gemMin, gemMax);
            itemLoot.Add(ItemID.Amber, 8, gemMin, gemMax);

            // Add Thorium gems if Thorium is loaded.
            Mod thorium = CalamityMod.Instance.thorium;
            if (thorium is null)
                return;

            var aquamarine = thorium.Find<ModItem>("Aquamarine");
            if (aquamarine is not null)
                itemLoot.Add(aquamarine.Type, 4, gemMin, gemMax);
            else
                CalamityMod.Instance.Logger.Warn("Could not find Thorium Aquamarine gem. This item will not be added to Glimmering Gemfish.");

            var opal = thorium.Find<ModItem>("Opal");
            if (opal is not null)
                itemLoot.Add(opal.Type, 4, gemMin, gemMax);
            else
                CalamityMod.Instance.Logger.Warn("Could not find Thorium Opal gem. This item will not be added to Glimmering Gemfish.");
        }
    }
}
