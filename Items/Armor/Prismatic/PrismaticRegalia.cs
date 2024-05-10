﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor.Prismatic
{
    [AutoloadEquip(EquipType.Body)]
    public class PrismaticRegalia : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.PostMoonLord";
        public override void SetStaticDefaults()
        {

            if (Main.netMode == NetmodeID.Server)
                return;

            int equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);

            ArmorIDs.Body.Sets.HidesTopSkin[equipSlot] = true;
            ArmorIDs.Body.Sets.HidesArms[equipSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 33;

            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.Calamity().donorItem = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.Calamity().prismaticRegalia = true;
            player.statLifeMax2 += 20;
            player.statManaMax2 += 40;
            player.GetDamage<MagicDamageClass>() += 0.12f;
            player.GetCritChance<MagicDamageClass>() += 15;
            player.GetDamage<GenericDamageClass>() -= 0.2f;
            player.GetDamage<MagicDamageClass>() += 0.2f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ArmoredShell>(3).
                AddIngredient<ExodiumCluster>(5).
                AddIngredient<DivineGeode>(8).
                AddIngredient(ItemID.Nanites, 300).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
