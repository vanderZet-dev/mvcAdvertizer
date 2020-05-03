using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcAdvertizer.Config.Database;
using MvcAdvertizer.Config.Tools;
using MvcAdvertizer.Core.Domains;
using System;
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
        public async Task<IActionResult> Index(
                                                string sortOrder,
                                                string currentFilter,
                                                string searchString,            
                                                int? pageNumber,
                                                int pageSize = 3
                                                )
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NumberSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Number_desc" : "";
            ViewData["CreatedOnSortParm"] = sortOrder == "CreatedOn" ? "CreatedOn_desc" : "CreatedOn";
            ViewData["PageSize"] = pageSize;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            IQueryable<Advert> adverts = db.Adverts.Include(c => c.User);

            if (!String.IsNullOrEmpty(searchString))
            {
                adverts = adverts.Where(s => s.Number.ToString().Equals(searchString)
                                       || (User != null && EF.Functions.Like(s.User.Name.ToUpper(), $"%{searchString.ToUpper()}%"))
                                       || EF.Functions.Like(s.Content.ToUpper(), $"%{searchString.ToUpper()}%")
                                       );
            }

            switch (sortOrder)
            {
                case "Number_desc":
                    adverts = adverts.OrderByDescending(s => s.Number);
                    break;
                case "CreatedOn":
                    adverts = adverts.OrderBy(s => s.CreatedOn);
                    break;
                case "CreatedOn_desc":
                    adverts = adverts.OrderByDescending(s => s.CreatedOn);
                    break;
                default:
                    adverts = adverts.OrderBy(s => s.Number);
                    break;
            }

                        
            return View(await PaginatedList<Advert>.CreateAsync(adverts.AsNoTracking(), pageNumber ?? 1, pageSize));                     
        }
    }
}
