﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.NPCs;

namespace CalamityMod.Buffs
{
	public class AbyssalDivingSuitPlatesBroken : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Abyssal Diving Suit Plates Broken");
			Description.SetDefault("The plates are regenerating");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
			canBeCleared = false;
		}
		
		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<CalamityPlayer>(mod).abyssalDivingSuitCooldown = true;
		}
	}
}
