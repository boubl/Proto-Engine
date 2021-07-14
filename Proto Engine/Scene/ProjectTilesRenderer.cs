using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_LDtk_Importer;

namespace Proto_Engine.Scene
{
    public class ProjectTilesRenderer
    {
        List<LevelTilesRenderer> levels;
        public Dictionary<int, Texture2D> tilesets;

        public ProjectTilesRenderer(LDtkProject project, GraphicsDevice graphicsDevice)
        {
            levels = new List<LevelTilesRenderer>();
            foreach (Level level in project.Levels)
            {
                levels.Add(new LevelTilesRenderer(level, graphicsDevice));
            }

            tilesets = new Dictionary<int, Texture2D>();
            foreach (TilesetDef tileset in project.Definitions.Tilesets)
            {
                tilesets.Add(tileset.Uid, tileset.GetTilesetTexture(graphicsDevice));
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (LevelTilesRenderer level in levels)
            {
                level.Render(spriteBatch, tilesets);
            }
        }
    }
}
