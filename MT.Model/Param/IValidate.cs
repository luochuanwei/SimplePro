using MT.LQQ.Models.Enum;

namespace MT.LQQ.Models.Param
{
    /// <summary>
    ///请求参数参数验证接口
    /// </summary>
    public interface IValidate
    {
        /// <summary>
        /// 请求来源
        /// </summary>
        FromTypeEnum FromType { get; set; }

        /// <summary>
        /// 验证参数的有效性
        /// </summary>
        /// <param name="message"></param>
        /// <returns>True：验证成功，False：验证失败</returns>
        bool Validate(out string message);
    }
}