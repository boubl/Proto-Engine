using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Proto_Engine.ECS
{
    public abstract class Entity
    {
        public Vector2 Position;
        public Rectangle BaseRectangle;
        public Texture2D Texture;

        private protected int? TileTsUid;
        private protected Rectangle? TileSourceRect;

        public Entity(MonoGame_LDtk_Importer.Entity entity, GraphicsDevice graphicsDevice)
        {
            Position = entity.Coordinates;
            BaseRectangle = new Rectangle(0, 0, entity.Width, entity.Height);

            if (entity.Tile != null)
            {
                TileTsUid = entity.Tile.TilesetUid;
                TileSourceRect = entity.Tile.SourceRectangle;
            }

            Texture = new Texture2D(graphicsDevice, entity.Width, entity.Height);
            Color[] data = new Color[entity.Width * entity.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Red;
            Texture.SetData(data);
        }

        public Entity(Rectangle rectangle, GraphicsDevice graphicsDevice)
        {
            Position = rectangle.Location.ToVector2();
            BaseRectangle = new Rectangle(0, 0, rectangle.Width, rectangle.Height);

            Texture = new Texture2D(graphicsDevice, rectangle.Width, rectangle.Height);
            Color[] data = new Color[rectangle.Width * rectangle.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Red;
            Texture.SetData(data);
        }

        public Entity(Vector2 position, Rectangle rectangle, GraphicsDevice graphicsDevice)
        {
            this.Position = position;
            BaseRectangle = rectangle;

            Texture = new Texture2D(graphicsDevice, rectangle.Width, rectangle.Height);
            Color[] data = new Color[rectangle.Width * rectangle.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Red;
            Texture.SetData(data);
        }

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
