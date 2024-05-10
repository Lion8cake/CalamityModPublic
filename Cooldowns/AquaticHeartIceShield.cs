﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;

namespace CalamityMod.Cooldowns
{
    public class AquaticHeartIceShield : CooldownHandler
    {
        public static new string ID => "AquaticHeartIceShield";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => CalamityUtils.GetText($"UI.Cooldowns.{ID}");
        public override string Texture => "CalamityMod/Cooldowns/AquaticHeartIceShield";
        public override Color OutlineColor => Color.Lerp(new Color(163, 186, 198), new Color(146, 187, 255), (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f + 0.5f);
        public override Color CooldownStartColor => new Color(124, 195, 214);
        public override Color CooldownEndColor => new Color(147, 230, 253);
    }
}
