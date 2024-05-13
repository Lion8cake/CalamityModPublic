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
    public class CeaselessDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(Mod.Assets.Request<Effect>("Effects/Dyes/CeaselessDyeShader"), "DyePass");
        public override void SafeSetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
        }

        public override void SafeSetDefaults()
        {
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.value = Item.sellPrice(0, 4, 0, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe(2).
                AddIngredient(ItemID.VoidDye).
                AddIngredient(ItemID.ShadowDye).
                AddIngredient<DarkPlasma>().
                AddTile(TileID.DyeVat).
                Register();
        }
    }
}
