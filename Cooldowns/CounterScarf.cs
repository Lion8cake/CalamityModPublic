﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;

namespace CalamityMod.Cooldowns
{
    public class CounterScarf : CooldownHandler
    {
        public static new string ID => "CounterScarf";

        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => CalamityUtils.GetText($"UI.Cooldowns.{ID}");
        public override string Texture => "CalamityMod/Cooldowns/CounterScarf";
        public override Color OutlineColor => Color.Lerp(new Color(255, 115, 178), new Color(255, 76, 76), (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2f) * 0.5f + 0.5f);
        public override Color CooldownStartColor => new Color(194, 75, 97);
        public override Color CooldownEndColor => new Color(255, 76, 76);
    }
}
