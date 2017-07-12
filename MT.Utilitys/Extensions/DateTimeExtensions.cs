using System;

namespace MT.LQQ.Utilitys.Extensions
{
    /// <summary>
    /// 时间Helper
    /// </summary>
    public static class DateTimeExtensions
    {
        public static string ToAboutTimeString(this DateTime dateTime, int status)
        {
            var now = DateTime.Now;
            var min  = (dateTime - now).TotalMinutes;
            var hour = (dateTime - now).TotalHours;
            var day = (dateTime - now).TotalDays;
            if (status == 0)
            {
                return "直播中";
            }
            if(status == 2)
            {
                return "已结束";
            }
            if (min <= 60)
            {
                return "即将开始";
            }
            if (hour <= 24)
            {
                return Math.Ceiling(hour).ToString() + "小时后";
            }
            return Math.Ceiling(day).ToString() + "天后";
        }

        /// <summary>
        /// 转成自己的时间表示（刚刚、5分钟前等等）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToCustomString(this DateTime dateTime)
        {
            var now = DateTime.Now;
            if ((now - dateTime).TotalMinutes <= 5)
            {
                return "刚刚";
            }

            if ((now - dateTime).TotalMinutes <= 10)
            {
                return "5分钟前";
            }

            if ((now - dateTime).TotalMinutes <= 30)
            {
                return "10分钟前";
            }

            if ((now - dateTime).TotalHours <= 1)
            {
                return "半小时前";
            }

            if ((now - dateTime).TotalHours <= 2)
            {
                return "1小时前";
            }

            if ((now - dateTime).TotalDays <= 1)
            {
                return "今天";
            }

            if ((now - dateTime).TotalDays <= 2)
            {
                return "昨天";
            }

            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 转成自己的时间表示（刚刚、5分钟前等等）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToCustomString(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return string.Empty;
            }

            return dateTime.Value.ToCustomString();
        }

        /// <summary>
        /// 转成时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>时间戳（秒）</returns>
        public static long ToTimestamp(this DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime - start).TotalMilliseconds);
        }

        /// <summary>
        /// 转成时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>时间戳（秒）</returns>
        public static long ToUtcTimestamp(this DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((dateTime.ToUniversalTime() - start).TotalMilliseconds);
        }

        /// <summary>
        /// 转成当前时间
        /// </summary>
        /// <param name="timestamp">时间戳（秒）</param>
        /// <returns>当前时间</returns>
        public static DateTime UtcTimestampToDateTime(this long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return start.AddMilliseconds(timestamp).ToLocalTime();
        }

        /// <summary>
        /// 转成当前时间
        /// </summary>
        /// <param name="timestamp">时间戳（秒）</param>
        /// <param name="dateTime"></param>
        /// <returns>当前时间</returns>
        public static DateTime TimestampToDateTime(this DateTime dateTime, long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return start.AddMilliseconds(timestamp);
        }

        /// <summary>
        /// 获取年份字符串
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static Tuple<string, string> ToYearString(this DateTime dateTime)
        {
            var currentDateTime = DateTime.Now;
            if (DateTime.Compare(currentDateTime.Date, dateTime.Date) == 0)
            {
                return new Tuple<string, string>("今天", dateTime.ToString("HH:mm"));
            }

            if (DateTime.Compare(currentDateTime.AddDays(-1).Date, dateTime.Date) == 0)
            {
                return new Tuple<string, string>("昨天", dateTime.ToString("HH:mm"));
            }

            return new Tuple<string, string>(dateTime.Year.ToString(), dateTime.ToString("MM-dd"));
        }

        /// <summary>
        /// 获取指定日期的月初日期
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetFirstDateOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// 获取指定日期的月末日期
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetLastDateOfMonth(this DateTime dateTime)
        {
            return dateTime.GetFirstDateOfMonth().AddMonths(1).AddMilliseconds(-1);
        }
    }
}