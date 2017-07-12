using System.Text.RegularExpressions;

namespace MT.LQQ.Utilitys.Helpers
{
    /// <summary>
    /// 正则助手
    /// </summary>
    public class RegexHelper
    {
        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>        
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="options">筛选条件,比如是否忽略大小写</param>
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        /// <summary>
        /// 获取匹配的值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="resultPattern">结果模式字符串,范例："$1"用来获取第一个( )内的值</param>
        /// <param name="options">筛选条件,比如是否忽略大小写</param>
        public static string GetMatchValue(string input, string pattern, string resultPattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options) ? Regex.Match(input, pattern, options).Result(resultPattern) : string.Empty;
        }

        /// <summary>
        /// 获取匹配的值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="resultPattern">结果模式字符串,范例："$1"用来获取第一个( )内的值</param>
        public static string GetMatchValue(string input, string pattern, string resultPattern)
        {
            return GetMatchValue(input, pattern, resultPattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 获取匹配的值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>
        public static string GetMatchValue(string input, string pattern)
        {
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase) ? Regex.Match(input, pattern, RegexOptions.IgnoreCase).Value : string.Empty;
        }
    }
}