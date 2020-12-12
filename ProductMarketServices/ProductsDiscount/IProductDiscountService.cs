using ProductMarketModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.ProductsDiscount
{
    public interface IProductDiscountService
    {
        /// <summary>
        /// Получает все скидки продукта
        /// </summary>
        /// <param name="idProduct">Номер продукта</param>
        /// <returns>Все скидки продукта</returns>
        public Task<List<DiscountProduct>> GetDiscountsProduct(int idProduct);

        /// <summary>
        /// Добавить продукт
        /// </summary>
        /// <param name="discount">Редактируемая Акция</param>
        public void AddDiscountProduct(DiscountProduct discount);


        /// <summary>
        /// Редактирование продукта
        /// </summary>
        /// <param name="discount">Редактируемая Акция</param>
        public void EditDiscountProduct(DiscountProduct discount);



        /// <summary>
        /// Удаление из базы данных
        /// </summary>
        /// <param name="IdDiscount">Номер акции</param>
        /// <returns>True, если удаление успешно</returns>
        public Task<bool> RemoveDiscountProduct(int IdDiscount);
    }
}
