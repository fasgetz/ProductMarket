using ProductMarketModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.Notification
{

    /// <summary>
    /// Сервис оповещений пользователей
    /// </summary>
    public class NotificationServiceEmail : INotificationService
    {

        public NotificationServiceEmail()
        {

        }



        /// <summary>
        /// Оповещение пользователя
        /// </summary>
        /// <param name="order">Заказ</param>
        /// <returns></returns>
        public void NotificationUser(Order order)
        {
            try
            {
                // отправитель - устанавливаем адрес и отображаемое в письме имя
                MailAddress from = new MailAddress("marketproductsbot@gmail.com", "Сервис-почта");
                // кому отправляем
                MailAddress to = new MailAddress(order.UserId);


                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);

                // тема письма
                m.Subject = $"Заказ магазина market-products {order.Id}";


                // текст письма
                m.Body = $"<h1>Ваш заказ успешно оформлен!</h2>";
                m.Body += $"<p>Номер вашего заказа: {order.Id}</p>";
                m.Body += $"<p>Сумма вашего заказа: {order.TotalPrice}</p>";
                m.Body += $"<p>P.S. произведена симуляция оплаты, оформление заказа ненастоящее!</p>";
                // письмо представляет код html
                m.IsBodyHtml = true;
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

                // логин и пароль
                smtp.Credentials = new NetworkCredential("marketproductsbot@gmail.com", "mybot12345S");
                smtp.EnableSsl = true;

                // Отправляем
                smtp.Send(m);
            }
            catch (Exception ex)
            {

            }

        }
    }
}
