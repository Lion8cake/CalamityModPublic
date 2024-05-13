﻿using CalamityMod.Dusts;
using CalamityMod.Events;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.Items.SummonItems;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.Projectiles.Boss;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalamityMod.Tiles.Astral
{
    public class AstralBeacon : ModTile
    {
        public const int Width = 5;
        public const int Height = 4;
        public static readonly Color FailColor = new Color(237, 93, 83);

        public static readonly SoundStyle UseSound = new("CalamityMod/Sounds/Custom/AstralBeaconUse");

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileSpelunker[Type] = true;

            // Various data sets to protect this tile from unintentional death
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(128, 128, 158), CalamityUtils.GetItemName<AstralBeaconItem>());
            TileID.Sets.DisableSmartCursor[Type] = true;
            MinPick = 200;
        }

        public override bool CanExplode(int i, int j) => false;

        public override bool CreateDust(int i, int j, ref int type)
        {
            type = Utils.SelectRandom(Main.rand, ModContent.DustType<AstralBlue>(), ModContent.DustType<AstralOrange>());
            return true;
        }

        public override bool RightClick(int i, int j)
        {
            Tile tile = Main.tile[i, j];

            int left = i - tile.TileFrameX / 18;
            int top = j - tile.TileFrameY / 18;

            if (!Main.LocalPlayer.HasItem(ModContent.ItemType<TitanHeart>()) &&
                !Main.LocalPlayer.HasItem(ModContent.ItemType<Starcore>()))
                return true;

            if (NPC.AnyNPCs(ModContent.NPCType<AstrumDeusHead>()) || BossRushEvent.BossRushActive)
                return true;

            if (CalamityUtils.CountProjectiles(ModContent.ProjectileType<DeusRitualDrama>()) > 0)
                return true;

            bool usingStarcore = Main.LocalPlayer.HasItem(ModContent.ItemType<Starcore>());

            if (Main.IsItDay())
            {
                CalamityUtils.DisplayLocalizedText("Mods.CalamityMod.Status.Boss.DeusAltarRejectNightText", FailColor);
                return false;
            }

            Vector2 ritualSpawnPosition = new Vector2(left + Width / 2, top).ToWorldCoordinates();
            ritualSpawnPosition += new Vector2(0f, -24f);

            SoundEngine.PlaySound(UseSound, ritualSpawnPosition);
            Projectile.NewProjectile(new EntitySource_WorldEvent(), ritualSpawnPosition, Vector2.Zero, ModContent.ProjectileType<DeusRitualDrama>(), 0, 0f, Main.myPlayer, 0f, usingStarcore.ToInt());

            if (!usingStarcore)
                Main.LocalPlayer.ConsumeItem(ModContent.ItemType<TitanHeart>(), true);

            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<TitanHeart>();
            if (Main.LocalPlayer.HasItem(ModContent.ItemType<Starcore>()))
                Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<Starcore>();
            Main.LocalPlayer.noThrow = 2;
            Main.LocalPlayer.cursorItemIconEnabled = true;
        }

        public override void MouseOverFar(int i, int j)
        {
            Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<TitanHeart>();
            if (Main.LocalPlayer.HasItem(ModContent.ItemType<Starcore>()))
                Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<Starcore>();
            Main.LocalPlayer.noThrow = 2;
            Main.LocalPlayer.cursorItemIconEnabled = true;
        }
    }
}
