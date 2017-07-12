using MT.Core.DB;
using MT.LQQ.Core.CouchBase;
using MT.Model.Model;

namespace MT.DAL.User.Instance
{
    public class UserDAL : DataAccessBase, IUserDAL
    {
        private readonly CouchBaseFactory _couchBaseFactory;

        public UserDAL()
        {
            //_couchBaseFactory = couchBaseFactory;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserEntity GetUserInfo(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            //var key = UserInfoStored.GetKey(userId);
            //var stored = _couchBaseFactory.Get<UserInfoStored>(UserInfoStored.GetBucket(), key);
            //if (stored != null)
            //{
            //    return stored.UserInfo;
            //}

            var userInfo = GetUserInfoByUserIdFromDb(userId);
            //stored = new UserInfoStored
            //{
            //    UserId = userId,
            //    UserInfo = userInfo
            //};

            //_couchBaseFactory.Update(stored, UserInfoStored.CacheTimeSpan);

            if (userInfo == null)
            {
                return null;
            }

            return userInfo.IsEnable ? userInfo : null;
        }

        /// <summary>
        /// 从数据库中获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private UserEntity GetUserInfoByUserIdFromDb(string userId)
        {
            var sql = "select * from test";

            var userInfo = DbContext.IgnoreIfAutoMapFails(true)
                .Sql(sql)
                .Parameter("userid", userId)
                .QuerySingle<UserEntity>();

            return userInfo;
        }
    }
}
