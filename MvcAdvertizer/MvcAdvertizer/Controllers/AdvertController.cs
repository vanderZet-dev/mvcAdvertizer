using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MvcAdvertizer.Config;
using MvcAdvertizer.Data;
using MvcAdvertizer.Data.AdditionalObjects;
using MvcAdvertizer.Data.DTO;
using MvcAdvertizer.Data.Models;
using MvcAdvertizer.Services.Interfaces;
using MvcAdvertizer.Utils;
using MvcAdvertizer.ViewModels;

namespace MvcAdvertizer.Controllers
{
    public class AdvertController : Controller
    {
        private static object locker = new object();

        private readonly UsersAdvertsSettings usersAdvertsSettings;
        private readonly IRecaptchaService recaptchaService;        
        private readonly IMapper mapper;
        private readonly IAdvertService advertService;
        private readonly IUserService userService;
        private readonly IFileStorageService fileStorageService;

        public AdvertController(IOptions<AppSettings> settings,
                                IRecaptchaService recaptchaService,
                                IMapper mapper,
                                IAdvertService advertService,
                                IUserService userService, 
                                IFileStorageService fileStorageService) {
            usersAdvertsSettings = settings?.Value?.UsersAdvertsSettings;
            this.recaptchaService = recaptchaService;            
            this.mapper = mapper;
            this.advertService = advertService;
            this.userService = userService;
            this.fileStorageService = fileStorageService;

            HttpExecutor.TestUserId = userService.FindAll().OrderBy(x=>x.Id).FirstOrDefault()?.Id;
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

            var recaptchaResponse = new RecaptchaResponse();
            recaptchaResponse.Response = Request.Form["g-recaptcha-response"];
            recaptchaResponse.ConnectionRemoteIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            recaptchaResponse.Valid = await recaptchaService
                .CheckRecaptcha(recaptchaResponse.Response, recaptchaResponse.ConnectionRemoteIpAddress);

            viewModel.ShowRecaptchaErrorMessage = !recaptchaResponse.ValidateModelState(ModelState);            

            lock (locker)
            {
                var userId = (Guid)viewModel.AdvertDto?.UserId;
                var limitExceeded = CheckUsersAdvertLimitCountForExceeded(userId);
                var showMaxUserAdvertsCountLimitErrorMessage = limitExceeded;
                if (limitExceeded)
                {
                    ModelState.AddModelError("MaxUserAdvertsCountLimit", "Max user adverts count limit is exceeded.");
                }

                viewModel = SetupCreateAfterPost(viewModel, showMaxUserAdvertsCountLimitErrorMessage);

                var advert = mapper.Map<Advert>(viewModel.AdvertDto);

                if (ModelState.IsValid)
                {
                    if (viewModel.ImageFromFile != null)
                    {
                        var savedImageName = fileStorageService.Save(viewModel.ImageFromFile);
                        advert.ImageHash = savedImageName;
                    }

                    advertService.Create(advert);
                    TempData["toaster"] = ToastGeneratorUtils.GetSuccessRecordCreateSerializedToasterData();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(viewModel);
                }
            }              
        }
        

        [HttpPost]
        public IActionResult CreateTestWithLock(AdvertViewModel viewModel) {   

            lock (locker)
            {
                var userId = (Guid)viewModel.AdvertDto?.UserId;
                var limitExceeded = CheckUsersAdvertLimitCountForExceeded(userId);
                var showMaxUserAdvertsCountLimitErrorMessage = limitExceeded;
                if (limitExceeded)
                {
                    ModelState.AddModelError("MaxUserAdvertsCountLimit", "Max user adverts count limit is exceeded.");
                }

                viewModel = SetupCreateAfterPost(viewModel, showMaxUserAdvertsCountLimitErrorMessage);

                var advert = mapper.Map<Advert>(viewModel.AdvertDto);

                if (ModelState.IsValid)
                {
                    if (viewModel.ImageFromFile != null)
                    {
                        var savedImageName = fileStorageService.Save(viewModel.ImageFromFile);
                        advert.ImageHash = savedImageName;
                    }

                    advertService.Create(advert);
                    TempData["toaster"] = ToastGeneratorUtils.GetSuccessRecordCreateSerializedToasterData();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [HttpPost]
        public IActionResult CreateTestWithoutLock(AdvertViewModel viewModel) {
            
            var userId = (Guid)viewModel.AdvertDto?.UserId;
            var limitExceeded = CheckUsersAdvertLimitCountForExceeded(userId);
            var showMaxUserAdvertsCountLimitErrorMessage = limitExceeded;
            if (limitExceeded)
            {
                ModelState.AddModelError("MaxUserAdvertsCountLimit", "Max user adverts count limit is exceeded.");
            }

            viewModel = SetupCreateAfterPost(viewModel, showMaxUserAdvertsCountLimitErrorMessage);

            var advert = mapper.Map<Advert>(viewModel.AdvertDto);

            if (ModelState.IsValid)
            {
                if (viewModel.ImageFromFile != null)
                {
                    var savedImageName = fileStorageService.Save(viewModel.ImageFromFile);
                    advert.ImageHash = savedImageName;
                }

                advertService.Create(advert);
                TempData["toaster"] = ToastGeneratorUtils.GetSuccessRecordCreateSerializedToasterData();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
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

        public IActionResult ShowImage(Guid id, int width) {

            var viewModel = new AdvertViewModel();            

            var advert = advertService.FindById(id);

            var advertDto = mapper.Map<AdvertDto>(advert);

            viewModel.AdvertDto = advertDto;

            AttachImageToViewModel(advert, viewModel);

            var image = viewModel.AdvertDto.Image;

            if (image != null)
            {
                viewModel.AdvertDto.Image = ImageResizerUtill.ScaledByWidth(image, width);
            }            

            return View(viewModel);
        }
         
        public IActionResult GenerateUsersAdvertsWithLock() {

            for (int i = 0; i < 10; i++)
            {
                Thread myThread = new Thread(HttpExecutor.ExecuteCreateAdvertsWithLock);                
                myThread.Start();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult GenerateUsersAdvertsWithoutLock() {

            for (int i = 0; i < 10; i++)
            {
                Thread myThread = new Thread(HttpExecutor.ExecuteCreateAdvertsWithoutLock);
                myThread.Start();
            }

            return RedirectToAction("Index", "Home");
        }

        private AdvertViewModel SetupCreateBeforePost() {

            var viewModel = new AdvertViewModel();

            var allUserList = userService.FindAll().ToList();
            var allUserDtoList = mapper.Map<List<UserDto>>(allUserList);
            viewModel.SetupCreateBeforePost(allUserDtoList);

            return viewModel;
       }

        private AdvertViewModel SetupCreateAfterPost(AdvertViewModel viewModel, bool showMaxUserAdvertsCountLimitErrorMessage) {

            var allUserList = userService.FindAll().ToList();
            var allUserDtoList = mapper.Map<List<UserDto>>(allUserList);
            viewModel.SetupCreateAfterPost(allUserDtoList, showMaxUserAdvertsCountLimitErrorMessage);

            return viewModel;
        }

        private AdvertViewModel SetupForDetail(Guid advertId, AdvertViewModel viewModel) {

            var advert = advertService.FindById(advertId);
            var allUserList = userService.FindAll().ToList();
            var allUserDtoList = mapper.Map<List<UserDto>>(allUserList);
            var advertDto = mapper.Map<AdvertDto>(advert);            

            viewModel.SetupForDetail(advertDto, allUserDtoList);

            AttachImageToViewModel(advert, viewModel);            

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

            var maxUserAdvertsCount = usersAdvertsSettings.MaxUserAdvertsCount;
            var exceptUsersForChecking = usersAdvertsSettings.ExceptUsersForChecking;            

            if (!exceptUsersForChecking.Contains(userId.ToString()))
            {
                var actualUserAdvertsCount = advertService.CountByUserId(userId);
                limitExceeded = actualUserAdvertsCount >= maxUserAdvertsCount;                
            }

            return limitExceeded;
        }

        private void AttachImageToViewModel(Advert advert, AdvertViewModel viewModel) {

            if (advert?.ImageHash?.Length > 0)
            {
                var img = fileStorageService.GetFileData(advert.ImageHash);                

                viewModel.AdvertDto.Image = img;
            }
        }
    }
}
