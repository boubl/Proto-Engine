using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proto_Engine.Editor.UI.Tools
{
    public abstract class Tool
    {
        public virtual string Name { get; set; }
        public virtual string Description {  get; set; }
        public System.Numerics.Vector2 Icon {  get; set; }
        public virtual bool Clicked(ToolArgs e)
        {
            return false;
        }
        public virtual bool RightClicked(ToolArgs e)
        {
            return false;
        }
        public virtual bool Hold(ToolArgs e)
        {
            return false;
        }
        public virtual bool RightHold(ToolArgs e)
        {
            return false;
        }
    }

    public class ToolArgs
    {
        public Point SelectedTile;
        public int HoveredLevel;
        public ToolArgs(Point selectedTile, int hoveredLevel)
        {
            SelectedTile = selectedTile;
            HoveredLevel = hoveredLevel;
        }
    }
}
