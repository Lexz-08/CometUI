using System.Drawing;
using System.Drawing.Drawing2D;

namespace CometUI
{
	internal static class RoundRect
	{
		public static GraphicsPath Roundify(int x, int y, int w, int h, int radius)
		{
			return Roundify(new Rectangle(x, y, w, h), radius);
		}

		private static GraphicsPath Roundify(Rectangle rect, int radius)
		{
			GraphicsPath path = new GraphicsPath();
			radius *= 2;

			path.StartFigure();
			path.AddArc(rect.X, rect.X, radius, radius, 180, 90);
			path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
			path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
			path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
			path.CloseFigure();

			return path;
		}
	}
}
