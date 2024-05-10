﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class MangroveChakramProjectile : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/MangroveChakram";

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = ProjAIStyleID.Boomerang;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.WoodenBoomerang;
            Projectile.DamageType = RogueDamageClass.Instance;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 6;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0.25f, 0f);

            if (Main.rand.NextBool(5))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.JungleSpore, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);

            if (Projectile.Calamity().stealthStrike)
            {
                // Die early.
                if (Projectile.timeLeft < 240)
                    Projectile.Kill();

                Projectile.localAI[0] += Main.rand.Next(0, 3);
                if (Projectile.localAI[0] >= 10f)
                {
                    Projectile.localAI[0] = 0f;
                    Vector2 flowerSpawnPosition = Projectile.Center + Main.rand.NextVector2Square(-10f, 10f);
                    Vector2 flowerShootVelocity = Projectile.velocity.RotatedByRandom(0.1f) * 0.25f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), flowerSpawnPosition, flowerShootVelocity, ModContent.ProjectileType<MangroveChakramFlower>(), Projectile.damage / 4, 0f, Projectile.owner);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.Venom, 120);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffID.Venom, 120);
    }
}
