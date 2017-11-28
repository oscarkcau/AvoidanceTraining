using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AvoidanceTrainingGame
{
	class Entity
	{
		public delegate void OnRemoveEventHandler(object sender);
		public event OnRemoveEventHandler OnRemove;

		// public properties
		public string Id { get; private set; }
		public Program.GameObjectType ObjectType { get; set; }
		public int State { get; set; }
		public object Tag { get; set; }
		public Vector Position { get; set; }
		public float X { get { return Position.X; } set { Position = new Vector(value, Y); } }
		public float Y { get { return Position.Y; } set { Position = new Vector(X, value); } }
		public bool CanRemove { get; set; }
		public long CreateTime { get; private set; }
		public long? RemoveTime { get; set; }
		public bool Visible { get; set; }

		public MotionComponent Motion { get; set; }

		// constructor
		public Entity(string id, float x = 0, float y = 0, Program.GameObjectType type = Program.GameObjectType.Unknown)
		{
			this.Id = id;
			this.Position = new Vector(x, y);
			this.CanRemove = false;
			this.ObjectType = Program.GameObjectType.Unknown;
			this.State = 0;
			this.Tag = null;
			this.CreateTime = Program.CurrentTime;
			this.RemoveTime = null;
			this.ObjectType = type;
			this.Visible = true;
		}

		// virtual methods
		virtual public void Update()
		{
			if (this.Motion != null) this.Motion.Update();
		}
		virtual public void Render(Graphics g) { }

		// public methods
		public void RaiseOnRemove()
		{
			if (OnRemove != null)
			{
				OnRemove(this);
			}
		}
		public void AddMotionComponent()
		{
			if (this.Motion == null)
				this.Motion = new MotionComponent(this);
		}
		public bool IsInRect(float left, float top, float width, float height)
		{
			return (
				this.X >= left &&
				this.X < left + width &&
				this.Y >= top &&
				this.Y < top + height);
		}
		public void ConstrainInRect(float left, float top, float width, float height)
		{
			if (this.X < left) this.X = left;
			if (this.X > left + width) this.X = left + width;
			if (this.Y < top) this.Y = top;
			if (this.Y > top + height) this.Y = top + height;
		}
	}
}
