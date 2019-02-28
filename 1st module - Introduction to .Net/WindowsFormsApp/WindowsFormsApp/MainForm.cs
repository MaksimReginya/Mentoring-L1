using System;
using System.Windows.Forms;
using StandartClassLibrary;

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
			string helloMessage = HelloBuilder.BuildHelloMessage(name);

			MessageBox.Show(helloMessage);
		}
	}
}
