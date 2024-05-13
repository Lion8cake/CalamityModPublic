﻿using System;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class PhoenixFlameBarrage : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 106;
            Item.height = 140;
            Item.damage = 140;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 35;
            Item.useTime = 15;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<HolyFlame>();
            Item.shootSpeed = 30f;
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float flameSpeed = Item.shootSpeed;
            player.itemTime = Item.useTime;
            Vector2 realPlayerPos = player.RotatedRelativePoint(player.MountedCenter, true);
            float mouseXDist = (float)Main.mouseX + Main.screenPosition.X - realPlayerPos.X;
            float mouseYDist = (float)Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
            if (player.gravDir == -1f)
            {
                mouseYDist = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - realPlayerPos.Y;
            }
            float mouseDistance = (float)Math.Sqrt((double)(mouseXDist * mouseXDist + mouseYDist * mouseYDist));
            if ((float.IsNaN(mouseXDist) && float.IsNaN(mouseYDist)) || (mouseXDist == 0f && mouseYDist == 0f))
            {
                mouseXDist = (float)player.direction;
                mouseYDist = 0f;
                mouseDistance = flameSpeed;
            }
            else
            {
                mouseDistance = flameSpeed / mouseDistance;
            }

            int flameAmt = 30;
            if (Main.rand.NextBool(3))
            {
                flameAmt++;
            }
            if (Main.rand.NextBool(3))
            {
                flameAmt++;
            }
            for (int i = 0; i < flameAmt; i++)
            {
                realPlayerPos = new Vector2(player.position.X + (float)player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)player.direction) + ((float)Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
                realPlayerPos.X = (realPlayerPos.X + player.Center.X) / 2f + (float)Main.rand.Next(-200, 201);
                realPlayerPos.Y -= (float)(100 * i);
                mouseXDist = (float)Main.mouseX + Main.screenPosition.X - realPlayerPos.X;
                mouseYDist = (float)Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
                if (mouseYDist < 0f)
                {
                    mouseYDist *= -1f;
                }
                if (mouseYDist < 20f)
                {
                    mouseYDist = 20f;
                }
                mouseDistance = (float)Math.Sqrt((double)(mouseXDist * mouseXDist + mouseYDist * mouseYDist));
                mouseDistance = flameSpeed / mouseDistance;
                mouseXDist *= mouseDistance;
                mouseYDist *= mouseDistance;
                float speedX4 = mouseXDist + (float)Main.rand.Next(-30, 31) * 0.02f;
                float speedY5 = mouseYDist + (float)Main.rand.Next(-30, 31) * 0.02f;
                Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, speedX4, speedY5, type, damage, knockback, player.whoAmI, 0f, (float)Main.rand.Next(15));
            }
            return false;
        }
    }
}
