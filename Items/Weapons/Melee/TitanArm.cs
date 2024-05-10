﻿using CalamityMod.Buffs.DamageOverTime;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class TitanArm : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 58;
            Item.damage = 69;
            Item.knockBack = 80f; //This number doesn't mean anything, but it's not 9001f because that caused bugs.
            Item.useAnimation = Item.useTime = 12;
            Item.DamageType = DamageClass.Melee;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Lime;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
        }

        // Boosting crit in SetDefaults along with knockback seemed to severely inflate the reforging price. Guaranteed crits for more knockback.
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit = 100;

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300);
            YeetEnemies(player, target, hit.Crit);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300);
        }

        private void YeetEnemies(Player player, NPC target, bool crit)
        {
            // Manually doing knockback because it is capped in vanilla. This lets Titan Arm reach its full potential. =D
            // This is modified vanilla code from StrikeNPC method in NPC.cs
            // Extra Note: Will cause an out of bounds error on enemies that don't despawn and are affected. See Blue Cultist Archer.
            // Extra (extra) Note: Fails in ModifyHitNPC if the enemy has too much health due to velocity clamping if you don't do enough damage.
            float kbAmt = player.GetWeaponKnockback(Item, Item.knockBack) * target.knockBackResist; //That obligatory over 9000 reference
            if (crit)
                kbAmt *= 1.4f;
            float kbAmtY = target.noGravity ? kbAmt * -0.5f : kbAmt * -0.75f;
            target.velocity.Y += kbAmtY;
            target.velocity.X += kbAmt * player.direction;
        }
    }
}
