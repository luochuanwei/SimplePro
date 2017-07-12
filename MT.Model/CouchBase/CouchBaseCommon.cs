using System;

namespace MT.LQQ.Models.CouchBase
{
    public class CouchBaseCommon
    {
        public static TimeSpan GetTimeSpan(TimeSpanLevelEnum level)
        {
#if DEBUG
            return TimeSpan.FromSeconds(1);
#endif
            switch (level)
            {
                case TimeSpanLevelEnum.Level1:
                    return TimeSpan.FromMinutes(1);
                case TimeSpanLevelEnum.Level2:
                    return TimeSpan.FromMinutes(5);
                case TimeSpanLevelEnum.Level3:
                    return TimeSpan.FromMinutes(30);
                case TimeSpanLevelEnum.Level4:
                    return TimeSpan.FromHours(1);
                case TimeSpanLevelEnum.Level5:
                    return TimeSpan.FromHours(12);
                case TimeSpanLevelEnum.Level6:
                    return TimeSpan.FromDays(1);
                case TimeSpanLevelEnum.Level7:
                    return TimeSpan.FromDays(7);
                case TimeSpanLevelEnum.Level8:
                    return TimeSpan.FromDays(30);
                default:
                    return TimeSpan.MaxValue;
            }
        }
    }
}
