using System;
using Xamarin.Forms;
using StandartClassLibrary;

namespace XamarinForms
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			string helloMessage = HelloBuilder.BuildHelloMessage(entryName.Text);

			lblName.Text = helloMessage;
		}
	}
}
