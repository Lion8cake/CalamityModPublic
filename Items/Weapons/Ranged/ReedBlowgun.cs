﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    [LegacyName("Seabow")]
    public class ReedBlowgun : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        public static readonly SoundStyle BubbleBurstSound = new("CalamityMod/Sounds/Custom/PistolShrimpBubbleBurst") { PitchVariance = 0.15f, Volume = 0.2f };

        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsRangedSpecialistWeapon[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 46;
            Item.damage = 21;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.holdStyle = 16;
            Item.noMelee = true;
            Item.knockBack = 4.5f;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PressurizedBubbleStream>();
            Item.shootSpeed = 16f;
        }

        public override void HoldItem(Player player)
        {
            player.Calamity().mouseWorldListener = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(0.01f);
        }

        public static Vector2 getPlayerMouth(Player player) => player.MountedCenter - 5f * Vector2.UnitY * player.gravDir + Vector2.UnitX * 6f * player.direction;
        public static Vector2 getPlayerShoulder(Player player) => player.MountedCenter - Vector2.UnitX * 4f * player.direction;

        public void SetItemInHand(Player player, Rectangle heldItemFrame)
        {
            //Make the player face where they're aiming.
            if (Main.MouseWorld.X > player.Center.X)
            {
                player.ChangeDir(1);
            }
            else
            {
                player.ChangeDir(-1);
            }

            Vector2 playerMouth = getPlayerMouth(player);

            Vector2 playerToCursor = (player.Calamity().mouseWorld - playerMouth).SafeNormalize(Vector2.UnitX);
            float pointingDirection = (playerToCursor.ToRotation() + MathHelper.PiOver4 / 3f * player.direction * player.gravDir);

            CalamityUtils.CleanHoldStyle(player, pointingDirection, playerMouth, new Vector2(50, 18), new Vector2(-23, 6));
        }


        public void SetPlayerArms(Player player, bool frontArm = false)
        {
            //Calculate the dirction in which the players arms should be pointing at.
            Vector2 playerToCursor = (player.Calamity().mouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            float backArmDirection = playerToCursor.ToRotation();

            Vector2 playerMouth = getPlayerMouth(player);
            Vector2 mouthToCursor = (player.Calamity().mouseWorld - playerMouth).SafeNormalize(Vector2.UnitX);

            float frontArmDirection = (playerMouth + mouthToCursor * 25f - getPlayerShoulder(player)).ToRotation();

            if (frontArm)
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, frontArmDirection * player.gravDir - MathHelper.PiOver2);

            player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, backArmDirection * player.gravDir - MathHelper.PiOver2);
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            SetItemInHand(player, heldItemFrame);
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            SetItemInHand(player, heldItemFrame);
        }

        public override void HoldItemFrame(Player player)
        {
            SetPlayerArms(player);
        }

        public override void UseItemFrame(Player player)
        {
            SetPlayerArms(player, true);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SeaRemains>(2).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
