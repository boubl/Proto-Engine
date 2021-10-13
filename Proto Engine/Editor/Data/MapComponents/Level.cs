using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Proto_Engine.Editor.Data.MapComponents.Instances;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proto_Engine.Editor.Data.MapComponents
{
    public class Level : IDisposable
    {
        private bool disposedValue;

        public Level(Vector2 position, int width, int height, string name = "Default")
        {
            Name = name;
            Width = width;
            Height = height;
            BgLayer = new Layer(Width, Height);
            FgLayer = new Layer(Width, Height);
            Entities = new List<Entity>();
            Position = position;
        }
        public string Name { get; set; }
        public Layer BgLayer { get; set; }
        public Layer FgLayer { get; set; }
        public List<Entity> Entities { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle { get => new Rectangle((int)Position.X, (int)Position.Y, Width, Height); }
        public Rectangle RectanglePx { get => new Rectangle(
            (int)Position.X * EditMode.TileSize,
            (int)Position.Y * EditMode.TileSize,
            Width * EditMode.TileSize,
            Height * EditMode.TileSize); }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    BgLayer.Dispose();
                    FgLayer.Dispose();
                    foreach (Entity entity in Entities)
                    {
                        entity.Dispose();
                    }
                    Entities = null;
                    Width = 0;
                    Height = 0;
                    Position = Vector2.Zero;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Level()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
