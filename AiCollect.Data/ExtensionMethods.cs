
using AiCollect.Core;
using AiCollect.Core.Sync;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data
{
    public static class ExtensionMethods
    {
        public static byte[] ToByteArray(this System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] gh = ms.ToArray();
            return ms.ToArray();
        }

        public static Image ToImage(this byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        
        ///// <summary>
        ///// Get all attributes from a data form that may be displayed on the GUI
        ///// </summary>
        ///// <param name="form"></param>
        ///// <returns></returns>
        //public static IEnumerable<Attribute> GetDisplayableAttributes(this DataCollectionObject form)
        //{
        //    return form.Attributes.Where(att => att.ColumnName.NotIn("Deleted", "ChangeOperation"));
        //}

        /// <summary>
        /// Remove any opening and closing brackets from the string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveBrackets(this string input)
        {
            return input.Replace("[", "").Replace("]", "");
        }

        /// <summary>
        /// Add enclosing brackets to the given string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string AddEnclosingBrackets(this string input)
        {
            return string.Format("[{0}]", input);
        }

        /// <summary>
        /// Combines all given non-empty values to a concatenated AND-clause
        /// </summary>
        /// <param name="input"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string JoinAndClause(this string input, params string[] values)
        {
            IList<string> relevantValues = values.Where(value => !string.IsNullOrEmpty(value)).ToList();
            return string.Join(" AND ", relevantValues.ToArray());
        }


        /// <summary>
        /// Checks if a string is contained in an array of strings
        /// </summary>
        /// <param name="input"></param>
        /// <param name="exeptionValues"></param>
        /// <returns></returns>
        public static bool In<T>(this T input, params T[] exeptionValues)
        {
            return exeptionValues.Contains(input) == true;
        }

        /// <summary>
        /// Checks if a string is not contained in an array of strings
        /// </summary>
        /// <param name="input"></param>
        /// <param name="exeptionValues"></param>
        /// <returns></returns>
        public static bool NotIn<T>(this T input, params T[] exeptionValues)
        {
            return exeptionValues.Contains(input) == false;
        }


        internal static bool Exists(this dsoDataRow row, dloDataApplication app)
        {
            bool exists = false;

            DataTable dt = null;
            DbDataAdapter da = null;
            DataRow dr = null;

            string tableName = row.Table.TableName;
            string guid = (string)row["Guid"];

            DbCommand cmd = app.DbInfo.CreateSqlCommand();

            switch (app.DbInfo.Provider)
            {
                case DataProviders.SQL:
                    da = new SqlDataAdapter();
                    break;
                case DataProviders.MYSQL:
                    da = new MySqlDataAdapter();
                    break;
                case DataProviders.SQLCE:
                   // da = new SqlCeDataAdapter();
                    break;
            }

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = string.Format("SELECT * FROM {0} WHERE Guid = '{1}'", tableName, guid);

            dt = new DataTable(tableName);

            da.SelectCommand = cmd;
            da.Fill(dt);

            if (dt != null && dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
            }


            return exists;
        }


        public static DbType ToDbType(this DataTypes dataType, DataProviders provider)
        {
            DbType dbType = DbType.AnsiString;
            switch (provider)
            {
                case DataProviders.SQL:

                    break;
            }

            return dbType;
        }

        public static DbType ToDbType(this DataTypes dataType, DataProviders provider, ref DbParameter par)
        {
            DbType dbType = DbType.AnsiString;
            switch (dataType)
            {
                case DataTypes.Alphanumeric:
                case DataTypes.AlphanumericMasked:
                    switch (provider)
                    {
                        case DataProviders.SQL:
                            ((SqlParameter)par).SqlDbType = SqlDbType.NVarChar;
                            break;
                        case DataProviders.MYSQL:
                            ((MySqlParameter)par).MySqlDbType = MySqlDbType.VarChar;
                            break;
                        case DataProviders.SQLCE:
                            //((SqlCeParameter)par).SqlDbType = SqlDbType.NVarChar;
                            break;
                    }

                    break;

                case DataTypes.Date:
                    switch (provider)
                    {
                        case DataProviders.SQL:
                            ((SqlParameter)par).SqlDbType = SqlDbType.Date;
                            break;
                        case DataProviders.SQLCE:
                            //((SqlCeParameter)par).SqlDbType = SqlDbType.DateTime;
                            break;
                        case DataProviders.MYSQL:
                            ((MySqlParameter)par).MySqlDbType = MySqlDbType.Date;
                            break;
                    }
                    break;
                case DataTypes.Time:
                case DataTypes.DateTime:
                    switch (provider)
                    {
                        case DataProviders.SQL:
                            ((SqlParameter)par).SqlDbType = SqlDbType.DateTime;
                            break;
                        case DataProviders.SQLCE:
                            //((SqlCeParameter)par).SqlDbType = SqlDbType.DateTime;
                            break;
                        case DataProviders.MYSQL:
                            ((MySqlParameter)par).MySqlDbType = MySqlDbType.DateTime;
                            break;
                    }
                    break;

                case DataTypes.Autonumber:
                    switch (provider)
                    {
                        case DataProviders.SQL:
                            ((SqlParameter)par).SqlDbType = SqlDbType.Int;
                            break;
                        case DataProviders.SQLCE:
                            //((SqlCeParameter)par).SqlDbType = SqlDbType.Int;
                            break;
                        case DataProviders.MYSQL:
                            ((MySqlParameter)par).MySqlDbType = MySqlDbType.Int32;
                            break;
                    }
                    break;
                case DataTypes.List:
                    switch (provider)
                    {
                        case DataProviders.SQL:
                            ((SqlParameter)par).SqlDbType = SqlDbType.VarChar;
                            break;
                        case DataProviders.SQLCE:
                           // ((SqlCeParameter)par).SqlDbType = SqlDbType.NVarChar;
                            break;
                        case DataProviders.MYSQL:
                            ((MySqlParameter)par).MySqlDbType = MySqlDbType.VarChar;
                            break;
                    }
                    break;

                case DataTypes.Numeric:
                    switch (provider)
                    {
                        case DataProviders.SQL:
                            ((SqlParameter)par).SqlDbType = SqlDbType.Decimal;
                            //((SqlParameter)par).Precision = sa.Length;
                            //((SqlParameter)par).Scale = (byte)sa.Decimals;
                            break;
                        case DataProviders.SQLCE:
                           // ((SqlCeParameter)par).SqlDbType = SqlDbType.Decimal;
                            //((SqlCeParameter)par).Precision = (byte)sa.Length;
                            //((SqlCeParameter)par).Scale = (byte)sa.Decimals;
                            break;
                        case DataProviders.MYSQL:
                            ((MySqlParameter)par).MySqlDbType = MySqlDbType.Decimal;
                            //((MySqlParameter)par).Precision = (byte)sa.Length;
                            //((MySqlParameter)par).Scale = (byte)sa.Decimals;
                            break;
                    }
                    break;

                case DataTypes.YesNo:
                    switch (provider)
                    {
                        case DataProviders.SQL:
                            ((SqlParameter)par).SqlDbType = SqlDbType.Bit;
                            break;
                    }
                    break;

                case DataTypes.Calculated:
                    switch (provider)
                    {
                        case DataProviders.SQL:
                            ((SqlParameter)par).SqlDbType = SqlDbType.Int;
                            break;
                        case DataProviders.SQLCE:
                            //((SqlCeParameter)par).SqlDbType = SqlDbType.Int;
                            break;
                        case DataProviders.MYSQL:
                            ((MySqlParameter)par).MySqlDbType = MySqlDbType.Int32;
                            break;
                    }
                    break;

                case DataTypes.Memo:
                    switch (provider)
                    {
                        case DataProviders.SQL:
                            ((SqlParameter)par).SqlDbType = SqlDbType.VarChar;
                            break;
                        case DataProviders.SQLCE:
                            //((SqlCeParameter)par).SqlDbType = SqlDbType.NVarChar;
                            break;
                        case DataProviders.MYSQL:
                            ((MySqlParameter)par).MySqlDbType = MySqlDbType.Text;
                            break;
                    }
                    break;
                case DataTypes.Binary:
                    switch (provider)
                    {
                        case DataProviders.SQL:
                            ((SqlParameter)par).SqlDbType = SqlDbType.VarBinary;
                            break;
                        case DataProviders.SQLCE:
                            //((SqlCeParameter)par).SqlDbType = SqlDbType.VarBinary;
                            break;
                        case DataProviders.MYSQL:
                            ((MySqlParameter)par).MySqlDbType = MySqlDbType.VarBinary;
                            break;
                    }
                    break;

                case DataTypes.Image:
                    switch (provider)
                    {
                        case DataProviders.SQL:
                            ((SqlParameter)par).SqlDbType = SqlDbType.Image;
                            par.Direction = ParameterDirection.Input;
                            break;
                        case DataProviders.SQLCE:
                            //((SqlCeParameter)par).SqlDbType = SqlDbType.Image;
                            par.Direction = ParameterDirection.Input;
                            break;
                        case DataProviders.MYSQL:
                            ((MySqlParameter)par).MySqlDbType = MySqlDbType.Blob;
                            par.Direction = ParameterDirection.Input;
                            break;
                    }
                    break;

                case DataTypes.Audio:
                case DataTypes.Video:
                    switch (provider)
                    {
                        case DataProviders.SQL:
                            ((SqlParameter)par).SqlDbType = SqlDbType.VarBinary;
                            break;
                        case DataProviders.SQLCE:
                            //((SqlCeParameter)par).SqlDbType = SqlDbType.VarBinary;
                            break;
                        case DataProviders.MYSQL:
                            ((MySqlParameter)par).MySqlDbType = MySqlDbType.VarBinary;
                            break;
                    }
                    break;

                case DataTypes.UniqueIdentifier:
                    switch (provider)
                    {
                        case DataProviders.SQL:
                            ((SqlParameter)par).SqlDbType = SqlDbType.UniqueIdentifier;
                            break;
                        case DataProviders.SQLCE:
                            //((SqlCeParameter)par).SqlDbType = SqlDbType.UniqueIdentifier;
                            break;
                        case DataProviders.MYSQL:
                            ((MySqlParameter)par).MySqlDbType = MySqlDbType.VarChar;
                            break;

                    }
                    break;
                default:
                    throw new Exception("Unsupported DataType");
            }

            return dbType;
        }

       

       

        public static DataTable Flip(this DataTable table)
        {
            DataTable t = new DataTable();
            for (int i = 0; i <= 1; i++)
            {
                t.Columns.Add(Convert.ToString(i));
            }

            DataRow r;
            for (int k = 0; k < table.Rows[0].Table.Columns.Count; k++)
            {
                string colName = table.Rows[0].Table.Columns[k].ToString();
                if (!colName.ToLower().Equals("Guid") && !colName.ToLower().Equals("Deleted") && !colName.ToLower().Equals("Created_On") && !colName.ToLower().Equals("Created_By") && !colName.ToLower().Equals("Updated_On") && !colName.ToLower().Equals("Updated_By") && !colName.ToLower().Equals("ChangeOperation"))
                {
                    r = t.NewRow();
                    r[0] = colName;
                    r[1] = table.Rows[0].ItemArray[k];

                    t.Rows.Add(r);
                }
            }
            return t;
        }

        public static DataTable Flip(this DataRow datarow)
        {
            DataTable t = new DataTable();
            for (int i = 0; i <= 1; i++)
            {
                t.Columns.Add(Convert.ToString(i));
            }

            DataRow r;
            for (int k = 0; k < datarow.Table.Columns.Count; k++)
            {
                string colName = datarow.Table.Columns[k].ToString();
                if (!colName.ToLower().Equals("Guid") && !colName.ToLower().Equals("Deleted") && !colName.ToLower().Equals("Created_On") && !colName.ToLower().Equals("Created_By") && !colName.ToLower().Equals("Updated_On") && !colName.ToLower().Equals("Updated_By") && !colName.ToLower().Equals("ChangeOperation"))
                {
                    r = t.NewRow();
                    r[0] = colName;
                    r[1] = datarow.ItemArray[k];

                    t.Rows.Add(r);
                }
            }
            return t;
        }

        /// <summary>
        /// Crops an image circularly
        /// </summary>
        /// <param name="source"></param>
        /// <param name="circleUpperLeftX"></param>
        /// <param name="circleUpperLeftY"></param>
        /// <param name="circleDiameter"></param>
        /// <returns></returns>
        public static Image CropToCircle(this Bitmap source, int circleUpperLeftX, int circleUpperLeftY, int circleDiameter)
        {
            Bitmap final = null;
            Rectangle CropRect = new Rectangle(circleUpperLeftX, circleUpperLeftY, circleDiameter, circleDiameter);
            using (Bitmap CroppedImage = source.Clone(CropRect, source.PixelFormat))
            {
                using(TextureBrush tb=new TextureBrush(CroppedImage))
                {
                    final = new Bitmap(circleDiameter, circleDiameter);
                    using(var g =Graphics.FromImage(final))
                    {
                        g.FillEllipse(tb, 0, 0, circleDiameter, circleDiameter);
                        return final;
                    }
                }
            }
        }
    }
}
