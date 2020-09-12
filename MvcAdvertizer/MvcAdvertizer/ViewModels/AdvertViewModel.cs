using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcAdvertizer.Data.Models;
using MvcAdvertizer.Utils.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

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
        public string PublishingDate { get; set; }

        public bool ShowViewModelPublishingDate { get; set; } = false;        
    }
}
