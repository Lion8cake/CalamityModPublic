﻿using CalamityMod.CalPlayer;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories.Vanity
{
    [AutoloadEquip(EquipType.Head)]

    //A lot of legacy names that's for sure. A combo of the pre "WulfrumHeadX" names, and the aforementionned "WulfrumHeadX" names.
    //This is done so that non summoners don't end up with a helmet they don't really care about anyways, and is a cute reference to the old look.
    [LegacyName("WulfrumMask")]
    [LegacyName("WulfrumHeadRogue")]
    [LegacyName("WulfrumHeadgear")]
    [LegacyName("WulfrumHeadRanged")]
    [LegacyName("WulfrumHelm")]
    [LegacyName("WulfrumHeadMelee")]
    [LegacyName("WulfrumHood")]
    [LegacyName("WulfrumHeadMagic")]
    public class AbandonedWulfrumHelmet : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                EquipLoader.AddEquipTexture(Mod, "CalamityMod/Items/Accessories/Vanity/AbandonedWulfrumHelmet_HeadSet", EquipType.Head, name: "WulfrumOldSetHead");
                EquipLoader.AddEquipTexture(Mod, "CalamityMod/Items/Accessories/Vanity/AbandonedWulfrumHelmet_Body", EquipType.Body, this);
                EquipLoader.AddEquipTexture(Mod, "CalamityMod/Items/Accessories/Vanity/AbandonedWulfrumHelmet_Legs", EquipType.Legs, this);
            }
        }

        public override void SetStaticDefaults()
        {

            if (Main.netMode == NetmodeID.Server)
                return;

            int equipSlotHead = EquipLoader.GetEquipSlot(Mod, "WulfrumOldSetHead", EquipType.Head);
            ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;

            int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
            ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;

            int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
            ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 30;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.rare = ItemRarityID.Green;
            Item.vanity = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<WulfrumTransformationPlayer>().transformationActive = true;
            player.GetModPlayer<WulfrumTransformationPlayer>().vanityEquipped = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
            {
                player.GetModPlayer<WulfrumTransformationPlayer>().transformationActive = true;
                player.GetModPlayer<WulfrumTransformationPlayer>().vanityEquipped = true;
            }
        }
    }

    public class WulfrumTransformationPlayer : ModPlayer
    {
        public bool vanityEquipped = false;
        public bool transformationActive = false;
        public bool forceHelmetOn = false;

        public override void ResetEffects()
        {
            vanityEquipped = false;
            transformationActive = false;
            forceHelmetOn = false;
        }

        public override void FrameEffects()
        {
            if (forceHelmetOn || transformationActive)
            {
                Player.head = EquipLoader.GetEquipSlot(Mod, "WulfrumOldSetHead", EquipType.Head);
                Player.face = -1;
            }

            if (transformationActive)
            {
                Player.legs = EquipLoader.GetEquipSlot(Mod, "AbandonedWulfrumHelmet", EquipType.Legs);
                Player.body = EquipLoader.GetEquipSlot(Mod, "AbandonedWulfrumHelmet", EquipType.Body);

                Player.HideAccessories();
            }
        }
    }
}
