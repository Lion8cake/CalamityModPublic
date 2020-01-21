﻿using CalamityMod.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Items.Tools.ClimateChange
{
    public class AridArtifact : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arid Artifact");
            Tooltip.SetDefault("Summons a sandstorm\n" +
                               "The sandstorm will happen shortly after the item is used");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 20;
            item.rare = 5;
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = 4;
            item.UseSound = SoundID.Item66;
            item.consumable = true;
        }

        // this is extremely ugly and has to be fully qualified because we add an item called Sandstorm
        public override bool CanUseItem(Player player)
        {
            return CalamityWorld.downedDesertScourge;
        }

        public override bool UseItem(Player player)
        {
            if (Terraria.GameContent.Events.Sandstorm.Happening)
                CalamityUtils.StopSandstorm();
            else
                CalamityUtils.StartSandstorm();
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial);
            recipe.AddRecipeGroup("AnyAdamantiteBar", 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
