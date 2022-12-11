using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CometUI
{
	public class CometCheckBox : CheckBox
	{
		private Color checkColor = Color.FromArgb(160, 160, 160);

		/// <summary>
		/// The color used when drawing the check-box for the control
		/// </summary>
		[Description("The color used when drawing the check-box for the control.")]
		public Color CheckColor
		{
			get { return checkColor; }
			set { checkColor = value; Invalidate(); }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new CheckState CheckState { get; set; }

		public CometCheckBox()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint |
					ControlStyles.UserPaint |
					ControlStyles.ResizeRedraw |
					ControlStyles.OptimizedDoubleBuffer |
					ControlStyles.SupportsTransparentBackColor, true);
			DoubleBuffered = true;

			Font = new Font("Segoe UI", 10.0f);
			BackColor = Color.FromArgb(25, 25, 25);
			ForeColor = Color.FromArgb(200, 200, 200);

			Cursor = Cursors.Hand;
		}

		private bool mouseOver = false;
		private bool mouseDown = false;

		protected override void OnMouseEnter(EventArgs eventargs)
		{
			base.OnMouseEnter(eventargs);

			mouseOver = true;
			Invalidate();
		}
		protected override void OnMouseLeave(EventArgs eventargs)
		{
			base.OnMouseLeave(eventargs);

			mouseOver = false;
			Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			base.OnMouseDown(mevent);

			mouseDown = true;
			Invalidate();
		}
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			base.OnMouseUp(mevent);

			mouseDown = false;
			Invalidate();
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			Invalidate();
		}
		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);

			pevent.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			pevent.Graphics.TextContrast = 0;

			byte brightness = checkColor.Brightness();

			pevent.Graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, Width, Height);
			pevent.Graphics.FillRectangle(new SolidBrush(checkColor), 0, 0, Math.Min(Height, 32), Math.Min(Height, 32));

			if (brightness > 127)
			{
				if (mouseOver)
					pevent.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(20, Color.Black)), 0, 0, Math.Min(Height, 32), Math.Min(Height, 32));

				if (mouseDown)
					pevent.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(60, Color.White)), 0, 0, Math.Min(Height, 32), Math.Min(Height, 32));
			}
			else if (brightness <= 127)
			{
				if (mouseOver)
					pevent.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(60, Color.White)), 0, 0, Math.Min(Height, 32), Math.Min(Height, 32));

				if (mouseDown)
					pevent.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(40, Color.Black)), 0, 0, Math.Min(Height, 32), Math.Min(Height, 32));
			}

			pevent.Graphics.FillRectangle(new SolidBrush(BackColor), 2, 2, Math.Min(Height, 32) - 4, Math.Min(Height, 32) - 4);

			if (Checked)
			{
				pevent.Graphics.FillRectangle(new SolidBrush(checkColor), new Rectangle(4, 4, Math.Min(Height, 32) - 8, Math.Min(Height, 32) - 8));

				if (brightness > 127)
				{
					if (mouseOver)
						pevent.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(20, Color.Black)), new Rectangle(4, 4, Math.Min(Height, 32) - 8, Math.Min(Height, 32) - 8));

					if (mouseDown)
						pevent.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(60, Color.White)), new Rectangle(4, 4, Math.Min(Height, 32) - 8, Math.Min(Height, 32) - 8));
				}
				else if (brightness <= 127)
				{
					if (mouseOver)
						pevent.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(60, Color.White)), new Rectangle(4, 4, Math.Min(Height, 32) - 8, Math.Min(Height, 32) - 8));

					if (mouseDown)
						pevent.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(40, Color.Black)), new Rectangle(4, 4, Math.Min(Height, 32) - 8, Math.Min(Height, 32) - 8));
				}
			}

			pevent.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor),
				new Rectangle(Math.Min(Height, 32) + 2, 0, Width - Math.Min(Height, 32) - 1, Height),
				new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
		}
	}
}
