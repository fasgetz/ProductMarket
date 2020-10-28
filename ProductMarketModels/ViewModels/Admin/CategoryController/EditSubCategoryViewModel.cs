using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProductMarketModels.ViewModels.Admin.CategoryController
{
    public class EditSubCategoryViewModel
    {
        [Required(ErrorMessage = "Пожалуйста, введите название категории")]
        [MinLength(3)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, выберите категорию")]
        [Display(Name = "Категория")]
        public int IdCategory { get; set; }

        [Display(Name = "Постер продукта")]
        public IFormFile file { get; set; }

        // Категория продукта
        public int IdSubCategory { get; set; }
    }
}
