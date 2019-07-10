﻿using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Buffs
{
	public class SolarSpirit : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Solar Spirit");
			Description.SetDefault("The solar spirit will protect you");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}
		
		public override void Update(Player player, ref int buffIndex)
		{
			CalamityPlayer modPlayer = player.GetModPlayer<CalamityPlayer>(mod);
			if (player.ownedProjectileCounts[mod.ProjectileType("SolarPixie")] > 0)
			{
				modPlayer.SP = true;
			}
			if (!modPlayer.SP)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else
			{
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}
