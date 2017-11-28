using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AvoidanceTrainingGame
{
	class GameOverScene : Scene
	{
		public GameOverScene(float width, float height) : base("gameover")
		{

		}

		public override void KeyDown(Keys k)
		{
			if (k == Keys.Enter)
			{
				RaiseOnLeave(SceneType.GamePlay);
			}
		}
	}
}
