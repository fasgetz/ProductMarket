using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSiteProductMarket.Identity;
using WebSiteProductMarket.Models.ViewModels.Profile;

namespace WebSiteProductMarket.Service
{
    public interface IProfileService
    {
        /// <summary>
        /// Удалить аватар пользователю
        /// </summary>
        /// <param name="userName">Айди юзера</param>
        /// <returns>true, если аватар успешно удален из БД</returns>
        Task<bool> RemoveUserAvatar(string userName);

        /// <summary>
        /// Обновить аватар юзера
        /// </summary>
        /// <param name="userName">Айди юзера</param>
        /// <param name="data">Массив байтов image</param>
        /// <returns>true, если аватар обновлен</returns>
        Task<bool> UpdateAvatar(string userName, byte[] data);

        /// <summary>
        /// Получить юзера
        /// </summary>
        /// <param name="UserName">Имя юзера</param>
        /// <returns>Юзера</returns>
        Task<User> GetUser(string UserName);


        /// <summary>
        /// Обновить модель
        /// </summary>
        /// <param name="vm">Юзер модель</param>
        /// <returns>true, если обновилось</returns>
        Task<bool> UpdateUser(IndexProfileViewModel vm);
    }
}
