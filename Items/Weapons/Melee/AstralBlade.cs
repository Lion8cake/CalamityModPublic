﻿using System;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityMod.Items.Weapons.Melee
{
    public class AstralBlade : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = 80;
            Item.height = 80;
            Item.damage = 142;
            Item.DamageType = TrueMeleeDamageClass.Instance;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 25;

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300);

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo) => target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300);

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            for (int i = 0; i < 3; i++)
            {
                Dust d = CalamityUtils.MeleeDustHelper(player, Main.rand.NextBool() ? ModContent.DustType<AstralOrange>() : ModContent.DustType<AstralBlue>(), 0.7f, 55, 110, -0.07f, 0.07f);
                if (d != null)
                    d.customData = 0.03f;
            }
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            // Avoid dividing by 0 at all costs.
            if (target.lifeMax <= 0)
                return;

            float lifeRatio = MathHelper.Clamp(target.life / (float)target.lifeMax, 0f, 1f);
            float maxMultiplier = 1.25f;
            float multiplier = MathHelper.Lerp(1f, maxMultiplier, lifeRatio);

            modifiers.SourceDamage *= multiplier;
            modifiers.Knockback *= multiplier;

            if (Main.rand.NextBool((int)MathHelper.Clamp((Item.crit + player.GetCritChance<MeleeDamageClass>()) * multiplier, 0f, 99f), 100))
                modifiers.SetCrit();

            SoundEngine.PlaySound(SoundID.Item105, player.Center);

            bool blue = Main.rand.NextBool();
            float angleStart = Main.rand.NextFloat(0f, MathHelper.TwoPi);
            float var = 0.05f + (maxMultiplier - multiplier);
            for (float angle = 0f; angle < MathHelper.TwoPi; angle += var)
            {
                blue = !blue;
                Vector2 velocity = angle.ToRotationVector2() * (2f + (float)(Math.Sin(angleStart + angle * 3f) + 1) * 2.5f) * Main.rand.NextFloat(0.95f, 1.05f);
                Dust d = Dust.NewDustPerfect(target.Center, blue ? ModContent.DustType<AstralBlue>() : ModContent.DustType<AstralOrange>(), velocity);
                d.customData = 0.025f;
                d.scale = multiplier;
                d.noLight = false;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AstralBar>(8).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
