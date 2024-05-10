﻿using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class ForsakenSaber : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 56;
            Item.damage = 80;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.knockBack = 6;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<SandBlade>();
            Item.shootSpeed = 15f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // One randomly spread
            Vector2 spreadVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15f)) * Main.rand.NextFloat(0.8f, 1.2f);
            Projectile.NewProjectile(source, position, spreadVelocity, type, damage / 2, knockback * 0.5f, player.whoAmI);

            // One at the cursor
            Projectile.NewProjectile(source, position, velocity, type, damage / 2, knockback * 0.5f, player.whoAmI);
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Teleporter);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("AnyAdamantiteBar", 5).
                AddIngredient(ItemID.AncientBattleArmorMaterial, 2).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
