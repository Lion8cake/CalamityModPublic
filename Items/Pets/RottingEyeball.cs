﻿using CalamityMod.Buffs.Pets;
using CalamityMod.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Pets
{
    public class RottingEyeball : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Pets";
        public override void SetDefaults()
        {
            /*item.DefaultToVanitypet(ModContent.ProjectileType<MiniHiveMind>(), ModContent.BuffType<HiveMindPet>());
            item.rare = -13;*/
            Item.width = 16;
            Item.height = 30;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.damage = 0;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.NPCHit2;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<MiniHiveMind>();
            Item.buffType = ModContent.BuffType<MiniMindBuff>();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
    }
}
