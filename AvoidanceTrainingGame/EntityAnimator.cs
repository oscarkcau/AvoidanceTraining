using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvoidanceTrainingGame
{
	class EntityAnimator
	{
		// private Animation class
		private class Animation
		{
			// private fields
			private Func<float> getter;
			private Action<float> setter;
			private float sourceValue;
			private float targetValue;
			private int nFrames;
			private int currentFrame = -1;	
			
			// public properties
			public Animation Next { get; set; }
			public bool IsReset { get { return currentFrame == -1; } }
			public bool IsPlaying { get { return (currentFrame >= 0 && currentFrame < nFrames); } }
			public bool IsCompleted { get { return currentFrame == nFrames; } }
			// constructor
			public Animation(Func<float> getter, Action<float> setter, float targetValue, int frames)
			{
				this.getter = getter;
				this.setter = setter;
				this.sourceValue = 0;
				this.targetValue = targetValue;
				this.nFrames = frames;
				this.Next = null;
			}
			public Animation(Action<float> setter, float sourceValue, float targetValue, int frames)
			{
				this.getter = null;
				this.setter = setter;
				this.sourceValue = sourceValue;
				this.targetValue = targetValue;
				this.nFrames = frames;
				this.Next = null;
			}

			// public methods
			public void Reset()
			{
				currentFrame = -1;
			}
			public void Start()
			{
				if (getter != null)
					this.sourceValue = getter();
			}
			public void Update()
			{
				if (currentFrame == nFrames) throw new InvalidOperationException();

				currentFrame++;
				float newValue = sourceValue + (targetValue - sourceValue) * ((float)currentFrame / (float)nFrames);
				this.setter(newValue);
			}
		}

		// private fields
		private List<Animation> Animations = new List<Animation>();
		private int currentStep = 0;

		// public properties
		public bool IsCompleted { get { return currentStep == Animations.Count; } }

		// constructor
		public EntityAnimator()
		{

		}

		// public methods
		public void Reset()
		{
			currentStep = 0;
			foreach (Animation _a in Animations)
			{
				Animation a = _a;
				do
				{
					a.Reset();
					a = a.Next;
				}
				while (a != null);
			}
		}
		public void Update()
		{
			if (currentStep == Animations.Count) throw new InvalidOperationException();

			Animation a = Animations[currentStep];
			bool finished = true;

			do
			{
				if (a.IsReset)
				{
					a.Start();
				}
				if (a.IsCompleted == false)
				{
					a.Update();
					finished = false;
				}
				a = a.Next;
			} while (a != null);

			if (finished) currentStep++;
		}

		public void AddAnimation(Func<float> getter, Action<float> setter, float targetValue, int frames, bool startWithPrevious = false)
		{
			Animation a = new Animation(getter, setter, targetValue, frames);

			if (startWithPrevious == false)
			{
				Animations.Add(a);
			}
			else
			{
				int count = Animations.Count;
				if (count == 0) throw new ArgumentException();

				a.Next = Animations[count - 1];
				Animations[count - 1] = a;
			}
		}
		public void AddAnimation(Action<float> setter, float sourceValue, float targetValue, int frames, bool startWithPrevious = false)
		{
			Animation a = new Animation(setter, sourceValue, targetValue, frames);

			if (startWithPrevious == false)
			{
				Animations.Add(a);
			}
			else
			{
				int count = Animations.Count;
				if (count == 0) throw new ArgumentException();

				a.Next = Animations[count - 1];
				Animations[count - 1] = a;
			}
		}
		public void AddPause(int frames, bool startWithPrevious = false)
		{
			this.AddAnimation(f => { }, 0, 100, frames, startWithPrevious);
		}
	}
}
