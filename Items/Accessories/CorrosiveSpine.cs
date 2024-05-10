﻿using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories
{
    public class CorrosiveSpine : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 46;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.defense = 4;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed += 0.05f;
            player.Calamity().corrosiveSpine = true;
            if (player.immune)
            {
                if (Main.rand.NextBool(15))
                {
                    var source = player.GetSource_Accessory(Item);
                    int cloudCount = Main.rand.Next(2, 5);
                    for (int i = 0; i < cloudCount; i++)
                    {
                        int type = Utils.SelectRandom(Main.rand, new int[]
                        {
                            ModContent.ProjectileType<Corrocloud1>(),
                            ModContent.ProjectileType<Corrocloud2>(),
                            ModContent.ProjectileType<Corrocloud3>()
                        });
                        float speed = Main.rand.NextFloat(3f, 11f);
                        int damage = (int)player.GetTotalDamage<RogueDamageClass>().ApplyTo(80);
                        Projectile.NewProjectile(source, player.Center, Vector2.One.RotatedByRandom(MathHelper.TwoPi) * speed,
                            type, damage, 0f, player.whoAmI);
                    }
                }
            }
        }
    }
}
