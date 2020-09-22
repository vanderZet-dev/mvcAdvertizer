using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MvcAdvertizer.Data.DTO;
using MvcAdvertizer.Data.Models;
using MvcAdvertizer.Services.Interfaces;
using MvcAdvertizer.Utils;
using MvcAdvertizer.ViewModels;

namespace MvcAdvertizer.Controllers
{
    public class AdvertController : Controller
    {        
        private readonly IRecaptchaService recaptchaService;           
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IAdvertService advertService;
        private readonly IUserService userService;

        public AdvertController(IRecaptchaService recaptchaService,                                                              
                                IConfiguration configuration,
                                IMapper mapper,
                                IAdvertService advertService, 
                                IUserService userService) {
            this.recaptchaService = recaptchaService;                      
            this.configuration = configuration;
            this.mapper = mapper;
            this.advertService = advertService;
            this.userService = userService;
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

            var advert = mapper.Map<Advert>(viewModel.AdvertDto);

            if (ModelState.IsValid)
            {
                advertService.Update(advert);                
                TempData["toaster"] = ToastGeneratorUtils.GetSuccessRecordUpdateSerializedToasterData();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Edit", new { id = advert.Id });
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

            var userId = (Guid)viewModel.AdvertDto?.UserId;
            var limitExceeded = CheckUsersAdvertLimitCountForExceeded(userId);
            var showMaxUserAdvertsCountLimitErrorMessage = limitExceeded;
            if (limitExceeded)
            {
                ModelState.AddModelError("MaxUserAdvertsCountLimit", "Max user adverts count limit is exceeded.");
            }   

            viewModel = SetupCreateAfterPost(viewModel, showRecaptchaErrorMessage, showMaxUserAdvertsCountLimitErrorMessage);

            var advert = mapper.Map<Advert>(viewModel.AdvertDto);

            if (ModelState.IsValid)
            {
                advertService.Create(advert);                
                TempData["toaster"] = ToastGeneratorUtils.GetSuccessRecordCreateSerializedToasterData();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(viewModel);
            }            
        }       


        public IActionResult SoftDelete(Guid id) {

            var existedAdvert = advertService.FindById(id);

            if (existedAdvert != null && !existedAdvert.Deleted)
            {
                existedAdvert.Deleted = true;
                advertService.Update(existedAdvert);                
                TempData["toaster"] = ToastGeneratorUtils.GetSuccessRecordDeleteSerializedToasterData();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ShowImage(Guid id) {

            var viewModel = new AdvertViewModel();            

            var advert = advertService.FindById(id);

            var advertDto = mapper.Map<AdvertDto>(advert);

            viewModel.AdvertDto = advertDto;

            return View(viewModel);
        }
              

        private AdvertViewModel SetupCreateBeforePost() {

            var viewModel = new AdvertViewModel();

            var allUserList = userService.FindAll().ToList();
            var allUserDtoList = mapper.Map<List<UserDto>>(allUserList);
            viewModel.SetupCreateBeforePost(allUserDtoList);

            return viewModel;
       }

        private AdvertViewModel SetupCreateAfterPost(AdvertViewModel viewModel, bool showRecaptchaErrorMessage, bool showMaxUserAdvertsCountLimitErrorMessage) {

            var allUserList = userService.FindAll().ToList();
            var allUserDtoList = mapper.Map<List<UserDto>>(allUserList);
            viewModel.SetupCreateAfterPost(allUserDtoList, showRecaptchaErrorMessage, showMaxUserAdvertsCountLimitErrorMessage);

            return viewModel;
        }

        private AdvertViewModel SetupForDetail(Guid advertId, AdvertViewModel viewModel) {

            var advert = advertService.FindById(advertId);
            var allUserList = userService.FindAll().ToList();
            var allUserDtoList = mapper.Map<List<UserDto>>(allUserList);
            var advertDto = mapper.Map<AdvertDto>(advert);

            viewModel.SetupForDetail(advertDto, allUserDtoList);

            return viewModel;
        }        

        private AdvertViewModel SetupForEditBeforePost(Guid advertId) {

            var viewModel = new AdvertViewModel();

            var advert = advertService.FindById(advertId);
            var allUserList = userService.FindAll().ToList();
            var allUserDtoList = mapper.Map<List<UserDto>>(allUserList);
            var advertDto = mapper.Map<AdvertDto>(advert);

            viewModel.SetupForEditBeforePost(advertDto, allUserDtoList);

            return viewModel;
        }


        private bool CheckUsersAdvertLimitCountForExceeded(Guid userId) {

            var limitExceeded = false;

            var maxUserAdvertsCount = Convert.ToInt64(configuration.GetSection("MaxUserAdvertsCount").Value);
            var exceptUsersForCheckingSection = configuration.GetSection("ExceptUsersForChecking");            
            var exceptUsersForChecking = exceptUsersForCheckingSection.Get<string[]>();

            if (!exceptUsersForChecking.Contains(userId.ToString()))
            {
                var actualUserAdvertsCount = advertService.CountByUserId(userId);
                limitExceeded = actualUserAdvertsCount >= maxUserAdvertsCount;                
            }

            return limitExceeded;
        }
    }
}
