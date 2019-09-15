﻿using Terraria;
using Terraria.ModLoader;
using CalamityMod.CalPlayer;

namespace CalamityMod.Buffs.Fabsol
{
    public class Everclear : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Everclear");
			Description.SetDefault("25% increased damage, -10 life regen and -40 defense");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = false;
			longerExpertDebuff = false;
            canBeCleared = false;
        }

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetCalamityPlayer().everclear = true;
		}
	}
}
