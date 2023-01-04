using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TireProject;
using TireProject.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(SystemHelper))]

namespace TireProject.Droid
{
    public class SystemHelper : ISystemHelper
    {
        public string GetDefaultSystemFont()
        {
            return "sans-serif";
            //return "sans-serif";
            //Roboto
        }

        public string GetTemporaryDirectory()
        {
            return System.IO.Path.GetTempPath();
        }
    }
}