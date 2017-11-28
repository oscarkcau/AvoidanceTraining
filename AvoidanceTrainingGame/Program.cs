using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace AvoidanceTrainingGame
{
	static class Program
	{
		// private static fields
		private static Stopwatch stopWatch = new Stopwatch();
		private static Random rand = new Random();

		// public enum
		public enum GameObjectType { Unknown = 0, Alien, WarmingArrow, Bullet, HomingBullet, Laser, Bomb };
		public enum BulletState { Normal = 1, Approached };

		// public static properties
		public static Random Rand { get { return rand; } }
		public static long CurrentTime { get { return stopWatch.ElapsedMilliseconds; } }
		public static Dictionary<string, Font> Fonts { get; private set; }
		public static Dictionary<string, Bitmap> Sprites { get; private set; } 

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Program.stopWatch.Start();
			Program.Fonts = new Dictionary<string, Font>();
			Program.Sprites = new Dictionary<string, Bitmap>();

			InitFonts();
			LoadSprites();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FormMain());
		}

		// public static methods
		public static dynamic LoadJsonConfig(string filename)
		{
			string full_filename = AppDomain.CurrentDomain.BaseDirectory + filename;
			if (File.Exists(full_filename) == false) throw new ArgumentException();

			using (StreamReader file = File.OpenText(filename))
			using (JsonTextReader reader = new JsonTextReader(file))
			{
				JsonSerializer serializer = new JsonSerializer();
				return serializer.Deserialize(reader);
			}
		}

		// private static methods
		private static void InitFonts()
		{
			Fonts["title"] = new Font("Munro", 24, FontStyle.Bold); ;
			Fonts["heading"] = new Font("Munro Small", 12, FontStyle.Bold); ;
			Fonts["default"] = new Font("Munro Small", 12, FontStyle.Regular);
			Fonts["huge"] = new Font("Munro", 128, FontStyle.Regular);
		}
		private static void LoadSprites()
		{
			string sprites_folder = AppDomain.CurrentDomain.BaseDirectory + "sprites";
			DirectoryInfo d = new DirectoryInfo(sprites_folder);

			foreach (var file in d.GetFiles("*.png"))
			{
				Bitmap bmp = new Bitmap(file.FullName);
				string filename = Path.GetFileNameWithoutExtension(file.Name);
				Sprites.Add(filename, bmp);
			}
		}
	}
}
