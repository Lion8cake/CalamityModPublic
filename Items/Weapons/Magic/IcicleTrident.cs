﻿using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class IcicleTrident : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 44;
            Item.damage = 69;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 21;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TridentIcicle>();
            Item.shootSpeed = 12f;
        }

        public override Vector2? HoldoutOrigin() => new Vector2(15, 15);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 speed = velocity;
            Projectile.NewProjectile(source, position, speed, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, speed.RotatedBy(MathHelper.ToRadians(5)), type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, speed.RotatedBy(MathHelper.ToRadians(-5)), type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
