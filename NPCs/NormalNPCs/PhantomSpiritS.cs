﻿using CalamityMod.Dusts;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Banners;
using CalamityMod.NPCs.CalamityAIs.CalamityRegularEnemyAIs;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace CalamityMod.NPCs.NormalNPCs
{
    public class PhantomSpiritS : ModNPC
    {
        public override LocalizedText DisplayName => CalamityUtils.GetText("NPCs.PhantomSpirit.DisplayName");
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 80;
            NPC.width = 32;
            NPC.height = 80;
            NPC.defense = 20;
            NPC.lifeMax = 1500;
            NPC.knockBackResist = 0.2f;
            AIType = -1;
            NPC.value = Item.buyPrice(0, 0, 30, 0);
            NPC.HitSound = SoundID.NPCHit36;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            Banner = ModContent.NPCType<PhantomSpirit>();
            BannerItem = ModContent.ItemType<PhantomSpiritBanner>();
            NPC.Calamity().VulnerableToSickness = false;

            // Scale stats in Expert and Master
            CalamityGlobalNPC.AdjustExpertModeStatScaling(NPC);
            CalamityGlobalNPC.AdjustMasterModeStatScaling(NPC);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
                new FlavorTextBestiaryInfoElement("Mods.CalamityMod.Bestiary.PhantomSpirit")
            });
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            float speed = CalamityWorld.death ? 20f : CalamityWorld.revenge ? 17.5f : 15f;
            CalamityRegularEnemyAI.DungeonSpiritAI(NPC, Mod, speed, -MathHelper.PiOver2);
            int polterDust = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Necroplasm, 0f, 0f, 0, default, 1f);
            Dust dust = Main.dust[polterDust];
            dust.velocity *= 0.1f;
            dust.scale = 1.3f;
            dust.noGravity = true;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Necroplasm, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 50; i++)
                {
                    int hitPolterDust = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Necroplasm, NPC.velocity.X, NPC.velocity.Y, 0, default, 1f);
                    Dust dust = Main.dust[hitPolterDust];
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    dust.scale = 1.4f;
                }
            }
        }

        public override Color? GetAlpha(Color drawColor) => new Color(200, 200, 200, 0);

        public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.Add(ModContent.ItemType<Necroplasm>(), 1, 1, 2);
    }
}
