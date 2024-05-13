﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class Omniblade : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 146;
            Item.damage = 100;
            Item.DamageType = TrueMeleeNoSpeedDamageClass.Instance;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.useTurn = true;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<OmnibladeSwing>();
            Item.shootSpeed = 24f;
            Item.rare = ItemRarityID.Yellow;
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 45;

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Katana).
                AddIngredient<LifeAlloy>(5).
                AddIngredient<CoreofCalamity>(2).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
