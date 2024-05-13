﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Summon
{
    public class Endogenesis : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Summon";
        //Cooper be like cool

        public static int AttackMode = 0;
        public override void SetStaticDefaults()
        {
            //Icy no problems with that
        }

        public override void SetDefaults()
        {
            Item.width = 80;
            Item.height = 80;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item78;

            Item.DamageType = DamageClass.Summon;
            Item.mana = 80;
            Item.damage = 1300;
            Item.knockBack = 4f;
            Item.autoReuse = true;
            Item.useTime = Item.useAnimation = 10;
            Item.shoot = ModContent.ProjectileType<EndoCooperBody>();
            Item.shootSpeed = 10f;

            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.Calamity().devItem = true;
        }

        public override bool CanUseItem(Player player) => player.maxMinions >= 10f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                player.itemTime = Item.useTime;
                CalamityUtils.KillShootProjectileMany(player, new int[]
                {
                    type,
                    ModContent.ProjectileType<EndoCooperLimbs>(),
                    ModContent.ProjectileType<EndoBeam>()
                });

                SummonEndoCooper(source, AttackMode, Main.MouseWorld, damage, Item.damage, knockback, player, out _, out _);

                AttackMode++;
                if (AttackMode > 3)
                    AttackMode = 0;
            }
            return false;
        }

        public static void SummonEndoCooper(IEntitySource source, int attackMode, Vector2 spawnPosition, int damage, int baseDamage, float knockback, Player owner, out int bodyIndex, out int limbsIndex)
        {
            bodyIndex = limbsIndex = -1;
            if (Main.myPlayer != owner.whoAmI)
                return;

            float dmgMult = 1f;
            if (attackMode == 0) //lasers
                dmgMult = 0.65f;
            if (attackMode == 1) //icicles
                dmgMult = 1f;
            if (attackMode == 2) //melee
                dmgMult = 0.95f;
            if (attackMode == 3) //flamethrower
                dmgMult = 0.9f;
            bodyIndex = Projectile.NewProjectile(source, spawnPosition, Vector2.Zero, ModContent.ProjectileType<EndoCooperBody>(), (int)(damage * dmgMult), knockback, owner.whoAmI, attackMode, 0f);
            limbsIndex = Projectile.NewProjectile(source, spawnPosition, Vector2.Zero, ModContent.ProjectileType<EndoCooperLimbs>(), (int)(damage * dmgMult), knockback, owner.whoAmI, attackMode, bodyIndex);
            Main.projectile[bodyIndex].ai[1] = limbsIndex;
            Main.projectile[bodyIndex].originalDamage = (int)(baseDamage * dmgMult);
            Main.projectile[limbsIndex].originalDamage = (int)(baseDamage * dmgMult);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CryogenicStaff>().
                AddIngredient(ItemID.BlizzardStaff).
                AddIngredient<EndothermicEnergy>(100).
                AddIngredient<CoreofEleum>(15).
                AddIngredient<ShadowspecBar>(5).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
