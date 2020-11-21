using System;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels
{
    public class CarViewModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле является обязательным")]
        [Display(Name = "Марка автомобиля")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Ожидается не менее 2-х символов")]
        public string Brand { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле является обязательным")]
        [Display(Name = "Модель автомобиля")]
        public string Model { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле является обязательным")]
        [Display(Name = "Двигатель автомобиля")]
        public string Engine { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле является обязательным")]
        [Display(Name = "Год выпска автомобиля")]
        [Range(1900, 2020, ErrorMessage = "Год выпуска должен быть между 1900 и 2020")]
        public int ReleaseYear { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле является обязательным")]
        [Display(Name = "Тип кузова автомобиля")]
        public string CarBody { get; set; }
    }
}
