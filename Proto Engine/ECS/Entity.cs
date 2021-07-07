using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Proto_Engine.ECS
{
    public abstract class Entity
    {
        public Vector2 Position { get; set; }
        public Rectangle BaseRectangle { get; set; }

        private protected int? TileTsUid;
        private protected Rectangle? TileSourceRect;

        public Entity(MonoGame_LDtk_Importer.Entity entity)
        {
            Position = entity.Coordinates;
            BaseRectangle = new Rectangle(0, 0, entity.Width, entity.Height);

            if (entity.Tile != null)
            {
                TileTsUid = entity.Tile.TilesetUid;
                TileSourceRect = entity.Tile.SourceRectangle;
            }
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

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
