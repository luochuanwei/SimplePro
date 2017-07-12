using System;
using System.Security.Cryptography;
using System.Text;

namespace MT.LQQ.Utilitys.Helpers
{
    /// <summary>
    /// 加解密助手类
    /// </summary>
    public static class CryptogramHelper
    {
        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="source">待加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Md5(string source)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var data = Encoding.UTF8.GetBytes(source);
            var encs = md5.ComputeHash(data);
            return BitConverter.ToString(encs).Replace("-", "");
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="sourceStr">待加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string SHA256(string sourceStr)
        {
            var sha256Byte = Encoding.UTF8.GetBytes(sourceStr);
            var sha256 = new SHA256Managed();
            var result = sha256.ComputeHash(sha256Byte);
            sha256.Clear();
            return Convert.ToBase64String(result);
        }


        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="source">待编码的字符串</param>
        /// <param name="encoding">编码，可以为null</param>
        /// <returns>编码后的后的字符串</returns>
        public static string EncryptBase64(string source, Encoding encoding = null)
        {
            var bytes = encoding == null ? Encoding.UTF8.GetBytes(source) : encoding.GetBytes(source);
            return Convert.ToBase64String(bytes);
        }


        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="source">已编码的符串</param>
        /// <param name="encoding">编码，可以为null</param>
        /// <returns>解码后的字符串</returns>
        public static string DecryptBase64(string source, Encoding encoding)
        {
            var bytes = Convert.FromBase64String(source);
            return encoding == null ? Encoding.UTF8.GetString(bytes) : encoding.GetString(bytes);
        }


        /// <summary>
        /// Md5和Base64双重加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string MD5AndBase64(string source)
        {
            var pBytes = Encoding.UTF8.GetBytes(source);
            var md5 = new MD5CryptoServiceProvider();
            var oBytes = md5.ComputeHash(pBytes);
            return Convert.ToBase64String(oBytes);
        }


        /// <summary>
        /// 对字符串用指定密钥进行3DES加密，秘钥进行MD5加密
        /// </summary>
        /// <param name="source">要加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后并经base64编码的字符串</returns>
        public static string Encrypt3DES(string source, string key)
        {
            return Encrypt3DES(source, key, Encoding.UTF8);
        }

        /// <summary>
        /// 对字符串用指定密钥与指定编码方式加密
        /// </summary>
        /// <param name="source">要加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>加密后并经base64编码的字符串</returns>
        public static string Encrypt3DES(string source, string key, Encoding encoding)
        {

            var des = new TripleDESCryptoServiceProvider();
            var hashMd5 = new MD5CryptoServiceProvider();
            var keybytes = encoding.GetBytes(key);
            des.Key = hashMd5.ComputeHash(keybytes);

            des.Mode = CipherMode.ECB;
            var desEncrypt = des.CreateEncryptor();
            var buffer = encoding.GetBytes(source);
            var finalByte = desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length);
            return Convert.ToBase64String(finalByte);
        }

        /// <summary>
        /// 使用3DES算法解密
        /// </summary>
        /// <param name="source">待解密的字符串</param>
        /// <param name="key">秘钥</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt3DES(string source, string key)
        {
            return Decrypt3DES(source, key, Encoding.UTF8);
        }

        /// <summary>
        /// 使用3DES算法解密
        /// </summary>
        /// <param name="source">待解密的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt3DES(string source, string key, Encoding encoding)
        {
            //using (var tripleDES = new TripleDESCryptoServiceProvider())
            //{
            //    tripleDES.Key = encoding.GetBytes(key);
            //    var encrypted = Convert.FromBase64String(source);
            //    var  roundtrip = DecryptStringFromBytes(encrypted, tripleDES.Key, tripleDES.IV);
            //    return roundtrip;
            //}
            var des = new TripleDESCryptoServiceProvider();
            var hashMd5 = new MD5CryptoServiceProvider();
            des.Key = hashMd5.ComputeHash(encoding.GetBytes(key));
            des.Mode = CipherMode.ECB;
            var desDecrypt = des.CreateDecryptor();
            var result = "";
            try
            {
                byte[] buffer = Convert.FromBase64String(source);
                result = encoding.GetString(desDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 对字符串用指定密钥进行3DES加密，秘钥不进行MD5加密
        /// </summary>
        /// <param name="source">要加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后并经base64编码的字符串</returns>
        public static string Encrypt3DESWithTrueKey(string source, string key)
        {
            var keybytes = Encoding.UTF8.GetBytes(key);
            var tripleDES = new TripleDESCryptoServiceProvider
            {
                Key = keybytes,
                Mode = CipherMode.ECB,
            };
            var tripleDESEncrypt = tripleDES.CreateEncryptor();
            var buffer = Encoding.UTF8.GetBytes(source);
            var finalBytes = tripleDESEncrypt.TransformFinalBlock(buffer, 0, buffer.Length);
            return Convert.ToBase64String(finalBytes);
        }

        /// <summary>
        /// 对加密字符串用指定密钥进行3DES解密
        /// </summary>
        /// <param name="source">要解密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt3DESWithTrueKey(string source, string key)
        {
            var tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.CBC;
            tripleDES.Padding = PaddingMode.PKCS7;
            tripleDES.IV = Encoding.UTF8.GetBytes("12345678");
            var DESDecrypt = tripleDES.CreateDecryptor();
            var result = "";
            try
            {
                byte[] buffer = Convert.FromBase64String(source);
                result = Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch
            {
            }
            return result;
        }


        #region DES加解密

        /// <summary>
        /// 对字符串用指定密钥进行DES加密，秘钥不进行MD5加密
        /// </summary>
        /// <param name="source">要加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后并经base64编码的字符串</returns>
        public static string EncryptDESWithTrueKey(string source, string key)
        {
            var keybytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            var des = new DESCryptoServiceProvider
            {
                Key = keybytes,
                Mode = CipherMode.ECB
            };
            var desEncrypt = des.CreateEncryptor();
            var buffer = Encoding.UTF8.GetBytes(source);
            var finalBytes = desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length);
            return Convert.ToBase64String(finalBytes);
        }

        /// <summary>
        /// 对加密字符串用指定密钥进行3DES解密
        /// </summary>
        /// <param name="source">要解密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptDESWithTrueKey(string source, string key)
        {
            var des = new DESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(key.Substring(0, 8)),
                Mode = CipherMode.ECB
            };
            var decrypt = des.CreateDecryptor();
            string result;
            var buffer = Convert.FromBase64String(source);
            result = Encoding.UTF8.GetString(decrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            return result;
        }
        #endregion

    }
}
