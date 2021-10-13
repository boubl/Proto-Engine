using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Proto_Engine.Utils
{
    public static class IconsTools
    {
        public static Texture2D Texture;
        public static IntPtr TexturePtr;

        public static void Init()
        {
            Texture = Texture2D.FromFile(ProtoEngine.GraphicsDeviceOMG, "Content/textures/icons/sheet.png");
            TexturePtr = ProtoEngine.ImGuiRenderer.BindTexture(Texture);
        }
        public static Vector2 SquareSelection { get => new Vector2(0, 0); }
        public static Vector2 RoundSelection { get => new Vector2(16f / 96f, 0); }
        public static Vector2 LassoSelection { get => new Vector2(32f / 96f, 0); }
        public static Vector2 PolygonSelection { get => new Vector2(48f / 96f, 0); }
        public static Vector2 MagicWand { get => new Vector2(64f / 96f, 0); }
        public static Vector2 ParametersTools { get => new Vector2(80f / 96f, 0); }
        public static Vector2 Pen { get => new Vector2(0, 16f / 128f); }
        public static Vector2 Aerobrush { get => new Vector2(16f / 96f, 16f / 128f); }
        public static Vector2 BlackEye { get => new Vector2(32f / 96f, 16f / 128f); }
        public static Vector2 WhiteEye { get => new Vector2(48f / 96f, 16f / 128f); }
        public static Vector2 WhiteParameters { get => new Vector2(64f / 96f, 16f / 128f); }
        public static Vector2 BlackParameters { get => new Vector2(80f / 96f, 16f / 128f); }
        public static Vector2 Eraser { get => new Vector2(0, 32f / 128f); }
        public static Vector2 Eyedropper { get => new Vector2(16f / 96f, 32f / 128f); }
        public static Vector2 BlackClosedEye { get => new Vector2(32f / 96f, 32f / 128f); }
        public static Vector2 WhiteClosedEye { get => new Vector2(48f / 96f, 32f / 128f); }
        public static Vector2 BlackOpenedLock { get => new Vector2(64f / 96f, 32f / 128f); }
        public static Vector2 WhiteOpenedLock { get => new Vector2(80f / 96f, 32f / 128f); }
        public static Vector2 WaterDrop { get => new Vector2(0, 48f / 128f); }
        public static Vector2 Dithering { get => new Vector2(16f / 96f, 48f / 128f); }
        public static Vector2 Zoom { get => new Vector2(32f / 96f, 48f / 128f); }
        public static Vector2 Hand { get => new Vector2(48f / 96f, 48f / 128f); }
        public static Vector2 BlackClosedLock { get => new Vector2(64f / 96f, 48f / 128f); }
        public static Vector2 WhiteClosedLock { get => new Vector2(80f / 96f, 48f / 128f); }
        public static Vector2 Line { get => new Vector2(0, 64f / 128f); }
        public static Vector2 Curve { get => new Vector2(16f / 96f, 64f / 128f); }
        public static Vector2 Move { get => new Vector2(32f / 96f, 64f / 128f); }
        public static Vector2 Knife { get => new Vector2(48f / 96f, 64f / 128f); }
        public static Vector2 BlackTasks { get => new Vector2(64f / 96f, 64f / 128f); }
        public static Vector2 WhiteTasks { get => new Vector2(80f / 96f, 64f / 128f); }
        public static Vector2 Rectangle { get => new Vector2(0, 80f / 128f); }
        public static Vector2 FilledRectangle { get => new Vector2(16f / 96f, 80f / 128f); }
        public static Vector2 Ellipse { get => new Vector2(32f / 96f, 80f / 128f); }
        public static Vector2 FilledEllipse { get => new Vector2(48f / 96f, 80f / 128f); }
        public static Vector2 BlackFolder { get => new Vector2(64f / 96f, 80f / 128f); }
        public static Vector2 WhiteFolder { get => new Vector2(80f / 96f, 80f / 128f); }
        public static Vector2 RoundShape { get => new Vector2(0, 96f / 128f); }
        public static Vector2 SquaredShape { get => new Vector2(16f / 96f, 96f / 128f); }
        public static Vector2 InkPot { get => new Vector2(32f / 96f, 96f / 128f); }
        public static Vector2 BlackOpenedFolder { get => new Vector2(64f / 96f, 96f / 128f); }
        public static Vector2 WhiteOpenedFolder { get => new Vector2(80f / 96f, 96f / 128f); }
        public static Vector2 Asteroid { get => new Vector2(0, 112f / 128f); }
        public static Vector2 Fire { get => new Vector2(16f / 96f, 112f / 128f); }
        public static Vector2 BlackAddFolder { get => new Vector2(32f / 96f, 112f / 128f); }
        public static Vector2 WhiteAddFolder { get => new Vector2(48f / 96f, 112f / 128f); }
        public static Vector2 BlackFlag { get => new Vector2(64f / 96f, 112f / 128f); }
        public static Vector2 WhiteFlag { get => new Vector2(80f / 96f, 112f / 128f); }
        public static Vector2 AddSize { get => new Vector2(16f / 96f, 16f / 128f); }
        public static Vector2 Size { get => new Vector2(16, 16); }
    }
}
