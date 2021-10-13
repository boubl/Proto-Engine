using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Proto_Engine.Editor.Data.MapComponents.Defs;
using Proto_Engine.Editor.Data.MapComponents.Instances;
using Proto_Engine.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proto_Engine.Editor.UI.Renderers
{
    public class LayerRenderer
    {
        public void Render(SpriteBatch spriteBatch, Layer layer, Vector2 position, float zoom)
        {
            for (int y = 0; y < layer.Height; y++)
            {
                for (int x = 0; x < layer.Width; x++)
                {
                    if (LayersHelper.GetGridValueAt(layer.Grid, layer.Width, x, y, 1) == 1)
                    {
                        int tilesetValue = LayersHelper.GetGridValueAt(layer.Tilesets, layer.Width, x, y, 0);
                        if (EditMode.currentMap.Definitions.Tilesets.Count > tilesetValue)
                        {
                            DrawTile(
                                spriteBatch,
                                EditMode.currentMap.Definitions.Tilesets[tilesetValue],
                                LayersHelper.GetTileType(layer.Grid, layer.Width, x, y),
                                new Vector2(x * 8, y * 8),
                                position,
                                LayersHelper.GetGridValueAt(layer.Randomizer, layer.Width, x, y, 0),
                                zoom
                            );
                        }
                    }
                }
            }
        }

        private void DrawTile(SpriteBatch spriteBatch, Tileset tileset, int tileType, Vector2 positionGrid, Vector2 positionWorld, int randomizer, float zoom)
        {
            if (tileType == -1) return; //blank tile
            #region a lot of "if"
            int txX = 0;
            int txY = 0;
            if (tileType == 0)
            {
                while (randomizer > 2)
                {
                    randomizer -= 3;
                }
                txX = 5;
                txY = 12 + randomizer;
            }
            else if (tileType == 1)
            {
                while (randomizer > 11)
                {
                    randomizer -= 12;
                }
                txX = 5;
                txY = randomizer;
            }
            else if (tileType == 2)
            {
                txX = 4;
                txY = 7;
            }
            else if (tileType == 3)
            {
                txX = 4;
                txY = 6;
            }
            else if (tileType == 4)
            {
                txX = 4;
                txY = 5;
            }
            else if (tileType == 5)
            {
                txX = 4;
                txY = 4;
            }
            else if (tileType == 6)
            {
                txX = 4;
                txY = 14;
            }
            else if (tileType == 7)
            {
                txX = 4;
                txY = 13;
            }
            else if (tileType == 8)
            {
                txX = 4;
                txY = 0;
            }
            else if (tileType == 9)
            {
                txX = 4;
                txY = 2;
            }
            else if (tileType == 10)
            {
                txX = 4;
                txY = 3;
            }
            else if (tileType == 11)
            {
                txX = 4;
                txY = 1;
            }
            else if (tileType == 12)
            {
                txX = 4;
                txY = 12;
            }
            else if (tileType == 13)
            {
                while (randomizer > 3)
                {
                    randomizer -= 4;
                }
                txX = randomizer;
                txY = 3;
            }
            else if (tileType == 14)
            {
                while (randomizer > 3)
                {
                    randomizer -= 4;
                }
                txX = randomizer;
                txY = 2;
            }
            else if (tileType == 15)
            {
                while (randomizer > 3)
                {
                    randomizer -= 4;
                }
                txX = randomizer;
                txY = 0;
            }
            else if (tileType == 16)
            {
                while (randomizer > 3)
                {
                    randomizer -= 4;
                }
                txX = randomizer;
                txY = 1;
            }
            else if (tileType == 17)
            {
                while (randomizer > 3)
                {
                    randomizer -= 4;
                }
                txX = randomizer;
                txY = 4;
            }
            else if (tileType == 18)
            {
                while (randomizer > 3)
                {
                    randomizer -= 4;
                }
                txX = randomizer;
                txY = 5;
            }
            else if (tileType == 19)
            {
                while (randomizer > 3)
                {
                    randomizer -= 4;
                }
                txX = randomizer;
                txY = 6;
            }
            else if (tileType == 20)
            {
                while (randomizer > 3)
                {
                    randomizer -= 4;
                }
                txX = randomizer;
                txY = 7;
            }
            else if (tileType == 21)
            {
                while (randomizer > 3)
                {
                    randomizer -= 4;
                }
                txX = randomizer;
                txY = 8;
            }
            else if (tileType == 22)
            {
                while (randomizer > 3)
                {
                    randomizer -= 4;
                }
                txX = randomizer;
                txY = 9;
            }
            else if (tileType == 23)
            {
                while (randomizer > 3)
                {
                    randomizer -= 4;
                }
                txX = randomizer;
                txY = 10;
            }
            else if (tileType == 24)
            {
                while (randomizer > 3)
                {
                    randomizer -= 4;
                }
                txX = randomizer;
                txY = 11;
            }
            else if (tileType == 25)
            {
                while (randomizer > 3)
                {
                    randomizer -= 4;
                }
                txX = randomizer;
                txY = 12;
            }
            else if (tileType == 26)
            {
                while (randomizer > 3)
                {
                    randomizer -= 4;
                }
                txX = randomizer;
                txY = 13;
            }
            else if (tileType == 27)
            {
                while (randomizer > 3)
                {
                    randomizer -= 4;
                }
                txX = randomizer;
                txY = 14;
            }
            #endregion
            if (tileset.IsTextureLoaded)
            {
                spriteBatch.Draw(tileset.Texture, positionGrid * zoom + positionWorld, new Rectangle(txX * 8, txY * 8, 8, 8), Color.White, 0, Vector2.Zero, zoom, SpriteEffects.None, 1);
            }
        }
    }
}
