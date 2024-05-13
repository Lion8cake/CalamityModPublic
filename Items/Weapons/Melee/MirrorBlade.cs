﻿using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class MirrorBlade : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        private int baseDamage = 100;

        public override void SetDefaults()
        {
            Item.width = 72;
            Item.height = 72;
            Item.damage = baseDamage;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 12;
            Item.useTime = 12;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.shootSpeed = 9f;
            Item.shoot = ModContent.ProjectileType<MirrorBlast>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            int conDamage = target.damage + baseDamage;
            if (conDamage < baseDamage)
            {
                conDamage = baseDamage;
            }
            if (conDamage > 400)
            {
                conDamage = 400;
            }
            Item.damage = conDamage;
        }
    }
}
