﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class BrinyBaron : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 100;
            Item.height = 102;
            Item.damage = 182;
            Item.knockBack = 4f;
            Item.useAnimation = Item.useTime = 20;
            Item.DamageType = DamageClass.Melee;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.shootSpeed = 4f;

            Item.shoot = ModContent.ProjectileType<Razorwind>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noMelee = true;
            }
            else
            {
                Item.noMelee = false;
            }

            return base.UseItem(player);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                damage = (int)(damage * 0.3);
                type = ModContent.ProjectileType<Razorwind>();
            }

            else
                type = ProjectileID.None;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Flare_Blue, 0f, 0f, 100, new Color(53, Main.DiscoG, 255));
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 180);
            var source = player.GetSource_ItemUse(Item);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<BrinySpout>()] == 0)
                Projectile.NewProjectile(source, target.Center, Vector2.Zero, ModContent.ProjectileType<BrinyTyphoonBubble>(), Item.damage, Item.knockBack, player.whoAmI);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 180);
            var source = player.GetSource_ItemUse(Item);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<BrinySpout>()] == 0)
                Projectile.NewProjectile(source, target.Center, Vector2.Zero, ModContent.ProjectileType<BrinyTyphoonBubble>(), Item.damage, Item.knockBack, player.whoAmI);
        }

        public override void UseAnimation(Player player)
        {
            Item.noUseGraphic = false;
            Item.UseSound = SoundID.Item1;

            if (player.altFunctionUse == 2)
            {
                Item.noUseGraphic = true;
                Item.UseSound = SoundID.Item84;
            }
        }
    }
}
