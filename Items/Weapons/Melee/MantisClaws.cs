﻿using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class MantisClaws : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 20;
            Item.damage = 144;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 8;
            Item.useTurn = true;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Water);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var source = player.GetSource_ItemUse(Item);

            //does no damage. Explosion is visual
            Projectile.NewProjectile(source, target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<FuckYou>(), 0, 0f, player.whoAmI, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
            target.AddBuff(BuffID.OnFire3, 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            var source = player.GetSource_ItemUse(Item);

            //does no damage. Explosion is visual
            Projectile.NewProjectile(source, target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<FuckYou>(), 0, 0f, player.whoAmI, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
            target.AddBuff(BuffID.OnFire3, 300);
        }
    }
}
