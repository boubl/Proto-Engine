﻿using System;
using System.Collections.Generic;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using Proto_Engine.Data;
using Proto_Engine.Scene;
using Proto_Engine.TheGame;
using Proto_Engine.Tools;
using Proto_Engine.Utils;

namespace Proto_Engine.Entities
{
    public class Player : Entity
    {
        #region Constructors
        public Player(MonoGame_LDtk_Importer.Entity entity, string animation) : base(entity)
        {
            _animations = DataManager.Animations[animation].GetAnimationsByTags();
            currentAnimation = "Walk";
        }

        public Player(Rectangle rectangle, string animation) : base(rectangle)
        {
            _animations = DataManager.Animations[animation].GetAnimationsByTags();
            currentAnimation = "Walk";
        }

        public Player(Vector2 position, Rectangle rectangle, string animation) : base(position, rectangle)
        {
            _animations = DataManager.Animations[animation].GetAnimationsByTags();
            currentAnimation = "Walk";
        }
        #endregion

        const float maxSpeed = 2;
        const float speedIncrease = 1f;
        const float speedDecrease = 0.25f;

        const float gravity = 4f;
        const float jumpSpeedIncrease = 1.5f;
        const float jumpSpeedDecrease = 0.75f;
        const float maxJumpSpeed = 6f;

        Dictionary<string, Animation> _animations;
        Point textureSize;
        string currentAnimation;
        bool touchGround;
        bool crouch;
        bool goLeft = false;
        bool hasJumped = false;
        bool jumpButtonWasPressed;
        Rectangle collision { get => GetCurrentCollision(); }
        Vector2 velocity = Vector2.Zero;
        Vector2 remainder = Vector2.Zero;

        #region Update
        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            UpdateAnimations(ks, gameTime);

            #region Get Input
            if (ks.IsKeyDown(Keys.Right) && ks.IsKeyUp(Keys.Left))
            {
                velocity.X += speedIncrease;
                if (velocity.X > maxSpeed) velocity.X = maxSpeed;
                goLeft = false;
            }
            else if (ks.IsKeyDown(Keys.Left) && ks.IsKeyUp(Keys.Right))
            {
                velocity.X -= speedIncrease;
                if (velocity.X < -maxSpeed) velocity.X = -maxSpeed;
                goLeft = true;
            }
            else
            {
                if (goLeft)
                {
                    if (velocity.X + speedDecrease > 0) velocity.X = 0;
                    else velocity.X += speedDecrease;
                }
                else
                {
                    if (velocity.X - speedDecrease < 0) velocity.X = 0;
                    else velocity.X -= speedDecrease;
                }
            }

            if (!ks.IsKeyDown(Keys.Space))
            {
                if (!hasJumped && jumpButtonWasPressed)
                {
                    hasJumped = true;
                }
                jumpButtonWasPressed = false;
            }
            else
            {
                if (!hasJumped)
                {
                    velocity.Y -= jumpSpeedIncrease;
                    if (velocity.Y < -maxJumpSpeed)
                    {
                        hasJumped = true;
                    }
                }
                jumpButtonWasPressed = true;
            }
            #endregion

            #region Apply Gravity
            if (new Rectangle(collision.X, collision.Y + 1, collision.Width, collision.Height).CollideAt(GameMode.currentProject.currentCollisions))
            {
                touchGround = true;
                hasJumped = false;
                if (velocity.Y > 0) velocity.Y = 0;
            }
            else
            {
                touchGround = false;
                if (ks.IsKeyUp(Keys.Space)) hasJumped = true; 
                velocity.Y += jumpSpeedDecrease;
                if (velocity.Y > gravity)
                {
                    velocity.Y = gravity;
                }
            }
            #endregion

            #region Apply Velocity

            remainder.X += velocity.X;
            remainder.Y += velocity.Y;
            int moveX = (int)Math.Round(remainder.X);
            int moveY = (int)Math.Round(remainder.Y);
            if (moveX != 0 || moveY != 0)
            {
                remainder.X -= moveX;
                remainder.Y -= moveY;
                int signX = Math.Sign(moveX);
                int signY = Math.Sign(moveY);
                while (moveX != 0)
                {
                    if (!new Rectangle(collision.Location + new Point(signX, 0), collision.Size).CollideAt(GameMode.currentProject.currentCollisions))
                    {
                        //There is no Solid immediately beside us 
                        Position.X += signX;
                        moveX -= signX;
                    }
                    else
                    {
                        //Hit a solid!
                        break;
                    }
                }

                while (moveY != 0)
                {
                    if (!new Rectangle(collision.Location + new Point(0, signY), collision.Size).CollideAt(GameMode.currentProject.currentCollisions))
                    {
                        //There is no Solid immediately beside us 
                        Position.Y += signY;
                        moveY -= signY;
                    }
                    else
                    {
                        //Hit a solid!
                        break;
                    }
                }
            }

            #endregion
        }

        private Rectangle GetCurrentCollision()
        {
            Rectangle newCollision;
            if (goLeft)
            {
                newCollision = _animations[currentAnimation].GetCurrentCollision();
                if (crouch) newCollision = new Rectangle(
                                                newCollision.Location - new Point(-1, -7),
                                                new Point(
                                                    newCollision.Width + 2, newCollision.Height - 7));
                newCollision.X = textureSize.X - newCollision.X - newCollision.Width;
                newCollision.Location += Position.ToPoint();
            }
            else
            {
                newCollision = _animations[currentAnimation].GetCurrentCollision();
                if (crouch) newCollision = new Rectangle(
                                                newCollision.Location - new Point(1, -7),
                                                new Point(
                                                    newCollision.Width + 2, newCollision.Height - 7));
                newCollision.Location += Position.ToPoint();
            }
            return newCollision;
        }

        private void UpdateAnimations(KeyboardState ks, GameTime gameTime)
        {
            if (ks.IsKeyDown(Keys.Right) && ks.IsKeyUp(Keys.Left)) currentAnimation = "Walk";
            else if (ks.IsKeyDown(Keys.Left) && ks.IsKeyUp(Keys.Right)) currentAnimation = "Walk";
            else currentAnimation = "Idle";

            if (ks.IsKeyDown(Keys.Down) && touchGround) crouch = true;
            else crouch = false;

            if (!touchGround && velocity.Y <= 1 && velocity.Y >= -1) currentAnimation = "AfterJump";
            else if (!touchGround && velocity.Y < -1) currentAnimation = "Jump";
            else if (!touchGround && velocity.Y > 1) currentAnimation = "Falling";
            _animations[currentAnimation].Update(gameTime);
            textureSize = _animations[currentAnimation].GetCurrentFrameSize();
        }

        #endregion

        #region Drawing
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (crouch)
                _animations[currentAnimation].Draw(
                    spriteBatch,
                    new Rectangle(
                        Position.ToPoint() - Camera.offset.ToPoint() - new Point(1, -7),
                        new Point(
                            _animations[currentAnimation].CurrentFrame.Width + 2, _animations[currentAnimation].CurrentFrame.Height - 7)),
                    goLeft
                );
            else
                _animations[currentAnimation].Draw(spriteBatch, Position - Camera.offset, goLeft);
#if DEBUG
            if (GameMode.viewCollisions) spriteBatch.DrawRectangle(new Rectangle(collision.Location - Camera.offset.ToPoint(), collision.Size), Color.Red);
#endif
        }
        public void DrawLight(SpriteBatch spriteBatch)
        {
            Vector2 pos = Position - Camera.offset - (new Vector2(BaseRectangle.Width * 5 / 2, BaseRectangle.Width * 5 / 2));
            spriteBatch.Draw(LightCreator.Light, new Rectangle(pos.ToPoint(), new Point(BaseRectangle.Width * 5, BaseRectangle.Width * 5 )), Color.White);
        }

        public void DrawImGui(ref bool drawIt)
        {
            if (drawIt && ImGui.Begin("Player", ref drawIt))
            {
                ImGui.Text("Position: " + Position.ToString());
                ImGui.Text("Velocity:" + velocity.ToString());
                ImGui.Text("Has Jumped: " + hasJumped.ToString());
                ImGui.Text("Touch Ground: " + touchGround);
                ImGui.Text("Jump Button Was Pressed: " + jumpButtonWasPressed.ToString());
                ImGui.End();
            }
        }
        #endregion
    }
}
