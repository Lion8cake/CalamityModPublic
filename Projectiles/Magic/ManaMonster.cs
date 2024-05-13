﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Magic
{
    public class ManaMonster : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Magic";
        public ref float Time => ref Projectile.ai[0];
        public Player Target => Main.player[Projectile.owner];
        public const int NPCAttackTime = 50;
        public const int PlayerAttackRedirectTime = 45;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 52;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.Opacity = 0f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            if (Time < NPCAttackTime)
            {
                Projectile.Opacity = Utils.GetLerpValue(0f, 30f, Time, true);
                if (Projectile.velocity.Length() < 27f)
                    Projectile.velocity *= 1.05f;
            }
            else
            {
                // Make an alert sound to indicate that the monster has become enraged and will now attack its caster.
                if (Time == NPCAttackTime)
                {
                    SoundStyle ambientNoise = Utils.SelectRandom(Main.rand, new SoundStyle[]
                    {
                        SoundID.Zombie39,
                        SoundID.Zombie40,
                        SoundID.Zombie41
                    });

                    SoundEngine.PlaySound(ambientNoise, Target.Center);
                    SoundEngine.PlaySound(SoundID.DD2_DrakinShot, Target.Center);
                    CreateTransitionBurstDust();
                }

                if (Time < NPCAttackTime + PlayerAttackRedirectTime)
                {
                    float idealMovementDirection = Projectile.AngleTo(Target.Center);
                    float angularTurnSpeed = 0.09f;
                    float newSpeed = MathHelper.Lerp(Projectile.velocity.Length(), 22f, 0.1f);
                    Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(idealMovementDirection, angularTurnSpeed).ToRotationVector2() * newSpeed;
                }
                else if (Projectile.velocity.Length() < 35f)
                    Projectile.velocity *= 1.04f;

                // Fade out.
                Projectile.Opacity = Utils.GetLerpValue(0f, 15f, Projectile.timeLeft, true);
            }

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Time++;
        }

        public void CreateTransitionBurstDust()
        {
            if (Main.dedServ)
                return;

            for (int i = 0; i < 75; i++)
            {
                Dust brimstone = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Square(-25f, 25f), (int)CalamityDusts.Brimstone);
                brimstone.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 3.1f) * MathHelper.Lerp(1f, 2.175f, i / 75f);
                brimstone.velocity = Vector2.Lerp(brimstone.velocity, -Vector2.UnitY * brimstone.velocity.Length(), 0.5f);
                brimstone.scale = MathHelper.Lerp(1f, 1.875f, i / 75f) * Main.rand.NextFloat(0.8f, 1f);
                brimstone.fadeIn = 0.4f;
                brimstone.noGravity = true;
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.SourceDamage *= 0f;
            if (Main.masterMode) modifiers.SourceDamage.Flat += 540f;
            else if (Main.expertMode) modifiers.SourceDamage.Flat += 450f;
            else modifiers.SourceDamage.Flat += 360f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 180);

        public override bool? CanDamage() => Projectile.Opacity >= 1f ? null : false;

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
