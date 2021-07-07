using System;
using Microsoft.Xna.Framework;
using Proto_Engine.ECS;

namespace Proto_Engine.Scene
{
    public class Camera
    {
        private Entity focusedEntity;
        public static Vector2 offset;

        public Camera(Entity focus)
        {
            focusedEntity = focus;
        }

        public void Update()
        {
            offset = focusedEntity.Position;
        }
    }
}
