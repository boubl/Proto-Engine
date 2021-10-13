using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ImGuiNET;
using Proto_Engine.TheGame;
using Proto_Engine.Editor;
using Proto_Engine.Utils;
using System;
using System.Windows;

namespace Proto_Engine
{
    public class ProtoEngine : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private float _avgFps = 0;

        public static EngineMode Mode = EngineMode.Game;
        public static int ScreenWidth = 960;
        public static int ScreenHeight = 540;
        public static bool ExitGame = false;
        public static GraphicsDevice GraphicsDeviceOMG;

        // ImGui stuff
        public static ImGuiRenderer ImGuiRenderer;
        public static ImFontPtr font;

        public GameMode theGame = new GameMode();
        public EditMode editor = new EditMode();
        public ProtoEngine()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //InactiveSleepTime = new TimeSpan(0);
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ScreenWidth = 1920;
            ScreenHeight = 1009;
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            ImGuiRenderer = new ImGuiRenderer(this);
            font = ImGui.GetIO().Fonts.AddFontFromFileTTF("Content/fonts/roboto.ttf", 16);
            ImGuiRenderer.RebuildFontAtlas();

            editor.Init(GraphicsDevice);
            theGame.Init(GraphicsDevice);
            GraphicsDeviceOMG = GraphicsDevice;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            editor.LoadContent(GraphicsDevice);
            theGame.LoadContent(GraphicsDevice);
            IconsTools.Init();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float fps = 0;
            if (gameTime.ElapsedGameTime.TotalMilliseconds > 0)
                fps = (float)Math.Round(1000 / (gameTime.ElapsedGameTime.TotalMilliseconds), 1);

            //Set _avgFPS to the first fps value when started.
            if (_avgFps < 0.01f) _avgFps = fps;

            //Average over 20 frames
            _avgFps = _avgFps * 0.95f + fps * 0.05f;

            Window.Title = "Proto Engine v0.2.1 | FPS: " + _avgFps.ToString();

            if (ExitGame) Exit();
            // TODO: Add your update logic here

            ScreenWidth = Window.ClientBounds.Width;
            ScreenHeight = Window.ClientBounds.Height;

            if (Mode == EngineMode.Game)
                theGame.Update(gameTime, GraphicsDevice);
            else
                editor.Update(gameTime, GraphicsDevice);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (Mode == EngineMode.Game)
                theGame.Draw(GraphicsDevice, _spriteBatch, gameTime);
            else
                editor.Draw(GraphicsDevice, _spriteBatch, gameTime);
            base.Draw(gameTime);
        }
    }

    public enum EngineMode
    {
        Editor,
        Game
    }
}
