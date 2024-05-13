﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class SomaPrime : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        private static readonly float XYInaccuracy = 0.32f;

        public override void SetDefaults()
        {
            Item.width = 94;
            Item.height = 34;
            Item.damage = 400;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.BulletHighVelocity;
            Item.shootSpeed = 9f;
            Item.useAmmo = AmmoID.Bullet;

            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().devItem = true;
            Item.Calamity().canFirePointBlankShots = true;
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 30;

        public override Vector2? HoldoutOffset() => new Vector2(-25, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == ProjectileID.Bullet)
            {
                type = ProjectileID.BulletHighVelocity;
                damage += 4; // in 1.4, HVBs deal 11 damage and Musket Balls deal 7
            }

            velocity.X += Main.rand.NextFloat(-XYInaccuracy, XYInaccuracy);
            velocity.Y += Main.rand.NextFloat(-XYInaccuracy, XYInaccuracy);
            Vector2 vel = velocity;
            Projectile shot = Projectile.NewProjectileDirect(source, position, vel, type, damage, knockback, player.whoAmI);
            CalamityGlobalProjectile cgp = shot.Calamity();
            cgp.supercritHits = -1;
            cgp.appliesSomaShred = true;
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() > 0.8f;

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Kingsbane>().
                AddIngredient<ClockGatlignum>().
                AddIngredient<ShadowspecBar>(5).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
