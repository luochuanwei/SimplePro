namespace MT.LQQ.Models.CouchBase
{
    /// <summary>
    /// 缓存过期时间级别枚举
    /// </summary>
    public enum TimeSpanLevelEnum
    {
        /// <summary>
        /// 1分钟过期
        /// </summary>
        Level1 = 1,
        /// <summary>
        /// 5分钟过期
        /// </summary>
        Level2 = 2,
        /// <summary>
        /// 30分钟过期
        /// </summary>
        Level3 = 3,
        /// <summary>
        /// 1小时过期
        /// </summary>
        Level4 = 4,
        /// <summary>
        /// 12小时过期
        /// </summary>
        Level5 = 5,
        /// <summary>
        /// 1天过期
        /// </summary>
        Level6 = 6,
        /// <summary>
        /// 7天过期
        /// </summary>
        Level7 = 7,
        /// <summary>
        /// 30天过期
        /// </summary>
        Level8 = 8
    }
}
