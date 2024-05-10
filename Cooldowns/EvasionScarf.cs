﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;

namespace CalamityMod.Cooldowns
{
    public class EvasionScarf : CooldownHandler
    {
        public static new string ID => "EvasionScarf";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => CalamityUtils.GetText($"UI.Cooldowns.{ID}");
        public override string Texture => "CalamityMod/Cooldowns/EvasionScarf";
        public override Color OutlineColor => Color.Lerp(new Color(255, 194, 150), new Color(255, 160, 150), (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2f) * 0.5f + 0.5f);
        public override Color CooldownStartColor => new Color(132, 23, 32);
        public override Color CooldownEndColor => new Color(164, 52, 45);
    }
}
