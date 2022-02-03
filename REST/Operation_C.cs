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
        public string Operation_order(int page, string _operator, string orderstatus, DateTime startdate, DateTime enddate)
        {
            page = Pagenation_number(page);
            _operator = injection.injection_string(_operator);
            orderstatus = injection.injection_string(orderstatus);
            SQL_Com = new SqlCommand(@"WITH OPERATION (operatorName,CreatedDate,orderStatus,orderCount) AS(
                SELECT usr.FullName AS OperatorName,
					ord.CreatedDate,
					(CASE 
						WHEN ord.OrderStatus=0 THEN N'Sifariş götürüldü'
						WHEN ord.OrderStatus=1 THEN N'Maraqlandı'
						WHEN ord.OrderStatus=3 THEN N'İmtina'
						ELSE 'Geri qaytarma'
					END) OrderStatus,
					COUNT(ord.OrderStatus) OrderCount
				FROM Orders ord
				INNER JOIN Users usr on usr.Id=ord.CreatedBy
				WHERE ord.OrderStatus<>2
				GROUP BY usr.FullName,
					ord.CreatedDate,
					ord.OrderStatus
                ORDER BY ORD.CREATEDDATE DESC
				OFFSET @PAGINATIONID ROW
                )  
                SELECT TOP(100) OperatorName as [operatorName], CONVERT(nvarchar, CREATEDDATE, 20) AS [createDate],OrderStatus AS [orderStatus], OrderCount AS [orderCount] FROM OPERATION
                WHERE operatorName LIKE N'%" + _operator + "%' AND OrderStatus LIKE N'%" + orderstatus + "%' AND CreatedDate BETWEEN '" + startdate + "' AND '" + enddate + "'");
            SQL_Com.Parameters.Add("@PAGINATIONID", SqlDbType.Int).Value = page;

            return Jeson.ConvertDataTabletoString(SQL_Connect.Select(SQL_Com));
        }
        public string Product_sell(int page, string _operator, DateTime startdate, DateTime enddate)
        {
            page = Pagenation_number(page);
            _operator = injection.injection_string(_operator);
            SQL_Com = new SqlCommand(@"WITH OPERATION (FACT_NO,Expeditor_Name,GIVE_DATE,Product_Name,quantity,price,discount,amount,lastAmount,note) AS(
	                                        SELECT (drv.Code+CAST(ord.Id AS varchar)) AS FACT_NO,
		                                        usr.FullName AS Expeditor_Name,
		                                        ordPc.CreatedDate AS GIVE_DATE,
		                                        itm.Name AS Product_Name,
		                                        pl.Quantity,
		                                        pl.Price,
		                                        pl.Discount,
		                                        pl.Amount,
		                                        pl.LastAmount,
		                                        pl.Note
	                                        FROM 
	                                        Orders ord
	                                        INNER JOIN Drivers drv on drv.Id=ord.DriverId
	                                        INNER JOIN Users usr on usr.Id=drv.UserId
	                                        INNER JOIN OrderProcess ordPc on ordPc.OrderId=ord.Id and ordPc.OperationStatus=2
	                                        INNER JOIN OrderProcessLine pl on pl.OrderProcessId=ordPc.Id
	                                        INNER JOIN Items itm on itm.Id=pl.ItemId and itm.ProductSales=1
	                                        order by ordPc.CreatedDate DESC
	                                        OFFSET @PAGINATIONID ROW  
                                        )  
                                        SELECT TOP(100) FACT_NO AS [texture], Expeditor_Name AS [expeditorName], CONVERT(nvarchar, GIVE_DATE, 20) AS [giveDate], Product_Name AS [productName], Quantity AS [quantity], 
                                        Price AS [price], Discount AS [discount], Amount AS [amount], LastAmount AS [lastAmount], Note AS [note]
                                        FROM OPERATION
                                        where Expeditor_Name LIKE '%" + _operator + "%' AND GIVE_DATE BETWEEN '" + startdate + "' AND '" + enddate + "'");
            SQL_Com.Parameters.Add("@PAGINATIONID", SqlDbType.Int).Value = page;

            return Jeson.ConvertDataTabletoString(SQL_Connect.Select(SQL_Com));
        } 
        public string OperatorName_and_OrderStatus()
        {
            string All_data;

            SQL_Com = new SqlCommand("SELECT DISTINCT OperatorName FROM oper_sif");
            All_data = Jeson.ConvertDataTabletoString(SQL_Connect.Select(SQL_Com));
            All_data += ",";
            SQL_Com = new SqlCommand("SELECT DISTINCT OrderStatus FROM oper_sif");
            All_data += Jeson.ConvertDataTabletoString(SQL_Connect.Select(SQL_Com));

            return All_data;
        }
        public string OperatorName_and_OrderStatus_Dashboard(string OperatorName, string OrderStatus,DateTime startDate, DateTime endDate)
        {
            OperatorName = injection.injection_string(OperatorName);
            OrderStatus = injection.injection_string(OrderStatus);
            endDate.AddDays(1);
            SQL_Com = new SqlCommand(@"select 
                                            OperatorName,
                                            OrderStatus,
                                            sum(OrderCount) Toplam
                                        from oper_sif
                                        where OperatorName like '%" + OperatorName + "%' and OrderStatus like '%" + OrderStatus + "%' and CreatedDate between '" + startDate + "' and  '" + endDate + "'group by OperatorName,OrderStatus");
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