using Microsoft.AspNetCore.Http;
using MvcAdvertizer.Config.Database;
using MvcAdvertizer.Utils.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcAdvertizer.Data.Models
{
    public class Advert : IAuditedEntity
    {
        [Key]
        public Guid Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; set; }

        [Display(Name = "Выберите пользователя")]
        [Required(ErrorMessage = "Нельзя создать объявление, не указав пользователя")]
        public Guid? UserId { get; set; }

        public virtual User User { get; set; }

        [Display(Name = "Текст")]        
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Длина строки не должна быть от 5 до 200 символов")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string Content { get; set; }
        
        [NotMapped]
        [Display(Name = "Выберите изображение")]
        [Required(ErrorMessage = "Вы не указали изображение")]
        [AllowedExtensions(new string[] { ".jpg", ".png" }, "Изображение имеет не верный формат")] 
        [MaxFileSize(10, "Объем загружаемых файлов не должен превышать 10 мб")]
        public IFormFile ImageFromFile { get; set; }

        public string ImageName { get; set; }
        public byte[] ImageContent { get; set; }

        [Display(Name = "Выставьте рейтинг от 1 до 10")]
        [Required(ErrorMessage = "Вы не указали рейтинг")]
        [Range(1, 10, ErrorMessage = "Рейтинг можно выставить в диапазоне от 1 до 10")]
        public int? Rate { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [NotMapped]
        public bool? Selected { get; set; } = false;
    }
}
