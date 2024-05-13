﻿using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Items.Weapons.Rogue
{
    public class OrichalcumSpikedGemstone : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 34;
            Item.damage = 40;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 13;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.maxStack = 9999;
            Item.value = 1200;
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = ModContent.ProjectileType<OrichalcumSpikedGemstoneProjectile>();
            Item.shootSpeed = 12f;
            Item.DamageType = RogueDamageClass.Instance;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int gemstone = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (gemstone.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[gemstone].Calamity().stealthStrike = true;
                    Main.projectile[gemstone].timeLeft = 600;
                }
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(150).
                AddIngredient(ItemID.OrichalcumBar).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
