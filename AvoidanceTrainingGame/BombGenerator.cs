using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvoidanceTrainingGame
{
	class BombGenerator
	{
		// private fields
		private Random random = Program.Rand;
		private GamePlayScene scene;
		private double runningGenerationRate;

		// public properties
		public double GenerationRate { get; set; }

		// constructor
		public BombGenerator(GamePlayScene scene)
		{
			this.scene = scene;
			this.GenerationRate = 0.001;
			this.runningGenerationRate = GenerationRate;
		}

		// public methods
		public void Update()
		{
			if (random.NextDouble() > runningGenerationRate)
			{
				runningGenerationRate += 0.0002;
				return;
			}

			this.runningGenerationRate = this.GenerationRate;

			float w = this.scene.Width;
			float h = this.scene.Height;
			int x = random.Next((int)(w * 0.1), (int)(w * 0.9));
			int y = random.Next((int)(h * 0.1), (int)(h * 0.9));

			BombEntity bomb = new BombEntity(null, x, y);

			this.scene.AddEntity(bomb);
		}

	}
}
