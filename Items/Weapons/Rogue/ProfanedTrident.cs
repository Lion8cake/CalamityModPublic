using CalamityMod.Projectiles.Rogue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class ProfanedTrident : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infernal Spear");
            Tooltip.SetDefault("Throws a homing spear that explodes on enemy hits");
        }

        public override void SafeSetDefaults()
        {
            item.width = 72;
            item.damage = 1000;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.useAnimation = 13;
            item.useStyle = 1;
            item.useTime = 13;
            item.knockBack = 8f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.height = 72;
            item.value = Item.buyPrice(1, 80, 0, 0);
            item.rare = 10;
            item.shoot = ModContent.ProjectileType<InfernalSpearProjectile>();
            item.shootSpeed = 28f;
            item.Calamity().rogue = true;
            item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }
    }
}
