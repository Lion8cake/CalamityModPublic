﻿using CalamityMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Buffs.StatDebuffs
{
    public class KamiFlu : ModBuff
    {
        public const float MultiplicativeDamageReduction = 0.8f;
        // Hard-cap for npc speed when afflicted with this debuff. Does not affect certain NPCs and does not affect any bosses (Basically only works on boss minions).
        public const float MaxNPCSpeed = 16f;

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.Calamity().kamiFlu < npc.buffTime[buffIndex])
                npc.Calamity().kamiFlu = npc.buffTime[buffIndex];
            if ((CalamityLists.enemyImmunityList.Contains(npc.type) || npc.boss) && npc.Calamity().debuffResistanceTimer <= 0)
                npc.Calamity().debuffResistanceTimer = CalamityGlobalNPC.slowingDebuffResistanceMin + npc.Calamity().kamiFlu;
            npc.DelBuff(buffIndex);
            buffIndex--;
        }
    }
}
