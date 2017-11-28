using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace AvoidanceTrainingGame
{
	public partial class FormMain : Form
	{
		Dictionary<string, Font> fonts = new Dictionary<string, Font>();
		Scene currentScene = null;

		// constructor
		public FormMain()
		{
			InitializeComponent();
		}

		// winform event handlers
		private void FormMain_Load(object sender, EventArgs e)
		{
			this.ClientSize = new Size(400, 400);

			StartNewScene(Scene.SceneType.StartScreen);
		}
		private void FormMain_Paint(object sender, PaintEventArgs e)
		{
			Debug.Assert(currentScene != null);

			currentScene.Render(e.Graphics);
		}
		private void FormMain_MouseMove(object sender, MouseEventArgs e)
		{

		}
		private void FormMain_KeyDown(object sender, KeyEventArgs e)
		{
			Debug.Assert(currentScene != null);

			currentScene.KeyDown(e.KeyCode);
		}
		private void FormMain_KeyUp(object sender, KeyEventArgs e)
		{
			Debug.Assert(currentScene != null);

			currentScene.KeyUp(e.KeyCode);
		}
		private void timerMain_Tick(object sender, EventArgs e)
		{
			Debug.Assert(currentScene != null);

			currentScene.Update();

			this.Invalidate();
		}

		// main procedures
		private void StartNewScene(Scene.SceneType scene, object data = null)
		{
			Scene s = null;
			switch (scene)
			{
				case Scene.SceneType.StartScreen:
					s = new StartScreenScene(this.ClientSize.Width, this.ClientSize.Height);
					break;
				case Scene.SceneType.GamePlay:
					s = new GamePlayScene(this.ClientSize.Width, this.ClientSize.Height, data);
					break;
				case Scene.SceneType.Intermission:
					s = new IntermissionScene(this.ClientSize.Width, this.ClientSize.Height, data);
					break;
				case Scene.SceneType.GameOver:
					s = new GameOverScene(this.ClientSize.Width, this.ClientSize.Height);
					break;
			}
			s.OnLeave += Scene_OnLeave;
			s.Init();
			this.currentScene = s;
		}

		// scene event handlers
		private void Scene_OnLeave(object sender, Scene.SceneType nextScene, object data)
		{
			StartNewScene(nextScene, data);
		}

	}
}
