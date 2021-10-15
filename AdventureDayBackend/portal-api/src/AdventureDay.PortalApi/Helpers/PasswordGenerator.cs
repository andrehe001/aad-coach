using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureDay.PortalApi.Helpers
{
    public class PasswordGenerator
    {

        private static string characterSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static string GetPassword()
        {
            var length = 9;
            var builder = new StringBuilder(length);
            var random = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < length; i++)
            {
                builder.Append(characterSet[random.Next(0, characterSet.Length - 1)]);
            }

            return builder.ToString();
        }
    }
}
