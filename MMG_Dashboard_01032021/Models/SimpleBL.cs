using System;
using SimpleTest;
using System.Data;
using System.Text;

/// <summary>
/// Summary description for SimpleBL
/// </summary>
public class SimpleBL
{
    SimpleUtil objUti = SimpleUtil.Instance;
    public SimpleBL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public DataTable getLoginDetails(string _gUserName, string _gPassword)
    {
        string sql = "SELECT a.emp_name, a.emp_id, imei_no, division, emp_type, ROLE, a.vendor_id, a.active_flag, b.login_id, PASSWORD, login_type, b.active_flag, a.COMPANY ";
        sql += " ,(SELECT VENDOR_NAME FROM mobint.mcr_vendor_mst where 1=1 AND EMP_ID=upper('" + _gUserName + "') AND ACTIVE_FLAG='Y') Vendorname FROM MOBINT.MCR_USER_DETAILS a, MOBINT.MCR_LOGIN_MST b";
        sql += " WHERE a.EMP_ID=b.LOGIN_ID ";
        sql += " AND a.ACTIVE_FLAG='Y' AND b.ACTIVE_FLAG='Y' AND upper(login_id)=upper('" + _gUserName + "') ";

        if (_gPassword != "")
            sql += " AND PASSWORD='" + _gPassword + "'";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }
    public int UpdatePassword(string _gUserName, string _gOldPassword, string _gPassword)
    {
        string sql = "UPDATE MOBINT.MCR_LOGIN_MST SET PASSWORD='" + _gPassword + "' WHERE LOGIN_ID ='" + _gUserName + "' AND PASSWORD='" + _gOldPassword + "'";

        int Result = objUti.ExecuteNonQuery(sql);
        return Result;
    }
    public DataTable getDivisionDetails()
    {
        string sql = "SELECT DIST_CD, DIVISION_NAME FROM MOBINT.mcr_division where DIST_CD like 'S%' OR DIST_CD like 'W%'  ORDER BY  DIVISION_NAME ASC";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable getVendorDetails(string Vendorid)
    {
        string sql = "SELECT VENDOR_ID,VENDOR_ID||'|'|| VENDOR_NAME NAME FROM MOBINT.MCR_VENDOR_MST WHERE ADDRESS='" + Vendorid + "'";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }
    public DataTable getRoleRightName_IDWise(string _sRoleTYpe)
    {
        string sql = "";
        sql = "SELECT ROLE_NAME FROM MOBINT.MCR_ROLE_MST WHERE ROLE_ID='" + _sRoleTYpe + "' ";

        DataTable dt = objUti.ExecuteReader(sql.ToString());
        return dt;
    }
    public DataTable GetPunchData(string _gUserName, string _gPassword)
    {
        string sql = "SELECT INVOICE_ID,DIVISION,TYPE,MONTH,YEAR,BILLNO,WORKORDERNO,CASE WHEN STATUS ='D' THEN 'DRAFT' WHEN STATUS='S' THEN 'Pending For Approval' WHEN STATUS='R' THEN 'Rollback' WHEN STATUS='P' THEN 'Approved' ELSE 'NA' END AS STATUS,STATUS as STATUS_Code,to_char(ENTRY_DATE,'DD/MM/YYYY') AS CREATEDON,APPROVER_REMARKS FROM MOBINT.MMG_BILL_STATUS WHERE ISACTIVE='Y' AND VENDORCODE='221074' AND BILLNO='BILL1234'";
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable GetGenerate_Invoice(string Month, string Year, string VendorId)
    {
        string sql = "SELECT distinct M.ITEMCODE,M.DESCRIPTION,M.UNIT,M.UNITRATE,B.QUANTITY,(M.UNITRATE * B.QUANTITY) AS AMOUNT FROM MOBINT.MMG_ITEM_MASTER M, MOBINT.TRN_WORK_MEASUREMENT_BASE B";
        sql += " WHERE M.ITEMCODE = B.ITEMCODE";
        sql += " AND B.MONTH = '" + Month + "'";
        sql += " AND B.YEAR = '" + Year + "'";
        sql += " AND B.VENDORCODE = '" + VendorId + "'";
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable GetGenerate_Invoice1(string Month, string Year, string VendorId)
    {
        string sql = "SELECT ITEMCODE,DESCRIPTION,UNIT,UNIT_RATE UNITRATE,QUANTITY, AMOUNT FROM MOBINT.WORK_MEASUREMENT_BASE";
        sql += " WHERE MONTH = '" + Month + "'";
        sql += " AND YEAR = '" + Year + "'";
        sql += " AND VENDORCODE = '" + VendorId + "'";
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable GetAnnexure1(string Vendor, string Month, string Year)
    {
        string sql = "SELECT DISTINCT MMGWORKID,YEAR,MONTH,VENDORCODE,ORDERTYPE,ACTIVITYTYPE,MAT_DESC,ISOLDMETER,METERTYPE_1PH,METERTYPE_3PH,CABLE_INS_2X10";
        sql += ",CABLE_INS_2X25,CABLE_INS_4X25,CABLE_INS_4X50,CABLE_INS_4X150,CABLE_REMOVED_2X10,CABLE_REMOVED_2X25,CABLE_REMOVED_4X25";
        sql += ",CABLE_REMOVED_4X50,CABLE_REMOVED_4X150 ";
        sql += ",CASE WHEN ORDERTYPE = 'ZDIN' THEN 1 ";
        sql += " WHEN ORDERTYPE = 'ZDRP' THEN 2";
        sql += " WHEN ORDERTYPE = 'ZDRM' THEN 3";
        sql += " WHEN ORDERTYPE = 'ZMSO' THEN 4";
        sql += " WHEN ORDERTYPE = 'ZDIV' THEN 5";
        sql += " WHEN ORDERTYPE = 'ZMSC' THEN 6";
        sql += " END AS SEQ";
        sql += " FROM MOBINT.TRN_MMG_WORK_SUMMARY ";
        sql += " WHERE VENDORCODE='" + Vendor + "' AND ";
        sql += " Month=" + Month + " AND YEAR =" + Year + "";
        sql += " ORDER BY ORDERTYPE";
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable GetAnnexure_Excel1(string Vendor, string Month, string Year)
    {
        string sql = "SELECT DISTINCT ORDERTYPE,ACTIVITYTYPE,MAT_DESC,ISOLDMETER,METERTYPE_1PH,METERTYPE_3PH,CABLE_INS_2X10";
        sql += ",CABLE_INS_2X25,CABLE_INS_4X25,CABLE_INS_4X50,CABLE_INS_4X150,CABLE_REMOVED_2X10,CABLE_REMOVED_2X25,CABLE_REMOVED_4X25";
        sql += ",CABLE_REMOVED_4X50,CABLE_REMOVED_4X150 ";
        sql += " FROM MOBINT.TRN_MMG_WORK_SUMMARY ";
        sql += " WHERE VENDORCODE='" + Vendor + "' AND ";
        sql += " Month=" + Month + " AND YEAR =" + Year + "";
        sql += " ORDER BY ORDERTYPE";
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }
    public DataTable GetScrapVerification(string Vendor, string Month, string Year)
    {
        string sql = "SELECT DISTINCT SCRAP_VERIFICATION_ID,MONTH,YEAR,VENDORCODE,JOB_IN SCRAP_JOB_ID ,JOB_DESCRIPTION JOB_NAME,VALUE,CONSUMED_CABLE_2X10,";
        sql += " CONSUMED_CABLE_2X25,CONSUMED_CABLE_4X25 ,CONSUMED_CABLE_4X50,CONSUMED_CABLE_4X150 ,";
        sql += "CONSUMED_CABLE_2X10 * (VALUE / 100) AS SCRAP_CABLE_2X10, CONSUMED_CABLE_2X25 *(VALUE / 100) AS SCRAP_CABLE_2X25,";
        sql += "CONSUMED_CABLE_4X25 *(VALUE / 100) AS SCRAP_CABLE_4X25, CONSUMED_CABLE_4X50 *(VALUE / 100) AS SCRAP_CABLE_4X50,";
        sql += "  CONSUMED_CABLE_4X150 *(VALUE / 100) AS SCRAP_CABLE_4X150,'' AS REMARKS";
        sql += " FROM MOBINT.MMG_SCRAP_VERIFICATION";
        sql += " WHERE MONTH = " + Month + " AND YEAR =" + Year + " AND VENDORCODE ='" + Vendor + "'";
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable GetPenalty(string Billexcel)
    {
        string sql = string.Empty;
        if (String.IsNullOrEmpty(Billexcel))
        {
            sql = "SELECT DISTINCT PENALTYID,PENALTY,PENALTY_INFO,SEQ, '' as AMOUNT FROM MOBINT.MMG_PENALTY_MASTER WHERE STATUS='Y' ORDER BY PENALTYID";
        }
        else
        {
            sql = "SELECT DISTINCT PENALTYID,PENALTY,PENALTY_INFO,AMOUNT FROM MOBINT.MMG_PENALTY_MASTER WHERE STATUS='Y' ORDER BY PENALTYID";
        }
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }
    public DataTable GetAnnexure2(string Vendor, string Month, string Year)
    {
        //string sql = "SELECT DISTINCT MC.MATERIALCONSUMPTIONID,I.ITEMID ,MC.MONTH,MC.YEAR,MC.VENDORCODE ";
        //sql += ", I.ITEMNAME,I.UNIT,MC.CONSUMPTION ,'' as PREVIOUS_MONTH_BALANCE,'' as RECEIVED_FROM_DIVISION,'' as REMOVED_FROM_SITE,'' as RECEIVED_FROM_STORE,'' as TOTAL,";
        //sql += " '' as RETURNED_TO_STORE,'' as TRANSFER_TO_DIVISION,'' as BALANCE,'' as REMARKS FROM TRN_MATERIAL_CONSUMPTION MC JOIN ITEM_MASTER I ON MC.ITEMID = I.ITEMID ";
        //sql += " WHERE MC.VENDORCODE = '2553586' AND";
        //sql += " MC.Month = 3 AND MC.YEAR = 2021";
        //DataTable dt = objUti.ExecuteReader(sql);
        //return dt;

        string sql = "SELECT ITEMID,ITEM_NAME ITEMNAME,UNIT,";
        sql += "PREVIOUS_MONTH_BALANCE,RECEIVED_FROM_DIVISION,REMOVED_FROM_SITE,RECEIVED_FROM_STORE,TOTAL,CONSUMPTION,RETURNED_TO_STORE,";
        sql += "TRANSFER_TO_DIVISION,BALANCE,REMARKS FROM MOBINT.MMG_MATERIAL_CONSUMPTION ";
        sql += " WHERE VENDORCODE = '" + Vendor + "' AND";
        sql += " Month =" + Month + " AND YEAR = " + Year + "";
        sql += " ORDER BY ITEMID ASC";
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable Get_Before_Gen_Annexure2(string Vendor, string Month, string Year)
    {
        string sql = "SELECT MATERIALCONSUMPTIONID,ITEMID ,MONTH,YEAR,VENDORCODE ";
        sql += ", ITEM_NAME ITEMNAME,UNIT,CONSUMPTION ,'' as PREVIOUS_MONTH_BALANCE,'' as RECEIVED_FROM_DIVISION,'' as REMOVED_FROM_SITE,'' as RECEIVED_FROM_STORE,'' as TOTAL,";
        sql += " '' as RETURNED_TO_STORE,'' as TRANSFER_TO_DIVISION,'' as BALANCE,'' as REMARKS FROM MOBINT.TRN_MATERIAL_CONSUMPTION";
        sql += " WHERE VENDORCODE = '" + Vendor + "' AND";
        sql += " Month = " + Month + " AND YEAR = " + Year + "";
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }
    public DataTable GetOrderDetails(string Division, string Vendorid, string Orderid, string CA, string Month, string Year, string Activityfromdate, string Activitytodate)
    {
        string sql = "SELECT ORDER_CREATION_ID, ORDERID, ORDER_TYPE, PM_ACTIVITY, VENDOR_CODE, to_char(ACTIVITY_DATE, 'DD/MM/YYYY') AS ACTIVITY_DATE,";
        sql += " CASE WHEN APPROVAL_STATUS = 'P' THEN 'Pending' ELSE 'In-Progress' END AS Status,CASE WHEN PUNCH_STATUS = 'M' THEN 'Manual' ELSE 'System' END AS OrderEntryType,";
        sql += " APPROVER_REMARKS,to_char(APPROVED_DATE, 'DD/MM/YYYY') AS CREATEDON,CA,DIVISION,MONTH,YEAR from MOBINT.MMG_ORDER_CREATION_DETAILS WHERE ACTIVITY_DATE BETWEEN TO_DATE('" + Activityfromdate + "','dd/MM/yyyy') AND  TO_DATE('" + Activitytodate + "','dd/MM/yyyy')";
        if (!String.IsNullOrEmpty(Orderid))
        {
            sql += " AND UPPER(TRIM(ORDERID)) LIKE '%" + Orderid + "%'";
        }
        sql += " AND APPROVAL_STATUS = 'P'";
        if (!String.IsNullOrEmpty(Division) && Division != "0")
        {
            sql += " AND DIVISION = '" + Division + "'";
        }
        if (!String.IsNullOrEmpty(Vendorid) && Vendorid != "0" && Vendorid != "Select")
        {
            sql += " AND VENDOR_CODE = '" + Vendorid + "'";
        }
        if (!String.IsNullOrEmpty(Month) && Month != "0")
        {
            sql += " AND MONTH = '" + Month + "'";
        }
        if (!String.IsNullOrEmpty(Year) && Year != "0")
        {
            sql += " AND YEAR = '" + Year + "'";
        }
        if (!String.IsNullOrEmpty(CA))
        {
            sql += " AND CA='" + CA + "'";
        }
        sql += " AND PUNCH_STATUS = 'M'";
        sql += " ORDER BY ORDER_CREATION_ID DESC";
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }
    public DataTable GetOrderTabDetails(string Division, string Vendorid, string Orderid, string CA, string Month, string Year, string Activityfromdate, string Activitytodate)
    {
        string sql = "SELECT ORDER_CREATION_ID, ORDERID, ORDER_TYPE, PM_ACTIVITY, VENDOR_CODE, to_char(ACTIVITY_DATE, 'DD/MM/YYYY') AS ACTIVITY_DATE,";
        sql += " CASE WHEN APPROVAL_STATUS = 'P' THEN 'Pending' ELSE 'In-Progress' END AS Status,CASE WHEN PUNCH_STATUS = 'M' THEN 'Manual' ELSE 'System' END AS OrderEntryType,";
        sql += " APPROVER_REMARKS,to_char(APPROVED_DATE, 'DD/MM/YYYY') AS CREATEDON,CA,DIVISION,MONTH,YEAR FROM MOBINT.MMG_ORDER_CREATION_DETAILS";
        sql += " WHERE ACTIVITY_DATE BETWEEN TO_DATE('" + Activityfromdate + "','dd/MM/yyyy') AND  TO_DATE('" + Activitytodate + "','dd/MM/yyyy')";

        if (!String.IsNullOrEmpty(Orderid))
        {
            sql += " AND UPPER(TRIM(ORDERID)) LIKE '%" + Orderid + "%'";
        }
        sql += " AND APPROVAL_STATUS = 'P'";
        if (!String.IsNullOrEmpty(Division) && Division != "0")
        {
            sql += " AND DIVISION = '" + Division + "'";
        }
        if (!String.IsNullOrEmpty(Vendorid) && Vendorid != "0" && Vendorid != "Select")
        {
            sql += " AND VENDOR_CODE = '" + Vendorid + "'";
        }
        if (!String.IsNullOrEmpty(Month) && Month != "0")
        {
            sql += " AND MONTH = '" + Month + "'";
        }
        if (!String.IsNullOrEmpty(Year) && Year != "0")
        {
            sql += " AND YEAR = '" + Year + "'";
        }
        if (!String.IsNullOrEmpty(CA))
        {
            sql += " AND CA='" + CA + "'";
        }
        sql += " AND PUNCH_STATUS = 'A'";
        sql += " ORDER BY ORDER_CREATION_ID DESC";
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }
    public DataTable GetActivityType(string orderType)
    {
        string sql = "SELECT ORDERTYPE,ACTIVITYtYPE,ACTIVITYtYPE||'-'||ACTIVITY_DESC AS ACTIVITY_DESC FROM MOBINT.ORDER_TYPE_ACTIVITY_MAPPING WHERE ORDERTYPE ='" + orderType + "'";
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable Get_Scrap_Verification(string Vendor, string Month, string Year)
    {
        //string sql = "SELECT DISTINCT SCRAP_VERIFICATION_ID,MONTH,YEAR,VENDORCODE,JOB_IN SCRAP_JOB_ID ,JOB_DESCRIPTION JOB_NAME,VALUE,CONSUMED_CABLE_2X10,";
        //sql += "CONSUMED_CABLE_2X25,CONSUMED_CABLE_4X25 ,CONSUMED_CABLE_4X50,CONSUMED_CABLE_4X150 ,";
        //sql += "CONSUMED_CABLE_2X10 * (VALUE / 100) AS SCRAP_CABLE_2X10, CONSUMED_CABLE_2X25 *(VALUE / 100) AS SCRAP_CABLE_2X25,";
        //sql += " CONSUMED_CABLE_4X25 *(VALUE / 100) AS SCRAP_CABLE_4X25, CONSUMED_CABLE_4X50 *(VALUE / 100) AS SCRAP_CABLE_4X50,";
        //sql += " CONSUMED_CABLE_4X150 * (VALUE / 100) AS SCRAP_CABLE_4X150,' ' AS REMARKS";
        //sql += " FROM MOBINT.MMG_SCRAP_VERIFICATION WHERE MONTH = 03 AND YEAR = 2021 AND VENDORCODE = '2553586'";
        //sql += " union ";
        //sql += "SELECT SCRAP_VERIFICATION_FOOTER_ID SCRAP_VERIFICATION_ID, MONTH, YEAR, VENDORCODE,'0' AS SCRAP_JOB_ID,'Previous Balance Old Cable' AS JOB_NAME,0 AS VALUE";
        //sql += ", PREVBAL_CONSUMED_CABLE_2X10, PREVBAL_CONSUMED_CABLE_2X25, PREVBAL_CONSUMED_CABLE_4X25, PREVBAL_CONSUMED_CABLE_4X50, PREVBAL_CONSUMED_CABLE_4X150";
        //sql += ", PREVBAL_SCRAP_CABLE_2X10, PREVBAL_SCRAP_CABLE_2X25, PREVBAL_SCRAP_CABLE_4X25, PREVBAL_SCRAP_CABLE_4X50, PREVBAL_SCRAP_CABLE_4X150,' ' AS REMARKS";
        //sql += " FROM MMG_SCRAP_VERIFICATION_FOOTER WHERE MONTH = 03 AND YEAR = 2021 AND VENDORCODE = '2553586'";
        //sql += " union ";
        //sql += "SELECT SCRAP_VERIFICATION_FOOTER_ID,MONTH,YEAR,VENDORCODE,'0' AS SCRAP_JOB_ID,'Cable Reused' AS JOB_NAME,0 AS VALUE";
        //sql += ", CABLEREUSED_CONSUMED_CABLE_2X10, CABLEREUSED_CONSUMED_CABLE_2X25, CABLEREUSED_CONSUMED_CABLE_4X25, CABLEREUSED_CONSUMED_CABLE_4X50, CABLEREUSED_CONSUMED_CABLE_4X150";
        //sql += ", CABLEREUSED_SCRAP_CABLE_2X10, CABLEREUSED_SCRAP_CABLE_2X25, CABLEREUSED_SCRAP_CABLE_4X25, CABLEREUSED_SCRAP_CABLE_4X50, CABLEREUSED_SCRAP_CABLE_4X150";
        //sql += ",' ' AS REMARKS FROM MMG_SCRAP_VERIFICATION_FOOTER WHERE MONTH = 03 AND YEAR = 2021 AND VENDORCODE = '2553586'";
        //sql += " union ";
        //sql += "SELECT SCRAP_VERIFICATION_FOOTER_ID,MONTH,YEAR,VENDORCODE,'0' AS SCRAP_JOB_ID,'Balance Old Cable to be Reused' AS JOB_NAME,0 AS VALUE";
        //sql += ", OLDCABLE_CONSUMED_CABLE_2X10, OLDCABLE_CONSUMED_CABLE_2X25, OLDCABLE_CONSUMED_CABLE_4X25, OLDCABLE_CONSUMED_CABLE_4X50, OLDCABLE_CONSUMED_CABLE_4X150, OLDCABLE_SCRAP_CABLE_2X10";
        //sql += ", OLDCABLE_SCRAP_CABLE_2X25, OLDCABLE_SCRAP_CABLE_4X25, OLDCABLE_SCRAP_CABLE_4X50, OLDCABLE_SCRAP_CABLE_4X150";
        //sql += ",' ' AS REMARKS FROM MMG_SCRAP_VERIFICATION_FOOTER   WHERE MONTH = 03 AND YEAR = 2021 AND VENDORCODE = '2553586'";
        //sql += " union ";
        //sql += "SELECT SCRAP_VERIFICATION_FOOTER_ID,MONTH,YEAR,VENDORCODE,'0' AS SCRAP_JOB_ID,'New Cables Return To Store As Scrap' AS JOB_NAME,0 AS VALUE";
        //sql += ", NEWCABLE_CONSUMED_CABLE_2X10, NEWCABLE_CONSUMED_CABLE_2X25, NEWCABLE_CONSUMED_CABLE_4X25, NEWCABLE_CONSUMED_CABLE_4X50, NEWCABLE_CONSUMED_CABLE_4X150";
        //sql += ", NEWCABLE_SCRAP_CABLE_2X10, NEWCABLE_SCRAP_CABLE_2X25, NEWCABLE_SCRAP_CABLE_4X25, NEWCABLE_SCRAP_CABLE_4X50, NEWCABLE_SCRAP_CABLE_4X150";
        //sql += ",' ' AS REMARKS FROM MMG_SCRAP_VERIFICATION_FOOTER WHERE MONTH = 03 AND YEAR = 2021 AND VENDORCODE = '2553586'";
        string sql = "SELECT JOB_DESCRIPTION JOB_NAME,SUM(VALUE) VALUE,SUM(CONSUMED_CABLE_2X10) CONSUMED_CABLE_2X10,";
        sql += "SUM(CONSUMED_CABLE_2X25) CONSUMED_CABLE_2X25,SUM(CONSUMED_CABLE_4X25) CONSUMED_CABLE_4X25,SUM(CONSUMED_CABLE_4X50) CONSUMED_CABLE_4X50,SUM(CONSUMED_CABLE_4X150) CONSUMED_CABLE_4X150 ,";
        sql += "SUM(CONSUMED_CABLE_2X10 * (VALUE / 100)) AS SCRAP_CABLE_2X10, SUM(CONSUMED_CABLE_2X25 *(VALUE / 100)) AS SCRAP_CABLE_2X25,";
        sql += "SUM(CONSUMED_CABLE_4X25 *(VALUE / 100)) AS SCRAP_CABLE_4X25, SUM(CONSUMED_CABLE_4X50 *(VALUE / 100)) AS SCRAP_CABLE_4X50,";
        sql += "SUM(CONSUMED_CABLE_4X150 * (VALUE / 100)) AS SCRAP_CABLE_4X150,' ' AS REMARKS";
        sql += " FROM MOBINT.MMG_SCRAP_VERIFICATION WHERE MONTH =" + Month + " AND YEAR = " + Year + " AND VENDORCODE = '" + Vendor + "' GROUP BY ROLLUP(JOB_DESCRIPTION)";
        sql += " union ";
        sql += "SELECT 'Previous Balance Old Cable' AS JOB_NAME,0 AS VALUE";
        sql += ", PREVBAL_CONSUMED_CABLE_2X10, PREVBAL_CONSUMED_CABLE_2X25, PREVBAL_CONSUMED_CABLE_4X25, PREVBAL_CONSUMED_CABLE_4X50, PREVBAL_CONSUMED_CABLE_4X150";
        sql += ", PREVBAL_SCRAP_CABLE_2X10, PREVBAL_SCRAP_CABLE_2X25, PREVBAL_SCRAP_CABLE_4X25, PREVBAL_SCRAP_CABLE_4X50, PREVBAL_SCRAP_CABLE_4X150,' ' AS REMARKS";
        sql += " FROM MMG_SCRAP_VERIFICATION_FOOTER WHERE MONTH = " + Month + " AND YEAR = " + Year + " AND VENDORCODE = '" + Vendor + "'";
        sql += " union ";
        sql += "SELECT 'Cable Reused' AS JOB_NAME,0 AS VALUE";
        sql += ", CABLEREUSED_CONSUMED_CABLE_2X10, CABLEREUSED_CONSUMED_CABLE_2X25, CABLEREUSED_CONSUMED_CABLE_4X25, CABLEREUSED_CONSUMED_CABLE_4X50, CABLEREUSED_CONSUMED_CABLE_4X150";
        sql += ", CABLEREUSED_SCRAP_CABLE_2X10, CABLEREUSED_SCRAP_CABLE_2X25, CABLEREUSED_SCRAP_CABLE_4X25, CABLEREUSED_SCRAP_CABLE_4X50, CABLEREUSED_SCRAP_CABLE_4X150";
        sql += ",' ' AS REMARKS FROM MMG_SCRAP_VERIFICATION_FOOTER WHERE MONTH = " + Month + " AND YEAR = " + Year + " AND VENDORCODE = '" + Vendor + "'";
        sql += " union ";
        sql += "SELECT 'Balance Old Cable to be Reused' AS JOB_NAME,0 AS VALUE";
        sql += ", OLDCABLE_CONSUMED_CABLE_2X10, OLDCABLE_CONSUMED_CABLE_2X25, OLDCABLE_CONSUMED_CABLE_4X25, OLDCABLE_CONSUMED_CABLE_4X50, OLDCABLE_CONSUMED_CABLE_4X150, OLDCABLE_SCRAP_CABLE_2X10";
        sql += ", OLDCABLE_SCRAP_CABLE_2X25, OLDCABLE_SCRAP_CABLE_4X25, OLDCABLE_SCRAP_CABLE_4X50, OLDCABLE_SCRAP_CABLE_4X150";
        sql += ",' ' AS REMARKS FROM MMG_SCRAP_VERIFICATION_FOOTER   WHERE MONTH = " + Month + " AND YEAR = " + Year + " AND VENDORCODE = '" + Vendor + "'";
        sql += " union ";
        sql += "SELECT 'New Cables Return To Store As Scrap' AS JOB_NAME,0 AS VALUE";
        sql += ", NEWCABLE_CONSUMED_CABLE_2X10, NEWCABLE_CONSUMED_CABLE_2X25, NEWCABLE_CONSUMED_CABLE_4X25, NEWCABLE_CONSUMED_CABLE_4X50, NEWCABLE_CONSUMED_CABLE_4X150";
        sql += ", NEWCABLE_SCRAP_CABLE_2X10, NEWCABLE_SCRAP_CABLE_2X25, NEWCABLE_SCRAP_CABLE_4X25, NEWCABLE_SCRAP_CABLE_4X50, NEWCABLE_SCRAP_CABLE_4X150";
        sql += ",' ' AS REMARKS FROM MMG_SCRAP_VERIFICATION_FOOTER WHERE MONTH = " + Month + " AND YEAR = " + Year + " AND VENDORCODE = '" + Vendor + "' order by VALUE desc";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }
    public int Insert_LooseMtrDT_InputDetails(string _sCompCode, string _sOrdID, string _sMeterNo)
    {
        string sqlinsert = " INSERT INTO  MOBINT.MCR_input_details (COMP_CODE,ORDERID,METER_NO,FLAG)VALUES('" + _sCompCode + "','" + _sOrdID + "','" + _sMeterNo + "','C') ";

        return objUti.ExecuteNonQuery(sqlinsert);
    }
    public int Insert_Manual_Data(string COMPANY_CODE, string DIVISION, string VENDOR_CODE, string ORDERID, string ACTIVITY_DATE,
    string ORDER_TYPE, string PM_ACTIVITY, string ACCOUNT_CLASS, string PLANNER_GROUP, string BASIC_START_DATE, string BASIC_FINISH_DATE,
    string CUSTOMER_NAME, string ACTIVITY_REASON, string MOBILE_NO, string BP_NO, string CA, string DEVICENO, string METER_PHASE,
    string BOX_TYPE, string BOX_NO, string ADDRESS, string TERMINAL_SEAL1, string TERMINAL_SEAL2, string METERBOXSEAL1,
    string METERBOXSEAL2, string BUSBARSEAL1, string BUSBARSEAL2, string INSTALLEDBUSBAR, string BB_CABLE_USED,
    string BB_CAB_REMOVE_FRM_SITE, string BUSBARSIZE, string BUS_BAR_NO, string BUS_BAR_DRUM_NO, string B_BAR_CABLE_SIZE,
    string RMVD_BB_CBL_LENTH, string BUS_BAR_CABLE_LENG, string RUNNING_LENGTH_FROM_BB, string RUNNING_LENGTH_TO_BB,
    string RMVD_BB_CBL_SIZE, string REM_BUSBAR_SEAL1, string REM_BUSBAR_SEAL2, string BB_CABLE_NOT_INSTALL_REASON, string CABLE_REQD,
    string CABLE_LEN_USED, string CABLEINSTALLTYPE, string CABLE_REQD1, string OUTPUT_CABLE_LEN_USED, string CAB_REMOVE_FRM_SITE,
    string ELCB_INSTALLED, string DURM_NO, string CABLESIZE_OLD, string CABLELENGTH_OLD, string MCR_NO, string REMARKS, string CABLESIZE2,
    string CABLELENGTH, string OUTPUTCABLESIZE, string OUTPUTCABLELENGTH, string RUNNINGLENGTHFROM, string RUNNINGLENGTHTO,
    string CABLELENGTH_OLD1, string CABLENOTINSTALLREASON, string REM_BOX_SEAL1, string REM_BOX_SEAL2, string REM_OTHER_SEAL,
    string EARTHING_CONNECTOR, string JUBLIEE_CLAMPS, string HELPERNAME, string CLOSEHOOKBOLT, string NYLON_TIE, string FASTNER,
    string POLE_CONDITION, string SADDLES, string HAZARDOUS_TYPE, string NOS_CBLAT_POLE, string ADDITIONAL_POLE_REQUIRED,
    string IS_RECORD_PROCESSED, string ADDITIONAL_POLE_NUMBER, string SERVICE_PROVIDER, string DRIVER_NAME, string SUPERVISOR_NAME,
    string NOOFMETERS, string CONNECTEDMETERS, string OTHERSTICKER, string DB_TYPE, string GUNNYBAG_OLD, string GUNNYBAGSEAL_OLD,
    string LAB_TSTNG_NTC, string IS_GNY_BAG_PREPD, string VENDOR_NAME, string MONTH, string YEAR, string PIERCING_CONNECTOR,
    string PVC_GLAND, string THIMBLE, string ANCHOR_POLE_END_QTY, string UserId)
    {
        string sqlinsert = "INSERT INTO MOBINT.MMG_ORDER_CREATION_DETAILS (ORDER_CREATION_ID,COMPANY_CODE,DIVISION,VENDOR_CODE,ORDERID,";
        sqlinsert += " ACTIVITY_DATE,ORDER_TYPE,PM_ACTIVITY,ACCOUNT_CLASS,PLANNER_GROUP,BASIC_START_DATE,BASIC_FINISH_DATE,CUSTOMER_NAME,";
        sqlinsert += " ACTIVITY_REASON,MOBILE_NO,BP_NO,CA,DEVICENO,METER_PHASE,BOX_TYPE,BOX_NO,ADDRESS,TERMINAL_SEAL1,TERMINAL_SEAL2,";
        sqlinsert += " METERBOXSEAL1,METERBOXSEAL2,BUSBARSEAL1,BUSBARSEAL2,INSTALLEDBUSBAR,BB_CABLE_USED,BB_CAB_REMOVE_FRM_SITE,BUSBARSIZE,";
        sqlinsert += " BUS_BAR_NO,BUS_BAR_DRUM_NO,B_BAR_CABLE_SIZE,RMVD_BB_CBL_LENTH,BUS_BAR_CABLE_LENG,RUNNING_LENGTH_FROM_BB,";
        sqlinsert += " RUNNING_LENGTH_TO_BB,RMVD_BB_CBL_SIZE,REM_BUSBAR_SEAL1,REM_BUSBAR_SEAL2,BB_CABLE_NOT_INSTALL_REASON,CABLE_REQD1,";
        sqlinsert += " CABLE_LEN_USED,CABLEINSTALLTYPE,CABLE_REQD,OUTPUT_CABLE_LEN_USED,CAB_REMOVE_FRM_SITE,ELCB_INSTALLED,DURM_NO,";
        sqlinsert += " CABLESIZE_OLD,CABLELENGTH_OLD,MCR_NO,REMARKS,CABLESIZE2,CABLELENGTH,OUTPUTCABLESIZE,OUTPUTCABLELENGTH,";
        sqlinsert += " RUNNINGLENGTHFROM,RUNNINGLENGTHTO,CORD_INSTALLED,CABLENOTINSTALLREASON,REM_BOX_SEAL1,REM_BOX_SEAL2,REM_OTHER_SEAL,";
        sqlinsert += " EARTHING_CONNECTOR,JUBLIEE_CLAMPS,HELPERNAME,CLOSEHOOKBOLT,NYLON_TIE,FASTNER,POLE_CONDITION,SADDLES,HAZARDOUS_TYPE,";
        sqlinsert += " NOS_CBLAT_POLE,ADDITIONAL_POLE_REQUIRED,IS_RECORD_PROCESSED,ADDITIONAL_POLE_NUMBER,SERVICE_PROVIDER,DRIVER_NAME,SUPERVISOR_NAME,NOOFMETERS,";
        sqlinsert += " CONNECTEDMETERS,OTHERSTICKER,DB_TYPE,GUNNYBAG_OLD,GUNNYBAGSEAL_OLD,LAB_TSTNG_NTC,IS_GNY_BAG_PREPD,VENDOR_NAME,MONTH,YEAR,PUNCH_STATUS,PIERCING_CONNECTOR,PVC_GLAND,THIMBLE,ANCHOR_POLE_END_QTY,Punched_By)";
        sqlinsert += " VALUES ";
        sqlinsert += " (BILL_INVOICE.NEXTVAL,'" + COMPANY_CODE + "','" + DIVISION + "','" + VENDOR_CODE + "','" + ORDERID + "',TO_Date('" + ACTIVITY_DATE + "','dd-MM-yyyy'),";
        sqlinsert += "'" + ORDER_TYPE + "','" + PM_ACTIVITY + "','" + ACCOUNT_CLASS + "','" + PLANNER_GROUP + "',TO_DATE('" + BASIC_START_DATE + "','dd-MM-yyyy'),TO_DATE('" + BASIC_FINISH_DATE + "','dd-MM-yyyy'),";
        sqlinsert += "'" + CUSTOMER_NAME + "','" + ACTIVITY_REASON + "','" + MOBILE_NO + "','" + BP_NO + "','" + CA + "','" + DEVICENO + "','" + METER_PHASE + "',";
        sqlinsert += "'" + BOX_TYPE + "','" + BOX_NO + "','" + ADDRESS + "','" + TERMINAL_SEAL1 + "','" + TERMINAL_SEAL2 + "','" + METERBOXSEAL1 + "','" + METERBOXSEAL2 + "',";
        sqlinsert += "'" + BUSBARSEAL1 + "','" + BUSBARSEAL2 + "','" + INSTALLEDBUSBAR + "','" + BB_CABLE_USED + "','" + BB_CAB_REMOVE_FRM_SITE + "','" + BUSBARSIZE + "',";
        sqlinsert += "'" + BUS_BAR_NO + "','" + BUS_BAR_DRUM_NO + "','" + B_BAR_CABLE_SIZE + "','" + RMVD_BB_CBL_LENTH + "','" + BUS_BAR_CABLE_LENG + "',";
        sqlinsert += "'" + RUNNING_LENGTH_FROM_BB + "','" + RUNNING_LENGTH_TO_BB + "','" + RMVD_BB_CBL_SIZE + "','" + REM_BUSBAR_SEAL1 + "','" + REM_BUSBAR_SEAL2 + "',";
        sqlinsert += "'" + BB_CABLE_NOT_INSTALL_REASON + "','" + CABLE_REQD + "','" + CABLE_LEN_USED + "','" + CABLEINSTALLTYPE + "','" + CABLE_REQD1 + "','" + OUTPUT_CABLE_LEN_USED + "',";
        sqlinsert += "'" + CAB_REMOVE_FRM_SITE + "','" + ELCB_INSTALLED + "','" + DURM_NO + "','" + CABLESIZE_OLD + "','" + CABLELENGTH_OLD + "','" + MCR_NO + "','" + REMARKS + "',";
        sqlinsert += "'" + CABLESIZE2 + "','" + CABLELENGTH + "','" + OUTPUTCABLESIZE + "','" + OUTPUTCABLELENGTH + "','" + RUNNINGLENGTHFROM + "','" + RUNNINGLENGTHTO + "',";
        sqlinsert += "'" + CABLELENGTH_OLD1 + "','" + CABLENOTINSTALLREASON + "','" + REM_BOX_SEAL1 + "','" + REM_BOX_SEAL2 + "','" + REM_OTHER_SEAL + "',";
        sqlinsert += "" + Convert.ToInt32(EARTHING_CONNECTOR) + ",'" + JUBLIEE_CLAMPS + "','" + HELPERNAME + "','" + CLOSEHOOKBOLT + "'," + Convert.ToInt32(NYLON_TIE) + ",'" + FASTNER + "',";
        sqlinsert += "'" + POLE_CONDITION + "'," + Convert.ToInt32(SADDLES) + ",'" + HAZARDOUS_TYPE + "', '" + NOS_CBLAT_POLE + "','" + ADDITIONAL_POLE_REQUIRED + "',";
        sqlinsert += "'" + IS_RECORD_PROCESSED + "','" + ADDITIONAL_POLE_NUMBER + "','" + SERVICE_PROVIDER + "','" + DRIVER_NAME + "','" + SUPERVISOR_NAME + "','" + NOOFMETERS + "',";
        sqlinsert += "'" + CONNECTEDMETERS + "','" + OTHERSTICKER + "','" + DB_TYPE + "','" + GUNNYBAG_OLD + "','" + GUNNYBAGSEAL_OLD + "','" + LAB_TSTNG_NTC + "','" + IS_GNY_BAG_PREPD + "',";
        sqlinsert += "'" + VENDOR_NAME + "','" + MONTH + "','" + YEAR + "','M'," + Convert.ToInt32(PIERCING_CONNECTOR) + "," + Convert.ToInt32(PVC_GLAND) + "," + Convert.ToInt32(THIMBLE) + ",'" + ANCHOR_POLE_END_QTY + "','" + UserId + "')";
        return objUti.ExecuteNonQuery(sqlinsert);
    }
    public DataTable GetDetails(string _gOrderId)
    {
        string sql = "SELECT COMPANY_CODE,DIVISION,VENDOR_CODE,ORDERID,";
        sql += "ACTIVITY_DATE,ORDER_TYPE,PM_ACTIVITY,ACCOUNT_CLASS,PLANNER_GROUP,BASIC_START_DATE,BASIC_FINISH_DATE,CUSTOMER_NAME,";
        sql += "ACTIVITY_REASON,MOBILE_NO,BP_NO,CA,DEVICENO,METER_PHASE,BOX_TYPE,BOX_NO,ADDRESS,TERMINAL_SEAL1,TERMINAL_SEAL2,";
        sql += "METERBOXSEAL1,METERBOXSEAL2,BUSBARSEAL1,BUSBARSEAL2,INSTALLEDBUSBAR,BB_CABLE_USED,BB_CAB_REMOVE_FRM_SITE,BUSBARSIZE,";
        sql += "BUS_BAR_NO,BUS_BAR_DRUM_NO,B_BAR_CABLE_SIZE,RMVD_BB_CBL_LENTH,BUS_BAR_CABLE_LENG,RUNNING_LENGTH_FROM_BB,";
        sql += "RUNNING_LENGTH_TO_BB,RMVD_BB_CBL_SIZE,REM_BUSBAR_SEAL1,REM_BUSBAR_SEAL2,BB_CABLE_NOT_INSTALL_REASON,CABLE_REQD1,";
        sql += "CABLE_LEN_USED,CABLEINSTALLTYPE,CABLE_REQD,OUTPUT_CABLE_LEN_USED,CAB_REMOVE_FRM_SITE,ELCB_INSTALLED,DURM_NO,";
        sql += "CABLESIZE_OLD,CABLELENGTH_OLD,MCR_NO,REMARKS,CABLESIZE2,CABLELENGTH,OUTPUTCABLESIZE,OUTPUTCABLELENGTH,";
        sql += "RUNNINGLENGTHFROM,RUNNINGLENGTHTO,CORD_INSTALLED,CABLENOTINSTALLREASON,REM_BOX_SEAL1,REM_BOX_SEAL2,REM_OTHER_SEAL,";
        sql += "EARTHING_CONNECTOR,JUBLIEE_CLAMPS,HELPERNAME,CLOSEHOOKBOLT,NYLON_TIE,FASTNER,POLE_CONDITION,SADDLES,HAZARDOUS_TYPE,";
        sql += "NOS_CBLAT_POLE,ADDITIONAL_POLE_REQUIRED,IS_RECORD_PROCESSED,ADDITIONAL_POLE_NUMBER,SERVICE_PROVIDER,DRIVER_NAME,SUPERVISOR_NAME,NOOFMETERS,";
        sql += "CONNECTEDMETERS,OTHERSTICKER,DB_TYPE,GUNNYBAG_OLD,GUNNYBAGSEAL_OLD,LAB_TSTNG_NTC,IS_GNY_BAG_PREPD,VENDOR_NAME,MONTH,YEAR,PIERCING_CONNECTOR,";
        sql += "PVC_GLAND,THIMBLE,ANCHOR_POLE_END_QTY FROM MOBINT.MMG_ORDER_CREATION_DETAILS WHERE ORDERID='" + _gOrderId + "'";
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }
    public int Update_Manual_Data(string COMPANY_CODE, string DIVISION, string VENDOR_CODE, string ORDERID, string ACTIVITY_DATE,
string ORDER_TYPE, string PM_ACTIVITY, string ACCOUNT_CLASS, string PLANNER_GROUP, string BASIC_START_DATE, string BASIC_FINISH_DATE,
string CUSTOMER_NAME, string ACTIVITY_REASON, string MOBILE_NO, string BP_NO, string CA, string DEVICENO, string METER_PHASE,
string BOX_TYPE, string BOX_NO, string ADDRESS, string TERMINAL_SEAL1, string TERMINAL_SEAL2, string METERBOXSEAL1,
string METERBOXSEAL2, string BUSBARSEAL1, string BUSBARSEAL2, string INSTALLEDBUSBAR, string BB_CABLE_USED,
string BB_CAB_REMOVE_FRM_SITE, string BUSBARSIZE, string BUS_BAR_NO, string BUS_BAR_DRUM_NO, string B_BAR_CABLE_SIZE,
string RMVD_BB_CBL_LENTH, string BUS_BAR_CABLE_LENG, string RUNNING_LENGTH_FROM_BB, string RUNNING_LENGTH_TO_BB,
string RMVD_BB_CBL_SIZE, string REM_BUSBAR_SEAL1, string REM_BUSBAR_SEAL2, string BB_CABLE_NOT_INSTALL_REASON, string CABLE_REQD,
string CABLE_LEN_USED, string CABLEINSTALLTYPE, string CABLE_REQD1, string OUTPUT_CABLE_LEN_USED, string CAB_REMOVE_FRM_SITE,
string ELCB_INSTALLED, string DURM_NO, string CABLESIZE_OLD, string CABLELENGTH_OLD, string MCR_NO, string REMARKS, string CABLESIZE2,
string CABLELENGTH, string OUTPUTCABLESIZE, string OUTPUTCABLELENGTH, string RUNNINGLENGTHFROM, string RUNNINGLENGTHTO,
string CABLELENGTH_OLD1, string CABLENOTINSTALLREASON, string REM_BOX_SEAL1, string REM_BOX_SEAL2, string REM_OTHER_SEAL,
string EARTHING_CONNECTOR, string JUBLIEE_CLAMPS, string HELPERNAME, string CLOSEHOOKBOLT, string NYLON_TIE, string FASTNER,
string POLE_CONDITION, string SADDLES, string HAZARDOUS_TYPE, string NOS_CBLAT_POLE, string ADDITIONAL_POLE_REQUIRED,
string IS_RECORD_PROCESSED, string ADDITIONAL_POLE_NUMBER, string SERVICE_PROVIDER, string DRIVER_NAME, string SUPERVISOR_NAME,
string NOOFMETERS, string CONNECTEDMETERS, string OTHERSTICKER, string DB_TYPE, string GUNNYBAG_OLD, string GUNNYBAGSEAL_OLD,
string LAB_TSTNG_NTC, string IS_GNY_BAG_PREPD, string PIERCING_CONNECTOR, string PVC_GLAND, string THIMBLE, string ANCHOR_POLE_END_QTY, string Userid)
    {
        string sqlinsert = "UPDATE MOBINT.MMG_ORDER_CREATION_DETAILS SET COMPANY_CODE='" + COMPANY_CODE + "',DIVISION='" + DIVISION + "',VENDOR_CODE='" + VENDOR_CODE + "',ORDERID='" + ORDERID + "',";
        sqlinsert += " ACTIVITY_DATE=TO_DATE('" + ACTIVITY_DATE + "','dd-MM-yyyy'),ORDER_TYPE='" + ORDER_TYPE + "',PM_ACTIVITY='" + PM_ACTIVITY + "',ACCOUNT_CLASS='" + ACCOUNT_CLASS + "',PLANNER_GROUP='" + PLANNER_GROUP + "',BASIC_START_DATE=TO_DATE('" + BASIC_START_DATE + "','dd-MM-yyyy'),BASIC_FINISH_DATE=TO_DATE('" + BASIC_FINISH_DATE + "','dd-MM-yyyy'),CUSTOMER_NAME='" + CUSTOMER_NAME + "',";
        sqlinsert += " ACTIVITY_REASON='" + ACTIVITY_REASON + "',MOBILE_NO='" + MOBILE_NO + "',BP_NO='" + BP_NO + "',CA='" + CA + "',DEVICENO='" + DEVICENO + "',METER_PHASE='" + METER_PHASE + "',BOX_TYPE='" + BOX_TYPE + "',BOX_NO='" + BOX_NO + "',ADDRESS='" + ADDRESS + "',TERMINAL_SEAL1='" + TERMINAL_SEAL1 + "',TERMINAL_SEAL2='" + TERMINAL_SEAL2 + "',";
        sqlinsert += " METERBOXSEAL1='" + METERBOXSEAL1 + "',METERBOXSEAL2='" + METERBOXSEAL1 + "',BUSBARSEAL1='" + BUSBARSEAL1 + "',BUSBARSEAL2='" + BUSBARSEAL2 + "',INSTALLEDBUSBAR='" + INSTALLEDBUSBAR + "',BB_CABLE_USED='" + BB_CABLE_USED + "',BB_CAB_REMOVE_FRM_SITE='" + BB_CAB_REMOVE_FRM_SITE + "',BUSBARSIZE='" + BUSBARSIZE + "',";
        sqlinsert += " BUS_BAR_NO='" + BUS_BAR_NO + "',BUS_BAR_DRUM_NO='" + BUS_BAR_DRUM_NO + "',B_BAR_CABLE_SIZE='" + B_BAR_CABLE_SIZE + "',RMVD_BB_CBL_LENTH='" + RMVD_BB_CBL_LENTH + "',BUS_BAR_CABLE_LENG='" + BUS_BAR_CABLE_LENG + "',RUNNING_LENGTH_FROM_BB='" + RUNNING_LENGTH_FROM_BB + "',";
        sqlinsert += " RUNNING_LENGTH_TO_BB='" + RUNNING_LENGTH_TO_BB + "',RMVD_BB_CBL_SIZE='" + RMVD_BB_CBL_SIZE + "',REM_BUSBAR_SEAL1='" + REM_BUSBAR_SEAL1 + "',REM_BUSBAR_SEAL2='" + REM_BUSBAR_SEAL2 + "',BB_CABLE_NOT_INSTALL_REASON='" + BB_CABLE_NOT_INSTALL_REASON + "',CABLE_REQD1='" + CABLE_REQD + "',";
        sqlinsert += " CABLE_LEN_USED='" + CABLE_LEN_USED + "',CABLEINSTALLTYPE='" + CABLEINSTALLTYPE + "',CABLE_REQD='" + CABLE_REQD1 + "',OUTPUT_CABLE_LEN_USED='" + OUTPUT_CABLE_LEN_USED + "',CAB_REMOVE_FRM_SITE='" + CAB_REMOVE_FRM_SITE + "',ELCB_INSTALLED='" + ELCB_INSTALLED + "',DURM_NO='" + DURM_NO + "',";
        sqlinsert += " CABLESIZE_OLD='" + CABLESIZE_OLD + "',CABLELENGTH_OLD='" + CABLELENGTH_OLD + "',MCR_NO='" + MCR_NO + "',REMARKS='" + REMARKS + "',CABLESIZE2='" + CABLESIZE2 + "',CABLELENGTH='" + CABLELENGTH + "',OUTPUTCABLESIZE='" + OUTPUTCABLESIZE + "',OUTPUTCABLELENGTH='" + OUTPUTCABLELENGTH + "',";
        sqlinsert += " RUNNINGLENGTHFROM='" + RUNNINGLENGTHFROM + "',RUNNINGLENGTHTO='" + RUNNINGLENGTHTO + "',CORD_INSTALLED='" + CABLELENGTH_OLD1 + "',CABLENOTINSTALLREASON='" + CABLENOTINSTALLREASON + "',REM_BOX_SEAL1='" + REM_BOX_SEAL1 + "',REM_BOX_SEAL2='" + REM_BOX_SEAL2 + "',REM_OTHER_SEAL='" + REM_OTHER_SEAL + "',";
        sqlinsert += " EARTHING_CONNECTOR=" + Convert.ToInt32(EARTHING_CONNECTOR) + ",JUBLIEE_CLAMPS='" + JUBLIEE_CLAMPS + "',HELPERNAME='" + HELPERNAME + "',CLOSEHOOKBOLT='" + CLOSEHOOKBOLT + "',NYLON_TIE=" + Convert.ToInt32(NYLON_TIE) + ",FASTNER='" + FASTNER + "',POLE_CONDITION='" + POLE_CONDITION + "',SADDLES=" + Convert.ToInt32(SADDLES) + ",HAZARDOUS_TYPE='" + HAZARDOUS_TYPE + "',";
        sqlinsert += " NOS_CBLAT_POLE='" + NOS_CBLAT_POLE + "',ADDITIONAL_POLE_REQUIRED='" + ADDITIONAL_POLE_REQUIRED + "',IS_RECORD_PROCESSED='" + IS_RECORD_PROCESSED + "',ADDITIONAL_POLE_NUMBER='" + ADDITIONAL_POLE_NUMBER + "',SERVICE_PROVIDER='" + SERVICE_PROVIDER + "',DRIVER_NAME='" + DRIVER_NAME + "',SUPERVISOR_NAME='" + SUPERVISOR_NAME + "',NOOFMETERS='" + NOOFMETERS + "',";
        sqlinsert += " CONNECTEDMETERS='" + CONNECTEDMETERS + "',OTHERSTICKER='" + OTHERSTICKER + "',DB_TYPE='" + DB_TYPE + "',GUNNYBAG_OLD='" + GUNNYBAG_OLD + "',GUNNYBAGSEAL_OLD='" + GUNNYBAGSEAL_OLD + "',LAB_TSTNG_NTC='" + LAB_TSTNG_NTC + "',IS_GNY_BAG_PREPD='" + IS_GNY_BAG_PREPD + "',PIERCING_CONNECTOR=" + Convert.ToInt32(PIERCING_CONNECTOR) + ",";
        sqlinsert += " PVC_GLAND =" + Convert.ToInt32(PVC_GLAND) + ", THIMBLE=" + Convert.ToInt32(THIMBLE) + ", ANCHOR_POLE_END_QTY='" + ANCHOR_POLE_END_QTY + "',UPDATED_BY='" + Userid + "',UPDATED_DATE=SYSDATE  WHERE ORDERID='" + ORDERID + "'";
        return objUti.ExecuteNonQuery(sqlinsert);
    }
    public DataTable GetItemDetails(string Itemcode, string ItemDescription, string periodFrom, string periodTo, string Status)
    {
        string query = "SELECT  ITEM_ID,ITEMCODE,DESCRIPTION,UNIT,UNITRATE,TO_CHAR(PERIOD_FROM,'DD/MM/YYYY') AS PERIODFROM,DECODE(ISACTIVE,'Y','Active','N','In-Active') ISACTIVE FROM MOBINT.MMG_ITEM_MASTER";
        if ((!String.IsNullOrEmpty(periodFrom)) && (!String.IsNullOrEmpty(periodTo)))
        {
            query += " WHERE TRUNC(PERIOD_FROM) BETWEEN TO_DATE('" + periodFrom + "','dd/MM/yyyy') AND  TO_DATE('" + periodTo + "','dd/MM/yyyy')";
        }
        if (!String.IsNullOrEmpty(ItemDescription))
            query += " AND UPPER(TRIM(DESCRIPTION)) LIKE '%" + ItemDescription + "%'";
        if (!String.IsNullOrEmpty(Itemcode))
        {
            query += " AND UPPER(TRIM(ITEMCODE)) LIKE '%" + Itemcode + "%'";
        }
        if (!String.IsNullOrEmpty(Status))
        {
            query += " AND ISACTIVE = '" + Status + "'";
        }
        query += " ORDER BY ITEMCODE ASC";
        DataTable dt = objUti.ExecuteReader(query);
        return dt;
    }
    public DataTable GetItemDetails(string _gItemcode)
    {
        string sql = "SELECT ITEM_ID,ITEMCODE,DESCRIPTION,UNIT,UNITRATE,PERIOD_FROM FROM MOBINT.MMG_ITEM_MASTER WHERE ITEMCODE='" + _gItemcode + "'";
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }
    public int Insert_Item_Data(string Itemcode, string ItemDesc, string Unit, string Unitrate, string Fromdate)
    {
        string sqlinsert = "INSERT INTO MOBINT.MMG_ITEM_MASTER(ITEM_ID,ITEMCODE,DESCRIPTION,UNIT,UNITRATE,PERIOD_FROM) values";
        sqlinsert += "(Item_seq.nextval,'" + Itemcode + "','" + ItemDesc + "','" + Unit + "','" + Unitrate + "',TO_DATE('" + Fromdate + "','dd/MM/yyyy'))";
        return objUti.ExecuteNonQuery(sqlinsert);
    }
    public int Update_Item_Data(string Itemcode, string ItemDesc, string Unit, string Unitrate, string Fromdate)
    {
        string sqlinsert = "UPDATE MOBINT.MMG_ITEM_MASTER SET DESCRIPTION='" + ItemDesc + "',UNIT='" + Unit + "',UNITRATE='" + Unitrate + "',PERIOD_FROM=to_date('" + Fromdate + "','dd/MM/yyyy') where ITEMCODE='" + Itemcode + "' and ISACTIVE='Y'";
        return objUti.ExecuteNonQuery(sqlinsert);
    }
    public int Insert_Generate_Invoice(string VendorId, string Division, string Type, string Month, string Year, string BillNo, string WorkOrder)
    {
        return objUti.ExcuteProceudre(VendorId, Division, Type, Month, Year, BillNo, WorkOrder);
    }
    public int Generate_Annexure1(string VendorId, string Division, string Type, string Month, string Year, string BillNo, string WorkOrder)
    {
        return objUti.ExcuteAnnexure1Proceudre(VendorId, Division, Type, Month, Year, BillNo, WorkOrder);
    }
    public int Generate_Annexure2(string VendorId, string Division, string Type, string Month, string Year, string BillNo, string WorkOrder)
    {
        return objUti.ExcuteAnnexure2Proceudre(VendorId, Division, Type, Month, Year, BillNo, WorkOrder);
    }
    public int Generate_Scrap_Verificaion(string VendorId, string Division, string Type, string Month, string Year, string BillNo, string WorkOrder)
    {
        return objUti.ExcuteScrapProceudre(VendorId, Division, Type, Month, Year, BillNo, WorkOrder);
    }

    public int Generate_Masterdata()
    {
        return objUti.ExcuteMasterProceudre();
    }
    public DataTable GetDetailsMIS(string _sOrdNo, string _sOrdertype, string _sDivision, string _sVendor,
                                    string _sFrmDT, string _sToDT)
    {
        string sql = " SELECT ORDER_CREATION_ID,COMPANY_CODE,DIVISION,VENDOR_CODE,ORDERID, ";
        sql += " ACTIVITY_DATE,ORDER_TYPE,PM_ACTIVITY,ACCOUNT_CLASS,PLANNER_GROUP,BASIC_START_DATE,BASIC_FINISH_DATE,CUSTOMER_NAME, ";
        sql += " ACTIVITY_REASON,MOBILE_NO,BP_NO,CA,DEVICENO,METER_PHASE,BOX_TYPE,BOX_NO,ADDRESS,TERMINAL_SEAL1,TERMINAL_SEAL2, ";
        sql += " METERBOXSEAL1,METERBOXSEAL2,BUSBARSEAL1,BUSBARSEAL2,INSTALLEDBUSBAR,BB_CABLE_USED,BB_CAB_REMOVE_FRM_SITE,BUSBARSIZE, ";
        sql += "  BUS_BAR_NO,BUS_BAR_DRUM_NO,B_BAR_CABLE_SIZE,RMVD_BB_CBL_LENTH,BUS_BAR_CABLE_LENG,RUNNING_LENGTH_FROM_BB, ";
        sql += " RUNNING_LENGTH_TO_BB,RMVD_BB_CBL_SIZE,REM_BUSBAR_SEAL1,REM_BUSBAR_SEAL2,BB_CABLE_NOT_INSTALL_REASON,CABLE_REQD1, ";
        sql += " CABLE_LEN_USED,CABLEINSTALLTYPE,CABLE_REQD,OUTPUT_CABLE_LEN_USED,CAB_REMOVE_FRM_SITE,ELCB_INSTALLED,DURM_NO, ";
        sql += " CABLESIZE_OLD,CABLELENGTH_OLD,MCR_NO,REMARKS,CABLESIZE2,CABLELENGTH,OUTPUTCABLESIZE,OUTPUTCABLELENGTH, ";
        sql += " RUNNINGLENGTHFROM,RUNNINGLENGTHTO,CORD_INSTALLED,CABLENOTINSTALLREASON,REM_BOX_SEAL1,REM_BOX_SEAL2,REM_OTHER_SEAL, ";
        sql += " EARTHING_CONNECTOR,JUBLIEE_CLAMPS,HELPERNAME,CLOSEHOOKBOLT,NYLON_TIE,FASTNER,POLE_CONDITION,SADDLES,HAZARDOUS_TYPE, ";
        sql += "   NOS_CBLAT_POLE,ADDITIONAL_POLE_REQUIRED,IS_RECORD_PROCESSED,ADDITIONAL_POLE_NUMBER,SERVICE_PROVIDER,DRIVER_NAME,SUPERVISOR_NAME,NOOFMETERS, ";
        sql += "  CONNECTEDMETERS,OTHERSTICKER,DB_TYPE,GUNNYBAG_OLD,GUNNYBAGSEAL_OLD,LAB_TSTNG_NTC,IS_GNY_BAG_PREPD,VENDOR_NAME,MONTH,YEAR, ";
        sql += "  REM_CABLE_LEN,PIERCING_CONNECTOR,PVC_GLAND,THIMBLE,DECODE(PUNCH_STATUS,'A','Tab','M','Manual') PUNCH_STATUS from mobint.MMG_ORDER_CREATION_DETAILS where trunc(ACTIVITY_DATE) between TO_DATE('" + _sFrmDT + "','dd/MM/yyyy') and TO_DATE('" + _sToDT + "','dd/MM/yyyy') ";

        if (!String.IsNullOrEmpty(_sOrdNo))
        {
            sql += " AND UPPER(TRIM(ORDERID)) LIKE '%" + _sOrdNo + "%'";
        }
        sql += " AND APPROVAL_STATUS = 'P'";

        if ((!String.IsNullOrEmpty(_sOrdertype)) && (_sOrdertype != "0"))
        {
            sql += " AND PUNCH_STATUS = '" + _sOrdertype + "'";
        }
        if ((!String.IsNullOrEmpty(_sDivision)) && (_sDivision != "0"))
        {
            sql += " AND DIVISION = '" + _sDivision + "'";
        }
        if ((!String.IsNullOrEmpty(_sVendor)) && (_sVendor != "0"))
        {
            sql += " AND VENDOR_CODE = '" + _sVendor + "'";
        }

        sql += " ORDER BY ORDER_CREATION_ID DESC";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public int Delete_Old_Invoice_Data(string Itemcode, string Month, string Year, string Vendor)
    {
        string sqlinsert = "DELETE FROM MOBINT.TRN_WORK_MEASUREMENT_BASE WHERE ITEMCODE='" + Itemcode + "' AND VENDORCODE='" + Vendor + "' AND MONTH='" + Month + "' AND YEAR='" + Year + "'";
        return objUti.ExecuteNonQuery(sqlinsert);
    }
    public int Delete_Old_Annexure2(string Itemcode, string Month, string Year, string Vendor)
    {
        string sqlinsert = "DELETE FROM MOBINT.TRN_WORK_MEASUREMENT_BASE WHERE ITEMCODE='" + Itemcode + "' AND VENDORCODE='" + Vendor + "' AND MONTH='" + Month + "' AND YEAR='" + Year + "'";
        return objUti.ExecuteNonQuery(sqlinsert);
    }
    //public int Delete_Old_Invoice_Data_1(string Itemcode, string Month, string Year, string Vendor, string Bill, string WorkOrder)
    //{
    //    string sqlinsert = "DELETE FROM WORK_MEASUREMENT_BASE WHERE ITEMCODE='" + Itemcode + "' AND VENDORCODE='" + Vendor + "' AND MONTH='" + Month + "' AND YEAR='" + Year + "' AND UPPER(BILLHEADERID)=UPPER('" + Bill + "') AND UPPER(WORKORDER_NO)=UPPER('" + WorkOrder + "')";
    //    return objUti.ExecuteNonQuery(sqlinsert);
    //}
    public int Delete_Old_Invoice_Data_1(string Month, string Year, string Vendor, string Bill, string WorkOrder)
    {
        string sqlinsert = "DELETE FROM MOBINT.WORK_MEASUREMENT_BASE";
        return objUti.ExecuteNonQuery(sqlinsert);
    }
    public int Delete_Old_Annexure2_1(string Month, string Year, string Vendor, string Bill, string WorkOrder)
    {
        string sqlinsert = "DELETE FROM MOBINT.MMG_MATERIAL_CONSUMPTION";
        return objUti.ExecuteNonQuery(sqlinsert);
    }
    public int Delete_Old_Scrap_Verification(string Month, string Year, string Vendor, string Bill, string WorkOrder)
    {
        string sqlinsert = "DELETE FROM MOBINT.MMG_SCRAP_VERIFICATION_FOOTER";
        return objUti.ExecuteNonQuery(sqlinsert);
    }
    public int Insert_ModifyInvoce_Data(string Itemcode, string Unitrate, string Quantity1, string Amount, string Quantity2, string Month, string Year, string Vendorid, string userid, string Itemcode1)
    {
        string sqlinsert = "INSERT INTO MOBINT.TRN_WORK_MEASUREMENT_BASE(MONTH,YEAR,VENDORCODE,ITEMCODE,QUANTITY,CREATEDBY) values";
        sqlinsert += "('" + Month + "','" + Year + "','" + Vendorid + "',";
        if (Itemcode == "NA1" || Itemcode == "NA2" || Itemcode == "NA3" || Itemcode == "3012321" || Itemcode == "4060121" || Itemcode == "4060663" || Itemcode == "4060664")
        {
            sqlinsert += " '" + Itemcode1 + "','" + Quantity2 + "',";
        }
        else
        {
            sqlinsert += " '" + Itemcode + "','" + Quantity1 + "',";
        }
        sqlinsert += "'" + userid + "')";
        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int Insert_ModifyAnnexure2_Data(string ItemId, string prevMonthBalance, string receivedFromOtherDivision,
        string removedFromSite, string receivedFromStore, string total, string quantityConsumed, string returnToStore,
        string transferToOtherDivision, string balance, string remarks, string month, string year, string VendorId,
        string UserId, string Itemname, string Unit)
    {
        string sqlinsert = "INSERT INTO MOBINT.TRN_MATERIAL_CONSUMPTION(MONTH,YEAR,VENDORCODE,ITEMCODE,CONSUMPTION,CREATEDBY) values";
        sqlinsert += "('" + month + "','" + year + "','" + VendorId + "','" + ItemId + "','" + quantityConsumed + "','" + UserId + "')";
        return objUti.ExecuteNonQuery(sqlinsert);
    }
    public int Insert_ModifyInvoce_Log(string Division, string Type, string Month, string Year, string Bill_No, string Workorderno, string Vendorid, string BillType, string UserId)
    {
        string sqlinsert = "INSERT INTO MOBINT.MMG_BILL_DETAILS(BILL_ID, DIVISION, TYPE, MONTH, YEAR, BILLNO, WORKORDERNO, VENDORCODE, BILL_TYPE,GENERATED_BY)";
        sqlinsert += " VALUES(ITEM_SEQUENCE.NEXTVAL,'" + Division + "','" + Type + "','" + Month + "','" + Year + "','" + Bill_No + "','" + Workorderno + "','" + Vendorid + "','" + BillType + "','" + UserId + "')";
        return objUti.ExecuteNonQuery(sqlinsert);
    }
    public int Insert_ModifyInvoce_Data_1(string Itemcode, string Unitrate, string Quantity1, string Amount, string Quantity2, string Month, string Year, string Vendorid, string userid, string Itemcode1, string BillNo, string WorkOrder, string Desc, string Unit, string Amount1)
    {
        string sqlinsert = "INSERT INTO MOBINT.WORK_MEASUREMENT_BASE(MONTH,YEAR,VENDORCODE,ITEMCODE,QUANTITY,AMOUNT,CREATEDBY,BILLHEADERID,WORKORDER_NO,DESCRIPTION,UNIT,UNIT_RATE) values";
        sqlinsert += "('" + Month + "','" + Year + "','" + Vendorid + "',";
        if (Itemcode == "NA1" || Itemcode == "NA2" || Itemcode == "NA3" || Itemcode == "3012321" || Itemcode == "4060121" || Itemcode == "4060663" || Itemcode == "4060664")
        {
            sqlinsert += " '" + Itemcode1 + "','" + Quantity2 + "','" + Amount1 + "',";
        }
        else
        {
            sqlinsert += " '" + Itemcode + "','" + Quantity1 + "','" + Amount + "',";
        }
        sqlinsert += "'" + userid + "','" + BillNo + "','" + WorkOrder + "','" + Desc + "','" + Unit + "','" + Unitrate + "')";
        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int Insert_ModifyAnnexure2_Data_1(string ItemId, string prevMonthBalance, string receivedFromOtherDivision, string removedFromSite, string receivedFromStore,
        string total, string quantityConsumed, string returnToStore, string transferToOtherDivision, string balance, string remarks, string month, string year,
        string VendorId, string UserId, string Bill_No, string Workorder_No, string Itemname, string Unit)
    {
        string sqlinsert = "INSERT INTO MOBINT.MMG_MATERIAL_CONSUMPTION(MONTH,YEAR,VENDORCODE,CONSUMPTION,ITEMID,PREVIOUS_MONTH_BALANCE,RECEIVED_FROM_DIVISION,REMOVED_FROM_SITE,";
        sqlinsert += "RECEIVED_FROM_STORE,TOTAL,RETURNED_TO_STORE,TRANSFER_TO_DIVISION,BALANCE,REMARKS,BILL_NO,WORKORDER_NO,CREATEDBY,ITEM_NAME,UNIT) values";
        sqlinsert += "('" + month + "','" + year + "','" + VendorId + "','" + quantityConsumed + "','" + ItemId + "','" + prevMonthBalance + "','" + receivedFromOtherDivision + "',";
        sqlinsert += "'" + removedFromSite + "','" + receivedFromStore + "','" + total + "','" + returnToStore + "','" + transferToOtherDivision + "','" + balance + "','" + remarks + "',";
        sqlinsert += "'" + Bill_No + "','" + Workorder_No + "','" + UserId + "','" + Itemname + "','" + Unit + "')";
        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int Update_Penalty_Data(int PenaltyId, string Amount, string UserId)
    {
        string sqlinsert = "UPDATE MOBINT.MMG_PENALTY_MASTER SET AMOUNT='" + Amount + "',UPDATED_BY='" + UserId + "',UPDATED_DATE=SYSDATE WHERE PENALTYID=" + PenaltyId + " AND STATUS='Y'";
        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int Insert_ScrapVerification_Data(string MONTH, string YEAR, string VENDORCODE, string JOB_DESCRIPTION,
string PREVBAL_CONSUMED_CABLE_2X10, string PREVBAL_CONSUMED_CABLE_2X25, string PREVBAL_CONSUMED_CABLE_4X25, string PREVBAL_CONSUMED_CABLE_4X50, string PREVBAL_CONSUMED_CABLE_4X150,
string PREVBAL_SCRAP_CABLE_2X10, string PREVBAL_SCRAP_CABLE_2X25, string PREVBAL_SCRAP_CABLE_4X25, string PREVBAL_SCRAP_CABLE_4X50, string PREVBAL_SCRAP_CABLE_4X150, string CABLEREUSED_CONSUMED_CABLE_2X10,
string CABLEREUSED_CONSUMED_CABLE_2X25, string CABLEREUSED_CONSUMED_CABLE_4X25, string CABLEREUSED_CONSUMED_CABLE_4X50, string CABLEREUSED_CONSUMED_CABLE_4X150, string CABLEREUSED_SCRAP_CABLE_2X10,
string CABLEREUSED_SCRAP_CABLE_2X25, string CABLEREUSED_SCRAP_CABLE_4X25, string CABLEREUSED_SCRAP_CABLE_4X50, string CABLEREUSED_SCRAP_CABLE_4X150, string OLDCABLE_CONSUMED_CABLE_2X10,
string OLDCABLE_CONSUMED_CABLE_2X25, string OLDCABLE_CONSUMED_CABLE_4X25, string OLDCABLE_CONSUMED_CABLE_4X50, string OLDCABLE_CONSUMED_CABLE_4X150, string OLDCABLE_SCRAP_CABLE_2X10,
string OLDCABLE_SCRAP_CABLE_2X25, string OLDCABLE_SCRAP_CABLE_4X25, string OLDCABLE_SCRAP_CABLE_4X50, string OLDCABLE_SCRAP_CABLE_4X150, string NEWCABLE_CONSUMED_CABLE_2X10,
string NEWCABLE_CONSUMED_CABLE_2X25, string NEWCABLE_CONSUMED_CABLE_4X25, string NEWCABLE_CONSUMED_CABLE_4X50, string NEWCABLE_CONSUMED_CABLE_4X150, string NEWCABLE_SCRAP_CABLE_2X10,
string NEWCABLE_SCRAP_CABLE_2X25, string NEWCABLE_SCRAP_CABLE_4X25, string NEWCABLE_SCRAP_CABLE_4X50, string NEWCABLE_SCRAP_CABLE_4X150,
string REMARKS, string Userid)
    {
        string sqlinsert = "INSERT INTO MOBINT.MMG_SCRAP_VERIFICATION_FOOTER(MONTH,YEAR,VENDORCODE,JOB_DESCRIPTION,PREVBAL_CONSUMED_CABLE_2X10,";
        sqlinsert += "PREVBAL_CONSUMED_CABLE_2X25,PREVBAL_CONSUMED_CABLE_4X25,PREVBAL_CONSUMED_CABLE_4X50,PREVBAL_CONSUMED_CABLE_4X150,PREVBAL_SCRAP_CABLE_2X10,PREVBAL_SCRAP_CABLE_2X25,";
        sqlinsert += "PREVBAL_SCRAP_CABLE_4X25,PREVBAL_SCRAP_CABLE_4X50,PREVBAL_SCRAP_CABLE_4X150,CABLEREUSED_CONSUMED_CABLE_2X10,CABLEREUSED_CONSUMED_CABLE_2X25,CABLEREUSED_CONSUMED_CABLE_4X25,";
        sqlinsert += "CABLEREUSED_CONSUMED_CABLE_4X50,CABLEREUSED_CONSUMED_CABLE_4X150,CABLEREUSED_SCRAP_CABLE_2X10,CABLEREUSED_SCRAP_CABLE_2X25,CABLEREUSED_SCRAP_CABLE_4X25,CABLEREUSED_SCRAP_CABLE_4X50,";
        sqlinsert += "CABLEREUSED_SCRAP_CABLE_4X150,OLDCABLE_CONSUMED_CABLE_2X10,OLDCABLE_CONSUMED_CABLE_2X25,OLDCABLE_CONSUMED_CABLE_4X25,OLDCABLE_CONSUMED_CABLE_4X50,OLDCABLE_CONSUMED_CABLE_4X150,";
        sqlinsert += "OLDCABLE_SCRAP_CABLE_2X10,OLDCABLE_SCRAP_CABLE_2X25,OLDCABLE_SCRAP_CABLE_4X25,OLDCABLE_SCRAP_CABLE_4X50,OLDCABLE_SCRAP_CABLE_4X150,NEWCABLE_CONSUMED_CABLE_2X10,NEWCABLE_CONSUMED_CABLE_2X25,";
        sqlinsert += "NEWCABLE_CONSUMED_CABLE_4X25,NEWCABLE_CONSUMED_CABLE_4X50,NEWCABLE_CONSUMED_CABLE_4X150,NEWCABLE_SCRAP_CABLE_2X10,NEWCABLE_SCRAP_CABLE_2X25,NEWCABLE_SCRAP_CABLE_4X25,NEWCABLE_SCRAP_CABLE_4X50,";
        sqlinsert += "NEWCABLE_SCRAP_CABLE_4X150,REMARKS,CREATEDBY) values";
        sqlinsert += "('" + MONTH + "','" + YEAR + "','" + VENDORCODE + "','" + JOB_DESCRIPTION + "','" + PREVBAL_CONSUMED_CABLE_2X10 + "','" + PREVBAL_CONSUMED_CABLE_2X25 + "','" + PREVBAL_CONSUMED_CABLE_4X25 + "',";

        sqlinsert += "'" + PREVBAL_CONSUMED_CABLE_4X50 + "','" + PREVBAL_CONSUMED_CABLE_4X150 + "','" + PREVBAL_SCRAP_CABLE_2X10 + "','" + PREVBAL_SCRAP_CABLE_2X25 + "','" + PREVBAL_SCRAP_CABLE_4X25 + "',";

        sqlinsert += "'" + PREVBAL_SCRAP_CABLE_4X50 + "','" + PREVBAL_SCRAP_CABLE_4X150 + "','" + CABLEREUSED_CONSUMED_CABLE_2X10 + "','" + CABLEREUSED_CONSUMED_CABLE_2X25 + "','" + CABLEREUSED_CONSUMED_CABLE_4X25 + "',";

        sqlinsert += "'" + CABLEREUSED_CONSUMED_CABLE_4X50 + "','" + CABLEREUSED_CONSUMED_CABLE_4X150 + "','" + CABLEREUSED_SCRAP_CABLE_2X10 + "','" + CABLEREUSED_SCRAP_CABLE_2X25 + "','" + CABLEREUSED_SCRAP_CABLE_4X25 + "','" + CABLEREUSED_SCRAP_CABLE_4X50 + "',";

        sqlinsert += "'" + CABLEREUSED_SCRAP_CABLE_4X150 + "','" + OLDCABLE_CONSUMED_CABLE_2X10 + "','" + OLDCABLE_CONSUMED_CABLE_2X25 + "','" + OLDCABLE_CONSUMED_CABLE_4X25 + "','" + OLDCABLE_CONSUMED_CABLE_4X50 + "',";

        sqlinsert += "'" + OLDCABLE_CONSUMED_CABLE_4X150 + "','" + OLDCABLE_SCRAP_CABLE_2X10 + "','" + OLDCABLE_SCRAP_CABLE_2X25 + "','" + OLDCABLE_SCRAP_CABLE_4X25 + "','" + OLDCABLE_SCRAP_CABLE_4X50 + "','" + OLDCABLE_SCRAP_CABLE_4X150 + "',";

        sqlinsert += "'" + NEWCABLE_CONSUMED_CABLE_2X10 + "','" + NEWCABLE_CONSUMED_CABLE_2X25 + "','" + NEWCABLE_CONSUMED_CABLE_4X25 + "','" + NEWCABLE_CONSUMED_CABLE_4X50 + "','" + NEWCABLE_CONSUMED_CABLE_4X150 + "','" + NEWCABLE_SCRAP_CABLE_2X10 + "',";
        sqlinsert += "'" + NEWCABLE_SCRAP_CABLE_2X25 + "','" + NEWCABLE_SCRAP_CABLE_4X25 + "','" + NEWCABLE_SCRAP_CABLE_4X50 + "','" + NEWCABLE_SCRAP_CABLE_4X150 + "','" + REMARKS + "','" + Userid + "')";
        return objUti.ExecuteNonQuery(sqlinsert);
    }
}