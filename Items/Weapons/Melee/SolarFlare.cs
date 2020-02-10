using CalamityMod.Projectiles.Melee.Yoyos;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class SolarFlare : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Flare");
            Tooltip.SetDefault("Emits large holy explosions on hit\n" +
			"A very agile yoyo");
            ItemID.Sets.Yoyo[item.type] = true;
            ItemID.Sets.GamepadExtraRange[item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 38;
            item.melee = true;
            item.damage = 60;
            item.knockBack = 7.5f;
            item.useTime = 20;
            item.useAnimation = 20;
            item.autoReuse = true;

            item.useStyle = 5;
            item.UseSound = SoundID.Item1;
            item.channel = true;
            item.noUseGraphic = true;
            item.noMelee = true;

            item.shoot = ModContent.ProjectileType<SolarFlareYoyo>();
            item.shootSpeed = 16f;

            item.rare = 10;
            item.Calamity().customRarity = CalamityRarity.Turquoise;
            item.value = Item.buyPrice(platinum: 1, gold: 20);
        }
    }
}
