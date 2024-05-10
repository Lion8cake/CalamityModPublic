﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Rarities;
using CalamityMod.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class Auralis : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        public static readonly Color blueColor = new Color(0, 77, 255);
        public static readonly Color greenColor = new Color(0, 255, 77);

        public override void SetDefaults()
        {
            Item.width = 96;
            Item.height = 34;
            Item.damage = 695;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = 30;
            Item.knockBack = 10f;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AuralisBullet>();
            Item.shootSpeed = 7.5f;
            Item.useAmmo = AmmoID.Bullet;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.UseSound = CommonCalamitySounds.PlasmaBlastSound;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.Calamity().donorItem = true;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            float damageMult = MathHelper.Lerp(0f, 0.25f, player.Calamity().auralisStealthCounter / 300f);
            damage += damageMult;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI);
            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Ranged/AuralisGlow").Value);
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override void HoldItem(Player player) => player.scope = true;

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.Next(100) >= 50;

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SniperRifle).
                AddIngredient<UelibloomBar>(5).
                AddIngredient<AureusCell>(5).
                AddIngredient<StarblightSoot>(50).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
