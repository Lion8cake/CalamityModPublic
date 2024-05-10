﻿using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Dyes
{
    public class SwirlingCosmicFlameDye : BaseDye
    {
        public override ArmorShaderData ShaderDataToBind => new ArmorShaderData(Mod.Assets.Request<Effect>("Effects/Dyes/CosmicFlameShader"), "DyePass").
            UseColor(new Color(52, 212, 229)).UseSecondaryColor(new Color(255, 115, 221)).UseImage("Images/Misc/noise").UseSaturation(1f);
        public override void SafeSetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
        }

        public override void SafeSetDefaults()
        {
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.value = Item.sellPrice(0, 7, 50, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe(2).
                AddIngredient<BlueCosmicFlameDye>().
                AddIngredient<PinkCosmicFlameDye>().
                AddTile(TileID.DyeVat).
                Register();
        }
    }
}
