using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Proto_Engine.Entities
{
    public abstract class Entity
    {
        public Vector2 Position;
        public Rectangle BaseRectangle;

        public Entity(MonoGame_LDtk_Importer.Entity entity)
        {
            Position = entity.Coordinates;
            BaseRectangle = new Rectangle(0, 0, entity.Width, entity.Height);
        }

        public Entity(Rectangle rectangle)
        {
            Position = rectangle.Location.ToVector2();
            BaseRectangle = new Rectangle(0, 0, rectangle.Width, rectangle.Height);
        }

        public Entity(Vector2 position, Rectangle rectangle)
        {
            this.Position = position;
            BaseRectangle = rectangle;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
