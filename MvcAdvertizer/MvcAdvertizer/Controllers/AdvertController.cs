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

        
        public IActionResult Details(Guid id) {

            var vm = new AdvertViewModel();
            vm.ReadOnly = true;            

            var advert = advertRepository.findById(id);
            vm = addUserSelectList(vm);

            vm.Advert = advert;

            return View(vm);
        }

        public IActionResult Edit(Guid id) {

            var vm = new AdvertViewModel();
            vm.ReadOnly = false;
           
            var advert = advertRepository.findById(id);
            vm = addUserSelectList(vm);

            vm.Advert = advert;

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(AdvertViewModel viewModel) {

            viewModel = addUserSelectList(viewModel);

            viewModel.Advert.Image = fromFormFileToByteArray(viewModel.ImageFromFile);           

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

            var vm = generateInitialCreateAdvertViewModel();
            vm.HideImageChooser = false;

            return View(vm);
        }        

        [HttpPost]
        public IActionResult Create(AdvertViewModel vm) {
                                    
            vm.Advert.Image = fromFormFileToByteArray(vm.ImageFromFile);

            vm = addUserSelectList(vm);
            vm.HideImageChooser = false;
            vm.ShowViewModelPublishingDate = true;

            vm.Advert.PublishingDate = Convert.ToDateTime(vm.PublishingDate);

            if (ModelState.IsValid)
            {
                var created = advertRepository.Save(vm.Advert);
                return RedirectToAction("Details", new { id = created.Id });
            }
            else
            {
                return View(vm);
            }            
        }

        public IActionResult ShowImage(Guid id) {

            var vm = new AdvertViewModel();            

            var advert = advertRepository.findById(id);            

            vm.Advert = advert;

            return View(vm);
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

            viewModel.ShowViewModelPublishingDate = true;
            viewModel.PublishingDate = DateTime.Now.ToString("yyyy-MM-dd");

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
