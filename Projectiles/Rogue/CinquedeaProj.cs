﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class CinquedeaProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/Cinquedea";

        internal float gravspin = 0f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = RogueDamageClass.Instance;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }

        public override void AI()
        {
            DrawOriginOffsetY = 11;
            DrawOffsetX = -22;

            bool stealthstrike = Projectile.ai[1] == 1 && Projectile.penetrate == 1;
            if (Projectile.spriteDirection == 1)
            {
                gravspin = Projectile.velocity.Y * 0.03f;
            }
            if (Projectile.spriteDirection == -1)
            {
                gravspin = Projectile.velocity.Y * -0.03f;
            }
            Projectile.ai[0]++;
            //Fucking slopes
            if (Projectile.ai[0] > 2f)
            {
                Projectile.tileCollide = true;
            }
            //Face-forward rotation code
            if ((Projectile.ai[0] <= 80 && !stealthstrike) || stealthstrike || Projectile.velocity.Y <= 0)
            {
                Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
                Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
                //Rotating 45 degrees if shooting right
                if (Projectile.spriteDirection == 1)
                {
                    Projectile.rotation += MathHelper.ToRadians(45f);
                }
                //Rotating 45 degrees if shooting left
                if (Projectile.spriteDirection == -1)
                {
                    Projectile.rotation -= MathHelper.ToRadians(45f);
                }
            }

            //Stealth strike
            if (stealthstrike)
            {
                CalamityUtils.HomeInOnNPC(Projectile, true, 250f, 7f, 20f);
            }
            //Gravity code
            else
            {
                if (Projectile.ai[0] > 80)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y + 0.15f;
                    if (Projectile.velocity.Y > 0)
                    {
                        Projectile.rotation += gravspin;
                    }
                    if (Projectile.velocity.Y > 10f)
                    {
                        Projectile.velocity.Y = 10f;
                    }
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[1] == 1f && Projectile.penetrate == 1)
                Projectile.timeLeft = 180;
        }
    }
}
