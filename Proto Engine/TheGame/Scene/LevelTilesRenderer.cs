using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame_LDtk_Importer;
using System;
using System.Collections.Generic;
using System.Text;
using Proto_Engine.Utils;

namespace Proto_Engine.Scene
{
    public class LevelTilesRenderer
    {
        public Rectangle Rectangle;

        List<Layer> layers;
        Vector2 coordinates;
        Texture2D bg;
        Vector2 bgScale;
        Vector2 bgCoordinates;

        public LevelTilesRenderer(Level level, Color backgroundColor, GraphicsDevice graphicsDevice)
        {
            List<LayerType> types = new List<LayerType>();
            types.Add(LayerType.AutoLayer);
            types.Add(LayerType.Tiles);
            types.Add(LayerType.IntGrid);
            layers = level.GetLayersByType(types);
            layers.Reverse();

            coordinates = level.WorldCoordinates;

            bg = level.GetCroppedBackground(graphicsDevice);
            if (bg != null)
            {
                if (level.BackgroundPosition != null)
                {
                    bgScale = level.BackgroundPosition.Scale;
                    bgCoordinates = level.BackgroundPosition.Coordinates;
                }
                else
                {
                    bgScale = Vector2.One;
                    bgCoordinates = Vector2.Zero;
                }
            }
            else
            {
                bg = new Texture2D(graphicsDevice, level.Width, level.Height);
                Color[] data = new Color[level.Width * level.Height];
                for (int i = 0; i < data.Length; ++i) data[i] = level.BackgroundColor;
                bg.SetData(data);
                bgScale = Vector2.One;
            }

            

            Rectangle = new Rectangle((int)coordinates.X, (int)coordinates.Y, level.Width, level.Height);
        }

        public void Render(SpriteBatch spriteBatch, Dictionary<int, Texture2D> tilesets)
        {
            spriteBatch.Draw(bg, new Rectangle((int)(coordinates.X - Camera.offset.X + bgCoordinates.X), (int)(coordinates.Y - Camera.offset.Y + bgCoordinates.Y), (int)(bg.Width * bgScale.X), (int)(bg.Height * bgScale.Y)), Color.White);
            
            foreach (Layer layer in layers)
            {
                if (layer.Type == LayerType.AutoLayer)
                {
                    AutoLayer auto = layer as AutoLayer;
                    foreach (Tile tile in auto.AutoLayerTiles)
                    {
                        Rectangle sourceRectangle = new Rectangle((int)tile.Source.X, (int)tile.Source.Y, auto.GridSize, auto.GridSize);
                        Rectangle destinationRectangle = new Rectangle(
                            (int)(tile.Coordinates.X - Camera.offset.X + coordinates.X + auto.TotalOffset.X),
                            (int)(tile.Coordinates.Y - Camera.offset.Y + coordinates.Y + auto.TotalOffset.Y),
                            auto.GridSize, auto.GridSize);
                        SpriteEffects spriteEffects = SpriteEffects.None;
                        float rotation = 0;
                        Vector2 origin = new Vector2(0, 0);
                        if (tile.IsFlippedOnX && tile.IsFlippedOnY)
                        {
                            rotation = (float)Math.PI;
                            origin = new Vector2(auto.GridSize, auto.GridSize);
                        }
                        else if (tile.IsFlippedOnY)
                        {
                            spriteEffects = SpriteEffects.FlipVertically;
                        }
                        else if (tile.IsFlippedOnX)
                        {
                            spriteEffects = SpriteEffects.FlipHorizontally;
                        }
                        spriteBatch.Draw(tilesets[auto.TilesetDefUid.Value], destinationRectangle, sourceRectangle, Color.White, rotation, origin, spriteEffects, 1);
                    }
                }
                if (layer.Type == LayerType.IntGrid)
                {
                    IntGridLayer intGridLayer = layer as IntGridLayer;
                    foreach (Tile tile in intGridLayer.AutoLayerTiles)
                    {
                        Rectangle sourceRectangle = new Rectangle((int)tile.Source.X, (int)tile.Source.Y, intGridLayer.GridSize, intGridLayer.GridSize);
                        Rectangle destinationRectangle = new Rectangle(
                            (int)(tile.Coordinates.X - Camera.offset.X + coordinates.X + intGridLayer.TotalOffset.X),
                            (int)(tile.Coordinates.Y - Camera.offset.Y + coordinates.Y + intGridLayer.TotalOffset.Y),
                            intGridLayer.GridSize, intGridLayer.GridSize);

                        SpriteEffects spriteEffects = SpriteEffects.None;
                        float rotation = 0;
                        Vector2 origin = new Vector2(0, 0);
                        if (tile.IsFlippedOnX && tile.IsFlippedOnY)
                        {
                            rotation = (float)Math.PI;
                            origin = new Vector2(intGridLayer.GridSize, intGridLayer.GridSize);
                        }
                        else if (tile.IsFlippedOnY)
                        {
                            spriteEffects = SpriteEffects.FlipVertically;
                        }
                        else if (tile.IsFlippedOnX)
                        {
                            spriteEffects = SpriteEffects.FlipHorizontally;
                        }
                        spriteBatch.Draw(tilesets[intGridLayer.TilesetDefUid.Value], destinationRectangle, sourceRectangle, Color.White, rotation, origin, spriteEffects, 1);
                    }
                }
                if (layer.Type == LayerType.Tiles)
                {
                    TileLayer tileLayer = layer as TileLayer;
                    foreach (Tile tile in tileLayer.GridTilesInstances)
                    {
                        Rectangle sourceRectangle = new Rectangle((int)tile.Source.X, (int)tile.Source.Y, tileLayer.GridSize, tileLayer.GridSize);
                        Rectangle destinationRectangle = new Rectangle(
                            (int)(tile.Coordinates.X - Camera.offset.X + coordinates.X + tileLayer.TotalOffset.X),
                            (int)(tile.Coordinates.Y - Camera.offset.Y + coordinates.Y + tileLayer.TotalOffset.Y),
                            tileLayer.GridSize, tileLayer.GridSize);

                        SpriteEffects spriteEffects = SpriteEffects.None;
                        float rotation = 0;
                        Vector2 origin = new Vector2(0, 0);
                        if (tile.IsFlippedOnX && tile.IsFlippedOnY)
                        {
                            rotation = (float)Math.PI;
                            origin = new Vector2(tileLayer.GridSize, tileLayer.GridSize);
                        }
                        else if (tile.IsFlippedOnY)
                        {
                            spriteEffects = SpriteEffects.FlipVertically;
                        }
                        else if (tile.IsFlippedOnX)
                        {
                            spriteEffects = SpriteEffects.FlipHorizontally;
                        }
                        spriteBatch.Draw(tilesets[tileLayer.TilesetDefUid.Value], destinationRectangle, sourceRectangle, Color.White, rotation, origin, spriteEffects, 1);
                    }
                }
            }
        }
    }
}
