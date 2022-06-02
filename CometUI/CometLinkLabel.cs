using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace CometUI
{
	public class CometLinkLabel : Label
	{
		private string link = "https://www.google.com/";

		/// <summary>
		/// The link opened when the label is clicked.
		/// </summary>
		[Description("The link opened when the label is clicked.")]
		public string Link
		{
			get { return link; }
			set { link = value; Invalidate(); }
		}

		public CometLinkLabel()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint |
					ControlStyles.UserPaint |
					ControlStyles.ResizeRedraw |
					ControlStyles.OptimizedDoubleBuffer |
					ControlStyles.SupportsTransparentBackColor, true);
			DoubleBuffered = true;

			Font = new Font("Segoe UI", 10.0f, FontStyle.Underline);
			BackColor = Color.FromArgb(25, 25, 25);
			ForeColor = Color.FromArgb(25, 160, 255);

			Cursor = Cursors.Hand;
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);

			Process.Start(link);
		}
	}
}
