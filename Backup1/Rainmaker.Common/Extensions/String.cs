using System;
using System.Collections.Generic;
using System.Text;

namespace Rainmaker.Common.Extensions
{
    public static class StringExtensions
    {
        public static string prepSQL(this string _string)
        {
            return _string.Replace("'", "''");
        }
    }
}
