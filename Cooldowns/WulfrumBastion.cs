﻿using System;
using CalamityMod.Items.Armor.Wulfrum;
using CalamityMod.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;

namespace CalamityMod.Cooldowns
{
    public class WulfrumBastion : CooldownHandler
    {
        public bool PowerActive => instance.timeLeft > WulfrumHat.BastionCooldown;
        public float DurabilityPercent => (instance.timeLeft - WulfrumHat.BastionCooldown) / (float)WulfrumHat.BastionTime;

        public static new string ID => "WulfrumBastion";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => CalamityUtils.GetText("UI.Cooldowns.WulfrumBastion" + (PowerActive ? "Active" : "Cooldown"));
        public override string Texture => PowerActive ? "CalamityMod/Cooldowns/WulfrumBastionActive" : "CalamityMod/Cooldowns/WulfrumBastion";
        public override string OutlineTexture => "CalamityMod/Cooldowns/WulfrumBastionOutline";
        public override string OverlayTexture => "CalamityMod/Cooldowns/WulfrumBastionOverlay";
        public override Color OutlineColor => PowerActive ? new Color(194, 255, 67) : new Color(206, 201, 170);
        public override Color CooldownStartColor => PowerActive ? Color.Lerp(new Color(112, 244, 244), new Color(54, 177, 221), DurabilityPercent) : new Color(92, 187, 99);
        public override Color CooldownEndColor => PowerActive ? Color.Lerp(new Color(112, 244, 244), new Color(54, 177, 221), DurabilityPercent) : new Color(160, 232, 77);
        public override SoundStyle? EndSound => new("CalamityMod/Sounds/Custom/AbilitySounds/WulfrumBastionRecharge");
        //public override bool SavedWithPlayer => false;

        public override void OnCompleted()
        {
            for (int i = 0; i < 6; i++)
            {
                Vector2 dustDirection = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust d = Dust.NewDustPerfect(instance.player.Center + dustDirection * Main.rand.NextFloat(0.4f, 10f), 226, dustDirection * Main.rand.NextFloat(1f, 4f), 100, Color.Transparent, Main.rand.NextFloat(0.8f, 1.2f));
                d.noGravity = true;
                d.noLight = true;
                d.fadeIn = 1f;
            }
        }

        //Charge down at first, and then charge back up
        private float AdjustedCompletion => CooldownRackUI.DebugFullDisplay ? CooldownRackUI.DebugForceCompletion : PowerActive ? (instance.timeLeft - WulfrumHat.BastionCooldown) / (float)WulfrumHat.BastionTime : 1 - (instance.timeLeft / (float)WulfrumHat.BastionCooldown);

        public override void ApplyBarShaders(float opacity)
        {
            //Use the adjusted completion
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseOpacity(opacity);
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseSaturation(AdjustedCompletion);
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseColor(CooldownStartColor);
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseSecondaryColor(CooldownEndColor);
            GameShaders.Misc["CalamityMod:CircularBarShader"].Apply();
        }

        public override void DrawCompact(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
        {
            Texture2D sprite = Request<Texture2D>(Texture).Value;
            Texture2D outline = Request<Texture2D>(OutlineTexture).Value;
            Texture2D overlay = Request<Texture2D>(OverlayTexture).Value;

            //Draw the outline
            spriteBatch.Draw(outline, position, null, OutlineColor * opacity, 0, outline.Size() * 0.5f, scale, SpriteEffects.None, 0f);

            //Draw the icon
            spriteBatch.Draw(sprite, position, null, Color.White * opacity, 0, sprite.Size() * 0.5f, scale, SpriteEffects.None, 0f);

            //Draw the small overlay
            int lostHeight = (int)Math.Ceiling(overlay.Height * AdjustedCompletion);
            Rectangle crop = new Rectangle(0, lostHeight, overlay.Width, overlay.Height - lostHeight);
            spriteBatch.Draw(overlay, position + Vector2.UnitY * lostHeight * scale, crop, OutlineColor * opacity * 0.9f, 0, sprite.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}
