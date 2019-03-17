using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Learning.Android.Services;
using Learning.Android.ViewAdapters;
using Newtonsoft.Json;
using XamNative.Core.Entities;
using XamNative.Core.Interfaces;
using XamNative.Core.Services;

namespace Learning.Android.Fragments
{
    public class PoiListFragment : ListFragment
    {
        private ProgressBar _progressBar;
        private List<PointOfInterest> _poiListData;
        private POIListViewAdapter _poiListViewAdapter;
        private Activity _activity;
        private INetworkState _networkState;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
            if (context is Activity activity)
            {
                _activity = activity;
                _networkState = new NetworkState(context);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view =  LayoutInflater.Inflate(Resource.Layout.PoiListFragment, container, false);

            _progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBar);

            SetHasOptionsMenu(true);
            return view;
        }

        public async void DownloadPoisListAsync()
        {
            if (!_networkState.IsConnected())
            {
                Toast toast = Toast.MakeText(_activity, "not connected to the internet please check network device settings", ToastLength.Short);
                toast.Show();
            }
            else
            {
                POIService service = new POIService();
                _progressBar.Visibility = ViewStates.Visible;
                _poiListData = await service.GetPOIListAsync();
                _progressBar.Visibility = ViewStates.Gone;

                _poiListViewAdapter = new POIListViewAdapter(_activity, _poiListData);

                this.ListAdapter = _poiListViewAdapter;
            }
        }

        public override void OnResume()
        {
            DownloadPoisListAsync();
            base.OnResume();
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater menuInflater)
        {
            menuInflater.Inflate(Resource.Menu.POIListViewMenu, menu);
            base.OnCreateOptionsMenu(menu, menuInflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.actionNew:
                    var intent = new Intent(_activity, typeof(POIDetailActivity));
                    StartActivity(intent);
                    return true;
                case Resource.Id.actionRefresh:
                    DownloadPoisListAsync();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            //PointOfInterest poi = _poiListData[position];
            //var intent = new Intent(_activity, typeof(POIDetailActivity));
            //var jsonPOI = JsonConvert.SerializeObject(poi);
            //intent.PutExtra("poi", jsonPOI);
            //StartActivity(intent);

            //Console.Out.WriteLine("POI Clicked, Name Is {0}", poi.Name);
        }
    }
}