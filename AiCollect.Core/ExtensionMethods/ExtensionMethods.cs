using System;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core
{
    public static class ExtensionMethods
    {
        public static int ToInt(this string str)
        {
            int i = 0;
            if (!int.TryParse(str, out i))
                i = 0;
            return i;
        }

        /// <summary>
        /// Makes a single qoute sql compliant
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CleanSqlAndXmlSingleQuote(this string str)
        {
            return str.Replace(@"'", "''");
        }

        /// <summary>
        /// Replaces the sql safe escaped single qoute
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DirtySqlAndXmlSingleQuote(this string str)
        {
            return str.Replace(@"''", "'");
        }

        /// <summary>
        /// Converts a string to an sql complaint string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToSafeName(this string name)
        {
            StringBuilder temp = new StringBuilder("");
            if (!string.IsNullOrWhiteSpace(name))
            {
                foreach (char a in name.ToCharArray())
                {
                    if (char.IsSymbol(a) == true)
                    {
                        temp = temp.Append('_');
                    }
                    else if (char.IsPunctuation(a) == true)
                    {
                        temp = temp.Append('_');
                    }
                    else if (char.Equals(a, '/'))
                    {
                        temp = temp.Append('_');
                    }
                    else if (char.Equals(a, '-'))
                    {
                        temp = temp.Append('_');
                    }
                    else if (char.IsWhiteSpace(a) == true)
                    {
                        temp = temp.Append('_');
                    }
                    else
                    {
                        temp = temp.Append(a);
                    }
                }
            }
            return temp.ToString();
        }

        /// <summary>
        /// Adds backticks to strings for use with MySql
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string AddBackTicks(this string str)
        {
            return string.Format("`{0}`", str);
        }

        /// <summary>
        /// Adds square brackets to string for use with SQL server
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string AddSquareBrackets(this string str)
        {
            return string.Format("[{0}]", str);
        }

        /// <summary>
        /// Converts a specified DataType to DataProvider equivalent string
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static string ToDbString(this DataTypes dataType, DataProviders provider)
        {
            StringBuilder sb = new StringBuilder();
            switch (provider)
            {
                case DataProviders.SQL:
                    switch (dataType)
                    {
                        case DataTypes.Alphanumeric:
                            sb.Append("VARCHAR");
                            break;
                        case DataTypes.Numeric:
                            sb.Append("INT");
                            break;
                    }
                    break;
            }
            return sb.ToString();
        }
       
     
    }
}
