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
                for(int i = 0; i < 10; i++)
                {
                    var newUser = new User() { Name = "Пользователь " + i };
                    context.Users.Add(newUser);
                    context.UsersAdvertsCounters.Add(new UserAdvertsCounter() { UserId = newUser.Id, Count = 0 });
                }
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
                        User = allUsers[rng.Next(allUsers.Count)],
                        Rate = rng.Next(11),
                        PublishingDate = datetime,
                        Deleted = false
                    });
                }
                context.SaveChanges();
            }
        }        
    }
}
