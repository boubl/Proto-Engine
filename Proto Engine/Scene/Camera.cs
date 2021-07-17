using System;
using Microsoft.Xna.Framework;
using Proto_Engine.ECS;

namespace Proto_Engine.Scene
{
    public static class Camera
    {
        public static Entity focusedEntity;
        public static Vector2 offset;

        public static void SetCamera(Entity focus)
        {
            focusedEntity = focus;
        }

        public static void Update()
        {
            CalculateOffset();
        }

        /// <summary>
        /// Calculate and apply the offset according to the player's position
        /// </summary>
        private static void CalculateOffset()
        {
            Rectangle levelRect = Game1.currentProject.currentLevelRectangle;
            offset = new Vector2(focusedEntity.Position.X - (Game1.PixelWidth / 2 - focusedEntity.BaseRectangle.Width), focusedEntity.Position.Y - (Game1.PixelHeight / 2 - focusedEntity.BaseRectangle.Height));

            if (offset.X > levelRect.Location.X + levelRect.Width - Game1.PixelWidth)
            {
                offset.X = levelRect.Location.X + levelRect.Width - Game1.PixelWidth;
            }
            if (offset.X < levelRect.Location.X )
            {
                offset.X = levelRect.Location.X;
            }
            if (offset.Y > levelRect.Location.Y + levelRect.Height - Game1.PixelHeight)
            {
                offset.Y = levelRect.Location.Y + levelRect.Height - Game1.PixelHeight;
            }
            if (offset.Y < levelRect.Location.Y)
            {
                offset.Y = levelRect.Location.Y;
            }
        }
    }
}
