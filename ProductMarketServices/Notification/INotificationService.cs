using ProductMarketModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.Notification
{

    /// <summary>
    /// Сервис SMS оповещений пользователей
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Оповещение пользователя
        /// </summary>
        /// <param name="order">Заказ</param>
        /// <returns></returns>
        public void NotificationUser(Order order);
    }
}
