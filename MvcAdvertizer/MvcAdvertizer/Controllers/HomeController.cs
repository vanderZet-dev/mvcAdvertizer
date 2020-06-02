using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcAdvertizer.Config.Database;
using MvcAdvertizer.Config.Tools;
using MvcAdvertizer.Core.AdditionalObjects;
using MvcAdvertizer.Core.Domains;
using MvcAdvertizer.Core.ViewModels;
using System.ComponentModel.DataAnnotations;
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
        public async Task<IActionResult> Index(AdvertSearchObject advertListViewModel,
                                                RepresentObjectConfigurator representObjectConfigurator,
                                                SortingList<Advert> sortingListAdvert){

            var result = new AdvertListViewModel(representObjectConfigurator, advertListViewModel, sortingListAdvert);            

            //формирования списка пользователей для осуществления поиска по юзерам        
            IQueryable<User> users = db.Users;
            result.GenerateUserSearchList(users);

            //отправка собственно самого основного запроса
            IQueryable<Advert> adverts = db.Adverts.Include(c => c.User);
            await result.CreateAsync(adverts);

            return View(result);
        }
                
    }
}
