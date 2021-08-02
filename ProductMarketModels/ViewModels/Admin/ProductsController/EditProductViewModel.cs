using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMarketModels.ViewModels.Admin.ProductsController
{
    /// <summary>
    /// Модель редактирования продукта
    /// </summary>
    public class EditProductViewModel
    {
        [Required]
        public int id { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите название продукта")]
        [MinLength(5)]
        [Display(Name = "Название")]
        public string name { get; set; }

        /// <summary>
        /// Описание товара
        /// </summary>
        public string description { get; set; }

        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Пожалуйста, выберите подкатегорию продукта")]
        [Range(1, double.MaxValue, ErrorMessage = "Выберите значение > 0")]
        [Display(Name = "Категория продукта")]
        public int subcategoryId { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите стоимость продукта")]
        [Range(1, double.MaxValue, ErrorMessage = "Введите стоимость в пределе диапазона 1 ... 1000000")]
        [Display(Name = "Стоимость")]
        public int price { get; set; }


        [Required(ErrorMessage = "Пожалуйста, введите количество товара")]
        [Range(1, double.MaxValue, ErrorMessage = "Введите количество товара в пределе диапазона 1 ... 1000000")]
        [Display(Name = "Количество")]
        public int count { get; set; }

        public byte[] image { get; set; }

        [Display(Name = "Постер продукта")]
        public IFormFile file { get; set; }

    }
}
