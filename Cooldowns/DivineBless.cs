﻿using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;

namespace CalamityMod.Cooldowns
{
    public class DivineBless : CooldownHandler
    {
        public static new string ID => "DivineBless";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => CalamityUtils.GetText($"UI.Cooldowns.{ID}");
        public override string Texture => "CalamityMod/Cooldowns/DivineBless";
        public override Color OutlineColor => new Color(233, 192, 68);
        public override Color CooldownStartColor => new Color(177, 105, 33);
        public override Color CooldownEndColor => new Color(233, 192, 68);

        public override void OnCompleted()
        {
            if (instance.player.whoAmI == Main.myPlayer)
                Projectile.NewProjectile(new EntitySource_Parent(instance.player), instance.player.Center, Vector2.Zero, ProjectileType<AllianceTriangle>(), 0, 0f, instance.player.whoAmI);
        }
    }
}
