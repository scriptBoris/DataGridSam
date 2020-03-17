using Sample.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Models
{
    public class User : BaseNotify
    {
        public DateTime BirthDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoUrl { get; set; }
        public Ranks Rank { get; set; }
    }

    public enum Ranks
    {
        OfficePlankton,
        Manager,
        Admin,
    }
}
