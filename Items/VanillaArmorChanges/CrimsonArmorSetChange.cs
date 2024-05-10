﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.VanillaArmorChanges
{
    public class CrimsonArmorSetChange : VanillaArmorChange
    {
        public override int? HeadPieceID => ItemID.CrimsonHelmet;

        public override int? BodyPieceID => ItemID.CrimsonScalemail;

        public override int? LegPieceID => ItemID.CrimsonGreaves;

        public override string ArmorSetName => "Crimson";

        public const float ArmorPieceDamage = 0.05f;
        public const int ArmorPieceLifeRegen = 1;

        // Set bonus clarification
        public override void UpdateSetBonusText(ref string setBonusText)
        {
            Player player = Main.LocalPlayer;
            setBonusText = $"{CalamityUtils.GetTextValue($"Vanilla.Armor.SetBonus.{ArmorSetName}")}";
        }

        private void ApplyAnyPieceEffect(Player player)
        {
            // Remove the vanilla +3% damage and add the new damage value at the same time
            player.GetDamage<GenericDamageClass>() += ArmorPieceDamage - 0.03f;

            // Give life regen
            player.lifeRegen += ArmorPieceLifeRegen;
        }

        public override void ApplyHeadPieceEffect(Player player) => ApplyAnyPieceEffect(player);

        public override void ApplyBodyPieceEffect(Player player) => ApplyAnyPieceEffect(player);

        public override void ApplyLegPieceEffect(Player player) => ApplyAnyPieceEffect(player);
    }
}
