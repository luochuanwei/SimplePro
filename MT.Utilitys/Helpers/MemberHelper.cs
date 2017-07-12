using System;

namespace MT.LQQ.Utilitys.Helpers
{
    /// <summary>
    /// 会员助手
    /// </summary>
    public static class MemberHelper
    {
        /// <summary>
        /// 检查是否为会员，并格式到期时间
        /// </summary>
        /// <param name="memberFlag">会员标记</param>
        /// <param name="endDate">会员到期时间</param>
        /// <returns>Item1:True：成功，False：失败。Item2：空，或者到期时间字符串,格式:2017-03-01 13:59:59</returns>
        public static Tuple<bool, DateTime?> IsMemberAndFormatData(bool memberFlag, DateTime? endDate)
        {
            if (!memberFlag || endDate == null)
            {
                return new Tuple<bool, DateTime?>(false, null);
            }

            if (endDate.Value < DateTime.Now)
            {
                return new Tuple<bool, DateTime?>(false, null);
            }

            return new Tuple<bool, DateTime?>(true, endDate.Value);
        }
    }
}