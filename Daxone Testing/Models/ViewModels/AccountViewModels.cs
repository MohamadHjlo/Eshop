using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Daxone_Testing.Models.ViewModels
{
    public class RegisterViewModel
    {
        public RegisterViewModel ()
        {
            LoginViewModel= new LoginViewModel();
        }



        [Required(ErrorMessage = "{0}الزامیست")]
        [Display(Name = "نام کاربری:")]

        public string RegUserName { get; set; }

        [Required(ErrorMessage = "{0}الزامیست")]
        [Display(Name = "ایمیل:")]
        [EmailAddress]

        public string RegEmail { get; set; }

        [Phone]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "شماره تلفن: ")]
        public string RegPhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50)]
        [MinLength(6)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور: ")]
        public string RegPassword { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50)]
        [MinLength(6)]
        [DataType(DataType.Password)]
        [Compare("RegPassword")]//مقایسه کن با اون پراپرتی پسوورد تا مچ باشن
        [Display(Name = "تکرار کلمه عبور: ")]
        public string RegPasswordRepeat { get; set; }

        public LoginViewModel LoginViewModel { get; set; }
        
    }

    public class LoginViewModel
    {


        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام کاربری:")]
        public string Username { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور: ")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")] public bool LogRememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public Microsoft.AspNetCore.Http.CookieBuilder CorrelationCookie { get; set; }
        public List<AuthenticationScheme> ExternalLogins { get; set; }



    }
}
