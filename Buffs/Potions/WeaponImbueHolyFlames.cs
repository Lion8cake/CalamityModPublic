﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Buffs.Potions
{
    public class WeaponImbueHolyFlames : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.meleeBuff[Type] = true;
            Main.persistentBuff[Type] = true;
            BuffID.Sets.IsAFlaskBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Calamity().flaskHoly = true;
            // A very large number to indicate it's a modded Flask
            player.meleeEnchant = CalamityGlobalBuff.ModdedFlaskEnchant;
        }
    }
}
