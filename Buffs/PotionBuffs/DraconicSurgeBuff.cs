﻿using Terraria;
using Terraria.ModLoader;
using CalamityMod.CalPlayer;

namespace CalamityMod.Buffs.PotionBuffs
{
    public class DraconicSurgeBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Draconic Surge");
			Description.SetDefault("The power of a dragon courses through your veins");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = false;
			longerExpertDebuff = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetCalamityPlayer().draconicSurge = true;
		}
	}
}
