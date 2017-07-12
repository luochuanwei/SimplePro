using MT.LQQ.Models.Enum;

namespace MT.LQQ.Models.Param
{
    /// <summary>
    /// 请求参数基类
    /// </summary>
    public class RequestDataBase : IValidate
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 请求来源
        /// </summary>
        public FromTypeEnum FromType { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual bool Validate(out string message)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                message = "UserId不能为空";
                return false;
            }

            message = string.Empty;
            return true;
        }
    }
}