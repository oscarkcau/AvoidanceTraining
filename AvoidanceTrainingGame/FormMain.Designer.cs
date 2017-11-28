namespace AvoidanceTrainingGame
{
	partial class FormMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timerMain = new System.Windows.Forms.Timer(this.components);
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// timerMain
			// 
			this.timerMain.Enabled = true;
			this.timerMain.Interval = 30;
			this.timerMain.Tick += new System.EventHandler(this.timerMain_Tick);
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(24F, 47F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(378, 344);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Century Gothic", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Avoidance Training";
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMain_Paint);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyUp);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseMove);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer timerMain;
		private System.Windows.Forms.ImageList imageList1;
	}
}

