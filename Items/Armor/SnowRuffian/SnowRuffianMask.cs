﻿using CalamityMod.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor.SnowRuffian
{
    [AutoloadEquip(EquipType.Head)]
    public class SnowRuffianMask : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.PreHardmode";
        private bool shouldBoost = false;


        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                EquipLoader.AddEquipTexture(Mod, "CalamityMod/Items/Armor/SnowRuffian/SnowRuffianWings", EquipType.Wings, this, equipTexture: new SnowRuffianWings());
            }
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 2; //9
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SnowRuffianChestplate>() && legs.type == ModContent.ItemType<SnowRuffianGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer = player.Calamity();
            modPlayer.snowRuffianSet = true;
            modPlayer.rogueStealthMax += 0.5f;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            player.GetDamage<ThrowingDamageClass>() += 0.05f;
            player.Calamity().wearingRogueArmor = true;

            if (player.controlJump)
            {
                player.noFallDmg = true;
                player.UpdateJumpHeight();
                if (shouldBoost && !player.mount.Active)
                {
                    player.velocity.X *= 1.1f;
                    shouldBoost = false;
                }

            }
            else if (!shouldBoost && player.velocity.Y == 0)
            {
                shouldBoost = true;
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<ThrowingDamageClass>() += 0.02f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("AnySnowBlock", 20).
                AddIngredient(ItemID.FlinxFur).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class SnowRuffianWings : EquipTexture
    {
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.5f;
            ascentWhenRising = 0f;
            maxCanAscendMultiplier = 0f;
            maxAscentMultiplier = 0f;
            constantAscend = 0f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 2f;
            acceleration *= 1.25f;
        }
    }
}
