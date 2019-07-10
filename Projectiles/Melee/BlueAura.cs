﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Melee
{
    public class BlueAura : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Aura");
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            aiType = 348;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 300;
        }

        public override void AI()
        {
            for (int num468 = 0; num468 < 3; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 15, 0f, 0f, 100, default(Color), 1.3f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 0f;
            }
        }
    }
}
