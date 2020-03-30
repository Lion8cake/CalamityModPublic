using CalamityMod.Projectiles.Hybrid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class CorpusAvertorMelee : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corpus Avertor");
            Tooltip.SetDefault("Seems like it has worn down over time");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 32;
            item.damage = 140;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.useAnimation = 15;
            item.useStyle = 1;
            item.useTime = 15;
            item.knockBack = 3f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.value = Item.buyPrice(0, 80, 0, 0);
            item.rare = 8;
            item.Calamity().customRarity = CalamityRarity.Dedicated;
            item.shoot = ModContent.ProjectileType<CorpusAvertorProj>();
            item.shootSpeed = 5f;
            item.melee = true;
        }

        // Gains 20% of missing health as base damage.
        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            int lifeAmount = player.statLifeMax2 - player.statLife;
            flat += lifeAmount * 0.2f * player.meleeDamage;
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
