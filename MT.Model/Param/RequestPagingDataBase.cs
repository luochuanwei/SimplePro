namespace MT.LQQ.Models.Param
{
    /// <summary>
    /// 分页请求参数
    /// </summary>
    public class RequestPagingDataBase : RequestDataBase
    {
        /// <summary>
        /// 页码,页码从1开始。
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页面大小，需要大于0。
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Validate(out string message)
        {
            if (PageIndex <= 0)
            {
                PageIndex = 1;
            }

            if (PageSize <= 0)
            {
                PageSize = 10;
            }

            return base.Validate(out message);
        }
    }
}