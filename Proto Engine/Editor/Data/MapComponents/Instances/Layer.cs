using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proto_Engine.Editor.Data.MapComponents.Instances
{
    public class Layer : IDisposable
    {
        private bool disposedValue;

        public Layer(int width, int height)
        {
            Grid = new List<int>();
            for (int i = 0; i < width * height; i++ ) Grid.Add(0);
            Tilesets = new List<int>(width * height);
            for (int i = 0; i < width * height; i++) Tilesets.Add(0);
            Randomizer = new List<int>(width * height);
            for (int i = 0; i < width * height; i++) Randomizer.Add(0);
            Width = width;
            Height = height;
        }
        public List<int> Grid { get; set; }
        public List<int> Tilesets { get; set; }
        public List<int> Randomizer { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Point Size { get => new Point(Width, Height) ; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    Grid = null;
                    Tilesets = null;
                    Randomizer = null;
                    Width = 0;
                    Height = 0;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Layer()
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
