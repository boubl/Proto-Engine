using BmFont;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_LDtk_Importer;
using Proto_Engine.Entities;
using Proto_Engine.Scene;
using ImGuiNET;
using System;
using System.IO;
using Proto_Engine.Lights;
using System.Collections.Generic;

namespace Proto_Engine
{
    public class ProtoEngine : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

#if DEBUG
        public static bool debug = false;
        private ImGuiRenderer _imGuiRenderer;
        public bool drawPlayerGui = true;
        public static string text = "Test";
#endif

        public static int ScreenWidth = 1920 / 2;
        public static int ScreenHeight = 1080 / 2;
        public static int PixelWidth = ScreenWidth / 3;
        public static int PixelHeight = ScreenHeight / 3;

        RenderTarget2D gameplay;
        RenderTarget2D lightMask;
        RenderTarget2D pixelRender;

        Vector4[] grayPalette = { new Color(0, 0, 0, 255).ToVector4(), new Color(97, 97, 97, 255).ToVector4(), new Color(175, 175, 175, 255).ToVector4(), new Color(254, 254, 254, 255).ToVector4() };

        BitmapFont font;

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


        public ProtoEngine()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            gameplay = new RenderTarget2D(
                            GraphicsDevice,
                            ScreenWidth / 3,
                            ScreenHeight / 3,
                            false,
                            GraphicsDevice.PresentationParameters.BackBufferFormat,
                            DepthFormat.Depth24);

            lightMask = new RenderTarget2D(
                            GraphicsDevice,
                            ScreenWidth / 3,
                            ScreenHeight / 3,
                            false,
                            GraphicsDevice.PresentationParameters.BackBufferFormat,
                            DepthFormat.Depth24);

            pixelRender = new RenderTarget2D(
                            GraphicsDevice,
                            ScreenWidth / 3,
                            ScreenHeight / 3,
                            false,
                            GraphicsDevice.PresentationParameters.BackBufferFormat,
                            DepthFormat.Depth24);

#if DEBUG
            _imGuiRenderer = new ImGuiRenderer(this);
            _imGuiRenderer.RebuildFontAtlas();
#endif

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //DataManager
            DataManager.LoadProjects();
            DataManager.LoadTextures(GraphicsDevice);
            DataManager.LoadAnimations();
            DataManager.LoadEffects(GraphicsDevice);
            //DataManager.LoadTilesets(GraphicsDevice);

            // Init the light creator
            LightCreator.InitLight(GraphicsDevice);

            player = new Player(new Rectangle(37, 102, 16, 16), "noel character");
            currentProject = new ProjectTilesRenderer("platformer", GraphicsDevice);
            Camera.SetCamera(player);
            font = FontLoader.Load("C:/Users/Titou/Documents/GitHub/Proto-Engine/Proto Engine/Content/fonts/Tamsyn.fnt", GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Camera.Update();
            currentProject.Update(GraphicsDevice);
            player.Update(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Gameplay rendering
            GraphicsDevice.SetRenderTarget(gameplay);
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
            currentProject.Render(_spriteBatch);
            player.Draw(_spriteBatch);
            _spriteBatch.End();

            //Light Mask rendering
            GraphicsDevice.SetRenderTarget(lightMask);
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp);
            currentProject.RenderLights(_spriteBatch);
            player.DrawLight(_spriteBatch);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(pixelRender);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            _spriteBatch.Draw(gameplay, new Vector2(0, 0), Color.White);
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Immediate, bsSubtract, SamplerState.PointClamp);
            //_spriteBatch.Draw(lightMask, new Vector2(0, 0), Color.White);
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
#if DEBUG
            _spriteBatch.DrawString(font, text, new Vector2(0, 0), Color.White);
#endif
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
            DataManager.Effects["restrictPalette"].Parameters["palette"].SetValue(grayPalette);
            //DataManager.Effects["restrictPalette"].CurrentTechnique.Passes[0].Apply();
            _spriteBatch.Draw(pixelRender, new Vector2(0, 0), new Rectangle(0, 0, pixelRender.Width, pixelRender.Height), Color.White, 0, new Vector2(0, 0), 3, SpriteEffects.None, 1);
            _spriteBatch.End();

#if DEBUG
            // Call BeforeLayout first to set things up
            _imGuiRenderer.BeforeLayout(gameTime);

            // Draw our UI
            ImGui.Begin("Debug");
            ImGui.Text("Camera offset:" + Camera.offset.ToString());
            ImGui.Text("Current level rectangle: " + currentProject.currentLevelRectangle);
            ImGui.Checkbox("Debug", ref debug);
            ImGui.InputText("Text", ref text, 300);

            if (ImGui.Button("Player window"))
            {
                if (!drawPlayerGui) drawPlayerGui = true;
            }
            if (drawPlayerGui) player.DrawImGui(ref drawPlayerGui);

            ImGui.End();

            // Call AfterLayout now to finish up and draw all the things
            _imGuiRenderer.AfterLayout();
#endif

            base.Draw(gameTime);
        }
    }
}
