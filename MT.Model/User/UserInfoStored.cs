using System;
using MT.LQQ.Models.CouchBase;
using MT.LQQ.Models.Enum;
using MT.Model.Model;

namespace MT.LQQ.Models.User
{
    /// <summary>
    /// 用户信息存储
    /// </summary>
    public class UserInfoStored : ICouchBaseStored
    {
        /// <summary>
        /// 
        /// </summary>
        public static string Prefix = "userentity";

        /// <summary>
        /// 
        /// </summary>
        public static TimeSpan CacheTimeSpan = TimeSpan.FromDays(1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetKey(string userId)
        {
            return $"{Prefix}_{userId}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataBucketEnum GetBucket()
        {
            return DataBucketEnum.User;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Key => GetKey(UserId);

        /// <summary>
        /// 
        /// </summary>
        public DataBucketEnum Bucket => GetBucket();

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserEntity UserInfo { get; set; }
    }
}