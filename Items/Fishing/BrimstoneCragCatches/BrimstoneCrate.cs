﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions;
using CalamityMod.Tiles.Crags;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Fishing.BrimstoneCragCatches
{
    public class BrimstoneCrate : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Fishing";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
            ItemID.Sets.IsFishingCrate[Type] = true;
            ItemID.Sets.IsFishingCrateHardmode[Type] = true;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<SlagCrate>();
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
            Item.createTile = ModContent.TileType<BrimstoneCrateTile>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Crates;
        }

        public override bool CanRightClick() => true;
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Materials
            itemLoot.Add(ItemID.Obsidian, 1, 2, 5);
            itemLoot.Add(ItemID.Hellstone, 4, 2, 5);
            itemLoot.Add(ItemID.HellstoneBar, 10, 1, 3);
            itemLoot.Add(ModContent.ItemType<DemonicBoneAsh>(), 1, 1, 4);
            itemLoot.AddIf(() => DownedBossSystem.downedBrimstoneElemental, ModContent.ItemType<UnholyCore>(), 10, 1, 3);
            itemLoot.AddIf(() => DownedBossSystem.downedProvidence, ModContent.ItemType<Bloodstone>(), 2, 1, 3);

            // Weapons (none)

            // Bait
            itemLoot.Add(ItemID.MasterBait, 10, 1, 2);
            itemLoot.Add(ItemID.JourneymanBait, 5, 1, 3);
            itemLoot.Add(ItemID.ApprenticeBait, 3, 2, 3);

            // Potions
            itemLoot.Add(ItemID.InfernoPotion, 10, 1, 3);
            itemLoot.AddCratePotionRules();

            // Money
            itemLoot.Add(ItemID.SilverCoin, 1, 10, 90);
            itemLoot.Add(ItemID.GoldCoin, 2, 1, 5);
        }
    }
}
