using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MT.DAL.User;
using MT.Model.Model;
using MT.WCF.User.Request;

namespace MT.BLL.User.Instance
{
    public class UserBLL : IUserBLL
    {
        private readonly IUserDAL _userDAL;

        public UserBLL(IUserDAL userDAL)
        {
            _userDAL = userDAL;
        }

        /// <summary>
        /// 通过用户ID获取用户信息
        /// </summary>
        /// <param name="param">用户ID</param>
        /// <returns>用户信息</returns>
        public UserEntity GetUserInfoByUserId(ParamForGetUser param)
        {
            return string.IsNullOrEmpty(param.UserId) ? null : _userDAL.GetUserInfo(param.UserId);
        }
    }
}
