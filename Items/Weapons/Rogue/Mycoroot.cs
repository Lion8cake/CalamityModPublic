﻿using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class Mycoroot : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 12;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.shoot = ModContent.ProjectileType<MycorootProj>();
            Item.shootSpeed = 20f;
            Item.DamageType = RogueDamageClass.Instance;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            if (player.Calamity().StealthStrikeAvailable() && player.ownedProjectileCounts[ModContent.ProjectileType<ShroomerangSpore>()] < 20 && stealth.WithinBounds(Main.maxProjectiles))
            {
                Main.projectile[stealth].Calamity().stealthStrike = true;
                int projAmt = Main.rand.Next(7, 11);
                for (int i = 0; i < projAmt; i++)
                {
                    int spore = Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<ShroomerangSpore>(), (int)(damage * 0.5f), knockback, player.whoAmI);
                    if (spore.WithinBounds(Main.maxProjectiles))
                        Main.projectile[spore].ai[1] = 1f;
                }
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player other = Main.player[i];
                    if (other is null || !other.active || other.dead)
                        continue;
                    if ((other.team == player.team && player.team != 0) || player.whoAmI == i)
                    {
                        if (player.Distance(other.Center) <= 800f)
                            other.AddBuff(ModContent.BuffType<Mushy>(), 900);
                    }
                }
            }
            return false;
        }
    }
}
