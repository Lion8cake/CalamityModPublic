﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Melee.Spears
{
	public class EarthenPike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pike");
		}

		public override void SetDefaults()
		{
			projectile.width = 40;  //The width of the .png file in pixels divided by 2.
			projectile.aiStyle = 19;
			projectile.melee = true;  //Dictates whether projectile is a melee-class weapon.
			projectile.timeLeft = 90;
			projectile.height = 40;  //The height of the .png file in pixels divided by 2.
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.ownerHitCheck = true;
			projectile.hide = true;
		}

		public override void AI()
		{
			Main.player[projectile.owner].direction = projectile.direction;
			Main.player[projectile.owner].heldProj = projectile.whoAmI;
			Main.player[projectile.owner].itemTime = Main.player[projectile.owner].itemAnimation;
			projectile.position.X = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - (float)(projectile.width / 2);
			projectile.position.Y = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - (float)(projectile.height / 2);
			projectile.position += projectile.velocity * projectile.ai[0];
			if (Main.rand.Next(4) == 0)
			{
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 32, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			}
			if (projectile.ai[0] == 0f)
			{
				projectile.ai[0] = 3f;
				projectile.netUpdate = true;
			}
			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] >= 6f)
			{
				projectile.localAI[0] = 0f;
				if (Main.myPlayer == projectile.owner)
				{
					float velocityY = projectile.velocity.Y * 1.25f;
					if (velocityY < 0.1f)
					{
						velocityY = 0.1f;
					}
					int proj = Projectile.NewProjectile(projectile.Center.X + projectile.velocity.X, projectile.Center.Y + projectile.velocity.Y,
						projectile.velocity.X * 1.25f, velocityY, mod.ProjectileType("FossilShard"), (int)((double)projectile.damage * 0.25), 0f, projectile.owner, 0f, 0f);
					Main.projectile[proj].GetGlobalProjectile<CalamityGlobalProjectile>(mod).forceMelee = true;
				}
			}
			if (Main.player[projectile.owner].itemAnimation < Main.player[projectile.owner].itemAnimationMax / 3)
			{
				projectile.ai[0] -= 2.4f;
				if (projectile.localAI[0] == 0f && Main.myPlayer == projectile.owner)
				{
					projectile.localAI[0] = 1f;
				}
			}
			else
			{
				projectile.ai[0] += 0.95f;
			}

			if (Main.player[projectile.owner].itemAnimation == 0)
			{
				projectile.Kill();
			}

			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 2.355f;
			if (projectile.spriteDirection == -1)
			{
				projectile.rotation -= 1.57f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 8;
			target.AddBuff(mod.BuffType("ArmorCrunch"), 300);
		}
	}
}
