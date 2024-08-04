using CalamityMod.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Waters
{
    public class CragsLava : ModLavaStyle
    {
        public override string WaterfallTexture => "CalamityMod/Waters/CragsLavaflow";

        public override int GetSplashDust() => 0;

        public override int GetDropletGore() => 0;

        public override bool IsLavaActive() => Main.LocalPlayer.Calamity().ZoneCalamity || Main.LocalPlayer.Calamity().BrimstoneLavaFountainCounter > 0;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 2.48f / 3;
            g = 1.05f / 3;
            b = 0.98f / 3;
        }

        public override void InflictDebuff(Player player, NPC npc, int onfireDuration)
        {
            //Add Searing lava here
            /*int buffID = 0;
            if (player != null)
            {
                player.AddBuff(buffID, onfireDuration / 2);
            }
            if (npc != null)
            {
                if (Main.remixWorld && !npc.friendly)
                {
                    npc.AddBuff(buffID, onfireDuration / 2);
                }
                else
                {
                    npc.AddBuff(buffID, onfireDuration / 2);
                }
            }*/
        }
    }

    /*public class CragsLava2 : ModLavaStyle
    {
        public override string Texture => "CalamityMod/Waters/CragsLava";

        //public override string LavaTexturePath => "CalamityMod/Waters/CragsLava";

        //public override string BlockTexturePath => LavaTexturePath + "_Block";

        //public override string SlopeTexturePath => LavaTexturePath + "_Slope"; //Will deprecate these as they should be automatic 

        //public override bool ChooseLavaStyle() => Main.LocalPlayer.Calamity().ZoneCalamity || Main.LocalPlayer.Calamity().BrimstoneLavaFountainCounter > 0; //unclear whether to move it to a new modbiome or leave it as is

        //public override int ChooseWaterfallStyle() => ModContent.Find<ModWaterfallStyle>("CalamityMod/CragsLavaflow").Slot;

        public override int GetSplashDust() => 0;

        public override int GetDropletGore() => 0;

        /*public override void SelectLightColor(ref Color initialLightColor)
        {
            initialLightColor = Color.Lerp(initialLightColor, Color.White, 0.5f);
            initialLightColor.A = 255;
        }*/
    //}
}
