using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProductMarketModels.ViewModels.Admin.CategoryController
{
    public class EditCategoryViewModel
    {
        [Required(ErrorMessage = "Пожалуйста, введите название категории")]
        [MinLength(3)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Постер продукта")]
        public IFormFile file { get; set; }

        // Категория продукта
        public int IdCategory { get; set; }
    }
}
