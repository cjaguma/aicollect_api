using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AiCollect.Core
{
    public static class Validator
    {
        /// <summary>
        /// Initialises an array of sql reserved words.
        /// </summary>
        private static string[] _sqlReservedWords =  {"ADD", "EXCEPT", "PERCENT" ,"ALL", "EXEC", "PLAN", "ALTER", "EXECUTE", "PRECISION",
                "AND", "EXISTS", "PRIMARY", "ANY", "EXIT", "PRINT",
                "AS", "FETCH", "PROC", "ASC", "FILE", "PROCEDURE",
                "AUTHORIZATION", "FILLFACTOR", "PUBLIC",
                "BACKUP", "FOR", "RAISERROR", "BEGIN", "FOREIGN", "READ",
                "BETWEEN", "FREETEXT", "READTEXT", "BREAK", "FREETEXTTABLE",
                "RECONFIGURE", "BROWSE", "FROM", "REFERENCES",
                "BULK", "FULL", "REPLICATION", "BY", "FUNCTION", "RESTORE",
                "CASCADE", "GOTO", "RESTRICT", "CASE", "GRANT", "RETURN",
                "CHECK", "GROUP", "REVOKE", "CHECKPOINT", "HAVING", "RIGHT",
                "CLOSE", "HOLDLOCK", "ROLLBACK", "CLUSTERED", "IDENTITY",
                "ROWCOUNT", "COALESCE", "IDENTITY_INSERT", "ROWGUIDCOL",
                "COLLATE", "IDENTITYCOL", "RULE", "COLUMN", "IF", "SAVE",
                "COMMIT", "IN", "SCHEMA", "COMPUTE", "INDEX", "SELECT",
                "CONSTRAINT", "INNER", "SESSION_USER", "CONTAINS", "INSERT",
                "SET", "CONTAINSTABLE", "INTERSECT", "SETUSER",
                "CONTINUE", "INTO", "SHUTDOWN", "CONVERT", "IS", "SOME",
                "CREATE", "JOIN", "STATISTICS", "CROSS", "KEY", "SYSTEM_USER",
                "CURRENT", "KILL", "TABLE", "CURRENT_DATE", "LEFT", "TEXTSIZE",
                "CURRENT_TIME", "LIKE", "THEN", "CURRENT_TIMESTAMP",
                "LINENO", "TO", "CURRENT_USER", "LOAD", "TOP",
                "CURSOR", "NATIONAL", "TRAN", "DATABASE", "NOCHECK",
                "TRANSACTION", "DBCC", "NONCLUSTERED", "TRIGGER",
                "DEALLOCATE", "NOT", "TRUNCATE", "DECLARE", "NULL", "TSEQUAL",
                "DEFAULT", "NULLIF", "UNION", "DELETE", "OF", "UNIQUE",
                "DENY", "OFF", "UPDATE", "DESC", "OFFSETS", "UPDATETEXT",
                "DISK", "ON", "USE", "DISTINCT", "OPEN", "USER",
                "DISTRIBUTED", "OPENDATASOURCE", "VALUES",
                "DOUBLE", "OPENQUERY", "VARYING", "DROP", "OPENROWSET",
                "VIEW", "DUMMY", "OPENXML", "WAITFOR", "DUMP", "OPTION",
                "WHEN", "ELSE", "OR", "WHERE", "END", "ORDER", "WHILE",
                "ERRLVL", "OUTER", "WITH", "ESCAPE", "OVER", "WRITETEXT"};
        /// <summary>
        ///  Initialises an array of CSharp reserved words.
        /// </summary>
        private static string[] _CSharpReservedWords = { "class","abstract","as","int","break","char","Continue",
                                                           "do","event","finally","foreach","void","internal","namespace",
                                                           "in","operator","params","readonly","sealed","static","this","typeof",
                                                           "unsafe","byte","checked","decimal","double","explicit","fixed","goto","is",
                                                           "new","out","private","public","internal","ref","short","string","throw","uint",
                                                           "ushort","volatile","base","case","class","default","else","extern","float","if",
                                                           "lock","null","out","protected","return","sizeof","struct","true","false","using",
                                                           "for","bool","catch","const","delegate","enum","implicit","interface","long","object",
                                                           "override","sbyte","stackalloc","switch","try","unchecked","virtual"

                                                       };
        /// <summary>
        /// Returns a string of letters only.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string TranslateNameToSafeName(string name)
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
        /// Returns a string of letters only on generate.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string TranslateNameToSafeNameOnGenerate(string name)
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
                    else if (((int)a) > 127)
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
        /// Capitalizes the first letter for a Snippet.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string CapitalizeFirstLetterForASnippet(string name)
        {
            string newName = "";
            char firstLetter = name[0];

            if (char.IsLower(firstLetter))
            {
                char c = char.ToUpper(firstLetter);
                newName = name.Remove(0, 1);
                newName = TranslateNameToSafeName(newName.Insert(0, c.ToString()));
            }
            return newName;
        }
        /// <summary>
        /// Cleans the Sql and Xml single quote.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static string CleanSqlAndXmlSingleQuote(string description)
        {
            return description.Replace(@"'", "''");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static string DirtySqlAndXmlSingleQuote(string description)
        {
            return description.Replace(@"''", "'");
        }
        /// <summary>
        /// Returns a value indicating whether the supplied name is an sql reserved word.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsReservedName(string name)
        {
            if (_sqlReservedWords.Contains(name))
                return true;
            return false;
        }
        /// <summary>
        ///  Returns a value indicating whether the supplied name is a CSharp reserved word.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsCSharpReservedName(string name)
        {
            if (_CSharpReservedWords.Contains(name.ToLower()))
                return true;
            return false;
        }
    }
}
