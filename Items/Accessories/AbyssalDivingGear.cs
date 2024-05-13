﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    [AutoloadEquip(EquipType.Face)]
    public class AbyssalDivingGear : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetStaticDefaults()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                int equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Face);
                ArmorIDs.Face.Sets.PreventHairDraw[equipSlot] = true;
                ArmorIDs.Face.Sets.OverrideHelmet[equipSlot] = true;
            }
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.depthCharm = true;
            modPlayer.jellyfishNecklace = true;
            player.arcticDivingGear = true;
            player.accFlipper = true;
            player.accDivingHelm = true;
            player.iceSkate = true;
            if (player.wet)
            {
                Lighting.AddLight((int)player.Center.X / 16, (int)player.Center.Y / 16, 0.2f, 0.8f, 0.9f);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ArcticDivingGear).
                AddIngredient<DepthCharm>().
                AddIngredient<DepthCells>(10).
                AddIngredient<Lumenyl>(10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
