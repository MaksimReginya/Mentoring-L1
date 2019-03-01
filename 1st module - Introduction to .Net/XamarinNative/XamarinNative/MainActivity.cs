using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using StandartClassLibrary;

namespace XamarinNative
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

			Button btn = this.FindViewById<Button>(Resource.Id.btnHello);
			btn.Click += this.Btn_Click;
        }

		private void Btn_Click(object sender, System.EventArgs e)
		{
			EditText etInputName = this.FindViewById<EditText>(Resource.Id.etInputName);
			TextView tvOutputHello = this.FindViewById<TextView>(Resource.Id.tvOutputHello);

			string helloMessage = HelloBuilder.BuildHelloMessage(etInputName.Text);
			tvOutputHello.Text = helloMessage;
		}
	}
}