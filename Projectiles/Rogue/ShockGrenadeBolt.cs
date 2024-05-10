﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class ShockGrenadeBolt : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public static int frameWidth = 12;
        public static int frameHeight = 26;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 120;
            Projectile.DamageType = RogueDamageClass.Instance;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.timeLeft < 55)
            {
                Projectile.tileCollide = true;
            }

            if (Projectile.ai[1] == 1f)
            {
                float minDist = 999f;
                int index = 0;
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(Projectile, false))
                    {
                        float dist = (Projectile.Center - npc.Center).Length();
                        if (dist < minDist)
                        {
                            minDist = dist;
                            index = i;
                        }
                    }
                }

                Vector2 velocityNew;
                if (minDist < 999f)
                {
                    velocityNew = Main.npc[index].Center - Projectile.Center;
                    velocityNew.Normalize();
                    velocityNew *= 2f;
                    Projectile.velocity += velocityNew;
                    if (Projectile.velocity.Length() > 10f)
                    {
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 10f;
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.Electrified, 120);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffID.Electrified, 120);

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite;
            if (Projectile.ai[0] == 0f)
                sprite = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Rogue/ShockGrenadeBolt").Value;
            else
                sprite = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Rogue/ShockGrenadeBolt2").Value;
            Color drawColour = Color.White;

            Vector2 origin = new Vector2(frameWidth / 2, frameHeight / 2);
            Main.EntitySpriteDraw(sprite, Projectile.Center - Main.screenPosition, new Rectangle(0, frameHeight * Projectile.frame, frameWidth, frameHeight), drawColour, Projectile.rotation, origin, 1f, SpriteEffects.None, 0);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item93 with { Volume = SoundID.Item93.Volume * 0.25f }, Projectile.position);

            for (int i = 0; i < 5; i++)
            {
                int dustType = 132;
                int dust = Dust.NewDust(Projectile.Center, 1, 1, dustType, Projectile.velocity.X, Projectile.velocity.Y, 0, default, 0.5f);
                Main.dust[dust].noGravity = true;
            }
        }
    }
}
