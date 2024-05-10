﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Tools
{
    public class AstralHamaxe : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Tools";
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 70;
            Item.damage = 80;
            Item.knockBack = 5f;
            Item.useTime = 5;
            Item.useAnimation = 20;
            Item.hammer = 100;
            Item.axe = 165 / 5;
            Item.tileBoost += 3;

            Item.DamageType = DamageClass.Melee;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 25;

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust d = CalamityUtils.MeleeDustHelper(player, Main.rand.NextBool() ? ModContent.DustType<AstralOrange>() : ModContent.DustType<AstralBlue>(), 0.48f, 50, 78, -0.1f, 0.1f);
            if (d != null)
            {
                d.customData = 0.02f;
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300);
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
