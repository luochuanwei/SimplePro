namespace MT.LQQ.Models.Enum
{
    /// <summary>
    /// 阿里云OSS bucket 枚举（阿里云上的Bucket名称此处枚举转小写一致）
    /// </summary>
    public enum OssBucketEnum
    {
        /// <summary>
        /// 什么也没有
        /// </summary>
        None = 0,

        /// <summary>
        /// AMR语音存放 Bucket （私有）
        /// </summary>
        LuquAMR = 1,

        /// <summary>
        /// 通用 Bucket （公有读）
        /// </summary>
        LuquCommon = 2,

        /// <summary>
        /// MP3语音存放 Bucket （私有）
        /// </summary>
        LuquMP3 = 3,

        /// <summary>
        /// 用户 Bucket （公有读）
        /// </summary>
        LuquUser = 4,

        /// <summary>
        /// 公有语音存储Bucket （公有读）
        /// </summary>
        LuquVoicePublic = 5,

        /// <summary>
        /// 录趣语音Bucket
        /// </summary>
        LuquVoice = 6,

        /// <summary>
        /// 录趣院校Bucket（公有读）
        /// </summary>
        LuquUniversity = 7,
    }
}