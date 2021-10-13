using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Proto_Engine.Editor.Data.MapComponents;
using Proto_Engine.Editor.UI.Tools;
using Proto_Engine.Utils;
using System;
using System.Collections;

namespace Proto_Engine.Editor.UI.Renderers
{
    public class MapRenderer
    {
        public static bool ShowWalls = false;
        public static bool ShowTooltip = false;
        public static bool IntegerZoom = false;
        public static Vector2 Camera;
        public static float Zoom;
        public Point selectedTile;

        private MouseState lastMouseState;
        private bool isCursorOutBounds;
        private int hoveredLevel;
        private LevelRenderer levelRenderer = new LevelRenderer();
        private Vector2 startPan = Vector2.Zero;
        public MapRenderer()
        {
            Zoom = 1;
            Camera = Vector2.Zero;
            lastMouseState = Mouse.GetState();
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            Vector2 mouseWorldBeforeZoom = Vector2.Zero;
            Vector2 mouseWorldAfterZoom = Vector2.Zero;
            float scroll = (mouse.ScrollWheelValue - lastMouseState.ScrollWheelValue) / 500f;

            // Span
            if (mouse.RightButton == ButtonState.Pressed && lastMouseState.RightButton != ButtonState.Pressed)
            {
                startPan = mouse.Position.ToVector2();
            }
            else if (mouse.RightButton == ButtonState.Pressed)
            {
                Camera -= (mouse.Position - startPan.ToPoint()).ToVector2() / Zoom;

                // Start "new" pan for next epoch
                startPan = mouse.Position.ToVector2();
            }

            ScreenToWorld(mouse.Position, ref mouseWorldBeforeZoom);

            // Set zoom with scroll
            if (!ImGui.GetIO().WantCaptureMouse && mouse.ScrollWheelValue != lastMouseState.ScrollWheelValue)
            {
                // interger of float zoom
                if (IntegerZoom)
                {
                    if (scroll > 0) Zoom += 1f;
                    if (scroll < 0) Zoom -= 1f;
                    Zoom = (int)Zoom;
                    if (Zoom < 1) Zoom = 1;
                }
                else
                {
                    if (scroll > 0) Zoom *= 1.1f;
                    if (scroll < 0) Zoom *= 0.9f;
                }
            }

            ScreenToWorld(mouse.Position, ref mouseWorldAfterZoom);
            Camera += (mouseWorldBeforeZoom - mouseWorldAfterZoom);

            // Get hovered level
            hoveredLevel = -1;
            isCursorOutBounds = true;
            for (int i = 0; i < EditMode.currentMap.Levels.Count; i++)
            {
                Rectangle rectangle = EditMode.currentMap.Levels[i].RectanglePx;

                if (rectangle.Contains(mouseWorldAfterZoom) && !ImGui.GetIO().WantCaptureMouse)
                {
                    hoveredLevel = i;
                    isCursorOutBounds = false;
                }
            }

            // Get selected tile
            if (hoveredLevel > -1 && !isCursorOutBounds)
            {
                Level level = EditMode.currentMap.Levels[hoveredLevel];
                int x;
                int y;
                if (mouseWorldAfterZoom.X >= 0)
                    x = (int)(Math.Floor(mouseWorldBeforeZoom.X) / 8 - EditMode.currentMap.Levels[hoveredLevel].Position.X);
                else
                    x = (int)(Math.Ceiling(mouseWorldBeforeZoom.X) / 8 - EditMode.currentMap.Levels[hoveredLevel].Position.X);
                if (mouseWorldAfterZoom.Y >= 0)
                    y = (int)(Math.Floor(mouseWorldBeforeZoom.Y) / 8 - EditMode.currentMap.Levels[hoveredLevel].Position.Y);
                else
                    y = (int)(Math.Ceiling(mouseWorldBeforeZoom.Y) / 8 - EditMode.currentMap.Levels[hoveredLevel].Position.Y);
                selectedTile = new Point(x, y);
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    Toolbar.ToolList[Toolbar.SelectedTool].Clicked(new ToolArgs(selectedTile, hoveredLevel));
                }
                if (mouse.RightButton == ButtonState.Pressed)
                {
                    Toolbar.ToolList[Toolbar.SelectedTool].RightClicked(new ToolArgs(selectedTile, hoveredLevel));
                }
            }

            lastMouseState = mouse;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw levels
            foreach (Level level in EditMode.currentMap.Levels)
            {
                Point pos = Point.Zero;
                WorldToScreen(level.Position * 8, ref pos);
                levelRenderer.Render(spriteBatch, level, pos.ToVector2(), Zoom);

                // Draw wall tiles bounds
                if (ShowWalls)
                {
                    for (int y = 0; y < level.Height; y++)
                    {
                        for (int x = 0; x < level.Width; x++)
                        {
                            int value = LayersHelper.GetGridValueAt(
                                level.BgLayer.Grid,
                                level.Width,
                                x, y, 0);
                            if (value == 1)
                            {
                                Point wallPos = Point.Zero;
                                WorldToScreen(new Vector2(x * 8, y * 8) + level.Position * 8, ref wallPos);
                                spriteBatch.DrawRectangle(wallPos.ToVector2(), new Vector2(8 * Zoom, 8 * Zoom), Color.Gray);
                            }
                        }
                    }
                }
            }

            // Draw rectangles bounds
            foreach (Level level in EditMode.currentMap.Levels)
            {
                Point pos = level.RectanglePx.Location;
                WorldToScreen(level.RectanglePx.Location.ToVector2(), ref pos);
                spriteBatch.DrawRectangle(pos.ToVector2(), level.RectanglePx.Size.ToVector2() * Zoom, Color.Green);
            }

            // Draw hovered tile
            if (!isCursorOutBounds)
            {
                Point pos = selectedTile;
                WorldToScreen((selectedTile.ToVector2() + EditMode.currentMap.Levels[hoveredLevel].Position) * 8, ref pos);
                spriteBatch.DrawRectangle(
                    pos.ToVector2(),
                    new Vector2(8 * Zoom, 8 * Zoom),
                    Color.Red);
            }

            //tooltip with values around the selected tile
            if (ShowTooltip && !isCursorOutBounds)
            {
                ImGui.BeginTooltip();
                int tileX = selectedTile.X;
                int tileY = selectedTile.Y;
                int[] tile = LayersHelper.GetAroundTile(
                    EditMode.currentMap.Levels[hoveredLevel].BgLayer.Grid,
                    EditMode.currentMap.Levels[hoveredLevel].Width,
                    tileX,
                    tileY,
                    5, 1);
                int counter = 0;
                for (int i = 0; i < tile.Length; i++)
                {
                    ImGui.Text(tile[i].ToString());
                    if (counter > 3)
                    {
                        counter = -1;
                    }
                    else
                        ImGui.SameLine();
                    counter++;
                }
                ImGui.TextDisabled("---");
                ImGui.Text(
                    "Selected tile: " +
                    (tileX + (int)EditMode.currentMap.Levels[hoveredLevel].Position.X) +
                    ", " +
                    (tileY + (int)EditMode.currentMap.Levels[hoveredLevel].Position.Y));
                ImGui.EndTooltip();
            }
        }

        private void WorldToScreen(Vector2 fWorld, ref Point nScreen)
        {
            nScreen = ((fWorld - Camera) * Zoom).ToPoint();
        }

        private void ScreenToWorld(Point nScreen, ref Vector2 fWorld)
        {
            fWorld = (nScreen.ToVector2() / Zoom) + Camera;
        }


    }
}
