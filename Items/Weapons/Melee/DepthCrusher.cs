﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    [LegacyName("DepthBlade")]
    public class DepthCrusher : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";

        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 50;
            Item.damage = 35;
            Item.knockBack = 7.25f;
            Item.useTime = 65;
            Item.useAnimation = 65;
            Item.shoot = ModContent.ProjectileType<DepthCrusherProjectile>();
            Item.shootSpeed = 3f;

            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = null;
            Item.autoReuse = true;

            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.rare = ItemRarityID.Green;
        }
    }
}
