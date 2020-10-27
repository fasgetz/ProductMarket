using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProductMarketModels.ViewModels.Admin.ProductsController
{
    public class AddProductViewModel
    {
        [Required(ErrorMessage = "Пожалуйста, введите название продукта")]
        [MinLength(5)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, загрузите постер продукта")]
        [Display(Name = "Постер продукта")]
        public IFormFile file { get; set; }

        //public byte[] Poster { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите стоимость продукта")]
        [Range(1, double.MaxValue, ErrorMessage = "Введите стоимость в пределе диапазона 1 ... 1000000")]
        [Display(Name = "Стоимость")]
        public int price { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите количество товара")]
        [Range(1, double.MaxValue, ErrorMessage = "Введите количество товара в пределе диапазона 1 ... 1000000")]
        [Display(Name = "Количество")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "Пожалуйста, выберите категорию продукта")]
        [Range(1, double.MaxValue, ErrorMessage = "Выберите значение > 0")]
        [Display(Name = "Категория продукта")]
        public short idSubCategoryProduct { get; set; }
    }
}
