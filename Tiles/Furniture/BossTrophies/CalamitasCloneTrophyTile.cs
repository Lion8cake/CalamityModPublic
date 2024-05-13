﻿using CalamityMod.Items.Placeables.Furniture.Trophies;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityMod.Tiles.Furniture.BossTrophies
{
    [LegacyName("CalamitasTrophyTile")]
    public class CalamitasCloneTrophyTile : ModTile
    {
        public override void SetStaticDefaults() => this.SetUpTrophy();
    }
}
