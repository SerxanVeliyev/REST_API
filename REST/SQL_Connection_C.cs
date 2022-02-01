using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace REST
{
    public class SQL_Connection_C
    {
        SqlConnection SQL_CON = new SqlConnection();
        SqlDataAdapter SQL_Adap = new SqlDataAdapter();
        SqlCommand SQL_Com = new SqlCommand();
        DataTable Table = new DataTable();

        public DataTable Select(SqlCommand com)
        {
            //"Server=135.125.178.166,1433;Database=PakXalcaWeb;User Id=sa;Password=Aa12345;"
            //"Server=PAKSRV;Database=PakXalcaWeb;User Id=sa;Password=Aa12345;"
            using (SQL_CON = new SqlConnection("Server=PAKSRV;Database=PakXalcaWeb;User Id=sa;Password=Aa12345;"))
            {
                SQL_Com = new SqlCommand();
                SQL_Com = com;
                SQL_Com.Connection = SQL_CON;
                SQL_Adap.SelectCommand = com;
                SQL_Adap.Fill(Table);
            }
            return Table;
        }
    }
}