using System;
using System.IO;
using System.Text;

namespace MT.LQQ.Utilitys.Helpers
{
    public class FileHelper
    {
        /// <summary>
        /// 同步标识
        /// </summary>
        private static object sync = new object();

        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>        
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// 检测指定文件是否存在,如果存在则返回true。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 检测指定目录是否为空
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                //判断是否存在文件
                var fileNames = GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }

                //判断是否存在文件夹
                string[] directoryNames = GetDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {

                return true;
            }
        }


        /// <summary>
        /// 检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>        
        public static bool Contains(string directoryPath, string searchPattern)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, false);

                //判断指定文件是否存在
                return fileNames.Length != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 检测指定目录中是否存在指定的文件
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param> 
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, true);

                //判断指定文件是否存在
                return fileNames.Length != 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// 创建一个目录
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        public static void CreateDirectory(string directoryPath)
        {
            //如果目录不存在则创建该目录
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        /// <summary>
        /// 创建一个文件
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void CreateFile(string filePath)
        {
            try
            {
                if (IsExistFile(filePath)) return;
                var directoryPath = GetDirectoryFromFilePath(filePath);
                CreateDirectory(directoryPath);
                lock (sync)
                {                  
                    using (new FileStream(filePath, FileMode.OpenOrCreate))
                    {
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 创建一个文件,并将字节流写入文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="buffer">二进制流数据</param>
        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                if (IsExistFile(filePath)) return;
                var directoryPath = GetDirectoryFromFilePath(filePath);
                CreateDirectory(directoryPath);
                var file = new FileInfo(filePath);
                using (var fs = file.Create())
                {
                    fs.Write(buffer, 0, buffer.Length);
                }
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 创建一个文件,并将字符串写入文件。(默认utf-8)
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">字符串数据</param>
        public static void CreateFile(string filePath, string text)
        {
            CreateFile(filePath, text, Encoding.UTF8);
        }

        /// <summary>
        /// 创建一个文件,并将字符串写入文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">字符串数据</param>
        /// <param name="encoding">字符编码</param>
        public static void CreateFile(string filePath, string text, Encoding encoding)
        {
            try
            {
                //如果文件不存在则创建该文件
                if (IsExistFile(filePath)) return;
                //获取文件目录路径
                var directoryPath = GetDirectoryFromFilePath(filePath);

                //如果文件的目录不存在，则创建目录
                CreateDirectory(directoryPath);

                //创建文件
                var file = new FileInfo(filePath);
                using (var stream = file.Create())
                {
                    using (var writer = new StreamWriter(stream, encoding))
                    {
                        //写入字符串     
                        writer.Write(text);

                        //输出
                        writer.Flush();
                    }
                }
            }
            catch
            {
                // ignored
            }
        }


        /// <summary>
        /// 从文件绝对路径中获取目录路径
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static string GetDirectoryFromFilePath(string filePath)
        {
            var file = new FileInfo(filePath);
            var directory = file.Directory;
            return directory.FullName;
        }


        /// <summary>
        /// 获取文本文件的行数
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static int GetLineCount(string filePath)
        {
            //创建流读取器
            using (var reader = new StreamReader(filePath))
            {
                //行数
                var i = 0;

                while (true)
                {
                    //如果读取到内容就把行数加1
                    if (reader.ReadLine() != null)
                    {
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }

                //返回行数
                return i;
            }
        }

        /// <summary>
        /// 获取一个文件的长度,单位为Byte
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static int GetFileSize(string filePath)
        {
            var fi = new FileInfo(filePath);
            return (int) fi.Length;
        }

        /// <summary>
        /// 获取一个文件的长度,单位为KB
        /// </summary>
        /// <param name="filePath">文件的路径</param>        
        public static double GetFileSizeByKB(string filePath)
        {
            var fi = new FileInfo(filePath);
            return ConvertHelper.ToDouble(ConvertHelper.ToDouble(fi.Length)/1024, 1);
        }

        /// <summary>
        /// 获取一个文件的长度,单位为MB
        /// </summary>
        /// <param name="filePath">文件的路径</param>        
        public static double GetFileSizeByMB(string filePath)
        {
            var fi = new FileInfo(filePath);
            return ConvertHelper.ToDouble(ConvertHelper.ToDouble(fi.Length)/1024/1024, 1);
        }


        /// <summary>
        /// 获取指定目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetFileNames(string directoryPath)
        {
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }
            return Directory.GetFiles(directoryPath);
        }

        /// <summary>
        /// 获取指定目录及子目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            try
            {
                return Directory.GetFiles(directoryPath, searchPattern, isSearchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取指定目录及子目录中所有子目录列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                return Directory.GetDirectories(directoryPath, searchPattern, isSearchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 向文本文件中写入内容（默认utf-8）
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">写入的内容</param>        
        public static void WriteText(string filePath, string text)
        {
            WriteText(filePath, text, Encoding.UTF8);
        }

        /// <summary>
        /// 向文本文件中写入内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">写入的内容</param>
        /// <param name="encoding">编码</param>
        public static void WriteText(string filePath, string text, Encoding encoding)
        {
            File.WriteAllText(filePath, text, encoding);
        }


        /// <summary>
        /// 向文本文件的尾部追加内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">写入的内容</param>
        public static void AppendText(string filePath, string text)
        {
            try
            {
                lock (sync)
                {
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine(text);
                    }
                }
            }
            catch
            {
                // ignored
            }
        }


        /// <summary>
        /// 将源文件的内容复制到目标文件中
        /// </summary>
        /// <param name="sourceFilePath">源文件的绝对路径</param>
        /// <param name="destFilePath">目标文件的绝对路径</param>
        public static void CopyTo(string sourceFilePath, string destFilePath)
        {
            if (!IsExistFile(sourceFilePath))
            {
                return;
            }
            try
            {
                var destDirectoryPath = GetDirectoryFromFilePath(destFilePath);
                CreateDirectory(destDirectoryPath);
                var file = new FileInfo(sourceFilePath);
                file.CopyTo(destFilePath, true);
            }
            catch
            {
                // ignored
            }
        }



        /// <summary>
        /// 将文件移动到指定目录( 剪切 )
        /// </summary>
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>
        /// <param name="descDirectoryPath">移动到的目录的绝对路径</param>
        public static void MoveToDirectory(string sourceFilePath, string descDirectoryPath)
        {
            if (!IsExistFile(sourceFilePath))
            {
                return;
            }

            try
            {
                var sourceFileName = GetFileName(sourceFilePath);
                CreateDirectory(descDirectoryPath);
                if (IsExistFile(descDirectoryPath + "\\" + sourceFileName))
                {
                    DeleteFile(descDirectoryPath + "\\" + sourceFileName);
                }
                string descFilePath;
                if (!descDirectoryPath.EndsWith(@"\"))
                {
                    descFilePath = descDirectoryPath + "\\" + sourceFileName;
                }
                else
                {
                    descFilePath = descDirectoryPath + sourceFileName;
                }
                File.Move(sourceFilePath, descFilePath);
            }
            catch
            {
                // ignored
            }
        }



        /// <summary>
        /// 将文件移动到指定目录，并指定新的文件名( 剪切并改名 )
        /// </summary>
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>
        /// <param name="descFilePath">目标文件的绝对路径</param>
        public static void Move(string sourceFilePath, string descFilePath)
        {
            if (!IsExistFile(sourceFilePath))
            {
                return;
            }

            try
            {
                var descDirectoryPath = GetDirectoryFromFilePath(descFilePath);
                CreateDirectory(descDirectoryPath);
                File.Move(sourceFilePath, descFilePath);
            }
            catch
            {
                // ignored
            }
        }


        /// <summary>
        /// 将流读取到缓冲区中
        /// </summary>
        /// <param name="stream">原始流</param>
        public static byte[] StreamToBytes(Stream stream)
        {
            try
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, ConvertHelper.ToInt32(stream.Length));
                return buffer;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }


        /// <summary>
        /// 将文件读取到缓冲区中
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static byte[] FileToBytes(string filePath)
        {
            var fileSize = GetFileSize(filePath);
            var buffer = new byte[fileSize];
            var file = new FileInfo(filePath);
            using (var fs = file.Open(FileMode.Open))
            {
                fs.Read(buffer, 0, fileSize);
                return buffer;
            }
        }


        /// <summary>
        /// 将文件读取到字符串中(默认utf-8)
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static string FileToString(string filePath)
        {
            return FileToString(filePath, Encoding.UTF8);
        }

        /// <summary>
        /// 将文件读取到字符串中
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="encoding">字符编码</param>
        public static string FileToString(string filePath, Encoding encoding)
        {
            var reader = new StreamReader(filePath, encoding);
            try
            {
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// 从文件的绝对路径中获取文件名( 包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetFileName(string filePath)
        {
            var fi = new FileInfo(filePath);
            return fi.Name;
        }


        /// <summary>
        /// 从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetFileNameNoExtension(string filePath)
        {
            var fi = new FileInfo(filePath);
            return fi.Name.Split('.')[0];
        }


        /// <summary>
        /// 从文件的绝对路径中获取扩展名
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetExtension(string filePath)
        {
            var fi = new FileInfo(filePath);
            return fi.Extension;
        }


        /// <summary>
        /// 清空指定目录下所有文件及子目录,但该目录依然保存.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void ClearDirectory(string directoryPath)
        {
            if (!IsExistDirectory(directoryPath)) return;
            var fileNames = GetFileNames(directoryPath);
            foreach (var fileName in fileNames)
            {
                DeleteFile(fileName);
            }
            var directoryNames = GetDirectories(directoryPath);
            foreach (var directoryName in directoryNames)
            {
                DeleteDirectory(directoryName);
            }
        }


        /// <summary>
        /// 清空文件内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void ClearFile(string filePath)
        {
            File.Delete(filePath);
            CreateFile(filePath);
        }


        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void DeleteFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                File.Delete(filePath);
            }
        }


        /// <summary>
        /// 删除指定目录及其所有子目录
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

    }
}
