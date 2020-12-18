using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebSiteProductMarket.Identity;

namespace WebSiteProductMarket.Models.ViewModels.Profile
{
    public class IndexProfileViewModel
    {
        public User user { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите имя")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите фамилию")]
        [Display(Name = "Фамилия")]
        public string Family { get; set; }

        [DataType(DataType.Password)]

        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]

        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }

        public int DayBirth { get; set; }
        public int MonthBirth { get; set; }
        public int YearBirth { get; set; }


        /// <summary>
        /// EMAIL пользователя
        /// </summary>
        public string email { get; set; }
    }
}
