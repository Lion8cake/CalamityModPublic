﻿using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Placeables
{
    [LegacyName("AstralSilt")]
    public class NovaeSlag : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placeables";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 200;
            ItemID.Sets.SortingPriorityExtractibles[Type] = 1; // Silt Block
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
            Item.createTile = ModContent.TileType<Tiles.Astral.NovaeSlag>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 9999;
        }
        public override void ExtractinatorUse(int extractinatorBlockType, ref int resultType, ref int resultStack)
        {
            /*
                Novae slag will give stardust, fallen stars, gems and HM ores always by default
                When Astrum Deus has been defeated, it will give Astral Ore
            */

            bool twoMechsDowned =
                (NPC.downedMechBoss1 && NPC.downedMechBoss2 && !NPC.downedMechBoss3) ||
                (NPC.downedMechBoss2 && NPC.downedMechBoss3 && !NPC.downedMechBoss1) ||
                (NPC.downedMechBoss3 && NPC.downedMechBoss1 && !NPC.downedMechBoss2) ||
                (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3);

            float val = Main.rand.NextFloat(100);
            if (val < 30f)
            {
                resultType = ItemID.CopperCoin;
                resultStack = Main.rand.Next(1, 100);
            }
            else if (val < 37f)
            {
                resultType = ItemID.SilverCoin;
                resultStack = Main.rand.Next(1, 100);
            }
            else if (val < 38f)
            {
                resultType = ItemID.GoldCoin;
                resultStack = Main.rand.Next(1, 100);
            }
            else if (val < 38.03f)
            {
                resultType = ItemID.PlatinumCoin;
                resultStack = Main.rand.Next(1, 11);
            }
            else if (val < 48.03f && !Main.dayTime)
            {
                resultType = ItemID.FallenStar;
                resultStack = Main.rand.Next(1, 21);
            }
            else if (val < 58.03f)
            {
                resultType = ModContent.ItemType<StarblightSoot>();
                resultStack = Main.rand.Next(1, 21);
            }
            else if (val < 61.03f)
            {
                resultType = ItemID.Diamond;
                resultStack = Main.rand.Next(1, 21);
            }
            else if (val < 64.03f)
            {
                resultType = ItemID.Ruby;
                resultStack = Main.rand.Next(1, 21);
            }
            else if (val < 67.03f)
            {
                resultType = ItemID.Topaz;
                resultStack = Main.rand.Next(1, 21);
            }
            else if (val < 70.03f)
            {
                resultType = ItemID.Emerald;
                resultStack = Main.rand.Next(1, 21);
            }
            else if (val < 73.03f)
            {
                resultType = ItemID.Sapphire;
                resultStack = Main.rand.Next(1, 21);
            }
            else if (val < 76.03f)
            {
                resultType = ItemID.Amethyst;
                resultStack = Main.rand.Next(1, 21);
            }
            else if (val < 76.53f)
            {
                resultType = ItemID.Amber;
                resultStack = Main.rand.Next(1, 21);
            }
            else if (val < 78.78f)
            {
                resultType = ItemID.PalladiumOre;
                resultStack = Main.rand.Next(1, 17);
            }
            else if (val < 81.03f)
            {
                resultType = ItemID.CobaltOre;
                resultStack = Main.rand.Next(1, 17);
            }
            else if (val < 83.03f)
            {
                resultType = CalamityConfig.Instance.EarlyHardmodeProgressionRework ? (!NPC.downedMechBossAny ? ItemID.CobaltOre : ItemID.MythrilOre) : ItemID.MythrilOre;
                resultStack = Main.rand.Next(1, 17);
            }
            else if (val < 85.03f)
            {
                resultType = CalamityConfig.Instance.EarlyHardmodeProgressionRework ? (!NPC.downedMechBossAny ? ItemID.PalladiumOre : ItemID.OrichalcumOre) : ItemID.OrichalcumOre;
                resultStack = Main.rand.Next(1, 17);
            }
            else if (val < 86.78f)
            {
                resultType = CalamityConfig.Instance.EarlyHardmodeProgressionRework ? (!NPC.downedMechBossAny ? ItemID.CobaltOre : !twoMechsDowned ? ItemID.MythrilOre : ItemID.AdamantiteOre) : ItemID.AdamantiteOre;
                resultStack = Main.rand.Next(1, 17);
            }
            else if (val < 88.53f)
            {
                resultType = CalamityConfig.Instance.EarlyHardmodeProgressionRework ? (!NPC.downedMechBossAny ? ItemID.PalladiumOre : !twoMechsDowned ? ItemID.OrichalcumOre : ItemID.TitaniumOre) : ItemID.TitaniumOre;
                resultStack = Main.rand.Next(1, 17);
            }
            else if (DownedBossSystem.downedAstrumDeus)
            {
                resultType = ModContent.ItemType<Ores.AstralOre>();
                resultStack = Main.rand.Next(1, 2);
            }
        }
    }
}
