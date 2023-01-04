using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using TireProject.Droid;
using Android.Content;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer_Droid))]
namespace TireProject.Droid
{
    public class CustomEntryRenderer_Droid : EntryRenderer
    {
        public CustomEntryRenderer_Droid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            //Android.Graphics.Drawables.GradientDrawable gd = new Android.Graphics.Drawables.GradientDrawable();

            ////this line sets the bordercolor
            //gd.SetColor(Android.Graphics.Color.ParseColor("#e74c3c"));

            //Control?.SetBackgroundDrawable(gd);

            //Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);


            if (e.OldElement == null)
            {
                var nativeEditText = (global::Android.Widget.EditText)Control;
                var shape = new Android.Graphics.Drawables.ShapeDrawable(new Android.Graphics.Drawables.Shapes.RectShape());
                shape.Paint.Color = Xamarin.Forms.Color.FromHex("#e74c3c").ToAndroid();
                shape.Paint.SetStyle(Android.Graphics.Paint.Style.Stroke);
                nativeEditText.Background = shape;
            }

        }
    }
}