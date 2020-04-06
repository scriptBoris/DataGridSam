using Sample.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Sample.Core
{
    public static class DataCollector
    {
        public static ObservableCollection<User> GetUsers()
        {
            var res = new ObservableCollection<User>
            {
                new User
                {
                    BirthDate = new DateTime(1992, 7, 28),
                    FirstName = "Diana",
                    LastName = "Roseborough",
                    PhotoUrl = "https://randomuser.me/api/portraits/women/19.jpg",
                    Rank = Ranks.OfficePlankton,
                },
                new User
                {
                    BirthDate = new DateTime(1989, 9, 29),
                    FirstName = "Carmen",
                    LastName = "Speights",
                    PhotoUrl = "https://randomuser.me/api/portraits/men/85.jpg",
                    Rank = Ranks.OfficePlankton,
                },
                new User
                {
                    BirthDate = new DateTime(1991, 2, 20),
                    FirstName = "Boris",
                    LastName = "Krit",
                    PhotoUrl = "https://randomuser.me/api/portraits/men/72.jpg",
                    Rank = Ranks.OfficePlankton,
                },
                new User
                {
                    BirthDate = new DateTime(1979, 1, 12),
                    FirstName = "Anna",
                    LastName = "Abraham",
                    PhotoUrl = "https://randomuser.me/api/portraits/women/85.jpg",
                    Rank = Ranks.Manager,
                },
                new User
                {
                    BirthDate = new DateTime(1996, 12, 13),
                    FirstName = "Sam",
                    LastName = "Super",
                    PhotoUrl = "https://randomuser.me/api/portraits/men/55.jpg",
                    Rank = Ranks.Admin,
                },
                new User
                {
                    BirthDate = new DateTime(1996, 8, 8),
                    FirstName = "Tommy",
                    LastName = "Mcsherry",
                    PhotoUrl = "https://randomuser.me/api/portraits/men/46.jpg",
                    Rank = Ranks.Manager,
                },
                new User
                {
                    BirthDate = new DateTime(2001, 1, 27),
                    FirstName = "Candie",
                    LastName = "Hopping",
                    PhotoUrl = "https://randomuser.me/api/portraits/women/26.jpg",
                    Rank = Ranks.OfficePlankton,
                },
                new User
                {
                    BirthDate = new DateTime(1985, 10, 3),
                    FirstName = "Vincent",
                    LastName = "Ruvalcaba",
                    PhotoUrl = "https://randomuser.me/api/portraits/men/15.jpg",
                    Rank = Ranks.OfficePlankton,
                },
                new User
                {
                    BirthDate = new DateTime(1988, 1, 13),
                    FirstName = "Jeffry",
                    LastName = "Wehner",
                    PhotoUrl = "https://randomuser.me/api/portraits/men/64.jpg",
                    Rank = Ranks.OfficePlankton,
                },
            };

            return res;
        }

        public static ObservableCollection<Actor> GetActors()
        {
            var res = new ObservableCollection<Actor>
            {
                new Actor
                {
                    Name = "Christian",
                    Family = "Bale",
                    PhotoUrl = "https://m.media-amazon.com/images/M/MV5BMTkxMzk4MjQ4MF5BMl5BanBnXkFtZTcwMzExODQxOA@@._V1_UX140_CR0,0,140,209_AL_.jpg",
                    FamousRole = "The Dark Knight",
                    Description = "Christian Charles Philip Bale was born in Pembrokeshire, Wales, UK on January 30, 1974, to English parents Jennifer \"Jenny\" (James) and David Bale.",
                },
                new Actor
                {
                    Name = "Daniel",
                    Family = "Day-Lewis",
                    PhotoUrl = "https://m.media-amazon.com/images/M/MV5BMjE2NDY2NDc1Ml5BMl5BanBnXkFtZTcwNjAyMjkwOQ@@._V1_UY209_CR9,0,140,209_AL_.jpg",
                    FamousRole = "There Will Be Blood",
                    Description = "Born in London, England, Daniel Michael Blake Day-Lewis is the second child of Cecil Day-Lewis (pseudonym Nicholas Blake), Poet Laureate of the U.K",
                },
                new Actor
                {
                    Name = "Matt",
                    Family = "Damon",
                    PhotoUrl = "https://m.media-amazon.com/images/M/MV5BMTM0NzYzNDgxMl5BMl5BanBnXkFtZTcwMDg2MTMyMw@@._V1_UY209_CR8,0,140,209_AL_.jpg",
                    FamousRole = "Good Will Hunting",
                    Description = "Matthew Paige Damon was born on October 8, 1970, in Boston, Massachusetts, to Kent Damon, a stockbroker, realtor and tax preparer",
                },
                new Actor
                {
                    Name = "Leonardo",
                    Family = "DiCaprio",
                    PhotoUrl = "https://m.media-amazon.com/images/M/MV5BMjI0MTg3MzI0M15BMl5BanBnXkFtZTcwMzQyODU2Mw@@._V1_UY209_CR7,0,140,209_AL_.jpg",
                    FamousRole = "Inception",
                    //Description = "Few actors in the world have had a career quite as diverse as Leonardo DiCaprio's. DiCaprio has gone from relatively humble beginnings",
                    Description = "Just great actor",
                },
                new Actor
                {
                    Name = "Heath",
                    Family = "Ledger",
                    PhotoUrl = "https://m.media-amazon.com/images/M/MV5BMTI2NTY0NzA4MF5BMl5BanBnXkFtZTYwMjE1MDE0._V1_UX140_CR0,0,140,209_AL_.jpg",
                    FamousRole = "Brokeback Mountain",
                    Description = "When hunky, twenty-year-old heart-throb Heath Ledger first came to the attention of the public in 1999, it was all too easy to tag him.",
                },
            };
            return res;
        }

        public static ObservableCollection<Ware> GetWares()
        {
            var res = new ObservableCollection<Ware>()
            {
                new Ware
                {
                    Name = "Stainless steel bottle MBI-A",
                    Price = 47.1f,
                    Weight = 0.0f,
                    Need = 100,
                },
                new Ware
                {
                    Name = "Toaster oven kaj-B",
                    Price = 87.4f,
                    Weight = 0.0f,
                    Need = 150,
                },
                new Ware
                {
                    Name = "Thermal magic cooker NFI-A",
                    Price = 159.56f,
                    Weight = 100.00f,
                    Need = 100,
                },
                new Ware
                {
                    Name = "Close-up of jeans pockets",
                    Price = 199.0f,
                    Weight = 88.0f,
                    Need = 300.0f,
                },
                new Ware
                {
                    Name = "Game Handles for Playing Games",
                    Price = 1200.0f,
                    Need = 1000.0f,
                    Weight = 0.0f,
                },
                new Ware
                {
                    Name = "Children's sweets. Dark chocolate with lemon and mint flavor, twenty cubes per pack.",
                    Price = 18.3f,
                    Need = 100.0f,
                    Weight = 0.0f,
                },
                new Ware
                {
                    Name = "Cola",
                    Price = 2.0f,
                    Need = 710.0f,
                    Weight = 0.0f,
                },
            };

            return res;
        }

        public static ObservableCollection<Vehicle> GetVehicle()
        {
            var res = new ObservableCollection<Vehicle>()
            {
                new Vehicle
                {
                    Name = "Rover",
                    Company = "Ford",
                    Engine = new Engine
                    {
                        HorsePower = 270,
                        SerialNumber = "0A0813546",
                        Volume = 3.5f,
                    },
                },
                new Vehicle
                {
                    Name = "Mustang",
                    Company = "Ford",
                    Engine = new Engine
                    {
                        HorsePower = 315,
                        SerialNumber = "0A081133",
                        Volume = 3.8f,
                    },
                },
                new Vehicle
                {
                    Name = "Priora",
                    Company = "Lada",
                    Engine = new Engine
                    {
                        HorsePower = 87,
                        SerialNumber = "0R0005135",
                        Volume = 1.6f,
                    },
                },
                new Vehicle
                {
                    Name = "Granta",
                    Company = "Lada",
                    Engine = new Engine
                    {
                        HorsePower = 97,
                        SerialNumber = "0R0015981",
                        Volume = 1.8f,
                    },
                },
            };

            return res;
        }
    }
}
