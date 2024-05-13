﻿using System;
using System.Collections.Generic;
using CalamityMod.Items.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Systems
{
    public interface IPingedTileEffect
    {
        /// <summary>
        /// The blend state of the tile effect's main shader.
        /// </summary>
        public BlendState BlendState => BlendState.AlphaBlend;
        /// <summary>
        /// Create and set up an effect to be drawn over all the registered tiles for this shader.
        /// </summary>
        /// <returns>The configured effect</returns>
        public Effect SetupEffect();
        /// <summary>
        /// Modifies the shader for each tile. Use this if your shader is using tile-specific data.
        /// </summary>
        /// <param name="pos">The position of the tile</param>
        /// <param name="effect">The effect being used</param>
        public void PerTileSetup(Point pos, ref Effect effect) { }
        /// <summary>
        /// Draws the tile, or an overlay for it. The shader is automatically applied.
        /// </summary>
        /// <param name="pos">The position of the tile</param>
        public void DrawTile(Point pos);
        /// <summary>
        /// What happens when a ping for this effect gets requested. Return false if the ping couldn't get added.
        /// </summary>
        /// <param name="position">Position of the ping being requested</param>
        /// <param name="pinger">The player that initiated the ping</param>
        /// <returns>Wether or not the ping's setup was successful</returns>
        public bool TryAddPing(Vector2 position, Player pinger);
        /// <summary>
        /// Wether or not this effect is active.
        /// </summary>
        public bool Active => true;
        /// <summary>
        /// Check to know if a tile needs to be drawn with this effect. This is only called if the effect is active.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Wether or not the tile should be drawn with the effect</returns>
        public bool ShouldRegisterTile(int x, int y);
        /// <summary>
        /// Called after a tile has been queued to be drawn with this effect. Can be used to edit the draw data of the tile.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="drawData">The tile's draw data</param>
        public void EditDrawData(int i, int j, ref TileDrawInfo drawData) { }
        /// <summary>
        /// Update call ran once per frame.
        /// </summary>
        public void UpdateEffect() { }
    }

    public class TilePingerSystem : ModSystem
    {
        internal static Dictionary<IPingedTileEffect, List<Point>> pingedTiles;
        internal static Dictionary<IPingedTileEffect, List<Point>> pingedNonSolidTiles;
        internal static Dictionary<IPingedTileEffect, List<Point>> drawCache;

        internal static Dictionary<string, IPingedTileEffect> tileEffects;

        public override void Load()
        {
            if (Main.dedServ)
                return;

            drawCache = new Dictionary<IPingedTileEffect, List<Point>>();
            pingedTiles = new Dictionary<IPingedTileEffect, List<Point>>();
            pingedNonSolidTiles = new Dictionary<IPingedTileEffect, List<Point>>();
            tileEffects = new Dictionary<string, IPingedTileEffect>();
        }

        public override void Unload()
        {
            drawCache = null;
            pingedTiles = null;
            pingedNonSolidTiles = null;
            tileEffects = null;
        }

        public static bool AddPing(string effectName, Vector2 position, Player pinger)
        {
            if (!Main.dedServ)
                return AddPing(tileEffects[effectName], position, pinger);

            return false;
        }

        public static bool AddPing(IPingedTileEffect effect, Vector2 position, Player pinger) => effect.TryAddPing(position, pinger);


        public static void RegisterTileToDraw(Point tilePos, string effectName, bool solid = true) => RegisterTileToDraw(tilePos, tileEffects[effectName], solid);
        public static void RegisterTileToDraw(Point tilePos, IPingedTileEffect effect, bool solid = true)
        {
            //Unless we are in color light mode, we do not need the distinction between solid and non solid tiles.
            if (solid || !(Lighting.Mode == LightMode.Color))
            {
                if (!pingedTiles.ContainsKey(effect))
                    pingedTiles.Add(effect, new List<Point>());

                if (!pingedTiles[effect].Contains(tilePos))
                    pingedTiles[effect].Add(tilePos);
            }

            else
            {
                if (!pingedNonSolidTiles.ContainsKey(effect))
                    pingedNonSolidTiles.Add(effect, new List<Point>());

                if (!pingedNonSolidTiles[effect].Contains(tilePos))
                    pingedNonSolidTiles[effect].Add(tilePos);


            }
        }

        public override void PostUpdateEverything()
        {
            if (Main.dedServ || tileEffects is null)
                return;

            foreach (IPingedTileEffect effect in tileEffects.Values)
            {
                effect.UpdateEffect();
            }
        }

        public override void PostDrawTiles()
        {
            if (pingedTiles is null || (pingedTiles.Keys.Count + pingedNonSolidTiles.Count < 1))
                return;

            drawCache.Clear();

            foreach (IPingedTileEffect solidEffect in pingedTiles.Keys)
            {
                drawCache.Add(solidEffect, pingedTiles[solidEffect].ConvertAll(position => new Point(position.X, position.Y)));
            }

            if (Lighting.Mode == LightMode.Color)
            {
                foreach (IPingedTileEffect nonSolidEffect in pingedNonSolidTiles.Keys)
                {
                    List<Point> clonedList = pingedNonSolidTiles[nonSolidEffect].ConvertAll(position => new Point(position.X, position.Y));

                    if (!drawCache.ContainsKey(nonSolidEffect))
                        drawCache.Add(nonSolidEffect, clonedList);

                    else
                    {
                        drawCache[nonSolidEffect].AddRange(clonedList);
                    }
                }
            }

            foreach (IPingedTileEffect tileEffect in drawCache.Keys)
            //foreach (IPingedTileEffect tileEffect in pingedTiles.Keys)
            {
                Effect effect = tileEffect.SetupEffect();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, tileEffect.BlendState, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, effect, Main.GameViewMatrix.TransformationMatrix);

                foreach (Point tilePos in drawCache[tileEffect])
                //foreach (Point tilePos in pingedTiles[tileEffect])
                {
                    tileEffect.PerTileSetup(tilePos, ref effect);
                    tileEffect.DrawTile(tilePos);

                }

                Main.spriteBatch.End();
            }
        }

        public static void ClearTiles()
        {
            ClearTiles(true);
            ClearTiles(false);
        }
        public static void ClearTiles(bool solid)
        {
            if (solid)
                pingedTiles.Clear();

            else
                pingedNonSolidTiles.Clear();
        }
    }

    public class GlobalPingableTile : GlobalTile
    {
        public override void DrawEffects(int i, int j, int type, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            foreach (IPingedTileEffect effect in TilePingerSystem.tileEffects.Values)
            {
                if (effect.Active && effect.ShouldRegisterTile(i, j))
                {
                    int tileType = Main.tile[i, j].TileType;
                    bool solid = true;

                    //Necessary separation in the color lighting mode.
                    if (Lighting.Mode == LightMode.Color)
                    {
                        if (TileID.Sets.DrawTileInSolidLayer[tileType].HasValue)
                            solid = TileID.Sets.DrawTileInSolidLayer[tileType].Value;
                        else
                            solid = Main.tileSolid[tileType];
                    }

                    TilePingerSystem.RegisterTileToDraw(new Point(i, j), effect, solid);
                    effect.EditDrawData(i, j, ref drawData);
                }
            }
        }
    }

    public class WulfrumPingTileEffect : IPingedTileEffect, ILoadable
    {
        internal static Texture2D emptyFrame;
        const int MaxPingLife = 350;
        const int MaxPingTravelTime = 60;
        const float PingWaveThickness = 50f;

        const float MaxPingRadius = 1700f;
        public static Vector2 PingCenter = Vector2.Zero;
        public static int PingTimer = 0;
        public static float PingProgress => (MaxPingLife - PingTimer) / (float)MaxPingLife;

        public bool Active => PingTimer > 0;

        public BlendState BlendState => BlendState.Additive;

        public bool TryAddPing(Vector2 position, Player pinger)
        {
            //Only one ping at a time
            if (Active)
                return false;

            PingCenter = position;
            PingTimer = MaxPingLife;
            return true;
        }


        public Effect SetupEffect()
        {
            if (emptyFrame == null)
                emptyFrame = ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj").Value;

            Effect tileEffect = Filters.Scene["CalamityMod:WulfrumTilePing"].GetShader().Shader;
            tileEffect.Parameters["pingCenter"].SetValue(PingCenter);
            tileEffect.Parameters["pingRadius"].SetValue(MaxPingRadius);
            tileEffect.Parameters["pingWaveThickness"].SetValue(PingWaveThickness);
            tileEffect.Parameters["pingProgress"].SetValue(PingProgress);
            tileEffect.Parameters["pingTravelTime"].SetValue(MaxPingTravelTime / (float)MaxPingLife);
            tileEffect.Parameters["pingFadePoint"].SetValue(0.9f);
            tileEffect.Parameters["edgeBlendStrength"].SetValue(1f);
            tileEffect.Parameters["edgeBlendOutLength"].SetValue(6f);
            tileEffect.Parameters["tileEdgeBlendStrenght"].SetValue(2f);

            tileEffect.Parameters["waveColor"].SetValue(Color.GreenYellow.ToVector4());
            tileEffect.Parameters["baseTintColor"].SetValue(Color.DeepSkyBlue.ToVector4() * 0.5f);
            tileEffect.Parameters["scanlineColor"].SetValue(Color.YellowGreen.ToVector4() * 1f);
            tileEffect.Parameters["tileEdgeColor"].SetValue(Color.GreenYellow.ToVector3());
            tileEffect.Parameters["Resolution"].SetValue(8f);

            tileEffect.Parameters["time"].SetValue(Main.GameUpdateCount);
            Vector4[] scanLines = new Vector4[]
            {
                new Vector4(0f, 4f, 0.1f, 0.5f),
                new Vector4(1f, 4f, 0.1f, 0.5f),
                new Vector4(37f, 60f, 0.4f, 1f),
                new Vector4(2f, 6f, -0.2f, 0.3f),
                new Vector4(0f, 4f, 0.1f, 0.5f), //vertical start
                new Vector4(1f, 4f, 0.1f, 0.5f),
                new Vector4(2f, 6f, -0.2f, 0.3f)
            };

            tileEffect.Parameters["ScanLines"].SetValue(scanLines);
            tileEffect.Parameters["ScanLinesCount"].SetValue(scanLines.Length);
            tileEffect.Parameters["verticalScanLinesIndex"].SetValue(4);

            return tileEffect;
        }

        public void PerTileSetup(Point pos, ref Effect effect)
        {
            //Up, left, right, down.
            effect.Parameters["cardinalConnections"].SetValue(new bool[] { Connected(pos, 0, -1), Connected(pos, -1, 0), Connected(pos, 1, 0), Connected(pos, 0, 1) });
            effect.Parameters["tilePosition"].SetValue(pos.ToVector2() * 16f);
        }

        public static bool Connected(Point pos, int displaceX, int displaceY) => Main.IsTileSpelunkable(pos.X + displaceX, pos.Y + displaceY) && Main.tile[pos].TileType == Main.tile[pos.X + displaceX, pos.Y + displaceY].TileType;

        public bool ShouldRegisterTile(int x, int y)
        {
            return Main.IsTileSpelunkable(x, y);
        }

        public void DrawTile(Point pos)
        {
            Main.spriteBatch.Draw(emptyFrame, pos.ToWorldCoordinates() - Main.screenPosition, null, Color.White, 0, new Vector2(emptyFrame.Width / 2f, emptyFrame.Height / 2f), 16f, 0, 0);
        }

        public void EditDrawData(int i, int j, ref TileDrawInfo drawData)
        {
            float distanceFromCenter = (new Point(i, j).ToWorldCoordinates() - PingCenter).Length();
            float currentExpansion = MathHelper.Clamp(PingProgress * MaxPingLife / (float)MaxPingTravelTime, 0f, 1f) * MaxPingRadius;

            if (distanceFromCenter - 8 > currentExpansion)
                return;

            float brightness = 1f;
            Tile tile = Framing.GetTileSafely(i, j);
            //Counteracts slopes and half tiles being too bright
            if (tile.Slope != SlopeType.Solid || tile.IsHalfBlock)
                brightness = 0.64f;

            //Fade on the edges
            if (distanceFromCenter + 8 > currentExpansion)
                brightness *= 1 - (distanceFromCenter - currentExpansion + 8f) / 16f;

            //Fade away with the effect
            brightness *= 1 - Math.Max(PingProgress - 0.9f, 0) / (0.1f);

            if (drawData.tileLight.R < 200 * brightness) drawData.tileLight.R = (byte)(200 * brightness);
            if (drawData.tileLight.G < 200 * brightness) drawData.tileLight.G = (byte)(200 * brightness);
            if (drawData.tileLight.B < 200 * brightness) drawData.tileLight.B = (byte)(200 * brightness);
        }

        public void UpdateEffect()
        {
            if (PingTimer > 0)
            {
                PingTimer--;

                //if the effect ended (and the player has a treasure pigner in their inventory, of course), play a recharge beep
                if (PingTimer == 0 && Main.LocalPlayer.InventoryHas(ModContent.ItemType<WulfrumTreasurePinger>()))
                    SoundEngine.PlaySound(WulfrumTreasurePinger.RechargeBeepSound);
            }
        }

        public void Load(Mod mod)
        {
            if (Main.dedServ)
                return;

            TilePingerSystem.tileEffects.Add("WulfrumPing", this);
        }

        public void Unload() { }
    }
}
