﻿using System;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class CrushsawCrasher : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 22;
            Item.damage = 85;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ModContent.ProjectileType<Crushax>();
            Item.shootSpeed = 13.5f;
            Item.DamageType = RogueDamageClass.Instance;
        }

        public override float StealthDamageMultiplier => 0.7f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int spread = 3;
                for (int i = 0; i < 5; i++)
                {
                    Vector2 perturbedspeed = new Vector2(velocity.X + Main.rand.Next(-3, 4), velocity.Y + Main.rand.Next(-3, 4)).RotatedBy(MathHelper.ToRadians(spread));
                    int proj = Projectile.NewProjectile(source, position, perturbedspeed, type, damage, knockback, player.whoAmI);
                    if (proj.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[proj].Calamity().stealthStrike = true;
                    }
                    spread -= Main.rand.Next(1, 4);
                }
                return false;
            }
            return true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
        }
    }
}
