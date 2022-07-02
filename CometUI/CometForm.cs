﻿using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
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
		private bool canResize = true;
		private bool useWinDropShadow = true;
		private FormWindowState winState;

		/// <summary>
		/// The background color for the CometForm's header.
		/// </summary>
		[Description("The background color for the CometForm's header.")]
		public Color HeaderColor
		{
			get { return headerColor; }
			set { headerColor = value; Invalidate(); }
		}

		/// <summary>
		/// The foreground color for the CometForm's close button.
		/// </summary>
		[Description("The foreground color for the CometForm's close button.")]
		public Color CloseColor
		{
			get { return closeColor; }
			set { closeColor = value; Invalidate(); }
		}

		/// <summary>
		/// The foreground color for the CometForm's minimize button.
		/// </summary>
		[Description("The foreground color for the CometForm's minimize button.")]
		public Color MinimizeColor
		{
			get { return minimizeColor; }
			set { minimizeColor = value; Invalidate(); }
		}

		/// <summary>
		/// The foreground color for the CometForm's maximize button.
		/// </summary>
		[Description("The foreground color for the CometForm's maximize button.")]
		public Color MaximizeColor
		{
			get { return maximizeColor; }
			set { maximizeColor = value; Invalidate(); }
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
		/// Determines whether or not the CometForm will use the current Windows drop-shadow.<br/><br/>
		/// <b>Important Note</b>: Only works if Aero theme is enabled on the device.
		/// </summary>
		[Description("Determines whether or not the CometForm will use the curent Windows drop-shadow.\n\nImportant Note: Only works if Aero theme is enabled on the device.")]
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

			winState = WindowState;
		}

		private Rectangle left, topLeft, bottomLeft,
			right, topRight, bottomRight,
			top, bottom, close, minimize, maximize;

		private const int headerHeight = 22,
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

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			Cursor hand = LoadFromRegistry("Hand") ?? Cursors.Hand;

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
				(MinimizeBox && MaximizeBox && maximize.Contains(e.Location) && canResize))
				Cursor = hand;
			else Cursor = Cursors.Default;

			Invalidate();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (left.Contains(e.Location) && canResize)
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
				(MinimizeBox ? (MaximizeBox ? maximize.Width : 0) + minimize.Width : 0) &&
				resizeBorder <= e.Y && e.Y <= headerHeight + (resizeBorder * 2) && canResize)
			{
				ReleaseCapture();
				SendMessage(Handle, 161, 2, 0);
			}
			else if (0 <= e.X && e.X <= Width - close.Width - (MinimizeBox ? minimize.Width : 0) &&
					0 <= e.Y && e.Y <= headerHeight + (resizeBorder * 2))
			{
				ReleaseCapture();
				SendMessage(Handle, 161, 2, 0);
			}
			else if (close.Contains(e.Location))
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

			e.Graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, Width, Height);
			e.Graphics.FillRectangle(new SolidBrush(headerColor), 0, 0, Width, headerHeight + (resizeBorder * 2));

			topLeft = new Rectangle(0, 0, resizeBorder, resizeBorder);
			top = new Rectangle(resizeBorder, 0, Width - (resizeBorder * 2), resizeBorder);
			topRight = new Rectangle(Width - resizeBorder, 0, resizeBorder, resizeBorder);
			left = new Rectangle(0, resizeBorder, resizeBorder, Height - (resizeBorder * 2));
			right = new Rectangle(Width - resizeBorder, resizeBorder, resizeBorder, Height - (resizeBorder * 2));
			bottomLeft = new Rectangle(0, Height - resizeBorder, resizeBorder, resizeBorder);
			bottom = new Rectangle(resizeBorder, Height - resizeBorder, Width - (resizeBorder * 2), resizeBorder);
			bottomRight = new Rectangle(Width - resizeBorder, Height - resizeBorder, resizeBorder, resizeBorder);

			Rectangle text = new Rectangle(resizeBorder + 2, resizeBorder + 1, Width - (resizeBorder * 2) - headerHeight, headerHeight);
			if (ShowIcon)
			{
				text.X += headerHeight + 2;
				text.Width -= headerHeight;
			}
			if (MinimizeBox) text.Width -= headerHeight;
			if (MaximizeBox && canResize) text.Width -= headerHeight;

			if (ShowIcon)
				e.Graphics.DrawIcon(Icon, new Rectangle(resizeBorder, resizeBorder, headerHeight, headerHeight + 1));

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

			e.Graphics.DrawLine(new Pen(Color.FromArgb(100, Color.White), 1.0f),
				new Point(close.X + 6, close.Y + 6),
				new Point(close.X + close.Width - 6, close.Y + close.Height - 6));
			e.Graphics.DrawLine(new Pen(Color.FromArgb(100, Color.White), 1.0f),
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
