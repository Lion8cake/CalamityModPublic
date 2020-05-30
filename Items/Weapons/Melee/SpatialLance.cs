using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee.Spears;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class SpatialLance : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Elemental Lance");
            Tooltip.SetDefault("Rend the cosmos asunder!\n" +
                "Fires a lance beam that splits multiple times as it travels");
        }

        public override void SetDefaults()
        {
            item.width = 88;
            item.damage = 210;
            item.melee = true;
            item.noMelee = true;
            item.useTurn = true;
            item.noUseGraphic = true;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useTime = 30;
            item.knockBack = 9.5f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.height = 88;
            item.value = Item.buyPrice(1, 20, 0, 0);
            item.rare = 10;
            item.shoot = ModContent.ProjectileType<SpatialLanceProjectile>();
            item.shootSpeed = 12f;
            item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot)
                {
                    return false;
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<TerraLance>());
            recipe.AddIngredient(ItemID.NorthPole);
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
