using System;
using Gtk;
using StandartClassLibrary;

public partial class MainWindow : Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnBtnHelloClicked(object sender, EventArgs e)
    {
		string helloMessage = HelloBuilder.BuildHelloMessage(this.entryName.Text);

        MessageDialog messageDialog = new MessageDialog(
			this,
			DialogFlags.Modal,
			MessageType.Info,
			ButtonsType.Ok,
			helloMessage);

        messageDialog.Run();
        messageDialog.Destroy();
    }
}
