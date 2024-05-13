﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor.FathomSwarmer
{
    [AutoloadEquip(EquipType.Head)]
    public class FathomSwarmerVisage : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.Hardmode";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 8; //41 +10 underwater
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<FathomSwarmerBreastplate>() && legs.type == ModContent.ItemType<FathomSwarmerBoots>();
        }

        public override void PreUpdateVanitySet(Player player)
        {
            player.Calamity().fathomSwarmerTail = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus");
            var modPlayer = player.Calamity();
            modPlayer.fathomSwarmer = true;
            player.spikedBoots = 2;
            player.maxMinions += 2;
            player.GetDamage<SummonDamageClass>() += 0.1f;
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.GetDamage<SummonDamageClass>() += 0.2f;
                player.statDefense += 10;
                player.lifeRegen += 5;
            }
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.Calamity();
            player.GetDamage<SummonDamageClass>() += 0.08f;
            player.maxMinions += 1;
            if (player.breath <= player.breathMax + 2 && !modPlayer.ZoneAbyss)
            {
                player.breath = player.breathMax + 3;
            }
            modPlayer.fathomSwarmerVisage = true;
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                Lighting.AddLight((int)player.Center.X / 16, (int)player.Center.Y / 16, 0.3f, 0.9f, 1.35f);
            }

        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SeaRemains>(5).
                AddIngredient<PlantyMush>(6).
                AddIngredient<DepthCells>(3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
