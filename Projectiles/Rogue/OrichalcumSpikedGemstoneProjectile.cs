﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Projectiles.Rogue
{
    public class OrichalcumSpikedGemstoneProjectile : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/OrichalcumSpikedGemstone";

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.aiStyle = ProjAIStyleID.ThrownProjectile;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 360;
            AIType = ProjectileID.ThrowingKnife;
            Projectile.DamageType = RogueDamageClass.Instance;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Vector2 velocity = Projectile.velocity;
            if (Projectile.velocity.Y != velocity.Y && (velocity.Y < -3f || velocity.Y > 3f))
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            }
            if (Projectile.velocity.X != velocity.X)
            {
                Projectile.velocity.X = velocity.X * -0.5f;
            }
            if (Projectile.velocity.Y != velocity.Y && velocity.Y > 1f)
            {
                Projectile.velocity.Y = velocity.Y * -0.5f;
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => OnHitEffect(target.Center);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => OnHitEffect(target.Center);

        private void OnHitEffect(Vector2 targetPos)
        {
            if (Main.myPlayer != Projectile.owner || !Projectile.Calamity().stealthStrike)
                return;

            for (int i = 0; i < 2; i++)
            {
                int direction = Main.player[Projectile.owner].direction;
                float xStart = Main.screenPosition.X;
                if (direction < 0)
                    xStart += Main.screenWidth;
                float yStart = Main.screenPosition.Y + Main.rand.Next(Main.screenHeight);
                Vector2 startPos = new Vector2(xStart, yStart);
                Vector2 pathToTravel = targetPos - startPos;
                pathToTravel.X += Main.rand.NextFloat(-50f, 50f) * 0.1f;
                pathToTravel.Y += Main.rand.NextFloat(-50f, 50f) * 0.1f;
                float speedMult = 24f / pathToTravel.Length();
                pathToTravel.X *= speedMult;
                pathToTravel.Y *= speedMult;
                int petal = Projectile.NewProjectile(Projectile.GetSource_FromThis(), startPos, pathToTravel, ProjectileID.FlowerPetal, Projectile.damage, 0f, Projectile.owner);
                if (petal.WithinBounds(Main.maxProjectiles))
                    Main.projectile[petal].DamageType = RogueDamageClass.Instance;
            }
        }
    }
}
