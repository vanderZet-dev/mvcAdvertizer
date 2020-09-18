using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Services.Interfaces;
using MvcAdvertizer.Utils;
using MvcAdvertizer.ViewModels;

namespace MvcAdvertizer.Controllers
{
    public class AdvertController : Controller
    {        
        private readonly IRecaptchaService recaptchaService;
        private readonly IUsers userRepository;
        private readonly IAdverts advertRepository;
        private readonly IConfiguration configuration;

        public AdvertController(IRecaptchaService recaptchaService,
                                IUsers userRepository,
                                IAdverts advertRepository,
                                IConfiguration configuration) {            
            this.recaptchaService = recaptchaService;
            this.userRepository = userRepository;
            this.advertRepository = advertRepository;
            this.configuration = configuration;
        }

        
        public IActionResult Details(Guid id) {

            var viewModel = new AdvertViewModel();            

            var advertId = id;            
            viewModel = SetupForDetail(advertId, viewModel);

            return View(viewModel);
        }

        public IActionResult Edit(Guid id) {            

            var advertId = id;
            var viewModel = SetupForEditBeforePost(advertId);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(AdvertViewModel viewModel) {                        

            if (ModelState.IsValid)
            {
                advertRepository.Update(viewModel.Advert);                
                TempData["toaster"] = ToastGeneratorUtils.GetSuccessRecordUpdateSerializedToasterData();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Edit", new { id = viewModel.Advert.Id });
            }
        }

        public IActionResult Create() {

            var viewModel = SetupCreateBeforePost();           

            return View(viewModel);
        }        

        [HttpPost]
        public async Task<IActionResult> Create(AdvertViewModel viewModel) {

            string recaptchaResponse = Request.Form["g-recaptcha-response"];
            string connectionRemoteIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            var validRecaptcha = await recaptchaService.CheckRecaptcha(recaptchaResponse, connectionRemoteIpAddress);

            var showRecaptchaErrorMessage = !validRecaptcha;
            if (!validRecaptcha)
            {
                ModelState.AddModelError("Recaptcha", "Check request return false value.");
            }

            var userId = (Guid)viewModel?.Advert?.UserId;
            var limitExceeded = CheckUsersAdvertLimitCountForExceeded(userId);
            var showMaxUserAdvertsCountLimitErrorMessage = limitExceeded;
            if (limitExceeded)
            {
                ModelState.AddModelError("MaxUserAdvertsCountLimit", "Max user adverts count limit is exceeded.");
            }   

            viewModel = SetupCreateAfterPost(viewModel, showRecaptchaErrorMessage, showMaxUserAdvertsCountLimitErrorMessage);

            if (ModelState.IsValid)
            {
                advertRepository.Add(viewModel.Advert);
                TempData["toaster"] = ToastGeneratorUtils.GetSuccessRecordCreateSerializedToasterData();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(viewModel);
            }            
        }       


        public IActionResult SoftDelete(Guid id) {                        

            var existedAdvert = advertRepository.FindById(id);

            if (existedAdvert != null && !existedAdvert.Deleted)
            {
                existedAdvert.Deleted = true;
                advertRepository.Update(existedAdvert);
                TempData["toaster"] = ToastGeneratorUtils.GetSuccessRecordDeleteSerializedToasterData();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ShowImage(Guid id) {

            var viewModel = new AdvertViewModel();            

            var advert = advertRepository.FindById(id);            

            viewModel.Advert = advert;

            return View(viewModel);
        }
              

        private AdvertViewModel SetupCreateBeforePost() {

            var viewModel = new AdvertViewModel();

            var allUserList = userRepository.FindAll().ToList();
            viewModel.SetupCreateBeforePost(allUserList);

            return viewModel;
       }

        private AdvertViewModel SetupCreateAfterPost(AdvertViewModel viewModel, bool showRecaptchaErrorMessage, bool showMaxUserAdvertsCountLimitErrorMessage) {            

            var allUserList = userRepository.FindAll().ToList();
            viewModel.SetupCreateAfterPost(allUserList, showRecaptchaErrorMessage, showMaxUserAdvertsCountLimitErrorMessage);

            return viewModel;
        }

        private AdvertViewModel SetupForDetail(Guid advertId, AdvertViewModel viewModel) {

            var advert = advertRepository.FindById(advertId);
            var allUserList = userRepository.FindAll().ToList();

            viewModel.SetupForDetail(advert, allUserList);

            return viewModel;
        }        

        private AdvertViewModel SetupForEditBeforePost(Guid advertId) {

            var viewModel = new AdvertViewModel();

            var advert = advertRepository.FindById(advertId);
            var allUserList = userRepository.FindAll().ToList();

            viewModel.SetupForEditBeforePost(advert, allUserList);

            return viewModel;
        }


        private bool CheckUsersAdvertLimitCountForExceeded(Guid userId) {

            var limitExceeded = false;

            var maxUserAdvertsCount = Convert.ToInt64(configuration.GetSection("MaxUserAdvertsCount").Value);
            var exceptUsersForCheckingSection = configuration.GetSection("ExceptUsersForChecking");            
            var exceptUsersForChecking = exceptUsersForCheckingSection.Get<string[]>();

            if (!exceptUsersForChecking.Contains(userId.ToString()))
            {
                var actualUserAdvertsCount = advertRepository.CountByUserId(userId);
                limitExceeded = actualUserAdvertsCount >= maxUserAdvertsCount;                
            }

            return limitExceeded;
        }
    }
}
