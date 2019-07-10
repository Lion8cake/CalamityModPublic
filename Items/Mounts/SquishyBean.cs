﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using CalamityMod.Items;

namespace CalamityMod.Items.Mounts
{
    class SquishyBean : ModMountData
    {
        public override void SetDefaults()
        {
            mountData.buff = mod.BuffType("SquishyBeanBuff");
            mountData.heightBoost = 58;
            mountData.fallDamage = -1;
            mountData.runSpeed = 5f;
            mountData.dashSpeed = 8f;
            mountData.fatigueMax = 0;
            mountData.jumpHeight = 20;
            mountData.acceleration = 0.1f;
            mountData.jumpSpeed = 40f; //20
            mountData.blockExtraJumps = true;
            mountData.totalFrames = 4;
            mountData.constantJump = true;
            mountData.usesHover = false;
            int[] array = new int[mountData.totalFrames];
            for (int l = 0; l < array.Length; l++)
            {
                switch (l)
                {
                    case 0:
                        array[l] = 44;
                        break;
                    case 1:
                        array[l] = 46;
                        break;
                    case 2:
                        array[l] = 48;
                        break;
                    case 3:
                        array[l] = 48;
                        break;
                }
            }
            mountData.playerYOffsets = array;
            mountData.xOffset = 0;
            mountData.bodyFrame = 3;
            mountData.yOffset = 19;
            mountData.playerHeadOffset = 30;
            mountData.standingFrameCount = 1;
            mountData.standingFrameDelay = 12;
            mountData.standingFrameStart = 0;
            mountData.runningFrameCount = 4;
            mountData.runningFrameDelay = 24;
            mountData.runningFrameStart = 0;
            mountData.flyingFrameCount = 0;
            mountData.flyingFrameDelay = 0;
            mountData.flyingFrameStart = 0;
            mountData.inAirFrameCount = 1;
            mountData.inAirFrameDelay = 12;
            mountData.inAirFrameStart = 0;
            mountData.idleFrameCount = 4;
            mountData.idleFrameDelay = 12;
            mountData.idleFrameStart = 0;
            mountData.idleFrameLoop = true;
            mountData.flyingFrameCount = 4;
            mountData.flyingFrameDelay = 12;
            mountData.flyingFrameStart = 0;
            mountData.swimFrameCount = mountData.inAirFrameCount;
            mountData.swimFrameDelay = mountData.inAirFrameDelay;
            mountData.swimFrameStart = mountData.inAirFrameStart;
            if (Main.netMode != 2)
            {
                mountData.textureWidth = mountData.backTexture.Width;
                mountData.textureHeight = mountData.backTexture.Height;
            }
        }

        public override void UpdateEffects(Player player)
        {
            player.statDefense += 60;
            player.gravity = 2f;
            player.maxFallSpeed = 25f;
        }
    }
}

