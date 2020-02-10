﻿using CalamityMod.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Items.LoreItems
{
    public class KnowledgeDevourerofGods : LoreItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Devourer of Gods");
            Tooltip.SetDefault("This serpent’s power to assimilate the abilities and energy of those it consumed is unique in almost all the known cosmos, save for its lesser brethren.\n" +
                "I would have soon had to eliminate it as a threat had it been given more time and creatures to feast upon.\n" +
                "Place in your inventory to boost your true melee damage by 50%.\n" +
				"However, due to your reckless nature you will take increased damage.");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.rare = 10;
            item.consumable = false;
            item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override void UpdateInventory(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.DoGLore = true;
        }
    }
}
