using System.IO;

namespace vanko.Util
{
    public static class FileUtil
    {
        public static string GetFileMD5(string filePath)
        {
            var stream = new FileStream(filePath, FileMode.Open);
            var md5 = System.Security.Cryptography.MD5.Create();
            var hash = md5.ComputeHash(stream);
            stream.Close();
            return System.BitConverter.ToString(hash).Replace("-", "");
        }
    }
}