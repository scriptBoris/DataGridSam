using Sample.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Models
{
    public class Vehicle : BaseNotify
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public Engine Engine { get; set; }
        public Ware Ware { get; set; }
    }

    public class Engine : BaseNotify
    {
        public float Volume { get; set; }
        public int HorsePower { get; set; }
        public string SerialNumber { get; set; }
    }
}
