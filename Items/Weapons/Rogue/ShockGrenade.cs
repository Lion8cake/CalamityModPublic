﻿using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class ShockGrenade : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 30;
            Item.damage = 108;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.consumable = true;
            Item.useAnimation = Item.useTime = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1f;
            Item.autoReuse = true;
            Item.maxStack = 9999;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<ShockGrenadeProjectile>();
            Item.shootSpeed = 12.5f;
            Item.DamageType = RogueDamageClass.Instance;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (p.WithinBounds(Main.maxProjectiles))
                    Main.projectile[p].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Rogue/ShockGrenadeGlow").Value);
        }

        public override void AddRecipes()
        {
            CreateRecipe(150).
                AddIngredient(ItemID.Grenade, 30).
                AddIngredient(ItemID.MartianConduitPlating, 5).
                AddIngredient(ItemID.Nanites, 5).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
