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
            r = 2.48f / 4;
            g = 1.05f / 4;
            b = 0.98f / 4;
        }

        public override void InflictDebuff(Player player, int onfireDuration)
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
}
