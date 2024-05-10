﻿using CalamityMod.Particles;
using CalamityMod.Projectiles.Summon.SmallAresArms;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Summon
{
    public class AresExoskeleton : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Summon";
        public int FrameCounter = 0;

        public int Frame = 0;

        public const int BoxParticleLifetime = 95;

        public const int PlasmaCannonShootRate = 30;

        public const int TeslaCannonShootRate = 36;

        public const int LaserCannonNormalShootRate = 15;

        public const int GaussNukeShootRate = 240;

        public const float TargetingDistance = 1020f;

        public const float MinionSlotsPerCannon = 3f;

        public const float PlasmaCannonBlastFactor = 0.9f;

        // Multiple orbs can arc together when there are 2+ tesla cannons, resulting in a LOT of hits.
        // The local i-frames are also quite high to combat this problem.
        public const float TeslaOrbDamageFactor = 1f;

        public const float LaserDamageFactor = 1.1f;

        public const float NukeDamageFactor = 1f;

        public const float MaxNukeExplosionRadius = 720f;

        public const float TeslaOrbDetatchDistance = 1500f;

        public static bool ArmExists(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ExoskeletonPlasmaCannon>()] >= 1)
                return true;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ExoskeletonTeslaCannon>()] >= 1)
                return true;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ExoskeletonLaserCannon>()] >= 1)
                return true;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ExoskeletonGaussNukeCannon>()] >= 1)
                return true;

            return false;
        }

        public override void Load()
        {
            // Add the body equip texture.
            if (Main.netMode != NetmodeID.Server)
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Body}", EquipType.Body, this);
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 36;
            Item.mana = 80;
            Item.damage = 625;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noUseGraphic = true;
            Item.useTime = Item.useAnimation = 9;
            Item.noMelee = true;
            Item.knockBack = 1f;

            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();

            Item.UseSound = SoundID.Item117;
            Item.shoot = ModContent.ProjectileType<ExoskeletonPlasmaCannon>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameI, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Summon/AresExoskeleton").Value;
            if (!Main.gameMenu && ArmExists(Main.LocalPlayer))
            {
                texture = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Summon/AresExoskeletonRemote").Value;
                position.X += scale * 6f;
            }

            spriteBatch.Draw(texture, position, new(0, 0, texture.Width, texture.Height), Color.White, 0f, origin, scale, 0, 0);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Summon/AresExoskeleton").Value;
            spriteBatch.Draw(texture, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref Frame, ref FrameCounter, 1, 1), lightColor, 0f, Vector2.Zero, 1f, 0, 0);
            return false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int panelID = ModContent.ProjectileType<ExoskeletonPanel>();

            // If the player owns a panel, make it fade away.
            if (player.ownedProjectileCounts[panelID] >= 1)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].type != panelID || Main.projectile[i].owner != player.whoAmI || !Main.projectile[i].active)
                        continue;

                    Main.projectile[i].ai[0] = 1f;
                    Main.projectile[i].netUpdate = true;
                }
            }

            // Otherwise, create one. While it doesn't do damage on its own, it does store it for reference by the cannons that might be spawned.
            else
            {
                int panel = Projectile.NewProjectile(source, position, Vector2.Zero, panelID, damage, 0f, player.whoAmI);
                if (Main.projectile.IndexInRange(panel))
                    Main.projectile[panel].originalDamage = Item.damage;

                // Also throw a cool mechanical box particle out.
                Vector2 boxVelocity = -Vector2.UnitY.RotatedByRandom(0.7f) * 6f + Vector2.UnitX * player.direction * 4f;
                Particle box = new AresSummonCrateParticle(player, boxVelocity, BoxParticleLifetime);
                GeneralParticleHandler.SpawnParticle(box);
            }

            return false;
        }
    }
}
