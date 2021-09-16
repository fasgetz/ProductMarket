using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.EmailServiceNew
{
    public interface IEmalService
    {
        /// <summary>
        /// Метод отправки сообщения на email
        /// </summary>
        /// <param name="emailTo">Адрес</param>
        /// <param name="subject">Тема сообщения</param>
        /// <param name="message">Сообщение</param>
        /// <param name="description">Дескрипшен к почте (предисловие в письме)</param>
        /// <returns></returns>
        public Task SendEmailAsync(string emailTo, string subject, string message, string description = "Сообщение от сервиса busmansoft");
    }
}
