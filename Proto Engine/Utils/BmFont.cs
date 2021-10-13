// ---- AngelCode BmFont XML serializer ----------------------
// ---- By DeadlyDan @ deadlydan@gmail.com -------------------
// ---- There's no license restrictions, use as you will. ----
// ---- Credits to http://www.angelcode.com/ -----------------
// ---- Monogame renderer based on MonoGame.Extended ---------
// ---- by chamalowmoelleux ----------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BmFont
{
	[Serializable]
	[XmlRoot ( "font" )]
	public class FontFile
	{
		[XmlElement ( "info" )]
		public FontInfo Info
		{
			get;
			set;
		}

		[XmlElement ( "common" )]
		public FontCommon Common
		{
			get;
			set;
		}

		[XmlArray ( "pages" )]
		[XmlArrayItem ( "page" )]
		public List<FontPage> Pages
		{
			get;
			set;
		}

		[XmlArray ( "chars" )]
		[XmlArrayItem ( "char" )]
		public List<FontChar> Chars
		{
			get;
			set;
		}

		[XmlArray ( "kernings" )]
		[XmlArrayItem ( "kerning" )]
		public List<FontKerning> Kernings
		{
			get;
			set;
		}
	}

	[Serializable]
	public class FontInfo
	{
		[XmlAttribute ( "face" )]
		public String Face
		{
			get;
			set;
		}

		[XmlAttribute ( "size" )]
		public Int32 Size
		{
			get;
			set;
		}

		[XmlAttribute ( "bold" )]
		public Int32 Bold
		{
			get;
			set;
		}

		[XmlAttribute ( "italic" )]
		public Int32 Italic
		{
			get;
			set;
		}

		[XmlAttribute ( "charset" )]
		public String CharSet
		{
			get;
			set;
		}

		[XmlAttribute ( "unicode" )]
		public Int32 Unicode
		{
			get;
			set;
		}

		[XmlAttribute ( "stretchH" )]
		public Int32 StretchHeight
		{
			get;
			set;
		}

		[XmlAttribute ( "smooth" )]
		public Int32 Smooth
		{
			get;
			set;
		}

		[XmlAttribute ( "aa" )]
		public Int32 SuperSampling
		{
			get;
			set;
		}

		private Rectangle _Padding;
		[XmlAttribute ( "padding" )]
		public String Padding
		{
			get
			{
				return _Padding.X + "," + _Padding.Y + "," + _Padding.Width + "," + _Padding.Height;
			}
			set
			{
				String[] padding = value.Split ( ',' );
				_Padding = new Rectangle ( Convert.ToInt32 ( padding[0] ), Convert.ToInt32 ( padding[1] ), Convert.ToInt32 ( padding[2] ), Convert.ToInt32 ( padding[3] ) );
			}
		}

		private Point _Spacing;
		[XmlAttribute ( "spacing" )]
		public String Spacing
		{
			get
			{
				return _Spacing.X + "," + _Spacing.Y;
			}
			set
			{
				String[] spacing = value.Split ( ',' );
				_Spacing = new Point ( Convert.ToInt32 ( spacing[0] ), Convert.ToInt32 ( spacing[1] ) );
			}
		}

		[XmlAttribute ( "outline" )]
		public Int32 OutLine
		{
			get;
			set;
		}
	}

	[Serializable]
	public class FontCommon
	{
		[XmlAttribute ( "lineHeight" )]
		public Int32 LineHeight
		{
			get;
			set;
		}

		[XmlAttribute ( "base" )]
		public Int32 Base
		{
			get;
			set;
		}

		[XmlAttribute ( "scaleW" )]
		public Int32 ScaleW
		{
			get;
			set;
		}

		[XmlAttribute ( "scaleH" )]
		public Int32 ScaleH
		{
			get;
			set;
		}

		[XmlAttribute ( "pages" )]
		public Int32 Pages
		{
			get;
			set;
		}

		[XmlAttribute ( "packed" )]
		public Int32 Packed
		{
			get;
			set;
		}

		[XmlAttribute ( "alphaChnl" )]
		public Int32 AlphaChannel
		{
			get;
			set;
		}

		[XmlAttribute ( "redChnl" )]
		public Int32 RedChannel
		{
			get;
			set;
		}

		[XmlAttribute ( "greenChnl" )]
		public Int32 GreenChannel
		{
			get;
			set;
		}

		[XmlAttribute ( "blueChnl" )]
		public Int32 BlueChannel
		{
			get;
			set;
		}
	}

	[Serializable]
	public class FontPage
	{
		[XmlAttribute ( "id" )]
		public Int32 ID
		{
			get;
			set;
		}

		[XmlAttribute ( "file" )]
		public String File
		{
			get;
			set;
		}
	}

	[Serializable]
	public class FontChar
	{
		[XmlAttribute ( "id" )]
		public Int32 ID
		{
			get;
			set;
		}

		[XmlAttribute ( "x" )]
		public Int32 X
		{
			get;
			set;
		}

		[XmlAttribute ( "y" )]
		public Int32 Y
		{
			get;
			set;
		}

		[XmlAttribute ( "width" )]
		public Int32 Width
		{
			get;
			set;
		}

		[XmlAttribute ( "height" )]
		public Int32 Height
		{
			get;
			set;
		}

		[XmlAttribute ( "xoffset" )]
		public Int32 XOffset
		{
			get;
			set;
		}

		[XmlAttribute ( "yoffset" )]
		public Int32 YOffset
		{
			get;
			set;
		}

		[XmlAttribute ( "xadvance" )]
		public Int32 XAdvance
		{
			get;
			set;
		}

		[XmlAttribute ( "page" )]
		public Int32 Page
		{
			get;
			set;
		}

		[XmlAttribute ( "chnl" )]
		public Int32 Channel
		{
			get;
			set;
		}
	}

	[Serializable]
	public class FontKerning
	{
		[XmlAttribute ( "first" )]
		public Int32 First
		{
			get;
			set;
		}

		[XmlAttribute ( "second" )]
		public Int32 Second
		{
			get;
			set;
		}

		[XmlAttribute ( "amount" )]
		public Int32 Amount
		{
			get;
			set;
		}
	}

	public class FontLoader
	{
		private static FontFile LoadFile(String filename)
		{
			XmlSerializer deserializer = new XmlSerializer(typeof(FontFile));
			TextReader textReader = new StreamReader(filename);
			FontFile file = (FontFile)deserializer.Deserialize(textReader);
			textReader.Close();
			return file;
		}

		public static BitmapFont Load(String filename, GraphicsDevice graphicsDevice)
		{
			FontFile fontFile = LoadFile(filename);
			string baseFolder = filename.Remove(filename.Length - Path.GetFileName(filename).Length);
			var assets = new List<string>();
			foreach (var fontPage in fontFile.Pages)
			{
				var assetName = Path.GetFileName(fontPage.File);
				assets.Add(assetName);
			}

			var textures = assets
				.Select(textureName => Texture2D.FromFile(graphicsDevice, baseFolder + textureName))
				.ToArray();

			var lineHeight = fontFile.Common.LineHeight;

			var regions = new BitmapFontRegion[fontFile.Chars.Count];
			int i = 0;
			foreach (var c in fontFile.Chars)
			{
				var character = c.ID;
				var textureIndex = c.Page;
				var x = c.X;
				var y = c.Y;
				var width = c.Width;
				var height = c.Height;
				var xOffset = c.XOffset;
				var yOffset = c.YOffset;
				var xAdvance = c.XAdvance;
				var texture = textures[textureIndex];
				var region = new Rectangle(x, y, width, height);
				regions[i] = new BitmapFontRegion(texture, region, character, xOffset, yOffset, xAdvance);
				i++;
			}

			var characterMap = regions.ToDictionary(r => r.Character);
			var kerningsCount = fontFile.Kernings.Count;

			foreach (var k in fontFile.Kernings)
			{
				var first = k.First;
				var second = k.Second;
				var amount = k.Amount;

				// Find region
				if (!characterMap.TryGetValue(first, out var region))
					continue;

				region.Kernings[second] = amount;
			}

			return new BitmapFont(Path.GetFileNameWithoutExtension(filename), regions, lineHeight);
		}
	}

	public class BitmapFont
	{
		private readonly Dictionary<int, BitmapFontRegion> _characterMap = new Dictionary<int, BitmapFontRegion>();

		public BitmapFont(string name, IEnumerable<BitmapFontRegion> regions, int lineHeight)
		{
			foreach (var region in regions)
				_characterMap.Add(region.Character, region);

			Name = name;
			LineHeight = lineHeight;
		}

		public string Name { get; }
		public int LineHeight { get; }
		public int LetterSpacing { get; set; }
		public static bool UseKernings { get; set; } = true;

		public BitmapFontRegion GetCharacterRegion(int character)
		{
			return _characterMap.TryGetValue(character, out var region) ? region : null;
		}

		public Vector2 MeasureString(string text)
		{
			if (string.IsNullOrEmpty(text))
				return Vector2.Zero;

			var stringRectangle = GetStringRectangle(text);
			return new Vector2(stringRectangle.Width, stringRectangle.Height);
		}

		public Vector2 MeasureString(StringBuilder text)
		{
			if (text == null || text.Length == 0)
				return Vector2.Zero;

			var stringRectangle = GetStringRectangle(text);
			return new Vector2(stringRectangle.Width, stringRectangle.Height);
		}

		public Rectangle GetStringRectangle(string text)
		{
			return GetStringRectangle(text, Vector2.Zero);
		}

		public Rectangle GetStringRectangle(string text, Vector2 position)
		{
			var glyphs = GetGlyphs(text, position);
			var rectangle = new Rectangle((int)position.X, (int)position.Y, 0, LineHeight);

			foreach (var glyph in glyphs)
			{
				if (glyph.FontRegion != null)
				{
					var right = glyph.Position.X + glyph.FontRegion.Width;

					if (right > rectangle.Right)
						rectangle.Width = (int)(right - rectangle.Left);
				}

				if (glyph.Character == '\n')
					rectangle.Height += LineHeight;
			}

			return rectangle;
		}

		public Rectangle GetStringRectangle(StringBuilder text, Vector2? position = null)
		{
			var position1 = position ?? new Vector2();
			var glyphs = GetGlyphs(text, position1);
			var rectangle = new Rectangle((int)position1.X, (int)position1.Y, 0, LineHeight);

			foreach (var glyph in glyphs)
			{
				if (glyph.FontRegion != null)
				{
					var right = glyph.Position.X + glyph.FontRegion.Width;

					if (right > rectangle.Right)
						rectangle.Width = (int)(right - rectangle.Left);
				}

				if (glyph.Character == '\n')
					rectangle.Height += LineHeight;
			}

			return rectangle;
		}

		public StringGlyphEnumerable GetGlyphs(string text, Vector2? position = null)
		{
			return new StringGlyphEnumerable(this, text, position);
		}

		public StringBuilderGlyphEnumerable GetGlyphs(StringBuilder text, Vector2? position)
		{
			return new StringBuilderGlyphEnumerable(this, text, position);
		}

		public override string ToString()
		{
			return $"{Name}";
		}

		public struct StringGlyphEnumerable : IEnumerable<BitmapFontGlyph>
		{
			private readonly StringGlyphEnumerator _enumerator;

			public StringGlyphEnumerable(BitmapFont font, string text, Vector2? position)
			{
				_enumerator = new StringGlyphEnumerator(font, text, position);
			}

			public StringGlyphEnumerator GetEnumerator()
			{
				return _enumerator;
			}

			IEnumerator<BitmapFontGlyph> IEnumerable<BitmapFontGlyph>.GetEnumerator()
			{
				return _enumerator;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return _enumerator;
			}
		}

		public struct StringGlyphEnumerator : IEnumerator<BitmapFontGlyph>
		{
			private readonly BitmapFont _font;
			private readonly string _text;
			private int _index;
			private readonly Vector2 _position;
			private Vector2 _positionDelta;
			private BitmapFontGlyph _currentGlyph;
			private BitmapFontGlyph? _previousGlyph;

			object IEnumerator.Current
			{
				get
				{
					// casting a struct to object will box it, behaviour we want to avoid...
					throw new InvalidOperationException();
				}
			}

			public BitmapFontGlyph Current => _currentGlyph;

            public StringGlyphEnumerator(BitmapFont font, string text, Vector2? position)
			{
				_font = font;
				_text = text;
				_index = -1;
				_position = position ?? new Vector2();
				_positionDelta = new Vector2();
				_currentGlyph = new BitmapFontGlyph();
				_previousGlyph = null;
			}

			public bool MoveNext()
			{
				if (++_index >= _text.Length)
					return false;

				var character = GetUnicodeCodePoint(_text, ref _index);
				_currentGlyph.Character = character;
				_currentGlyph.FontRegion = _font.GetCharacterRegion(character);
				_currentGlyph.Position = _position + _positionDelta;

				if (_currentGlyph.FontRegion != null)
				{
					_currentGlyph.Position.X += _currentGlyph.FontRegion.XOffset;
					_currentGlyph.Position.Y += _currentGlyph.FontRegion.YOffset;
					_positionDelta.X += _currentGlyph.FontRegion.XAdvance + _font.LetterSpacing;
				}

				if (UseKernings && _previousGlyph?.FontRegion != null)
				{
					if (_previousGlyph.Value.FontRegion.Kernings.TryGetValue(character, out var amount))
					{
						_positionDelta.X += amount;
						_currentGlyph.Position.X += amount;
					}
				}

				_previousGlyph = _currentGlyph;

				if (character != '\n')
					return true;

				_positionDelta.Y += _font.LineHeight;
				_positionDelta.X = 0;
				_previousGlyph = null;

				return true;
			}

			private static int GetUnicodeCodePoint(string text, ref int index)
			{
				return char.IsHighSurrogate(text[index]) && ++index < text.Length
					? char.ConvertToUtf32(text[index - 1], text[index])
					: text[index];
			}

			public void Dispose()
			{
			}

			public void Reset()
			{
				_positionDelta = new Vector2();
				_index = -1;
				_previousGlyph = null;
			}
		}

		public struct StringBuilderGlyphEnumerable : IEnumerable<BitmapFontGlyph>
		{
			private readonly StringBuilderGlyphEnumerator _enumerator;

			public StringBuilderGlyphEnumerable(BitmapFont font, StringBuilder text, Vector2? position)
			{
				_enumerator = new StringBuilderGlyphEnumerator(font, text, position);
			}

			public StringBuilderGlyphEnumerator GetEnumerator()
			{
				return _enumerator;
			}

			IEnumerator<BitmapFontGlyph> IEnumerable<BitmapFontGlyph>.GetEnumerator()
			{
				return _enumerator;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return _enumerator;
			}
        }

		public struct StringBuilderGlyphEnumerator : IEnumerator<BitmapFontGlyph>
		{
			private readonly BitmapFont _font;
			private readonly StringBuilder _text;
			private int _index;
			private readonly Vector2 _position;
			private Vector2 _positionDelta;
			private BitmapFontGlyph _currentGlyph;
			private BitmapFontGlyph? _previousGlyph;

			object IEnumerator.Current
			{
				get
				{
					// casting a struct to object will box it, behaviour we want to avoid...
					throw new InvalidOperationException();
				}
			}

			public BitmapFontGlyph Current => _currentGlyph;

            public StringBuilderGlyphEnumerator(BitmapFont font, StringBuilder text, Vector2? position)
			{
				_font = font;
				_text = text;
				_index = -1;
				_position = position ?? new Vector2();
				_positionDelta = new Vector2();
				_currentGlyph = new BitmapFontGlyph();
				_previousGlyph = null;
			}

			public bool MoveNext()
			{
				if (++_index >= _text.Length)
					return false;

				var character = GetUnicodeCodePoint(_text, ref _index);
				_currentGlyph = new BitmapFontGlyph
				{
					Character = character,
					FontRegion = _font.GetCharacterRegion(character),
					Position = _position + _positionDelta
				};

				if (_currentGlyph.FontRegion != null)
				{
					_currentGlyph.Position.X += _currentGlyph.FontRegion.XOffset;
					_currentGlyph.Position.Y += _currentGlyph.FontRegion.YOffset;
					_positionDelta.X += _currentGlyph.FontRegion.XAdvance + _font.LetterSpacing;
				}

				if (UseKernings && _previousGlyph.HasValue && _previousGlyph.Value.FontRegion != null)
				{
					int amount;
					if (_previousGlyph.Value.FontRegion.Kernings.TryGetValue(character, out amount))
					{
						_positionDelta.X += amount;
						_currentGlyph.Position.X += amount;
					}
				}

				_previousGlyph = _currentGlyph;

				if (character != '\n')
					return true;

				_positionDelta.Y += _font.LineHeight;
				_positionDelta.X = _position.X;
				_previousGlyph = null;

				return true;
			}

			private static int GetUnicodeCodePoint(StringBuilder text, ref int index)
			{
				return char.IsHighSurrogate(text[index]) && ++index < text.Length
					? char.ConvertToUtf32(text[index - 1], text[index])
					: text[index];
			}

			public void Dispose()
			{
			}

			public void Reset()
			{
				_positionDelta = new Vector2();
				_index = -1;
				_previousGlyph = null;
			}
		}
	}

	public struct BitmapFontGlyph
	{
		public int Character;
		public Vector2 Position;
		public BitmapFontRegion FontRegion;
	}

	public class BitmapFontRegion
	{
		public BitmapFontRegion(Texture2D texture, Rectangle region, int character, int xOffset, int yOffset, int xAdvance)
		{
			Texture = texture;
			Region = region;
			Character = character;
			XOffset = xOffset;
			YOffset = yOffset;
			XAdvance = xAdvance;
			Kernings = new Dictionary<int, int>();
		}

		public int Character { get; }
		public Texture2D Texture { get; }
		public Rectangle Region { get; }
		public int XOffset { get; }
		public int YOffset { get; }
		public int XAdvance { get; }
		public int Width => Region.Width;
		public int Height => Region.Height;
		public Dictionary<int, int> Kernings { get; }

		public override string ToString()
		{
			return $"{Convert.ToChar(Character)} {Region}";
		}
	}

	public static class BitmapFontExtensions
	{
		public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color, Rectangle? clippingRectangle = null)
		{
			Draw(spriteBatch, texture, position, sourceRectangle, color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0, clippingRectangle);
		}

		public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color,
			float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, Rectangle? clippingRectangle = null)
		{
			var bounds = sourceRectangle;

			if (clippingRectangle.HasValue)
			{
				var x = (int)(position.X - origin.X);
				var y = (int)(position.Y - origin.Y);
				var width = (int)(sourceRectangle.Width * scale.X);
				var height = (int)(sourceRectangle.Height * scale.Y);
				var destinationRectangle = new Rectangle(x, y, width, height);

				bounds = ClipSourceRectangle(sourceRectangle, destinationRectangle, clippingRectangle.Value);
				position.X += bounds.X - sourceRectangle.X;
				position.Y += bounds.Y - sourceRectangle.Y;

				if (bounds.Width <= 0 || bounds.Height <= 0)
					return;
			}

			spriteBatch.Draw(texture, position, bounds, color, rotation, origin, scale, effects, layerDepth);
		}

		public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRectangle, Rectangle destinationRectangle, Color color, Rectangle? clippingRectangle)
		{
			if (clippingRectangle.HasValue)
			{
				sourceRectangle = ClipSourceRectangle(sourceRectangle, destinationRectangle, clippingRectangle.Value);
				destinationRectangle = ClipDestinationRectangle(destinationRectangle, clippingRectangle.Value);
			}

			if (destinationRectangle.Width > 0 && destinationRectangle.Height > 0)
				spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
		}

		private static Rectangle ClipSourceRectangle(Rectangle sourceRectangle, Rectangle destinationRectangle, Rectangle clippingRectangle)
		{
			var left = (float)(clippingRectangle.Left - destinationRectangle.Left);
			var right = (float)(destinationRectangle.Right - clippingRectangle.Right);
			var top = (float)(clippingRectangle.Top - destinationRectangle.Top);
			var bottom = (float)(destinationRectangle.Bottom - clippingRectangle.Bottom);
			var x = left > 0 ? left : 0;
			var y = top > 0 ? top : 0;
			var w = (right > 0 ? right : 0) + x;
			var h = (bottom > 0 ? bottom : 0) + y;

			var scaleX = (float)destinationRectangle.Width / sourceRectangle.Width;
			var scaleY = (float)destinationRectangle.Height / sourceRectangle.Height;
			x /= scaleX;
			y /= scaleY;
			w /= scaleX;
			h /= scaleY;

			return new Rectangle((int)(sourceRectangle.X + x), (int)(sourceRectangle.Y + y), (int)(sourceRectangle.Width - w), (int)(sourceRectangle.Height - h));
		}

		private static Rectangle ClipDestinationRectangle(Rectangle destinationRectangle, Rectangle clippingRectangle)
		{
			var left = clippingRectangle.Left < destinationRectangle.Left ? destinationRectangle.Left : clippingRectangle.Left;
			var top = clippingRectangle.Top < destinationRectangle.Top ? destinationRectangle.Top : clippingRectangle.Top;
			var bottom = clippingRectangle.Bottom < destinationRectangle.Bottom ? clippingRectangle.Bottom : destinationRectangle.Bottom;
			var right = clippingRectangle.Right < destinationRectangle.Right ? clippingRectangle.Right : destinationRectangle.Right;
			return new Rectangle(left, top, right - left, bottom - top);
		}
		/// <summary>
		///     Adds a string to a batch of sprites for rendering using the specified font, text, position, color, rotation,
		///     origin, scale, effects and layer.
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="bitmapFont">A font for displaying text.</param>
		/// <param name="text">The text message to display.</param>
		/// <param name="position">The location (in screen coordinates) to draw the text.</param>
		/// <param name="color">
		///     The <see cref="Color" /> to tint a sprite. Use <see cref="Color.White" /> for full color with no
		///     tinting.
		/// </param>
		/// <param name="rotation">Specifies the angle (in radians) to rotate the text about its origin.</param>
		/// <param name="origin">The origin for each letter; the default is (0,0) which represents the upper-left corner.</param>
		/// <param name="scale">Scale factor.</param>
		/// <param name="effect">Effects to apply.</param>
		/// <param name="layerDepth">
		///     The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer.
		///     Use SpriteSortMode if you want sprites to be sorted during drawing.
		/// </param>
		/// <param name="clippingRectangle">Clips the boundaries of the text so that it's not drawn outside the clipping rectangle</param>
		public static void DrawString(this SpriteBatch spriteBatch, BitmapFont bitmapFont, string text, Vector2 position,
			Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float layerDepth, Rectangle? clippingRectangle = null)
		{
			if (text == null)
				throw new ArgumentNullException(nameof(text));
			if (effect != SpriteEffects.None)
				throw new NotSupportedException($"{effect} is not currently supported for {nameof(BitmapFont)}");

			var glyphs = bitmapFont.GetGlyphs(text, position);
			foreach (var glyph in glyphs)
			{
				if (glyph.FontRegion == null)
					continue;
				var characterOrigin = position - glyph.Position + origin;
				spriteBatch.Draw(glyph.FontRegion.Texture, position, glyph.FontRegion.Region, color, rotation, characterOrigin, scale, effect, layerDepth, clippingRectangle);
			}
		}

		public static void DrawString(this SpriteBatch spriteBatch, BitmapFont bitmapFont, StringBuilder text, Vector2 position,
			Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float layerDepth, Rectangle? clippingRectangle = null)
		{
			if (text == null)
				throw new ArgumentNullException(nameof(text));
			if (effect != SpriteEffects.None)
				throw new NotSupportedException($"{effect} is not currently supported for {nameof(BitmapFont)}");

			var glyphs = bitmapFont.GetGlyphs(text, position);
			foreach (var glyph in glyphs)
			{
				if (glyph.FontRegion == null)
					continue;
				var characterOrigin = position - glyph.Position + origin;
				spriteBatch.Draw(glyph.FontRegion.Texture, position, glyph.FontRegion.Region, color, rotation, characterOrigin, scale, effect, layerDepth, clippingRectangle);
			}
		}

		/// <summary>
		///     Adds a string to a batch of sprites for rendering using the specified font, text, position, color, rotation,
		///     origin, scale, effects and layer.
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="font">A font for displaying text.</param>
		/// <param name="text">The text message to display.</param>
		/// <param name="position">The location (in screen coordinates) to draw the text.</param>
		/// <param name="color">
		///     The <see cref="Color" /> to tint a sprite. Use <see cref="Color.White" /> for full color with no
		///     tinting.
		/// </param>
		/// <param name="rotation">Specifies the angle (in radians) to rotate the text about its origin.</param>
		/// <param name="origin">The origin for each letter; the default is (0,0) which represents the upper-left corner.</param>
		/// <param name="scale">Scale factor.</param>
		/// <param name="effect">Effects to apply.</param>
		/// <param name="layerDepth">
		///     The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer.
		///     Use SpriteSortMode if you want sprites to be sorted during drawing.
		/// </param>
		/// <param name="clippingRectangle">Clips the boundaries of the text so that it's not drawn outside the clipping rectangle</param>
		public static void DrawString(this SpriteBatch spriteBatch, BitmapFont font, string text, Vector2 position,
			Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, float layerDepth, Rectangle? clippingRectangle = null)
		{
			DrawString(spriteBatch, font, text, position, color, rotation, origin, new Vector2(scale, scale), effect, layerDepth, clippingRectangle);
		}

		public static void DrawString(this SpriteBatch spriteBatch, BitmapFont font, StringBuilder text, Vector2 position,
			Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, float layerDepth, Rectangle? clippingRectangle = null)
		{
			DrawString(spriteBatch, font, text, position, color, rotation, origin, new Vector2(scale, scale), effect, layerDepth, clippingRectangle);
		}

		/// <summary>
		///     Adds a string to a batch of sprites for rendering using the specified font, text, position, color, layer,
		///     and width (in pixels) where to wrap the text at.
		/// </summary>
		/// <remarks>
		///     <see cref="BitmapFont" /> objects are loaded from the Content Manager. See the <see cref="BitmapFont" /> class for
		///     more information.
		///     Before any calls to this method you must call <see cref="SpriteBatch.Begin" />. Once all calls 
		///     are complete, call <see cref="SpriteBatch.End" />.
		///     Use a newline character (\n) to draw more than one line of text.
		/// </remarks>
		/// <param name="spriteBatch"></param>
		/// <param name="font">A font for displaying text.</param>
		/// <param name="text">The text message to display.</param>
		/// <param name="position">The location (in screen coordinates) to draw the text.</param>
		/// <param name="color">
		///     The <see cref="Color" /> to tint a sprite. Use <see cref="Color.White" /> for full color with no
		///     tinting.
		/// </param>
		/// <param name="layerDepth">
		///     The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer.
		///     Use SpriteSortMode if you want sprites to be sorted during drawing.
		/// </param>
		/// <param name="clippingRectangle">Clips the boundaries of the text so that it's not drawn outside the clipping rectangle</param>
		public static void DrawString(this SpriteBatch spriteBatch, BitmapFont font, string text, Vector2 position, Color color, float layerDepth, Rectangle? clippingRectangle = null)
		{
			DrawString(spriteBatch, font, text, position, color, rotation: 0, origin: Vector2.Zero, scale: Vector2.One, effect: SpriteEffects.None,
				layerDepth: layerDepth, clippingRectangle: clippingRectangle);
		}

		/// <summary>
		///     Adds a string to a batch of sprites for rendering using the specified font, text, position, color,
		///     and width (in pixels) where to wrap the text at. The text is drawn on layer 0f.
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="font">A font for displaying text.</param>
		/// <param name="text">The text message to display.</param>
		/// <param name="position">The location (in screen coordinates) to draw the text.</param>
		/// <param name="color">
		///     The <see cref="Color" /> to tint a sprite. Use <see cref="Color.White" /> for full color with no
		///     tinting.
		/// </param>
		/// <param name="clippingRectangle">Clips the boundaries of the text so that it's not drawn outside the clipping rectangle</param>
		public static void DrawString(this SpriteBatch spriteBatch, BitmapFont font, string text, Vector2 position, Color color, Rectangle? clippingRectangle = null)
		{
			DrawString(spriteBatch, font, text, position, color, rotation: 0, origin: Vector2.Zero, scale: Vector2.One, effect: SpriteEffects.None,
				layerDepth: 0, clippingRectangle: clippingRectangle);
		}

		public static void DrawString(this SpriteBatch spriteBatch, BitmapFont font, StringBuilder text, Vector2 position, Color color, Rectangle? clippingRectangle = null)
		{
			DrawString(spriteBatch, font, text, position, color, rotation: 0, origin: Vector2.Zero, scale: Vector2.One, effect: SpriteEffects.None,
				layerDepth: 0, clippingRectangle: clippingRectangle);
		}
	}
}