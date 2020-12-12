using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProductMarketModels.ViewModels.Admin.DiscountController
{

    /// <summary>
    /// Редактирование акции продукта
    /// </summary>
    public partial class EditDiscountViewModel
    {
        // Айди акции
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите дату начала акции")]
        //[Range(1, double.MaxValue, ErrorMessage = "Выберите значение > 0")]
        [Display(Name = "Категория продукта")]
        public DateTime DateStart { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите дату окончания акции")]
        //[Range(1, double.MaxValue, ErrorMessage = "Выберите значение > 0")]
        [Display(Name = "Категория продукта")]
        public DateTime DateEnd { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите процент скидки")]
        [Range(1, 100, ErrorMessage = "Введите процент в пределе диапазона 1 ... 100")]
        [Display(Name = "Процент")]
        public float ProcentDiscount { get; set; }
    }
}
