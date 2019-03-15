using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Learning.Android.Services;
using Newtonsoft.Json;
using XamNative.Core.Entities;
using XamNative.Core.Interfaces;
using XamNative.Core.Services;

namespace Learning.Android.Fragments
{
    public class PoiDetailFragment : Fragment
    {
        private PointOfInterest _pointOfInterest;

        private EditText _namedEditText;
        private EditText _descrEditText;
        private EditText _addressEditText;
        private EditText _latEditText;
        private EditText _longEditText;

        private Activity _activity;
        private IPOIService _poiService = new POIService();
        private INetworkState _networkState;

        private bool _hasErrors = false;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (Arguments != null && Arguments.ContainsKey("poi"))
            {
                string poiJson = Arguments.GetString("poi");
                _pointOfInterest = JsonConvert.DeserializeObject<PointOfInterest>(poiJson);
            }
            else
            {
                _pointOfInterest = new PointOfInterest();
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = LayoutInflater.Inflate(Resource.Layout.POIDetailFragment, container, false);

            _namedEditText = view.FindViewById<EditText>(Resource.Id.nameEditText);
            _descrEditText = view.FindViewById<EditText>(Resource.Id.descEditText);
            _addressEditText = view.FindViewById<EditText>(Resource.Id.addrEditText);
            _latEditText = view.FindViewById<EditText>(Resource.Id.editLatitude);
            _longEditText = view.FindViewById<EditText>(Resource.Id.editLongitude);

            UpdateUI();

            SetHasOptionsMenu(true);

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.POIDetailMenu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            base.OnPrepareOptionsMenu(menu);
            if (_pointOfInterest.Id <= 0)
            {
                IMenuItem item = menu.FindItem(Resource.Id.actionDelete);
                item.SetEnabled(false);
                item.SetVisible(false);
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.actionSave:
                    SavePOI();
                    return true;
                case Resource.Id.actionDelete:
                    DeletePOI();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
            if (context is Activity activity){
                _activity = activity;
                _networkState = new NetworkState(context);
            }
        }

        protected void UpdateUI()
        {
            _namedEditText.Text = _pointOfInterest.Name;
            _addressEditText.Text = _pointOfInterest.Address;
            _descrEditText.Text = _pointOfInterest.Address;
            _latEditText.Text = _pointOfInterest.Latitude.ToString();
            _longEditText.Text = _pointOfInterest.Longitude.ToString();
        }

        private async void SavePOI()
        {
            if (!_networkState.IsConnected())
            {
                Toast.MakeText(_activity, "Not connected to the internet", ToastLength.Short).Show();
                return;
            }

            ValidatePointOfInterest();
            if (_hasErrors)
                return;

            var poiToSave = new PointOfInterest()
            {
                Name = _namedEditText.Text,
                Address = _addressEditText.Text,
                Description = _descrEditText.Text,
                Latitude = double.Parse(_latEditText.Text),
                Longitude = double.Parse(_longEditText.Text)
            };
            if (_pointOfInterest != null)
                poiToSave.Id = _pointOfInterest.Id;

            var response = await _poiService.CreateOrUpdatePOIAsync(poiToSave);
            if (response == null)
            {
                Toast.MakeText(_activity, "Something went wrong", ToastLength.Short).Show();
                return;
            }

            Toast.MakeText(_activity, $"Saved {poiToSave.Name} to database", ToastLength.Short).Show();
            _activity.Finish();

        }

        private void DeletePOI()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(_activity);
            builder.SetTitle("ConfirmDelete");
            builder.SetCancelable(false);
            builder.SetPositiveButton("Ok", ConfirmDelete);
            builder.SetNegativeButton("Cancel", delegate { });
            builder.SetMessage($"are you sure you want to delete {_pointOfInterest.Name}?");
            builder.Show();
        }

        private async void DeletePOIAsync()
        {
            if (!_networkState.IsConnected())
            {
                Toast.MakeText(_activity, "Not connected to the internet", ToastLength.Short).Show();
                return;
            }
            var response = await _poiService.DeletePOIAsync((int)_pointOfInterest.Id);

            if (response == null)
            {
                Toast.MakeText(_activity, "Something went wrong", ToastLength.Short).Show();
                return;
            }

            Toast.MakeText(_activity, $"Deleted {_pointOfInterest.Name} from database", ToastLength.Short).Show();
        }

        private void ConfirmDelete(object sender, DialogClickEventArgs e)
        {
            DeletePOIAsync();
        }

        void ValidatePointOfInterest()
        {
            var validName = IsNameValid();
            var validLong = IsLongitudeValid();
            var validLat = IsLatitudeValid();

            _hasErrors = !(validName && validLat && validLong);
        }
        private bool IsNameValid()
        {
            if (string.IsNullOrEmpty(_namedEditText.Text))
            {
                _namedEditText.Error = "Name cannot be empty";
                return false;
            }
            return true;
        }
        private bool IsLongitudeValid()
        {
            if (string.IsNullOrEmpty(_longEditText.Text))
            {
                _longEditText.Error = "Longitude Cannot Be Empty";
                return false;
            }

            double result;
            if (!double.TryParse(_longEditText.Text, out result))
            {
                _longEditText.Error = "Longitude Must be a valid number";
                return false;
            }

            if (result < -180 || result > 180)
            {
                _longEditText.Error = "Longitude Must be between 180 and -180";
                return false;
            }

            return true;
        }
        private bool IsLatitudeValid()
        {
            if (string.IsNullOrEmpty(_latEditText.Text))
            {
                _latEditText.Error = "Latitude Cannot Be Empty";
                return false;
            }

            double result;
            if (!double.TryParse(_latEditText.Text, out result))
            {
                _latEditText.Error = "Latitude Must be a valid number";
                return false;
            }

            if (result < -90 || result > 90)
            {
                _latEditText.Error = "Latitude Must be between 90 and -90";
                return false;
            }

            return true;
        }
    }
}