﻿using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.DraedonsArsenal
{
    public class AnomalysNanogunMPFBBoom : BaseMassiveExplosionProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Misc";
        public static readonly SoundStyle MPFBExplosion = new("CalamityMod/Sounds/Item/AnomalysNanogunMPFBExplosion");
        public override int Lifetime => 40;
        public override bool UsesScreenshake => true;
        public override float GetScreenshakePower(float pulseCompletionRatio) => CalamityUtils.Convert01To010(pulseCompletionRatio) * 3f;
        public override Color GetCurrentExplosionColor(float pulseCompletionRatio) => Color.Lerp(Color.Blue, Color.CornflowerBlue, MathHelper.Clamp(pulseCompletionRatio * 2.2f, 0f, 1f));

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = Lifetime;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void PostAI()
        {
            Lighting.AddLight(Projectile.Center, 0, Projectile.Opacity * 0.7f / 255f, Projectile.Opacity);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.numHits > 0)
                Projectile.damage = (int)(Projectile.damage * 0.6f);
            if (Projectile.damage < 1)
                Projectile.damage = 1;
        }
    }
}
