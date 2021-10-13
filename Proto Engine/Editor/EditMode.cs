using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Proto_Engine.Editor.Data;
using Proto_Engine.Editor.Data.MapComponents.Defs;
using Proto_Engine.Editor.UI;
using Proto_Engine.Editor.UI.Managers;
using Proto_Engine.Editor.UI.Renderers;
using Proto_Engine.Editor.UI.Tools;
using Proto_Engine.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proto_Engine.Editor
{
    public class EditMode
    {
        public const int TileSize = 8;
        public static MapFile currentMap;
        public MapRenderer mapRenderer;
        public static string MapFolder = "Content/maps";

        private LevelsManager levelsManager = new LevelsManager();
        private TilesetsManager tilesetsManager = new TilesetsManager();

        #region View Bools
        private bool showLevelsManager = false;
        private bool showTilesetsManager = false;
        private bool showEntityEditor = false;
        private bool showDemoWindow = false;
        private bool showToolbar = false;
        #endregion

        public void Init(GraphicsDevice graphicsDevice)
        {
            currentMap = new MapFile();
            mapRenderer = new MapRenderer();
        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {

        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            mapRenderer.Update(gameTime);
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch _spriteBatch, GameTime gameTime)
        {
            ProtoEngine.ImGuiRenderer.BeforeLayout(gameTime);
            ImGui.PushFont(ProtoEngine.font);
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.MenuItem("Amunero Editor")) ProtoEngine.Mode = EngineMode.Game;
                ImGui.Text("|");
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("New Map", "Ctrl+N")) currentMap = new MapFile();
                    if (ImGui.MenuItem("Load Map", "Ctrl+O"))
                    {
                        currentMap.Dispose();
                        currentMap = MapFile.LoadFile();
                    }
                    if (ImGui.MenuItem("Save Map", "Ctrl+S")) currentMap.Save();
                    ImGui.TextDisabled("---");
                    if (ImGui.MenuItem("Quit App", "Echap")) ProtoEngine.ExitGame = true;
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Edit"))
                {
                    if (ImGui.MenuItem("Undo", "CTRL+Z")) throw new NotImplementedException();
                    if (ImGui.MenuItem("Redo", "CTRL+Y")) throw new NotImplementedException();
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("View"))
                {
                    ImGui.MenuItem("Levels Manager", null, ref showLevelsManager);
                    ImGui.MenuItem("Tilesets Manager", null, ref showTilesetsManager);
                    ImGui.MenuItem("Toolbar", null, ref showToolbar);
                    ImGui.MenuItem("Demo Window", null, ref showDemoWindow);
                    ImGui.MenuItem("Show walls bounds", null, ref MapRenderer.ShowWalls);
                    ImGui.MenuItem("Show selected tile's tooltip", null, ref MapRenderer.ShowTooltip);
                    ImGui.MenuItem("Integer zoom", null, ref MapRenderer.IntegerZoom);
                    if (ImGui.MenuItem("Reset view"))
                    {
                        MapRenderer.Zoom = 1;
                        MapRenderer.Camera = Vector2.Zero;
                    }
                    ImGui.EndMenu();
                }
                ImGui.EndMainMenuBar();
            }


            graphicsDevice.Clear(Color.White);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
            mapRenderer.Draw(_spriteBatch);
            _spriteBatch.End();

            if (showDemoWindow) ImGui.ShowDemoWindow();
            if (showLevelsManager) levelsManager.Show(currentMap.Levels, ref showLevelsManager);
            if (showTilesetsManager) tilesetsManager.Draw(currentMap.Definitions.Tilesets, ref showTilesetsManager);
            if (showToolbar) Toolbar.Show(ref showToolbar);

            ProtoEngine.ImGuiRenderer.AfterLayout();
        }
    }
}
