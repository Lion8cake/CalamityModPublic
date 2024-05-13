﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Placeables
{
    [LegacyName("AstralFossil")]
    public class CelestialRemains : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placeables";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 200;
            ItemID.Sets.SortingPriorityExtractibles[Type] = 3; // Desert Fossil
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
            Item.createTile = ModContent.TileType<Tiles.AstralDesert.CelestialRemains>();
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
                Celestial remains will give stardust, fallen stars, ancient bone dust, gems and HM ores always by default
                When Astrum Deus has been defeated, it will give Astral Ore
            */

            bool twoMechsDowned =
                (NPC.downedMechBoss1 && NPC.downedMechBoss2 && !NPC.downedMechBoss3) ||
                (NPC.downedMechBoss2 && NPC.downedMechBoss3 && !NPC.downedMechBoss1) ||
                (NPC.downedMechBoss3 && NPC.downedMechBoss1 && !NPC.downedMechBoss2) ||
                (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3);

            float val = Main.rand.NextFloat(100);
            if (val < 20f)
            {
                resultType = ItemID.CopperCoin;
                resultStack = Main.rand.Next(1, 100);
            }
            else if (val < 27f)
            {
                resultType = ItemID.SilverCoin;
                resultStack = Main.rand.Next(1, 100);
            }
            else if (val < 28f)
            {
                resultType = ItemID.GoldCoin;
                resultStack = Main.rand.Next(1, 100);
            }
            else if (val < 28.03f)
            {
                resultType = ItemID.PlatinumCoin;
                resultStack = Main.rand.Next(1, 11);
            }
            else if (val < 48.03f)
            {
                resultType = ModContent.ItemType<AncientBoneDust>();
                resultStack = Main.rand.Next(1, 11);
            }
            else if (val < 58.03f && !Main.dayTime)
            {
                resultType = ItemID.FallenStar;
                resultStack = Main.rand.Next(1, 11);
            }
            else if (val < 58.03f && !Main.dayTime)
            {
                resultType = ItemID.FallenStar;
                resultStack = Main.rand.Next(1, 11);
            }
            else if (val < 68.03f)
            {
                resultType = ModContent.ItemType<StarblightSoot>();
                resultStack = Main.rand.Next(1, 11);
            }
            else if (val < 69.03f)
            {
                resultType = ItemID.Diamond;
                resultStack = Main.rand.Next(1, 21);
            }
            else if (val < 70.03f)
            {
                resultType = ItemID.Ruby;
                resultStack = Main.rand.Next(1, 21);
            }
            else if (val < 71.03f)
            {
                resultType = ItemID.Topaz;
                resultStack = Main.rand.Next(1, 21);
            }
            else if (val < 72.03f)
            {
                resultType = ItemID.Emerald;
                resultStack = Main.rand.Next(1, 21);
            }
            else if (val < 73.03f)
            {
                resultType = ItemID.Sapphire;
                resultStack = Main.rand.Next(1, 21);
            }
            else if (val < 74.03f)
            {
                resultType = ItemID.Amethyst;
                resultStack = Main.rand.Next(1, 21);
            }
            else if (val < 79.03f)
            {
                resultType = ItemID.Amber;
                resultStack = Main.rand.Next(1, 21);
            }
            else if (val < 81.78f)
            {
                resultType = ItemID.PalladiumOre;
                resultStack = Main.rand.Next(1, 17);
            }
            else if (val < 83.03f)
            {
                resultType = ItemID.CobaltOre;
                resultStack = Main.rand.Next(1, 17);
            }
            else if (val < 85.03f)
            {
                resultType = CalamityConfig.Instance.EarlyHardmodeProgressionRework ? (!NPC.downedMechBossAny ? ItemID.CobaltOre : ItemID.MythrilOre) : ItemID.MythrilOre;
                resultStack = Main.rand.Next(1, 17);
            }
            else if (val < 87.03f)
            {
                resultType = CalamityConfig.Instance.EarlyHardmodeProgressionRework ? (!NPC.downedMechBossAny ? ItemID.PalladiumOre : ItemID.OrichalcumOre) : ItemID.OrichalcumOre;
                resultStack = Main.rand.Next(1, 17);
            }
            else if (val < 88.78f)
            {
                resultType = CalamityConfig.Instance.EarlyHardmodeProgressionRework ? (!NPC.downedMechBossAny ? ItemID.CobaltOre : !twoMechsDowned ? ItemID.MythrilOre : ItemID.AdamantiteOre) : ItemID.AdamantiteOre;
                resultStack = Main.rand.Next(1, 17);
            }
            else if (val < 89.53f)
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

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CelestialRemainsWall>(4).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
