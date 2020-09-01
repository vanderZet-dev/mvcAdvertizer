using MvcAdvertizer.Config.Database;
using MvcAdvertizer.Data.Models;
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
                var user = context.Users.FirstOrDefault();
                for(int i = 1; i <= 25; i++)
                {
                    context.Adverts.Add(new Advert
                    {
                        Content = "Тестовое содержание объявления " + i,
                        User = user
                    });
                }
                context.SaveChanges();
            }
        }
    }
}
