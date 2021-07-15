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
        private string projectName;
        LDtkProject currentProject;

        public ProjectTilesRenderer(string projectName, GraphicsDevice graphicsDevice)
        {
            levels = new List<LevelTilesRenderer>();
            currentProject = DataManager.projects[projectName];
            foreach (Level level in currentProject.Levels)
            {
                levels.Add(new LevelTilesRenderer(level, graphicsDevice));
            }

            tilesets = new Dictionary<int, Texture2D>();
            foreach (TilesetDef tileset in currentProject.Definitions.Tilesets)
            {
                tilesets.Add(tileset.Uid, tileset.GetTilesetTexture(graphicsDevice));
            }

            this.projectName = projectName;
        }

        public void Update(GraphicsDevice graphicsDevice)
        {
            if (currentProject != DataManager.projects[projectName])
            {
                currentProject = DataManager.projects[projectName];
                levels = new List<LevelTilesRenderer>();
                foreach (Level level in currentProject.Levels)
                {
                    levels.Add(new LevelTilesRenderer(level, graphicsDevice));
                }

                tilesets = new Dictionary<int, Texture2D>();
                foreach (TilesetDef tileset in currentProject.Definitions.Tilesets)
                {
                    tilesets.Add(tileset.Uid, tileset.GetTilesetTexture(graphicsDevice));
                }
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
