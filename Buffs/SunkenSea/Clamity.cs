﻿using Terraria;
using Terraria.ModLoader;
using CalamityMod.CalPlayer;

namespace CalamityMod.Buffs.SunkenSea
{
    public class Clamity : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Clamity");
			Description.SetDefault("The clams have been angered!");
			Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
            canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetCalamityPlayer().clamity = true;
		}
	}
}
