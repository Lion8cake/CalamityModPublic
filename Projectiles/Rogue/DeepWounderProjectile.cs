﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class DeepWounderProjectile : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/DeepWounder";

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
            Projectile.DamageType = RogueDamageClass.Instance;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI()
        {
            Projectile.rotation += 0.2f * Projectile.direction;

            if (Projectile.Calamity().stealthStrike)
            {
                int spriteWidth = 52;
                int spriteHeight = 48;
                Vector2 spriteCenter = Projectile.Center - new Vector2(spriteWidth / 2, spriteHeight / 2);

                for (int i = 0; i < 30; i++)
                {
                    int dust = Dust.NewDust(spriteCenter, spriteWidth, spriteHeight, DustID.Water, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 0, default, 1f);
                    Main.dust[dust].noGravity = true;
                }

                if (Main.rand.NextBool(5))
                {
                    Vector2 waterVelocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
                    waterVelocity.Normalize();
                    waterVelocity *= 3;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), spriteCenter, waterVelocity, ModContent.ProjectileType<DeepWounderWater>(), (int)(Projectile.damage * 0.05f), 1, Projectile.owner, 0, 0);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 120);
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 120);
            if (Projectile.Calamity().stealthStrike)
            {
                target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 120);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 150);
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 150);
            if (Projectile.Calamity().stealthStrike)
            {
                target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 150);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            if (Projectile.direction > 0)
            {
                Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            }
            else
            {
                Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
            }
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            Projectile.Kill();
            return false;
        }
    }
}
