using Microsoft.Xna.Framework;
using Proto_Engine.Editor.Data.MapComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Proto_Engine.Editor.Data
{
    public class MapFile :IDisposable
    {
        private bool disposedValue;

        public MapFile()
        {
            Levels = new List<Level>();
            Definitions = new Definitions();
            FilePath = null;

            Level level = new Level(Vector2.Zero, 100, 100);
            level.BgLayer.Grid[815] = 1;
            level.Position = Vector2.Zero;
            Levels.Add(level);
        }

        public List<Level> Levels { get; set; }
        public Definitions Definitions { get; set; }
        public string FilePath { get; set; }

        public void Save()
        {
            if (FilePath == null || !File.Exists(FilePath))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.InitialDirectory = Path.GetFullPath(EditMode.MapFolder);
                dialog.Filter = "Amunero Map File (*.amn)|*.amn";
                dialog.Title = "Save Map";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    FilePath = dialog.FileName;
                }
                else return;
                dialog.Dispose();
            }

            FileStream stream = new FileStream(FilePath, FileMode.OpenOrCreate);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(Levels.Count);
            foreach(Level level in Levels)
            {
                writer.Write(level);
            }
            writer.Write(Definitions);
            writer.Dispose();
            writer.Close();
            stream.Dispose();
            stream.Close();
        }

        public static MapFile LoadFile()
        {
            MapFile mapFile = new MapFile();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Path.GetFullPath(EditMode.MapFolder);
            dialog.Filter = "Amunero Map File (*.amn)|*.amn|All files (*.*)|*.*";
            dialog.Title = "Open Map";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(dialog.FileName))
                {
                    FileStream stream = new FileStream(dialog.FileName, FileMode.OpenOrCreate);
                    BinaryReader reader = new BinaryReader(stream);
                    int levelCount = reader.ReadInt32();
                    mapFile.Levels = new List<Level>();
                    for (int i = 0; i < levelCount; i++)
                    {
                        mapFile.Levels.Add(reader.ReadLevel());
                    }
                    mapFile.Definitions = reader.ReadDefinitions();
                    reader.Dispose();
                    reader.Close();
                    stream.Dispose();
                    stream.Close();
                }
            }
            dialog.Dispose();
            return mapFile;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    foreach (Level level in Levels)
                    {
                        level.Dispose();
                    }
                    Levels = null;
                    Definitions.Dispose();
                }
                FilePath = null;
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }
        
        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~MapFile()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
