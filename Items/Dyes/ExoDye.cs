﻿using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Dyes
{
    public class ExoDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind
        {
            get => new ArmorShaderData(Mod.Assets.Request<Effect>("Effects/Dyes/ExoDyeShader"), "DyePass").UseImage("Images/Misc/Perlin");
        }

        public override void SafeSetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
        }

        public override void SafeSetDefaults()
        {
            Item.rare = ModContent.RarityType<Violet>();
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe(3).
                AddIngredient(ItemID.BottledWater, 3).
                AddIngredient<ExoPrism>().
                AddTile(TileID.DyeVat).
                Register();
        }
    }
}
