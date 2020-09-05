using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Data.Models;
using MvcAdvertizer.ViewModels;

namespace MvcAdvertizer.Controllers
{
    public class AdvertController : Controller
    {
        private readonly IUsers userRepository;
        private readonly IAdverts advertRepository;

        public AdvertController(IUsers userRepository,
                                IAdverts advertRepository) {

            this.userRepository = userRepository;
            this.advertRepository = advertRepository;
        }

        
        public IActionResult AdvertDetails(Guid id) {

            var vm = new AdvertViewModel();            

            var advert = advertRepository.findById(id);
            vm = addUserSelectList(vm);

            vm.Advert = advert;

            return View(vm);
        }
        
        public IActionResult Create() {

            var viewModel = generateInitialCreateAdvertViewModel();

            return View(viewModel);
        }        

        [HttpPost]
        public IActionResult Create(AdvertViewModel viewModel) {
                                    
            viewModel.Advert.Image = fromFormFileToByteArray(viewModel.ImageFromFile);

            viewModel = addUserSelectList(viewModel);

            if (ModelState.IsValid)
            {
                var saved = advertRepository.Save(viewModel.Advert);                
                return RedirectToAction("AdvertDetails", new { id = saved.Id });
            }
            else
            {
                return View(viewModel);
            }            
        }

        [HttpPost]
        public IActionResult Save(AdvertViewModel viewModel) {

            viewModel = addUserSelectList(viewModel);
                        
            viewModel.Advert.Image = fromFormFileToByteArray(viewModel.ImageFromFile);

            if (ModelState.IsValid)
            {
                var updated = advertRepository.Update(viewModel.Advert);
                return RedirectToAction("AdvertDetails", new { id = updated.Id });
            }
            else
            {
                return View("AdvertDetails", viewModel);
            }
        }

        public IActionResult CreateSuccess() {
            ViewBag.Message = "Новое объявление успешно добавлено";
            return View();
        }

        private byte[] fromFormFileToByteArray(IFormFile file) { 
            
            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();                    
                    return fileBytes;
                }
            }
            return null;
        }        

        private AdvertViewModel generateInitialCreateAdvertViewModel() {

            var viewModel = new AdvertViewModel();

            viewModel = addUserSelectList(viewModel);

            return viewModel;
        }    
        
        private AdvertViewModel addUserSelectList(AdvertViewModel viewModel) {

            var userList = userRepository.findAll().ToList();
            var advert = viewModel.Advert;

            var selectedElement = userList.FirstOrDefault(x => x.Id == advert?.UserId);
            viewModel.UserSelectList = new SelectList(userList, "Id", "Name", selectedElement);

            viewModel.ImageFromFile = retrieveIFormFile(viewModel.Advert);

            return viewModel;
        }

        private IFormFile retrieveIFormFile(Advert advert) {

            IFormFile file = null;

            if (advert?.Image != null)
            {
                var stream = new MemoryStream(advert.Image);
                file = new FormFile(stream, 0, advert.Image.Length, "name", "fileName");
            }           

            return file;
        }
    }
}
