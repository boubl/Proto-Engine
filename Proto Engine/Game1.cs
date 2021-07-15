using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_LDtk_Importer;
using Proto_Engine.ECS;
using Proto_Engine.Scene;

namespace Proto_Engine
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static int ScreenWidth = 1920 / 2;
        public static int ScreenHeight = 1080 / 2;

        RenderTarget2D renderTarget;

        ProjectTilesRenderer projectTilesRenderer;
        Camera mainCamera;
        Player player;

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
                            1920 / 6,
                            1080 / 6,
                            false,
                            GraphicsDevice.PresentationParameters.BackBufferFormat,
                            DepthFormat.Depth24);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            DataManager.LoadProjects();
            //DataManager.LoadTilesets(GraphicsDevice);
            player = new Player(new Rectangle(0, 1080, 1, 1));
            projectTilesRenderer = new ProjectTilesRenderer("Test_file_for_API_showing_all_features", GraphicsDevice);
            mainCamera = new Camera(player);
            // TODO: use this.Content to load your game content here
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            player.Update();
            mainCamera.Update();
            projectTilesRenderer.Update(GraphicsDevice);
            // TODO: Add your update logic here
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            projectTilesRenderer.Render(_spriteBatch);

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);

            //ScreenWidth / 320
            _spriteBatch.Draw(renderTarget, new Vector2(0, 0), new Rectangle(0, 0, renderTarget.Width, renderTarget.Height), Color.White, 0, new Vector2(0, 0), 3, SpriteEffects.None, 1);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
