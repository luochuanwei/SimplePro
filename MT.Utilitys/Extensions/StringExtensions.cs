using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MT.LQQ.Utilitys.Extensions
{
    /// <summary>
    /// String扩展方法
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 替换所有
        /// </summary>
        /// <param name="original"></param>
        /// <param name="replacements"></param>
        /// <returns></returns>
        public static string ReplaceAll(this string original, IDictionary<string, string> replacements)
        {
            var pattern = $"{string.Join("|", replacements.Keys)}";
            return Regex.Replace(original, pattern, match => replacements[match.Value]);
        }

        /// <summary>
        /// 转成Base64字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToBase64(this string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 从Base64位字符串转成标准字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FromBase64(this string value)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }

        /// <summary>
        /// 判断字符是否为字母
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsLetter(this char c)
        {
            return ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z');
        }

        /// <summary>
        /// 判断字符是否为空格
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsSpace(this char c)
        {
            return (c == '\r' || c == '\n' || c == '\t' || c == '\f' || c == ' ');
        }

        /// <summary>
        /// 替换换行符
        /// </summary>
        /// <param name="text"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string ReplaceNewLinesWith(this string text, string replacement)
        {
            return String.IsNullOrWhiteSpace(text)
                ? String.Empty
                : text
                    .Replace("\r\n", "\r\r")
                    .Replace("\n", String.Format(replacement, "\r\n"))
                    .Replace("\r\r", String.Format(replacement, "\r\n"));
        }

        /// <summary>
        /// 移除HTML标签
        /// </summary>
        /// <param name="html"></param>
        /// <param name="htmlDecode">是否HTML解码</param>
        /// <returns></returns>
        public static string RemoveTags(this string html, bool htmlDecode = false)
        {
            if (String.IsNullOrEmpty(html))
            {
                return String.Empty;
            }
            var result = new char[html.Length];
            var cursor = 0;
            var inside = false;
            for (var i = 0; i < html.Length; i++)
            {
                char current = html[i];
                switch (current)
                {
                    case '<':
                        inside = true;
                        continue;
                    case '>':
                        inside = false;
                        continue;
                }
                if (!inside)
                {
                    result[cursor++] = current;
                }
            }
            var stringResult = new string(result, 0, cursor);

            if (htmlDecode)
            {
                stringResult = HttpUtility.HtmlDecode(stringResult);
            }
            return stringResult;
        }

        /// <summary>
        /// 移除HTML标签
        /// </summary>
        /// <param name="obj"></param>
        public static void RemoveTags(this object obj)
        {
            if (obj == null)
                return;

            var type = obj.GetType();
            if (!type.IsClass)
                return;
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.PropertyType != typeof(string))
                    continue;
                if (!property.CanRead || !property.CanWrite)
                    continue;
                var currentValue = property.GetValue(obj) == null ? string.Empty : property.GetValue(obj).ToString();

                var html = Regex.Replace(currentValue, "<.+?>", " ");
                html = Regex.Replace(html, " <br> ", " ", RegexOptions.IgnoreCase);
                property.SetValue(obj, html);
            }
        }

        public static void RemoveHtmlBrackets(this object obj)
        {
            if (obj == null)
                return;

            var type = obj.GetType();
            if (!type.IsClass)
                return;
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.PropertyType != typeof(string))
                    continue;
                if (!property.CanRead || !property.CanWrite)
                    continue;
                var currentValue = property.GetValue(obj) == null ? string.Empty : property.GetValue(obj).ToString();

                var html = RemoveBrackets(currentValue);
                property.SetValue(obj, html);
            }
        }

        private static string RemoveBrackets(string html)
        {
            var result = html;
            var reg = new Regex("<.+?>");
            var matches = reg.Matches(result);
            foreach (Match matche in matches)
            {
                if (!matche.Success)
                    continue;
                var matcheValue = matche.Value;
                result = result.Replace(matcheValue, matcheValue.Replace("<", " ").Replace(">", " "));
            }

            return result;
        }

        /// <summary>
        /// 加省略号
        /// </summary>
        /// <param name="text"></param>
        /// <param name="characterCount">保留的字符数量</param>
        /// <returns></returns>
        public static string Ellipsize(this string text, int characterCount)
        {
            return text.Ellipsize(characterCount, "&#160;&#8230;");
        }

        /// <summary>
        /// 省略指定字符串数后面的字符串。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="characterCount"></param>
        /// <param name="ellipsis"></param>
        /// <param name="wordBoundary"></param>
        /// <returns></returns>
        public static string Ellipsize(this string text, int characterCount, string ellipsis, bool wordBoundary = false)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            if (characterCount < 0 || text.Length <= characterCount)
                return text;

            // search beginning of word
            var backup = characterCount;
            while (characterCount > 0 && text[characterCount - 1].IsLetter())
            {
                characterCount--;
            }

            // search previous word
            while (characterCount > 0 && text[characterCount - 1].IsSpace())
            {
                characterCount--;
            }

            // if it was the last word, recover it, unless boundary is requested
            if (characterCount == 0 && !wordBoundary)
            {
                characterCount = backup;
            }

            var trimmed = text.Substring(0, characterCount);
            return trimmed + ellipsis;
        }

        /// <summary>
        ///  剥去指定的字符
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="stripped">指定的字符数组</param>
        /// <returns></returns>
        public static string Strip(this string subject, params char[] stripped)
        {
            if (stripped == null || stripped.Length == 0 || String.IsNullOrEmpty(subject))
            {
                return subject;
            }

            var result = new char[subject.Length];

            var cursor = 0;
            for (var i = 0; i < subject.Length; i++)
            {
                char current = subject[i];
                if (Array.IndexOf(stripped, current) < 0)
                {
                    result[cursor++] = current;
                }
            }

            return new string(result, 0, cursor);
        }

        /// <summary>
        /// 剥去指定的字符
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="predicate">判断是否剥去的条件</param>
        /// <returns></returns>
        public static string Strip(this string subject, Func<char, bool> predicate)
        {

            var result = new char[subject.Length];

            var cursor = 0;
            for (var i = 0; i < subject.Length; i++)
            {
                char current = subject[i];
                if (!predicate(current))
                {
                    result[cursor++] = current;
                }
            }

            return new string(result, 0, cursor);
        }

        /// <summary>
        /// 指定的字符（字符数组中的任意一个字符）是否存在字符串中
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static bool Any(this string subject, params char[] chars)
        {
            if (string.IsNullOrEmpty(subject) || chars == null || chars.Length == 0)
            {
                return false;
            }

            return subject.Any(current => Array.IndexOf(chars, current) >= 0);
        }

        /// <summary>
        /// 指定的字符是否全部存在字符串中
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="chars">指定的字符数组</param>
        /// <returns></returns>
        public static bool All(this string subject, params char[] chars)
        {
            if (string.IsNullOrEmpty(subject))
            {
                return true;
            }

            if (chars == null || chars.Length == 0)
            {
                return false;
            }

            return subject.All(current => Array.IndexOf(chars, current) >= 0);
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string Translate(this string subject, char[] from, char[] to)
        {
            if (string.IsNullOrEmpty(subject))
            {
                return subject;
            }

            if (from == null || to == null)
            {
                throw new ArgumentNullException();
            }

            if (from.Length != to.Length)
            {
                throw new ArgumentNullException("from", "Parameters must have the same length");
            }

            var map = new Dictionary<char, char>(from.Length);
            for (var i = 0; i < from.Length; i++)
            {
                map[from[i]] = to[i];
            }

            var result = new char[subject.Length];

            for (var i = 0; i < subject.Length; i++)
            {
                var current = subject[i];
                if (map.ContainsKey(current))
                {
                    result[i] = map[current];
                }
                else
                {
                    result[i] = current;
                }
            }

            return new string(result);
        }

        public static string ToHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static byte[] ToByteArray(this string hex)
        {
            return Enumerable.Range(0, hex.Length).
                Where(x => 0 == x % 2).
                Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).
                ToArray();
        }
    }
}