﻿using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class SpectralstormCannon : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 26;
            Item.damage = 48;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 4;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Flare;
            Item.shootSpeed = 9.5f;
            Item.useAmmo = AmmoID.Flare;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 70)
                return false;
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + (float)Main.rand.Next(-40, 41) * 0.05f;
            float SpeedY = velocity.Y + (float)Main.rand.Next(-40, 41) * 0.05f;
            int flare = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
            if (flare.WithinBounds(Main.maxProjectiles))
            {
                Main.projectile[flare].timeLeft = 200;
                Main.projectile[flare].DamageType = DamageClass.Ranged;
            }

            float SpeedX2 = velocity.X + (float)Main.rand.Next(-20, 21) * 0.05f;
            float SpeedY2 = velocity.Y + (float)Main.rand.Next(-20, 21) * 0.05f;
            int soul = Projectile.NewProjectile(source, position.X, position.Y, SpeedX2, SpeedY2, ModContent.ProjectileType<LostSoulFriendly>(), damage, knockback, player.whoAmI, 2f, 0f);
            if (soul.WithinBounds(Main.maxProjectiles))
            {
                Main.projectile[soul].timeLeft = 600;
                Main.projectile[soul].DamageType = DamageClass.Ranged;
                Main.projectile[soul].frame = Main.rand.Next(4);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FirestormCannon>().
                AddIngredient(ItemID.FragmentSolar, 6).
                AddIngredient(ItemID.Ectoplasm, 10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
