using Microsoft.Xna.Framework.Graphics;
using MonoGame_LDtk_Importer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proto_Engine
{
    public class DataManager
    {
        public static Dictionary<int, Texture2D> tilesets;
        public static Dictionary<string, LDtkProject> projects;

        public static void LoadProjects()
        {
            projects = new Dictionary<string, LDtkProject>();
            projects.Add("Typical_TopDown_example", new LDtkProject("/Users/AlexisNicolas/Documents/GitHub/Proto-Engine/Proto Engine/Content/Typical_TopDown_example.ldtk"));
        }

        public static void LoadTilesets(GraphicsDevice graphicsDevice)
        {
            tilesets = new Dictionary<int, Texture2D>();
            foreach (LDtkProject project in projects.Values)
            {
                foreach (TilesetDef tileset in project.Definitions.Tilesets)
                {
                    tilesets.Add(tileset.Uid, tileset.GetTilesetTexture(graphicsDevice));
                }
            }
        }
    }
}
