﻿using CalamityMod.Buffs.StatBuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class InfestedClawmerangProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/InfestedClawmerang";

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = ProjAIStyleID.Boomerang;
            Projectile.timeLeft = 300;
            AIType = ProjectileID.WoodenBoomerang;
            Projectile.DamageType = RogueDamageClass.Instance;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 0.35f / 255f, (255 - Projectile.alpha) * 0.5f / 255f);
            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueFairy, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            if (Projectile.Calamity().stealthStrike)
            {
                if (Projectile.timeLeft % 15 == 0 && Projectile.owner == Main.myPlayer)
                {
                    Vector2 vector62 = Main.player[Projectile.owner].Center - Projectile.Center;
                    Vector2 vector63 = vector62 * -1f;
                    vector63.Normalize();
                    vector63 *= (float)Main.rand.Next(45, 65) * 0.1f;
                    vector63 = vector63.RotatedBy((Main.rand.NextDouble() - 0.5) * 1.5707963705062866, default);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vector63.X, vector63.Y, ModContent.ProjectileType<ShroomerangSpore>(), (int)(Projectile.damage * 0.1), Projectile.knockBack * 0.2f, Projectile.owner, -10f, 0f);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.Calamity().stealthStrike)
                player.AddBuff(ModContent.BuffType<Mushy>(), 720);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.Calamity().stealthStrike)
                player.AddBuff(ModContent.BuffType<Mushy>(), 720);
        }
    }
}
