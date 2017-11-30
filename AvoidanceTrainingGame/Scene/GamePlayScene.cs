using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace AvoidanceTrainingGame
{
	class GamePlayScene : Scene
	{
		public enum State { Ready, Playing, Completed, Failed };

		// private fields
		bool leftKeyDown = false;
		bool rightKeyDown = false;
		bool upKeyDown = false;
		bool downKeyDown = false;
		float moveSpeed = 2.0f;
		float alienSize = 20;
		float closeDistance = 20;
		long SceneStartTime, PlayStartTime;
		int readyCountdown = 3;
		int levelCountdown = 30;
		int score = 0;
		float attractorFactor = 0.2f;
		State state = State.Ready;
		EntityAnimator animator = null;
 
        // public properties;
        public float Width { get; private set; }
		public float Height { get; private set; }
		public SpriteEntity Alien { get; private set; }
		public NormalBulletGenerator NormalBulletGenerator { get; private set; }
		public HomingBulletGenerator HomingBulletGenerator { get; private set; }
		public LaserGenerator LaserGenerator { get; private set; }
		public BombGenerator BombGenerator { get; private set; }

		// constructor
		public GamePlayScene(float width, float height, object data = null)
			: base("game_play")
		{
			this.Width = width;
			this.Height = height;
			this.Alien = (SpriteEntity)this.NamedEntities["alien"];
			this.NormalBulletGenerator = new NormalBulletGenerator(this);
			this.HomingBulletGenerator = new HomingBulletGenerator(this);
			this.LaserGenerator = new LaserGenerator(this);
			this.BombGenerator = new BombGenerator(this);

            if (data != null)
			{
				dynamic settings = data;
				NormalBulletGenerator.BulletSpeed = settings.Speed;
				NormalBulletGenerator.GenerationRate = settings.Rate;
			}

			TextEntity cdt = this.NamedEntities["countdown_text"] as TextEntity;
			cdt.Text = levelCountdown.ToString();
		}

		// override methods
		public override void Init()
		{
			this.SceneStartTime = Program.CurrentTime;
		}
		public override void Update()
		{
			switch (this.state)
			{
				case State.Ready: Update_Ready(); break;
				case State.Playing: Update_Play(); break;
				case State.Failed: Update_Failed(); break;
				case State.Completed: Update_Completed(); break;
			}

			base.Update();
		}
		public override void KeyDown(Keys k)
		{
			if (this.state != State.Playing) return;

			switch (k)
			{
				case Keys.Left:
					leftKeyDown = true;
					Alien.Motion.SpeedX = -moveSpeed;
					break;
				case Keys.Right:
					rightKeyDown = true;
					Alien.Motion.SpeedX = moveSpeed;
					break;
				case Keys.Up:
					upKeyDown = true;
					Alien.Motion.SpeedY = -moveSpeed;
					break;
				case Keys.Down:
					downKeyDown = true;
					Alien.Motion.SpeedY = moveSpeed;
					break;
			}
		}
		public override void KeyUp(Keys k)
		{
			if (this.state != State.Playing) return;

			switch (k)
			{
				case Keys.Left:
					leftKeyDown = false;
					Alien.Motion.SpeedX = rightKeyDown ? moveSpeed : 0;
					break;
				case Keys.Right:
					rightKeyDown = false;
					Alien.Motion.SpeedX = leftKeyDown ? -moveSpeed : 0;
					break;
				case Keys.Up:
					upKeyDown = false;
					Alien.Motion.SpeedY = downKeyDown ? moveSpeed : 0;
					break;
				case Keys.Down:
					downKeyDown = false;
					Alien.Motion.SpeedY = upKeyDown ? -moveSpeed : 0; ;
					break;
			}
		}

		// private methods
		private void Update_Ready()
		{
			int timePassed = (int)(Program.CurrentTime - this.SceneStartTime) / 1000;
			int remainingReadyTime = this.readyCountdown - timePassed;
			if (remainingReadyTime > 0)
			{
				TextEntity t = this.NamedEntities["ready_countdown_text"] as TextEntity;
				t.Text = remainingReadyTime.ToString();
			}
			else
			{
				TextEntity t = this.NamedEntities["ready_countdown_text"] as TextEntity;
				t.CanRemove = true;
				this.PlayStartTime = Program.CurrentTime;
				this.state = State.Playing;
			}
		}
		private void Update_Play()
		{
			float halfAlienSize = alienSize / 2;

			// update alien
			var alien = NamedEntities["alien"];
			alien.ConstrainInRect(0, 0, this.Width, this.Height);

			// update bullet entities
			foreach (Entity e in this.Entities)
			{
				// update speed for homing bullet
				if (e.ObjectType == Program.GameObjectType.HomingBullet &&
					(long)e.Tag > Program.CurrentTime)
				{
					float sp = e.Motion.Speed.Norm();
					Vector u = Alien.Position - e.Position;
					u = u.SafeNormalize();
					Vector v = e.Motion.Speed.SafeNormalize();
					e.Motion.Speed = (u * attractorFactor + v * (1 - attractorFactor)).SafeNormalize() * sp;
				}

				// update bullets
				if (e.ObjectType == Program.GameObjectType.Bullet ||
					e.ObjectType == Program.GameObjectType.HomingBullet)
				{
					SpriteEntity se = e as SpriteEntity;

					// remove off screen bullets
					if (!se.IsInRect(-50, -50, this.Width + 100, this.Height + 100))
					{
						se.CanRemove = true;
						continue;
					}

					float dis = se.Position.DistanceFrom(this.Alien.Position);
					if (dis < halfAlienSize)
					{
						// goto Failed state
						state = State.Failed;
					}
					else if (se.State == (int)Program.BulletState.Normal && dis < halfAlienSize + this.closeDistance)
					{
						// increase score if bullet is close enough
						score++;
						se.State = (int)Program.BulletState.Approached;
                        /*
						if (e.ObjectType == Program.GameObjectType.Bullet)
							se.Bitmaps = this.NormalBulletGenerator.ApproachedSprites;
						if (e.ObjectType == Program.GameObjectType.HomingBullet)
							se.Bitmaps = this.HomingBulletGenerator.ApproachedSprites;
						*/
                    }
				}

				if (e.ObjectType == Program.GameObjectType.Laser)
				{
					LaserEntity laser = e as LaserEntity;
					float dis = laser.DistanceTo(this.Alien.Position);

					if (dis < halfAlienSize)
					{
						// goto Failed state
						state = State.Failed;
					}
					else if (dis < halfAlienSize + this.closeDistance)
					{
						// increase score if bullet is close enough
						score++;
                        var p1 = new System.Windows.Media.MediaPlayer();
                        p1.Open(new System.Uri(AppDomain.CurrentDomain.BaseDirectory + "wav/click.wav"));
                        p1.Play();
                    }
				}

				if (e.ObjectType == Program.GameObjectType.Bomb)
				{
					BombEntity laser = e as BombEntity;
					float dis = laser.DistanceTo(this.Alien.Position);

					if (dis < halfAlienSize)
					{
						// goto Failed state
						state = State.Failed;
					}
					else if (dis < halfAlienSize + this.closeDistance)
					{
						// increase score if bullet is close enough
						score++;
					}
				}
			}

			// make new bullet randomly
			this.NormalBulletGenerator.Update();
			this.HomingBulletGenerator.Update();
			this.LaserGenerator.Update();
			this.BombGenerator.Update();

			// update time and score text entitye
			int remainingTime = this.levelCountdown - (int)(Program.CurrentTime - this.PlayStartTime) / 1000;
			TextEntity cdt = this.NamedEntities["countdown_text"] as TextEntity;
			cdt.Text = remainingTime.ToString();

			TextEntity st = this.NamedEntities["score_text"] as TextEntity;
			st.Text = this.score.ToString("0000");

			// if time up, goto intermission scene
			if (remainingTime == 0)
			{
				// goto Completed state
				state = State.Completed;
			}
		}
		private void Update_Failed()
		{
			if (this.NamedEntities.ContainsKey("cover_rect") == false)
			{
				this.AddEntity(new RectEntity(
					"cover_rect",
					this.Width / 2, this.Height / 2,
					this.Width + 2, this.Height + 2,
					Color.FromArgb(0, Color.Black)
					));

				// stop all moving objects
				foreach (Entity e in this.Entities)
				{
					if (e.Motion != null)
					{
						e.Motion.Speed = new Vector(0, 0);
					}

					e.RemoveTime = null;
				}
			}

			RectEntity r = this.NamedEntities["cover_rect"] as RectEntity;
			if (r.Color.A < 200)
			{
				r.Color = Color.FromArgb(r.Color.A + 5, r.Color.R, r.Color.G, r.Color.B);
			}
			else
			{
				// goto game over screen
				RaiseOnLeave(SceneType.GameOver);
			}
		}
		private void Update_Completed()
		{
			if (this.NamedEntities.ContainsKey("cover_rect") == false)
			{
				RectEntity rect = new RectEntity(
					"cover_rect",
					this.Width / 2, this.Height / 2,
					this.Width + 2, this.Height + 2,
					Color.FromArgb(0, Color.Black)
					);
				this.AddEntity(rect);

				// stop all moving objects
				foreach (Entity e in this.Entities)
				{
					if (e.Motion != null)
					{
						e.Motion.Speed = new Vector(0, 0);
					}

					e.RemoveTime = null;
				}

				this.MoveEntityToFront(Alien);

				this.animator = new EntityAnimator();
				animator.AddAnimation(a => rect.Color = Color.FromArgb((int)a, rect.Color), 0, 255, 50);
				animator.AddAnimation(x => Alien.X = x, Alien.X, 200, 50, true);
				animator.AddAnimation(y => Alien.Y = y, Alien.Y, Height - 40, 50, true);
				animator.AddPause(20);
				animator.AddAnimation(() => Alien.Y, value => Alien.Y = value, -50, 30);
			}

			if (this.animator != null && this.animator.IsCompleted == false)
			{
				this.animator.Update();
			}

			if (this.animator != null && this.animator.IsCompleted == true)
			{
				// goto intermission scene
				RaiseOnLeave(SceneType.Intermission, this);
			}
		}
	}
}
