using Microsoft.Xna.Framework.Graphics;
using MonoGame_LDtk_Importer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Proto_Engine
{
    public static class DataManager
    {
        //public static Dictionary<int, Texture2D> tilesets;
        public static Dictionary<string, LDtkProject> projects;

        private static List<string> projectList;
        private static Dictionary<string, DateTime> lastWrite;
        private static FileSystemWatcher watcher;

        public static void LoadProjects()
        {
            projects = new Dictionary<string, LDtkProject>();
            projectList = new List<string>();

            projectList.Add("/Users/AlexisNicolas/Documents/GitHub/LDtk/LDtk.app/Contents/samples/AutoLayers_1_basic.ldtk");
            projectList.Add("/Users/AlexisNicolas/Documents/GitHub/LDtk/LDtk.app/Contents/samples/AutoLayers_2_stamps.ldtk");
            projectList.Add("/Users/AlexisNicolas/Documents/GitHub/LDtk/LDtk.app/Contents/samples/AutoLayers_3_Mosaic.ldtk");
            projectList.Add("/Users/AlexisNicolas/Documents/GitHub/LDtk/LDtk.app/Contents/samples/AutoLayers_4_Advanced.ldtk");
            projectList.Add("/Users/AlexisNicolas/Documents/GitHub/LDtk/LDtk.app/Contents/samples/AutoLayers_5_OptionalRules.ldtk");
            projectList.Add("/Users/AlexisNicolas/Documents/GitHub/LDtk/LDtk.app/Contents/samples/Entities.ldtk");
            projectList.Add("/Users/AlexisNicolas/Documents/GitHub/LDtk/LDtk.app/Contents/samples/SeparateLevelFiles.ldtk");
            projectList.Add("/Users/AlexisNicolas/Documents/GitHub/LDtk/LDtk.app/Contents/samples/Test_file_for_API_showing_all_features.ldtk");
            projectList.Add("/Users/AlexisNicolas/Documents/GitHub/LDtk/LDtk.app/Contents/samples/Typical_2D_platformer_example.ldtk");
            projectList.Add("/Users/AlexisNicolas/Documents/GitHub/LDtk/LDtk.app/Contents/samples/Typical_TopDown_example.ldtk");
            projectList.Add("/Users/AlexisNicolas/Documents/GitHub/LDtk/LDtk.app/Contents/samples/WorldMap_Free_layout.ldtk");
            projectList.Add("/Users/AlexisNicolas/Documents/GitHub/LDtk/LDtk.app/Contents/samples/WorldMap_GridVania_layout.ldtk");

            foreach(string path in projectList)
            {
                projects.Add(Path.GetFileNameWithoutExtension(path), new LDtkProject(path));
            }

            watcher = new FileSystemWatcher("/Users/AlexisNicolas/Documents/GitHub/LDtk/LDtk.app/Contents/samples/");

            watcher.NotifyFilter = NotifyFilters.LastWrite;

            watcher.Changed += OnChanged;

            watcher.Filter = "*.ldtk";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            if (projects.ContainsKey(Path.GetFileNameWithoutExtension(e.FullPath)))
            {
                projects[Path.GetFileNameWithoutExtension(e.FullPath)] = new LDtkProject(e.FullPath);
            }

        }

    }
}
