using System;
using System.IO;
using System.Windows.Forms;

namespace CometUI
{
	public partial class CometLogConsole : CometForm
	{
		private ConsoleWriter writer = null;

		public CometLogConsole()
		{
			InitializeComponent();

			writer = new ConsoleWriter(CometConsole_Editor);
			Console.SetOut(writer);
		}

		public void Write(bool value) => writer.Write(value);
		public void Write(char value) => writer.Write(value);
		public void Write(char[] buffer) => writer.Write(buffer);
		public void Write(char[] buffer, int index, int count) => writer.Write(buffer, index, count);
		public void Write(decimal value) => writer.Write(value);
		public void Write(double value) => writer.Write(value);
		public void Write(float value) => writer.Write(value);
		public void Write(int value) => writer.Write(value);
		public void Write(long value) => writer.Write(value);
		public void Write(object value) => writer.Write(value);
		public void Write(string format, object arg0) => writer.Write(format, arg0);
		public void Write(string format, object arg0, object arg1) => writer.Write(format, arg0, arg1);
		public void Write(string format, object arg0, object arg1, object arg2) => writer.Write(format, arg0, arg1, arg2);
		public void Write(string format, params object[] arg) => writer.Write(format, arg);
		public void Write(string value) => writer.Write(value);
		public void Write(uint value) => writer.Write(value);
		public void Write(ulong value) => writer.Write(value);

		public void WriteLine() => writer.WriteLine();
		public void WriteLine(bool value) => writer.WriteLine(value);
		public void WriteLine(char value) => writer.WriteLine(value);
		public void WriteLine(char[] buffer) => writer.WriteLine(buffer);
		public void WriteLine(char[] buffer, int index, int count) => writer.Write(buffer, index, count);
		public void WriteLine(decimal value) => writer.WriteLine(value);
		public void WriteLine(double value) => writer.WriteLine(value);
		public void WriteLine(float value) => writer.WriteLine(value);
		public void WriteLine(int value) => writer.WriteLine(value);
		public void WriteLine(long value) => writer.WriteLine(value);
		public void WriteLine(object value) => writer.WriteLine(value);
		public void WriteLine(string format, object arg0) => writer.WriteLine(format, arg0);
		public void WriteLine(string format, object arg0, object arg1) => writer.WriteLine(format, arg0, arg1);
		public void WriteLine(string format, object arg0, object arg1, object arg2) => writer.WriteLine(format, arg0, arg1, arg2);
		public void WriteLine(string format, params object[] arg) => writer.WriteLine(format, arg);
		public void WriteLine(string value) => writer.WriteLine(value);
		public void WriteLine(uint value) => writer.WriteLine(value);
		public void WriteLine(ulong value) => writer.WriteLine(value);
	}

	internal class ConsoleWriter : StringWriter
	{
		private RichTextBox output;

		public ConsoleWriter(RichTextBox output)
		{
			this.output = output;
		}

		public override void Write(bool value)
		{
			Write(value.ToString().ToLower());
		}
		public override void Write(char value)
		{
			Write(value);
		}
		public override void Write(char[] buffer)
		{
			Write(buffer);
		}
		public override void Write(char[] buffer, int index, int count)
		{
			for (int i = index; i < count; i++)
				Write(buffer[i]);
		}
		public override void Write(decimal value)
		{
			Write(value.ToString());
		}
		public override void Write(double value)
		{
			Write(value.ToString());
		}
		public override void Write(float value)
		{
			Write(value.ToString());
		}
		public override void Write(int value)
		{
			Write(value.ToString());
		}
		public override void Write(long value)
		{
			Write(value.ToString());
		}
		public override void Write(object value)
		{
			Write(value.ToString());
		}
		public override void Write(string format, object arg0)
		{
			Write(string.Format(format, arg0));
		}
		public override void Write(string format, object arg0, object arg1)
		{
			Write(string.Format(format, arg0, arg1));
		}
		public override void Write(string format, object arg0, object arg1, object arg2)
		{
			Write(string.Format(format, arg0, arg1, arg2));
		}
		public override void Write(string format, params object[] arg)
		{
			Write(string.Format(format, arg));
		}
		public override void Write(string value)
		{
			output.Text += value;
		}
		public override void Write(uint value)
		{
			Write(value.ToString());
		}
		public override void Write(ulong value)
		{
			Write(value.ToString());
		}

		public override void WriteLine()
		{
			Write("\n");
		}
		public override void WriteLine(bool value)
		{
			WriteLine(value.ToString().ToLower());
		}
		public override void WriteLine(char value)
		{
			WriteLine(value);
		}
		public override void WriteLine(char[] buffer)
		{
			WriteLine(buffer);
		}
		public override void WriteLine(char[] buffer, int index, int count)
		{
			for (int i = index; i < count; i++)
				WriteLine(buffer[i]);
		}
		public override void WriteLine(decimal value)
		{
			WriteLine(value.ToString());
		}
		public override void WriteLine(double value)
		{
			WriteLine(value.ToString());
		}
		public override void WriteLine(float value)
		{
			WriteLine(value.ToString());
		}
		public override void WriteLine(int value)
		{
			WriteLine(value.ToString());
		}
		public override void WriteLine(long value)
		{
			WriteLine(value.ToString());
		}
		public override void WriteLine(object value)
		{
			WriteLine(value.ToString());
		}
		public override void WriteLine(string format, object arg0)
		{
			WriteLine(string.Format(format, arg0));
		}
		public override void WriteLine(string format, object arg0, object arg1)
		{
			WriteLine(string.Format(format, arg0, arg1));
		}
		public override void WriteLine(string format, object arg0, object arg1, object arg2)
		{
			WriteLine(string.Format(format, arg0, arg1, arg2));
		}
		public override void WriteLine(string format, params object[] arg)
		{
			WriteLine(string.Format(format, arg));
		}
		public override void WriteLine(string value)
		{
			Write(value + "\n");
		}
		public override void WriteLine(uint value)
		{
			WriteLine(value.ToString());
		}
		public override void WriteLine(ulong value)
		{
			WriteLine(value.ToString());
		}
	}
}
