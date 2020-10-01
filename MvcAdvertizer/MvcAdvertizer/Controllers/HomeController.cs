using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MvcAdvertizer.Config.Tools;
using MvcAdvertizer.Data.AdditionalObjects;
using MvcAdvertizer.Data.DTO;
using MvcAdvertizer.Services.Interfaces;
using MvcAdvertizer.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MvcAdvertizer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper mapper;

        private readonly IUserService userService;
        private readonly IAdvertService advertService;

        public HomeController(IUserService userService,
                              IMapper mapper,
                              IAdvertService advertService) {
            this.userService = userService;
            this.mapper = mapper;
            this.advertService = advertService;
        }

        public async Task<IActionResult> Index(AdvertSearchObject searchObject,
                                                SortingList sortingObject) {

            Toaster toaster = null;
            if (TempData["toaster"] != null)
            {
                toaster = JsonConvert.DeserializeObject<Toaster>((string)TempData["toaster"]);
            }

            var result = new AdvertListViewModel(searchObject, sortingObject, toaster);

            var allUserList = userService.FindAll().ToList();
            var allUserDtoList = mapper.Map<List<UserDto>>(allUserList);

            result.GenerateUserSearchList(allUserDtoList);

            var adverts = await advertService.GetFiltredAdverts(searchObject, result.SortingObject);
            result.Adverts = new PaginatedList<AdvertDto>(mapper.Map<List<AdvertDto>>(adverts.ToList()), adverts.ItemsCount, adverts.PageIndex, (int)searchObject.pageSize);

            return View(result);
        }

        public async Task<IActionResult> AdvertsDeleteAll() {

            await advertService.DeleteAll();
            return RedirectToAction("Index");
        }
    }
}
