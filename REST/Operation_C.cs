using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace REST
{
    public class Operation_C
    {
        SqlCommand SQL_Com = new SqlCommand();
        SQL_Connection_C SQL_Connect = new SQL_Connection_C();
        Injection_C injection = new Injection_C();
        JESON_C Jeson = new JESON_C();

        public string Operation(int page, string _operator, string expeditor, string name, DateTime startdate, DateTime enddate)
        {
            page = Pagenation_number(page);
            _operator = injection.injection_string(_operator);
            expeditor = injection.injection_string(expeditor);
            name = injection.injection_string(name);
            #region
            /*
             WITH OPERATION (ID,IL,AY,GUN,CREATEDDATE,OPERATORNAME,EXPEDİTORNAME,NAME,CONFIRM) AS(
                                        SELECT ORD.Id,
	                                        YEAR(ORD.CREATEDDATE) AS [İL] ,
	                                        MONTH(ORD.CREATEDDATE) AS [AY] ,
	                                        DAY(ORD.CREATEDDATE) AS [GÜN] ,
	                                        ORD.CREATEDDATE  ,  
	                                        USR.FULLNAME OPERATORNAME,
	                                        USRDRV.FULLNAME EXPEDİTORNAME,
	                                        İTM.NAME,
	                                        (CASE 
		                                        WHEN ORD.OPERATİONSTATUS>=1 THEN 1 
		                                        ELSE 0 
	                                        END) AS CONFIRM 
                                        FROM 
	                                        ORDERS ORD 
	                                        INNER JOIN USERS USR ON USR.ID=ORD.CREATEDBY 
	                                        INNER JOIN DRİVERS DRV ON DRV.ID=ORD.DRİVERID
	                                        INNER JOIN USERS USRDRV ON USRDRV.ID=DRV.USERID
	                                        INNER JOIN ITEMS İTM ON İTM.ID=ORD.ITEMID
                                        ORDER BY ORD.CREATEDDATE DESC
                                        OFFSET 0 ROW
                                        )
                                        SELECT TOP(10000) ID AS [id],IL AS [year],AY AS [month],GUN AS [day],CONVERT(nvarchar, CREATEDDATE, 20) AS [createDate],OPERATORNAME AS [operatorName] ,EXPEDİTORNAME AS [expeditorName],NAME AS [name],CONFIRM AS [confirm] FROM OPERATION
										WHERE OPERATORNAME LIKE '%%' AND EXPEDİTORNAME LIKE '%%'  AND NAME LIKE '%%' AND  CREATEDDATE BETWEEN '2022-01-01'  AND '2022-01-02'*/
            #endregion
            SQL_Com = new SqlCommand(@"WITH OPERATION (ID,IL,AY,GUN,CREATEDDATE,OPERATORNAME,EXPEDİTORNAME,NAME,CONFIRM) AS(
                                        SELECT ORD.Id,
	                                        YEAR(ORD.CREATEDDATE) AS [İL] ,
	                                        MONTH(ORD.CREATEDDATE) AS [AY] ,
	                                        DAY(ORD.CREATEDDATE) AS [GÜN] ,
	                                        ORD.CREATEDDATE  ,  
	                                        USR.FULLNAME OPERATORNAME,
	                                        USRDRV.FULLNAME EXPEDİTORNAME,
	                                        İTM.NAME,
	                                        (CASE 
		                                        WHEN ORD.OPERATİONSTATUS>=1 THEN 1 
		                                        ELSE 0 
	                                        END) AS CONFIRM 
                                        FROM 
	                                        ORDERS ORD 
	                                        INNER JOIN USERS USR ON USR.ID=ORD.CREATEDBY 
	                                        INNER JOIN DRİVERS DRV ON DRV.ID=ORD.DRİVERID
	                                        INNER JOIN USERS USRDRV ON USRDRV.ID=DRV.USERID
	                                        INNER JOIN ITEMS İTM ON İTM.ID=ORD.ITEMID
                                        ORDER BY ORD.CREATEDDATE DESC
                                        OFFSET @PAGINATIONID ROW
                                        )
                                        SELECT TOP(100) ID AS [id],IL AS [year],AY AS [month],GUN AS [day],CONVERT(nvarchar, CREATEDDATE, 20) AS [createDate],OPERATORNAME AS [operatorName] ,EXPEDİTORNAME AS [expeditorName],NAME AS [name],CONFIRM AS [confirm] FROM OPERATION
										WHERE OPERATORNAME LIKE '%" + _operator + "%' AND EXPEDİTORNAME LIKE '%" + expeditor + "%'  AND NAME LIKE '%" + name + "%' AND  CREATEDDATE BETWEEN '" + startdate + "'  AND '" + enddate + "'");

            SQL_Com.Parameters.Add("@PAGINATIONID", SqlDbType.Int).Value = page;

            return Jeson.ConvertDataTabletoString(SQL_Connect.Select(SQL_Com));
        }

        public bool Security(string User, string Password)
        {
            bool Permission = false;
            if (User == "PakXali" && Password == "P@kK@lh!")
                Permission = true;
            else
                Permission = false;

            return Permission;
        }

        private int Pagenation_number(int page)
        {
            page = page * 100;
            return page;
        }
    }
}