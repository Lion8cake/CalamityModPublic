﻿using CalamityMod.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
namespace CalamityMod.Projectiles.DraedonsArsenal
{
    public class PhaseslayerBeam : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Misc";
        public const int Lifetime = 180;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 120;
            Projectile.scale = 0.5f;
            Projectile.Size *= Projectile.scale;

            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = Lifetime;
            Projectile.localNPCHitCooldown = 6;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            NPC potentialTarget = Projectile.Center.ClosestNPCAt(1000f);
            if (potentialTarget != null && Projectile.Distance(potentialTarget.Center) > 40f && Projectile.timeLeft > Lifetime - 60)
                Projectile.velocity = (Projectile.velocity * 7f + Projectile.SafeDirectionTo(potentialTarget.Center, -Vector2.UnitY) * 24f) / 8f;

            Projectile.frameCounter++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D bladeTexture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Rectangle frame = bladeTexture.Frame(1, 5, 0, Projectile.frameCounter / 4 % 5);
            var drawData2 = new DrawData(bladeTexture, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, bladeTexture.Size() / new Vector2(1f, 5f) * 0.5f, Projectile.scale * 1.2f, SpriteEffects.None, 0);

            CalamityShaders.LightDistortionShader.CurrentTechnique.Passes[0].Apply();
            drawData2.Draw(Main.spriteBatch);

            return false;
        }
        public override void PostDraw(Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
