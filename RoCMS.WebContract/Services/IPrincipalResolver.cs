using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Web.Contract.Models.Security;

namespace RoCMS.Web.Contract.Services
{
    /// <summary>
    /// Сервис, определяющий текущего пользователя
    /// </summary>
    public interface IPrincipalResolver
    {

        /// <summary>
        /// Возвращает текущего пользователя
        /// </summary>
        /// <returns></returns>
        RoPrincipal GetCurrentUser();

        /// <summary>
        /// Возвращает идентификатор пользователя
        /// </summary>
        /// <returns></returns>
        int GetUserId();
        /// <summary>
        /// Возвращает ID юзера или null, если не авторизован
        /// </summary>
        /// <returns></returns>
        int? GetUserIdIfAuthenticated();
    }
}
