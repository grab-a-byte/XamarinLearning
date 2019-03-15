using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Learning.Android.Services;
using Newtonsoft.Json;
using XamNative.Core.Entities;
using XamNative.Core.Interfaces;
using XamNative.Core.Services;
using AlertDialog = Android.App.AlertDialog;

namespace Learning.Android
{
    [Activity(Label = "POIDetailActivity")]
    public class POIDetailActivity : AppCompatActivity
    {
        private EditText _namedEditText;
        private EditText _descrEditText;
        private EditText _addressEditText;
        private EditText _latEditText;
        private EditText _longEditText;

        private PointOfInterest _pointOfInterest;

        private bool hasErrors = false;

        private IPOIService _poiService = new POIService();
        private INetworkState _networkState;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            _networkState =  new NetworkState(this);
            SetContentView(Resource.Layout.POIDetail);
            _namedEditText = FindViewById<EditText>(Resource.Id.nameEditText);
            _descrEditText = FindViewById<EditText>(Resource.Id.descEditText);
            _addressEditText = FindViewById<EditText>(Resource.Id.addrEditText);
            _latEditText = FindViewById<EditText>(Resource.Id.editLatitude);
            _longEditText = FindViewById<EditText>(Resource.Id.editLongitude);

            SetupFromIntent();

            base.OnCreate(savedInstanceState);
        }

        private void SetupFromIntent()
        {
            if (Intent.HasExtra("poi"))
            {
                string jsonPOI = Intent.GetStringExtra("poi");
                var poi = JsonConvert.DeserializeObject<PointOfInterest>(jsonPOI);
                _pointOfInterest = poi;
                UpdateUI();
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.POIDetailMenu,menu);
            return base.OnCreateOptionsMenu(menu);
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

        private async void SavePOI()
        {
            if (!_networkState.IsConnected())
            {
                Toast.MakeText(this, "Not connected to the internet", ToastLength.Short).Show();
                return;
            }

            ValidatePointOfInterest();
            if (hasErrors)
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
               Toast.MakeText(this, "Something went wrong", ToastLength.Short).Show();
               return;
            }

           Toast.MakeText(this, $"Saved {poiToSave.Name} to database", ToastLength.Short).Show();
           Finish();

        }

        private void DeletePOI()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
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
                Toast.MakeText(this, "Not connected to the internet", ToastLength.Short).Show();
                return;
            }
            var response = await _poiService.DeletePOIAsync((int)_pointOfInterest.Id);

            if (response == null)
            {
                Toast.MakeText(this, "Something went wrong", ToastLength.Short).Show();
                return;
            }

            Toast.MakeText(this, $"Deleted {_pointOfInterest.Name} from database", ToastLength.Short).Show();
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

            hasErrors = !(validName && validLat && validLong);
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