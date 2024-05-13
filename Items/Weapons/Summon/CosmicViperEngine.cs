﻿using System;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Summon
{
    public class CosmicViperEngine : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Summon";
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 28;
            Item.damage = 321;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item15; //phaseblade sound effect
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CosmicViperSummon>();
            Item.shootSpeed = 10f; // Affects bullet speed
            Item.DamageType = DamageClass.Summon;

            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                float speed = Item.shootSpeed;
                player.itemTime = Item.useTime;
                Vector2 spawnPos = player.RotatedRelativePoint(player.MountedCenter, true);
                float xPos = (float)Main.mouseX + Main.screenPosition.X - spawnPos.X;
                float yPos = (float)Main.mouseY + Main.screenPosition.Y - spawnPos.Y;
                if (player.gravDir == -1f)
                {
                    yPos = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - spawnPos.Y;
                }
                Vector2 vel = new Vector2(xPos, yPos);
                float dist = vel.Length();
                if ((float.IsNaN(vel.X) && float.IsNaN(vel.Y)) || (vel.X == 0f && vel.Y == 0f))
                {
                    vel.X = (float)player.direction;
                    vel.Y = 0f;
                    dist = speed;
                }
                else
                {
                    dist = speed / dist;
                }
                vel.X *= dist;
                vel.Y *= dist;
                spawnPos.X = (float)Main.mouseX + Main.screenPosition.X;
                spawnPos.Y = (float)Main.mouseY + Main.screenPosition.Y;
                vel = vel.RotatedBy(MathHelper.PiOver2, default);
                int p = Projectile.NewProjectile(source, spawnPos + vel, vel, type, damage, knockback, player.whoAmI, 0f, 1f);
                if (Main.projectile.IndexInRange(p))
                    Main.projectile[p].originalDamage = Item.damage;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TacticalPlagueEngine>().
                AddIngredient<CosmiliteBar>(10).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
