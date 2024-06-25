using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.Liquid;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ModLoader;

namespace CalamityMod.Systems
{
    public abstract class ModLavaStyle : ModTexturedType
    {
        /// <summary>
        /// The ID of the lava style.
        /// </summary>
        public int Slot { get; internal set; }

        protected sealed override void Register()
        {
            Slot = LoaderManager.Get<LavaStylesLoader>().Register(this);
        }

        public sealed override void SetupContent()
        {
            SetStaticDefaults();
            CalamityMod.LavaTextures.liquid[Slot] = ModContent.Request<Texture2D>(Texture, (AssetRequestMode)2);
            CalamityMod.LavaTextures.slope[Slot] = ModContent.Request<Texture2D>(SlopeTexture, (AssetRequestMode)2);
            CalamityMod.LavaTextures.block[Slot] = ModContent.Request<Texture2D>(BlockTexture, (AssetRequestMode)2);
        }

        public virtual string BlockTexture => Texture + "_Block";

        public virtual string SlopeTexture => Texture + "_Slope";

        /// <summary>
        /// The ID of the waterfall style the game should use when this lava style is in use.
        /// </summary>
        public abstract int ChooseWaterfallStyle();

        /// <summary>
        /// The ID of the dust that is created when anything splashes in lava.
        /// </summary>
        public abstract int GetSplashDust();

        /// <summary>
        /// The ID of the gore that represents droplets of water falling down from a block. Return <see cref="F:Terraria.ID.GoreID.LavaDrip" /> (or another existing droplet gore) or make a custom ModGore that uses <see cref="F:Terraria.ID.GoreID.Sets.LiquidDroplet" />.
        /// </summary>
        public abstract int GetDropletGore();

        public virtual void DrawColor(int x, int y, ref VertexColors liquidColor)
        {
        }

        public virtual void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
        }
    }

    public class LavaStylesLoader : SceneEffectLoader<ModLavaStyle> //not fully sketched out, here as a sub for implementing ModWaterStyle.DrawColor
    {
        public int TotalCount { 
            get => (int)typeof(LavaStylesLoader).GetRuntimeField("TotalCount").GetValue(this); 
            set => typeof(LavaStylesLoader).GetRuntimeField("TotalCount").SetValue(this, TotalCount); }

        public LavaStylesLoader()
        {
            Initialize(0);
        }

        internal void ResizeArray() //doesnt actually connect to anything yet
        {
            Array.Resize(ref CalamityMod.LavaTextures.liquid, TotalCount);
            Array.Resize(ref CalamityMod.LavaTextures.slope, TotalCount);
            Array.Resize(ref CalamityMod.LavaTextures.block, TotalCount);
        }

        public void DrawWaterfall(WaterfallManager waterfallManager)
        {
            foreach (ModWaterStyle waterStyle in list)
            {
                if (Main.liquidAlpha[waterStyle.Slot] > 0f)
                {
                    waterfallManager.DrawWaterfall(waterStyle.ChooseWaterfallStyle(), Main.liquidAlpha[waterStyle.Slot]);
                }
            }
        }

        internal static void ModifyLightSetup(int i, int j, int type, ref float r, ref float g, ref float b)
        {
        }

        internal static void DrawColorSetup(int x, int y, int type, ref VertexColors liquidColor)
        {
        }
    }
}
