﻿using CalamityMod.Projectiles.Healing;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Melee
{
    public class BurntSienna : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 54;
            Item.damage = 32;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = Item.useTime = 21;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 5f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var source = player.GetSource_ItemUse(Item);
            if (target.life <= 0 && !player.moonLeech)
            {
                float randomSpeedX = Main.rand.Next(3);
                float randomSpeedY = Main.rand.Next(3, 5);
                Projectile.NewProjectile(source, target.Center.X, target.Center.Y, -randomSpeedX, -randomSpeedY, ModContent.ProjectileType<BurntSiennaProj>(), 0, 0f, player.whoAmI, player.whoAmI);
                Projectile.NewProjectile(source, target.Center.X, target.Center.Y, randomSpeedX, -randomSpeedY, ModContent.ProjectileType<BurntSiennaProj>(), 0, 0f, player.whoAmI, player.whoAmI);
                Projectile.NewProjectile(source, target.Center.X, target.Center.Y, 0f, -randomSpeedY, ModContent.ProjectileType<BurntSiennaProj>(), 0, 0f, player.whoAmI, player.whoAmI);
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            var source = player.GetSource_ItemUse(Item);
            if (target.statLife <= 0 && !player.moonLeech)
            {
                float randomSpeedX = Main.rand.Next(3);
                float randomSpeedY = Main.rand.Next(3, 5);
                Projectile.NewProjectile(source, target.Center.X, target.Center.Y, -randomSpeedX, -randomSpeedY, ModContent.ProjectileType<BurntSiennaProj>(), 0, 0f, player.whoAmI, player.whoAmI);
                Projectile.NewProjectile(source, target.Center.X, target.Center.Y, randomSpeedX, -randomSpeedY, ModContent.ProjectileType<BurntSiennaProj>(), 0, 0f, player.whoAmI, player.whoAmI);
                Projectile.NewProjectile(source, target.Center.X, target.Center.Y, 0f, -randomSpeedY, ModContent.ProjectileType<BurntSiennaProj>(), 0, 0f, player.whoAmI, player.whoAmI);
            }
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.GoldCoin);
            }
        }
    }
}
