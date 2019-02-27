using System;
using System.Windows.Forms;

namespace WindowsFormsApp
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void btnHello_Click(object sender, EventArgs e)
		{
			string name = this.tbInputName.Text;

			MessageBox.Show($"Hello, {name}!");
		}
	}
}
