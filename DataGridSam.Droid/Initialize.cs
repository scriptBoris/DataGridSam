using System;

namespace DataGridSam.Droid
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public static class Initialize
    {
        public static void Init()
        {
            DataGrid.Init();
            TouchDroid.Init();
        }
    }
}
