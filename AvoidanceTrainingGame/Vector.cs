using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvoidanceTrainingGame
{
	// a simple 2D vector class
	struct Vector
	{
		public float X { get; private set; }
		public float Y { get; private set; }

		// constructor
		public Vector(float x=0, float y=0)
		{
			this.X = x;
			this.Y = y;
		}

		// overloadded operators
		public static Vector operator +(Vector v1, Vector v2)
		{
			return new Vector(v1.X + v2.X, v1.Y + v2.Y);
		}
		public static Vector operator -(Vector v1, Vector v2)
		{
			return new Vector(v1.X - v2.X, v1.Y - v2.Y);
		}
		public static Vector operator *(Vector v, float f)
		{
			return new Vector(v.X * f, v.Y * f);
		}
		public static Vector operator /(Vector v, float f)
		{
			return new Vector(v.X / f, v.Y / f);
		}
		
		// public methods
		public float Norm()
		{
			float tmp = X * X + Y * Y;
			if (tmp == 0f) return 0;
			return (float)Math.Sqrt(tmp);
		}
		public float DistanceFrom(Vector v)
		{
			return (this - v).Norm();
		}
		public Vector Normalize()
		{
			float norm = this.Norm();
			if (norm > 0) return this / norm;
			else throw new DivideByZeroException();
		}
		public Vector SafeNormalize()
		{
			float norm = this.Norm();
			if (norm > 0) return this / norm;
			else return this;
		}
		public float Dot(Vector v)
		{
			return this.X * v.X + this.Y * v.Y;
		}
	}
}
