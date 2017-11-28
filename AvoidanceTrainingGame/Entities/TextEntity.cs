using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace AvoidanceTrainingGame
{
	class TextEntity : Entity
	{
		// public properties
		public string Text { get; set; }
		public Font Font { get; set; }
		public Color FontColor { get; set; }
		public StringAlignment Alignment { get { return stringFormat.Alignment; } set { stringFormat.Alignment = value; } }
		public StringAlignment LineAlignment { get { return stringFormat.LineAlignment; } set { stringFormat.LineAlignment = value; } }
		public RectangleF? Layout { get; set; }

		// private field
		StringFormat stringFormat = new StringFormat();

		// constructor
		public TextEntity(string id, string text = "", float x=0, float y=0, Font font=null, Color? color=null)
			: base(id, x, y)
		{
			Text = text;
			Font = font;
			FontColor = color ?? Color.White;
			stringFormat.Alignment = StringAlignment.Center;
			stringFormat.LineAlignment = StringAlignment.Center;
			Layout = null;
		}

		// override methods
		public override void Render(Graphics g)
		{
			using (SolidBrush sb = new SolidBrush(this.FontColor))
			{
				if (this.Layout != null)
				{
					RectangleF r = (RectangleF)this.Layout;
					r.X = this.X - r.Width / 2;
					r.Y = this.Y - r.Height / 2;
					g.DrawString(this.Text, this.Font, sb, r, this.stringFormat);
				}
				else
				{
					g.DrawString(this.Text, this.Font, sb, this.X, this.Y, this.stringFormat);
				}
			}
		}

		// static methods
		static public TextEntity CreateFromConfig(dynamic item)
		{
			string id = item.id;
			string text = item.text ?? "TextEntity";
			float x = item.x ?? 0;
			float y = item.y ?? 0;
			Font font = (item.font == null) ? Program.Fonts["default"] : Program.Fonts[(string)item.font];
			Color color = (item.color == null) ? Color.White : Color.FromName((string)item.color);

			StringAlignment alignment = ToStringAlignment(item.alignment);
			StringAlignment lineAlignment = ToStringAlignment(item.line_alignment);

			TextEntity t = new TextEntity(id, text, x, y, font, color);
			t.Alignment = alignment;
			t.LineAlignment = lineAlignment;

			if (item.layout != null)
			{
				float w = item.layout.width;
				float h = item.layout.height;
				float rx = x - w / 2;
				float ry = y - h / 2;

				t.Layout = new RectangleF
					(
					rx,
					ry,
					w,
					h
					);
			}

			return t;
		}

		// private helper methods
		private static StringAlignment ToStringAlignment(dynamic d)
		{
			return (d == null) ? StringAlignment.Center : (StringAlignment)Enum.Parse(typeof(StringAlignment), (string)d);
		} 
	}
}
