﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    [AutoloadEquip(EquipType.Shoes)]
    public class AngelTreads : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 36;
            Item.value = CalamityGlobalItem.RarityLightPurpleBuyPrice;
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.angelTreads = true;
            player.accRunSpeed = 7.5f;
            player.rocketBoots = player.vanityRocketBoots = 3;
            player.moveSpeed += 0.12f;
            player.iceSkate = true;
            player.waterWalk = true;
            player.fireWalk = true;
            player.lavaMax += 240;
            player.buffImmune[BuffID.OnFire] = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.vanityRocketBoots = 3;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.TerrasparkBoots).
                AddIngredient<HarpyRing>().
                AddIngredient<EssenceofSunlight>(5).
                AddIngredient(ItemID.SoulofFright).
                AddIngredient(ItemID.SoulofMight).
                AddIngredient(ItemID.SoulofSight).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
