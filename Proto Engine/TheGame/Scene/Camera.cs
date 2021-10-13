using System;
using Microsoft.Xna.Framework;
using Proto_Engine.Entities;
using Proto_Engine.TheGame;

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
            Rectangle levelRect = GameMode.currentProject.currentLevelRectangle;
            offset = new Vector2(
                focusedEntity.Position.X - (GameMode.PixelWidth / 2 - focusedEntity.BaseRectangle.Width),
                focusedEntity.Position.Y - (GameMode.PixelHeight / 2 - focusedEntity.BaseRectangle.Height)
                );

            if (offset.X > levelRect.Location.X + levelRect.Width - GameMode.PixelWidth)
            {
                offset.X = levelRect.Location.X + levelRect.Width - GameMode.PixelWidth;
            }
            if (offset.X < levelRect.Location.X )
            {
                offset.X = levelRect.Location.X;
            }
            if (offset.Y > levelRect.Location.Y + levelRect.Height - GameMode.PixelHeight)
            {
                offset.Y = levelRect.Location.Y + levelRect.Height - GameMode.PixelHeight;
            }
            if (offset.Y < levelRect.Location.Y)
            {
                offset.Y = levelRect.Location.Y;
            }
        }
    }
}
