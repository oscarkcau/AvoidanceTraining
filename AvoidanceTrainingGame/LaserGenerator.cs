using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvoidanceTrainingGame
{
	class LaserGenerator
	{
		// private fields
		private Random random = Program.Rand;
		private GamePlayScene scene;
		private double runningGenerationRate;

		// public properties
		public double GenerationRate { get; set; }

		// constructor
		public LaserGenerator(GamePlayScene scene)
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
			int side = random.Next(4);
			int pos1 = random.Next(10, 90);
			int pos2 = random.Next(10, 90);

			LaserEntity laser = null;

			switch (side)
			{
				case 0: // top
					laser = new LaserEntity(null, pos1 * w / 100.0f, 0, pos2 * w / 100.0f, h);
					break;
				case 1: // bottom
					laser = new LaserEntity(null, pos1 * w / 100.0f, h, pos2 * w / 100.0f, 0);
					break;
				case 2: // left
					laser = new LaserEntity(null, 0, pos1 * h / 100.0f, w, pos2 * h / 100.0f);
					break;
				case 3: // right
					laser = new LaserEntity(null, w, pos1 * h / 100.0f, 0, pos2 * h / 100.0f);
					break;
			}

			this.scene.AddEntity(laser);
		}
	}
}
