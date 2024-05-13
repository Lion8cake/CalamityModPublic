﻿using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Buffs.Potions
{
    public class BaguetteBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().baguette = true;
        }
    }
}
