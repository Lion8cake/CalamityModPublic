﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class ParasiticSceptor : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 52;
            Item.damage = 12;
            Item.knockBack = 3f;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 35;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Magic;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<WaterLeechProj>();

            Item.UseSound = SoundID.Item46;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Green;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float speed = Item.shootSpeed;
            Vector2 playerPos = player.RotatedRelativePoint(player.MountedCenter, true);
            float xDist = Main.mouseX + Main.screenPosition.X - playerPos.X;
            float yDist = Main.mouseY + Main.screenPosition.Y - playerPos.Y;
            if (player.gravDir == -1f)
            {
                yDist = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - playerPos.Y;
            }
            Vector2 vector = new Vector2(xDist, yDist);
            float speedMult = vector.Length();
            if ((float.IsNaN(xDist) && float.IsNaN(yDist)) || (xDist == 0f && yDist == 0f))
            {
                xDist = player.direction;
                yDist = 0f;
                speedMult = speed;
            }
            else
            {
                speedMult = speed / speedMult;
            }
            xDist *= speedMult;
            yDist *= speedMult;
            int leechAmt = 2;
            if (Main.rand.NextBool(3))
            {
                leechAmt++;
            }
            if (Main.rand.NextBool(4))
            {
                leechAmt++;
            }
            if (Main.rand.NextBool(5))
            {
                leechAmt++;
            }
            for (int i = 0; i < leechAmt; i++)
            {
                float xVec = xDist;
                float yVec = yDist;
                float spreadMult = 0.05f * i;
                xVec += Main.rand.NextFloat(-25f, 25f) * spreadMult;
                yVec += Main.rand.NextFloat(-25f, 25f) * spreadMult;
                Vector2 directionToShoot = new Vector2(xVec, yVec);
                speedMult = directionToShoot.Length();
                speedMult = speed / speedMult;
                xVec *= speedMult;
                yVec *= speedMult;
                directionToShoot = new Vector2(xVec, yVec);
                Projectile.NewProjectile(source, playerPos, directionToShoot, type, damage, knockback, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Acidwood>(15).
                AddIngredient<SulphuricScale>(18).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
