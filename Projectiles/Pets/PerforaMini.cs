﻿using CalamityMod.Buffs.Pets;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Pets
{
    public class PerforaMini : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Pets";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 6)
            .WithOffset(-8f, -20f).WithSpriteDirection(1).WhenNotSelected(0, 0);
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 perfcenter = Projectile.Center;
            Vector2 vectorperf = player.Center - perfcenter;
            float playerdistance = vectorperf.Length();
            if (!player.active)
            {
                Projectile.active = false;
                return;
            }

            //Delete the projectile if the player doesnt have the buff or is very far away (dunno if this needs to be deleted)
            if (!player.HasBuff(ModContent.BuffType<BloodBound>()) || playerdistance >= 4000f)
            {
                Projectile.Kill();
            }

            CalamityPlayer modPlayer = player.Calamity();
            if (player.dead)
            {
                modPlayer.perfmini = false;
            }
            if (modPlayer.perfmini)
            {
                Projectile.timeLeft = 2;
            }

            Projectile.FloatingPetAI(true, 0.1f);

            //Dust
            if (Main.rand.NextBool(50))
            {
                int d1 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, default, 1.5f);
                int d2 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Ichor, 0f, 0f, 170, default, 0.5f);
                Main.dust[d2].noLight = true;
                Main.dust[d1].position = Projectile.Center;
                Main.dust[d2].position = Projectile.Center;
            }

            //Animation
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 5)
            {
                Projectile.frame = 0;
            }
        }
    }
}
