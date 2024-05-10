﻿using System.Collections.Generic;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Enemy
{
    public class CragmawExplosion : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Enemy";
        public int FrameX = 0;
        public int FrameY = 0;
        public int CurrentFrame => FrameY + FrameX * 14;

        public override string Texture => "CalamityMod/Projectiles/Rogue/SulphuricNukesplosion";

        public override void SetDefaults()
        {
            Projectile.width = 140;
            Projectile.height = 290;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.hide = true;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 6 == 0)
            {
                FrameY += 1;
                if (FrameY >= 7)
                {
                    FrameX += 1;
                    FrameY = 0;
                }
                if (FrameX >= 2)
                    Projectile.Kill();
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage <= 0)
                return;

            target.AddBuff(ModContent.BuffType<Irradiated>(), 300);
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Rectangle frame = new Rectangle(FrameX * Projectile.width, FrameY * Projectile.height, Projectile.width, Projectile.height);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>("CalamityMod/Projectiles/Rogue/SulphuricNukesplosion").Value, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, Projectile.Size / 2f, 1f, SpriteEffects.None, 0);
            return false;
        }
    }
}
