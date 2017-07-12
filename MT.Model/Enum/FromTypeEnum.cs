namespace MT.LQQ.Models.Enum
{
    /// <summary>
    /// 请求来源
    /// </summary>
    public enum FromTypeEnum
    {
        /// <summary>
        /// 缺省
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 苹果
        /// </summary>
        Ios = 1,

        /// <summary>
        /// 安卓
        /// </summary>
        Android = 2,

        /// <summary>
        /// Web
        /// </summary>
        Web = 3,

        /// <summary>
        /// H5
        /// </summary>
        H5 = 4,

        /// <summary>
        /// 微软手机系统
        /// </summary>
        WinPhone = 5,

        /// <summary>
        /// 管理端
        /// </summary>
        Admin = 6,

        /// <summary>
        /// 事件消费者
        /// </summary>
        Consumer = 7,

        /// <summary>
        /// 其他来源
        /// </summary>
        Others = 99
    }
}