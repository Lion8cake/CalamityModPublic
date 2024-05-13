﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class CrystalPiercer : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 62;
            Item.damage = 92;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.maxStack = 9999;
            Item.value = 2500;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<CrystalPiercerProjectile>();
            Item.shootSpeed = 20f;
            Item.DamageType = RogueDamageClass.Instance;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (stealth.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                    Main.projectile[stealth].aiStyle = -1;
                    Main.projectile[stealth].tileCollide = false;
                    Main.projectile[stealth].usesLocalNPCImmunity = true;
                }
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(150).
                AddIngredient<CryonicBar>().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
