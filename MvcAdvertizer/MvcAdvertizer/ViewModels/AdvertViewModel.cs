using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcAdvertizer.Data.Models;
using MvcAdvertizer.Utils.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcAdvertizer.ViewModels
{
    public class AdvertViewModel {
        public SelectList UserSelectList { get; set; }

        public Advert Advert { get; set; }

        [Display(Name = "Выберите изображение")]
        [Required(ErrorMessage = "Вы не указали изображение")]
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
    }
}
