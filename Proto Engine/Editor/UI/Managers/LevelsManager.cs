using ImGuiNET;
using Microsoft.Xna.Framework;
using Proto_Engine.Editor.Data.MapComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proto_Engine.Editor.UI.Managers
{
    public class LevelsManager
    {
        private bool showNewLevelDialog = false;

        private Level newLevel = new Level(new Vector2(0, 0), 0, 0, "New level");

        public LevelsManager()
        {

        }

        //replace the list with nothing and get the list frome the current map opened
        public void Show(List<Level> levels, ref bool open)
        {
            ImGui.Begin("Levels", ref open);

            if (ImGui.Button("New")) showNewLevelDialog = true;

            if (showNewLevelDialog)
            {
                if (ShowNewLevelDialog(levels))
                {
                    showNewLevelDialog = false;
                    levels.Add(newLevel);
                }
            }

            int i = 0;
            List<int> levelsToDelete = new List<int>();
            foreach (Level level in levels)
            {
                ImGui.PushID(level.Name + "Button");
                if (ImGui.Button(level.Name))
                {

                }
                ImGui.PopID();
                ImGui.SameLine();
                ImGui.PushID(level.Name + "Delete");
                if (ImGui.Button("Delete"))
                {
                    levelsToDelete.Add(i);
                }
                ImGui.PopID();
                i++;
            }
            foreach (int j in levelsToDelete)
            {
                levels[j].Dispose();
                levels.RemoveAt(j);
            }

            ImGui.End();
        }

        private bool ShowNewLevelDialog(List<Level> levels)
        {
            // bool to know if the level is valid
            bool isValid = true;

            if (ImGui.Begin("New level", ImGuiWindowFlags.Modal))
            {
                string name = newLevel.Name;
                ImGui.InputText("Name", ref name, 32);
                int[] pos = { (int)newLevel.Position.X, (int)newLevel.Position.Y };
                ImGui.InputInt2("Position", ref pos[0]);
                int[] size = { newLevel.Width, newLevel.Height };
                ImGui.InputInt2("Size", ref size[0]);

                Rectangle levelRect = new Rectangle(pos[0], pos[1], size[0], size[1]);

                if (levelRect.Width * levelRect.Height == 0)
                {
                    isValid = false;
                }

                // check if the given coordinates are valid
                foreach (Level lvl in levels)
                {
                    if (lvl.Rectangle.Intersects(levelRect))
                    {
                        isValid = false;
                    }
                }

                if (!isValid)
                {
                    ImGui.TextColored(new System.Numerics.Vector4(1, 0, 0, 1), "Rectangle not valid");
                    ImGui.Button("Create");
                    ImGui.SameLine();
                }
                else
                {
                    isValid = ImGui.Button("Create");
                    ImGui.SameLine();
                }
                if (ImGui.Button("Cancel"))
                {
                    showNewLevelDialog = false;
                    ImGui.End();
                    return false;
                }

                newLevel = new Level(new Vector2(pos[0], pos[1]), size[0], size[1], name);

            }
            else isValid = false;
            ImGui.End();

            return isValid;
        }
    }
}
