using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CometUI
{
	public class CometTabControl : TabControl
	{
		private Color tabColor = Color.FromArgb(100, 100, 100);
		private bool underlineTab = false;
		private Color backColor = Color.FromArgb(25, 25, 25);
		private Color foreColor = Color.FromArgb(230, 230, 230);
		private Color unselectedForeColor = Color.FromArgb(200, 200, 200);

		/// <summary>
		/// The color used to draw the tabs.
		/// </summary>
		[Description("The color used to draw the tabs.")]
		public Color TabColor
		{
			get { return tabColor; }
			set { tabColor = value; Invalidate(); }
		}

		/// <summary>
		/// Determines whether or not each tab is rendered as an underline rather than an actual background.
		/// </summary>
		[Description("Determines whether or not each tab is rendered as an underline rather than an actual background.")]
		public bool UnderlineTab
		{
			get { return underlineTab; }
			set { underlineTab = value; Invalidate(); }
		}

		/// <summary>
		/// The background color of the control.
		/// </summary>
		[Description("The background color of the control.")]
		public Color Background
		{
			get { return backColor; }
			set { backColor = value; OnBackColorChanged(null); Invalidate(); }
		}

		/// <summary>
		/// The foreground color of the tab(s) (the tab's' text color).
		/// </summary>
		[Description("The foreground color of the tab(s) (the tab's' text color).")]
		public Color Foreground
		{
			get { return foreColor; }
			set { foreColor = value; OnForeColorChanged(null); Invalidate(); }
		}

		/// <summary>
		/// The color used to draw tab-text for unselected tabs.
		/// </summary>
		[Description("The color used to draw tab-text for unselected tabs.")]
		public Color UnselectedForeColor
		{
			get { return unselectedForeColor; }
			set { unselectedForeColor = value; Invalidate(); }
		}

		public CometTabControl()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint |
					ControlStyles.UserPaint |
					ControlStyles.ResizeRedraw |
					ControlStyles.OptimizedDoubleBuffer |
					ControlStyles.SupportsTransparentBackColor, true);
			DoubleBuffered = true;

			Font = new Font("Segoe UI", 10.0f);
			backColor = Color.FromArgb(25, 25, 25);
			foreColor = Color.FromArgb(230, 230, 230);

			ItemSize = new Size(120, 30);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			e.Graphics.Clear(backColor);

			e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			e.Graphics.TextContrast = 0;

			for (int i = 0; i < TabCount; i++)
			{
				Rectangle tab = GetTabRect(i);

				if (i == SelectedIndex)
				{
					if (underlineTab)
						e.Graphics.FillRectangle(new SolidBrush(tabColor), tab.X + 2, tab.Height - 2, tab.Width - 4, 2);
					else e.Graphics.FillRectangle(new SolidBrush(tabColor), tab.X + 2, tab.Y, tab.Width - 4, tab.Height);
				}
				else
				{
					if (underlineTab)
						e.Graphics.FillRectangle(new SolidBrush(backColor), tab.X + 2, tab.Height - 2, tab.Width - 4, 2);
					else e.Graphics.FillRectangle(new SolidBrush(backColor), tab.X + 2, tab.Y, tab.Width - 4, tab.Height);
				}

				e.Graphics.DrawString(TabPages[i].Text, Font, new SolidBrush(i == SelectedIndex ? foreColor : unselectedForeColor),
					new Rectangle(tab.X + 2, tab.Y + (underlineTab ? -1 : 0), tab.Width - 4, tab.Height),
					new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
			}
		}
	}
}
