using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvoidanceTrainingGame
{
	class BombEntity : Entity
	{
		// private enum
		private enum ShootingState { Warming, Explosing, Closing };

		// static private fields
		private static Func<float, float> easingFunction = MotionComponent.EasingFunction(MotionComponent.EasingType.EaseInCubic);
		private static Bitmap bitmap = Program.Sprites["explosion_3"];

		// private fields
		private ShootingState state = ShootingState.Warming;
		private float radius = 50;
		private float size = 0;

		// constructor
		public BombEntity(string id, float x, float y)
			: base(id, x, y, Program.GameObjectType.Bomb)
		{
		}

		// public methods
		public float DistanceTo(Vector p)
		{
			if (this.state == ShootingState.Warming) return float.MaxValue;

			float d = this.Position.DistanceFrom(p) - this.size * this.radius;

			return d > 0 ? d : 0;
		}

		// override methods
		public override void Update()
		{
			if (this.state == ShootingState.Warming && Program.CurrentTime - this.CreateTime > 3000)
			{
				this.state = ShootingState.Explosing;
			}
			if (this.state == ShootingState.Explosing && Program.CurrentTime - this.CreateTime > 4000)
			{
				this.state = ShootingState.Closing;
			}
			base.Update();
		}
		public override void Render(Graphics g)
		{
			if (state == ShootingState.Warming)
			{
				float thinkness = 5; // thickness of warming cross
				float length = 10;   // half length of warming cross
				
				using (Pen pen = new Pen(Color.Red, thinkness))
				{
					g.DrawLine(pen, X - length, Y - length, X + length, Y + length);
					g.DrawLine(pen, X + length, Y - length, X - length, Y + length);
				}
				using (Pen pen = new Pen(Color.DarkRed, thinkness - 2))
				{
					pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
					g.DrawEllipse(pen, X - radius, Y - radius, radius * 2, radius * 2);
				}
			}

			if (state == ShootingState.Explosing)
			{
				float w = bitmap.Width;
				float h = bitmap.Height;
				float s = easingFunction(this.size) * w;

				g.DrawImage(bitmap, X - s/2, Y - s/2, s, s);

				if (this.size < 1.0f) this.size += 0.1f;
			}

			if (state == ShootingState.Closing)
			{
				float w = bitmap.Width;
				float h = bitmap.Height;
				float s = easingFunction(this.size) * w;
				g.DrawImage(bitmap, X - s / 2, Y - s / 2, s, s);

				if (this.size > 0) this.size -= 0.1f;
				if (this.size <= 0) this.CanRemove = true;
			}
		}
	}
}

