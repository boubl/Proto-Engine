using BmFont;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_LDtk_Importer;
using Proto_Engine.ECS;
using Proto_Engine.Scene;
using ImGuiNET;
using System;

namespace Proto_Engine
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static int ScreenWidth = 1920 / 2;
        public static int ScreenHeight = 1080 / 2;
        public static int PixelWidth = ScreenWidth / 3;
        public static int PixelHeight = ScreenHeight / 3;

        RenderTarget2D renderTarget;
        BitmapFont font;
        
        private ImGuiRenderer _imGuiRenderer;
        IntPtr imGuiTx;

        public static ProjectTilesRenderer currentProject;
        public static Player player;

        public string text = "Test";

        public Game1()
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
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            renderTarget = new RenderTarget2D(
                            GraphicsDevice,
                            ScreenWidth / 3,
                            ScreenHeight / 3,
                            false,
                            GraphicsDevice.PresentationParameters.BackBufferFormat,
                            DepthFormat.Depth24);

            _imGuiRenderer = new ImGuiRenderer(this);
            _imGuiRenderer.RebuildFontAtlas();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            DataManager.LoadProjects();
            //DataManager.LoadTilesets(GraphicsDevice);
            player = new Player(new Rectangle(0, 1080, 16, 16), GraphicsDevice);
            currentProject = new ProjectTilesRenderer("Test_file_for_API_showing_all_features", GraphicsDevice);
            Camera.SetCamera(player);
            font = FontLoader.Load("/Users/AlexisNicolas/Documents/GitHub/Proto-Engine/Proto Engine/Content/textures/SF Mono/SF mono.fnt", GraphicsDevice);
            imGuiTx = _imGuiRenderer.BindTexture(player.Texture);
            // TODO: use this.Content to load your game content here
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            player.Update();
            Camera.Update();
            currentProject.Update(GraphicsDevice);
            // TODO: Add your update logic here
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            currentProject.Render(_spriteBatch);
            player.Draw(_spriteBatch);
            _spriteBatch.DrawString(font, text, new Vector2(0, 0), Color.White);

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);

            //ScreenWidth / 320
            _spriteBatch.Draw(renderTarget, new Vector2(0, 0), new Rectangle(0, 0, renderTarget.Width, renderTarget.Height), Color.White, 0, new Vector2(0, 0), 3, SpriteEffects.None, 1);

            _spriteBatch.End();

            // Call BeforeLayout first to set things up
            _imGuiRenderer.BeforeLayout(gameTime);

            // Draw our UI
            ImGui.Begin("Debug");
            ImGui.Text("Player position:" + player.Position.ToString());
            ImGui.Text("Camera offset:" + Camera.offset.ToString());
            ImGui.Text("Current level rectangle: " + currentProject.currentLevelRectangle);
            ImGui.InputText("Text", ref text, 50);
            ImGui.End();

            // Call AfterLayout now to finish up and draw all the things
            _imGuiRenderer.AfterLayout();

            base.Draw(gameTime);
        }
    }
}
