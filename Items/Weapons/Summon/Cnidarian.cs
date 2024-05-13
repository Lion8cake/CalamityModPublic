﻿using System.Linq;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityMod.Items.Weapons.Summon
{
    public class Cnidarian : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Summon";
        public override string Texture => "CalamityMod/Items/Weapons/Summon/CnidarianFishingRod";

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 26;
            Item.DamageType = DamageClass.Summon;
            Item.damage = 8;
            Item.knockBack = 3f;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.autoReuse = true;

            Item.holdStyle = 16; //Custom hold style
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<CnidarianJellyfishOnTheString>();
            Item.shootSpeed = 10f;

            Item.rare = ItemRarityID.Green;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.projectile.Any(n => n.active && n.owner == player.whoAmI && n.type == ProjectileType<CnidarianJellyfishOnTheString>());
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SeaRemains>(2).
                AddTile(TileID.Anvils).
                Register();
        }

        public override void HoldItem(Player player)
        {
            player.Calamity().mouseWorldListener = true;
        }


        #region drawing stuff
        public void SetItemInHand(Player player, Rectangle heldItemFrame)
        {
            //Make the player face where they're aiming.
            if (player.Calamity().mouseWorld.X > player.Center.X)
            {
                player.ChangeDir(1);
            }
            else
            {
                player.ChangeDir(-1);
            }


            CalamityUtils.CleanHoldStyle(player, player.compositeFrontArm.rotation + MathHelper.PiOver2 * player.gravDir, player.GetFrontHandPositionImproved(player.compositeFrontArm), new Vector2(42, 34), new Vector2(-15, 11), true);
        }


        public void SetPlayerArms(Player player)
        {
            //Calculate the dirction in which the players arms should be pointing at.
            Vector2 playerToCursor = (player.Calamity().mouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            float armPointingDirection = (playerToCursor.ToRotation());


            //"crop" the rotation so the player only tilts the fishing rod slightly up and slightly down.
            if (armPointingDirection < MathHelper.PiOver2 && armPointingDirection >= -MathHelper.PiOver2)
            {
                armPointingDirection = -MathHelper.PiOver2 + MathHelper.PiOver4 + MathHelper.PiOver2 * Utils.GetLerpValue(0f, MathHelper.Pi, armPointingDirection + MathHelper.PiOver2, true);
            }

            //It gets a bit harder if its pointing left; ouch
            else
            {
                if (armPointingDirection > 0)
                    armPointingDirection = MathHelper.PiOver2 + MathHelper.PiOver4 + MathHelper.PiOver4 * Utils.GetLerpValue(0f, MathHelper.PiOver2, armPointingDirection - MathHelper.PiOver2, true);
                else
                    armPointingDirection = -MathHelper.Pi + MathHelper.PiOver4 * Utils.GetLerpValue(-MathHelper.Pi, -MathHelper.PiOver4, armPointingDirection, true);
            }

            player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, armPointingDirection * player.gravDir - MathHelper.PiOver2);
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, armPointingDirection * player.gravDir - MathHelper.PiOver2);
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
            SetPlayerArms(player);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D properSprite = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Summon/Cnidarian").Value;

            spriteBatch.DrawNewInventorySprite(properSprite, new Vector2(42f, 34), position, drawColor, origin, scale);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D properSprite = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Summon/Cnidarian").Value;

            spriteBatch.Draw(properSprite, Item.position - Main.screenPosition, null, lightColor, rotation, properSprite.Size() / 2f, scale, 0, 0);
            return false;
        }
        #endregion
    }
}
