using System.Threading.Tasks;
using Android.Content;
using Android.Locations;
using TipCalcMvvmCrossForms.Services;
using Location = TipCalcMvvmCrossForms.ViewModels.Location;

namespace TipCalcMvvmCrossForms.Droid.Services
{
    public class LocationService : ILocationService
    {
        private LocationManager _locationManger;

        public LocationService(LocationManager locationManger)
        {
            _locationManger = locationManger;
        }

        private void FindLocation()
        {
            var criteria = new Criteria();
            criteria.Accuracy = Accuracy.High;
            var provider = _locationManger.GetBestProvider(criteria, true);
            var loc = _locationManger.GetLastKnownLocation(provider);
        }

        public Task<Location> GetLocation()
        {
            throw new System.NotImplementedException();
        }
    }
}