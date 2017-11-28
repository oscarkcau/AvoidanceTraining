using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AvoidanceTrainingGame
{
	class LaserEntity : Entity
	{
		private enum ShootingState { Warming, Shooting, Closing, Closed };

		// public properties
		public Vector Point1 { get; set; }
		public Vector Point2 { get; set; }
		public Color Color { get; set; }
		public Bitmap Bitmap { get; set; }

		// private fields
		private ShootingState state = ShootingState.Warming;
		private float thickness = 6;
		private int shootingStartStep = 0;
		private int shootingEndStep = 0;
		private Vector laserStartPoint, laserEndPoint;

		// constructor
		public LaserEntity(string id, float x1, float y1, float x2, float y2)
			: base(id, (x1 + x2)/2, (y1 + y2)/ 2, Program.GameObjectType.Laser)
		{
			this.Point1 = new Vector(x1, y1);
			this.Point2 = new Vector(x2, y2);
			this.Bitmap = Program.Sprites["bullet_pink_1"];
		}

		// public methods
		public float DistanceTo(Vector p)
		{
			if (this.state == ShootingState.Warming) return float.MaxValue;

			return MinimumDistance(this.laserStartPoint, this.laserEndPoint, p);
		}

		// override methods
		public override void Update()
		{
			if (this.state == ShootingState.Warming && Program.CurrentTime - this.CreateTime > 2000)
			{
				this.state = ShootingState.Shooting;
			}
			else if (this.state == ShootingState.Shooting && Program.CurrentTime - this.CreateTime > 4000)
			{
				this.state = ShootingState.Closing;
			}
			else if (this.state == ShootingState.Closed)
			{
				this.CanRemove = true;
			}

			base.Update();
		}
		public override void Render(Graphics g)
		{

			if (this.state == ShootingState.Warming)
			{
				g.DrawLine(Pens.Red, Point1.X, Point1.Y, Point2.X, Point2.Y);

				g.FillEllipse(Brushes.DarkRed, Point1.X - 10, Point1.Y - 10, 20, 20);

				return;
			}

			if (this.state == ShootingState.Shooting)
			{
				g.DrawLine(Pens.Red, Point1.X, Point1.Y, Point2.X, Point2.Y);

				float th = this.thickness;
				float len = (Point2 - Point1).Norm();
				int n = (int)(len / th) + 1;
				float step = 1.0f / n;
				float t = 0;
				int end = this.shootingEndStep;

				for (int i = 0; i < end; i++)
				{
					Vector p = Point1 + (Point2 - Point1) * t;
					t += step;

					float left = (p.X - Bitmap.Width / 2);
					float top = (p.Y - Bitmap.Height / 2);
					g.DrawImage(Bitmap, left, top);
				}

				this.laserStartPoint = Point1;
				this.laserEndPoint = Point1 + (Point2 - Point1) * t;

				if (shootingEndStep < n) shootingEndStep += 4;
			}


			if (this.state == ShootingState.Closing)
			{
				float th = this.thickness;
				float len = (Point2 - Point1).Norm();
				int n = (int)(len / th) + 1;
				float step = 1.0f / n;
				int end = n;

				for (int i = shootingStartStep; i < end; i++)
				{
					Vector p = Point1 + (Point2 - Point1) * (i*step);

					float left = (p.X - Bitmap.Width / 2);
					float top = (p.Y - Bitmap.Height / 2);
					g.DrawImage(Bitmap, left, top);
				}

				this.laserStartPoint = Point1 + (Point2 - Point1) * (shootingStartStep * step);
				this.laserEndPoint = Point2;


				if (shootingStartStep < n) shootingStartStep += 4;
				else
				{
					this.state = ShootingState.Closed;
				}
			}
		}

		// private methods
		private float MinimumDistance(Vector v, Vector w, Vector p)
		{
			// Return minimum distance between line segment vw and point p
			Vector vw = w - v;
			float l2 = vw.Dot(vw);                   // i.e. |w-v|^2 -  avoid a sqrt
			if (l2 == 0.0) return p.DistanceFrom(v); // v == w case
													 // Consider the line extending the segment, parameterized as v + t (w - v).
													 // We find projection of point p onto the line. 
													 // It falls where t = [(p-v) . (w-v)] / |w-v|^2
													 // We clamp t from [0,1] to handle points outside the segment vw.

			float tmp = (p - v).Dot(w - v) / l2;
			float t = Math.Max(0, Math.Min(1, tmp));
			Vector projection = v + (w - v) * t;  // Projection falls on the segment
			return p.DistanceFrom(projection); 
		}
	}
}
