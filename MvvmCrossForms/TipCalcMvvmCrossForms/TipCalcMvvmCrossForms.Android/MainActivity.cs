using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Views;
using TipCalcMvvmCrossForms.UI;
using Android.Locations;

namespace TipCalcMvvmCrossForms.Droid
{
    [Activity(
        Label = "TipCalc.Forms.Droid",
        Icon = "@drawable/icon",
        Theme = "@style/MyTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        LaunchMode = LaunchMode.SingleTask)]
    public class MainActivity : MvxFormsAppCompatActivity<Setup, App, FormsApp>
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Mvx.IoCProvider.RegisterType(() => (LocationManager)GetSystemService(LocationService));

            base.OnCreate(bundle);
        }
    }
}