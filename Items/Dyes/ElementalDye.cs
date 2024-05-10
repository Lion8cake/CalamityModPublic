﻿using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Dyes
{
    public class ElementalDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(Mod.Assets.Request<Effect>("Effects/Dyes/ElementalDyeShader"), "DyePass").UseImage("Images/Misc/Perlin");
        public override void SafeSetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
        }

        public override void SafeSetDefaults()
        {
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 2, 50, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe(5).
                AddIngredient(ItemID.SolarDye).
                AddIngredient(ItemID.VortexDye).
                AddIngredient(ItemID.StardustDye).
                AddIngredient(ItemID.NebulaDye).
                AddIngredient<GalacticaSingularity>().
                AddTile(TileID.DyeVat).
                Register();
        }
    }
}
