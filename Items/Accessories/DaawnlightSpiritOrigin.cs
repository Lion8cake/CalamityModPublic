﻿using CalamityMod.Buffs.Pets;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    public class DaawnlightSpiritOrigin : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        // "Despite the seemingly insane numbers here, I think this item might actually be underpowered"
        // hindsight: the item was not underpowered. Ozzatron 05NOV2021
        //
        // Regular crits are intentionally weak, especially because they rarely happen (your crit chance gets murdered).
        // Bullseyes should be doing all the work.
        private const float OriginBullseyeCritRatio = 3.5f; // Bullseye crits deal x3.5 damage instead of x2.
        private const float ForcedCritBullseyeCritRatio = 2.4f; // If your projectile is forced to crit, you get less of a reward.

        private const float StoredCritConversionRatio = 0.01f; // Add +1% more damage to crits for every 1% critical chance the player would have had.
        private const float MinUseTimeForSlowBonus = 11f;
        private const float MaxSlowBonusUseTime = 72f;
        private const float MaxSlowWeaponBonus = 0.33f; // Up to +33% more damage to crits for slower weapons.

        // These were very carefully calculated, please don't change them.
        internal const float RegularEnemyBullseyeRadius = 8f;
        internal const float BossBullseyeRadius = 18f;

        // Special search radius for coin ricoshots that only applies to DSO targets.
        public static readonly float RicoshotSearchDistance = 2800f;

        internal static float GetDamageMultiplier(Player p, CalamityPlayer mp, bool hitBullseye, bool wasForcedCrit)
        {
            float baseCritMult = 2f; // In vanilla Terraria, crits do +100% damage.

            // If a bullseye was struck, replace a "regular crit" with a "bullseye crit".
            if (hitBullseye)
            {
                // Bullseye crits are weaker if the projectile was already a forced crit.
                // This is currently implemented for ULTRAKILL-style ricoshots and for Heavenly Gale's lightning.
                baseCritMult = wasForcedCrit ? ForcedCritBullseyeCritRatio : OriginBullseyeCritRatio;
            }

            // Factor in the critical strike chance the player isn't getting to use.
            float convertedCritBonus = StoredCritConversionRatio * mp.spiritOriginConvertedCrit;

            float useTimeInterpolant = Utils.GetLerpValue(MinUseTimeForSlowBonus, MaxSlowBonusUseTime, p.ActiveItem().useTime, true);
            float slowWeaponBonus = MathHelper.Lerp(0f, MaxSlowWeaponBonus, useTimeInterpolant);
            return baseCritMult * (1f + convertedCritBonus + slowWeaponBonus);
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = CalamityGlobalItem.RarityPurpleBuyPrice;
            Item.Calamity().donorItem = true;
        }

        // The pet is purely visual and does not affect the functionality of the item.
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Calamity().spiritOrigin = true;

            // If visibility is disabled, despawn the pet.
            if (hideVisual)
            {
                if (player.FindBuffIndex(ModContent.BuffType<ArcherofLunamoon>()) != -1)
                    player.ClearBuff(ModContent.BuffType<ArcherofLunamoon>());
            }
            // If visibility is enabled, spawn the pet.
            else if (player.whoAmI == Main.myPlayer)
            {
                if (player.FindBuffIndex(ModContent.BuffType<ArcherofLunamoon>()) == -1)
                    player.AddBuff(ModContent.BuffType<ArcherofLunamoon>(), 18000, true);
            }
        }

        public override void UpdateVanity(Player player)
        {
            // Summon anime girl if it's in vanity slot as the pet is purely vanity
            // It's possible for other "pet" items like Fungal Clump or HotE to summon a passive version of their "pets" with some tweaks though
            player.Calamity().spiritOriginVanity = true;
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.FindBuffIndex(ModContent.BuffType<ArcherofLunamoon>()) == -1)
                    player.AddBuff(ModContent.BuffType<ArcherofLunamoon>(), 18000, true);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DeadshotBrooch>().
                AddIngredient<MysteriousCircuitry>(15).
                AddIngredient<DubiousPlating>(15).
                AddIngredient(ItemID.LunarBar, 10).
                AddIngredient<GalacticaSingularity>(4).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
