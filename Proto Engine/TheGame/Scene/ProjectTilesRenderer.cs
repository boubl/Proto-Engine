using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using MonoGame_LDtk_Importer;
using Proto_Engine.Data;
using Proto_Engine.TheGame;
using Proto_Engine.Utils;

namespace Proto_Engine.Scene
{
    public class ProjectTilesRenderer
    {
        public Rectangle currentLevelRectangle { get; private set; }
        public List<int> intersectedLevels { get; private set; }
        public List<Rectangle> currentCollisions { get; private set; }
        public Dictionary<int, Texture2D> tilesets;


        private List<LevelTilesRenderer> levels;
        private int currentLevelId = 0;
        private string projectName;
        private LDtkProject currentProject;

        public ProjectTilesRenderer(string projectName, GraphicsDevice graphicsDevice)
        {
            levels = new List<LevelTilesRenderer>();
            currentProject = DataManager.Projects[projectName];
            foreach (Level level in currentProject.Levels)
            {
                levels.Add(new LevelTilesRenderer(level, Color.White, graphicsDevice));
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
            if (currentProject != DataManager.Projects[projectName])
            {
                currentProject = DataManager.Projects[projectName];
                levels = new List<LevelTilesRenderer>();
                foreach (Level level in currentProject.Levels)
                {
                    levels.Add(new LevelTilesRenderer(level, Color.White, graphicsDevice));
                }

                tilesets = new Dictionary<int, Texture2D>();
                foreach (TilesetDef tileset in currentProject.Definitions.Tilesets)
                {
                    tilesets.Add(tileset.Uid, tileset.GetTilesetTexture(graphicsDevice));
                }
            }
            currentLevelRectangle = GetCurrentLevelRectangle();
            currentCollisions = GetCurrentLevelCollisions();
        }

        public void Render(SpriteBatch spriteBatch)
        {
            levels[currentLevelId].Render(spriteBatch, tilesets);
#if DEBUG
            if (GameMode.viewCollisions)
            foreach (Rectangle rect in currentCollisions)
            {
                spriteBatch.DrawRectangle(new Rectangle(rect.Location - Camera.offset.ToPoint(), rect.Size), Color.Red, 1);
            }
#endif
        }

        public void RenderLights(SpriteBatch spriteBatch)
        {
            foreach (Layer layer in currentProject.Levels[currentLevelId].LayerInstances)
            {
                if (layer.Identifier == "Lights")
                {
                    EntitieLayer entitieLayer = layer as EntitieLayer;
                    foreach (Entity entity in entitieLayer.GetEntitiesByIdentifier("Light"))
                    {
                        Vector2 pos = entity.Coordinates - Camera.offset - (new Vector2(entity.Width / 2, entity.Height / 2));
                        spriteBatch.Draw(LightCreator.Light, new Rectangle(pos.ToPoint(), new Point(entity.Width, entity.Height)), Color.White);
                    }
                }
            }
        }

        private Rectangle GetCurrentLevelRectangle()
        {
            intersectedLevels = new List<int>();
            int i = 0;
            int possibility = currentLevelId;
            Rectangle newRectangle = new Rectangle();
            Rectangle playerRectangle = new Rectangle(GameMode.player.Position.ToPoint(), GameMode.player.BaseRectangle.Size);
            int counter = 0;
            foreach (LevelTilesRenderer level in levels)
            {
                if (playerRectangle.Intersects(level.Rectangle))
                {
                    i++;
                    intersectedLevels.Add(counter);
                    newRectangle = level.Rectangle;
                    possibility = counter;
                }
                counter++;
            }

            if (i != 1)
            {
                intersectedLevels.Add(currentLevelId);
                return currentLevelRectangle;
            }
            else
            {
                currentLevelId = possibility;
                return newRectangle;
            }
        }

        private List<Rectangle> GetCurrentLevelCollisions()
        {
            List<Rectangle> result = new List<Rectangle>();
            foreach (int levelId in intersectedLevels)
            {
                foreach (Layer layer in currentProject.Levels[levelId].LayerInstances)
                {
                    if (layer.Identifier == "Foreground_IntGrid")
                    {
                        IntGridLayer intGridLayer = layer as IntGridLayer;
                        foreach (Point point in intGridLayer.GetPointsByValue(1))
                        {
                            result.Add(new Rectangle(
                                point.X * intGridLayer.GridSize + levels[levelId].Rectangle.Location.X,
                                point.Y * intGridLayer.GridSize + levels[levelId].Rectangle.Location.Y,
                                intGridLayer.GridSize,
                                intGridLayer.GridSize));
                        }
                    }
                }
            }
            return result;
        }
    }
}
