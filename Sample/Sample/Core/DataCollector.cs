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
                    Name = "Christian Bale",
                    PhotoUrl = "https://m.media-amazon.com/images/M/MV5BMTkxMzk4MjQ4MF5BMl5BanBnXkFtZTcwMzExODQxOA@@._V1_UX140_CR0,0,140,209_AL_.jpg",
                    FamousRole = "The Dark Knight",
                    Description = "Christian Charles Philip Bale was born in Pembrokeshire, Wales, UK on January 30, 1974, to English parents Jennifer \"Jenny\" (James) and David Bale.",
                },
                new Actor
                {
                    Name = "Daniel Day-Lewis",
                    PhotoUrl = "https://m.media-amazon.com/images/M/MV5BMjE2NDY2NDc1Ml5BMl5BanBnXkFtZTcwNjAyMjkwOQ@@._V1_UY209_CR9,0,140,209_AL_.jpg",
                    FamousRole = "There Will Be Blood",
                    Description = "Born in London, England, Daniel Michael Blake Day-Lewis is the second child of Cecil Day-Lewis (pseudonym Nicholas Blake), Poet Laureate of the U.K",
                },
                new Actor
                {
                    Name = "Matt Damon",
                    PhotoUrl = "https://m.media-amazon.com/images/M/MV5BMTM0NzYzNDgxMl5BMl5BanBnXkFtZTcwMDg2MTMyMw@@._V1_UY209_CR8,0,140,209_AL_.jpg",
                    FamousRole = "Good Will Hunting",
                    Description = "Matthew Paige Damon was born on October 8, 1970, in Boston, Massachusetts, to Kent Damon, a stockbroker, realtor and tax preparer",
                },
                new Actor
                {
                    Name = "Leonardo DiCaprio",
                    PhotoUrl = "https://m.media-amazon.com/images/M/MV5BMjI0MTg3MzI0M15BMl5BanBnXkFtZTcwMzQyODU2Mw@@._V1_UY209_CR7,0,140,209_AL_.jpg",
                    FamousRole = "Inception",
                    Description = "Few actors in the world have had a career quite as diverse as Leonardo DiCaprio's. DiCaprio has gone from relatively humble beginnings",
                },
                new Actor
                {
                    Name = "Heath Ledger",
                    PhotoUrl = "https://m.media-amazon.com/images/M/MV5BMTI2NTY0NzA4MF5BMl5BanBnXkFtZTYwMjE1MDE0._V1_UX140_CR0,0,140,209_AL_.jpg",
                    FamousRole = "Brokeback Mountain",
                    Description = "When hunky, twenty-year-old heart-throb Heath Ledger first came to the attention of the public in 1999, it was all too easy to tag him.",
                },
                //new Actor
                //{
                //    Name = "Christian Bale",
                //    PhotoUrl = "https://m.media-amazon.com/images/M/MV5BMTkxMzk4MjQ4MF5BMl5BanBnXkFtZTcwMzExODQxOA@@._V1_UX140_CR0,0,140,209_AL_.jpg",
                //    FamousRole = "The Dark Knight",
                //    Description = "Christian Charles Philip Bale was born in Pembrokeshire, Wales, UK on January 30, 1974, to English parents Jennifer \"Jenny\" (James) and David Bale.",
                //},
                //new Actor
                //{
                //    Name = "Christian Bale",
                //    PhotoUrl = "https://m.media-amazon.com/images/M/MV5BMTkxMzk4MjQ4MF5BMl5BanBnXkFtZTcwMzExODQxOA@@._V1_UX140_CR0,0,140,209_AL_.jpg",
                //    FamousRole = "The Dark Knight",
                //    Description = "Christian Charles Philip Bale was born in Pembrokeshire, Wales, UK on January 30, 1974, to English parents Jennifer \"Jenny\" (James) and David Bale.",
                //},
                //new Actor
                //{
                //    Name = "Christian Bale",
                //    PhotoUrl = "https://m.media-amazon.com/images/M/MV5BMTkxMzk4MjQ4MF5BMl5BanBnXkFtZTcwMzExODQxOA@@._V1_UX140_CR0,0,140,209_AL_.jpg",
                //    FamousRole = "The Dark Knight",
                //    Description = "Christian Charles Philip Bale was born in Pembrokeshire, Wales, UK on January 30, 1974, to English parents Jennifer \"Jenny\" (James) and David Bale.",
                //},
                //new Actor
                //{
                //    Name = "Christian Bale",
                //    PhotoUrl = "https://m.media-amazon.com/images/M/MV5BMTkxMzk4MjQ4MF5BMl5BanBnXkFtZTcwMzExODQxOA@@._V1_UX140_CR0,0,140,209_AL_.jpg",
                //    FamousRole = "The Dark Knight",
                //    Description = "Christian Charles Philip Bale was born in Pembrokeshire, Wales, UK on January 30, 1974, to English parents Jennifer \"Jenny\" (James) and David Bale.",
                //},
                //new Actor
                //{
                //    Name = "Christian Bale",
                //    PhotoUrl = "https://m.media-amazon.com/images/M/MV5BMTkxMzk4MjQ4MF5BMl5BanBnXkFtZTcwMzExODQxOA@@._V1_UX140_CR0,0,140,209_AL_.jpg",
                //    FamousRole = "The Dark Knight",
                //    Description = "Christian Charles Philip Bale was born in Pembrokeshire, Wales, UK on January 30, 1974, to English parents Jennifer \"Jenny\" (James) and David Bale.",
                //},
            };
            return res;
        }
    }
}
