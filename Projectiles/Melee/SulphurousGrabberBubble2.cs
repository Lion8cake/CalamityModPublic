﻿using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Projectiles.Melee
{
    public class SulphurousGrabberBubble2 : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.alpha = 60;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            Projectile.velocity.X *= 0.9f;
            Projectile.velocity.Y *= 0.9f;
            if (Main.rand.NextBool(5))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, (int)CalamityDusts.SulphurousSeaAcid, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item54, Projectile.position);
            int randAmt = Main.rand.Next(5, 9);
            for (int i = 0; i < randAmt; i++)
            {
                int toxicDust = Dust.NewDust(Projectile.Center, 0, 0, (int)CalamityDusts.SulphurousSeaAcid, 0f, 0f, 100, default, 1.4f);
                Main.dust[toxicDust].velocity *= 0.8f;
                Main.dust[toxicDust].position = Vector2.Lerp(Main.dust[toxicDust].position, Projectile.Center, 0.5f);
                Main.dust[toxicDust].noGravity = true;
            }
        }
    }
}
