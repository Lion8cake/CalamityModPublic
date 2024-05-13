﻿using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class LeonidProgenitor : RogueWeapon
    {
        public static readonly Color blueColor = new Color(48, 208, 255);
        public static readonly Color purpleColor = new Color(208, 125, 218);

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 48;
            Item.damage = 64;
            Item.DamageType = RogueDamageClass.Instance;
            Item.knockBack = 3f;
            Item.useTime = Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<LeonidProgenitorBombshell>();
            Item.shootSpeed = 12f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item61;

            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.Calamity().StealthStrikeAvailable() || player.altFunctionUse != 2)
            {
                Item.UseSound = SoundID.Item61;
                Item.shoot = ModContent.ProjectileType<LeonidProgenitorBombshell>();
            }
            else
            {
                Item.UseSound = SoundID.Item88;
                Item.shoot = ModContent.ProjectileType<LeonidCometSmall>();
            }
            return base.CanUseItem(player);
        }

        public override float UseSpeedMultiplier(Player player)
        {
            if (player.Calamity().StealthStrikeAvailable() || player.altFunctionUse != 2)
                return 1f;
            return 0.8f;
        }

        public override float StealthDamageMultiplier => 1.25f;

        public override void ModifyStatsExtra(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (!player.Calamity().StealthStrikeAvailable() && player.altFunctionUse == 2)
                damage = (int)(damage * 0.5f);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable() || player.altFunctionUse != 2)
            {
                int bomb = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (bomb.WithinBounds(Main.maxProjectiles))
                    Main.projectile[bomb].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
                return false;
            }
            else
            {
                for (float i = -2.5f; i < 3f; ++i)
                {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(i));
                    Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
                }
            }
            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Rogue/LeonidProgenitorGlow").Value);
        }
    }
}
