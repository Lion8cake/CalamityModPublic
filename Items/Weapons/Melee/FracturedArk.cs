﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityMod.Items.Weapons.Melee
{
    [LegacyName("ArkoftheAncients")]
    public class FracturedArk : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public float Combo = 1f;
        public float Charge = 0f;
        public static float chargeDamageMultiplier = 1.5f; //Extra damage from charge
        public static float beamDamageMultiplier = 0.8f; //Damage multiplier for the charged shots (remember it applies ontop of the charge damage multiplied

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (tooltips == null)
                return;

            Player player = Main.player[Main.myPlayer];
            if (player is null)
                return;

            var tooltip = tooltips.FirstOrDefault(x => x.Text.Contains("[PARRY]") && x.Mod == "Terraria");
            if (tooltip != null)
            {
                tooltip.Text = Lang.SupportGlyphs(this.GetLocalizedValue("ParryInfo"));
                tooltip.OverrideColor = Color.CornflowerBlue;
            }
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 60;
            Item.damage = 35;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6.25f;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 15f;
        }
        public override void HoldItem(Player player)
        {
            player.Calamity().mouseWorldListener = true;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            return !Main.projectile.Any(n => n.active && n.owner == player.whoAmI && n.type == ProjectileType<ArkoftheAncientsSwungBlade>());
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                if (!Main.projectile.Any(n => n.active && n.owner == player.whoAmI && (n.type == ProjectileType<ArkoftheAncientsParryHoldout>() || n.type == ProjectileType<TrueArkoftheAncientsParryHoldout>() || n.type == ProjectileType<ArkoftheElementsParryHoldout>() || n.type == ProjectileType<ArkoftheCosmosParryHoldout>())))
                    Projectile.NewProjectile(source, player.Center, velocity, ProjectileType<ArkoftheAncientsParryHoldout>(), damage, 0, player.whoAmI, 0, 0);
                return false;
            }

            //Failsafe
            if (Combo != -1 && Combo != 1)
                Combo = 1;

            if (Charge > 0)
                damage = (int)(chargeDamageMultiplier * damage);
            Projectile.NewProjectile(source, player.Center, velocity, ProjectileType<ArkoftheAncientsSwungBlade>(), damage, knockback, player.whoAmI, Combo, Charge);

            Combo *= -1f;
            Charge--;
            if (Charge < 0)
                Charge = 0;

            return false;
        }

        public override ModItem Clone(Item item)
        {
            var clone = base.Clone(item);

            if (clone is FracturedArk a && item.ModItem is FracturedArk a2)
                a.Charge = a2.Charge;

            return clone;
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(Charge);
        }

        public override void NetReceive(BinaryReader reader)
        {
            Charge = reader.ReadSingle();
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (Charge <= 0)
                return;

            float barScale = 1.34f;

            var barBG = Request<Texture2D>("CalamityMod/UI/MiscTextures/GenericBarBack").Value;
            var barFG = Request<Texture2D>("CalamityMod/UI/MiscTextures/GenericBarFront").Value;

            Vector2 barOrigin = barBG.Size() * 0.5f;
            float yOffset = 23f;
            Vector2 drawPos = position + Vector2.UnitY * scale * (frame.Height - yOffset);
            Rectangle frameCrop = new Rectangle(0, 0, (int)(Charge / 10f * barFG.Width), barFG.Height);
            Color color = Main.hslToRgb((Main.GlobalTimeWrappedHourly * 0.6f) % 1, 1, 0.85f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f) * 0.1f);

            spriteBatch.Draw(barBG, drawPos, null, color, 0f, barOrigin, scale * barScale, 0f, 0f);
            spriteBatch.Draw(barFG, drawPos, frameCrop, color * 0.8f, 0f, barOrigin, scale * barScale, 0f, 0f);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.EnchantedSword).
                AddIngredient<PurifiedGel>(5).
                AddRecipeGroup("AnyCopperBar", 10).
                AddTile(TileID.Anvils).
                Register();
            CreateRecipe().
                AddIngredient(ItemID.Terragrim).
                AddIngredient<PurifiedGel>(5).
                AddRecipeGroup("AnyCopperBar", 10).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
