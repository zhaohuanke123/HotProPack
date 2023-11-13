using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace vanko.Util
{
    public class FileUtil
    {
        // public static string GetFileMD5(string filePath)
        // {
        //     if (!File.Exists(filePath))
        //     {
        //         return "";
        //     }
        //     var stream = new FileStream(filePath, FileMode.Open);
        //     var md5 = System.Security.Cryptography.MD5.Create();
        //     var hash = md5.ComputeHash(stream);
        //     stream.Close();
        //     return System.BitConverter.ToString(hash).Replace("-", "").ToLower();
        // }

        public static Task<string> CalculateMd5ForFileAsync(string filePath)
        {
            return new Task<string>(() => ComputeMd5HashForFile(filePath));
        }

        public static string ComputeMd5HashForFile(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }
    }
}