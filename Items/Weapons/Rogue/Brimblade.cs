﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class Brimblade : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.damage = 28;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.knockBack = 6.5f;
            Item.UseSound = SoundID.Item1;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<BrimbladeProj>();
            Item.shootSpeed = 12f;
            Item.DamageType = RogueDamageClass.Instance;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int blade = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (blade.WithinBounds(Main.maxProjectiles))
                    Main.projectile[blade].Calamity().stealthStrike = true;

                for (int i = -6; i <= 6; i += 4)
                {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(i));
                    int dart = Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<SeethingDischargeBrimstoneBarrage>(), damage, knockback * 0.5f, player.whoAmI);
                    if (dart.WithinBounds(Main.maxProjectiles))
                        Main.projectile[dart].DamageType = RogueDamageClass.Instance;
                }
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<UnholyCore>(4).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
