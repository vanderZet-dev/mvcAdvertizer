using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcAdvertizer.Config.Database;
using MvcAdvertizer.Config.Tools;
using MvcAdvertizer.Core.Domains;
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
        public async Task<IActionResult> Index(string sortOrderName,
                                                string sortDirection,
                                                bool notChangeSort,
                                                Guid? userId,                                                
                                                string searchStringQuery,
                                                int? pageNumber,
                                                int? pageSize)
        {
            var result = new AdvertListViewModel("Number", "desc", 5);            
            result.SortingListTool.ActivateSortingElement(sortOrderName, sortDirection, notChangeSort);

            //биндим параметры сортировки и элементы представления
            ViewData["Number"] = result.SortingListTool?.GetSortingElementByName("Number");
            ViewData["NumberSortDirection"] = result.SortingListTool?.GetSortingElementByName("Number")?.SortDirection;
            ViewData["CreatedOn"] = result.SortingListTool.GetSortingElementByName("CreatedOn");
            ViewData["CreatedOnSortDirection"] = result.SortingListTool.GetSortingElementByName("CreatedOn")?.SortDirection;
            ViewData["Content"] = result.SortingListTool.GetSortingElementByName("Content");
            ViewData["ContentSortDirection"] = result.SortingListTool.GetSortingElementByName("Content")?.SortDirection;
            ViewData["Rate"] = result.SortingListTool.GetSortingElementByName("Rate");
            ViewData["RateSortDirection"] = result.SortingListTool.GetSortingElementByName("Rate")?.SortDirection;
            ViewData["User"] = result.SortingListTool.GetSortingElementByName("User");
            ViewData["UserSortDirection"] = result.SortingListTool.GetSortingElementByName("User")?.SortDirection;

            //биндим буферные настройки сортировки для случаев, когда они не должны изменяться - запрос приходит не из контроллера, отвечающего за сортировку
            ViewData["BufferSortParam"] = result.SortingListTool?.GetActiveSortingElement();
            ViewData["BufferSortDirection"] = result.SortingListTool?.GetActiveSortingElement()?.SortDirection;

            //задаем настройки для страниц
            result.SelectedPageNumber = pageNumber;
            result.SelectedPageSize = pageSize;
            ViewData["PageSize"] = result.SelectedPageSize;


            //формирования списка пользователей для осуществления поиска по юзерам
            result.SearchedUserId = userId;
            IQueryable<User> users = db.Users;
            ViewData["UserList"] = result.GetUserSearchList(users);
            ViewData["BufferUserId"] = userId;
            
            //обработка поисковой строки
            result.StringQuerySearch = searchStringQuery;
            ViewData["SearchStringQuery"] = searchStringQuery;

            //отправка собственно самого основного запроса
            IQueryable<Advert> adverts = db.Adverts.Include(c => c.User);
            await result.CreateAsync(adverts);

            return View(result);                     
        }
                
    }
}
