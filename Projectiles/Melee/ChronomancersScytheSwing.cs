﻿using System;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Melee
{
    public class ChronomancersScytheSwing : ModProjectile
    {
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<ChronomancersScythe>();
        public static int IcicleSpeed = 24;
        public static int IcicleVariance = 4;
        public static int IcicleFrequency = 5;
        public static float IcicleDamageMultiplier = 1.2f;
        public static int ClockChance = 30;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 148;
            Projectile.height = 68;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.coldDamage = true;
        }

        public override void AI()
        {
            Player Owner = Main.player[Projectile.owner];
            Projectile.damage = (int)(Owner.GetTotalDamage(Projectile.DamageType).ApplyTo(Projectile.originalDamage) * (1f + Utils.GetLerpValue(0f, 300f, Projectile.ai[1], true)));

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 1)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
                Projectile.frame = 0;

            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
                Projectile.soundDelay = 24;
            }

            if (Main.myPlayer == Projectile.owner)
            {
                if (!Owner.CantUseHoldout())
                {
                    float scaleFactor6 = 1f;

                    if (Owner.ActiveItem().shoot == Projectile.type)
                        scaleFactor6 = Owner.ActiveItem().shootSpeed * Projectile.scale;

                    Vector2 slashDirection = Main.MouseWorld - Owner.RotatedRelativePoint(Owner.MountedCenter, true);
                    slashDirection.Normalize();
                    if (slashDirection.HasNaNs())
                        slashDirection = Vector2.UnitX * (float)Owner.direction;

                    slashDirection *= scaleFactor6;
                    if (slashDirection.X != Projectile.velocity.X || slashDirection.Y != Projectile.velocity.Y)
                        Projectile.netUpdate = true;

                    Projectile.velocity = slashDirection;
                }
                else
                    Projectile.Kill();
            }

            Projectile.ai[0]++;
            // rapidly fire icicles
            if (Projectile.ai[0] % IcicleFrequency == 0)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Vector2 velocity = Projectile.Center.DirectionTo(Main.MouseWorld) * IcicleSpeed + new Vector2(Main.rand.NextFloat(-IcicleVariance, IcicleVariance), Main.rand.NextFloat(-IcicleVariance, IcicleVariance));
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<ChronoIcicleSmall>(), (int)(Projectile.damage * IcicleDamageMultiplier), Projectile.knockBack, Projectile.owner);
                }
            }

            Vector2 dustSpawn = Projectile.Center + Projectile.velocity * 3f;
            Lighting.AddLight(dustSpawn, Color.LightBlue.R * 0.001f, Color.LightBlue.G * 0.001f, Color.LightBlue.B * 0.001f);

            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(dustSpawn - Projectile.Size * 0.5f, Projectile.width, Projectile.height, DustID.IceRod, Projectile.velocity.X, Projectile.velocity.Y, 100);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].position -= Projectile.velocity;
            }

            Projectile.position = Owner.RotatedRelativePoint(Owner.MountedCenter, true) - Projectile.Size * 0.5f + Vector2.UnitX * 8 + Vector2.UnitY * 4;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.spriteDirection == 1)
                Projectile.rotation += MathHelper.Pi;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
            Owner.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<TimeDistortion>(), 60);
            if (Main.rand.NextBool(ClockChance))
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Main.rand.NextVector2Circular(-2, 2), ModContent.ProjectileType<ChronoClock>(), 0, 0, Projectile.owner);
            }
        }
    }
}
