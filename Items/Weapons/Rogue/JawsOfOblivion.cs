﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class JawsOfOblivion : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 40;
            Item.damage = 159;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<JawsProjectile>();
            Item.shootSpeed = 25f;
            Item.DamageType = RogueDamageClass.Instance;

            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
        }

        public override float StealthDamageMultiplier => 1.8f;
        public override float StealthKnockbackMultiplier => 7f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float spreadAngle = MathHelper.ToRadians(2.5f);
            Vector2 direction = velocity;
            Vector2 baseDirection = direction.RotatedBy(-spreadAngle * 2.5f);

            for (int i = 0; i < 6; i++)
            {
                Vector2 currentDirection = baseDirection.RotatedBy(spreadAngle * i);
                currentDirection = currentDirection.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-1f, 1f)));

                if (player.Calamity().StealthStrikeAvailable())
                {
                    int p = Projectile.NewProjectile(source, position, currentDirection, type, damage, knockback, player.whoAmI);
                    if (p.WithinBounds(Main.maxProjectiles))
                        Main.projectile[p].Calamity().stealthStrike = true;
                }
                else
                {
                    Projectile.NewProjectile(source, position, currentDirection, type, damage, knockback, player.whoAmI);
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<LeviathanTeeth>().
                AddIngredient<ReaperTooth>(6).
                AddIngredient<Lumenyl>(15).
                AddIngredient<RuinousSoul>(2).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
