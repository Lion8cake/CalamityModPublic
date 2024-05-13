﻿using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    // only renamed because vanilla added a Shroomerang
    [LegacyName("Shroomerang")]
    public class InfestedClawmerang : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 50;
            Item.damage = 18;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.shoot = ModContent.ProjectileType<InfestedClawmerangProj>();
            Item.shootSpeed = 15f;
            Item.DamageType = RogueDamageClass.Instance;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (stealth.WithinBounds(Main.maxProjectiles))
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
    }
}
