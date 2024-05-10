﻿using System.Linq;
using CalamityMod.Projectiles.Damageable;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Typeless
{
    public class RelicOfResilience : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Typeless";
        public const int CooldownSeconds = 5;
        public const float WeaknessDR = 0.45f;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanBePlacedOnWeaponRacks[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 34;
            Item.useAnimation = Item.useTime = 28;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item45;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.value = CalamityGlobalItem.RarityPurpleBuyPrice;
            Item.rare = ItemRarityID.Purple;
            Item.shoot = ModContent.ProjectileType<ArtifactOfResilienceBulwark>();
            Item.shootSpeed = 0f;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = (ContentSamples.CreativeHelper.ItemGroup)CalamityResearchSorting.ToolsOther;
        }

        public override bool CanUseItem(Player player) => !player.HasCooldown(Cooldowns.RelicOfResilience.ID);
        public override bool? UseItem(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddCooldown(Cooldowns.RelicOfResilience.ID, CalamityUtils.SecondsToFrames(CooldownSeconds));
            int[] shardTypes = new int[]
            {
                ModContent.ProjectileType<ArtifactOfResilienceShard1>(),
                ModContent.ProjectileType<ArtifactOfResilienceShard2>(),
                ModContent.ProjectileType<ArtifactOfResilienceShard3>(),
                ModContent.ProjectileType<ArtifactOfResilienceShard4>(),
                ModContent.ProjectileType<ArtifactOfResilienceShard5>(),
                ModContent.ProjectileType<ArtifactOfResilienceShard6>(),
            };
            if (player.ownedProjectileCounts[Item.shoot] > 0)
            {
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].type == Item.shoot)
                    {
                        Main.projectile[i].Center = Main.MouseWorld;
                        Main.projectile[i].netUpdate = true;
                    }
                }
            }
            else if (shardTypes.All(proj => player.ownedProjectileCounts[proj] == 0))
                Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
