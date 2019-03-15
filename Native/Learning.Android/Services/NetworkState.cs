using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.Telephony;
using XamNative.Core.Interfaces;

namespace Learning.Android.Services
{
    public class NetworkState : INetworkState
    {
        private readonly Context _activityContext;

        public NetworkState(Context activityContext)
        {
            _activityContext = activityContext;
        }

        public bool IsConnected()
        {
            var connectivityManager =
                (ConnectivityManager) _activityContext.GetSystemService(Context.ConnectivityService);

            var activeConnection = connectivityManager.ActiveNetworkInfo;
            return activeConnection != null && activeConnection.IsConnected;
        }
    }
}
