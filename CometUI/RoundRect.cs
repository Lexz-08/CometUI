using System.Drawing.Drawing2D;

namespace CometUI
{
	internal class RoundRect
	{
		public static GraphicsPath CreateRoundRect(int x, int y, int w, int h, int r)
		{
			int r2 = r * 2;
			int r3 = (int)(r * 2.2);
			int r4 = (int)(r * 2.1);
			int offset = 1;

			GraphicsPath path = new GraphicsPath();
			path.StartFigure();
			path.AddArc(x, y, r2, r2, 180, 90);
			path.AddArc(x + w - r2 - offset, y, r2, r4, 270, 90);
			path.AddArc(x + w - r3 - offset, y + h - r3 - offset, r3, r3, 0, 90);
			path.AddArc(x, y + h - r4 - offset, r2, r4, 90, 90);
			path.CloseFigure();

			return path;
		}
	}
}
