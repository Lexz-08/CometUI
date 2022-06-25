using System.Windows.Forms;

namespace CometUI
{
	public enum CometFileInputType
	{
		/// <summary>
		/// Specifies that a given <see cref="CometFileInput"/> should open a <see cref="SaveFileDialog"/> when the "<b>Choose File</b>" button is clicked.
		/// </summary>
		SaveFile,

		/// <summary>
		/// Specifies that a given <see cref="CometFileInput"/> should open an <see cref="OpenFileDialog"/> when the "<b>Choose File</b>" button is clicked.
		/// </summary>
		OpenFile,
	}
}
