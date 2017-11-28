using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace AvoidanceTrainingGame
{
	class Scene
	{
		public enum SceneType { StartScreen, GamePlay, Intermission, GameOver };

		// public events
		public delegate void OnLeaveEventHandler (object sender, SceneType nextScene, object data=null);
		public event OnLeaveEventHandler OnLeave;

		// public properties
		public string Id { get; private set; }
		public Dictionary<string, Entity> NamedEntities { get; private set; }
		public List<Entity> Entities { get; private set; }

		// private fields
		private dynamic config = null;
		protected List<Entity> temporaryRemovedEntities = new List<Entity>();

		// constructor
		public Scene(string id)
		{
			// initialize properties
			this.Id = id;
			this.NamedEntities = new Dictionary<string, Entity>();
			this.Entities = new List<Entity>();

			// load config
			if (this.config == null)
				this.config = Program.LoadJsonConfig(id + ".json");

			// create entities defined in config file
			foreach (var item in this.config)
			{
				if (item.entity == null) continue;

				Entity e = null; 

				if (item.entity == "TextEntity")
					e = TextEntity.CreateFromConfig(item);

				if (item.entity == "SpriteEntity")
					e = SpriteEntity.CreateFromConfig(item);

				if (item.entity == "BoxEntity")
					e = BoxEntity.CreateFromConfig(item);

				if (item.motion != null)
					e.Motion = MotionComponent.CreateFromConfig(e, item.motion);

				this.AddEntity(e);
			}
		}
	
		// public methods
		public void AddEntity(Entity e)
		{
			// add to entity list
			Entities.Add(e);

			// add to entity dictionary if its id is not empty
			if (e.Id != null)
			{
				if (NamedEntities.ContainsKey(e.Id)) throw new ArgumentException();

				NamedEntities.Add(e.Id, e);
			}
		}
		public void MoveEntityToFront(Entity e)
		{
			int n = this.Entities.Count;
			int i = 0;
			while (i < n && this.Entities[i] != e) i++;

			if (i == n) throw new ArgumentException();

			for (int j = i; j+1<n; j++)
			{
				this.Entities[j] = this.Entities[j + 1];
			}

			this.Entities[n - 1] = e;
		}

		// virtual methods
		virtual public void Init() { }
		virtual public void Update()
		{
			// pack all living entity to the front of entity list
			// and remove unused named entities from dictionary
			
			temporaryRemovedEntities.Clear();
			int runningIndex = 0;
			for (int i = 0; i < Entities.Count; i++)
			{
				Entity e = Entities[i];
				if (e.CanRemove || (e.RemoveTime != null && Program.CurrentTime > e.RemoveTime))
				{
					if (e.Id != null) NamedEntities.Remove(e.Id);

					temporaryRemovedEntities.Add(e);
				}
				else
				{
					Entities[runningIndex++] = e; ;
				}
			}

			// resize the list to the size of living entities
			if (runningIndex < Entities.Count)
			{
				Entities.RemoveRange(runningIndex, Entities.Count - runningIndex);
			}

			// raise OnRemove event from removed entities
			foreach (var e in temporaryRemovedEntities) e.RaiseOnRemove();
			temporaryRemovedEntities.Clear();

			// update all living entities
			foreach (Entity e in Entities)
				e.Update();
		}
		virtual public void Render(Graphics g)
		{
			g.Clear(Color.Black);

			foreach (Entity e in Entities)
				e.Render(g);
		}
		virtual public void KeyDown(Keys k)
		{

		}
		virtual public void KeyUp(Keys k)
		{

		}

		// protected methods
		protected void RaiseOnLeave(SceneType nextScene, object data=null)
		{
			if (OnLeave != null)
			{
				OnLeave(this, nextScene, data);
			}
		}
	}
}
