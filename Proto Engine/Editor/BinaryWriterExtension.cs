using Microsoft.Xna.Framework;
using Proto_Engine.Editor.Data.MapComponents;
using Proto_Engine.Editor.Data.MapComponents.Defs;
using Proto_Engine.Editor.Data.MapComponents.Instances;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Proto_Engine.Editor
{
    public static class BinaryWriterExtension
    {
        public static void Write(this BinaryWriter writer, Level level)
        {
            writer.Write(level.Width);
            writer.Write(level.Height);

            writer.Write(level.Position.X);
            writer.Write(level.Position.Y);

            writer.Write(level.BgLayer);
            writer.Write(level.FgLayer);
            // TODO: Save entities
        }
        public static void Write(this BinaryWriter writer, Layer layer)
        {
            writer.Write(layer.Width);
            writer.Write(layer.Height);


            writer.Write(layer.Grid.Count);
            foreach (int i in layer.Grid)
            {
                writer.Write(i);
            }
            writer.Write(layer.Tilesets.Count);
            foreach (int i in layer.Tilesets)
            {
                writer.Write(i);
            }
            writer.Write(layer.Randomizer.Count);
            foreach (int i in layer.Randomizer)
            {
                writer.Write(i);
            }
        }
        public static void Write(this BinaryWriter writer, Definitions definitions)
        {
            writer.Write(definitions.Tilesets.Count);
            foreach (Tileset tileset in definitions.Tilesets)
            {
                writer.Write(tileset);
            }
        }
        public static void Write(this BinaryWriter writer, Tileset tileset)
        {
            writer.Write(tileset.Id);
            writer.Write(tileset.Name);
            writer.Write(tileset.TextureOrigin);
            writer.Write(tileset.Texture.Width);
            writer.Write(tileset.Texture.Height);

            int[] textureData = new int[tileset.Texture.Width * tileset.Texture.Height];
            tileset.Texture.GetData(textureData);
            // the length of textureData can be found with the Width and the Height
            foreach (int i in textureData)
            {
                writer.Write(i);
            }
        }
    }
}
