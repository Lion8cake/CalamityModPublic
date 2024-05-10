﻿using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Fishing.SunkenSeaCatches
{
    public class SparklingEmpress : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Fishing";
        public static int BaseDamage = 10;

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true; //so it doesn't look weird af when holding it
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 44;
            Item.damage = BaseDamage;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.channel = true; //Channel so that you can hold the weapon [Important]
            Item.rare = ItemRarityID.Green;
            Item.mana = 5;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.UseSound = SoundID.Item13;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 14f;
            Item.shoot = ModContent.ProjectileType<SparklingLaser>();
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
        }

        //Looks scuffed with the laser when there is offset
        //public override Vector2? HoldoutOrigin() => new Vector2(10, 10);
    }
}
