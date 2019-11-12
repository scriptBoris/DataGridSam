using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Models
{
    public class Ware
    {
        public bool IsCompleted { get; set; }
        public int Pos { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public float Weight { get; set; }
    }
}
