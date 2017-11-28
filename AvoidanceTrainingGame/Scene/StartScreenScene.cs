using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


namespace AvoidanceTrainingGame
{
	class StartScreenScene : Scene
	{
		public StartScreenScene(float width, float height)
			: base("start_screen")
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
