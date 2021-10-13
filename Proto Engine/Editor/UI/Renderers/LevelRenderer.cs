using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Proto_Engine.Editor.Data.MapComponents;
using Proto_Engine.Editor.Data.MapComponents.Instances;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proto_Engine.Editor.UI.Renderers
{
    public class LevelRenderer
    {
        LayerRenderer layerRenderer = new LayerRenderer();
        public LevelRenderer() { }
        public void Render(SpriteBatch spriteBatch, Level level, Vector2 position, float zoom)
        {
            layerRenderer.Render(spriteBatch, level.BgLayer, position, zoom);
            foreach (Entity entity in level.Entities)
            {
                entity.Render(spriteBatch);
            }
        }
    }
}
