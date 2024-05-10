﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    public class AbyssalMirror : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 38;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.stealthGenStandstill += 0.25f;
            modPlayer.stealthGenMoving += 0.12f;
            modPlayer.abyssalMirror = true;
            player.aggro -= 450;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MirageMirror>().
                AddIngredient<InkBomb>().
                AddIngredient<DepthCells>(5).
                AddIngredient<Lumenyl>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
