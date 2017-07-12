using MT.LQQ.Models.Enum;

namespace MT.LQQ.Models.Param
{
    /// <summary>
    /// 回应数据
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    public class ResponseData<T>
    {
        /// <summary>
        /// 结果类型
        /// </summary>
        public ResultTypeEnum ResultType { get; set; }

        /// <summary>
        /// 结果值
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// 简短描述信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="message">描述信息</param>
        /// <param name="value">返回值</param>
        /// <returns></returns>
        public static ResponseData<T> Failed(string message = "Failed", T value = default(T))
        {
            return new ResponseData<T>
            {
                ResultType = ResultTypeEnum.Failed,
                Message = message,
                Value = value
            };
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="message">描述信息</param>
        /// <param name="value">返回值</param>
        /// <returns></returns>
        public static ResponseData<T> Success(string message = "Success", T value = default(T))
        {
            return new ResponseData<T>
            {
                ResultType = ResultTypeEnum.Success,
                Message = message,
                Value = value
            };
        }

        /// <summary>
        /// 异常 or 错误
        /// </summary>
        /// <param name="message">描述信息</param>
        /// <param name="value">返回值</param>
        /// <returns></returns>
        public static ResponseData<T> Error(string message = "Error", T value = default(T))
        {
            return new ResponseData<T>
            {
                ResultType = ResultTypeEnum.Error,
                Message = message,
                Value = value
            };
        }

        /// <summary>
        /// 结果
        /// </summary>
        /// <param name="resultType">返回类型 <see cref="ResultTypeEnum"/></param>
        /// <param name="message">描述信息</param>
        /// <param name="value">返回值</param>
        /// <returns></returns>
        public static ResponseData<T> Result(ResultTypeEnum resultType, string message = "", T value = default(T))
        {
            return new ResponseData<T>
            {
                ResultType = resultType,
                Message = message,
                Value = value
            };
        }
    }
}