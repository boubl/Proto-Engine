using BmFont;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Proto_Engine.Data;
using Proto_Engine.Entities;
using Proto_Engine.Utils;
using Proto_Engine.Scene;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proto_Engine.TheGame
{
    public class GameMode
    {
        private bool dWasPressed = false;
        public static bool debugMode = true;
        public static bool enableInfoText = false;
        private int scale;
        public static int resolutionX = 1920;
        public static int resolutionY = 1080;
        public static bool viewCollisions = false;
        public bool drawPlayerGui = false;
        public static string text =
            "Proto Engine v0.2.1\n" +
            "Copyright (c) 2021 chamalowmoelleux\n" +
            "This is a demo, press [D] to unable/disable debug windows.";

        public const int PixelWidth = 320;
        public const int PixelHeight = 180;

        Vector4[] grayPalette = { new Color(0, 0, 0, 255).ToVector4(), new Color(97, 97, 97, 255).ToVector4(), new Color(175, 175, 175, 255).ToVector4(), new Color(254, 254, 254, 255).ToVector4() };

        RenderTarget2D gameplay;
        RenderTarget2D lightMask;
        RenderTarget2D pixelRender;

        public readonly static BlendState bsSubtract = new BlendState
        {
            ColorSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.One,
            ColorBlendFunction = BlendFunction.ReverseSubtract,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.One,
            AlphaBlendFunction = BlendFunction.ReverseSubtract
        };

        public static ProjectTilesRenderer currentProject;
        public static Player player;

        public void Init(GraphicsDevice graphicsDevice)
        {

            gameplay = new RenderTarget2D(
                            graphicsDevice,
                            PixelWidth,
                            PixelWidth,
                            false,
                            graphicsDevice.PresentationParameters.BackBufferFormat,
                            DepthFormat.Depth24);

            lightMask = new RenderTarget2D(
                            graphicsDevice,
                            PixelWidth,
                            PixelWidth,
                            false,
                            graphicsDevice.PresentationParameters.BackBufferFormat,
                            DepthFormat.Depth24);

            pixelRender = new RenderTarget2D(
                            graphicsDevice,
                            PixelWidth,
                            PixelWidth,
                            false,
                            graphicsDevice.PresentationParameters.BackBufferFormat,
                            DepthFormat.Depth24);

        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            //DataManager
            DataManager.LoadProjects();
            DataManager.LoadTextures(graphicsDevice);
            DataManager.LoadAnimations();
            DataManager.LoadEffects(graphicsDevice);
            DataManager.LoadFonts(graphicsDevice);
            //DataManager.LoadTilesets(GraphicsDevice);
            if (ProtoEngine.ScreenWidth / PixelWidth <= ProtoEngine.ScreenHeight / PixelHeight) scale = ProtoEngine.ScreenWidth / PixelWidth;
            else scale = ProtoEngine.ScreenHeight / PixelHeight;

            // Init the light creator
            LightCreator.InitLight(graphicsDevice);

            player = new Player(new Rectangle(37, 102, 16, 16), "sourya");
            currentProject = new ProjectTilesRenderer("platformer", graphicsDevice);
            Camera.SetCamera(player);
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (!dWasPressed)
                {
                    if (debugMode) debugMode = false;
                    else debugMode = true;
                }
                dWasPressed = true;
            }
            else dWasPressed = false;

            Camera.Update();
            currentProject.Update(graphicsDevice);
            player.Update(gameTime);

            if (ProtoEngine.ScreenWidth / PixelWidth <= ProtoEngine.ScreenHeight / PixelHeight) scale = ProtoEngine.ScreenWidth / PixelWidth;
            else scale = ProtoEngine.ScreenHeight / PixelHeight;
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch _spriteBatch, GameTime gameTime)
        {
            //Gameplay rendering
            graphicsDevice.SetRenderTarget(gameplay);
            graphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            currentProject.Render(_spriteBatch);
            player.Draw(_spriteBatch);
            _spriteBatch.End();

            //Light Mask rendering
            graphicsDevice.SetRenderTarget(lightMask);
            graphicsDevice.Clear(Color.White);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp);
            currentProject.RenderLights(_spriteBatch);
            player.DrawLight(_spriteBatch);
            _spriteBatch.End();

            graphicsDevice.SetRenderTarget(pixelRender);
            graphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            _spriteBatch.Draw(gameplay, new Vector2(0, 0), Color.White);
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Immediate, bsSubtract, SamplerState.PointClamp);
            //_spriteBatch.Draw(lightMask, new Vector2(0, 0), Color.White);
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);

            if (enableInfoText)
                _spriteBatch.DrawString(DataManager.Fonts["Tamsyn"], text, new Vector2(0, 0), Color.White);

            _spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
            DataManager.Effects["restrictPalette"].Parameters["palette"].SetValue(grayPalette);
            //DataManager.Effects["restrictPalette"].CurrentTechnique.Passes[0].Apply();
            _spriteBatch.Draw(
                pixelRender,
                new Vector2((ProtoEngine.ScreenWidth / 2) - (PixelWidth* scale) / 2, (ProtoEngine.ScreenHeight / 2) - (PixelHeight* scale) / 2),
                new Rectangle(0, 0, pixelRender.Width, pixelRender.Height),
                Color.White, 0,
                new Vector2(0, 0),
                scale,
                SpriteEffects.None,
                1
            );
            _spriteBatch.End();


            // Call BeforeLayout first to set things up
            ProtoEngine.ImGuiRenderer.BeforeLayout(gameTime);
            ImGui.PushFont(ProtoEngine.font);
            if (debugMode)
            {
                // Draw our UI
                if (ImGui.BeginMainMenuBar())
                {
                    if (ImGui.MenuItem("Proto Engine")) ProtoEngine.Mode = EngineMode.Editor;
                    ImGui.Text("|");
                    if (ImGui.BeginMenu("Options"))
                    {
                        ImGui.MenuItem("Enable collisions view", null, ref viewCollisions);
                        ImGui.MenuItem("Enable info text", null, ref enableInfoText);
                        ImGui.MenuItem("Player window", null, ref drawPlayerGui);
                        ImGui.EndMenu();
                    }
                    player.DrawImGui(ref drawPlayerGui);
                    ImGui.EndMainMenuBar();
                }

                ImGui.End();
            }

            // Call AfterLayout now to finish up and draw all the things
            ProtoEngine.ImGuiRenderer.AfterLayout();

        }
    }
}
