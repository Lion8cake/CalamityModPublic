using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class AlulaAustralis : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Alula Australis");
            Tooltip.SetDefault("Fires a beautiful aurora trailed by a star shower");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 90;
            item.magic = true;
            item.mana = 8;
            item.width = 52;
            item.height = 52;
            item.useTime = 17;
            item.useAnimation = 17;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 3f;
            item.value = Item.buyPrice(0, 60, 0, 0);
            item.rare = 7;
            item.UseSound = SoundID.Item9;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<AuroraAustralis>();
            item.shootSpeed = 13f;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(15, 15);
        }
    }
}
