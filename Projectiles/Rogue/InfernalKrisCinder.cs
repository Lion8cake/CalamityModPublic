﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Projectiles.Rogue
{
    public class InfernalKrisCinder : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 3;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 120;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = RogueDamageClass.Instance;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.1f;
            Projectile.rotation += 0.4f * Projectile.direction;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);

            float minScale = 1.9f;
            float maxScale = 2.5f;
            int numDust = 2;
            for (int i = 0; i < numDust; i++)
            {
                Dust.NewDust(Projectile.position, 4, 4, DustID.Torch, Projectile.velocity.X, Projectile.velocity.Y, 0, default, Main.rand.NextFloat(minScale, maxScale));
            }

            Projectile.Kill();
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.OnFire, 90);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffID.OnFire, 90);

        public override bool PreDraw(ref Color lightColor)
        {
            // If this is a stealth strike, make the blade glow orange
            Color glowColour = new Color(255, 215, 100, 100);
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, glowColour, Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);

            float minScale = 1.9f;
            float maxScale = 2.5f;
            int numDust = 2;
            for (int i = 0; i < numDust; i++)
            {
                int dust = Dust.NewDust(Projectile.position, 4, 4, DustID.Torch, 0f, -2f, 0, default, Main.rand.NextFloat(minScale, maxScale));
                Main.dust[dust].noGravity = true;
            }
            return false;
        }
    }
}
