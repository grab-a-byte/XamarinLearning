using System.Collections.Generic;
using System;
using System.Net;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using XamNative.Core.Entities;
using Android.Net;

namespace Learning.Android.ViewAdapters
{
    public class POIListViewAdapter : BaseAdapter<PointOfInterest>
    {
        private readonly Activity _context;
        private readonly List<PointOfInterest> _poiListData;

        public POIListViewAdapter(Activity context, List<PointOfInterest> poiListData)
        {
            _context = context;
            _poiListData = poiListData;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.POIListItem, null);

            PointOfInterest poi = this[position];
            view.FindViewById<TextView>(Resource.Id.nameTextView).Text = poi.Name;

            if (String.IsNullOrEmpty(poi.Address))
            {
                view.FindViewById<TextView>(Resource.Id.addrTextView).Visibility = ViewStates.Gone;
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.addrTextView).Text = poi.Address;
            }

            if (string.IsNullOrEmpty(poi.Image))
            {
                view.FindViewById<ImageView>(Resource.Id.poiImageView).SetImageBitmap(GetImageBitmapFromUrl(poi.Image).Result);
            }

            return view;
        }

        public override int Count => _poiListData.Count;

        public override PointOfInterest this[int index] => _poiListData[index];

        private async Task<Bitmap> GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes =  await webClient.DownloadDataTaskAsync(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}