using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Proto_Engine.Editor.Data.MapComponents.Defs;
using Proto_Engine.Editor.UI.Managers;
using Proto_Engine.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Proto_Engine.Editor.UI.Editors
{
    public class TilesetEditor
    {
        Tileset _tileset;
        public TilesetEditor(Tileset tileset)
        {
            _tileset = tileset;
        }

        public void Draw(ref bool open)
        {
            if (open)
            {
                ImGui.OpenPopup("Tileset Edition: " + _tileset.Name + "###TilesetEditor");
                if (ImGui.BeginPopupModal("Tileset Edition: " + _tileset.Name + "###TilesetEditor"))
                {
                    string name = _tileset.Name;
                    ImGui.InputText("Name", ref name, 32);
                    _tileset.Name = name;

                    ImGui.Text("Tileset image file: " + Path.GetFileName(_tileset.TextureOrigin));
                    
                    if (ImGui.Button("Load tileset image"))
                    {
                        string path = _tileset.TextureOrigin;
                        Texture2D texture = _tileset.Texture;
                        if (LoadTexture(ref path, ref texture))
                        {
                            _tileset.Texture = texture;
                            _tileset.TextureOrigin = path;
                            if (ProtoEngine.ImGuiRenderer.IsTextureBinded(_tileset.TexturePtr))
                                ProtoEngine.ImGuiRenderer.UnbindTexture(_tileset.TexturePtr);
                            _tileset.TexturePtr = ProtoEngine.ImGuiRenderer.BindTexture(_tileset.Texture);
                            _tileset.IsTextureLoaded = true;
                        }
                    }
                    ImGui.SameLine();
                    ImGuiExtension.HelpMarker(
                        "Tileset texture must:\n" +
                        "- be [insert size]\n" +
                        "- with 8x8 tiles\n" +
                        "- respect a pattern you can see here:\n\n" +
                        "../Content/textures/examples/tileset.png");

                    if (File.Exists(_tileset.TextureOrigin))
                    {
                        if (ImGui.Button("Open in explorer"))
                        {
                            System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(Path.GetFullPath(_tileset.TextureOrigin)));
                        }
                    }

                    if (ProtoEngine.ImGuiRenderer.IsTextureBinded(_tileset.TexturePtr))
                        ImGui.Image(_tileset.TexturePtr, new System.Numerics.Vector2(_tileset.Texture.Width, _tileset.Texture.Height));

                    if (ImGui.Button("Close"))
                    {
                        open = false;
                        ImGui.CloseCurrentPopup();
                    }

                    ImGui.EndPopup();
                }
            }
        }

        public bool LoadTexture(ref string path, ref Texture2D texture)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Path.GetFullPath(EditMode.MapFolder);
            dialog.Filter = "PNG File (*.png)|*.png|Suck my dick (*.*)|*.*";
            dialog.Title = "Load Image";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(dialog.FileName))
                {
                    path = dialog.FileName;
                    texture = Texture2D.FromFile(ProtoEngine.GraphicsDeviceOMG, path);
                    dialog.Dispose();
                    return true;
                }
                else
                {
                    dialog.Dispose();
                    return false;
                }
            }
            else
            {
                if (File.Exists(path))
                {
                    dialog.Dispose();
                    return true;
                }
                else
                {
                    path = "File not found";
                    dialog.Dispose();
                    return false;
                }
            }
        }
    }
}
