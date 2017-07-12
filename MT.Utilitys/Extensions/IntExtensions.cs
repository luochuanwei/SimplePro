namespace MT.LQQ.Utilitys.Extensions
{

    /// <summary>
    /// Int 扩展
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// 转成自定义字符串表示
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToCustomString(this int number)
        {
            return number < 10000 ? number.ToString() : $"{number / 10000:F1}万";
        }

        /// <summary>
        /// 微信性别转字符
        /// </summary>
        /// <param name="sex">性别数字</param>
        /// <returns>字符表示</returns>
        public static string WeChatSexToString(this int sex)
        {
            switch (sex)
            {
                case 0:
                    return "未知";
                case 1:
                    return "男";
                case 2:
                    return "女";
                default:
                    return "未知";
            }
        }
    }
}