﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class EpidemicShredder : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.damage = 80;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4.5f;
            Item.UseSound = SoundID.Item1;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<EpidemicShredderProjectile>();
            Item.shootSpeed = 18f;
            Item.DamageType = RogueDamageClass.Instance;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            float strikeValue = player.Calamity().StealthStrikeAvailable().ToInt(); //0 if false, 1 if true
            int projectileIndex = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<EpidemicShredderProjectile>(), damage, knockback, player.whoAmI, ai1: strikeValue);
            if (player.Calamity().StealthStrikeAvailable() && projectileIndex.WithinBounds(Main.maxProjectiles))
            {
                Main.projectile[projectileIndex].Calamity().stealthStrike = strikeValue == 1f;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ChlorophyteBar, 20).
                AddIngredient(ItemID.Nanites, 150).
                AddIngredient<PlagueCellCanister>(15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
