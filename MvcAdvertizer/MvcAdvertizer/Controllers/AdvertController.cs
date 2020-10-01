﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcAdvertizer.Core.Exceptions;
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
        private readonly IRecaptchaService recaptchaService;
        private readonly IMapper mapper;
        private readonly IAdvertService advertService;
        private readonly IUserService userService;
        private readonly IFileStorageService fileStorageService;

        public AdvertController(IRecaptchaService recaptchaService,
                                IMapper mapper,
                                IAdvertService advertService,
                                IUserService userService,
                                IFileStorageService fileStorageService) {
            this.recaptchaService = recaptchaService;
            this.mapper = mapper;
            this.advertService = advertService;
            this.userService = userService;
            this.fileStorageService = fileStorageService;
        }

        public async Task<IActionResult> Details(Guid id) {

            var viewModel = new AdvertViewModel();

            var advertId = id;
            viewModel = await SetupForDetail(advertId, viewModel);

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(Guid id) {

            var advertId = id;
            var viewModel = await SetupForEditBeforePost(advertId);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdvertViewModel viewModel) {

            var advert = mapper.Map<Advert>(viewModel.AdvertDto);

            if (ModelState.IsValid)
            {
                await advertService.Update(advert);
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

            viewModel = await AddRecaptchaResponse(viewModel);            

            return await CreateAdvert(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWithoutRecaptcha(AdvertViewModel viewModel) {

            return await CreateAdvert(viewModel);
        }

        private async Task<IActionResult> CreateAdvert(AdvertViewModel viewModel) {

            viewModel = SetupCreateAfterPost(viewModel);

            var advert = mapper.Map<Advert>(viewModel.AdvertDto);

            try
            {
                if (ModelState.IsValid)
                {
                    if (viewModel.ImageFromFile != null)
                    {
                        var savedImageName = await fileStorageService.Save(viewModel.ImageFromFile);
                        advert.ImageHash = savedImageName;
                    }

                    await advertService.Create(advert);
                    TempData["toaster"] = ToastGeneratorUtils.GetSuccessRecordCreateSerializedToasterData();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(viewModel);
                }
            }
            catch (UserAdvertLimitExceededException ex)
            {
                Debug.WriteLine(ex.Message);
                ModelState.AddModelError("MaxUserAdvertsCountLimit", "Max user adverts count limit is exceeded.");
                viewModel.ShowMaxUserAdvertsCountLimitErrorMessage = true;
                return View(viewModel);
            }
        }

        private async Task<AdvertViewModel> AddRecaptchaResponse(AdvertViewModel viewModel) {

            var recaptchaResponse = new RecaptchaResponse();
            recaptchaResponse.Response = Request.Form["g-recaptcha-response"];
            recaptchaResponse.ConnectionRemoteIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            recaptchaResponse.Valid = await recaptchaService
                .CheckRecaptcha(recaptchaResponse.Response, recaptchaResponse.ConnectionRemoteIpAddress);

            viewModel.ShowRecaptchaErrorMessage = !recaptchaResponse.ValidateModelState(ModelState);

            return viewModel;
        }

        public async Task<IActionResult> SoftDelete(Guid id) {

            var existedAdvert = await advertService.FindById(id);

            if (existedAdvert != null && !existedAdvert.Deleted)
            {
                existedAdvert.Deleted = true;
                await advertService.Update(existedAdvert);
                TempData["toaster"] = ToastGeneratorUtils.GetSuccessRecordDeleteSerializedToasterData();
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ShowImage(Guid id, int width) {

            var viewModel = new AdvertViewModel();

            var advert = await advertService.FindById(id);

            var advertDto = mapper.Map<AdvertDto>(advert);

            viewModel.AdvertDto = advertDto;

            await AttachImageToViewModel(advert, viewModel);

            var image = viewModel.AdvertDto.Image;

            if (image != null)
            {
                viewModel.AdvertDto.Image = ImageResizerUtill.ScaledByWidth(image, width);
            }

            return View(viewModel);
        }

        private AdvertViewModel SetupCreateBeforePost() {

            var viewModel = new AdvertViewModel();

            var allUserList = userService.FindAll().ToList();
            var allUserDtoList = mapper.Map<List<UserDto>>(allUserList);
            viewModel.SetupCreateBeforePost(allUserDtoList);

            return viewModel;
        }

        private AdvertViewModel SetupCreateAfterPost(AdvertViewModel viewModel) {

            var allUserList = userService.FindAll().ToList();
            var allUserDtoList = mapper.Map<List<UserDto>>(allUserList);
            viewModel.SetupCreateAfterPost(allUserDtoList);

            return viewModel;
        }

        private async Task<AdvertViewModel> SetupForDetail(Guid advertId, AdvertViewModel viewModel) {

            var advert = await advertService.FindById(advertId);
            var allUserList = userService.FindAll().ToList();
            var allUserDtoList = mapper.Map<List<UserDto>>(allUserList);
            var advertDto = mapper.Map<AdvertDto>(advert);

            viewModel.SetupForDetail(advertDto, allUserDtoList);

            await AttachImageToViewModel(advert, viewModel);

            return viewModel;
        }

        private async Task<AdvertViewModel> SetupForEditBeforePost(Guid advertId) {

            var viewModel = new AdvertViewModel();

            var advert = await advertService.FindById(advertId);
            var allUserList = userService.FindAll().ToList();
            var allUserDtoList = mapper.Map<List<UserDto>>(allUserList);
            var advertDto = mapper.Map<AdvertDto>(advert);

            viewModel.SetupForEditBeforePost(advertDto, allUserDtoList);

            return viewModel;
        }

        private async Task AttachImageToViewModel(Advert advert, AdvertViewModel viewModel) {

            if (advert?.ImageHash?.Length > 0)
            {
                var img = await fileStorageService.GetFileData(advert.ImageHash);

                viewModel.AdvertDto.Image = img;
            }
        }
    }
}
