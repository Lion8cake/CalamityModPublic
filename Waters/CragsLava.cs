using CalamityMod.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Waters
{
    public class CragsLavaflow : ModWaterfallStyle { } //Make a CalamityModWaterfallStyle for non-opaque waterfalls for lava.

    public class CragsLava : ModLavaStyle
    {
        //public override string LavaTexturePath => "CalamityMod/Waters/CragsLava";

        //public override string BlockTexturePath => LavaTexturePath + "_Block";

        //public override string SlopeTexturePath => LavaTexturePath + "_Slope"; //Will deprecate these as they should be automatic 

        //public override bool ChooseLavaStyle() => Main.LocalPlayer.Calamity().ZoneCalamity || Main.LocalPlayer.Calamity().BrimstoneLavaFountainCounter > 0; //unclear whether to move it to a new modbiome or leave it as is

        public override int ChooseWaterfallStyle() => ModContent.Find<ModWaterfallStyle>("CalamityMod/CragsLavaflow").Slot;

        public override int GetSplashDust() => 0;

        public override int GetDropletGore() => 0;

        /*public override void SelectLightColor(ref Color initialLightColor)
        {
            initialLightColor = Color.Lerp(initialLightColor, Color.White, 0.5f);
            initialLightColor.A = 255;
        }*/
    }
}
