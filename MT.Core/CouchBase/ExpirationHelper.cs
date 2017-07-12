using System;

namespace MT.LQQ.Core.CouchBase
{
    /// <summary>
    /// CouchBase 过期时间助手
    /// </summary>
    public class ExpirationHelper
    {
        private const int MaxSeconds = 60 * 60 * 24 * 30;
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);

        /// <summary>
        /// 获取过期时间，单位秒。
        /// </summary>
        /// <param name="validFor"></param>
        /// <returns></returns>
        public static uint GetExpirationBySeconds(TimeSpan validFor)
        {
            if (validFor == TimeSpan.Zero || validFor == TimeSpan.MaxValue)
            {
                return 0;
            }
            var seconds = (uint) validFor.TotalSeconds;
            return seconds > MaxSeconds ? GetExpirationBySeconds(DateTime.Now.Add(validFor)) : seconds;
        }

        /// <summary>
        /// 获取过期时间，单位秒。
        /// </summary>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        public static uint GetExpirationBySeconds(DateTime expiresAt)
        {
            if (expiresAt < UnixEpoch)
                throw new ArgumentOutOfRangeException(nameof(expiresAt), "expiresAt must be >= 1970/1/1");
            if (expiresAt == DateTime.MaxValue)
            {
                return 0;
            }
            var retval = (uint) (expiresAt.ToUniversalTime() - UnixEpoch).TotalSeconds;
            return retval;
        }

        /// <summary>
        /// 获取过期时间，单位分钟。
        /// </summary>
        /// <param name="validFor"></param>
        /// <returns></returns>
        public static uint GetExpirationByMinutes(TimeSpan validFor)
        {
            if (validFor == TimeSpan.Zero || validFor == TimeSpan.MaxValue)
            {
                return 0;
            }
            var minutes = (uint) validFor.TotalMinutes;
            return minutes > MaxSeconds / 60 ? GetExpirationByMinutes(DateTime.Now.Add(validFor)) : minutes;
        }

        /// <summary>
        /// 获取过期时间，单位分钟。
        /// </summary>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        public static uint GetExpirationByMinutes(DateTime expiresAt)
        {
            if (expiresAt < UnixEpoch)
                throw new ArgumentOutOfRangeException(nameof(expiresAt), "expiresAt must be >= 1970/1/1");
            if (expiresAt == DateTime.MaxValue)
            {
                return 0;
            }
            var retval = (uint) (expiresAt.ToUniversalTime() - UnixEpoch).TotalMinutes;
            return retval;
        }
    }
}