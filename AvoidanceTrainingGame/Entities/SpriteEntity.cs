using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace AvoidanceTrainingGame
{
	class SpriteEntity : Entity
	{
		// private fields
		private int spriteUpdateTime = 0;

		// public fields
		public float Width { get; private set; }
		public float Height { get; private set; }
		public Bitmap[] Bitmaps { get; set; }
		public int SpriteIndex { get; set; }
		public int SpriteUpdateInterval { get; set; }

		// constructors
		public SpriteEntity(string id, float x, float y, float w, float h, Bitmap[] bitmaps, Program.GameObjectType type) : base(id, x, y, type)
		{
			this.Width = w;
			this.Height = h;
			this.Bitmaps = bitmaps;
			this.SpriteIndex = 0;
			this.SpriteUpdateInterval = 1;
		}
		public SpriteEntity(string id, float x, float y, Bitmap[] bitmaps, Program.GameObjectType type)
			: this(id, x, y, bitmaps[0].Width, bitmaps[0].Height, bitmaps, type)
		{
		}
		public SpriteEntity(string id, float x, float y, float w, float h, Bitmap bmp, Program.GameObjectType type) 
			: this(id, x, y, w, h, new Bitmap[] { bmp }, type)
		{
		}
		public SpriteEntity(string id, float x, float y, Bitmap bmp, Program.GameObjectType type) 
			: this(id, x, y, bmp.Width, bmp.Height, bmp, type)
		{
		}


		// override methods
		public override void Update()
		{
			if (Bitmaps.Length > 1)
			{
				spriteUpdateTime++;
				if (spriteUpdateTime >= SpriteUpdateInterval)
				{
					SpriteIndex = (SpriteIndex + 1) % Bitmaps.Length;
					spriteUpdateTime = 0;
				}
			}

			base.Update();
		}
		public override void Render(Graphics g)
		{
			if (this.Visible)
				g.DrawImage(Bitmaps[SpriteIndex], X-Width/2, Y-Height/2, Width, Height);
		}

		// static method
		static public SpriteEntity CreateFromConfig(dynamic item)
		{
			string id = item.id;
			float x = item.x ?? 0;
			float y = item.y ?? 0;

			Bitmap bmp = Program.Sprites[(string)item.bmp];
			float w = item.width ?? bmp.Width;
			float h = item.height ?? bmp.Height;
			Program.GameObjectType type = ToGameObjectType(item.type);
			bool visible = item.visible ?? true;

			SpriteEntity se = new SpriteEntity(
				id, x, y, w, h, bmp, type
				);
			se.Visible = visible;

			return se;
		}

		// private helper methods
		private static Program.GameObjectType ToGameObjectType(dynamic d)
		{
			return (d == null) ? Program.GameObjectType.Unknown : 
				(Program.GameObjectType)Enum.Parse(typeof(Program.GameObjectType), (string)d);
		}
	}
}
