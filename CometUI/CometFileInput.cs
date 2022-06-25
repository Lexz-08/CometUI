using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CometUI
{
	public class CometFileInput : Control
	{
		private Color borderColor = Color.FromArgb(70, 70, 70);
		private Color focusedBorderColor = Color.DodgerBlue;
		private Color currentBorderColor = Color.FromArgb(70, 70, 70);
		private Color fileChooserColor = Color.DodgerBlue;
		private bool underlineBorder = false;
		private CometFileInputType inputType = CometFileInputType.OpenFile;
		private string dialogTitle = "Please choose a file to open...";
		private string fileFilter = "All Files|*.*";
		private int defaultFilter = 1;
		private TextBox internalTextBox = new TextBox { BorderStyle = BorderStyle.None };

		/// <summary>
		/// The background color of the textbox border.
		/// </summary>
		[Description("The background color of the textbox border.")]
		public Color BorderColor
		{
			get { return borderColor; }
			set
			{
				borderColor = value;
				if (!internalTextBox.Focused)
					currentBorderColor = value;
				Invalidate();
			}
		}

		/// <summary>
		/// The background color of the textbox border when the control has focus.
		/// </summary>
		[Description("The background color of the textbox border when the control has focus.")]
		public Color FocusedBorderColor
		{
			get { return focusedBorderColor; }
			set
			{
				focusedBorderColor = value;
				if (internalTextBox.Focused)
					currentBorderColor = value;
				Invalidate();
			}
		}

		/// <summary>
		/// The background color of the textbox file chooser button when the mouse is hovered over it.
		/// </summary>
		[Description("The background color of the textbox file chooser button when the mouse is hovered over it.")]
		public Color FileChooserColor
		{
			get { return fileChooserColor; }
			set { fileChooserColor = value; Invalidate(); }
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
		/// Determines what type of file dialog should open when the user clicks the ellipsis that represents the 'Choose File' action on the control.
		/// </summary>
		[Description("Determines what type of file dialog should open when the user clicks the ellipsis that represents the 'Choose File' action on the control.")]
		public CometFileInputType InputType
		{
			get { return inputType; }
			set { inputType = value; Invalidate(); }
		}

		/// <summary>
		/// The title of the file dialog when the user chooses a file.
		/// </summary>
		[Description("The title of the file dialog when the user chooses a file.")]
		public string DialogTitle
		{
			get { return dialogTitle; }
			set { dialogTitle = value; Invalidate(); }
		}

		/// <summary>
		/// The filter to use when displaying files in the file dialog when the user chooses a file.
		/// </summary>
		[Description("The filter to use when displaying files in the file dialog when the user chooses a file.")]
		public string FileFilter
		{
			get { return fileFilter; }
			set { fileFilter = value; Invalidate(); }
		}

		/// <summary>
		/// The <b>1-based</b> index position of the filter in the list specified in the FileFilter property.
		/// </summary>
		[Description("The 1-based index position of the filter in the list specified in the FileFilter property.")]
		public int DefaultFilter
		{
			get { return defaultFilter; }
			set { defaultFilter = value; Invalidate(); }
		}

		/// <summary>
		/// The file selected from the file dialog.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Always)]
		public string File => internalTextBox.Text;

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

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override string Text => string.Empty;

		/// <summary>
		/// The cursor used the indicate interactibility with the user.
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
		/// Indicates whether or not the control is currently focused with keyboard input.
		/// </summary>
		public override bool Focused => internalTextBox.Focused;

		/// <summary>
		/// Occurs when a file is selected.
		/// </summary>
		[Description("Occurs when a file is selected.")]
		public event EventHandler FileSelected;

		/// <summary>
		/// Raises the <see cref="FileSelected"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected void OnFileSelected(EventArgs e)
		{
			FileSelected?.Invoke(this, e);
			Invalidate();
		}

		public CometFileInput()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint |
					ControlStyles.UserPaint |
					ControlStyles.ResizeRedraw |
					ControlStyles.OptimizedDoubleBuffer, true);
			DoubleBuffered = true;

			Font = new Font("Segoe UI", 10.0f);
			BackColor = Color.FromArgb(25, 25, 25);
			ForeColor = Color.FromArgb(200, 200, 200);
			Width = 200;

			Cursor = Cursors.Default;

			internalTextBox.Location = new Point(4, 4);
			internalTextBox.Width = Width - 32;
			internalTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			internalTextBox.ReadOnly = true;

			internalTextBox.GotFocus += (s, e) => UpdateBorderColor();
			internalTextBox.LostFocus += (s, e) => { OnLostFocus(e); };

			internalTextBox.TextChanged += (s, e) => OnTextChanged(e);

			Controls.Add(internalTextBox);
		}

		private Rectangle file;

		[DllImport("user32.dll")]
		private static extern IntPtr LoadCursorFromFile(string lpFilename);

		private void UpdateBorderColor()
		{
			if (internalTextBox.Focused)
				currentBorderColor = focusedBorderColor;
			else currentBorderColor = borderColor;

			Invalidate();
		}

		private void UpdateHeight()
		{
			if (Height < internalTextBox.Height + 8)
				Height = internalTextBox.Height + 8;
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
			UpdateBorderColor();
		}
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);

			UpdateBorderColor();
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

			if (file.Contains(e.Location))
			{
				switch (inputType)
				{
					case CometFileInputType.OpenFile:
						using (OpenFileDialog ofd = new OpenFileDialog
						{
							Title = dialogTitle,
							Filter = fileFilter,
							FilterIndex = defaultFilter,
						}) if (ofd.ShowDialog() == DialogResult.OK)
						{
							internalTextBox.Text = ofd.FileName;
							internalTextBox.Invalidate();
						}
						break;
					case CometFileInputType.SaveFile:
						using (SaveFileDialog sfd = new SaveFileDialog
						{
							Title = dialogTitle,
							Filter = fileFilter,
							FilterIndex = defaultFilter,
						}) if (sfd.ShowDialog() == DialogResult.OK)
						{
							internalTextBox.Text = sfd.FileName;
							internalTextBox.Invalidate();
						}
						break;
				}

				OnFileSelected(e);
			}
			else internalTextBox.Focus();

			Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (file.Contains(e.Location))
			{
				string cursorPath = Registry.CurrentUser.OpenSubKey("Control Panel").OpenSubKey("Cursors").GetValue("Hand").ToString();
				IntPtr cursorHandle = string.IsNullOrEmpty(cursorPath) ? IntPtr.Zero : LoadCursorFromFile(cursorPath);
				Cursor cursorHand = cursorHandle == IntPtr.Zero ? Cursors.Hand : new Cursor(cursorHandle);

				Cursor = cursorHand;
			}
			else Cursor = Cursors.Default;

			Invalidate();
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			Cursor = Cursors.Default;
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			file = new Rectangle(Width - Height - 1, 1, Height - 2, Height - 2);

			Bitmap chooserBmp = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("CometUI.Images.file-chooser.png"));

			ImageAttributes imageAttr = new ImageAttributes();
			ColorMap clrMap = new ColorMap();

			clrMap.OldColor = Color.Black;
			clrMap.NewColor = file.Contains(PointToClient(MousePosition)) ? fileChooserColor : borderColor;

			ColorMap[] remapeTable = { clrMap };
			imageAttr.SetRemapTable(remapeTable, ColorAdjustType.Bitmap);

			e.Graphics.DrawImage(chooserBmp, file, 0, 0, chooserBmp.Width, chooserBmp.Height, GraphicsUnit.Pixel, imageAttr);

			if (underlineBorder)
				e.Graphics.FillRectangle(new SolidBrush(currentBorderColor), 0, Height - 1, Width, 1);
			else e.Graphics.DrawRectangle(new Pen(currentBorderColor), 0, 0, Width - 1, Height - 1);
		}
	}
}
