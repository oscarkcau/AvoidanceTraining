using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AvoidanceTrainingGame
{
	class IntermissionScene : Scene
	{
		// private enum
		private enum UserOption { Undefined, Option1, Option2 };

		// private fields
		private UserOption userOption = UserOption.Undefined;
		private GamePlayScene lastGamePlayScene = null;

		private float bulletSpeed;
		private double bulletRate;

		// constructor
		public IntermissionScene(float width, float height, object data)
			: base("intermission")
		{
			this.lastGamePlayScene = data as GamePlayScene;
			bulletSpeed = lastGamePlayScene.NormalBulletGenerator.BulletSpeed;
			bulletRate = lastGamePlayScene.NormalBulletGenerator.GenerationRate;
		}

		// override methods
		public override void Update()
		{
			if (NamedEntities["option_1_box"].Motion.IsEasing == false)
			{
				NamedEntities["triangle_up"].Visible = true;
				NamedEntities["triangle_down"].Visible = true;
			}

			base.Update();
		}
		public override void KeyDown(Keys k)
		{
			// accept user's input after animation is finished
			if (NamedEntities["option_1_box"].Motion.IsEasing == true) return;

			if (k == Keys.Up)
			{
				userOption = UserOption.Option1;
				((BoxEntity)NamedEntities["option_1_box"]).Color = Color.White;
				((BoxEntity)NamedEntities["option_2_box"]).Color = Color.Gray;
			}
			else if (k == Keys.Down)
			{
				userOption = UserOption.Option2;
				((BoxEntity)NamedEntities["option_1_box"]).Color = Color.Gray;
				((BoxEntity)NamedEntities["option_2_box"]).Color = Color.White;
			}
			else if (k == Keys.Enter)
			{
				if (userOption != UserOption.Undefined)
				{
					if (userOption == UserOption.Option1) bulletSpeed += 0.1f;
					if (userOption == UserOption.Option2) bulletRate += 0.01f;

					RaiseOnLeave(SceneType.GamePlay, new { Speed = bulletSpeed, Rate = bulletRate } );
				}
			}
		}
	}
}
