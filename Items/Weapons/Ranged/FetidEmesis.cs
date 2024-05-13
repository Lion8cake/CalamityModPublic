﻿using CalamityMod.Projectiles.Ranged;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class FetidEmesis : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.width = 76;
            Item.height = 46;
            Item.damage = 129;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;

            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.Next(100) > 40;

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.NextBool(8))
            {
                Projectile.NewProjectile(source, position, velocity * 0.8f,
                    ModContent.ProjectileType<EmesisGore>(), damage, knockback, player.whoAmI);
                for (int i = 0; i < 5; i++)
                {
                    Dust dust = Dust.NewDustDirect(position, 10, 10, DustID.Shadowflame);
                    dust.velocity = Vector2.Normalize(velocity).RotatedByRandom(MathHelper.ToRadians(15f));
                    dust.noGravity = true;
                }
                if (player.Calamity().soundCooldown <= 0)
                {
                    // WoF vomit sound.
                    SoundEngine.PlaySound(SoundID.NPCDeath13 with { Volume = SoundID.NPCDeath13.Volume * 0.5f }, position);
                    player.Calamity().soundCooldown = 120;
                }
                return false;
            }
            return true;
        }
    }
}
