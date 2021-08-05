using System;
using Microsoft.Xna.Framework;
using Proto_Engine.Entities;

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
            Rectangle levelRect = ProtoEngine.currentProject.currentLevelRectangle;
            offset = new Vector2(focusedEntity.Position.X - (ProtoEngine.PixelWidth / 2 - focusedEntity.BaseRectangle.Width), focusedEntity.Position.Y - (ProtoEngine.PixelHeight / 2 - focusedEntity.BaseRectangle.Height));

            if (offset.X > levelRect.Location.X + levelRect.Width - ProtoEngine.PixelWidth)
            {
                offset.X = levelRect.Location.X + levelRect.Width - ProtoEngine.PixelWidth;
            }
            if (offset.X < levelRect.Location.X )
            {
                offset.X = levelRect.Location.X;
            }
            if (offset.Y > levelRect.Location.Y + levelRect.Height - ProtoEngine.PixelHeight)
            {
                offset.Y = levelRect.Location.Y + levelRect.Height - ProtoEngine.PixelHeight;
            }
            if (offset.Y < levelRect.Location.Y)
            {
                offset.Y = levelRect.Location.Y;
            }
        }
    }
}
