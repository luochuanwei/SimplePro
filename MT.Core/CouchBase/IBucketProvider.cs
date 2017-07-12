using System;

namespace MT.LQQ.Core.CouchBase
{
    /// <summary>
    /// 缓存提供程序
    /// </summary>
    public interface IBucketProvider
    {
        /// <summary>
        /// 重置为给定的文档ID所提供的数值的到期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        bool Touch(string key, TimeSpan expiration);

        /// <summary>
        /// 获取文档并重置到期时提供的给定值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        T GetAndTouch<T>(string key, TimeSpan expiration);

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>缓存对象</returns>
        T Get<T>(string key);

        /// <summary>
        /// 添加(无过期时间)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>True:成功，False:失败</returns>
        bool Add<T>(string key, T value);

        ///// <summary>
        ///// 添加
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <param name="minutes">缓存多长时间，单位分钟</param>
        ///// <returns>True:成功，False:失败</returns>
        //[Obsolete("已过期")]
        //bool Add<T>(string key, T value, uint minutes);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        bool Add<T>(string key, T value, TimeSpan timeSpan);

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        bool Update<T>(string key, T value, TimeSpan timeSpan);

        ///// <summary>
        ///// 更新缓存
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <param name="minutes">缓存多长时间，单位分钟，0,则用不过期</param>
        ///// <returns>True:成功，False:失败</returns>
        //[Obsolete("已过期")]
        //bool Update<T>(string key, T value, uint minutes);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        /// <returns>True:成功，False:失败</returns>
        bool Remove(string key);
    }
}