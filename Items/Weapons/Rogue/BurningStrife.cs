﻿using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class BurningStrife : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 28;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.damage = 96;
            Item.shootSpeed = 8f;
            Item.shoot = ModContent.ProjectileType<BurningStrifeProj>();

            Item.DamageType = RogueDamageClass.Instance;
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 8;

        public override float StealthDamageMultiplier => 1.3f;
        public override float StealthVelocityMultiplier => 1.25f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (player.Calamity().StealthStrikeAvailable() && proj.WithinBounds(Main.maxProjectiles))
            {
                Main.projectile[proj].Calamity().stealthStrike = true;
                Main.projectile[proj].penetrate = 5;
            }
            return false;
        }
    }
}
