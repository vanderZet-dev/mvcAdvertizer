using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Data.DTO
{
    public class AdvertDto
    {        
        public Guid? Id { get; set; }

        [Display(Name = "Номер")]
        [Required(ErrorMessage = "Необходимо указать номер для объявления")]
        public int Number { get; set; }

        [Display(Name = "Выберите пользователя")]
        [Required(ErrorMessage = "Нельзя создать объявление, не указав пользователя")]
        public Guid? UserId { get; set; }

        public UserDto User { get; set; }

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

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
                
        public bool? Selected { get; set; } = false;
    }
}
