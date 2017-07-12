using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MT.LQQ.Utilitys.Helpers
{
    public class ConvertHelper
    {
        /// <summary>
        /// 指定字符串的固定长度，如果字符串小于固定长度，
        /// 则在字符串的前面补足零，可设置的固定长度最大为9位
        /// </summary>
        /// <param name="text">原始字符串</param>
        /// <param name="limitedLength">字符串的固定长度</param>
        public static string RepairZero(string text, int limitedLength)
        {
            var temp = "";
            for (var i = 0; i < limitedLength - text.Length; i++)
            {
                temp += "0";
            }
            temp += text;
            return temp;
        }

        /// <summary>
        /// 实现各进制数间的转换。ConvertBase("15",10,16)表示将十进制数15转换为16进制的数。
        /// </summary>
        /// <param name="value">要转换的值,即原值</param>
        /// <param name="from">原值的进制,只能是2,8,10,16四个值。</param>
        /// <param name="to">要转换到的目标进制，只能是2,8,10,16四个值。</param>
        public static string ConvertBase(string value, int from, int to)
        {
            try
            {
                int intValue = Convert.ToInt32(value, from); //先转成10进制
                string result = Convert.ToString(intValue, to); //再转成目标进制
                if (to == 2)
                {
                    int resultLength = result.Length; //获取二进制的长度
                    switch (resultLength)
                    {
                        case 7:
                            result = "0" + result;
                            break;
                        case 6:
                            result = "00" + result;
                            break;
                        case 5:
                            result = "000" + result;
                            break;
                        case 4:
                            result = "0000" + result;
                            break;
                        case 3:
                            result = "00000" + result;
                            break;
                    }
                }
                return result;
            }
            catch
            {
                return "0";
            }
        }



        /// <summary>
        /// 将string转换成byte[]（默认为utf-8）
        /// </summary>
        /// <param name="text">要转换的字符串</param>        
        public static byte[] StringToBytes(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        /// <summary>
        /// 使用指定字符集将string转换成byte[]
        /// </summary>
        /// <param name="text">要转换的字符串</param>
        /// <param name="encoding">字符编码</param>
        public static byte[] StringToBytes(string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }


        /// <summary>
        /// 将byte[]转换成string（默认为utf-8）
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>        
        public static string BytesToString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 使用指定字符集将byte[]转换成string
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <param name="encoding">字符编码</param>
        public static string BytesToString(byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }


        /// <summary>
        /// 将byte[]转换成int
        /// </summary>
        /// <param name="data">需要转换成整数的byte数组</param>
        public static int BytesToInt32(byte[] data)
        {
            //如果传入的字节数组长度小于4,则返回0
            if (data.Length < 4)
            {
                return 0;
            }

            //定义要返回的整数
            int num = 0;

            //如果传入的字节数组长度大于4,需要进行处理
            if (data.Length >= 4)
            {
                //创建一个临时缓冲区
                byte[] tempBuffer = new byte[4];

                //将传入的字节数组的前4个字节复制到临时缓冲区
                Buffer.BlockCopy(data, 0, tempBuffer, 0, 4);

                //将临时缓冲区的值转换成整数，并赋给num
                num = BitConverter.ToInt32(tempBuffer, 0);
            }

            //返回整数
            return num;
        }



        /// <summary>
        /// 将对象转为byte数组
        /// </summary>
        /// <param name="obj">数据对象</param>
        /// <returns>byte数组</returns>
        public static byte[] ConvertToByte(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            using (var ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                var bytes = new Byte[ms.Length];
                ms.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }

        /// <summary>
        /// 将Byte[]转为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="bytes">byte数组</param>
        /// <returns></returns>
        public static T ConvertByteToObject<T>(byte[] bytes) where T : class
        {
            if (bytes == null)
            {
                return default(T);
            }
            using (var ms = new MemoryStream(bytes))
            {
                IFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(ms) as T;
            }
        }

        /// <summary>
        /// 将数据转换为日期,如果数据无效则返回"1900-1-1"
        /// </summary>
        /// <param name="date">日期</param>
        public static DateTime ToDateTime(object date)
        {
            try
            {
                return ValidationHelper.IsNullOrEmpty(date) ? Convert.ToDateTime("1900-1-1") : Convert.ToDateTime(date);
            }
            catch
            {
                return Convert.ToDateTime("1900-1-1");
            }
        }

        /// <summary>
        /// 将数据转换为GUID
        /// </summary>
        /// <param name="data"></param>
        public static Guid ToGuid(object data)
        {
            //有效性验证
            if (ValidationHelper.IsNullOrEmpty(data))
            {
                return Guid.Empty;
            }

            try
            {
                return new Guid(data.ToString());
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// 将数据转换为整型
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        public static int ToInt32<T>(T data)
        {
            try
            {
                return ValidationHelper.IsNullOrEmpty(data) ? 0 : Convert.ToInt32(data);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将数据转换为整型
        /// </summary>
        /// <param name="data">要转换的数据</param>
        public static int ToInt32(object data)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(data);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将数据转换为字符串
        /// </summary>
        /// <param name="data">数据</param>
        public static string ToString(object data)
        {
            //有效性验证
            if (data == null)
            {
                return string.Empty;
            }

            return data.ToString();
        }

        /// <summary>
        /// 将数据转换为布尔型
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        public static bool ToBoolean<T>(T data)
        {
            try
            {
                //如果为空则返回false
                return !ValidationHelper.IsNullOrEmpty(data) && Convert.ToBoolean(data);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 将数据转换为布尔型
        /// </summary>
        /// <param name="data">要转换的数据</param>
        public static bool ToBoolean(object data)
        {
            try
            {
                //如果为空则返回false
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return false;
                }
                else
                {
                    return Convert.ToBoolean(data);
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 将数据转换为单精度浮点型
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        public static float ToFloat<T>(T data)
        {
            try
            {
                //如果为空则返回0
                return ValidationHelper.IsNullOrEmpty(data) ? 0 : Convert.ToSingle(data);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将数据转换为单精度浮点型
        /// </summary>
        /// <param name="data">要转换的数据</param>
        public static float ToFloat(object data)
        {
            try
            {
                return ValidationHelper.IsNullOrEmpty<object>(data) ? 0 : Convert.ToSingle(data);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将数据转换为双精度浮点型
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        public static double ToDouble<T>(T data)
        {
            try
            {
                return ValidationHelper.IsNullOrEmpty(data) ? 0 : Convert.ToDouble(data);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将数据转换为双精度浮点型,并设置小数位
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        /// <param name="decimals">小数的位数</param>
        public static double ToDouble<T>(T data, int decimals)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return 0;
                }
                else
                {
                    var temp = Convert.ToDouble(data);
                    return Math.Round(temp, decimals);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将数据转换为双精度浮点型
        /// </summary>
        /// <param name="data">要转换的数据</param>
        public static double ToDouble(object data)
        {
            try
            {
                //如果为空则返回0
                return ValidationHelper.IsNullOrEmpty(data) ? 0 : Convert.ToDouble(data);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将数据转换为双精度浮点型,并设置小数位
        /// </summary>
        /// <param name="data">要转换的数据</param>
        /// <param name="decimals">小数的位数</param>
        public static double ToDouble(object data, int decimals)
        {
            try
            {
                if (ValidationHelper.IsNullOrEmpty<object>(data))
                {
                    return 0;
                }
                else
                {
                    var temp = Convert.ToDouble(data);
                    return Math.Round(temp, decimals);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将数据转换为双精度浮点型,并设置小数位和舍入方法
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        /// <param name="decimals">小数的位数</param>
        /// <param name="pointRound">小数舍入模式</param>
        public static double ToDouble<T>(T data, int decimals, MidpointRounding pointRound)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return 0;
                }
                else
                {
                    var temp = Convert.ToDouble(data);
                    return Math.Round(temp, decimals, pointRound);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将数据转换为Decimal类型
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        public static decimal ToDecimal<T>(T data)
        {
            try
            {
                //如果为空则返回0
                return ValidationHelper.IsNullOrEmpty(data) ? 0 : Convert.ToDecimal(data);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将数据转换为Decimal类型
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        /// <param name="decimals">小数的位数</param>
        public static decimal ToDecimal<T>(T data, int decimals)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return 0;
                }
                else
                {
                    var temp = Convert.ToDecimal(data);
                    return Math.Round(temp, decimals);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将数据转换为Decimal类型
        /// </summary>
        /// <param name="data">要转换的数据</param>
        public static decimal ToDecimal(object data)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty(data))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDecimal(data);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将数据转换为Decimal类型
        /// </summary>
        /// <param name="data">要转换的数据</param>
        /// <param name="decimals">小数的位数</param>
        public static decimal ToDecimal(object data, int decimals)
        {
            try
            {
                //如果为空则返回0
                if (ValidationHelper.IsNullOrEmpty<object>(data))
                {
                    return 0;
                }
                else
                {
                    decimal temp = Convert.ToDecimal(data);
                    return Math.Round(temp, decimals);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将数据转换为指定类型
        /// </summary>
        /// <param name="data">转换的数据</param>
        /// <param name="targetType">转换的目标类型</param>
        public static object ConvertTo(object data, Type targetType)
        {
            //如果数据为空，则返回
            if (ValidationHelper.IsNullOrEmpty(data))
            {
                return null;
            }

            try
            {
                //如果数据实现了IConvertible接口，则转换类型
                if (data is IConvertible)
                {
                    return Convert.ChangeType(data, targetType);
                }
                else
                {
                    return data;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将数据转换为指定类型
        /// </summary>
        /// <typeparam name="T">转换的目标类型</typeparam>
        /// <param name="data">转换的数据</param>
        public static T ConvertTo<T>(object data)
        {
            //如果数据为空，则返回
            if (ValidationHelper.IsNullOrEmpty(data))
            {
                return default(T);
            }

            try
            {
                //如果数据是T类型，则直接转换
                if (data is T)
                {
                    return (T) data;
                }

                //如果目标类型是枚举
                if (typeof(T).BaseType == typeof(Enum))
                {
                    return EnumHelper.GetInstance<T>(data);
                }

                //如果数据实现了IConvertible接口，则转换类型
                if (data is IConvertible)
                {
                    return (T) Convert.ChangeType(data, typeof(T));
                }
                else
                {
                    return default(T);
                }
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 将全角数字与字母转换为半角数字与字母
        /// </summary>
        /// <param name="str">输入字符串</param>
        /// <returns>转换后的半角数字与字母</returns>
        public static string ConvertToHalfStr(string str)
        {
            int index;
            string retStr = str;
            string fullStr = "１２３４５６７８９０ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ";
            string halfStr = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            if (!string.IsNullOrEmpty(str))
            {
                retStr = null;

                foreach (char temp in str)
                {
                    index = fullStr.IndexOf(temp);

                    if (index < 0)
                    {
                        retStr += temp.ToString();
                    }
                    else
                    {
                        retStr += halfStr[index].ToString();
                    }
                }
            }
            return retStr;
        }
    }
}