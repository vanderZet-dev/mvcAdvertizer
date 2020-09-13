using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcAdvertizer.Data.Models;
using MvcAdvertizer.Utils;
using MvcAdvertizer.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MvcAdvertizer.ViewModels
{
    public class AdvertViewModel {

        public SelectList UserSelectList { get; set; }

        public Advert Advert { get; set; }

        public bool ReadOnly { get; set; } = false;

        public bool HideImageChooser { get; set; } = true;

        [Display(Name = "Выберите изображение")]  
        [AllowedExtensions(new string[] { ".jpg", ".png" }, "Изображение имеет не верный формат")]
        [MaxFileSize(10, "Объем загружаемых файлов не должен превышать 10 мб")]
        public IFormFile ImageFromFile { get; set; }
                
        public string Img() {

            if (Advert?.Image != null)
            {
                var base64 = Convert.ToBase64String(Advert.Image);
                var imgSrc = String.Format("data:image/png;base64,{0}", base64);

                return imgSrc;
            }
            else return "";            
        }

        public bool ImgExists() {

            var exists = true;

            if (Img().Equals(""))
            {
                exists = false;
            }

            return exists;
        }

        [Display(Name = "Укажите предпочитаемую дату публикации")]
        public string PublishingDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        public bool ShowViewModelPublishingDate { get; set; } = false;
        
        public bool ShowRecaptchaErrorMessage { get; set; } = false;     


        
        public void SetupCreateBeforePost(List<User> users) {

            ShowViewModelPublishingDate = true;
            HideImageChooser = false;

            InitialUserSelectList(users);
            RetrieveIFormFile();
        }

        public void SetupCreateAfterPost(List<User> users, bool showRecaptchaErrorMessage) {

            ShowRecaptchaErrorMessage = showRecaptchaErrorMessage;
            Advert.Image = IFromFileUtils.IFormFileToByteArray(ImageFromFile);

            ShowViewModelPublishingDate = true;
            HideImageChooser = false;
            
            InitialUserSelectList(users);
            RetrieveIFormFile();
            
            Advert.PublishingDate = Convert.ToDateTime(PublishingDate);
        }

        public void SetupForDetail(Advert advert, List<User> users) {

            Advert = advert;
            InitialUserSelectList(users);
            RetrieveIFormFile();
            ReadOnly = true;
        }

        public void SetupForEditBeforePost(Advert advert, List<User> users) {

            Advert = advert;
            InitialUserSelectList(users);
            RetrieveIFormFile();
            ReadOnly = false;
        }

        public void InitialUserSelectList(List<User> users) {            

            var selectedElement = users.FirstOrDefault(x => x.Id == Advert?.UserId);
            UserSelectList = new SelectList(users, "Id", "Name", selectedElement);            
        }

        public void RetrieveIFormFile() {
                        
            if (Advert?.Image != null)
            {
                ImageFromFile = IFromFileUtils.ByteArrayToIFormFile(Advert.Image);
            }            
        }
    }
}
