using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
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
    public class PoiListActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            try
            {
                SetContentView(Resource.Layout.POIList);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}