using System;
using System.Text;

namespace ArrayTextGenerator.lib
{
    internal class StringUnitTool
    {
        private const string CharacterList = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string GetRandomString(int length)
        {
            StringBuilder stringBuilder = new();
            Random        random        = new();
            while (stringBuilder.Length < length)
            {
                stringBuilder.Append(CharacterList[random.Next(0, CharacterList.Length)]);
            }

            return stringBuilder.ToString();
        }
    }
}