﻿using CalamityMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    [LegacyName("DeathValley")]
    public class DeathValleyDuster : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 40;
            Item.damage = 83;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 9;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DustProjectile>();
            Item.shootSpeed = 5f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SpellTome).
                AddRecipeGroup("AnyAdamantiteBar", 5).
                AddIngredient(ItemID.AncientBattleArmorMaterial).
                AddIngredient(ItemID.FossilOre, 25).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
