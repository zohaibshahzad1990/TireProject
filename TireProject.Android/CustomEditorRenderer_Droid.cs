using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using TireProject.Droid;
using Android.Content;

[assembly: ExportRenderer(typeof(Editor), typeof(CustomEditorRenderer_Droid))]
namespace TireProject.Droid
{
    public class CustomEditorRenderer_Droid : EditorRenderer
    {
        public CustomEditorRenderer_Droid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            Android.Graphics.Drawables.GradientDrawable gd = new Android.Graphics.Drawables.GradientDrawable();

            //this line sets the bordercolor
            gd.SetColor(Android.Graphics.Color.ParseColor("#e74c3c"));

            Control?.SetBackgroundDrawable(gd);

            Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);

        }
    }
}