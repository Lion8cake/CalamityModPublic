﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.NPCs;

namespace CalamityMod.Buffs
{
	public class Levi : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Levi");
			Description.SetDefault("Small and cute");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }
		
		public override void Update(Player player, ref int buffIndex)
		{
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<CalamityPlayer>(mod).leviPet = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("Levi")] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("Levi"), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
	}
}
