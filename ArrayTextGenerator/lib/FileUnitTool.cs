using System.IO;

namespace ArrayTextGenerator.lib
{
    internal class FileUnitTool
    {
        public static void MakeDir(string filePath)
        {
            if (!JudgeFileExist(filePath)) Directory.CreateDirectory(filePath);
        }

        public static bool JudgeFileExist(string filePath)
        {
            return File.Exists(filePath);
        }

        public static string GetRandomFileName(string filePath, string suffix)
        {
            var newFileName = StringUnitTool.GetRandomString(5) + '.' + suffix;
            var newFilePath = filePath                          + '/' + newFileName;
            while (JudgeFileExist(newFilePath))
            {
                newFileName = StringUnitTool.GetRandomString(5) + '.' + suffix;
                newFilePath = filePath                          + '/' + newFileName;
            }

            return newFileName;
        }
    }
}