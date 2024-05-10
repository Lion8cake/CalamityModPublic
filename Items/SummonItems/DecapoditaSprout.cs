﻿using CalamityMod.Events;
using CalamityMod.NPCs.Crabulon;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.SummonItems
{
    public class DecapoditaSprout : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.SummonItems";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 3; // Worm Food / Bloody Spine
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.rare = ItemRarityID.Green;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossItem;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ZoneGlowshroom && !NPC.AnyNPCs(ModContent.NPCType<Crabulon>()) && !BossRushEvent.BossRushActive;
        }

        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(SoundID.Roar, player.Center);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int npc = NPC.NewNPC(new EntitySource_BossSpawn(player), (int)(player.position.X + Main.rand.Next(-160, 161)), (int)(player.position.Y - 320f), ModContent.NPCType<Crabulon>(), 1);
                Main.npc[npc].timeLeft *= 20;
                CalamityUtils.BossAwakenMessage(npc);
            }
            else
                NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<Crabulon>());

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.GlowingMushroom, 50).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
