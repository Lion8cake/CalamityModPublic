﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class Butcher : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 12;
            Item.damage = 15;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1f;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item38;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.channel = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ButcherGun>();
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        // Spawning the holdout cannot consume ammo
        public override bool CanConsumeAmmo(Item ammo, Player player) => !Main.rand.NextBool(4) && player.ownedProjectileCounts[Item.shoot] > 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ButcherGun>(), damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Shotgun).
                AddIngredient(ItemID.IllegalGunParts).
                AddRecipeGroup("AnyCobaltBar", 5).
                AddIngredient<EssenceofHavoc>(4).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
