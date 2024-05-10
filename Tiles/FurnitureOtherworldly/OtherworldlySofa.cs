﻿using CalamityMod.Dusts.Furniture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Tiles.FurnitureOtherworldly
{
    [LegacyName("OccultSofa")]
    public class OtherworldlySofa : ModTile
    {
        public override void SetStaticDefaults() => this.SetUpSofa(ModContent.ItemType<Items.Placeables.FurnitureOtherworldly.OtherworldlySofa>(), true);

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, DustID.Stone, 0f, 0f, 1, new Color(125, 94, 128), 1f);
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, ModContent.DustType<OtherworldlyTileCloth>(), 0f, 0f, 1, new Color(255, 255, 255), 1f);
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info) => CalamityUtils.BenchSitInfo(i, j, ref info);

        public override bool RightClick(int i, int j) => CalamityUtils.ChairRightClick(i, j);

        public override void MouseOver(int i, int j) => CalamityUtils.BenchMouseOver(i, j, ModContent.ItemType<Items.Placeables.FurnitureOtherworldly.OtherworldlySofa>());

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance);
        }
    }
}
