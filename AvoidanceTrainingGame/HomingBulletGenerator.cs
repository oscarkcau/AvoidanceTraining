using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AvoidanceTrainingGame
{
	class HomingBulletGenerator
	{
		// private fields
		private Random random = Program.Rand;
		private GamePlayScene scene;
		private Bitmap[] normalSprites = new Bitmap[]
		{
			Program.Sprites["bullet_normal_1"],
			Program.Sprites["bullet_normal_2"],
			Program.Sprites["bullet_normal_3"],
			Program.Sprites["bullet_normal_4"]
		};
		private Bitmap[] approachedSprites = new Bitmap[]
		{
			Program.Sprites["bullet_dark_1"],
			Program.Sprites["bullet_dark_2"],
			Program.Sprites["bullet_dark_3"],
			Program.Sprites["bullet_dark_4"]
		};

		// public properties
		public double GenerationRate { get; set; }
		public float BulletSpeed { get; set; }
		public float WarmingSpriteOffset { get; set; }
		public Bitmap[] NormalSprites { get { return normalSprites; } }
		public Bitmap[] ApproachedSprites { get { return approachedSprites; } }

		// constructor
		public HomingBulletGenerator(GamePlayScene scene)
		{
			this.scene = scene;
			this.GenerationRate = 0.05;
			this.BulletSpeed = 1.0f;
			this.WarmingSpriteOffset = 10;
		}

		// public methods
		public void Update()
		{
			if (random.NextDouble() > GenerationRate) return;

			float w = this.scene.Width;
			float h = this.scene.Height;
			float offset = this.WarmingSpriteOffset;
			int side = random.Next(4);
			int pos = random.Next(100);
			Vector p_warming = new Vector();
			Vector p_bullet = new Vector();
			Bitmap bmp = null;
			switch (side)
			{
				case 0: // top
					p_bullet = new Vector(pos * w / 100.0f, 0);
					p_warming = new Vector(p_bullet.X, offset);
					bmp = Program.Sprites["red_arrow2_up"];
					break;

				case 1: // left
					p_bullet = new Vector(0, pos * h / 100.0f);
					p_warming = new Vector(offset, p_bullet.Y);
					bmp = Program.Sprites["red_arrow2_left"];
					break;

				case 2: // right
					p_bullet = new Vector(w, pos * h / 100.0f);
					p_warming = new Vector(w - offset, p_bullet.Y);
					bmp = Program.Sprites["red_arrow2_right"];
					break;

				case 3: // botton
					p_bullet = new Vector(pos * w / 100.0f, h);
					p_warming = new Vector(p_bullet.X, h - offset);
					bmp = Program.Sprites["red_arrow2_down"];
					break;
			}

			SpriteEntity nn = new SpriteEntity(null, p_warming.X, p_warming.Y, bmp, Program.GameObjectType.WarmingArrow);
			nn.RemoveTime = Program.CurrentTime + 1000;
			nn.OnRemove += OnWarningRemoveEventHandler;
			nn.Tag = p_bullet;
			this.scene.AddEntity(nn);
		}

		// entity event handler
		public void OnWarningRemoveEventHandler(object sender)
		{
			SpriteEntity se = sender as SpriteEntity;

			Vector diff = this.scene.Alien.Position - se.Position;
			Vector sp = new Vector();
			if (diff.Norm() > 0)
			{
				sp = diff.Normalize() * 1f;
			}

			Vector pos = (Vector)se.Tag;
			SpriteEntity bullet = new SpriteEntity(null, pos.X, pos.Y, this.normalSprites, Program.GameObjectType.HomingBullet);
			bullet.State = (int)Program.BulletState.Normal;
			bullet.SpriteUpdateInterval = 3;
			bullet.AddMotionComponent();
			bullet.Motion.Speed = sp;
			bullet.Tag = bullet.CreateTime + 5000;
			//bullet.RemoveTime = bullet.CreateTime + 5000;
			this.scene.AddEntity(bullet);
		}

	}
}
