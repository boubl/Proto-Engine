using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Proto_Engine.Editor.Data.MapComponents;
using Proto_Engine.Editor.Data.MapComponents.Defs;
using Proto_Engine.Editor.Data.MapComponents.Instances;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Proto_Engine.Editor
{
    public static class BinaryReaderExtension
    {
        public static Level ReadLevel(this BinaryReader reader)
        {
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();

            float posX = reader.ReadSingle();
            float posY = reader.ReadSingle();

            Level level = new Level(new Vector2(posX, posY), width, height);

            level.BgLayer = reader.ReadLayer();
            level.FgLayer = reader.ReadLayer();

            // TODO: Load Entities
            
            return level;
        }
        public static Layer ReadLayer(this BinaryReader reader)
        {
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();

            Layer layer = new Layer(width, height);

            layer.Grid = new List<int>();
            int gridCount = reader.ReadInt32();
            for (int i = 0; i < gridCount; i++)
            {
                layer.Grid.Add(reader.ReadInt32());
            }
            layer.Tilesets = new List<int>();
            int tilesetsCount = reader.ReadInt32();
            for (int i = 0; i < tilesetsCount; i++)
            {
                layer.Tilesets.Add(reader.ReadInt32());
            }
            layer.Randomizer = new List<int>();
            int randomizerCount = reader.ReadInt32();
            for (int i = 0; i < randomizerCount; i++)
            {
                layer.Randomizer.Add(reader.ReadInt32());
            }
            return layer;
        }
        public static Definitions ReadDefinitions(this BinaryReader reader)
        {
            Definitions definition = new Definitions();
            int tilesetsCount = reader.ReadInt32();
            for (int i = 0; i < tilesetsCount; i++)
            {
                definition.Tilesets.Add(reader.ReadTileset());
            }
            return definition;
        }
        public static Tileset ReadTileset(this BinaryReader reader)
        {
            Tileset tileset = new Tileset(reader.ReadInt32());
            tileset.Name = reader.ReadString();
            tileset.TextureOrigin = reader.ReadString();
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();

            int[] textureData = new int[width * height];
            for (int i = 0; i < width * height; i++)
            {
                textureData[i] = reader.ReadInt32();
            }
            tileset.Texture = new Texture2D(ProtoEngine.GraphicsDeviceOMG, width, height);
            tileset.Texture.SetData(textureData);
            tileset.IsTextureLoaded = true;
            tileset.TexturePtr = ProtoEngine.ImGuiRenderer.BindTexture(tileset.Texture);

            return tileset;
        }
    }
}
