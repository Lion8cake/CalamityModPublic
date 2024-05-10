﻿using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Tools
{
    public class SkyfringePickaxe : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Tools";
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 44;
            Item.damage = 12;
            Item.knockBack = 2.5f;
            Item.useTime = 12;
            Item.useAnimation = 16;
            Item.pick = 105;
            Item.tileBoost += 1;

            Item.DamageType = DamageClass.Melee;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AerialiteBar>(7).
                AddIngredient(ItemID.SunplateBlock, 3).
                AddTile(TileID.SkyMill).
                Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.BlueTorch);
            }
        }
    }
}
