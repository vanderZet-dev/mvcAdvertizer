using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcAdvertizer.Data.DTO;
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

        public AdvertDto AdvertDto { get; set; }

        public bool ReadOnly { get; set; } = false;

        public bool HideImageChooser { get; set; } = true;

        [Display(Name = "Выберите изображение")]  
        [AllowedExtensions(new string[] { ".jpg", ".png" }, "Изображение имеет не верный формат")]
        [MaxFileSize(10, "Объем загружаемых файлов не должен превышать 10 мб")]
        public IFormFile ImageFromFile { get; set; }
                
        public string Img() {

            if (AdvertDto?.Image != null)
            {
                var base64 = Convert.ToBase64String(AdvertDto.Image);
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
        public string PublishingDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(' ', 'T');

        public bool ShowViewModelPublishingDate { get; set; } = false;
        
        public bool ShowRecaptchaErrorMessage { get; set; } = false;

        public bool ShowMaxUserAdvertsCountLimitErrorMessage { get; set; } = false;



        public void SetupCreateBeforePost(List<UserDto> users) {

            ShowViewModelPublishingDate = true;
            HideImageChooser = false;

            InitialUserSelectList(users);
            RetrieveIFormFile();
        }

        public void SetupCreateAfterPost(List<UserDto> users, bool showRecaptchaErrorMessage, bool showMaxUserAdvertsCountLimitErrorMessage) {

            ShowRecaptchaErrorMessage = showRecaptchaErrorMessage;
            AdvertDto.Image = IFromFileUtils.IFormFileToByteArray(ImageFromFile);

            ShowViewModelPublishingDate = true;
            HideImageChooser = false;
            ShowMaxUserAdvertsCountLimitErrorMessage = showMaxUserAdvertsCountLimitErrorMessage;

            InitialUserSelectList(users);
            RetrieveIFormFile();

            AdvertDto.PublishingDate = Convert.ToDateTime(PublishingDate);
        }

        public void SetupForDetail(AdvertDto advertDto, List<UserDto> users) {

            AdvertDto = advertDto;
            InitialUserSelectList(users);
            RetrieveIFormFile();
            ReadOnly = true;
        }

        public void SetupForEditBeforePost(AdvertDto advertDto, List<UserDto> users) {

            AdvertDto = advertDto;
            InitialUserSelectList(users);
            RetrieveIFormFile();
            ReadOnly = false;
        }

        public void InitialUserSelectList(List<UserDto> users) {            

            var selectedElement = users.FirstOrDefault(x => x.Id == AdvertDto?.UserId);
            UserSelectList = new SelectList(users, "Id", "Name", selectedElement);            
        }

        public void RetrieveIFormFile() {
                        
            if (AdvertDto?.Image != null)
            {
                ImageFromFile = IFromFileUtils.ByteArrayToIFormFile(AdvertDto.Image);
            }            
        }
    }
}
