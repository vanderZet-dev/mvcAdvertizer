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
        
        [Display(Name = "Номер")]
        [Required(ErrorMessage = "Необходимо указать номер для объявления")]
        public int Number { get; set; }

        [Display(Name = "Выберите пользователя")]
        [Required(ErrorMessage = "Нельзя создать объявление, не указав пользователя")]
        public Guid? UserId { get; set; }

        public virtual User User { get; set; }

        [Display(Name = "Текст")]        
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Длина строки не должна быть от 5 до 200 символов")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string Content { get; set; }
                     
        public byte[] Image { get; set; }

        [Display(Name = "Выставьте рейтинг от 1 до 10")]
        [Required(ErrorMessage = "Вы не указали рейтинг")]
        [Range(1, 10, ErrorMessage = "Рейтинг можно выставить в диапазоне от 1 до 10")]
        public int? Rate { get; set; }

        [Display(Name = "Дата публикации")]
        [Required(ErrorMessage = "Вы не указали дату публикации")]
        public DateTime PublishingDate { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [NotMapped]
        public bool? Selected { get; set; } = false;
    }
}
