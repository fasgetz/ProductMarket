using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSiteProductMarket.Identity;
using WebSiteProductMarket.Models.ViewModels.Profile;

namespace WebSiteProductMarket.Service
{
    public class ProfileService : IProfileService
    {
        private readonly UsersContext _contextUsers;
        private readonly UserManager<User> _userManager;

        public ProfileService(UsersContext context, UserManager<User> userManager)
        {
            this._contextUsers = context;
            this._userManager = userManager;
        }

        /// <summary>
        /// Получить юзера
        /// </summary>
        /// <param name="UserName">Имя юзера</param>
        /// <returns>Юзера</returns>
        public async Task<User> GetUser(string UserName)
        {
            var user = await _contextUsers.Users.FirstOrDefaultAsync(i => i.Email == UserName);

            return user;
        }


        /// <summary>
        /// Удалить аватар пользователю
        /// </summary>
        /// <param name="userName">Айди юзера</param>
        /// <returns>true, если аватар успешно удален из БД</returns>
        public async Task<bool> RemoveUserAvatar(string userName)
        {
            var user = await _contextUsers.Users.FirstOrDefaultAsync(i => i.Email == userName);

            if (user == null)
                return false;

            user.ProfileImage = null;
            await _contextUsers.SaveChangesAsync();

            return true;
        }


        /// <summary>
        /// Обновить аватар юзера
        /// </summary>
        /// <param name="userName">Айди юзера</param>
        /// <param name="data">Массив байтов image</param>
        /// <returns>true, если аватар обновлен</returns>
        public async Task<bool> UpdateAvatar(string userName, byte[] data)
        {
            var user = await _contextUsers.Users.FirstOrDefaultAsync(i => i.Email == userName);

            if (user == null)
                return false;

            user.ProfileImage = data;
            await _contextUsers.SaveChangesAsync();

            return true;
        }


        /// <summary>
        /// Обновить модель
        /// </summary>
        /// <param name="vm">Юзер модель</param>
        /// <returns>true, если обновилось</returns>
        public async Task<bool> UpdateUser(IndexProfileViewModel vm)
        {

            var user = await _contextUsers.Users.FirstOrDefaultAsync(i => i.Email == vm.email);


            if (user == null)
                return false;


            user.Name = vm.Name;
            user.Family = vm.Family;
            user.dateBirth = new DateTime(vm.YearBirth, vm.MonthBirth, vm.DayBirth);
            _contextUsers.SaveChanges();

            // Смена пароля пользователем, если он ввел поля
            if (!string.IsNullOrEmpty(vm.NewPassword) && !string.IsNullOrEmpty(vm.OldPassword))
            {
                User Identity = await _userManager.FindByEmailAsync(vm.email);

                if (Identity != null)
                {
                    IdentityResult result = await _userManager.ChangePasswordAsync(Identity, vm.OldPassword, vm.NewPassword);

                    if (!result.Succeeded)
                        return false;
                }
            }

            return true;
        }
    }
}
