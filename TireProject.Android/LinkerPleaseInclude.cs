using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TireProject.Droid
{
    public class LinkerPleaseInclude
    {
        public void IncludePdfSharpFonts()
        {
            // Force the linker to keep these types
            var marker = typeof(PdfSharp.Fonts.IFontResolverMarker);
        }
    }
}