using System;
using System.Collections.Generic;
using System.Configuration;
using Couchbase;
using Couchbase.Configuration.Client;
using Couchbase.Configuration.Client.Providers;
using log4net;
using MT.LQQ.Models.CouchBase;
using MT.LQQ.Models.Enum;

namespace MT.LQQ.Core.CouchBase
{
    /// <summary>
    /// Couchbase工厂类 提供对CouchBase Bucket的操作
    /// </summary>
    public class CouchBaseFactory
    {
        private readonly Dictionary<string, BucketOperator> _bucketOperators = new Dictionary<string, BucketOperator>();
        private readonly object _lock = new object();
        private readonly string _couchbaseClientSection = "couchbaseClients/couchbase";
        private readonly ILog _logger = LogManager.GetLogger(typeof(CouchBaseFactory));

        #region Update

        /// <summary>
        /// 更新缓存（永不过期）
        /// </summary>
        /// <typeparam name="T">缓存的类型</typeparam>
        /// <param name="value">缓存对象</param>
        /// <returns>True：成功，False：失败</returns>
        public bool Update<T>(T value) where T : ICouchBaseStored
        {
            return Update(value, TimeSpan.FromMilliseconds(0));
        }

        /// <summary>
        /// 更新缓存（定过期时长）
        /// </summary>
        /// <typeparam name="T">缓存的类型</typeparam>
        /// <param name="value">缓存对象</param>
        /// <param name="timeSpan">缓存时长</param>
        /// <returns>True:成功，False：失败</returns>
        public bool Update<T>(T value, TimeSpan timeSpan) where T : ICouchBaseStored
        {
            var key = value.Key;
            var bucketName = value.Bucket.ToString();
            var bucketOperator = GetBucketOperator(bucketName);
            var result = bucketOperator.Update(key, value, timeSpan);
            if (timeSpan.TotalMilliseconds <= 0 && result)
            {
                BackUpPersistentCache(value);
            }
            return result;
        }
        #endregion


        #region Touch

        /// <summary>
        /// 重置为给定的文档ID所提供的数值的到期时间
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        public bool Touch(DataBucketEnum bucket, string key, TimeSpan expiration)
        {
            var bucketName = bucket.ToString();
            var bucketOperator = GetBucketOperator(bucketName);
            var result = bucketOperator.Touch(key, expiration);
            return result;
        }

        /// <summary>
        /// 获取文档并重置到期时提供的给定值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        public T GetAndTouch<T>(DataBucketEnum bucket, string key, TimeSpan expiration)
        {
            var bucketName = bucket.ToString();
            var bucketOperator = GetBucketOperator(bucketName);
            var result = bucketOperator.GetAndTouch<T>(key, expiration);
            return result;
        }

        #endregion

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        /// <param name="bucket">存放在哪个桶里</param>
        /// <param name="key">键名</param>
        /// <returns>缓存对象，如果没有，则返回null.</returns>
        public T Get<T>(DataBucketEnum bucket, string key)
        {
            var bucketName=bucket.ToString();
            var bucketOperator = GetBucketOperator(bucketName);
            if (bucketOperator == null || string.IsNullOrEmpty(key))
                return default(T);
            return bucketOperator.Get<T>(key);
        }

        /// <summary>
        /// 移除缓存（键不存在，也是删除成功）
        /// </summary>
        /// <param name="bucket">存放在哪个桶里</param>
        /// <param name="key">键名</param>
        /// <returns>True:成功，False：失败</returns>
        public bool Remove(DataBucketEnum bucket, string key)
        {
            var bucketName=bucket.ToString();
            var bucketOperator = GetBucketOperator(bucketName);
            if (bucketOperator == null || string.IsNullOrEmpty(key))
                return false;
            return bucketOperator.Remove(key);
        }

        #region private 

        /// <summary>
        /// 备份持久化的缓存
        /// </summary>
        /// <typeparam name="T">缓存的类型</typeparam>
        /// <param name="value">缓存对象</param>
        private static void BackUpPersistentCache<T>(T value)
        {
            //DOTO:永久缓存的东西，需要备份。
        }

        /// <summary>
        /// 获取Bucket操作者
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <returns>CouchBaseClusterManager对象</returns>
        private IBucketProvider GetBucketOperator(string bucketName)
        {
            IBucketProvider bucketOperator;
            if (_bucketOperators.ContainsKey(bucketName))
            {
                bucketOperator = _bucketOperators[bucketName];
            }
            else
            {
                lock (_lock)
                {
                    if (_bucketOperators.ContainsKey(bucketName))
                    {
                        return _bucketOperators[bucketName];
                    }
                    ReloadBucket();
                    bucketOperator = _bucketOperators[bucketName];
                }
            }
            if (bucketOperator == null)
            {
                _logger.Warn($"未获取到Bucket名为{bucketName}的操作者");
            }
            return bucketOperator;
        }

        /// <summary>
        /// 重新载入Bucket
        /// </summary>
        private void ReloadBucket()
        {
            foreach (var key in _bucketOperators.Keys)
            {
                _bucketOperators[key].Dispose();
            }
            _bucketOperators.Clear();

            ClusterHelper.Initialize(_couchbaseClientSection);
            var section = (CouchbaseClientSection) ConfigurationManager.GetSection(_couchbaseClientSection);
            var definition = (ICouchbaseClientDefinition) section;
            foreach (var bucketDef in definition.Buckets)
            {
                try
                {
                    var bucket = ClusterHelper.GetBucket(bucketDef.Name);
                    _bucketOperators.Add(bucketDef.Name, new BucketOperator(bucket));
                }
                catch (AggregateException ex)
                {
                    foreach (var innerException in ex.InnerExceptions)
                    {
                        _logger.Error($"ReloadBucket()异常,{innerException.Message}", ex);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"ReloadBucket()异常,{ex.Message}", ex);
                }
            }
        }
    }

    #endregion
}