﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.NPCs;

namespace CalamityMod.Buffs
{
	public class AstralInjectionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Astral Injection");
			Description.SetDefault("Extreme mana recovery");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = false;
			longerExpertDebuff = false;
		}
		
		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<CalamityPlayer>(mod).astralInjection = true;
		}
	}
}
