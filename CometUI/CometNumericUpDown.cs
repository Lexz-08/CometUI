using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace CometUI
{
	public class CometNumericUpDown : Control
	{
		private int value = 50;
		private int defValue = 50;
		private int maximum = 100;
		private int minimum = 0;
		private Color increaseColor = Color.Green;
		private Color decreaseColor = Color.Red;
		private bool colorizeButtons = false;

		/// <summary>
		/// The current value of the numeric control.
		/// </summary>
		[Description("The current value of the numeric control.")]
		public int Value
		{
			get { return value; }
			set { this.value = value; Invalidate(); }
		}

		/// <summary>
		/// The default value of the numeric control (what the value is reset to if '<see cref="ResetValue()"/>' is called).
		/// </summary>
		[Description("The default value of the numeric control (what the value is reset to if 'ResetValue()' is called).")]
		public int DefaultValue
		{
			get { return defValue; }
			set { defValue = value; Invalidate(); }
		}

		/// <summary>
		/// The maximum value the numeric control can achieve.
		/// </summary>
		[Description("The maximum value the numeric control can achieve.")]
		public int Maximum
		{
			get { return maximum; }
			set { maximum = value; Invalidate(); }
		}

		/// <summary>
		/// The minimum value the numeric control can achieve.
		/// </summary>
		[Description("The minimum value the numeric control can achieve.")]
		public int Minimum
		{
			get { return minimum; }
			set { minimum = value; Invalidate(); }
		}

		/// <summary>
		/// The background color of the increase button when the mouse hovers over it (and if '<see cref="ColorizeButtons"/>' set to '<see langword="true"/>', then the foreground color of the increase button (the increase button's text color)).
		/// </summary>
		[Description("The background color of the increase button when the mouse hovers over it (and if 'ColorizeButtons' is set to 'true', then the foreground color of the increase button (the increase button's text color)).")]
		public Color IncreaseColor
		{
			get { return increaseColor; }
			set { increaseColor = value; Invalidate(); }
		}

		/// <summary>
		/// The background color of the decrease button when the mouse hovers over it (and if '<see cref="ColorizeButtons"/>' is set to '<see langword="true"/>', then the foreground color of the decrease button (the decrease button's text color)).
		/// </summary>
		[Description("The background color of the decrease button when the mouse hovers over it (and if 'ColorizeButtons' is set to 'true', then the foreground color of the decrease button (the decrease button's text color)).")]
		public Color DecreaseColor
		{
			get { return decreaseColor; }
			set { decreaseColor = value; Invalidate(); }
		}

		/// <summary>
		/// Determines whether or not the numeric control's increase/decrease button's icons are colorized using the hover colors when the buttons are in their normal state.
		/// </summary>
		[Description("Determines whether or not the numeric control's increase/decrease buttons's icons are colorized using the hover colors when the buttons are in their normal state.")]
		public bool ColorizeButtons
		{
			get { return colorizeButtons; }
			set { colorizeButtons = value; Invalidate(); }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override string Text => string.Empty;

		/// <summary>
		/// Occurs when the numeric control's value has changed.
		/// </summary>
		[Description("Occurs when the numeric control's value has changed.")]
		public event EventHandler ValueChanged;

		/// <summary>
		/// Occurs when the mouse wheel turns while hovering over the numeric control.
		/// </summary>
		[Description("Occurs when the mouse wheel turns while hovering over the numeric control.")]
		public event EventHandler Scroll;

		/// <summary>
		/// Raises the <see cref="ValueChanged"/> event.
		/// </summary>
		protected void OnValueChanged(EventArgs e)
		{
			ValueChanged?.Invoke(this, e);
			Invalidate();
		}

		/// <summary>
		/// Raises the <see cref="Scroll"/> event.
		/// </summary>
		protected void OnScroll(EventArgs e)
		{
			ValueChanged?.Invoke(this, e);
			Scroll?.Invoke(this, e);
			Invalidate();
		}

		public CometNumericUpDown()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint |
					ControlStyles.UserPaint |
					ControlStyles.ResizeRedraw |
					ControlStyles.OptimizedDoubleBuffer, true);
			DoubleBuffered = true;

			Font = new Font("Segoe UI", 10.0f);
			BackColor = Color.FromArgb(50, 50, 50);
			ForeColor = Color.FromArgb(200, 200, 200);
			Size = new Size(100, 75);
		}

		private RectangleF increase, decrease;

		/// <summary>
		/// Resets the numeric control's value to its currently set default value (<see cref="DefaultValue"/>).
		/// </summary>
		public void ResetValue()
		{
			value = defValue;

			OnValueChanged(null);
			Invalidate();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			Invalidate();
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (increase.Contains(e.Location))
				value = Math.Max(minimum, Math.Min(value + 1, maximum));
			else if (decrease.Contains(e.Location))
				value = Math.Max(minimum, Math.Min(value - 1, maximum));

			OnValueChanged(e);
			Invalidate();
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);

			value = Math.Max(minimum, Math.Min(value + (e.Delta / 120), maximum));
			OnScroll(e);
			Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (increase.Contains(e.Location) || decrease.Contains(e.Location))
				Cursor = Cursors.Hand;
			else Cursor = Cursors.Default;

			Invalidate();
			GC.Collect();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			e.Graphics.TextContrast = 0;

			increase = new RectangleF(Width * (2.0f / 3.0f), 0, Width / 3.0f, Height / 2.0f);
			decrease = new RectangleF(Width * (2.0f / 3.0f), Height / 2.0f, Width / 3.0f, Height / 2.0f);

			e.Graphics.DrawString(value.ToString(), new Font(Font.Name, Height * 0.5f), new SolidBrush(ForeColor),
				new RectangleF(0, Height % 2 == 0 ? 1 : 0, Width * (2.0f / 3.0f), Height),
				new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

			Point mouse = PointToClient(MousePosition);

			e.Graphics.FillRectangle(new SolidBrush(increase.Contains(mouse) ? increaseColor : BackColor), increase);
			e.Graphics.FillRectangle(new SolidBrush(decrease.Contains(mouse) ? decreaseColor : BackColor), decrease);

			e.Graphics.DrawString("+", new Font("Consolas", increase.Height * 0.5f),
				new SolidBrush(increase.Contains(mouse) ? ForeColor : (colorizeButtons ? increaseColor : ForeColor)),
				new RectangleF(increase.X, increase.Y + (increase.Height % 2 == 0 ? 1 : 0), increase.Width, increase.Height),
				new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
			e.Graphics.DrawString("-", new Font("Consolas", decrease.Height * 0.5f),
				new SolidBrush(decrease.Contains(mouse) ? ForeColor : (colorizeButtons ? decreaseColor : ForeColor)),
				new RectangleF(decrease.X, decrease.Y + (decrease.Height % 2 == 0 ? 0 : 1), decrease.Width, decrease.Height),
				new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}
	}
}
