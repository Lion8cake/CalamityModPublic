﻿using CalamityMod.Projectiles.Melee;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Tools
{
    //Its not like its a renamed version of the spear, but i put this here more as a way to "refund" the item, so it doesnt end up rotting as an unloaded item.
    [LegacyName("MarniteSpear")]
    public class MarniteDeconstructor : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Tools";
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 18;
            Item.damage = 6;
            Item.ArmorPenetration = 10;
            Item.knockBack = 0f;
            Item.useTime = 7;
            Item.useAnimation = 25;
            Item.hammer = 40;

            Item.DamageType = TrueMeleeNoSpeedDamageClass.Instance;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item23;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MarniteDeconstructorProj>();
            Item.shootSpeed = 40f;
            Item.tileBoost = 7;
        }

        public override void HoldItem(Player player)
        {
            player.Calamity().mouseWorldListener = true;
        }


        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Sapphire).
                AddRecipeGroup("AnyGoldBar", 3).
                AddIngredient(ItemID.Granite, 5).
                AddIngredient(ItemID.Marble, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
