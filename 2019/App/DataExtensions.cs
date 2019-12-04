using System;
using System.Linq;

namespace App
{
    public static class DataExtensions
    {

        public static string[] Lines(this string data)
        {
            return data.Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] FromCommaSep(this string data)
        {
            return data.Split(',').Select(x => x.Trim()).ToArray();
        }
    }
}

