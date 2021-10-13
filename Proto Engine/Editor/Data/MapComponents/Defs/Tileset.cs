using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Proto_Engine.Editor.Data.MapComponents.Defs
{
    public class Tileset : IDisposable
    {
        private bool disposedValue;
        #region Private fields

        #endregion

        #region Public Properties
        public bool IsTextureLoaded { get; set; }
        public int Id {  get; set; }
        public string Name { get; set; }
        public Texture2D Texture { get; set; }
        public string TextureOrigin { get; set; }
        public IntPtr TexturePtr { get; set; }
        #endregion
        
        public Tileset(int id)
        {
            Id = id;
            Name = "New Tileset";
            TextureOrigin = "none";
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Texture != null)
                        Texture.Dispose();
                    if (ProtoEngine.ImGuiRenderer.IsTextureBinded(TexturePtr))
                        ProtoEngine.ImGuiRenderer.UnbindTexture(TexturePtr);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Tileset()
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
