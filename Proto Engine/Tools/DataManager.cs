using Microsoft.Xna.Framework.Graphics;
using MonoGame_LDtk_Importer;
using Proto_Engine.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Proto_Engine
{
    public static class DataManager
    {
        public static string ContentFolder = "Content/";

        //public static Dictionary<int, Texture2D> tilesets;


        //Projects
        public static Dictionary<string, LDtkProject> Projects { get; private set; }
        private static List<string> _projectList;
        private static FileSystemWatcher _watcher;
        public static void LoadProjects()
        {
            Projects = new Dictionary<string, LDtkProject>();
            _projectList = new List<string>();

            _projectList.Add(ContentFolder + "maps/platformer.ldtk");

            foreach(string path in _projectList)
            {
                Projects.Add(Path.GetFileNameWithoutExtension(path), new LDtkProject(path));
            }

            _watcher = new FileSystemWatcher(ContentFolder);

            _watcher.NotifyFilter = NotifyFilters.LastWrite;

            _watcher.Changed += OnChanged;

            _watcher.Filter = "*.ldtk";
            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;
        }
        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            if (Projects.ContainsKey(Path.GetFileNameWithoutExtension(e.FullPath)))
            {
                Projects[Path.GetFileNameWithoutExtension(e.FullPath)] = new LDtkProject(e.FullPath);
            }

        }

        //Textures
        public static Dictionary<string, Texture2D> Textures { get; private set; }
        public static void LoadTextures(GraphicsDevice graphicsDevice)
        {
            Textures = new Dictionary<string, Texture2D>();
            Textures.Add("noel character", Texture2D.FromFile(graphicsDevice, ContentFolder + "sprites/spriteSheets/noel character.png"));
        }

        //Aseprite Animations
        public static Dictionary<string, AsepriteFile> Animations { get; private set; }
        public static void LoadAnimations()
        {
            Animations = new Dictionary<string, AsepriteFile>();
            Animations.Add("noel character", new AsepriteFile(ContentFolder + "sprites/spriteSheets/noel character.json"));
        }

        //Effects
        public static Dictionary<string, Effect> Effects { get; private set; }
        public static void LoadEffects(GraphicsDevice graphicsDevice)
        {
            Effects = new Dictionary<string, Effect>();
            foreach (string path in Directory.GetFiles(ContentFolder + "/effects/", "*.xnb"))
            {
                Effects.Add(Path.GetFileNameWithoutExtension(path), new Effect(graphicsDevice, File.ReadAllBytes(path)));
            }
        }
    }
}
