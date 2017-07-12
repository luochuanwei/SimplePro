using System;
using System.Collections;

namespace MT.LQQ.Utilitys.Helpers
{
    /// <summary>
    /// 枚举操作公共类
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 通过字符串获取枚举成员实例
        /// </summary>
        /// <typeparam name="T">枚举名,比如Enum1</typeparam>
        /// <param name="member">枚举成员的常量名或常量值,
        /// 范例:Enum1枚举有两个成员A=0,B=1,则传入"A"或"0"获取 Enum1.A 枚举类型</param>
        public static T GetInstance<T>(string member)
        {
            return (T) Enum.Parse(typeof(T), member, true);
        }

        /// <summary>
        /// 通过字符串获取枚举成员实例
        /// </summary>
        /// <typeparam name="T">枚举名,比如Enum1</typeparam>
        /// <param name="member">枚举成员的常量名或常量值,
        /// 范例:Enum1枚举有两个成员A=0,B=1,则传入"A"或"0"获取 Enum1.A 枚举类型</param>
        public static T GetInstance<T>(object member)
        {
            return GetInstance<T>(member.ToString());
        }

        /// <summary>
        /// 获取枚举成员名称和成员值的键值对集合
        /// </summary>
        /// <typeparam name="T">枚举名,比如Enum1</typeparam>
        public static Hashtable GetMemberKeyValue<T>()
        {
            var ht = new Hashtable();
            var memberNames = GetMemberNames<T>();
            foreach (var memberName in memberNames)
            {
                ht.Add(memberName, GetMemberValue<T>(memberName));
            }
            return ht;
        }

        /// <summary>
        /// 获取枚举所有成员名称
        /// </summary>
        /// <typeparam name="T">枚举名,比如Enum1</typeparam>
        public static string[] GetMemberNames<T>()
        {
            return Enum.GetNames(typeof(T));
        }

        /// <summary>
        /// 获取枚举成员的名称
        /// </summary>
        /// <typeparam name="T">枚举名,比如Enum1</typeparam>
        /// <param name="member">枚举成员实例或成员值,
        /// 范例:Enum1枚举有两个成员A=0,B=1,则传入Enum1.A或0,获取成员名称"A"</param>
        public static string GetMemberName<T>(object member)
        {
            var underlyingType = GetUnderlyingType(typeof(T));
            var memberValue = ConvertHelper.ConvertTo(member, underlyingType);
            return Enum.GetName(typeof(T), memberValue);
        }

        /// <summary>
        /// 获取枚举所有成员值
        /// </summary>
        /// <typeparam name="T">枚举名,比如Enum1</typeparam>
        public static Array GetMemberValues<T>()
        {
            return Enum.GetValues(typeof(T));
        }

        /// <summary>
        /// 获取枚举成员的值
        /// </summary>
        /// <typeparam name="T">枚举名,比如Enum1</typeparam>
        /// <param name="memberName">枚举成员的常量名,
        /// 范例:Enum1枚举有两个成员A=0,B=1,则传入"A"获取0</param>
        public static object GetMemberValue<T>(string memberName)
        {
            var underlyingType = GetUnderlyingType(typeof(T));
            var instance = GetInstance<T>(memberName);
            return ConvertHelper.ConvertTo(instance, underlyingType);
        }

        /// <summary>
        /// 获取枚举的基础类型
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        public static Type GetUnderlyingType(Type enumType)
        {
            return Enum.GetUnderlyingType(enumType);
        }



        /// <summary>
        /// 检测枚举是否包含指定成员
        /// </summary>
        /// <typeparam name="T">枚举名,比如Enum1</typeparam>
        /// <param name="member">枚举成员名或成员值</param>
        public static bool IsDefined<T>(string member)
        {
            return Enum.IsDefined(typeof(T), member);
        }

    }
}