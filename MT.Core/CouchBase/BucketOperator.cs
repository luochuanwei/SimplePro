using System;
using Couchbase;
using Couchbase.Core;

namespace MT.LQQ.Core.CouchBase
{
    /// <summary>
    /// CouchBase Bucket 操作。
    /// </summary>
    public class BucketOperator : IBucketProvider, IDisposable
    {
        /// <summary>
        /// Bucket
        /// </summary>
        private readonly IBucket _bucket;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="bucket">Bucket</param>
        public BucketOperator(IBucket bucket)
        {
            _bucket = bucket;
        }

        /// <summary>
        /// 重置为给定的文档ID所提供的数值的到期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        public bool Touch(string key, TimeSpan expiration)
        {
            try
            {
                var result = _bucket.Touch(key, expiration);
                return result.Success;
            }
            catch (Exception ex)
            {
                throw new Exception($"CouchBase Touch Operate Error,Message:{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取文档并重置到期时提供的给定值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        public T GetAndTouch<T>(string key, TimeSpan expiration)
        {
            try
            {
                var result = _bucket.GetAndTouch<T>(key, expiration);
                return result.Success ? result.Value : default(T);
            }
            catch (Exception ex)
            {
                throw new Exception($"CouchBase GetAndTouch Operate Error,Message:{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            try
            {
                var result = _bucket.Get<T>(key);
                return result.Success ? result.Value : default(T);
            }
            catch (Exception ex)
            {
                throw new Exception($"CouchBase Get Operate Error,Message:{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add<T>(string key, T value)
        {
            return Add(key, value, TimeSpan.FromMilliseconds(0));
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public bool Add<T>(string key, T value, TimeSpan timeSpan)
        {
            return Update(key, value, timeSpan);
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        /// <typeparam name="T">存储的类型</typeparam>
        /// <param name="key">KEY</param>
        /// <param name="value">对象</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns>True：操作成功，False：操作失败</returns>
        public bool Update<T>(string key, T value, TimeSpan timeSpan)
        {
            try
            {
                var doc = new Document<dynamic> { Id = key, Content = value };
                var milliseconds = Convert.ToUInt32(timeSpan.TotalMilliseconds);
                if (milliseconds > 0)
                {
                    doc.Expiry = milliseconds;
                }
                var result = _bucket.Upsert(doc);

                return result.Success;
            }
            catch (Exception ex)
            {
                throw new Exception($"CouchBase Upsert Operate Error,Message:{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 移除（键不存在，也是删除成功）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            try
            {
                if (!_bucket.Exists(key))
                {
                    return true;
                }
                var result = _bucket.Remove(key);
                return result.Success;
            }
            catch (Exception ex)
            {
                throw new Exception($"CouchBase Remove Operate Error, Message：{ex.Message}", ex);
            }
        }

        public void Dispose()
        {
            _bucket.Dispose();
        }
    }
}