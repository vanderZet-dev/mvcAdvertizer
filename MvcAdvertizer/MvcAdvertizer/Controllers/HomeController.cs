using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcAdvertizer.Config;
using MvcAdvertizer.Config.Database;
using MvcAdvertizer.Core.Domains;
using MvcAdvertizer.Core.Domains.Enums;
using MvcAdvertizer.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index(Guid? user, string number, int page = 1, int pageSize = 5, AdvertSortState sortOrder = AdvertSortState.NumberDesc)
        {
            IQueryable<Advert> source = db.Adverts.Include(c => c.User);
            
            if (user != null)
            {
                source = source.Where(p => p.UserId == user);
            }
            if (!string.IsNullOrEmpty(number))
            {
                source = source.Where(p => p.Number.ToString().Contains(number));
            }

            List<User> users = db.Users.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            users.Insert(0, new User { Name = "Все"});

            source = sortOrder switch
            {
                AdvertSortState.RateAsc => source.OrderBy(s => s.Rate),
                AdvertSortState.RateDesc => source.OrderByDescending(s => s.Rate),                

                AdvertSortState.UserNameAsc => source.OrderBy(s => s.User.Name),
                AdvertSortState.UserNameDesc => source.OrderByDescending(s => s.User.Name),                

                AdvertSortState.CreatedOnAsc => source.OrderBy(s => s.CreatedOn),
                AdvertSortState.CreatedOnDesc => source.OrderByDescending(s => s.CreatedOn),

                AdvertSortState.NumberAsc => source.OrderBy(s => s.Number),                
                _ => source.OrderByDescending(s => s.Number)              
            };

            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                SortViewModel = new SortViewModel(sortOrder),
                Users = new SelectList(users, "id", "Name"),
                Number = number,
                Adverts = items
            };
            return View(viewModel);
        }
    }
}
