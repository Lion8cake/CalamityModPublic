﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Magic
{
	public class GleamingBolt2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bolt");
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.alpha = 255;
			projectile.timeLeft = 120;
			projectile.penetrate = 1;
			projectile.magic = true;
		}

		public override void AI()
		{
			projectile.velocity.X *= 0.985f;
			projectile.velocity.Y *= 0.985f;
			for (int dust = 0; dust < 2; dust++)
			{
				int randomDust = Main.rand.Next(2);
				if (randomDust == 0)
				{
					randomDust = 64;
				}
				else
				{
					randomDust = 204;
				}
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, randomDust, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int k = 0; k < 3; k++)
			{
				int randomDust = Main.rand.Next(2);
				if (randomDust == 0)
				{
					randomDust = 64;
				}
				else
				{
					randomDust = 204;
				}
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, randomDust, projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
			}
		}
	}
}
