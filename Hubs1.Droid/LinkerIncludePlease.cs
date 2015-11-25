using Android.Views;
using Android.Widget;

namespace Hubs1.Droid
{
    // things in this class are only required in order to prevent the linker overoptimising!
    class LinkerIncludePlease
    {
        private void IncludeVisibility(View widget)
        {
            widget.Visibility = widget.Visibility + 1;
        }

        private void IncludeClick(View widget)
        {
            widget.Click += (s, e) => { };
        }

        private void IncludeRelativeLayout(RelativeLayout relativeLayout)
        {
            relativeLayout.Click += (s, e) => { };
        }
    }
}