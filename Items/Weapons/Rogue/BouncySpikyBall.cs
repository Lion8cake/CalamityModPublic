﻿using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class BouncySpikyBall : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.damage = 10;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.value = Item.buyPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<BouncyBol>();
            Item.shootSpeed = 8f;
            Item.DamageType = RogueDamageClass.Instance;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int spread = 3;
                for (int i = 0; i < 4; i++)
                {
                    Vector2 perturbedspeed = new Vector2(velocity.X + Main.rand.Next(-3, 4), velocity.Y + Main.rand.Next(-3, 4)).RotatedBy(MathHelper.ToRadians(spread));
                    int proj = Projectile.NewProjectile(source, position, perturbedspeed, type, damage, knockback, player.whoAmI);
                    if (proj.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[proj].Calamity().stealthStrike = true;
                    }
                    spread -= Main.rand.Next(1, 4);
                }
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).
                AddIngredient(ItemID.SpikyBall, 100).
                AddIngredient(ItemID.PinkGel).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
