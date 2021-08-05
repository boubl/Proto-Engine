using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Proto_Engine
{
    public static class RectangleCollisionsExtensions
    {
        public static bool CollideAt(this Rectangle rectangle, List<Rectangle> collisions)
        {
            foreach (Rectangle collision in collisions)
            {
                if (rectangle.CollideAt(collision))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CollideAt(this Rectangle rectangle, Rectangle collision)
        {
            if (rectangle.LeftCollideAt(collision)) return true;
            else if (rectangle.RightCollideAt(collision)) return true;
            else if (rectangle.TopCollideAt(collision)) return true;
            else if (rectangle.BottomCollideAt(collision)) return true;
            else return false;
        }

        public static bool TopCollideAt(this Rectangle rectangle, List<Rectangle> collisions)
        {
            foreach (Rectangle collision in collisions)
            {
                if (rectangle.TopCollideAt(collision)) return true;
            }
            return false;
        }

        public static bool BottomCollideAt(this Rectangle rectangle, List<Rectangle> collisions)
        {
            foreach (Rectangle collision in collisions)
            {
                if (rectangle.BottomCollideAt(collision)) return true;
            }
            return false;
        }

        public static bool LeftCollideAt(this Rectangle rectangle, List<Rectangle> collisions)
        {
            foreach (Rectangle collision in collisions)
            {
                if (rectangle.LeftCollideAt(collision)) return true;
            }
            return false;
        }

        public static bool RightCollideAt(this Rectangle rectangle, List<Rectangle> collisions)
        {
            foreach (Rectangle collision in collisions)
            {
                if (rectangle.RightCollideAt(collision)) return true;
            }
            return false;
        }
        public static bool TopCollideAt(this Rectangle rectangle, Rectangle collision)
        {
            return rectangle.Bottom > collision.Top && rectangle.Top < collision.Top && rectangle.Right > collision.Left && rectangle.Left < collision.Right;
        }

        public static bool BottomCollideAt(this Rectangle rectangle, Rectangle collision)
        {
            return rectangle.Top < collision.Bottom && rectangle.Bottom > collision.Bottom && rectangle.Right > collision.Left && rectangle.Left < collision.Right;
        }

        public static bool LeftCollideAt(this Rectangle rectangle, Rectangle collision)
        {
            return rectangle.Right > collision.Left && rectangle.Left < collision.Left && rectangle.Bottom > collision.Top && rectangle.Top < collision.Bottom;
        }

        public static bool RightCollideAt(this Rectangle rectangle, Rectangle collision)
        {
            return rectangle.Left < collision.Right && rectangle.Right > collision.Right && rectangle.Bottom > collision.Top && rectangle.Top < collision.Bottom;
        }
    }
}
