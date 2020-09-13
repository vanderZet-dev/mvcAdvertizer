using MvcAdvertizer.Config.Database;
using MvcAdvertizer.Data.Models;
using System;
using System.Linq;

namespace MvcAdvertizer.Data
{
    public static class InitialData
    {
        public static void Initialize(ApplicationContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Name = "Пользователь 1"
                    },
                    new User
                    {
                        Name = "Пользователь 2"
                    },
                    new User
                    {
                        Name = "Пользователь 3"
                    }
                );
                context.SaveChanges();               
            }

            if (!context.Adverts.Any())
            {                
                Random rng = new Random();
                var allUsers = context.Users.ToList();
                for (int i = 1; i <= 100; i++)
                {
                    var datetime = DateTime.Now;
                    datetime = datetime.AddTicks(-(datetime.Ticks % TimeSpan.TicksPerSecond));
                    context.Adverts.Add(new Advert
                    {
                        Number = rng.Next(10000),
                        Content = "Тестовое содержание объявления " + i,
                        User = allUsers[rng.Next(3)],
                        Rate = rng.Next(11),
                        PublishingDate = datetime,
                        Deleted = false
                    }); ;
                }
                context.SaveChanges();
            }
        }
    }
}
