﻿using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityMod.Prefixes
{
    [LegacyName("CloakedPrefix", "QuietPrefix")]
    public class Cloaked : RogueAccessoryPrefix
    {
        public override float stealthGenBonus => 0.04f;
    }

    [LegacyName("SilentPrefix", "CamouflagedPrefix")]
    public class Silent : RogueAccessoryPrefix
    {
        public override float stealthGenBonus => 0.08f;
    }

    public abstract class RogueAccessoryPrefix : ModPrefix, ILocalizedModType
    {
        public new string LocalizationCategory => "Prefixes.Accessory";

        // Stats
        public virtual float stealthGenBonus => 0f;

        // Prefix roll logic
        public override PrefixCategory Category => PrefixCategory.Accessory;
        public override bool CanRoll(Item item) => GetType() != typeof(RogueAccessoryPrefix);

        // Applying stealth generation
        public override void ApplyAccessoryEffects(Player player)
        {
            player.Calamity().accStealthGenBoost += stealthGenBonus;
        }

        // Changing value based on prefix tier (rarity is set automatically around value multiplier)
        public override void ModifyValue(ref float valueMult)
        {
            float extraValue = 1f + (2.5f * stealthGenBonus);
            valueMult *= extraValue;
        }

        // Extra tooltip for new modifier stats
        public LocalizedText StealthGenTooltip => CalamityUtils.GetText($"{LocalizationCategory}.StealthGenTooltip");
        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            yield return new TooltipLine(Mod, "PrefixStealthGenBoost", StealthGenTooltip.Format((stealthGenBonus * 100).ToString("N0")))
            {
                IsModifier = true
            };
        }
    }
}
