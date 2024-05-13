﻿using System.Collections.Generic;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    [LegacyName("BladedgeGreatbow")]
    public class BladedgeRailbow : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";

        #region GFB Arrow Array
        public int[] arrowArr =
        {
            ProjectileID.WoodenArrowFriendly,
            ProjectileID.FireArrow,
            ProjectileID.UnholyArrow,
            ProjectileID.JestersArrow,
            ProjectileID.HellfireArrow,
            ProjectileID.HolyArrow,
            ProjectileID.CursedArrow,
            ProjectileID.FrostburnArrow,
            ProjectileID.ChlorophyteArrow,
            ProjectileID.IchorArrow,
            ProjectileID.VenomArrow,
            ProjectileID.BoneArrowFromMerchant,
            ProjectileID.MoonlordArrow,
            ProjectileID.ShimmerArrow,
            ProjectileID.DD2BetsyArrow,
            ProjectileID.BloodArrow,
            ProjectileID.FairyQueenRangedItemShot,
            ProjectileID.Hellwing,
            ProjectileID.FrostArrow,
            ProjectileID.BoneArrow,
            ProjectileID.DD2PhoenixBowShot,
            ProjectileID.PulseBolt,
            ProjectileID.ShadowFlameArrow,
            ProjectileID.BeeArrow,
            ProjectileID.Stake,

            ModContent.ProjectileType<BloodfireArrowProj>(),
            ModContent.ProjectileType<CinderArrowProj>(),
            ModContent.ProjectileType<ElysianArrowProj>(),
            ModContent.ProjectileType<IcicleArrowProj>(),
            ModContent.ProjectileType<SproutingArrowMain>(),
            ModContent.ProjectileType<SproutingArrowSplit>(),
            ModContent.ProjectileType<VanquisherArrowProj>(),
            ModContent.ProjectileType<VeriumBoltProj>(),
            ModContent.ProjectileType<TyphoonArrow>(),
            ModContent.ProjectileType<MiniSharkron>(),
            ModContent.ProjectileType<TorrentialArrow>(),
            ModContent.ProjectileType<AstrealArrow>(),
            ModContent.ProjectileType<BarinadeArrow>(),
            ModContent.ProjectileType<BoltArrow>(),
            ModContent.ProjectileType<LeafArrow>(),
            ModContent.ProjectileType<SporeBomb>(),
            ModContent.ProjectileType<BrimstoneBolt>(),
            ModContent.ProjectileType<PrecisionBolt>(),
            ModContent.ProjectileType<CondemnationArrow>(),
            ModContent.ProjectileType<ContagionArrow>(),
            ModContent.ProjectileType<CorrodedShell>(),
            ModContent.ProjectileType<DaemonsFlameArrow>(),
            ModContent.ProjectileType<DWArrow>(),
            ModContent.ProjectileType<DrataliornusFlame>(),
            ModContent.ProjectileType<FlareBat>(),
            ModContent.ProjectileType<FeatherLarge>(),
            ModContent.ProjectileType<SlimeStream>(), // Not ranged
            ModContent.ProjectileType<ExoCrystalArrow>(),
            ModContent.ProjectileType<MistArrow>(),
            ModContent.ProjectileType<LunarBolt>(),
            ModContent.ProjectileType<PlagueArrow>(),
            ModContent.ProjectileType<PlanetaryAnnihilationProj>(),
            ModContent.ProjectileType<Shell>(),
            ModContent.ProjectileType<TelluricGlareArrow>(),
            ModContent.ProjectileType<BallistaGreatArrow>(),
            ModContent.ProjectileType<TheMaelstromShark>(),
            ModContent.ProjectileType<Bolt>(),
            ModContent.ProjectileType<ToxicArrow>(),
            ModContent.ProjectileType<UltimaBolt>(),
            ModContent.ProjectileType<UltimaRay>(),
            ModContent.ProjectileType<UltimaSpark>(),
            ModContent.ProjectileType<VernalBolt>()
        };
        #endregion

        public override void SetDefaults()
        {
            Item.width = 74;
            Item.height = 24;
            Item.damage = 30;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 29;
            Item.useAnimation = 29;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 14f;
            Item.useAmmo = AmmoID.Arrow;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 4; i++)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-60, 61) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-60, 61) * 0.05f;
                int index = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
                Main.projectile[index].noDropItem = true;
            }
            Vector2 realPlayerPos = player.RotatedRelativePoint(player.MountedCenter, true);
            float PiOver10 = MathHelper.Pi * 0.1f;
            Vector2 arrowVel = velocity;
            arrowVel.Normalize();
            arrowVel *= 10f;
            bool arrowHitsTiles = Collision.CanHit(realPlayerPos, 0, 0, realPlayerPos + arrowVel, 0, 0);

            int numArrows = Main.zenithWorld ? 4 : 2;
            for (int i = 0; i < numArrows; i++)
            {
                float arrowOffset = (float)i - 1f / 2f;
                Vector2 offsetSpawn = arrowVel.RotatedBy((double)(PiOver10 * arrowOffset), default);
                if (!arrowHitsTiles)
                {
                    offsetSpawn -= arrowVel;
                }

                int projType;
                if (Main.zenithWorld)
                    projType = arrowArr[Main.rand.Next(0, arrowArr.Length)];
                else
                    projType = ProjectileID.Leaf;

                int projectile = Projectile.NewProjectile(source, realPlayerPos.X + offsetSpawn.X, realPlayerPos.Y + offsetSpawn.Y, velocity.X, velocity.Y, projType, damage, 0f, player.whoAmI);
                // Now for the special conditions to fix stuff...
                if (projectile.WithinBounds(Main.maxProjectiles) && (projType == ProjectileID.Leaf || projType == ModContent.ProjectileType<SlimeStream>()))
                    Main.projectile[projectile].DamageType = DamageClass.Ranged;
                if (projType == ModContent.ProjectileType<AstrealArrow>() || projType == ModContent.ProjectileType<CorrodedShell>() || projType == ModContent.ProjectileType<Shell>())
                    Main.projectile[projectile].velocity /= 2;
            }
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string tooltip = Main.zenithWorld ? this.GetLocalizedValue("TooltipGFB") : this.GetLocalizedValue("TooltipNormal");
            tooltips.FindAndReplace("[GFB]", tooltip);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PerennialBar>(12).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
