﻿using System;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class DivineRetribution : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 88;
            Item.damage = 48;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;

            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.Calamity().donorItem = true;

            Item.UseSound = SoundID.Item73;
            Item.autoReuse = true;
            Item.shootSpeed = 19f;
            Item.shoot = ModContent.ProjectileType<DivineRetributionSpear>();
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float spearSpeed = Item.shootSpeed;
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
            }
            else
            {
                mouseDistance = spearSpeed / mouseDistance;
            }
            int numProjectiles = 5;
            for (int i = 0; i < numProjectiles; i++)
            {
                realPlayerPos = new Vector2(player.position.X + (float)player.width * 0.5f + (float)(Main.rand.Next(51) * -(float)player.direction) + ((float)Main.mouseX + Main.screenPosition.X - /* - */ player.position.X), player.MountedCenter.Y + 600f); //-
                realPlayerPos.X = (realPlayerPos.X + player.Center.X) / 2f + (float)Main.rand.Next(-50, 51); //200
                realPlayerPos.Y += (float)(100 * i); //-=
                mouseXDist = (float)Main.mouseX + Main.screenPosition.X - realPlayerPos.X; //+ -
                mouseYDist = (float)Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y; //+ -
                if (mouseYDist < 0f)
                {
                    mouseYDist *= -1f;
                }
                if (mouseYDist < 20f)
                {
                    mouseYDist = 20f;
                }
                mouseDistance = (float)Math.Sqrt((double)(mouseXDist * mouseXDist + mouseYDist * mouseYDist));
                mouseDistance = spearSpeed / mouseDistance;
                mouseXDist *= mouseDistance;
                mouseYDist *= mouseDistance;
                float speedX6 = mouseXDist + (float)Main.rand.Next(-60, 61) * 0.02f;
                float speedY7 = mouseYDist + (float)Main.rand.Next(-60, 61) * 0.02f;
                float ai1 = Main.rand.NextFloat() + 0.5f;
                Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, speedX6, -speedY7, type, damage, knockback, player.whoAmI, 0.0f, ai1);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<UndinesRetribution>().
                AddIngredient<DivineGeode>(8).
                AddIngredient<UelibloomBar>(6).
                AddIngredient<UnholyEssence>(10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
