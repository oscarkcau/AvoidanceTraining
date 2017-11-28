using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AvoidanceTrainingGame
{
	class BoxEntity : Entity
	{
		// public properties
		public float Left { get { return X - Width / 2; } }
		public float Top { get { return Y - Height / 2; } }
		public float Width { get; set; }
		public float Height { get; set; }
		public float Thickness { get; set; }
		public Color Color { get; set; }

		public BoxEntity(string id, float x, float y, float width, float height, float thickness, Color? color = null)
			: base(id, x, y)
		{
			this.Width = width;
			this.Height = height;
			this.Thickness = thickness;
			this.Color = color ?? Color.Gray;
		}

		// override methods
		public override void Render(Graphics g)
		{
			using (Brush brush = new SolidBrush(this.Color))
			{
				// left side
				g.FillRectangle(
					brush,
					Left,
					Top + (Thickness * 2),
					Thickness,
					Height - (Thickness * 4)
				);

				// right side
				g.FillRectangle(
					brush,
					Left + Width - Thickness,
					Top + (Thickness * 2),
					Thickness,
					Height - (Thickness * 4)
				);

				// Top side
				g.FillRectangle(
					brush,
					Left + (Thickness * 2),
					Top,
					Width - (Thickness * 4),
					Thickness
				);

				// bottom side
				g.FillRectangle(
					brush,
					Left + (Thickness * 2),
					Top + Height - Thickness,
					Width - (Thickness * 4),
					Thickness
				);

				// Top Left cornor 
				g.FillRectangle(brush, Left + Thickness, Top + Thickness, Thickness, Thickness);

				// Top right cornor 
				g.FillRectangle(brush, Left + Width - Thickness * 2, Top + Thickness, Thickness, Thickness);

				// botton Left cornor 
				g.FillRectangle(brush, Left + Thickness, Top + Height - Thickness * 2, Thickness, Thickness);

				// botton right cornor 
				g.FillRectangle(brush, Left + Width - Thickness * 2, Top + Height - Thickness * 2, Thickness, Thickness);
			}
		}

		// static methods
		static public BoxEntity CreateFromConfig(dynamic item)
		{
			string id = item.id;
			float width = item.width ?? 100;
			float height = item.height ?? 100;
			float thickness = item.thickness ?? 5;
			Color color = (item.color == null) ? Color.Gray : Color.FromName((string)item.color);

			// left/top fields will overwrite x/y if both exist
			float x = item.x ?? 0;
			float y = item.y ?? 0;
			float left = x - width / 2;
			float top = y - height / 2;

			BoxEntity b = new BoxEntity(id, x, y, width, height, thickness, color);

			return b;
		}
	}
}
