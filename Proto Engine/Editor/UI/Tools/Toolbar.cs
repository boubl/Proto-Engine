using ImGuiNET;
using Proto_Engine.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proto_Engine.Editor.UI.Tools
{
    public static class Toolbar
    {
        public static List<Tool> ToolList = new List<Tool>();
        public static int SelectedTool = 0;
        public static void Show(ref bool open)
        {
            if (open)
            {
                if (ImGui.Begin("Tools", ref open, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize))
                {
                    for (int i = 0; i < ToolList.Count; i++)
                    {
                        if (i == SelectedTool)
                        {
                            ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(0.2f, 0.3f, 0.4f, 1.0f));
                            ImGui.ImageButton(IconsTools.TexturePtr, IconsTools.Size * 2, ToolList[i].Icon, ToolList[i].Icon + IconsTools.AddSize);
                            ImGui.PopStyleColor();
                        }
                        else
                            ImGui.ImageButton(IconsTools.TexturePtr, IconsTools.Size * 2, ToolList[i].Icon, ToolList[i].Icon + IconsTools.AddSize);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.Text(ToolList[i].Name);
                            ImGui.TextDisabled(ToolList[i].Description);
                            ImGui.EndTooltip();
                        }
                    }
                    ImGui.End();
                }
            }
        }
    }
}
