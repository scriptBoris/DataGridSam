using Sample.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Models
{
    public class Actor : BaseNotify
    {
        public string PhotoUrl { get; set; }
        public string Name { get; set; }
        public string FamousRole { get; set; }
        public string Description { get; set; }
    }
}
