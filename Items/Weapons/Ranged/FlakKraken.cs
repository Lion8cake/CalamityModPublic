﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class FlakKraken : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";

        #region Other Stats
        public static int OriginalUseTime = 63;

        public static float TimeBetweenShots = 21;
        public static float ProjectilesPerBurst = 2f;

        public static float OwnerKnockbackStrength = 2.5f;

        public static float ProjectileGravityStrength = 0.22f;
        public static float ProjectileShootSpeed = 25f;

        public static float InitialShotDamageMultiplier = 1f;
        public static float InitialShotHitShrapnelDamageMultiplier = 0.3f;

        public static int ShrapnelAmount = 5;
        public static float ShrapnelAngleOffset = MathHelper.PiOver4;

        public static int ClusterShrapnelAmount = 8;
        public static float ClusterShrapnelAngleOffset = MathHelper.PiOver2 + MathHelper.ToRadians(50f);
        #endregion

        public override void SetDefaults()
        {
            Item.damage = 124;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = OriginalUseTime;
            Item.shoot = ModContent.ProjectileType<FlakKrakenHoldout>();
            Item.shootSpeed = 15f;

            Item.width = 152;
            Item.height = 58;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.useAmmo = AmmoID.Rocket;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = new SoundStyle("CalamityMod/Sounds/Item/DudFire") with { Volume = .4f, Pitch = -.95f, PitchVariance = 0.1f };
        }

        // Obviously we don't want multiple holdouts existing at the same time.
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0;

        // Spawning the holdout won't consume ammo.
        public override bool CanConsumeAmmo(Item ammo, Player player) => player.ownedProjectileCounts[Item.shoot] != 0;

        // Makes the rotation of the mouse around the player sync in multiplayer.
        public override void HoldItem(Player player) => player.Calamity().mouseWorldListener = true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile holdout = Projectile.NewProjectileDirect(source, player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<FlakKrakenHoldout>(), 0, 0f, player.whoAmI);

            // We set the rotation to the direction to the mouse so the first frame doesn't appear bugged out.
            holdout.velocity = (player.Calamity().mouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FlakToxicannon>().
                AddIngredient<Voidstone>(20).
                AddIngredient<DepthCells>(20).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
