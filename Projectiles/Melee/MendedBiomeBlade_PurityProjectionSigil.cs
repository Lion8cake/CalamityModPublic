﻿using CalamityMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityMod.CalamityUtils;

namespace CalamityMod.Projectiles.Melee
{
    public class PurityProjectionSigil : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        private NPC target => Main.npc[(int)Projectile.ai[0]];

        public Player Owner => Main.player[Projectile.owner];
        public override string Texture => "CalamityMod/Projectiles/Melee/MendedBiomeBlade_PurityProjectionSigil";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = TrueBiomeBlade.DefaultAttunement_SigilTime;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.75f, 1f, 0.24f);
            int dustParticle = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch, 0f, 0f, 100, default, 0.9f);
            Main.dust[dustParticle].noGravity = true;
            Main.dust[dustParticle].velocity *= 0.5f;

            if (target.active)
            {
                Projectile.Center = target.Center;
            }
            else
                Projectile.active = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.myPlayer != Projectile.owner) // don't show for other players
                return false;
            DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
