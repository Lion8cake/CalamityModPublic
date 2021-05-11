using CalamityMod.CalPlayer;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalamityMod.Items.Accessories
{
    public class FabledTortoiseShell : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flame-Licked Shell");
            Tooltip.SetDefault("50% reduced movement speed\n" +
                                "Enemies take damage when they hit you\n" +
                                "You move faster and lose 18 defense for 3 seconds if you take damage");
        }

        public override void SetDefaults()
        {
            item.defense = 36;
            item.width = 36;
            item.height = 42;
            item.value = CalamityGlobalItem.Rarity5BuyPrice;
            item.rare = ItemRarityID.Pink;
            item.accessory = true;
			item.Calamity().challengeDrop = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.fabledTortoise = true;
			float moveSpeedDecrease = modPlayer.shellBoost ? 0.2f : 0.5f;
            player.moveSpeed -= moveSpeedDecrease;
            player.thorns += 0.25f;
			if (modPlayer.shellBoost)
				player.statDefense -= 18;
        }
    }
}
