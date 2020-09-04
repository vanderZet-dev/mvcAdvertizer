using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcAdvertizer.Data.Interfaces;
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

        public IActionResult Create() {

            var viewModel = generateInitialCreateAdvertViewModel();

            return View(viewModel);
        }        

        [HttpPost]
        public IActionResult Create(CreateAdvertViewModel viewModel) {

            viewModel = addUserSelectList(viewModel);

            viewModel.Advert.ImageName = viewModel.Advert?.ImageFromFile?.FileName;
            viewModel.Advert.ImageContent = fromFormFileToByteArray(viewModel.Advert.ImageFromFile);

            if (ModelState.IsValid)
            {
                advertRepository.Save(viewModel.Advert);
                return RedirectToAction("CreateSuccess");
            }
            else
            {
                return View(viewModel);
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

        private CreateAdvertViewModel generateInitialCreateAdvertViewModel() {

            var viewModel = new CreateAdvertViewModel();

            viewModel = addUserSelectList(viewModel);

            return viewModel;
        }    
        
        private CreateAdvertViewModel addUserSelectList(CreateAdvertViewModel viewModel) {

            var userList = userRepository.findAll().ToList();
            var advert = viewModel.Advert;

            var selectedElement = userList.FirstOrDefault(x => x.Id == advert?.UserId);
            viewModel.UserSelectList = new SelectList(userList, "Id", "Name", selectedElement);

            return viewModel;
        }
    }
}
