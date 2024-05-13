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
    public class BloodsoakedCrasher : RogueWeapon //This weapon has been coded by Ben || Termi
    {
        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 64;
            Item.damage = 245;
            Item.knockBack = 3f;
            Item.autoReuse = true;
            Item.DamageType = RogueDamageClass.Instance;
            Item.useAnimation = Item.useTime = 24;
            Item.shootSpeed = 9f;
            Item.shoot = ModContent.ProjectileType<BloodsoakedCrashax>();

            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (proj.WithinBounds(Main.maxProjectiles))
                Main.projectile[proj].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CrushsawCrasher>().
                AddIngredient<BloodstoneCore>(12).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
