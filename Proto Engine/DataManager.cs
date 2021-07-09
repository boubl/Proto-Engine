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
            projects.Add("AutoLayers_1_basic", new LDtkProject("/Applications/LDtk.app/Contents/samples/AutoLayers_1_basic.ldtk"));
            projects.Add("AutoLayers_2_stamps", new LDtkProject("/Applications/LDtk.app/Contents/samples/AutoLayers_2_stamps.ldtk"));
            projects.Add("AutoLayers_3_Mosaic", new LDtkProject("/Applications/LDtk.app/Contents/samples/AutoLayers_3_Mosaic.ldtk"));
            projects.Add("AutoLayers_4_Advanced", new LDtkProject("/Applications/LDtk.app/Contents/samples/AutoLayers_4_Advanced.ldtk"));
            projects.Add("AutoLayers_5_OptionalRules", new LDtkProject("/Applications/LDtk.app/Contents/samples/AutoLayers_5_OptionalRules.ldtk"));
            projects.Add("Entities", new LDtkProject("/Applications/LDtk.app/Contents/samples/Entities.ldtk"));
            projects.Add("SeparateLevelFiles", new LDtkProject("/Applications/LDtk.app/Contents/samples/SeparateLevelFiles.ldtk"));
            projects.Add("Test_file_for_API_showing_all_features", new LDtkProject("/Applications/LDtk.app/Contents/samples/Test_file_for_API_showing_all_features.ldtk"));
            projects.Add("Typical_2D_platformer_example", new LDtkProject("/Applications/LDtk.app/Contents/samples/Typical_2D_platformer_example.ldtk"));
            projects.Add("Typical_TopDown_example", new LDtkProject("/Applications/LDtk.app/Contents/samples/Typical_TopDown_example.ldtk"));
            projects.Add("WorldMap_Free_layout", new LDtkProject("/Applications/LDtk.app/Contents/samples/WorldMap_Free_layout.ldtk"));
            projects.Add("WorldMap_GridVania_layout", new LDtkProject("/Applications/LDtk.app/Contents/samples/WorldMap_GridVania_layout.ldtk"));
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
