using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proto_Engine.Editor.UI.Tools
{
    public class Pen : Tool
    {
        public override string Name { get { return "Pen"; } }
        public override string Description { get { return "Draw tiles from selected tileset"; } }

        // make it use the foreground tileset
        public override bool Hold(ToolArgs e)
        {
            Random random = new Random();
            int i = EditMode.currentMap.Levels[e.HoveredLevel].Width * e.SelectedTile.Y + e.SelectedTile.X;
            EditMode.currentMap.Levels[e.HoveredLevel].BgLayer.Grid[i] = 1;
            EditMode.currentMap.Levels[e.HoveredLevel].BgLayer.Tilesets[i] = 0;
            EditMode.currentMap.Levels[e.HoveredLevel].BgLayer.Randomizer[i] = random.Next(20);
            return true;
        }
        public override bool Clicked(ToolArgs e)
        {
            return Hold(e);
        }

        // make it use the background tileset
        public override bool RightHold(ToolArgs e)
        {
            Random random = new Random();
            int i = EditMode.currentMap.Levels[e.HoveredLevel].Width * e.SelectedTile.Y + e.SelectedTile.X;
            EditMode.currentMap.Levels[e.HoveredLevel].BgLayer.Grid[i] = 1;
            EditMode.currentMap.Levels[e.HoveredLevel].BgLayer.Tilesets[i] = 0;
            EditMode.currentMap.Levels[e.HoveredLevel].BgLayer.Randomizer[i] = random.Next(20);
            return true;
        }
        public override bool RightClicked(ToolArgs e)
        {
            return RightHold(e);
        }
    }
}
