using System.Drawing;

namespace CometUI
{
	internal static class ColorHelper
	{
		public static byte Brightness(this Color c) => (byte)((c.R + c.G + c.B) / 3);
	}
}
