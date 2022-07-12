using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CometUI
{
	public class CometSlider : Control
	{
		private int value = 50;
		private int defValue = 50;
		private int maximum = 100;
		private int minimum = 0;
		private Color trackColor = Color.FromArgb(25, 25, 25);
		private Color trackOutlineColor = Color.FromArgb(50, 50, 50);
		private Color knobColor = Color.FromArgb(70, 70, 70);
		private Color dragColor = Color.FromArgb(60, 60, 60);
		private Color valueTextColor = Color.FromArgb(200, 200, 200);
		private bool showValue = false;

		/// <summary>
		/// The current value of the slider control.
		/// </summary>
		[Description("The current value of the slider control.")]
		public int Value
		{
			get { return value; }
			set
			{
				this.value = value;

				CalculateKnobPosition();
				OnValueChanged(null);
				Invalidate();
			}
		}

		/// <summary>
		/// The default value of the slider control (what the value would reset to, if '<see cref="ResetValue()"/>' is called).
		/// </summary>
		[Description("The default value of the slider control (what the value would reset to, if 'ResetValue()' is called).")]
		public int DefaultValue
		{
			get { return defValue; }
			set { defValue = value; Invalidate(); }
		}

		/// <summary>
		/// The maximum value the slider control can achieve.
		/// </summary>
		[Description("The maximum value the slider control can achieve.")]
		public int Maximum
		{
			get { return maximum; }
			set
			{
				maximum = value;

				CalculateKnobPosition();
				Invalidate();
			}
		}

		/// <summary>
		/// The minimum value the slider control can achieve.
		/// </summary>
		[Description("The minimum value the slider control can achieve.")]
		public int Minimum
		{
			get { return minimum; }
			set
			{
				minimum = value;

				CalculateKnobPosition();
				Invalidate();
			}
		}

		/// <summary>
		/// The background color of the slider control track.
		/// </summary>
		[Description("The background color of the slider control track.")]
		public Color TrackColor
		{
			get { return trackColor; }
			set { trackColor = value; Invalidate(); }
		}

		/// <summary>
		/// The background color of the slider control track border.
		/// </summary>
		[Description("The background color of the slider control track border.")]
		public Color TrackOutlineColor
		{
			get { return trackOutlineColor; }
			set { trackOutlineColor = value; Invalidate(); }
		}

		/// <summary>
		/// The background color of the slider control track knob.
		/// </summary>
		[Description("The background color of the slider control track knob.")]
		public Color KnobColor
		{
			get { return knobColor; }
			set { knobColor = value; Invalidate(); }
		}

		/// <summary>
		/// The background color of the slider control track knob when it's pressed down on by the mouse or when it's being dragged by the mouse.
		/// </summary>
		[Description("The background color of the slider control track knob when it's pressed down on by the mouse or when it's being dragged by the mouse.")]
		public Color DragColor
		{
			get { return dragColor; }
			set { dragColor = value; Invalidate(); }
		}

		/// <summary>
		/// The foreground color of the value displayed on the slider knob (only if '<see cref="ShowSliderValue"/>' is set to '<see langword="true"/>').
		/// </summary>
		[Description("The foreground color of the value displayed on the slider knob (only if 'ShowSliderValue' is set to 'true').")]
		public Color ValueTextColor
		{
			get { return valueTextColor; }
			set { valueTextColor = value; Invalidate(); }
		}

		/// <summary>
		/// Determines whether or not the value of the slider will be drawn to the slider knob.
		/// </summary>
		[Description("Determines whether or not the value of the slider will be drawn to the slider knob.")]
		public bool ShowSliderValue
		{
			get { return showValue; }
			set { showValue = value; Invalidate(); }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override string Text => string.Empty;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override Font Font => new Font("Microsoft Sans Serif", 0.1f);

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor => Color.Transparent;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override Color ForeColor => Color.Transparent;

		/// <summary>
		/// Occurs when the value of the slider control changes (directly through code or from other events).
		/// </summary>
		[Description("Occurs when the value of the slider control changes (directly through code or from other events).")]
		public event EventHandler ValueChanged;

		/// <summary>
		/// Raises the <see cref="ValueChanged"/> event.
		/// </summary>
		protected void OnValueChanged(EventArgs e)
		{
			ValueChanged?.Invoke(this, e);
			Invalidate();
		}

		public CometSlider()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint |
					ControlStyles.UserPaint |
					ControlStyles.ResizeRedraw |
					ControlStyles.OptimizedDoubleBuffer |
					ControlStyles.SupportsTransparentBackColor, true);
			DoubleBuffered = true;

			Size = new Size(200, 16);
			Cursor = Cursors.Hand;

			knob = new Rectangle(Width / 2, 0, 16, 16);
		}

		private bool canDrag = false;
		private int offX = 0;

		private Rectangle knob;

		/// <summary>
		/// Resets the slider control value to its currently set default value (<see cref="DefaultValue"/>).
		/// </summary>
		public void ResetValue()
		{
			value = defValue;

			CalculateKnobPosition();
			OnValueChanged(null);
			Invalidate();
		}

		private void CalculateValue(MouseEventArgs e)
		{
			knob.X = Math.Max(Height / 2, Math.Min(e.X - offX, Width - (Height / 2) - 1));
			double width = Width - Height;
			double knobX = knob.X - (Height / 2);
			double percent = knobX / width;

			value = (int)Math.Round((percent * (maximum - minimum)) + minimum, 0);

			OnValueChanged(e);
			Invalidate();
		}

		private void CalculateKnobPosition()
		{
			double width = Width - Height;
			double num = value - minimum;
			double den = maximum - minimum;
			knob.X = Math.Max((Height / 2) + 1, Math.Min((int)((num / den * width) + (Height / 2)), Width - (Height / 2) - 1));

			Invalidate();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.KeyCode == Keys.Left)
			{
				if (value < minimum)
					value = maximum;
				else value -= 1;
			}
			else if (e.KeyCode == Keys.Right)
			{
				if (value > maximum)
					value = minimum;
				else value += 1;
			}

			CalculateKnobPosition();
			Invalidate();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			knob.Height = Height;
			knob.Width = knob.Height;

			CalculateKnobPosition();
			Invalidate();
		}
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			base.OnResize(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			canDrag = true;
			offX = e.X - knob.X;

			int cX = knob.X;
			int cY = knob.Y + (knob.Height / 2);
			int radius = Height / 2;

			if (!(Math.Pow(e.X - cX, 2) + Math.Pow(e.Y - cY, 2) <= Math.Pow(radius, 2))) // knob.X + (knob.Width / 2) < e.X || knob.X - (knob.Width / 2) > e.X
			{
				offX = 0;
				knob.X = e.X - offX;
			}

			CalculateValue(e);

			Invalidate();
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			canDrag = false;
			offX = 0;
			Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (canDrag)
			{
				CalculateValue(e);
				CalculateKnobPosition();
			}

			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Rectangle track = new Rectangle((Height / 2) - 1, (Height / 4) - 1, Width - Height, (Height / 2) + 1);

			int radius = Math.Min(Width, Height) / 2;
			GraphicsPath trackPath = new GraphicsPath();
			trackPath.StartFigure();
			trackPath.AddArc(track.X, track.Y, radius, radius, 180, 90);
			trackPath.AddArc(track.X + track.Width - radius, track.Y, radius, radius, 270, 90);
			trackPath.AddArc(track.X + track.Width - radius, track.Y + track.Height - radius, radius, radius, 0, 90);
			trackPath.AddArc(track.X, track.Y + track.Height - radius, radius, radius, 90, 90);
			trackPath.CloseFigure();

			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			e.Graphics.FillPath(new SolidBrush(trackColor), trackPath);
			e.Graphics.DrawPath(new Pen(trackOutlineColor, 2.0f), trackPath);
			e.Graphics.FillEllipse(new SolidBrush(canDrag ? dragColor : knobColor), knob.X - (knob.Width / 2), knob.Y - 1, knob.Width, knob.Height);
			e.Graphics.SmoothingMode = SmoothingMode.None;

			if (showValue)
			{
				e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
				e.Graphics.TextContrast = 0;

				e.Graphics.DrawString(value.ToString(), new Font(Font.Name, knob.Height / 3), new SolidBrush(valueTextColor),
					new Rectangle(knob.X - (knob.Width / 2), knob.Y + (knob.Height % 2 == 0 ? 0 : 1), knob.Width, knob.Height),
					new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
			}
		}
	}
}
