﻿using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Summon
{
    public class ForgottenApexWand : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Summon";
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 48;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Summon;
            Item.UseSound = SoundID.Item89;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.autoReuse = true;

            Item.knockBack = 4f;
            Item.mana = 10;
            Item.damage = 28;
            Item.useTime = Item.useAnimation = 25;
            Item.shoot = ModContent.ProjectileType<ApexShark>();
            Item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                player.itemTime = Item.useTime;
                Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
                vector2.X = Main.mouseX + Main.screenPosition.X;
                vector2.Y = Main.mouseY + Main.screenPosition.Y;
                int p = Projectile.NewProjectile(source, vector2, Vector2.Zero, ModContent.ProjectileType<ApexShark>(), damage, knockback, player.whoAmI, 0f, 0f);
                if (Main.projectile.IndexInRange(p))
                    Main.projectile[p].originalDamage = Item.damage;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("AnyAdamantiteBar", 5).
                AddIngredient(ItemID.AncientBattleArmorMaterial, 2).
                AddIngredient(ItemID.Amethyst, 4).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
