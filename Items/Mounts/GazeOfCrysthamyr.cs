﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Mounts
{
    public class GazeOfCrysthamyr : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Mounts";
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;

            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.Calamity().donorItem = true;

            Item.UseSound = SoundID.NPCHit56;
            Item.noMelee = true;
            Item.mountType = ModContent.MountType<Crysthamyr>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.DD2PetDragon).
                AddIngredient(ItemID.SoulofNight, 10).
                AddIngredient<DarksunFragment>(10).
                AddIngredient<ExodiumCluster>(25).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
