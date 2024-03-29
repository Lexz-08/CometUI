﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CometUI
{
	public class CometRoundGradientButton : Control
	{
		private Color gradientColor1 = Color.FromArgb(40, 40, 40);
		private Color gradientColor2 = Color.FromArgb(90, 90, 90);
		private float gradientAngle = 45.0f;
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
		/// The angle of gradience.
		/// </summary>
		[Description("The angle of gradience.")]
		public float GradientAngle
		{
			get { return gradientAngle; }
			set { gradientAngle = value; Invalidate(); }
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

		public CometRoundGradientButton()
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

			Size = new Size(190, 40);
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
			LinearGradientBrush lgb = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), gradientColor1, gradientColor2, gradientAngle);
			GraphicsPath path = RoundRect.Roundify(1, 1, Width - 3, Height - 3, radius);
			GraphicsPath outline = RoundRect.Roundify(1, 1, Width - 3, Height - 3, radius);
			e.Graphics.FillPath(lgb, path);
			e.Graphics.DrawPath(new Pen(lgb, 2.0f), outline);

			byte brightness = (byte)((gradientColor1.Brightness() + gradientColor2.Brightness()) / 2);

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

			e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor),
				new Rectangle(0, Height % 2 == 0 ? 1 : 0, Width, Height),
				new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}
	}
}
