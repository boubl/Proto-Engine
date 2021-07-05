using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_LDtk_Importer;
using Proto_Engine.Scene;

namespace Proto_Engine
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        LevelTilesRenderer levelTilesRenderer; 
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            DataManager.LoadProjects();
            DataManager.LoadTilesets(GraphicsDevice);
            levelTilesRenderer = new LevelTilesRenderer(DataManager.projects["Typical_TopDown_example"].Levels[0]);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            levelTilesRenderer.Render(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
