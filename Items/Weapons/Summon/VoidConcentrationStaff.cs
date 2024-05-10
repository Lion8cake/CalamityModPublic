﻿using CalamityMod.Buffs.Summon;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Summon
{
    public class VoidConcentrationStaff : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Summon";
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 72;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.DD2_EtherianPortalOpen;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.damage = 105;
            Item.knockBack = 4f;
            Item.useTime = Item.useAnimation = 15; // 14 because of useStyle 1
            Item.shoot = ModContent.ProjectileType<VoidConcentrationAura>();
            Item.shootSpeed = 10f;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse != 2)
                return player.ownedProjectileCounts[ModContent.ProjectileType<VoidConcentrationAura>()] == 0;
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                int p = Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, damage, knockback, player.whoAmI);
                if (Main.projectile.IndexInRange(p))
                    Main.projectile[p].originalDamage = Item.damage;
                player.AddBuff(ModContent.BuffType<VoidConcentrationBuff>(), 120);
            }
            return false;
        }

        // TODO -- should be strictly unnecessary as per API design
        public override bool AltFunctionUse(Player player) => base.AltFunctionUse(player);
    }
}
