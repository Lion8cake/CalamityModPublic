using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Magic
{
    public class HeliumFlash : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Helium Flash");
            Tooltip.SetDefault("The power of a galaxy, if only for mere moments\n" +
			"Launches volatile star cores which erupt into colossal fusion blasts");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.width = 76;
            item.height = 76;
            item.magic = true;
            item.damage = 1111;
            item.knockBack = 9.5f;
            item.mana = 26;
            item.useAnimation = 37;
            item.useTime = 37;
            item.autoReuse = true;
            item.noMelee = true;

            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item73;

			item.value = CalamityGlobalItem.Rarity14BuyPrice;
			item.rare = ItemRarityID.Purple;
			item.Calamity().customRarity = CalamityRarity.DarkBlue;

			item.shoot = ModContent.ProjectileType<VolatileStarcore>();
            item.shootSpeed = 15f;
        }

        // Creates dust at the tip of the staff when used.
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 dir = new Vector2(speedX, speedY);
            double angle = Math.Atan2(speedY, speedX) + MathHelper.PiOver4;
            dir = dir.SafeNormalize(Vector2.Zero);
            dir *= 64f * 1.4142f; // distance to gleaming point on staff
            Vector2 dustPos = position + dir;

            int dustType = 162;
            int dustCount = 72;
            float minSpeed = 4f;
            float maxSpeed = 11f;
            float minScale = 0.8f;
            float maxScale = 1.4f;
            Vector2 leftVec = new Vector2(-1f, 0f).RotatedBy(angle);
            Vector2 rightVec = new Vector2(1f, 0f).RotatedBy(angle);
            Vector2 upVec = new Vector2(0f, -1f).RotatedBy(angle);
            Vector2 downVec = new Vector2(0f, 1f).RotatedBy(angle);
            for (int i = 0; i < dustCount; i += 4)
            {
                int left = Dust.NewDust(dustPos, 1, 1, dustType, 0f, 0f);
                Main.dust[left].position = dustPos;
                Main.dust[left].velocity = leftVec * Main.rand.NextFloat(minSpeed, maxSpeed);
                Main.dust[left].scale = Main.rand.NextFloat(minScale, maxScale);
                Main.dust[left].noGravity = true;

                int right = Dust.NewDust(dustPos, 1, 1, dustType, 0f, 0f);
                Main.dust[right].position = dustPos;
                Main.dust[right].velocity = rightVec * Main.rand.NextFloat(minSpeed, maxSpeed);
                Main.dust[right].scale = Main.rand.NextFloat(minScale, maxScale);
                Main.dust[right].noGravity = true;

                int up = Dust.NewDust(dustPos, 1, 1, dustType, 0f, 0f);
                Main.dust[up].position = dustPos;
                Main.dust[up].velocity = upVec * Main.rand.NextFloat(minSpeed, maxSpeed);
                Main.dust[up].scale = Main.rand.NextFloat(minScale, maxScale);
                Main.dust[up].noGravity = true;

                int down = Dust.NewDust(dustPos, 1, 1, dustType, 0f, 0f);
                Main.dust[down].position = dustPos;
                Main.dust[down].velocity = downVec * Main.rand.NextFloat(minSpeed, maxSpeed);
                Main.dust[down].scale = Main.rand.NextFloat(minScale, maxScale);
                Main.dust[down].noGravity = true;
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddTile(TileID.LunarCraftingStation);
            r.AddIngredient(ModContent.ItemType<VenusianTrident>());
            r.AddIngredient(ModContent.ItemType<CalamitasInferno>());
            r.AddIngredient(ModContent.ItemType<ForbiddenSun>());
            r.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 10);
            r.AddIngredient(ModContent.ItemType<DarksunFragment>(), 10);
            r.AddIngredient(ItemID.FragmentSolar, 80);
            r.AddIngredient(ItemID.FragmentNebula, 20);
            r.AddRecipe();
        }
    }
}
