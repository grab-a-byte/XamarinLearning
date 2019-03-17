using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Learning.Android.Fragments;

namespace Learning.Android
{
    [Activity(Label = "POIDetailActivity")]
    public class POIDetailActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.POIDetail);

            var detailFragment = new PoiDetailFragment();
            detailFragment.Arguments = new Bundle();
            if (Intent.HasExtra("poi"))
            {
                string poiJson = Intent.GetStringExtra("poi");
                detailFragment.Arguments.PutString("poi", poiJson);
            }

            FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
            fragmentTransaction.Add(Resource.Id.poiDetailLayout, detailFragment);
            fragmentTransaction.Commit();
        }
    }
}