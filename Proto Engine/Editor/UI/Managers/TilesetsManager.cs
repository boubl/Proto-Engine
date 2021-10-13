using ImGuiNET;
using Proto_Engine.Editor.Data.MapComponents.Defs;
using Proto_Engine.Editor.UI.Editors;
using Proto_Engine.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proto_Engine.Editor.UI.Managers
{
    public class TilesetsManager
    {
        private bool _viewEditor;
        private int _tilesetEdited;
        private TilesetEditor _tilesetEditor;
        private static int tilesetIdGen;

        public TilesetsManager()
        {
            _viewEditor = false;
        }

        public void Draw(List<Tileset> tilesets, ref bool open)
        {
            ImGui.Begin("Tilesets", ref open);

            if (ImGui.Button("New")) tilesets.Add(new Tileset(tilesetIdGen++));

            int i = 0;
            List<int> tilesetsToDelete = new List<int>();
            foreach (Tileset tileset in tilesets)
            {
                if (ImGui.Button(tileset.Name))
                {
                    _viewEditor = true;
                    _tilesetEdited = i;
                    _tilesetEditor = new TilesetEditor(tileset);
                }
                if (ImGui.IsItemHovered())
                {
                    if (ProtoEngine.ImGuiRenderer.IsTextureBinded(tileset.TexturePtr))
                    {
                        ImGui.BeginTooltip();
                        ImGui.Image(tileset.TexturePtr, new System.Numerics.Vector2(tileset.Texture.Width, tileset.Texture.Height));
                        ImGui.EndTooltip();
                    }
                    else
                    {
                        ImGui.BeginTooltip();
                        ImGui.TextDisabled("No tileset loaded");
                        ImGui.EndTooltip();
                    }
                }
                ImGui.SameLine();
                ImGui.PushID(tileset.Name + "Delete");
                if (ImGui.Button("Delete"))
                {
                    tilesetsToDelete.Add(i);
                }
                ImGui.PopID();
                i++;
            }
            foreach (int j in tilesetsToDelete)
            {
                tilesets[j].Dispose();
                tilesets.RemoveAt(j);
            }
            if (_viewEditor)
                _tilesetEditor.Draw(ref _viewEditor);

            ImGui.End();
        }
    }
}
