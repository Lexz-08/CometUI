using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CometUI
{
	public class CometProgressBar : Control
	{
		private double value = 50.0d;
		private double maximum = 100.0d;
		private double minimum = 0.0d;
		private double defValue = 50.0d;
		private Color progressColor = Color.FromArgb(50,50,50);
		private Color borderColor = Color.FromArgb(70, 70, 70);

		/// <summary>
		/// The current progress value of the control.
		/// </summary>
		[Description("The current progress value of the control.")]
		public double Value
		{
			get { return value; }
			set
			{
				this.value = value;

				if (value == minimum) OnStarted(null);
				else if (value == maximum) OnStarted(null);
				else { /* do nothing */ }

				Invalidate();
			}
		}

		/// <summary>
		/// The progress value the control must achieve for the '<see cref="Completed"/>' event to occur.
		/// </summary>
		[Description("The progress value the control must achieve for the 'Completed' event to occur.")]
		public double Maximum
		{
			get { return maximum; }
			set { maximum = value; Invalidate(); }
		}

		/// <summary>
		/// The progress value the control must achieve for the '<see cref="Started"/>' even to occur.
		/// </summary>
		[Description("The progress value the control must achieve for the 'Started' event to occur.")]
		public double Minimum
		{
			get { return minimum; }
			set { minimum = value; Invalidate(); }
		}

		/// <summary>
		/// The progress value the goes to if its reset.
		/// </summary>
		[Description("The progress value the control goes to if it's reset.")]
		public double DefaultValue
		{
			get { return defValue; }
			set { defValue = value; Invalidate(); }
		}

		/// <summary>
		/// The background color of the progress bar in the control.
		/// </summary>
		[Description("The background color of the progress bar in the control.")]
		public Color ProgressColor
		{
			get { return progressColor; }
			set { progressColor = value; Invalidate(); }
		}

		/// <summary>
		/// The background coloor of the border around the progress bar.
		/// </summary>
		[Description("The background color of the border around the progress bar.")]
		public Color BorderColor
		{
			get { return borderColor; }
			set { borderColor = value; Invalidate(); }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override Font Font => new Font("Microsoft Sans Serif", 0.1f);

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override string Text => string.Empty;

		/// <summary>
		/// Occurs when the progress bar has achieved its maximum progress value.
		/// </summary>
		[Description("Occurs when the progress bar has achieved its maximum progress value.")]
		public event EventHandler Completed;

		/// <summary>
		/// Occurs when the progress bar has achieved its minimum progress value.
		/// </summary>
		[Description("Occurs when the progress bar has achieved its minimum progress value.")]
		public event EventHandler Started;

		/// <summary>
		/// Occurs when the progress value has changed.
		/// </summary>
		[Description("Occurs when the progress value has changed.")]
		public event EventHandler ValueChanged;

		/// <summary>
		/// Raises the <see cref="Completed"/> event.
		/// </summary>
		protected void OnCompleted(EventArgs e)
		{
			Completed?.Invoke(this, e);
			Invalidate();
		}

		/// <summary>
		/// Raises the <see cref="Started"/> event.
		/// </summary>
		protected void OnStarted(EventArgs e)
		{
			Started?.Invoke(this, e);
			Invalidate();
		}

		protected void OnValueChanged(EventArgs e)
		{
			ValueChanged?.Invoke(this, e);
			Invalidate();
		}

		public CometProgressBar()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint |
					ControlStyles.UserPaint |
					ControlStyles.ResizeRedraw |
					ControlStyles.OptimizedDoubleBuffer, true);
			DoubleBuffered = true;

			BackColor = Color.FromArgb(25, 25, 25);
			ForeColor = Color.FromArgb(200, 200, 200);
			Size = new Size(200, 10);
		}

		/// <summary>
		/// Increases the current progress by the specified value within a specified constrain.
		/// </summary>
		/// <param name="Amount">The amount to increase the current progress by.</param>
		public void IncreaseProgress(double Amount)
		{
			if (value == maximum)
				return;

			if (Amount < 1)
				value++;
			else if (Amount > Math.Abs(Math.Abs(maximum) - Amount))
				value = maximum;
			else value += Amount;

			if (value == maximum) OnCompleted(null);
			Invalidate();
		}

		/// <summary>
		/// Decreases the current progress by the specified value within a specified constrain.
		/// </summary>
		/// <param name="Amount">The amount to decrease the current progress by.</param>
		public void DecreaseProgress(double Amount)
		{
			if (value == minimum)
				return;

			if (Amount < 1)
				value--;
			else if (Amount > Math.Abs(Math.Abs(minimum) - Amount))
				value = minimum;
			else value -= Amount;

			if (value == minimum) OnStarted(null);
			Invalidate();
		}

		/// <summary>
		/// Resets the progress value of the control to the currently set default value.
		/// </summary>
		public void ResetValue()
		{
			value = defValue;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			double progress = (value / 100 * (maximum - minimum)) + minimum;

			e.Graphics.FillRectangle(new SolidBrush(progressColor), 1, 1, (float)(Width * (progress / 100.0f)) - 2, Height - 2);
			e.Graphics.DrawRectangle(new Pen(borderColor), 0, 0, Width - 1, Height - 1);
		}
	}
}
