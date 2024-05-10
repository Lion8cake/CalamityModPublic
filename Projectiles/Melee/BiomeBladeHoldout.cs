﻿using System;
using CalamityMod.DataStructures;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityMod.CalamityUtils;
using static Terraria.ModLoader.ModContent;

namespace CalamityMod.Projectiles.Melee
{
    public class BiomeBladeHoldout : ModProjectile, ILocalizedModType //Visuals
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        private Player Owner => Main.player[Projectile.owner];
        public bool OwnerCanUseItem => Owner.HeldItem == associatedItem ? (Owner.HeldItem.ModItem as TrueBiomeBlade).CanUseItem(Owner) : false;
        public bool OwnerMayChannel => Owner.itemAnimation == 0 && OwnerCanUseItem && Owner.Calamity().mouseRight && Owner.active && !Owner.dead && Owner.StandingStill() && !Owner.mount.Active && Owner.CheckSolidGround(1, 3);
        public ref float ChanneledState => ref Projectile.ai[0];
        public ref float ChannelTimer => ref Projectile.ai[1];
        public ref float Initialized => ref Projectile.localAI[0];

        private Item associatedItem;
        const int ChannelTime = 120;

        public override void SetStaticDefaults()
        {
        }
        public override string Texture => "CalamityMod/Items/Weapons/Melee/TrueBiomeBlade";
        public bool drawIndrawHeldProjInFrontOfHeldItemAndArms = true;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 36;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.damage = 0;
        }

        public CurveSegment anticipation = new CurveSegment(EasingType.SineOut, 0f, 1f, 0.35f);
        public CurveSegment thrust = new CurveSegment(EasingType.ExpIn, 0.85f, 1.35f, -1.45f);
        public CurveSegment bounceback = new CurveSegment(EasingType.SineOut, 0.95f, -0.1f, 0.1f);
        internal float SwordHeight() => PiecewiseAnimation(ChannelTimer / (float)ChannelTime, new CurveSegment[] { anticipation, thrust, bounceback });

        public override void AI()
        {

            if (Initialized == 0f)
            {
                //If dropped, kill it instantly
                if (Owner.HeldItem.type != ItemType<TrueBiomeBlade>())
                {
                    Projectile.Kill();
                    return;
                }

                if (Owner.whoAmI == Main.myPlayer)
                    SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact);

                associatedItem = Owner.HeldItem;
                //Switch up the attunements
                Attunement temporaryAttunementStorage = (associatedItem.ModItem as TrueBiomeBlade).mainAttunement;
                (associatedItem.ModItem as TrueBiomeBlade).mainAttunement = (associatedItem.ModItem as TrueBiomeBlade).secondaryAttunement;
                (associatedItem.ModItem as TrueBiomeBlade).secondaryAttunement = temporaryAttunementStorage;
                Initialized = 1f;
            }

            if (!OwnerMayChannel && ChanneledState == 0f) //IF the channeling gets interrupted for any reason
            {
                Projectile.Center = Owner.Top + new Vector2(18f, 0f);
                ChanneledState = 1f;
                Projectile.timeLeft = 60;
                return;
            }

            if (ChanneledState == 0f)
            {
                Owner.heldProj = Projectile.whoAmI;

                Projectile.Center = Owner.Center + new Vector2(16f * Owner.direction + Owner.direction < 0 ? -26f : 16f, -30 * SwordHeight() + 10f);
                Projectile.rotation = MathHelper.PiOver4 + MathHelper.PiOver2; // No more silly turnaround with the repaired one?
                ChannelTimer++;
                Projectile.timeLeft = 60;

                if (ChannelTimer >= ChannelTime)
                {
                    Attune((TrueBiomeBlade)associatedItem.ModItem);
                    Projectile.timeLeft = 120;
                    ChanneledState = 2f; //State where it stays invisible doing nothing. Acts as a cooldown

                    Color particleColor = (associatedItem.ModItem as TrueBiomeBlade).mainAttunement.tooltipColor;

                    for (int i = 0; i <= 5; i++)
                    {
                        Vector2 displace = Vector2.UnitX * 20 * Main.rand.NextFloat(-1f, 1f);
                        Particle Glow = new GenericBloom(Owner.Bottom + displace, -Vector2.UnitY * Main.rand.NextFloat(1f, 5f), particleColor, 0.02f + Main.rand.NextFloat(0f, 0.2f), 20 + Main.rand.Next(30));
                        GeneralParticleHandler.SpawnParticle(Glow);
                    }
                    for (int i = 0; i <= 10; i++)
                    {
                        Vector2 displace = Vector2.UnitX * 16 * Main.rand.NextFloat(-1f, 1f);
                        Particle Sparkle = new GenericSparkle(Owner.Bottom + displace, -Vector2.UnitY * Main.rand.NextFloat(1f, 5f), particleColor, particleColor, 0.5f + Main.rand.NextFloat(-0.2f, 0.2f), 20 + Main.rand.Next(30), 1, 2f);
                        GeneralParticleHandler.SpawnParticle(Sparkle);
                    }
                }
            }

            if (ChanneledState == 1f)
                Projectile.position += Vector2.UnitY * -0.3f * (1f + Projectile.timeLeft / 60f);
        }

        public void Attune(TrueBiomeBlade item)
        {
            bool jungle = Owner.ZoneJungle;
            bool snow = Owner.ZoneSnow;
            bool evil = Owner.ZoneCorrupt || Owner.ZoneCrimson;
            bool desert = Owner.ZoneDesert;
            bool hell = Owner.ZoneUnderworldHeight;
            bool ocean = Owner.ZoneBeach || Owner.Calamity().ZoneSulphur;
            bool hallow = Owner.ZoneHallow;
            bool astral = Owner.Calamity().ZoneAstral;
            bool marine = Owner.Calamity().ZoneAbyss || Owner.Calamity().ZoneSunkenSea;

            Attunement attunement = Attunement.attunementArray[(int)AttunementID.TrueDefault];

            if (desert || hell)
                attunement = Attunement.attunementArray[(int)AttunementID.TrueHot];
            if (snow)
                attunement = Attunement.attunementArray[(int)AttunementID.TrueCold];
            if (jungle || ocean)
                attunement = Attunement.attunementArray[(int)AttunementID.TrueTropical];
            if (evil)
                attunement = Attunement.attunementArray[(int)AttunementID.TrueEvil];
            if (hallow)
                attunement = Attunement.attunementArray[(int)AttunementID.Holy];
            if (astral)
                attunement = Attunement.attunementArray[(int)AttunementID.Astral];
            if (marine)
                attunement = Attunement.attunementArray[(int)AttunementID.Marine];

            //If the owner already had the attunement , break out of it (And unswap)
            if (item.secondaryAttunement == attunement)
            {
                SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, Projectile.Center);
                item.secondaryAttunement = item.mainAttunement;
                item.mainAttunement = attunement;
                return;
            }

            SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, Projectile.Center);
            item.mainAttunement = attunement;
        }

        public override void OnKill(int timeLeft)
        {
            if (associatedItem == null)
            {
                return;
            }
            //If we swapped out the main attunement for the second one despite the second attunement being empty at the time, unswap them.
            if ((associatedItem.ModItem as TrueBiomeBlade).mainAttunement == null && (associatedItem.ModItem as TrueBiomeBlade).secondaryAttunement != null)
            {
                (associatedItem.ModItem as TrueBiomeBlade).mainAttunement = (associatedItem.ModItem as TrueBiomeBlade).secondaryAttunement;
                (associatedItem.ModItem as TrueBiomeBlade).secondaryAttunement = null;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (ChanneledState == 0f && ChannelTimer > 6f)
                return base.PreDraw(ref lightColor);

            else if (ChanneledState == 1f)
            {
                Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
                Vector2 squishyScale = new Vector2(Math.Abs((float)Math.Sin(MathHelper.Pi + MathHelper.TwoPi * Projectile.timeLeft / 30f)), 1f);
                SpriteEffects flip = (float)Math.Sin(MathHelper.Pi + MathHelper.TwoPi * Projectile.timeLeft / 30f) > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Main.EntitySpriteDraw(tex, Projectile.position - Main.screenPosition, null, lightColor * (Projectile.timeLeft / 60f), 0, tex.Size() / 2, squishyScale * (2f - (Projectile.timeLeft / 60f)), flip, 0);

                return false;
            }

            else
                return false;
        }
    }
}
