using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CometUI
{
	public class CometForm : Form
	{
		private Color headerColor = Color.FromArgb(50, 50, 50);
		private Color closeColor = Color.Red;
		private Color minimizeColor = Color.Green;
		private Color maximizeColor = Color.CornflowerBlue;
		private Color borderColor = Color.FromArgb(70, 70, 70);
		private bool canResize = true;
		private bool useWinDropShadow = true;
		
		/// <summary>
		/// The background color for the form's header.
		/// </summary>
		[Description("The background color for the form's header.")]
		public Color HeaderColor
		{
			get { return headerColor; }
			set { headerColor = value; Invalidate(); }
		}

		/// <summary>
		/// The foreground color for the form's close button.
		/// </summary>
		[Description("The foreground color for the form's close button.")]
		public Color CloseColor
		{
			get { return closeColor; }
			set { closeColor = value; Invalidate(); }
		}

		/// <summary>
		/// The foreground color for the form's minimize button.
		/// </summary>
		[Description("The foreground color for the form's minimize button.")]
		public Color MinimizeColor
		{
			get { return minimizeColor; }
			set { minimizeColor = value; Invalidate(); }
		}

		/// <summary>
		/// The foreground color for the form's maximize button.
		/// </summary>
		[Description("The foreground color for the form's maximize button.")]
		public Color MaximizeColor
		{
			get { return maximizeColor; }
			set { maximizeColor = value; Invalidate(); }
		}

		/// <summary>
		/// The background color for the form's border, if <see cref="UseWindowsDropShadow"/> is set to <see langword="false"/>.
		/// </summary>
		[Description("The background color for the form's border, if UseWindowsDropShadow is set to false..")]
		public Color BorderColor
		{
			get { return borderColor; }
			set { borderColor = value; Invalidate(); }
		}

		/// <summary>
		/// Determines whether or not the form can be resized via the mouse cursor at runtime.
		/// </summary>
		[Description("Determines whether or not the form can be resized via the mouse cursor at runtime.")]
		public bool CanResize
		{
			get { return canResize; }
			set
			{
				canResize = value;

				if (!canResize)
					MaximizeBox = false;

				Invalidate();
			}
		}

		/// <summary>
		/// Determines whether or not the form will use the current Windows drop-shadow.<br/><br/>
		/// <b>Important Note</b>: Only works if Aero theme is enabled on the device.
		/// </summary>
		[Description("Determines whether or not the form will use the current Windows drop-shadow.\n\nImportant Note: Only works if Aero theme is enabled on the device.")]
		public bool UseWindowsDropShadow
		{
			get { return useWinDropShadow; }
			set { useWinDropShadow = value; Invalidate(); }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new FormBorderStyle FormBorderStyle
		{
			get { return base.FormBorderStyle; }
			set { base.FormBorderStyle = value; Invalidate(); }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new bool DoubleBuffered
		{
			get { return base.DoubleBuffered; }
			set { base.DoubleBuffered = value; Invalidate(); }
		}

		/// <summary>
		/// Occurs when the form's window state changes.
		/// </summary>
		[Description("Occurs when the form's window state changes.")]
		public event EventHandler WindowStateChanged;

		/// <summary>
		/// Raises the <see cref="WindowStateChanged"/> event.
		/// </summary>
		protected void OnWindowStateChanged(EventArgs e)
		{
			WindowStateChanged?.Invoke(this, e);
			Invalidate();
		}

		public CometForm()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint |
					ControlStyles.UserPaint |
					ControlStyles.ResizeRedraw |
					ControlStyles.OptimizedDoubleBuffer, true);
			DoubleBuffered = true;

			Font = new Font("Segoe UI", 10.0f);
			BackColor = Color.FromArgb(25, 25, 25);
			ForeColor = Color.FromArgb(200, 200, 200);

			MinimumSize = new Size(200, 100);
			FormBorderStyle = FormBorderStyle.None;
		}

		private Rectangle left, topLeft, bottomLeft,
			right, topRight, bottomRight,
			top, bottom, close, minimize, maximize;

		private Rectangle ctCaption1, ctCaption2;
		private Color ctCaptionClr1, ctCaptionClr2;
		private Color ctCaptionClr1_Orig, ctCaptionClr2_Orig;
		private Image captionCnt1, captionCnt2;
		private bool allowCaption1, allowCaption2;

		private EventHandler captionClick1, captionClick2;

		protected const int headerHeight = 22,
			resizeBorder = 6;

		[DllImport("user32.dll")]
		private static extern bool ReleaseCapture();

		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImport("user32.dll")]
		private static extern IntPtr LoadCursorFromFile(string lpFilename);

		private Cursor LoadFromRegistry(string Name)
		{
			string path = Registry.CurrentUser.OpenSubKey("Control Panel").OpenSubKey("Cursors").GetValue(Name).ToString();
			IntPtr handle = string.IsNullOrEmpty(path) ? IntPtr.Zero : LoadCursorFromFile(path);
			Cursor cursor = handle == IntPtr.Zero ? null : new Cursor(handle);

			return cursor;
		}

		/// <summary>
		/// Adds a new caption button to the header of the form.
		/// </summary>
		/// <param name="InteractionContent">The image to display where the button would be.</param>
		/// <param name="OriginalImageColor">The original, solid color of the image provided.</param>
		/// <param name="HoverColor">The color to change the image to when the mouse is hovered over where the button would be.</param>
		/// <param name="ClickEvent">The method to invoke when the user clicks on the added button.</param>
		/// <exception cref="ArgumentNullException">Thrown when information provided to one of the parameters is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">Thrown when more than one caption button is attempted to be added to the form's header.</exception>
		public void AddCustomCaptionButton(Image InteractionContent, Color OriginalImageColor, Color HoverColor, EventHandler ClickEvent)
		{
			if (InteractionContent == null)
				throw new ArgumentNullException("InteractionContent");
			else if (HoverColor == null)
				throw new ArgumentNullException("HoverColor");
			else if (ClickEvent == null)
				throw new ArgumentNullException("ClickEvent");

			if (!allowCaption1)
			{
				allowCaption1 = true;
				ctCaptionClr1_Orig = OriginalImageColor;
				ctCaptionClr1 = HoverColor;
				captionCnt1 = InteractionContent;
				captionClick1 = ClickEvent;
			}
			else if (!allowCaption2)
			{
				allowCaption2 = true;
				ctCaptionClr2_Orig = OriginalImageColor;
				ctCaptionClr2 = HoverColor;
				captionCnt2 = InteractionContent;
				captionClick2 = ClickEvent;
			}
			else if (allowCaption1 && allowCaption2)
				throw new InvalidOperationException("Cannot add another caption button to the form header because only 2 custom caption buttons maximum are supported.");
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if ((left.Contains(e.Location) || right.Contains(e.Location)) && canResize)
				Cursor = Cursors.SizeWE;
			else if ((top.Contains(e.Location) || bottom.Contains(e.Location)) && canResize)
				Cursor = Cursors.SizeNS;
			else if ((topLeft.Contains(e.Location) || bottomRight.Contains(e.Location)) && canResize)
				Cursor = Cursors.SizeNWSE;
			else if ((topRight.Contains(e.Location) || bottomLeft.Contains(e.Location)) && canResize)
				Cursor = Cursors.SizeNESW;
			else if (close.Contains(e.Location) ||
				(minimize.Contains(e.Location) && MinimizeBox) ||
				(MinimizeBox && MaximizeBox && maximize.Contains(e.Location) && canResize) ||
				(ctCaption1.Contains(e.Location) && allowCaption1) ||
				(ctCaption2.Contains(e.Location) && allowCaption2))
				Cursor = LoadFromRegistry("Hand") ?? Cursors.Hand;
			else Cursor = Cursors.Default;

			GC.Collect();
			Invalidate();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (close.Contains(e.Location))
				Close();
			else if (minimize.Contains(e.Location) && MinimizeBox)
				WindowState = FormWindowState.Minimized;
			else if (maximize.Contains(e.Location) && MinimizeBox && MaximizeBox && canResize)
			{
				if (WindowState == FormWindowState.Maximized)
					WindowState = FormWindowState.Normal;
				else if (WindowState == FormWindowState.Normal)
					WindowState = FormWindowState.Maximized;
			}
			else if (allowCaption1 && ctCaption1.Contains(e.Location))
				captionClick1.Invoke(this, e);
			else if (allowCaption2 && ctCaption2.Contains(e.Location))
				captionClick2.Invoke(this, e);
			else if (left.Contains(e.Location) && canResize)
			{
				ReleaseCapture();
				SendMessage(Handle, 161, 10, 0);
			}
			else if (right.Contains(e.Location) && canResize)
			{
				ReleaseCapture();
				SendMessage(Handle, 161, 11, 0);
			}
			else if (top.Contains(e.Location) && canResize)
			{
				ReleaseCapture();
				SendMessage(Handle, 161, 12, 0);
			}
			else if (topLeft.Contains(e.Location) && canResize)
			{
				ReleaseCapture();
				SendMessage(Handle, 161, 13, 0);
			}
			else if (topRight.Contains(e.Location) && canResize)
			{
				ReleaseCapture();
				SendMessage(Handle, 161, 14, 0);
			}
			else if (bottom.Contains(e.Location) && canResize)
			{
				ReleaseCapture();
				SendMessage(Handle, 161, 15, 0);
			}
			else if (bottomLeft.Contains(e.Location) && canResize)
			{
				ReleaseCapture();
				SendMessage(Handle, 161, 16, 0);
			}
			else if (bottomRight.Contains(e.Location) && canResize)
			{
				ReleaseCapture();
				SendMessage(Handle, 161, 17, 0);
			}
			else if (resizeBorder <= e.X && e.X <= Width - (resizeBorder * 2) - close.Width -
				(MinimizeBox ? (MaximizeBox ? maximize.Width : 0) + minimize.Width : 0) -
				(allowCaption1 ? headerHeight : 0) - (allowCaption2 ? headerHeight : 0) &&
				resizeBorder <= e.Y && e.Y <= headerHeight + (resizeBorder * 2) && canResize)
			{
				ReleaseCapture();
				SendMessage(Handle, 161, 2, 0);
			}
			else if (0 <= e.X && e.X <= Width - close.Width - (MinimizeBox ? minimize.Width : 0) -
					(allowCaption1 ? headerHeight : 0) - (allowCaption2 ? headerHeight : 0) &&
					0 <= e.Y && e.Y <= headerHeight + (resizeBorder * 2))
			{
				ReleaseCapture();
				SendMessage(Handle, 161, 2, 0);
			}
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			Invalidate();
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			#region Drop Shadow

			if (m_aeroEnabled && useWinDropShadow)
			{
				int v = 2;
				DwmSetWindowAttribute(Handle, 2, ref v, 4);
				MARGINS margins = new MARGINS
				{
					leftWidth = 0,
					rightWidth = 0,
					topHeight = 1,
					bottomHeight = 0,
				};
				DwmExtendFrameIntoClientArea(Handle, ref margins);
			}

			#endregion

			e.Graphics.SmoothingMode = SmoothingMode.None;
			e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			e.Graphics.TextContrast = 0;

			if (!useWinDropShadow) e.Graphics.FillRectangle(new SolidBrush(borderColor), 0, 0, Width, Height);
			e.Graphics.FillRectangle(new SolidBrush(BackColor),
				useWinDropShadow ? 0 : 1, useWinDropShadow ? 0 : 1, Width - (useWinDropShadow ? 0 : 2), Height - (useWinDropShadow ? 0 : 2));
			e.Graphics.FillRectangle(new SolidBrush(headerColor),
				useWinDropShadow ? 0 : 1, useWinDropShadow ? 0 : 1, Width - (useWinDropShadow ? 0 : 2), headerHeight + (resizeBorder * 2) - (useWinDropShadow ? 0 : 1));

			topLeft = new Rectangle(0, 0, resizeBorder, resizeBorder);
			top = new Rectangle(resizeBorder, 0, Width - (resizeBorder * 2), resizeBorder);
			topRight = new Rectangle(Width - resizeBorder, 0, resizeBorder, resizeBorder);
			left = new Rectangle(0, resizeBorder, resizeBorder, Height - (resizeBorder * 2));
			right = new Rectangle(Width - resizeBorder, resizeBorder, resizeBorder, Height - (resizeBorder * 2));
			bottomLeft = new Rectangle(0, Height - resizeBorder, resizeBorder, resizeBorder);
			bottom = new Rectangle(resizeBorder, Height - resizeBorder, Width - (resizeBorder * 2), resizeBorder);
			bottomRight = new Rectangle(Width - resizeBorder, Height - resizeBorder, resizeBorder, resizeBorder);

			Rectangle text = new Rectangle(resizeBorder + 2, resizeBorder + (useWinDropShadow ? 2 : 1), Width - (resizeBorder * 2) - headerHeight, headerHeight);
			if (ShowIcon)
			{
				text.X += headerHeight + 2;
				text.Width -= headerHeight;
			}
			if (MinimizeBox) text.Width -= headerHeight;
			if (MaximizeBox && canResize) text.Width -= headerHeight;
			if (allowCaption1) text.Width -= headerHeight;
			if (allowCaption2) text.Width -= headerHeight;

			if (ShowIcon)
				e.Graphics.DrawIcon(Icon, new Rectangle(
					resizeBorder + (useWinDropShadow ? 1 : 0), resizeBorder + (useWinDropShadow ? 1 : 0),
					headerHeight, headerHeight + 1));

			e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), text,
				new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });

			close = new Rectangle(Width - headerHeight - resizeBorder, resizeBorder, headerHeight, headerHeight);
			Point mouse = PointToClient(MousePosition);

			e.Graphics.DrawLine(new Pen(close.Contains(mouse) ? closeColor : ForeColor, 1.0f),
				new Point(close.X + 6, close.Y + 6),
				new Point(close.X + close.Width - 6, close.Y + close.Height - 6));
			e.Graphics.DrawLine(new Pen(close.Contains(mouse) ? closeColor : ForeColor, 1.0f),
				new Point(close.X + close.Width - 6, close.Y + 6),
				new Point(close.X + 6, close.Y + close.Height - 6));

			if (MinimizeBox)
			{
				minimize = new Rectangle(close.X - headerHeight, close.Y, headerHeight, headerHeight);

				if (MaximizeBox && canResize)
				{
					maximize = new Rectangle(close.X - headerHeight, minimize.Y, headerHeight, headerHeight);
					minimize = new Rectangle(maximize.X - headerHeight, maximize.Y, headerHeight, headerHeight);

					if (WindowState == FormWindowState.Maximized)
					{
						e.Graphics.DrawLine(new Pen(maximize.Contains(mouse) ? maximizeColor : ForeColor, 1.0f),
							new Point(maximize.X + 6, maximize.Y + 8),
							new Point(maximize.X + 6, maximize.Y + maximize.Height - 6));
						e.Graphics.DrawLine(new Pen(maximize.Contains(mouse) ? maximizeColor : ForeColor, 1.0f),
							new Point(maximize.X + 6, maximize.Y + maximize.Height - 6),
							new Point(maximize.X + maximize.Width - 8, maximize.Y + maximize.Height - 6));
						e.Graphics.DrawLine(new Pen(maximize.Contains(mouse) ? maximizeColor : ForeColor, 1.0f),
							new Point(maximize.X + maximize.Width - 8, maximize.Y + maximize.Height - 6),
							new Point(maximize.X + maximize.Width - 8, maximize.Y + 8));
						e.Graphics.DrawLine(new Pen(maximize.Contains(mouse) ? maximizeColor : ForeColor, 1.0f),
							new Point(maximize.X + maximize.Width - 8, maximize.Y + 8),
							new Point(maximize.X + 6, maximize.Y + 8));

						e.Graphics.DrawLine(new Pen(maximize.Contains(mouse) ? maximizeColor : ForeColor, 1.0f),
							new Point(maximize.X + 8, maximize.Y + 8),
							new Point(maximize.X + 8, maximize.Y + 6));
						e.Graphics.DrawLine(new Pen(maximize.Contains(mouse) ? maximizeColor : ForeColor, 1.0f),
							new Point(maximize.X + 8, maximize.Y + 6),
							new Point(maximize.X + maximize.Width - 6, maximize.Y + 6));
						e.Graphics.DrawLine(new Pen(maximize.Contains(mouse) ? maximizeColor : ForeColor, 1.0f),
							new Point(maximize.X + maximize.Width - 6, maximize.Y + 6),
							new Point(maximize.X + maximize.Width - 6, maximize.Y + maximize.Height - 8));
						e.Graphics.DrawLine(new Pen(maximize.Contains(mouse) ? maximizeColor : ForeColor, 1.0f),
							new Point(maximize.X + maximize.Width - 6, maximize.Y + maximize.Height - 8),
							new Point(maximize.X + maximize.Width - 8, maximize.Y + maximize.Height - 8));
					}
					else if (WindowState == FormWindowState.Normal)
					{
						e.Graphics.DrawLine(new Pen(maximize.Contains(mouse) ? maximizeColor : ForeColor, 1.0f),
							new Point(maximize.X + 6, maximize.Y + 6),
							new Point(maximize.X + 6, maximize.Y + maximize.Height - 6));
						e.Graphics.DrawLine(new Pen(maximize.Contains(mouse) ? maximizeColor : ForeColor, 1.0f),
							new Point(maximize.X + 6, maximize.Y + maximize.Height - 6),
							new Point(maximize.X + maximize.Width - 6, maximize.Y + maximize.Height - 6));
						e.Graphics.DrawLine(new Pen(maximize.Contains(mouse) ? maximizeColor : ForeColor, 1.0f),
							new Point(maximize.X + maximize.Width - 6, maximize.Y + maximize.Height - 6),
							new Point(maximize.X + maximize.Width - 6, maximize.Y + 6));
						e.Graphics.DrawLine(new Pen(maximize.Contains(mouse) ? maximizeColor : ForeColor, 1.0f),
							new Point(maximize.X + maximize.Width - 6, maximize.Y + 6),
							new Point(maximize.X + 6, maximize.Y + 6));
					}
				}

				e.Graphics.DrawLine(new Pen(minimize.Contains(mouse) ? minimizeColor : ForeColor, 1.0f),
					new Point(minimize.X + 6, minimize.Y + (minimize.Width / 2) + 1),
					new Point(minimize.X + minimize.Width - 6, minimize.Y + (minimize.Width / 2) + 1));

				if (allowCaption1)
				{
					ctCaption1 = new Rectangle(minimize.X - headerHeight, minimize.Y, headerHeight, headerHeight);

					ImageAttributes imageAttr = new ImageAttributes();
					ColorMap[] remapTable =
					{
						new ColorMap
						{
							OldColor = ctCaptionClr1_Orig,
							NewColor = ctCaption1.Contains(mouse) ? ctCaptionClr1 : ForeColor,
						},
					};

					imageAttr.SetRemapTable(remapTable);
					e.Graphics.DrawImage(captionCnt1,
						new Rectangle(ctCaption1.X + 2, ctCaption1.Y + 2, ctCaption1.Width - 4, ctCaption1.Height - 4),
						0, 0, captionCnt1.Width, captionCnt1.Height, GraphicsUnit.Pixel, imageAttr);
				}

				if (allowCaption2)
				{
					ctCaption2 = new Rectangle(ctCaption1.X - headerHeight, ctCaption1.Y, headerHeight, headerHeight);

					ImageAttributes imageAttr = new ImageAttributes();
					ColorMap[] remapTable =
					{
						new ColorMap
						{
							OldColor = ctCaptionClr2_Orig,
							NewColor = ctCaption2.Contains(mouse) ? ctCaptionClr2 : ForeColor,
						},
					};

					imageAttr.SetRemapTable(remapTable);
					e.Graphics.DrawImage(captionCnt2,
						new Rectangle(ctCaption2.X + 2, ctCaption2.Y + 2, ctCaption2.Width - 4, ctCaption2.Height - 4),
						0, 0, captionCnt2.Width, captionCnt2.Height, GraphicsUnit.Pixel, imageAttr);
				}
			}
		}

		protected override void OnClientSizeChanged(EventArgs e)
		{
			base.OnClientSizeChanged(e);

			switch (WindowState)
			{
				case FormWindowState.Maximized:
				case FormWindowState.Minimized:
				case FormWindowState.Normal:
					OnWindowStateChanged(e);
					break;
			}
		}

		protected override void OnResizeEnd(EventArgs e)
		{
			base.OnResizeEnd(e);

			if (canResize)
			{
				int screenY = Screen.FromPoint(Location).Bounds.Location.Y;
				int screenX = Screen.FromPoint(Location).Bounds.Location.X;
				int screenX2 = Screen.FromPoint(Location).Bounds.Location.X + Screen.FromPoint(Location).Bounds.Size.Width;

				bool wndSnap = Location.Y <= screenY + 6 && Location.Y >= screenY;
				bool mouseSnap = MousePosition.Y <= screenY + 6 && MousePosition.Y >= screenY;

				if (wndSnap || mouseSnap)
				{
					WindowState = FormWindowState.Maximized;
				}
			}
		}

		#region Custom Drop-Shadow

		private bool m_aeroEnabled = false;

		[DllImport("dwmapi.dll")]
		private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

		[DllImport("dwmapi.dll")]
		private static extern int DwmSetWindowAttribute(IntPtr hWnd, int attr, ref int attrValue, int attrSize);

		[DllImport("dwmapi.dll")]
		private static extern int DwmIsCompositionEnabled(ref int pfEnabled);

		private bool CheckAeroEnabled()
		{
			if (Environment.OSVersion.Version.Major >= 6)
			{
				int enabled = 0;
				DwmIsCompositionEnabled(ref enabled);

				return enabled == 1;
			}

			return false;
		}

		private struct MARGINS
		{
			public int leftWidth;
			public int rightWidth;
			public int topHeight;
			public int bottomHeight;
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				m_aeroEnabled = CheckAeroEnabled();

				if (!m_aeroEnabled && useWinDropShadow)
					cp.ClassStyle |= 0x20000;

				return cp;
			}
		}

		#endregion
	}
}
