namespace CometUI
{
	partial class CometLogConsole
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.CometConsole_Editor = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// CometConsole_Editor
			// 
			this.CometConsole_Editor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.CometConsole_Editor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
			this.CometConsole_Editor.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.CometConsole_Editor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.CometConsole_Editor.Location = new System.Drawing.Point(12, 46);
			this.CometConsole_Editor.Name = "CometConsole_Editor";
			this.CometConsole_Editor.ReadOnly = true;
			this.CometConsole_Editor.Size = new System.Drawing.Size(500, 314);
			this.CometConsole_Editor.TabIndex = 0;
			this.CometConsole_Editor.Text = "";
			// 
			// CometConsole
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(524, 372);
			this.Controls.Add(this.CometConsole_Editor);
			this.Name = "CometConsole";
			this.Text = "CometConsole";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox CometConsole_Editor;
	}
}