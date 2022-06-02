using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CometUI
{
	public class CometButton : Control
	{
		public CometButton()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
			DoubleBuffered = true;

			Font = new Font("Segoe UI", 10.0f);
			BackColor = Color.FromArgb(25, 25, 25);
			ForeColor = Color.FromArgb(200, 200, 200);

			Size = new Size(120, 40);
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

			e.Graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, Width, Height);

			byte brightness = BackColor.Brightness();

			if (brightness > 127)
			{
				if (mouseOver)
					e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(20, Color.Black)), 0, 0, Width, Height);

				if (mouseDown)
					e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(60, Color.White)), 0, 0, Width, Height);
			}
			else if (brightness <= 127)
			{
				if (mouseOver)
					e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(60, Color.White)), 0, 0, Width, Height);

				if (mouseDown)
					e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(40, Color.Black)), 0, 0, Width, Height);
			}

			e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor),
				new Rectangle(0, 0, Width, Height),
				new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}
	}
}
