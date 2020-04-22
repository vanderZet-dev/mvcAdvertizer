using MvcAdvertizer.Config.Database;
using MvcAdvertizer.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Support
{
    public static class SampleData
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
