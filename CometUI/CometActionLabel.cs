using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CometUI
{
	public class CometActionLabel : Label
	{
		private Color hoverColor = Color.DodgerBlue;

		/// <summary>
		/// The foreground color of the label when the mouse hovers over it.
		/// </summary>
		[Description("The foreground color of the label when the mouse hovers over it.")]
		public Color HoverColor
		{
			get { return hoverColor; }
			set { hoverColor = value; Invalidate(); }
		}

		public CometActionLabel()
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

		private StringFormat GetAlignment(ContentAlignment Alignment)
		{
			switch (Alignment)
			{
				case ContentAlignment.TopLeft:
					return new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
				case ContentAlignment.TopCenter:
					return new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near };
				case ContentAlignment.TopRight:
					return new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near };

				case ContentAlignment.MiddleLeft:
					return new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
				case ContentAlignment.MiddleCenter:
					return new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
				case ContentAlignment.MiddleRight:
					return new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };

				case ContentAlignment.BottomLeft:
					return new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far };
				case ContentAlignment.BottomCenter:
					return new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };
				case ContentAlignment.BottomRight:
					return new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };
			}

			return new StringFormat();
		}

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

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			e.Graphics.TextContrast = 0;

			e.Graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, Width, Height);
			e.Graphics.DrawString(Text, mouseOver ? new Font(Font, FontStyle.Underline) : Font, new SolidBrush(mouseOver ? hoverColor : ForeColor),
				new Rectangle(-1, Height % 2 == 0 ? 0 : 1, Width, Height), AutoSize ? GetAlignment(ContentAlignment.MiddleCenter) : GetAlignment(TextAlign));
		}
	}
}
