﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class LeviathanTooth : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.DamageType = RogueDamageClass.Instance;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0f)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                if (Projectile.timeLeft < 575 && !Projectile.Calamity().stealthStrike)
                {
                    Projectile.velocity.Y += 0.5f;
                    if (Projectile.velocity.Y > 16f)
                    {
                        Projectile.velocity.Y = 16f;
                    }
                }
            }
            //Sticky Behaviour
            Projectile.StickyProjAI(15);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) => Projectile.ModifyHitNPCSticky(6);
        public override bool? CanDamage() => Projectile.ai[0] == 1f ? false : base.CanDamage();

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Venom, 180);
            if (Projectile.Calamity().stealthStrike)
            {
                int toothBlast = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BrackishWaterBlast>(), (int)(Projectile.damage * 0.6f), Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[toothBlast].usesLocalNPCImmunity = true;
                Main.projectile[toothBlast].usesIDStaticNPCImmunity = false;
                Main.projectile[toothBlast].localNPCHitCooldown = -1;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Venom, 180);
            if (Projectile.Calamity().stealthStrike)
            {
                int toothBlast = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BrackishWaterBlast>(), (int)(Projectile.damage * 0.6f), Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[toothBlast].usesLocalNPCImmunity = true;
                Main.projectile[toothBlast].usesIDStaticNPCImmunity = false;
                Main.projectile[toothBlast].localNPCHitCooldown = -1;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            if (Projectile.Calamity().lineColor == 1)
                tex = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Rogue/LeviathanTooth2").Value;
            if (Projectile.Calamity().lineColor == 2)
                tex = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Rogue/LeviathanTooth3").Value;

            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            if (Projectile.Calamity().stealthStrike)
            {
                CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1, tex);
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

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.Water, 0, 0, 0, default, 1.5f);
                Main.dust[dust].noGravity = true;
            }
        }
    }
}
