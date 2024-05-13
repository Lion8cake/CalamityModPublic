﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class Plaguenade : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 28;
            Item.damage = 63;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.consumable = true;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.knockBack = 1.5f;
            Item.maxStack = 9999;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Yellow;
            Item.Calamity().donorItem = true;
            Item.shoot = ModContent.ProjectileType<PlaguenadeProj>();
            Item.shootSpeed = 12f;
            Item.DamageType = RogueDamageClass.Instance;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
                if (proj.WithinBounds(Main.maxProjectiles))
                    Main.projectile[proj].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(150).
                AddIngredient(ItemID.Beenade, 30).
                AddIngredient<PlagueCellCanister>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
