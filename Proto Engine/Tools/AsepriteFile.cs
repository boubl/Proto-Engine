using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Proto_Engine.Tools
{
    public class AsepriteFile
    {
        private List<Frame> _frames;
        private string _spriteSheetFile;
        private List<AsepriteFrameTag> _frameTags;
        private Dictionary<string, Slice> slices;

        public Dictionary<string, Animation> GetAnimationsByTags()
        {
            Dictionary<string, Animation> result = new Dictionary<string, Animation>();
            foreach(AsepriteFrameTag tag in _frameTags)
            {
                result.Add(tag.Name, new Animation(tag.Name, Path.GetFileNameWithoutExtension(_spriteSheetFile), _frames.GetRange(tag.From, tag.To - tag.From + 1), tag.Direction, slices["Collision"].Keys.GetRange(tag.From, tag.To - tag.From + 1)));
            }
            return result;
        }

        public AsepriteFile(string file)
        {
            JsonDocument jsonDocument = JsonDocument.Parse(File.ReadAllText(file));

            _frames = new List<Frame>();
            foreach (JsonProperty property in jsonDocument.RootElement.GetProperty("frames").EnumerateObject())
            {
                Rectangle sourceRectangle = new Rectangle();
                sourceRectangle.X = property.Value.GetProperty("frame").GetProperty("x").GetInt32();
                sourceRectangle.Y = property.Value.GetProperty("frame").GetProperty("y").GetInt32();
                sourceRectangle.Width = property.Value.GetProperty("frame").GetProperty("w").GetInt32();
                sourceRectangle.Height = property.Value.GetProperty("frame").GetProperty("h").GetInt32();

                Rectangle destinationRectangle = new Rectangle();
                destinationRectangle.X = property.Value.GetProperty("spriteSourceSize").GetProperty("x").GetInt32();
                destinationRectangle.Y = property.Value.GetProperty("spriteSourceSize").GetProperty("y").GetInt32();
                destinationRectangle.Width = property.Value.GetProperty("spriteSourceSize").GetProperty("w").GetInt32();
                destinationRectangle.Height = property.Value.GetProperty("spriteSourceSize").GetProperty("h").GetInt32();

                int duration = property.Value.GetProperty("duration").GetInt32();

                Point sourceSize = new Point(
                    property.Value.GetProperty("sourceSize").GetProperty("w").GetInt32(),
                    property.Value.GetProperty("sourceSize").GetProperty("h").GetInt32()
                    );

                _frames.Add(new Frame(duration, sourceRectangle, destinationRectangle, sourceSize));
            }

            _frameTags = new List<AsepriteFrameTag>();
            foreach (JsonElement element in jsonDocument.RootElement.GetProperty("meta").GetProperty("frameTags").EnumerateArray())
            {
                string name = element.GetProperty("name").GetString();
                int from = element.GetProperty("from").GetInt32();
                int to = element.GetProperty("to").GetInt32();
                AnimationDirection direction = (AnimationDirection)Enum.Parse(typeof(AnimationDirection), element.GetProperty("direction").GetString(), true);

                _frameTags.Add(new AsepriteFrameTag(name, from, to, direction));
            }

            slices = new Dictionary<string, Slice>();
            foreach (JsonElement element in jsonDocument.RootElement.GetProperty("meta").GetProperty("slices").EnumerateArray())
            {
                string name = element.GetProperty("name").GetString();
                List<Rectangle> bounds = new List<Rectangle>();
                int lastFrame = -1;
                Rectangle lastBounds = new Rectangle();
                foreach (JsonElement key in element.GetProperty("keys").EnumerateArray().ToArray())
                {
                    Rectangle newRectangle = new Rectangle(
                        key.GetProperty("bounds").GetProperty("x").GetInt32(),
                        key.GetProperty("bounds").GetProperty("y").GetInt32(),
                        key.GetProperty("bounds").GetProperty("w").GetInt32(),
                        key.GetProperty("bounds").GetProperty("h").GetInt32()
                        );
                    while (key.GetProperty("frame").GetInt32() > lastFrame + 1)
                    {
                        bounds.Add(lastBounds);
                        lastFrame++;
                    }
                    bounds.Add(newRectangle);
                    lastBounds = newRectangle;
                    lastFrame++;
                }
                while (lastFrame < _frames.Count)
                {
                    bounds.Add(lastBounds);
                    lastFrame++;
                }

                slices.Add(name, new Slice(name, bounds));
            }

            _spriteSheetFile = jsonDocument.RootElement.GetProperty("meta").GetProperty("image").GetString();
        }
    }

    public class Animation
    {
        public string Name { get; private set; }
        public int Duration { get => GetTotalDuration(); }
        public int CurrentFrameKey { get; private set; }
        public Frame CurrentFrame { get => _frames[CurrentFrameKey]; }
        public List<Rectangle> Collisions { get; private set; }

        private AnimationDirection _direction;
        private int _directionFlag;
        private List<Frame> _frames;
        private string _textureName;
        private float _timer;

        public Rectangle GetCurrentCollision()
        {
            return Collisions[CurrentFrameKey];
        }

        private int GetTotalDuration()
        {
            int result = 0;
            foreach (Frame frame in _frames)
            {
                result += frame.Duration;
            }
            return result;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_direction == AnimationDirection.Forward)
            {
                if (_timer > CurrentFrame.Duration)
                {
                    _timer = 0f;

                    CurrentFrameKey++;

                    if (CurrentFrameKey >= _frames.Count)
                        CurrentFrameKey = 0;
                }
            }
            else if (_direction == AnimationDirection.PingPong)
            {
                if (_timer > CurrentFrame.Duration)
                {
                    _timer = 0f;

                    if (_directionFlag == 0)
                    {
                        CurrentFrameKey++;

                        if (CurrentFrameKey >= _frames.Count)
                        {
                            CurrentFrameKey = _frames.Count - 2;
                            _directionFlag = 1;
                        }
                    }
                    else
                    {
                        CurrentFrameKey--;

                        if (CurrentFrameKey < 0)
                        {
                            CurrentFrameKey = 1;
                            _directionFlag = 0;
                        }
                    }
                }
            }
            else
            {
                if (_timer > CurrentFrame.Duration)
                {
                    _timer = 0f;

                    CurrentFrameKey--;

                    if (CurrentFrameKey < 0)
                        CurrentFrameKey = _frames.Count - 1;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Rectangle destinationRectangle = CurrentFrame.DestinationRectangle;
            destinationRectangle.Location = position.ToPoint();
            spriteBatch.Draw(DataManager.Textures[_textureName], destinationRectangle, CurrentFrame.SourceRectangle, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool isTextureReversedOnX)
        {
            Rectangle destinationRectangle = CurrentFrame.DestinationRectangle;
            if (isTextureReversedOnX)
            {
                destinationRectangle.X = CurrentFrame.SourceSize.X - destinationRectangle.X - destinationRectangle.Width + 1;
                destinationRectangle.Location += position.ToPoint();
                spriteBatch.Draw(DataManager.Textures[_textureName], destinationRectangle, CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);
            }
            else
            {
                destinationRectangle.Location += position.ToPoint();
                spriteBatch.Draw(DataManager.Textures[_textureName], destinationRectangle, CurrentFrame.SourceRectangle, Color.White);
            }
        }

        public Point GetCurrentFrameSize()
        {
            return _frames[CurrentFrameKey].SourceSize;
        }

        public Animation(string name, string textureName, List<Frame> frames, AnimationDirection direction, List<Rectangle> collisions)
        {
            Name = name;
            _textureName = textureName;
            _frames = frames;
            _direction = direction;
            _directionFlag = 0;
            CurrentFrameKey = 0;
            _timer = 0f;
            if (collisions.Count != frames.Count) throw new ArgumentException("There is not as much frames as collisions");
            else Collisions = collisions;
        }

        public void Stop()
        {
            _timer = 0f;
            CurrentFrameKey = 0;
        }
    }

    public class Frame
    {
        public int Duration { get; private set; }
        public Rectangle SourceRectangle { get; private set; }
        public Rectangle DestinationRectangle { get; private set; }
        public Point SourceSize { get; private set; }
        public int Height { get => DestinationRectangle.Height; }
        public int Width { get => DestinationRectangle.Width; }
        public Frame(int duration, Rectangle sourceRectangle, Rectangle destinationRectangle, Point sourceSize)
        {
            Duration = duration;
            SourceRectangle = sourceRectangle;
            DestinationRectangle = destinationRectangle;
            SourceSize = sourceSize;
        }
    }

    public class AsepriteFrameTag
    {
        public string Name { get; private set; }
        public int From { get; private set; }
        public int To { get; private set; }
        public AnimationDirection Direction { get; set; }

        public AsepriteFrameTag(string name, int from, int to, AnimationDirection direction)
        {
            Name = name;
            From = from;
            To = to;
            Direction = direction;
        }
    }

    public enum AnimationDirection
    {
        Forward,
        Reverse,
        PingPong
    }

    public class Slice
    {
        public string Name { get; set; }
        public List<Rectangle> Keys { get; set; }
        public Slice(string name, List<Rectangle> keys)
        {
            Name = name;
            Keys = keys;
        }
    }
}