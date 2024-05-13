﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Magic
{
    public class PhotosyntheticShard : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Magic";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            for (int i = 0; i < 4; i++)
            {
                Dust terraMagic = Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.TerraBlade, 0f, 0f, 0, default, 0.5f);
                terraMagic.scale = 0.42f;
                terraMagic.velocity *= 0.1f;
            }

            CalamityUtils.HomeInOnNPC(Projectile, false, 500f, 15f, 20f);
        }
    }
}
