using CalamityMod.Items.Placeables.FurnitureCosmilite;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ModLoader;
using Terraria.ID;
namespace CalamityMod.Items.Placeables.FurnitureOccult
{
    public class OccultLantern : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            item.SetNameOverride("Otherworldly Lantern");
            item.width = 28;
            item.height = 20;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.createTile = ModContent.TileType<Tiles.FurnitureOccult.OccultLantern>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<OccultStone>(), 18);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBrick>(), 1);
            recipe.SetResult(this, 1);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.AddRecipe();
        }
    }
}
