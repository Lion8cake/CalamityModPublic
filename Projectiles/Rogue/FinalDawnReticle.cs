﻿using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class FinalDawnReticle : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.scale = 1.5f;
            Projectile.width = 120;
            Projectile.height = 120;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = RogueDamageClass.Instance;
            Projectile.alpha = 0;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void AI()
        {
            if (Projectile.ai[1] == 0)
                Projectile.ai[1] = 1;
            if (Projectile.ai[0] == 0)
            {
                int dustCount = 36;
                for (int i = 0; i < dustCount; i++)
                {
                    Vector2 startingPosition = Projectile.Center + 10f * Vector2.UnitX;
                    Vector2 offset = Vector2.UnitX * Projectile.width * 0.1875f;
                    offset = offset.RotatedBy((i - (dustCount / 2 - 1)) * MathHelper.TwoPi / 20f);
                    int dustIdx = Dust.NewDust(startingPosition + offset, 0, 0, ModContent.DustType<FinalFlame>(), offset.X * 2f, offset.Y * 2f, 100, default, 3.4f);
                    Main.dust[dustIdx].noGravity = true;
                    Main.dust[dustIdx].noLight = true;
                    Main.dust[dustIdx].velocity = Vector2.Normalize(offset) * 5f;
                }
                Projectile.ai[0] = 1;
            }
            Projectile.alpha += 8;
            Projectile.scale *= 0.98f;
            Projectile.ai[1] *= 1.01f;
            if (Projectile.alpha >= 255)
                Projectile.Kill();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D ring = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Texture2D symbol = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Rogue/FinalDawnReticleSymbol").Value;
            Main.EntitySpriteDraw(symbol,
                             Projectile.Center - Main.screenPosition,
                             null,
                             Projectile.GetAlpha(Color.White),
                             Projectile.rotation,
                             new Vector2(symbol.Width / 2, symbol.Height / 2),
                             Projectile.scale,
                             Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                             0);
            Main.EntitySpriteDraw(ring,
                             Projectile.Center - Main.screenPosition,
                             null,
                             Projectile.GetAlpha(Color.White),
                             Projectile.rotation,
                             new Vector2(ring.Width / 2, ring.Height / 2),
                             Projectile.ai[1],
                             Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                             0);
            return false;
        }
    }
}
