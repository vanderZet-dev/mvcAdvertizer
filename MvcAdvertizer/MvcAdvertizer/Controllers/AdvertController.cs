using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Services.Interfaces;
using MvcAdvertizer.ViewModels;

namespace MvcAdvertizer.Controllers
{
    public class AdvertController : Controller
    {        
        private readonly IRecaptchaService recaptchaService;
        private readonly IUsers userRepository;
        private readonly IAdverts advertRepository;

        public AdvertController(IRecaptchaService recaptchaService,
                                IUsers userRepository,
                                IAdverts advertRepository) {            
            this.recaptchaService = recaptchaService;
            this.userRepository = userRepository;
            this.advertRepository = advertRepository;
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
                var updated = advertRepository.Update(viewModel.Advert);
                return RedirectToAction("Details", new { id = updated.Id });
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
            var validRecaptcha = await recaptchaService.checkRecaptcha(recaptchaResponse, connectionRemoteIpAddress);

            var showRecaptchaErrorMessage = !validRecaptcha;
            viewModel = SetupCreateAfterPost(viewModel, showRecaptchaErrorMessage);

            if (!validRecaptcha)
            {
                ModelState.AddModelError("Recaptcha", "Check request return false value.");
            }            

            if (ModelState.IsValid)
            {
                var created = advertRepository.Add(viewModel.Advert);
                return RedirectToAction("Details", new { id = created.Id });
            }
            else
            {
                return View(viewModel);
            }            
        }       


        public IActionResult SoftDelete(Guid id) {                        

            var existedAdvert = advertRepository.findById(id);

            if (existedAdvert != null && !existedAdvert.Deleted)
            {
                existedAdvert.Deleted = true;
                advertRepository.Update(existedAdvert);                
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ShowImage(Guid id) {

            var viewModel = new AdvertViewModel();            

            var advert = advertRepository.findById(id);            

            viewModel.Advert = advert;

            return View(viewModel);
        }
              

        private AdvertViewModel SetupCreateBeforePost() {

            var viewModel = new AdvertViewModel();

            var allUserList = userRepository.findAll().ToList();
            viewModel.SetupCreateBeforePost(allUserList);

            return viewModel;
       }

        private AdvertViewModel SetupCreateAfterPost(AdvertViewModel viewModel, bool showRecaptchaErrorMessage) {            

            var allUserList = userRepository.findAll().ToList();
            viewModel.SetupCreateAfterPost(allUserList, showRecaptchaErrorMessage);

            return viewModel;
        }

        private AdvertViewModel SetupForDetail(Guid advertId, AdvertViewModel viewModel) {

            var advert = advertRepository.findById(advertId);
            var allUserList = userRepository.findAll().ToList();

            viewModel.SetupForDetail(advert, allUserList);

            return viewModel;
        }        

        private AdvertViewModel SetupForEditBeforePost(Guid advertId) {

            var viewModel = new AdvertViewModel();

            var advert = advertRepository.findById(advertId);
            var allUserList = userRepository.findAll().ToList();

            viewModel.SetupForEditBeforePost(advert, allUserList);

            return viewModel;
        }
    }
}
