﻿using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalamityMod.Cooldowns
{
    public class LifeSteal : CooldownHandler
    {
        public static new string ID => "LifeSteal";
        public override bool ShouldDisplay => instance.player.lifeSteal < 0f;
        public override LocalizedText DisplayName => CalamityUtils.GetText($"UI.Cooldowns.{ID}");
        public override string Texture => "CalamityMod/Cooldowns/LifeSteal";
        public override Color OutlineColor => new Color(255, 142, 165);
        public override Color CooldownStartColor => Color.Lerp(new Color(255, 216, 216), new Color(255, 117, 117), instance.Completion);
        public override Color CooldownEndColor => Color.Lerp(new Color(255, 216, 216), new Color(255, 117, 117), instance.Completion);
    }
}
