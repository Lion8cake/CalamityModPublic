﻿using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class IonBlaster : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 28;
            Item.damage = 29;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5.5f;
            Item.UseSound = SoundID.Item91;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<IonBlast>();
            Item.shootSpeed = 3f;

            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.rare = ItemRarityID.Pink;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            float manaRatio = (float)player.statMana / player.statManaMax2;
            bool injectionNerf = player.Calamity().astralInjection;
            if (injectionNerf)
                manaRatio = MathHelper.Min(manaRatio, 0.65f);

            // 20% to 160% damage. Astral Injection caps it at 111% damage.
            float damageRatio = 0.2f + 1.4f * manaRatio;
            damage = (int)(damage * damageRatio);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float manaRatio = (float)player.statMana / player.statManaMax2;
            bool injectionNerf = player.Calamity().astralInjection;
            if (injectionNerf)
                manaRatio = MathHelper.Min(manaRatio, 0.65f);

            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            proj.scale = 0.75f + 0.75f * manaRatio;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("AnyAdamantiteBar", 7).
                AddIngredient(ItemID.SoulofFright, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
