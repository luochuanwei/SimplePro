using System;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;

namespace MT.LQQ.Utilitys.Helpers
{
    /// <summary>
    /// 通用操作
    /// </summary>
    public class CommonHelper
    {
        private static readonly Random random=new Random();

        /// <summary>
        /// 获取直播状态
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns>0:未开始，1:进行中，2：已经结束</returns>
        public  static int GetLiveStatus(DateTime time1, DateTime time2)
        {
            if (time1 < time2)
            {
                var temp = time1;
                time1 = time2;
                time2 = temp;
            }

            var currentTime = DateTime.Now;
            if (currentTime < time1)
            {
                return 0;
            }

            if (currentTime > time1 && currentTime < time2)
            {
                return 1;
            }

            if (currentTime > time2)
            {
                return 2;
            }
            return 0;
        }

        /// <summary>
        ///  添加HTML BodyBuf
        /// </summary>
        /// <param name="text"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string AddHtmlBody(string text, string title = "")
        {
            var builder = new StringBuilder("<!DOCTYPE html>");
            builder.Append("<html><head> <meta charset=\"utf-8\">");
            builder.Append("<meta name=\"viewport\" content=\"width=device-width,initial-scale=1, minimum-scale=1, maximum-scale=1,user-scalable=no\"/>");
            builder.Append($"<title>{title}</title>");
            builder.Append(" <style>img {width:100%} </style><body>");
            builder.Append(text);
            builder.Append("</body></html>");
            return builder.ToString();
        }

        /// <summary>
        /// 创建新ID
        /// </summary>
        /// <returns></returns>
        public static string NewId()
        {
            return $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{random.Next(1000000, 9999999)}{random.Next(10000000, 99999999)}";
        }

        /// <summary>
        /// 获取文件Host
        /// </summary>
        /// <returns></returns>
        public static string FileHost()
        {
            return GetAppSettings("filehost");
        }

        /// <summary>
        /// 获取H5的Host
        /// </summary>
        /// <returns></returns>
        public static string H5Host()
        {
            return GetAppSettings("h5host");
        }

        /// <summary>
        /// 组合URL
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public static string CombineUrl(string path, string host = "")
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            if (path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || path.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return path;
            }

            if (string.IsNullOrEmpty(host))
            {
                host = FileHost();
            }

            if (host.EndsWith("/"))
            {
                host = host.Remove(host.Length - 1, 1);
            }
            if (path.StartsWith("/"))
            {
                path = path.Remove(0, 1);
            }

            return $"{host}/{path}";
        }

        /// <summary>
        /// 组合H5的URL
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string CombineH5Url(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            if (path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || path.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return path;
            }
            var host = H5Host();
            if (string.IsNullOrEmpty(host))
            {
                return path;
            }

            if (host.EndsWith("/"))
            {
                host = host.Remove(host.Length - 1, 1);
            }
            if (path.StartsWith("/"))
            {
                path = path.Remove(0, 1);
            }

            return $"{host}/{path}";
        }

        /// <summary>
        /// 根据UTF-8编码获取字符串的字节长度(1个汉字为2字节)
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字符串的字节长度(1个汉字为2字节)，输入字符串IsNullOrEmpty，则返回0</returns>
        public static int GetLength(string str)
        {
            return ValidationHelper.IsNullOrEmpty(str) ? 0 : Encoding.UTF8.GetByteCount(str);
        }

        /// <summary>
        /// 从字符串的起始位置截取指定字节长度的字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="length">字节长度</param>
        /// <returns>指定字节长度的字符串</returns>
        public static string Substring(string str, int length)
        {
            var tempStr = Encoding.UTF8.GetBytes(str);
            return tempStr.Length > length ? Encoding.UTF8.GetString(tempStr, 0, length) : str;
        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">要返回的字符串的字节长度</param>
        /// <returns>指定字节长度的字符串</returns>
        public static string Substring(string str, int startIndex, int length)
        {
            var tempStr = Encoding.UTF8.GetBytes(str);

            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length*-1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }
                if (startIndex > tempStr.Length)
                {
                    return "";
                }
            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            if (tempStr.Length - startIndex < length)
            {
                length = tempStr.Length - startIndex;
            }
            return Encoding.UTF8.GetString(tempStr, startIndex, length);
        }

        /// <summary>
        /// 验证指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="searchStr">指定字符串</param>
        /// <param name="arrStr">指定字符串数组</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>		
        public static int GetIndexInArray(string searchStr, string[] arrStr)
        {
            return GetIndexInArray(searchStr, arrStr, true);
        }

        /// <summary>
        /// 验证指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="searchStr">指定字符串</param>
        /// <param name="arrStr">指定字符串数组</param>
        /// <param name="caseInsensetive">是否区分大小写, true为区分, false为不区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        public static int GetIndexInArray(string searchStr, string[] arrStr, bool caseInsensetive)
        {
            int retValue = -1;
            if (!string.IsNullOrEmpty(searchStr) && arrStr.Length > 0)
            {
                for (int i = 0; i < arrStr.Length; i++)
                {
                    if (caseInsensetive)
                    {
                        if (searchStr == arrStr[i])
                            retValue = i;
                    }
                    else
                    {
                        if (searchStr.ToLower() == arrStr[i].ToLower())
                            retValue = i;
                    }
                }
            }
            return retValue;
        }


        /// <summary>
        /// 分割字符串数组
        /// </summary>
        /// <param name="sourceStr">要分割字符串</param>
        /// <param name="splitStr">分割字符</param>
        /// <returns>分割后的字符串</returns>
        public static string[] Split(string sourceStr, string splitStr)
        {
            if (!string.IsNullOrEmpty(sourceStr))
            {
                if (sourceStr.IndexOf(splitStr, StringComparison.Ordinal) >= 0)
                    return Regex.Split(sourceStr, Regex.Escape(splitStr), RegexOptions.IgnoreCase);
                string[] tmp = {sourceStr};
                return tmp;
            }
            return new string[] {};
        }

        /// <summary>
        /// 分割字符串数组
        /// </summary>
        /// <param name="sourceStr">要分割字符串</param>
        /// <param name="splitStr">分割字符</param>
        /// <param name="count">将从要分割字符串中分割出的字符数组最大索引数</param>
        /// <returns></returns>
        public static string[] Split(string sourceStr, string splitStr, int count)
        {
            var result = new string[count];

            var splited = Split(sourceStr, splitStr);

            for (var i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// 获取APP设置的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        private static string GetAppSettings(string key)
        {
            try
            {
                var host = ConfigurationManager.AppSettings[key];
                return host;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
