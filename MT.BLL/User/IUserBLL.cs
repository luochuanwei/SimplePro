using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MT.Model.Model;
using MT.WCF.User.Request;

namespace MT.BLL.User
{
    public interface IUserBLL
    {
        /// <summary>
        /// 通过用户ID获取用户信息
        /// </summary>
        /// <param name="param">用户ID</param>
        /// <returns>用户信息</returns>
        UserEntity GetUserInfoByUserId(ParamForGetUser param);
    }
}
