﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    [LegacyName("Gehenna")]
    public class SlagsplitterPauldron : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 56;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.Pauldron = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
            AddIngredient<ScorchedBone>(12).
            AddIngredient<DemonicBoneAsh>(3).
            AddIngredient<EssenceofHavoc>(8).
            AddTile(TileID.Anvils).
            Register();
        }
    }
}
