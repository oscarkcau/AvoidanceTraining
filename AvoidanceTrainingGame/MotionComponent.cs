using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvoidanceTrainingGame
{
	class MotionComponent
	{
		// public enum
		public enum EasingType {
			Linear = 0,
			EaseInQuad, EaseOutQuad, EaseInOutQuad,
			EaseInCubic, EaseOutCubic, EaseInOutCubic,
			EaseInQuart, EaseOutQuart, EaseInOutQuart,
			EaseInElastic, EaseOutElastic, EaseInOutElastic
		}

		// private helping class for easing motion
		private class Easing
		{
			public static List<Func<float, float>> EasingFunctions = new List<Func<float, float>>
			{
				t => t,
				t => (t*t), t => t*(2-t), t => (t<.5f ? 2*t*t : -1+(4-2*t)*t),
				t => (t*t*t), t => ((--t)*t*t+1), t =>(t<.5f ? 4*t*t*t : (t-1)*(2*t-2)*(2*t-2)+1),
				t => (t*t*t*t), t => (1-(--t)*t*t*t), t =>(t<.5 ? 8*t*t*t*t : 1-8*(--t)*t*t*t),
				t => (float)(.04 * t / (--t) * Math.Sin(25 * t)),
				t => (float)((.04 - .04 / t) * Math.Sin(25 * t) + 1),
				t => (float)((t -= .5f) < 0 ? (.02 + .01 / t) * Math.Sin(50 * t) : (.02 - .01 / t) * Math.Sin(50 * t) + 1)
			};

			// public fields
			public Vector Source, Target;
			public float Parameter, StepSize;
			public int StepLeft;
			public EasingType Type;

			// constructor
			public Easing(Vector source, Vector target, int step, EasingType type)
			{
				this.Source = source;
				this.Target = target;
				this.StepLeft = step;
				this.Type = type;
				this.Parameter = 0;
				this.StepSize = 1.0f / step;
			}

			// public methods
			public bool Completed()
			{
				return StepLeft == 0;
			}
			public Vector ComputeNextPosition()
			{
				if (StepLeft == 0) throw new Exception();

				StepLeft--;
				Parameter += StepSize;

				return Source + (Target - Source) * EasingFunctions[(int)Type](Parameter);
			}
		}

		// private field
		private Entity entity;
		private Easing easing = null;

		// public properties
		public Vector Speed { get; set; }
		public float SpeedX { get { return Speed.X; } set { Speed = new Vector(value, SpeedY); } }
		public float SpeedY { get { return Speed.Y; } set { Speed = new Vector(SpeedX, value); } }
		public bool IsEasing { get { return easing != null; } }

		public MotionComponent(Entity entity)
		{
			this.entity = entity;
			this.Speed = new Vector();
		}

		// public methods
		public void Update()
		{
			if (this.easing != null)
			{
				if (this.easing.Completed() == false)
					this.entity.Position = this.easing.ComputeNextPosition();
				else
					this.easing = null;
			}
			else
			{
				float x = this.entity.X + SpeedX;
				float y = this.entity.Y + SpeedY;
				this.entity.Position = new Vector(x, y);
			}

		}
		public void AddEaseMotion(Vector target, int frames, EasingType type)
		{
			this.easing = new Easing(this.entity.Position, target, frames, type);
		}

		// static method
		static public MotionComponent CreateFromConfig(Entity e, dynamic item)
		{
			float speedX = 0, speedY = 0;
			if (item.speed != null)
			{
				speedX = item.speed.x ?? 0;
				speedY = item.speed.y ?? 0;
			}

			MotionComponent motion = new MotionComponent(e);
			motion.Speed = new Vector(speedX, speedY);
			e.Motion = motion;

			if (item.easing != null)
			{
				Vector target = new Vector((float)item.easing.target.x, (float)item.easing.target.y);
				int frames = item.easing.frames;
				EasingType type = item.easing.type;
				motion.AddEaseMotion(target, frames, type);
			}

			return motion;
		}
		static public Func<float, float> EasingFunction(EasingType type)
		{
			return Easing.EasingFunctions[(int)type];
		}
	}
}
