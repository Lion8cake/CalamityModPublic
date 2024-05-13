﻿using System;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Rogue
{
    public class Nychthemeron : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.damage = 60;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityLightPurpleBuyPrice;
            Item.rare = ItemRarityID.LightPurple;
            Item.shoot = ModContent.ProjectileType<NychthemeronProjectile>();
            Item.shootSpeed = 6f;
            Item.DamageType = RogueDamageClass.Instance;
        }

        public override bool AltFunctionUse(Player player)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.type == ModContent.ProjectileType<NychthemeronProjectile>() && p.owner == player.whoAmI)
                {
                    p.ai[0] = 1f;
                }
            }
            return true;
        }

        public override float StealthDamageMultiplier => 0.3333f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int orbDamage = (int)(damage * 0.75f);

            if (player.Calamity().StealthStrikeAvailable())
            {
                for (int j = 0; j < 10 - player.ownedProjectileCounts[ModContent.ProjectileType<NychthemeronProjectile>()]; j++)
                {
                    float spread = 2;
                    int pIndex = Projectile.NewProjectile(source, position.X, position.Y, velocity.X + Main.rand.NextFloat(-spread, spread), velocity.Y + Main.rand.NextFloat(-spread, spread), type, damage, knockback, player.whoAmI, 0f, 1f);
                    Projectile p = Main.projectile[pIndex];
                    if (pIndex.WithinBounds(Main.maxProjectiles))
                        p.Calamity().stealthStrike = true;
                    int pID = p.identity;

                    CreateOrbs(source, position, (int)(orbDamage * 0.675f), knockback, pID, player, true);
                }
            }
            else
            {
                int pIndex = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, 1f);
                int pID = Main.projectile[pIndex].identity;

                CreateOrbs(source, position, orbDamage, knockback, pID, player, false);
            }
            return false;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shoot = ProjectileID.None;
                return player.ownedProjectileCounts[ModContent.ProjectileType<NychthemeronProjectile>()] > 0;
            }
            else
            {
                Item.shoot = ModContent.ProjectileType<NychthemeronProjectile>();
                return player.ownedProjectileCounts[ModContent.ProjectileType<NychthemeronProjectile>()] < 10;
            }
        }

        private static void CreateOrbs(IEntitySource source, Vector2 position, int damage, float knockback, int projectileID, Player player, bool stealth)
        {
            float rotationOffset = 0f;

            // Ideally new projectiles will fill in the most recently vacated spots in the pattern
            int[] activeSlots = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.type == ModContent.ProjectileType<NychthemeronOrb>() && proj.owner == player.whoAmI && proj.active && proj.localAI[0] == 0f && activeSlots[(int)proj.localAI[1]] == -1)
                {
                    activeSlots[(int)proj.localAI[1]] = i;
                }
            }

            int pos = 0;
            bool assignedOffset = false;
            for (int i = 0; i < 10; i++)
            {
                if (activeSlots[i] != -1)
                {
                    rotationOffset = Main.projectile[activeSlots[i]].rotation;
                    assignedOffset = true;
                }
                if (activeSlots[i] == -1 && assignedOffset)
                {
                    pos = i;
                    break;
                }
            }

            float orb1Col = 0f;
            float orb2Col = 1f;

            if (pos > 0 && pos < 5)
            {
                rotationOffset += MathHelper.ToRadians(45f);

                orb1Col = pos % 2;
                orb2Col = pos % 2;
            }
            else if (pos >= 5)
            {
                rotationOffset += MathHelper.ToRadians(72f);
            }


            int orb1 = Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<NychthemeronOrb>(), damage, knockback, player.whoAmI, orb1Col, projectileID);
            int orb2 = Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<NychthemeronOrb>(), damage, knockback, player.whoAmI, orb2Col, projectileID);
            if (orb1.WithinBounds(Main.maxProjectiles))
            {
                Main.projectile[orb1].localAI[1] = pos;
                Main.projectile[orb1].rotation = rotationOffset;
                Main.projectile[orb1].Calamity().lineColor = stealth ? 1 : 0;
            }
            if (orb2.WithinBounds(Main.maxProjectiles))
            {
                Main.projectile[orb2].localAI[1] = pos;
                Main.projectile[orb2].rotation = rotationOffset + MathHelper.ToRadians(180f);
                Main.projectile[orb2].Calamity().lineColor = stealth ? 1 : 0;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("AnyMythrilBar", 10).
                AddIngredient(ItemID.DarkShard).
                AddIngredient(ItemID.LightShard).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
