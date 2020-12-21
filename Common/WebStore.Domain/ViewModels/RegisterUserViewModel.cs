using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        [MaxLength(15)]
        [Remote("IsNameFree","Account")]
        [MinLength(3, ErrorMessage = "Минимальная длина 3 символов")]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        //[Required, DataType(DataType.EmailAddress)]
        //public string Email { get; set; }

        [Display(Name = "Пароль")]
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
