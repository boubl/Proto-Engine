using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Proto_Engine.Scene;

namespace Proto_Engine.ECS
{
    public class Player : Entity
    {
        public Player(MonoGame_LDtk_Importer.Entity entity, GraphicsDevice graphicsDevice) : base(entity, graphicsDevice)
        {
        }

        public Player(Rectangle rectangle, GraphicsDevice graphicsDevice) : base(rectangle, graphicsDevice)
        {
        }

        public Player(Vector2 position, Rectangle rectangle, GraphicsDevice graphicsDevice) : base(position, rectangle, graphicsDevice)
        {
        }

        public override void Update()
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Up) && ks.IsKeyUp(Keys.Down))
            {
                Position = new Vector2(Position.X, Position.Y - 2);
            }
            if (ks.IsKeyDown(Keys.Down) && ks.IsKeyUp(Keys.Up))
            {
                Position = new Vector2(Position.X, Position.Y + 2);
            }
            if (ks.IsKeyDown(Keys.Right) && ks.IsKeyUp(Keys.Left))
            {
                Position = new Vector2(Position.X + 2, Position.Y);
            }
            if (ks.IsKeyDown(Keys.Left) && ks.IsKeyUp(Keys.Right))
            {
                Position = new Vector2(Position.X - 2, Position.Y);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position - Camera.offset, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, bool drawTile)
        {
            if (drawTile)
            {
                //spriteBatch.Draw(DataManager.tilesets[TileTsUid.Value], Position, TileSourceRect, Color.White);
            }
        }
    }
}
