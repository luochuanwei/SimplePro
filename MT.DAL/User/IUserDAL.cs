using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MT.Model.Model;

namespace MT.DAL.User
{
    public interface IUserDAL
    {
        UserEntity GetUserInfo(string userId);
    }
}
