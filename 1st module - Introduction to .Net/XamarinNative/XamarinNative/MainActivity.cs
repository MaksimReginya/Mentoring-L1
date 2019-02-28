using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

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

			Button btn = FindViewById<Button>(Resource.Id.btnHello);
			btn.Click += this.Btn_Click;
        }

		private void Btn_Click(object sender, System.EventArgs e)
		{
			TextView tvName = FindViewById<TextView>(Resource.Id.tvName);

			tvName.Text = "Hello, world!";
		}
	}
}