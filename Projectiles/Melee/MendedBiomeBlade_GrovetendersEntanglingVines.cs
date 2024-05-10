﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityMod.Projectiles.Melee
{
    public class GrovetendersEntanglingVines : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override string Texture => "CalamityMod/Projectiles/Melee/BrokenBiomeBlade_GrovetendersTouchChain";
        public Player Owner => Main.player[Projectile.owner];
        public float Timer => 20 - Projectile.timeLeft;
        public NPC NPCfrom
        {
            get => Main.npc[(int)Projectile.ai[0]];
            set => Projectile.ai[0] = value.whoAmI;
        }
        public NPC Target
        {
            get => Main.npc[(int)Projectile.ai[1]];
            set => Projectile.ai[1] = value.whoAmI;
        }

        const float curvature = 16f;

        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = Projectile.height = 8;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.timeLeft = 20;
        }

        public override bool? CanHitNPC(NPC target) => target == Target;

        public override void AI()
        {
            Projectile.Center = Target.Center;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D chainTex = Request<Texture2D>("CalamityMod/Projectiles/Melee/BrokenBiomeBlade_GrovetendersTouchChain").Value;

            float opacity = Projectile.timeLeft > 10 ? 1 : Projectile.timeLeft / 10f;
            Vector2 Shake = Projectile.timeLeft < 15 ? Vector2.Zero : Vector2.One.RotatedByRandom(MathHelper.TwoPi) * (15 - Projectile.timeLeft / 5f) * 0.5f;

            Vector2 lineDirection = Utils.SafeNormalize(Target.Center - NPCfrom.Center, Vector2.Zero);
            int dist = (int)Vector2.Distance(Target.Center, NPCfrom.Center) / 16;
            Vector2[] Nodes = new Vector2[dist + 1];
            Nodes[0] = NPCfrom.Center;
            Nodes[dist] = Target.Center;
            float pointUp = Target.Center.X > NPCfrom.Center.X ? -MathHelper.PiOver2 : MathHelper.PiOver2;

            for (int i = 1; i < dist + 1; i++)
            {
                Vector2 positionAlongLine = Vector2.Lerp(NPCfrom.Center, Target.Center, i / (float)dist); //Get the position of the segment along the line, as if it were a flat line
                float elevation = (float)Math.Sin(i / (float)dist * MathHelper.Pi) * curvature * dist / 10f;
                Nodes[i] = positionAlongLine + lineDirection.RotatedBy(pointUp) * elevation + Shake * (float)Math.Sin(i / (float)dist * MathHelper.Pi);

                float rotation = (Nodes[i] - Nodes[i - 1]).ToRotation() - MathHelper.PiOver2; //Calculate rotation based on direction from last point
                float yScale = Vector2.Distance(Nodes[i], Nodes[i - 1]) / chainTex.Height; //Calculate how much to squash/stretch for smooth chain based on distance between points
                Vector2 scale = new Vector2(1, yScale);

                Color chainLightColor = Lighting.GetColor((int)Nodes[i].X / 16, (int)Nodes[i].Y / 16); //Lighting of the position of the chain segment

                Vector2 origin = new Vector2(chainTex.Width / 2, chainTex.Height); //Draw from center bottom of texture
                Main.EntitySpriteDraw(chainTex, Nodes[i] - Main.screenPosition, null, chainLightColor * opacity, rotation, origin, scale, SpriteEffects.None, 0);
            }
            return false;
        }

    }
}
