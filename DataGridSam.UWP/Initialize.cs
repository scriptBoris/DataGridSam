using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGridSam.UWP
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public static class Initialize
    {
        public static void Init()
        {
            TouchUWP.Init();
        }
    }
}
