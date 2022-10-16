﻿using CalamityMod.Projectiles.Summon;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Summon
{
    [LegacyName("LanternoftheSoul")]
    public class GuidelightofOblivion : ModItem
    {
        public const int ActiveFlameLimit = 15;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guidelight of Oblivion");
            Tooltip.SetDefault("Shadows dream of endless fire, flames devour and embers swoop\n" +
            "Summons a lantern turret to fight for you");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 75;
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.mana = 10;
            Item.width = 42;
            Item.height = 60;
            Item.useTime = Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<LanternSoul>();
            Item.UseSound = SoundID.Item44;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //CalamityUtils.OnlyOneSentry(player, type);
            int p = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
            if (Main.projectile.IndexInRange(p))
                Main.projectile[p].originalDamage = Item.damage;
            player.UpdateMaxTurrets();
            return false;
        }
    }
}
