using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CometUI
{
	public class CometTextBox : Control
	{
		private Color borderColor = Color.FromArgb(90, 90, 90);
		private bool underlineBorder = false;
		private TextBox internalTextBox = new TextBox { BorderStyle = BorderStyle.None };

		/// <summary>
		/// The background color of the textbox border.
		/// </summary>
		[Description("The background color of the textbox border.")]
		public Color BorderColor
		{
			get { return borderColor; }
			set { borderColor = value; Invalidate(); }
		}

		/// <summary>
		/// Determines whether or not the border of the textbox will be an underline.
		/// </summary>
		[Description("Determines whether or not the border of the textbox will be an underline.")]
		public bool UnderlineBorder
		{
			get { return underlineBorder; }
			set { underlineBorder = value; Invalidate(); }
		}

		/// <summary>
		/// The background color of the textbox.
		/// </summary>
		[Description("The background color of the textbox.")]
		public override Color BackColor
		{
			get { return base.BackColor; }
			set
			{
				internalTextBox.BackColor = base.BackColor = value;
				internalTextBox.Invalidate();
				Invalidate();
				OnBackColorChanged(null);
			}
		}

		/// <summary>
		/// The foreground color of the textbox (the textbox text color).
		/// </summary>
		[Description("The foreground color of the textbox (the textbox text color).")]
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set
			{
				internalTextBox.ForeColor = base.ForeColor = value;
				internalTextBox.Invalidate();
				Invalidate();
				OnForeColorChanged(null);
			}
		}

		/// <summary>
		/// The font used when rendering/displaying the textbox text.
		/// </summary>
		[Description("The font used when rendering/displaying the textbox text.")]
		public override Font Font
		{
			get { return base.Font; }
			set
			{
				internalTextBox.Font = base.Font = value;
				UpdateHeight();
				internalTextBox.Invalidate();
				Invalidate();
				OnFontChanged(null);
			}
		}

		/// <summary>
		/// The text displayed in the textbox.
		/// </summary>
		[Description("The text displayed in the textbox.")]
		public override string Text
		{
			get { return internalTextBox.Text; }
			set
			{
				internalTextBox.Text = value;
				internalTextBox.Invalidate();
				Invalidate();
				OnTextChanged(null);
			}
		}

		/// <summary>
		/// The cursor used to indicate interactibility with the user.
		/// </summary>
		[Description("The cursor used to indicate interactibility with the user.")]
		public override Cursor Cursor
		{
			get { return base.Cursor; }
			set
			{
				internalTextBox.Cursor = base.Cursor = value;
				Invalidate();
				OnCursorChanged(null);
			}
		}

		/// <summary>
		/// Determines whether or not the textbox can display multiple lines of text at once.
		/// </summary>
		[Description("Determines whether or not the textbox can display multiple lines of text at once.")]
		public bool Multiline
		{
			get { return internalTextBox.Multiline; }
			set { internalTextBox.Multiline = value; Invalidate(); }
		}

		/// <summary>
		/// Determines whether or not the textbox's text can be edited at runtime by the user.
		/// </summary>
		[Description("Determines whether or not the textbox's text can be edited at runtime by the user.")]
		public bool Readonly
		{
			get { return internalTextBox.ReadOnly; }
			set { internalTextBox.ReadOnly = value; Invalidate(); }
		}

		/// <summary>
		/// Determines whether or not the textbox will display a password character instead of normal text when rendering/displaying the textbox text.
		/// </summary>
		[Description("Determines whether or not the textbox will display a password character instead of normal text when rendering/displaying the textbox text.")]
		public bool UseSystemPasswordChar
		{
			get { return internalTextBox.UseSystemPasswordChar; }
			set { internalTextBox.UseSystemPasswordChar = value; Invalidate(); }
		}

		public override bool Focused => internalTextBox.Focused;

		public CometTextBox()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint |
					ControlStyles.UserPaint |
					ControlStyles.ResizeRedraw |
					ControlStyles.OptimizedDoubleBuffer, true);
			DoubleBuffered = true;

			Font = new Font("Segoe UI", 10.0f);
			BackColor = Color.FromArgb(50, 50, 50);
			ForeColor = Color.FromArgb(200, 200, 200);
			Width = 200;

			Cursor = Cursors.IBeam;

			internalTextBox.Location = new Point(4, 5);
			internalTextBox.Width = Width - 8;
			internalTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			Controls.Add(internalTextBox);
		}

		private void UpdateHeight()
		{
			if (Height < TextRenderer.MeasureText("a", Font).Height + 8)
				Height = TextRenderer.MeasureText("a", Font).Height + 8;

			if (!Multiline)
				Height = TextRenderer.MeasureText("a", Font).Height + 8;
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			UpdateHeight();
		}
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			base.OnResize(e);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);

			internalTextBox.Focus();
			Invalidate();
		}
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);

			Invalidate();
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);

			internalTextBox.Focus();
			Invalidate();
		}
		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			internalTextBox.Focus();
			Invalidate();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			internalTextBox.Focus();
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (underlineBorder)
				e.Graphics.FillRectangle(new SolidBrush(borderColor), 0, Height - 1, Width, 1);
			else e.Graphics.DrawRectangle(new Pen(borderColor), 0, 0, Width - 1, Height - 1);
		}
	}
}
