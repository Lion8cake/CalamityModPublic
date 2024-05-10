﻿using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Pets
{
    public class RotomPet : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Pets";
        private bool initialized = false;

        private Form RotomType = Form.Normal;
        private enum Form
        {
            Normal,
            Dex,
            Wash,
            Heat,
            Frost,
            Mow,
            Fan
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 6)
            .WithOffset(-10f, 0f).WithSpriteDirection(1).WhenNotSelected(0, 0);
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();

            if (!player.active)
            {
                Projectile.active = false;
                return;
            }
            if (player.dead)
            {
                modPlayer.rotomPet = false;
            }
            if (modPlayer.rotomPet)
            {
                Projectile.timeLeft = 2;
            }

            if (!initialized)
            {
                DustEffects();
                initialized = true;
            }

            UpdateForm(player);
            UpdateFrames();

            Projectile.FloatingPetAI(true, 0.05f);
        }

        private void UpdateForm(Player player)
        {
            if (CalamityPlayer.areThereAnyDamnBosses)
                RotomType = Form.Dex;
            else if (player.ZoneBeach || player.InSunkenSea() || player.InSulphur() || player.InAbyss())
                RotomType = Form.Wash;
            else if (player.ZoneTowerSolar || player.ZoneDesert || player.ZoneUndergroundDesert || player.ZoneUnderworldHeight || player.InCalamity())
                RotomType = Form.Heat;
            else if (player.ZoneSnow || Main.snowMoon)
                RotomType = Form.Frost;
            else if (player.ZoneJungle)
                RotomType = Form.Mow;
            else if (player.ZoneSkyHeight || player.ZoneMeteor || player.InAstral())
                RotomType = Form.Fan;
            else
                RotomType = Form.Normal;
        }

        private void DustEffects()
        {
            int dustAmt = 25;
            for (int i = 0; i < dustAmt; i++)
            {
                int electric = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 16f), Projectile.width, Projectile.height - 16, DustID.Firework_Blue, 0f, 0f, 0, default, 1f);
                Main.dust[electric].velocity *= 2f;
                Main.dust[electric].scale *= 1.15f;
            }
        }

        private void UpdateFrames()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 4)
            {
                Projectile.frame = 0;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Drawing(lightColor,
                Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value,
                ModContent.Request<Texture2D>("CalamityMod/Projectiles/Pets/RotomDex").Value,
                ModContent.Request<Texture2D>("CalamityMod/Projectiles/Pets/RotomWash").Value,
                ModContent.Request<Texture2D>("CalamityMod/Projectiles/Pets/RotomHeat").Value,
                ModContent.Request<Texture2D>("CalamityMod/Projectiles/Pets/RotomFrost").Value,
                ModContent.Request<Texture2D>("CalamityMod/Projectiles/Pets/RotomMow").Value,
                ModContent.Request<Texture2D>("CalamityMod/Projectiles/Pets/RotomFan").Value);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Drawing(Color.White,
                ModContent.Request<Texture2D>("CalamityMod/Projectiles/Pets/RotomPetGlow").Value,
                ModContent.Request<Texture2D>("CalamityMod/Projectiles/Pets/RotomDexGlow").Value,
                ModContent.Request<Texture2D>("CalamityMod/Projectiles/Pets/RotomWashGlow").Value,
                ModContent.Request<Texture2D>("CalamityMod/Projectiles/Pets/RotomHeatGlow").Value,
                ModContent.Request<Texture2D>("CalamityMod/Projectiles/Pets/RotomFrostGlow").Value,
                ModContent.Request<Texture2D>("CalamityMod/Projectiles/Pets/RotomMowGlow").Value,
                ModContent.Request<Texture2D>("CalamityMod/Projectiles/Pets/RotomFanGlow").Value);
        }

        private void Drawing(Color color, Texture2D normal, Texture2D dex, Texture2D wash, Texture2D heat, Texture2D frost, Texture2D mow, Texture2D fan)
        {
            Texture2D texture = normal;
            switch (RotomType)
            {
                case Form.Dex:
                    texture = dex;
                    break;
                case Form.Wash:
                    texture = wash;
                    break;
                case Form.Heat:
                    texture = heat;
                    break;
                case Form.Frost:
                    texture = frost;
                    break;
                case Form.Mow:
                    texture = mow;
                    break;
                case Form.Fan:
                    texture = fan;
                    break;
                default:
                    break;
            }

            int height = texture.Height / Main.projFrames[Projectile.type];
            int frameHeight = height * Projectile.frame;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, frameHeight, texture.Width, height)), color, Projectile.rotation, new Vector2(texture.Width / 2f, height / 2f), Projectile.scale, spriteEffects, 0);
        }

        public override bool? CanDamage() => false;
    }
}
