﻿using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Projectiles.Summon.Umbrella
{
    public class MagicBulletBig : ModProjectile
    {
        public VertexStrip TrailDrawer;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bullet");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
			Projectile.extraUpdates = 12;
            Projectile.light = 0.5f;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
			Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.penetrate = 4;
        }

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (Projectile.FinalExtraUpdate())
			{
				if (Projectile.ai[0]++ > 1f)
					Projectile.alpha = 0;
			}
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.betsysCurse)
                target.AddBuff(BuffID.BetsysCurse, 180);
            if (!target.ichor)
                target.AddBuff(BuffID.Ichor, 180);
            if (target.Calamity().marked <= 0)
                target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 180);
            if (target.Calamity().aCrunch <= 0)
                target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 180);
            if (target.Calamity().wDeath <= 0)
                target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 180);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Ichor, 180);
            target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 180);
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 180);
            target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 180);
        }

		public override Color? GetAlpha(Color lightColor) => Color.White;

        public override bool PreDraw(ref Color lightColor)
        {
			if (Projectile.alpha != 0)
				return false;

			Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
			Rectangle frame = texture.Frame(1, 1, 0, 0);
			Vector2 origin = frame.Size() * 0.5f;
			Vector2 drawPosition = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

            // Draw the bullet.
			Main.spriteBatch.Draw(texture, drawPosition, frame, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

			// Draw Afterimages
			CalamityUtils.DrawAfterimagesFromEdge(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor);
            return false;
        }
    }
}
