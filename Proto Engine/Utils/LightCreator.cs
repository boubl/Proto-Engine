using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Proto_Engine.Scene;
using System;
using System.Collections.Generic;
using System.IO;

namespace Proto_Engine.Utils
{
    public static class LightCreator
    {
        public static Texture2D Light { get; private set; }

        public static void InitLight(GraphicsDevice graphicsDevice)
        {
            Light = GenerateLightTx(graphicsDevice, 500, 500, 0.5f, 0.5f);
        }

        public static Texture2D GenerateLightTx(GraphicsDevice graphicsDevice, int width, int height, float scaleX, float scaleY)
        {
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(width, height);
            // Create a path that consists of a single ellipse.
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, width, height);

            // Use the path to construct a brush.
            System.Drawing.Drawing2D.PathGradientBrush pthGrBrush = new System.Drawing.Drawing2D.PathGradientBrush(path);

            // Set the color at the center of the path to blue.
            pthGrBrush.CenterColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);

            // Set the color along the entire boundary
            // of the path to aqua.
            System.Drawing.Color[] colors = { System.Drawing.Color.FromArgb(0, 255, 255, 255) };
            pthGrBrush.SurroundColors = colors;
            pthGrBrush.FocusScales = new System.Drawing.PointF(scaleX, scaleY);
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(image);
            graphics.FillEllipse(pthGrBrush, 0, 0, width, height);

            Color[] colorss = new Color[width * height];
            int i = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    System.Drawing.Color current = image.GetPixel(x, y);
                    colorss[width * y + x] = new Color(current.R, current.G, current.B, current.A);
                    i++;
                }
                i++;
            }

            image.Dispose();
            graphics.Dispose();
            pthGrBrush.Dispose();
            path.Dispose();

            Texture2D tx = new Texture2D(graphicsDevice, width, height);
            tx.SetData(colorss);
            return tx;
        }


        public static void GenerateLightImage(int width, int height, float scaleX, float scaleY, string output, System.Drawing.Color? color)
        {
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(width, height);
            // Create a path that consists of a single ellipse.
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, width, height);

            // Use the path to construct a brush.
            System.Drawing.Drawing2D.PathGradientBrush pthGrBrush = new System.Drawing.Drawing2D.PathGradientBrush(path);

            // Set the color at the center of the path to blue.
            if (color.HasValue)
            {
                pthGrBrush.CenterColor = color.Value;
            }
            else
            {
                pthGrBrush.CenterColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            }

            // Set the color along the entire boundary
            // of the path to aqua.
            System.Drawing.Color[] colors = { System.Drawing.Color.FromArgb(0, 255, 255, 255) };
            pthGrBrush.SurroundColors = colors;
            pthGrBrush.FocusScales = new System.Drawing.PointF(scaleX, scaleY);
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(image);
            graphics.FillEllipse(pthGrBrush, 0, 0, width, height);

            image.Save(output, System.Drawing.Imaging.ImageFormat.Png);

            image.Dispose();
            graphics.Dispose();
            pthGrBrush.Dispose();
            path.Dispose();
        }

    }
}
