﻿using Terraria;
using Terraria.ModLoader;
using CalamityMod.NPCs;
using CalamityMod.CalPlayer;

namespace CalamityMod.Buffs.DoTDebuffs
{
    public class HolyLight : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Holy Flames");
			Description.SetDefault("Dissolving from holy light");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetCalamityPlayer().hFlames = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<CalamityGlobalNPC>(mod).hFlames = true;
		}
	}
}
