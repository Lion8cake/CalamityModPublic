﻿using CalamityMod.Projectiles.Rogue;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class CosmicKunai : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 48;
            Item.damage = 92;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTime = 2;
            Item.useAnimation = 10;
            Item.reuseDelay = 1;
            Item.useLimitPerAnimation = 5;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item109;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.shoot = ModContent.ProjectileType<CosmicKunaiProj>();
            Item.shootSpeed = 28f;
            Item.DamageType = RogueDamageClass.Instance;
        }

        public override float StealthDamageMultiplier => 1.5f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (player.Calamity().StealthStrikeAvailable())
            {
                Main.projectile[stealth].Calamity().stealthStrike = true;
                Main.projectile[stealth].penetrate = 3;
                SoundEngine.PlaySound(SoundID.Item73, player.Center);
                for (float i = 0; i < 5; i++)
                {
                    float angle = MathHelper.TwoPi / 5f * i;
                    Projectile.NewProjectile(source, player.Center, angle.ToRotationVector2() * 8f, ModContent.ProjectileType<CosmicScythe>(), (int)(damage * 1.55f), knockback, player.whoAmI, angle, 0f);
                }
            }
            return false;
        }
    }
}
