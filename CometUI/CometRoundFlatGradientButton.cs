using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CometUI
{
	public class CometRoundFlatGradientButton : Control
	{
		private Color gradientColor1 = Color.FromArgb(40, 40, 40);
		private Color gradientColor2 = Color.FromArgb(90, 90, 90);
		private float gradientAngle = 45.0f;
		private int borderWidth = 1;
		private int radius = 4;

		/// <summary>
		/// The first color used in the gradience.
		/// </summary>
		[Description("The first color used in the gradience.")]
		public Color GradientColor1
		{
			get { return gradientColor1; }
			set { gradientColor1 = value; Invalidate(); }
		}

		/// <summary>
		/// The second color used in the gradience.
		/// </summary>
		[Description("The second color used in the gradience.")]
		public Color GradientColor2
		{
			get { return gradientColor2; }
			set { gradientColor2 = value; Invalidate(); }
		}

		/// <summary>
		/// The angle of the gradience.
		/// </summary>
		[Description("The angle of the gradience.")]
		public float GradientAngle
		{
			get { return gradientAngle; }
			set { gradientAngle = value; Invalidate(); }
		}

		/// <summary>
		/// The thickness of the control border.
		/// </summary>
		[Description("The thickness of the control border.")]
		public int BorderWidth
		{
			get { return borderWidth; }
			set { borderWidth = value; Invalidate(); }
		}

		/// <summary>
		/// The arc length of the corners on the control border.
		/// </summary>
		[Description("The arc length of the corners on the control border.")]
		public int BorderRadius
		{
			get { return radius; }
			set { radius = value; Invalidate(); }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor => Color.Empty;

		public CometRoundFlatGradientButton()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint |
					ControlStyles.UserPaint |
					ControlStyles.ResizeRedraw |
					ControlStyles.OptimizedDoubleBuffer |
					ControlStyles.SupportsTransparentBackColor, true);
			DoubleBuffered = true;

			Font = new Font("Segoe UI", 10.0f);
			ForeColor = Color.FromArgb(200, 200, 200);
			Size = new Size(220, 40);

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
			base.OnPaint(e);

			e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			e.Graphics.TextContrast = 0;

			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			GraphicsPath path = RoundRect.Roundify(borderWidth / 2, borderWidth / 2, Width - borderWidth - 1, Height - borderWidth - 1, radius);
			LinearGradientBrush lgb = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), gradientColor1, gradientColor2, gradientAngle);
			e.Graphics.DrawPath(new Pen(lgb, borderWidth), path);

			e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor),
				new Rectangle(borderWidth, Height % 2 == 0 ? borderWidth + 1 : borderWidth, Width - (borderWidth * 2), Height - (borderWidth * 2)),
				new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

			byte brightness = ForeColor.Brightness();

			if (brightness > 127)
			{
				if (mouseOver)
					e.Graphics.DrawPath(new Pen(Color.FromArgb(40, Color.Black), borderWidth), path);

				if (mouseDown)
					e.Graphics.DrawPath(new Pen(Color.FromArgb(80, Color.White), borderWidth), path);
			}
			else if (brightness <= 127)
			{
				if (mouseOver)
					e.Graphics.DrawPath(new Pen(Color.FromArgb(80, Color.White), borderWidth), path);

				if (mouseDown)
					e.Graphics.DrawPath(new Pen(Color.FromArgb(60, Color.White), borderWidth), path);
			}
		}
	}
}
