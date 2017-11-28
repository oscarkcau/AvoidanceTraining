using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AvoidanceTrainingGame
{
	class RectEntity : Entity
	{
		// public properties
		public float Left { get { return X - Width / 2; } }
		public float Top { get { return Y - Height / 2; } }
		public float Width { get; set; }
		public float Height { get; set; }
		public Color Color { get; set; }

		// constructor
		public RectEntity(string id, float x, float y, float width, float height, Color? color = null)
			: base(id, x, y)
		{
			this.Width = width;
			this.Height = height;
			this.Color = color ?? Color.Gray;
		}

		// override methods
		public override void Render(Graphics g)
		{
			using (Brush brush = new SolidBrush(this.Color))
			{
				float x = this.X - this.Width / 2;
				float y = this.Y - this.Height / 2;
				g.FillRectangle(brush, x, y, this.Width, this.Height);
			}
		}
	}
}
