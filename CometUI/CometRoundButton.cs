using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CometUI
{
	public class CometRoundButton : Control
	{
		private Color background = Color.FromArgb(25, 25, 25);
		private int radius = 4;

		/// <summary>
		/// The background color of the control.
		/// </summary>
		[Description("The background color of the control.")]
		public Color Background
		{
			get { return background; }
			set { background = value; OnBackColorChanged(null); Invalidate(); }
		}

		/// <summary>
		/// The arc length of the corners of the control.
		/// </summary>
		[Description("The arc length of the corners of the control.")]
		public int BorderRadius
		{
			get { return radius; }
			set { radius = value; Invalidate(); }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get { return base.BackColor; }
			set { base.BackColor = value; OnBackColorChanged(null); Invalidate(); }
		}

		public CometRoundButton()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint |
					ControlStyles.UserPaint |
					ControlStyles.ResizeRedraw |
					ControlStyles.OptimizedDoubleBuffer |
					ControlStyles.SupportsTransparentBackColor, true);
			DoubleBuffered = true;

			Font = new Font("Segoe UI", 10.0f);
			BackColor = Color.Transparent;
			ForeColor = Color.FromArgb(200, 200, 200);

			Size = new Size(150, 40);
			Cursor = Cursors.Hand;
		}

		private bool mouseOver = false;
		private bool mouseDown = false;

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			mouseOver = true;
			Invalidate();
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			mouseOver = false;
			Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			mouseDown = true;
			Invalidate();
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			mouseDown = false;
			Invalidate();
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			Invalidate();
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			e.Graphics.TextContrast = 0;

			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			GraphicsPath path = RoundRect.Roundify(0, 0, Width - 1, Height - 1, radius, false);
			e.Graphics.FillPath(new SolidBrush(background), path);

			byte brightness = background.Brightness();

			if (brightness > 127)
			{
				if (mouseOver)
					e.Graphics.FillPath(new SolidBrush(Color.FromArgb(20, Color.Black)), path);

				if (mouseDown)
					e.Graphics.FillPath(new SolidBrush(Color.FromArgb(60, Color.White)), path);
			}
			else if (brightness <= 127)
			{
				if (mouseOver)
					e.Graphics.FillPath(new SolidBrush(Color.FromArgb(60, Color.White)), path);

				if (mouseDown)
					e.Graphics.FillPath(new SolidBrush(Color.FromArgb(40, Color.Black)), path);
			}

			e.Graphics.SmoothingMode = SmoothingMode.None;

			e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor),
				new Rectangle(0, Height % 2 == 0 ? 1 : 0, Width, Height),
				new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}
	}
}
