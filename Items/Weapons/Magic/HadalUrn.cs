﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.FurnitureVoid;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class HadalUrn : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";
        public static readonly SoundStyle ShootSound = new("CalamityMod/Sounds/Item/HadalUrnOpen");
        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 38;
            Item.damage = 37;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.75f;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.Calamity().donorItem = true;
            Item.UseSound = ShootSound;
            Item.autoReuse = true;
            Item.shootSpeed = 8f;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<HadalUrnHoldout>();
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item103 with { Volume = SoundID.Item103.Volume }, player.Center);
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<HadalUrnHoldout>(), damage, knockback, player.whoAmI, 12);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BlackAnurian>().
                AddIngredient<SmoothVoidstone>(20).
                AddIngredient<Lumenyl>(5).
                AddIngredient<DepthCells>(15).
                AddIngredient(ItemID.Bone, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
