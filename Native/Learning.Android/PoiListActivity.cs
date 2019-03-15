using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Learning.Android.Services;
using Learning.Android.ViewAdapters;
using Newtonsoft.Json;
using Org.Json;
using XamNative.Core.Entities;
using XamNative.Core.Interfaces;
using XamNative.Core.Services;

namespace Learning.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class PoiListActivity : AppCompatActivity
    {
        private ListView _poiListView;
        private ProgressBar _progressBar;
        private List<PointOfInterest> _poiListData;
        private POIListViewAdapter _poiListViewAdapter;
        private INetworkState _networkState;
        private IPOIService _poiService;
        private int _scrollPosition = 0;

        public PoiListActivity()
        {
            _networkState = new NetworkState(this); 
        }

        public async void DownloadPoisListAsync()
        {
            if (!_networkState.IsConnected())
            {
                Toast toast = Toast.MakeText(this, "not connected to the internet please check network device settings", ToastLength.Short);
                toast.Show();
            }
            else
            {
                POIService service = new POIService();
                _progressBar.Visibility = ViewStates.Visible;
                _poiListData = await service.GetPOIListAsync();
                _progressBar.Visibility = ViewStates.Gone;

                _poiListViewAdapter = new POIListViewAdapter(this, _poiListData);
                _poiListView.Adapter = _poiListViewAdapter;

                _poiListView.ItemClick += POIClicked;

                _poiListView.Post(() => { _poiListView.SetSelection(_scrollPosition); });
            }
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            _scrollPosition = savedInstanceState.GetInt("scroll_position");
        }

        protected override void OnCreate(Bundle savedInstanceState)
        { 
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.POIList);

            _poiListView = FindViewById<ListView>(Resource.Id.poiListView);
            _progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);

            DownloadPoisListAsync();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.POIListViewMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.actionNew:
                    StartActivity(typeof(POIDetailActivity));
                    return true;
                case Resource.Id.actionRefresh:
                    DownloadPoisListAsync();
                    return true;
                default: 
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected void POIClicked(object sender, ListView.ItemClickEventArgs args)
        {
            PointOfInterest poi = _poiListData[args.Position];
            var intent = new Intent(this, typeof(POIDetailActivity));
            var jsonPOI = JsonConvert.SerializeObject(poi);
            intent.PutExtra("poi", jsonPOI);
            StartActivity(intent);

            Console.Out.WriteLine("POI Clicked, Name Is {0}", poi.Name);
        }

        protected override void OnResume()
        {
            base.OnResume();
            DownloadPoisListAsync();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            int currentPosition = _poiListView.FirstVisiblePosition;
            outState.PutInt("scroll_position", currentPosition);
        }
    }
}