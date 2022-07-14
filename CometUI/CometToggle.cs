using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CometUI
{
	public class CometToggle : CheckBox
	{
		private Color checkedColor = Color.DodgerBlue;
		private Color uncheckedColor = Color.FromArgb(70, 70, 70);
		private Color borderColor = Color.FromArgb(70, 70, 70);

		/// <summary>
		/// The background color of the toggle when its '<see cref="CheckBox.Checked"/>' property is set to '<see langword="true"/>'.
		/// </summary>
		[Description("The background color of the toggle when its 'Checked' property is set to 'true'.")]
		public Color CheckedColor
		{
			get { return checkedColor; }
			set { checkedColor = value; Invalidate(); }
		}

		/// <summary>
		/// The background color of the toggle when its '<see cref="CheckBox.Checked"/>' property is set to '<see langword="false"/>'.
		/// </summary>
		[Description("The background color of the toggle when its 'Checked' property is set to 'false'.")]
		public Color UncheckedColor
		{
			get { return uncheckedColor; }
			set { uncheckedColor = value; Invalidate(); }
		}

		/// <summary>
		/// The background color of the control border.
		/// </summary>
		[Description("The background color of the control border.")]
		public Color BorderColor
		{
			get { return borderColor; }
			set { borderColor = value; Invalidate(); }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new CheckState CheckState { get; set; }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override Color ForeColor => Color.Empty;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override string Text => string.Empty;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override bool AutoSize { get; set; } = false;

		public CometToggle()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint |
					ControlStyles.UserPaint |
					ControlStyles.ResizeRedraw |
					ControlStyles.OptimizedDoubleBuffer, true);
			DoubleBuffered = true;

			BackColor = Color.FromArgb(25, 25, 25);
			Size = new Size(80, 16);

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

		protected override void OnPaint(PaintEventArgs pevent)
		{
			pevent.Graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, Width, Height);

			pevent.Graphics.SmoothingMode = SmoothingMode.HighQuality;

			int radius = (Math.Min(Width, Height) / 2) - 2;
			GraphicsPath path = RoundRect.Roundify(0, 0, Width - 1, Height - 1, radius);

			Rectangle toggle = new Rectangle(
				Checked ? Math.Max(Width, Height) - (Math.Min(Width, Height) - 7) - 3 : 3, 3,
				Math.Min(Width, Height) - 8, Math.Min(Width, Height) - 7);

			pevent.Graphics.DrawPath(new Pen(borderColor, 2.0f), path);
			pevent.Graphics.FillEllipse(new SolidBrush(Checked ? checkedColor : uncheckedColor), toggle);

			byte brightness = (Checked ? checkedColor : uncheckedColor).Brightness();

			if (brightness > 127)
			{
				if (mouseOver)
					pevent.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(20, Color.Black)), toggle);

				if (mouseDown)
					pevent.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(60, Color.White)), toggle);
			}
			else if (brightness <= 127)
			{
				if (mouseOver)
					pevent.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(60, Color.White)), toggle);

				if (mouseDown)
					pevent.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(40, Color.Black)), toggle);
			}

			pevent.Graphics.SmoothingMode = SmoothingMode.None;
		}
	}
}
