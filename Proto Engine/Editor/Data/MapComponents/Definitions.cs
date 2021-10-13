using Proto_Engine.Editor.Data.MapComponents.Defs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proto_Engine.Editor.Data.MapComponents
{
    public class Definitions : IDisposable
    {
        private bool disposedValue;

        public List<Tileset> Tilesets { get; set; }

        public Definitions()
        {
            Tilesets = new List<Tileset>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    foreach(Tileset tileset in Tilesets)
                    {
                        tileset.Dispose();
                    }
                    Tilesets = null;
                }
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Definitions()
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
