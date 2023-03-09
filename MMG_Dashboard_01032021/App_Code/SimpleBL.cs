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

    #region Login_Page_SMSMaster

    public DataTable getResend_SMS(string _sOrderID)
    {
        string sql = "  SELECT Mobile_no, MESSAGE_TXT  FROM mobint.MCR_SMS_APP_DETAILS WHERE  APP_ID='" + _sOrderID + "' and MESSAGE_TXT like 'As per%' ";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public int UpdateResend_SMSDate(string _sOrderID)
    {
        string sqlinsert = " UPDATE MOBINT.MCR_SMS_APP_DETAILS SET resend_date=SYSDATE WHERE  APP_ID='" + _sOrderID + "' and MESSAGE_TXT like 'As per%' ";

        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public DataTable 
        getLoginDetails(string _gUserName, string _gPassword)
    {
        string sql = "SELECT a.emp_name, a.emp_id, imei_no, division, emp_type, ROLE, a.vendor_id, a.active_flag, b.login_id, PASSWORD, login_type, b.active_flag, a.COMPANY ";
        sql += " FROM MOBINT.MCR_USER_DETAILS a, MOBINT.MCR_LOGIN_MST b ";
        sql += " WHERE a.EMP_ID=b.LOGIN_ID ";
        sql += " AND a.ACTIVE_FLAG='Y' AND b.ACTIVE_FLAG='Y' AND upper(login_id)=upper('" + _gUserName + "') ";

        if (_gPassword != "")
            sql += " AND PASSWORD='" + _gPassword + "'";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public bool Insert_SMS_Details(string _sSMSID, string _sOrdNo, string _sMobile, string _sMessage, string _sDivision, string _sCompany)
    {
        try
        {
            string sqlinsert = " INSERT INTO  MOBINT.MCR_SMS_APP_DETAILS(SMS_ID, APP_ID, APP_NAME, COMPANY,DIVISION, MOBILE_NO, MESSAGE_TXT)";
            sqlinsert = sqlinsert + "    VALUES ( ";
            sqlinsert = sqlinsert + "    '" + _sSMSID + "','" + _sOrdNo + "','MCR','" + _sCompany + "','" + _sDivision + "'," + _sMobile + ",'" + _sMessage + "')";

            if (objUti.ExecuteNonQuery(sqlinsert) > 0)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {            
            return false;
        }
    }

    public DataTable getSMS_Text_OrderWise(string _sOrderType, string _sCompany, string _sDivision, string _sSMSType, string _sSMSSubType)
    {
        string sql = " SELECT MESSAGE_TXT, MESS_VER1 FROM MOBINT.SMS_APPLICATION_MST WHERE SMS_TYPE='" + _sOrderType + "' AND SMS_SUBTYPE='" + _sSMSSubType + "' AND STATUS='Y' AND UPPER(COMPANY)='" + _sCompany.ToUpper() + "'  AND UPPER(DIVISION) LIKE '%" + _sDivision.ToUpper() + "%' AND MESS_VER1='" + _sSMSType + "'";
        
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable GetInstaller_Details(string _sInstallerID)
    {
        string sql = " SELECT EMP_NAME,MOBILE_NO, COMPANY FROM MOBINT.MCR_USER_DETAILS WHERE emp_id='" + _sInstallerID + "' ";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    #endregion

    #region ChangePassword
    public int UpdatePassword(string _gUserName, string _gOldPassword, string _gPassword)
    {
        string sql = "UPDATE MOBINT.MCR_LOGIN_MST SET PASSWORD='" + _gPassword + "' WHERE LOGIN_ID ='" + _gUserName + "' AND PASSWORD='" + _gOldPassword + "'";

        int Result = objUti.ExecuteNonQuery(sql);
        return Result;
    }
    #endregion


    #region MCR Punching
    

    public DataTable Get_MCR_InputData_Details(string _gID, string _gDivision, string _gCompany, string _gFlag, string _gDDlDivision, 
                            string _gAddress, string _gPostingDate, string _gInstallerName, string _sMeterNo, string _sOrderNo, string _sBasicFinDate,
                            string _gPostingToDate, string _sOrderType, string _sActType, string _sRole)    //16032018
    {
        string sql = string.Empty;

        if (_gFlag == "C")
        {
            sql = " SELECT * FROM (";
            sql += " SELECT UNIQUE AUART,ILART_ACTIVITY_TYPE, UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.AUART = B.ORDER_TYPE)) ORDER_TYPE,";
            sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.ILART_ACTIVITY_TYPE = B.PM_ACTIVTY)) PM_ACT,";
            sql += "COMP_CODE, a.ORDERID, METER_NO, a.DIVISION, a.VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, REQUEST_TYPE, STICKER_NO,  PLANNER_GROUP, CABLE_SIZE, CABLE_LENGTH, a.ENTRY_DATE,TO_CHAR(PUNCH_DATE, 'dd/MM/yyyy') PunchDate,";
            sql += "(SELECT EMP_NAME FROM MOBINT.MCR_USER_DETAILS WHERE EMP_ID = (SELECT INSTALLER_ID FROM (SELECT * FROM MOBINT.MCR_VEND_ORDER_INST_MAP ORDER BY ENTRY_DATE DESC) WHERE order_no = A.ORDERID AND ROWNUM = 1)) AllotedTo,";
            sql += "SANCTIONED_LOAD, a.ACCOUNT_CLASS, to_char(PSTING_DATE, 'dd/MM/yyyy') PostingDate, (select reason from MOBINT.MCR_ORDER_CANCEL where orderid=a.ORDERID AND ROWNUM<2) Cancel_Reason, ";
            sql += " FINISH_DATE, TO_CHAR(FINISH_DATE, 'dd/MM/yyyy') FIN_DATE,";
            sql += "  OLD_M_READING,OLD_MR_KWH,OLD_MR_KW,MRKVAH_OLD,OLD_MR_KVA,INSTALLEDCABLE_OLD,CABLESIZE_OLD,     ";
            sql += "  DRUMSIZE_OLD,CABLEINSTALLTYPE_OLD,RUNNINGLENGTHFROM_OLD,RUNNINGLENGTHTO_OLD,CABLELENGTH_OLD,OUTPUTBUSLENGTH_OLD,REM_TERMINAL_SEAL, ";
            sql += "  REM_OTHER_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2, REM_BUSBAR_SEAL1,REM_BUSBAR_SEAL2,BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,  ";
            sql += "  BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD,GUNNYBAGSEAL_OLD,LABTESTING_DATE_OLD,METERRELOCATE_OLD, ";
            sql += " TO_CHAR(CONTRACT_ST_DT, 'dd/MM/yyyy') CONTRACT_SD,TO_CHAR(CONTRACT_END_DT, 'dd/MM/yyyy') CONTRACT_ED,(TRUNC(CONTRACT_END_DT)-TRUNC(CONTRACT_ST_DT))DURATION ";
            sql += " FROM MOBINT.MCR_INPUT_DETAILS a, MOBINT.MCR_DETAILS b WHERE a.ORDERID=b.ORDERID "; //AND  SUBSTR(CA_NO,1,4) !='0003'    ";

            if (_gID != "" && _sRole == "V")    //21032018
                sql += " AND a.VENDOR_CODE ='" + _gID + "' ";

            if (_gDDlDivision != "")
                sql += " AND a.division IN('" + _gDDlDivision + "') ";
            else
                sql += " AND a.division IN('" + _gDivision + "')  ";

            sql += " AND comp_code='" + _gCompany + "' AND FLAG='" + _gFlag + "'";

            if (_gAddress != "")
                sql += "AND upper(address) LIKE upper('%" + _gAddress + "%') ";

            if (_sBasicFinDate != "")
                sql += " AND trunc(FINISH_DATE)='" + _sBasicFinDate + "' ";

            if ((_gPostingDate != "") && (_gPostingToDate != ""))
                sql += " AND  TRUNC(PUNCH_DATE) BETWEEN  TO_DATE('" + _gPostingDate + "') AND  TO_DATE('" + _gPostingToDate + "')";

            if (_sMeterNo != "")
                sql += " AND METER_NO like '%" + _sMeterNo + "%' ";
            if (_sOrderNo != "")
                sql += " AND a.ORDERID like '%" + _sOrderNo + "%'";

            //if (Convert.ToInt32(_sRoleCheck) > 1 || _sRole == "PV")
            //{
            if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
                sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";  //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";   //AND ROLE_TYPE = '" + _sRole + "'
            //}

            sql += ") WHERE 1=1";

            if (_gInstallerName != "")
                sql += " AND AllotedTo='" + _gInstallerName + "' ";
        }
        else
        {
            sql = "SELECT * FROM (";
            sql += " SELECT UNIQUE AUART,ILART_ACTIVITY_TYPE, UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.AUART = B.ORDER_TYPE)) ORDER_TYPE,";
            sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.ILART_ACTIVITY_TYPE = B.PM_ACTIVTY)) PM_ACT,";
            sql += "COMP_CODE, ORDERID, METER_NO, DIVISION, VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, REQUEST_TYPE, STICKER_NO,  PLANNER_GROUP, CABLE_SIZE, CABLE_LENGTH, ENTRY_DATE,TO_CHAR(PUNCH_DATE, 'dd/MM/yyyy') PunchDate,";
            sql += "(SELECT EMP_NAME FROM MOBINT.MCR_USER_DETAILS WHERE EMP_ID = (SELECT INSTALLER_ID FROM (SELECT * FROM MOBINT.MCR_VEND_ORDER_INST_MAP ORDER BY ENTRY_DATE DESC) WHERE order_no = ORDERID AND ROWNUM = 1)) AllotedTo,";
            sql += "SANCTIONED_LOAD, ACCOUNT_CLASS, to_char(PSTING_DATE, 'dd/MM/yyyy') PostingDate, (select reason from MOBINT.MCR_ORDER_CANCEL where orderid=a.ORDERID AND ROWNUM<2) Cancel_Reason, ";
            sql += " FINISH_DATE, TO_CHAR(FINISH_DATE, 'dd/MM/yyyy') FIN_DATE ,";

            sql += "  '' OLD_M_READING,'' OLD_MR_KWH,'' OLD_MR_KW,'' MRKVAH_OLD,'' OLD_MR_KVA,'' INSTALLEDCABLE_OLD,'' CABLESIZE_OLD,     ";
            sql += "  '' DRUMSIZE_OLD,'' CABLEINSTALLTYPE_OLD,'' RUNNINGLENGTHFROM_OLD,'' RUNNINGLENGTHTO_OLD,'' CABLELENGTH_OLD,'' OUTPUTBUSLENGTH_OLD,'' REM_TERMINAL_SEAL, ";
            sql += " '' REM_OTHER_SEAL, '' REM_BOX_SEAL1,'' REM_BOX_SEAL2, '' REM_BUSBAR_SEAL1,'' REM_BUSBAR_SEAL2,'' BOX_OLD,'' GLANDS_OLD,'' TCOVER_OLD,'' BRASSSCREW_OLD,  ";
            sql += "  '' BUSBAR_OLD,'' THIMBLE_OLD,'' SADDLE_OLD,'' GUNNYBAG_OLD,'' GUNNYBAGSEAL_OLD,'' LABTESTING_DATE_OLD,'' METERRELOCATE_OLD, ";
            sql += " TO_CHAR(CONTRACT_ST_DT, 'dd/MM/yyyy') CONTRACT_SD,TO_CHAR(CONTRACT_END_DT, 'dd/MM/yyyy') CONTRACT_ED,(TRUNC(CONTRACT_END_DT)-TRUNC(CONTRACT_ST_DT))DURATION ";
            sql += " FROM MOBINT.MCR_INPUT_DETAILS a WHERE 1=1";//SUBSTR(CA_NO,1,4) !='0003'  ";

            if (_gID != "" && _sRole == "V")
                sql += " AND VENDOR_CODE ='" + _gID + "' ";

            if (_gDDlDivision != "")
                sql += " AND division IN('" + _gDDlDivision + "') ";
            else
                sql += " AND division IN('" + _gDivision + "')  ";

            sql += " AND comp_code='" + _gCompany + "' AND FLAG='" + _gFlag + "'";

            if (_gAddress != "")
                sql += "AND upper(address) LIKE upper('%" + _gAddress + "%') ";

            if (_sBasicFinDate != "")
                sql += " AND trunc(FINISH_DATE)='" + _sBasicFinDate + "' ";

            if ((_gPostingDate != "") && (_gPostingToDate != ""))
                sql += " AND  TRUNC(PSTING_DATE) BETWEEN  TO_DATE('" + _gPostingDate + "') AND  TO_DATE('" + _gPostingToDate + "')";

            if (_sMeterNo != "")
                sql += " AND METER_NO like '%" + _sMeterNo + "%' ";
            if (_sOrderNo != "")
                sql += " AND ORDERID like '%" + _sOrderNo + "%'";

            if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
                sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";  //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";   //AND ROLE_TYPE = '" + _sRole + "'

            sql += ") WHERE 1=1";

            if (_gInstallerName != "")
                sql += " AND AllotedTo='" + _gInstallerName + "' ";

            //sql += " AND ROWNUM<10";
        }

        DataTable dt = new DataTable();
        dt = objUti.ExecuteReader(sql);

        return dt;

    }

    public DataTable Get_MCR_InputData_CancelDetails(string _gDivision, string _gCompany, string _gFlag, string _gDDlDivision,
                           string _gAddress, string _gPostingDate, string _gInstallerName, string _sMeterNo, string _sOrderNo, string _sBasicFinDate,
                           string _gPostingToDate, string _sOrderType, string _sActType)
    {
        string sql = string.Empty;

        sql = "SELECT * FROM (";
        sql += " SELECT AUART ORDER_TYPE, COMP_CODE, ORDERID, METER_NO, DIVISION, VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, REQUEST_TYPE, STICKER_NO,  PLANNER_GROUP, CABLE_SIZE, CABLE_LENGTH, ENTRY_DATE,TO_CHAR(PUNCH_DATE, 'dd/MM/yyyy') PunchDate,(SELECT EMP_NAME FROM MOBINT.MCR_USER_DETAILS WHERE EMP_ID=(SELECT  INSTALLER_ID FROM MOBINT.MCR_VEND_ORDER_INST_MAP  WHERE order_no=a.ORDERID AND ROWNUM='1')) AllotedTo, SANCTIONED_LOAD, ACCOUNT_CLASS, to_char(PSTING_DATE, 'dd/MM/yyyy') PostingDate, (select reason from MOBINT.MCR_ORDER_CANCEL where orderid=a.ORDERID  AND ROWNUM='1' ) Cancel_Reason, ";
        sql += " FINISH_DATE, TO_CHAR(FINISH_DATE, 'dd/MM/yyyy') FIN_DATE FROM MOBINT.MCR_INPUT_DETAILS a WHERE 1=1  ";

        if (_sOrderType != "-ALL-")
            sql += " AND AUART ='" + _sOrderType + "'";
        else
            sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        if (_sActType != "-ALL-")
            sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
        else
            sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        if (_gDDlDivision != "")
        {
            sql += " AND VENDOR_CODE in (SELECT VENDOR_CODE FROM  MOBINT.MCR_DIVISION WHERE DIST_CD IN ('" + _gDDlDivision + "'))";
            sql += " AND division IN('" + _gDDlDivision + "') ";
        }
        else
        {
            sql += " AND VENDOR_CODE in (SELECT VENDOR_CODE FROM  MOBINT.MCR_DIVISION WHERE DIST_CD IN('" + _gDivision + "') )";
            sql += " AND division IN('" + _gDivision + "')  ";
        }

        sql += " AND comp_code='" + _gCompany + "' AND FLAG='" + _gFlag + "'";

        if (_gAddress != "")
            sql += "AND upper(address) LIKE upper('%" + _gAddress + "%') ";

        if (_sBasicFinDate != "")
            sql += " AND trunc(FINISH_DATE)='" + _sBasicFinDate + "' ";

        if ((_gPostingDate != "") && (_gPostingToDate != ""))
            sql += " AND  TRUNC(PSTING_DATE) BETWEEN  TO_DATE('" + _gPostingDate + "') AND  TO_DATE('" + _gPostingToDate + "')";

        if (_sMeterNo != "")
            sql += " AND METER_NO like '%" + _sMeterNo + "%' ";
        if (_sOrderNo != "")
            sql += " AND ORDERID like '%" + _sOrderNo + "%'";

        sql += ") WHERE 1=1";

        if (_gInstallerName != "")
            sql += " AND AllotedTo='" + _gInstallerName + "' ";

        DataTable dt = new DataTable();
        dt = objUti.ExecuteReader(sql);

        return dt;

    }

   

    public int Update_Order_Status(string _sUpdateBy, string _sUpdateReason, string _sFlag, string _sOrderID, string _sMeterNo)
    {
        string sqlinsert = " UPDATE MOBINT.MCR_INPUT_DETAILS SET REALOC_DEL_BY='" + _sUpdateBy + "',REALOC_DEL_REASON='" + _sUpdateReason + "',REALOC_DEL_DATE=SYSDATE,flag='" + _sFlag + "',PUNCH_DATE=NULL, PUNCH_BY=NULL WHERE ORDERID ='" + _sOrderID + "' AND METER_NO='" + _sMeterNo + "'  ";

        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int Delete_Order_Status(string _sOrderID, string _sMeterNo)
    {
        string sqlDelete = " DELETE FROM MOBINT.MCR_VEND_ORDER_INST_MAP WHERE ORDER_NO='" + _sOrderID + "' AND METER_NO='" + _sMeterNo + "' ";

        return objUti.ExecuteNonQuery(sqlDelete);
    }

    public DataTable getOrderCancel_Type()
    {
        string sql = " 	SELECT ID, NAME FROM	MOBINT.MCR_COR_SYS_MST WHERE COR_SYS_TYPE='CAN_STATUS'  ";
        
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }
    

    public DataTable getEmpDetails(string _gID, string _gDivision, string _gCompany)    //03042018
    {
        string sql = " SELECT UPPER(EMP_NAME) EMPNAME, EMP_ID, (EMP_ID ||'-' || UPPER(EMP_NAME)) EMPLOYEE_NAME, (SELECT NAME FROM MOBINT.MCR_COR_SYS_MST WHERE COR_SYS_TYPE='ASSIGNEDAREA' AND ID=ASSIGNEDAREA AND ACTIVE='Y' ) ASSIGNEDAREA, (SELECT COUNT(1) FROM MOBINT.MCR_INPUT_DETAILS b, MOBINT.MCR_VEND_ORDER_INST_MAP c WHERE b.orderid=c.ORDER_NO AND b.FLAG='Y' AND INSTALLER_ID=a.EMP_ID) MeterAlloted, ";
        sql += "  (SELECT COUNT(1) FROM MOBINT.MCR_SEAL_DETAILS WHERE CONSUM_SEAL_FLAG='Y' AND VENDOR_CODE=VENDOR_ID AND ALLOTED_TO=EMP_ID) SealAlloted ";
        sql += " FROM MOBINT.MCR_USER_DETAILS a WHERE a.EMP_TYPE='I' and a.ACTIVE_FLAG='Y'";
        if (_gID != "")
            sql += " AND  a.VENDOR_ID='" + _gID + "' ";

        if (_gDivision != "")
            sql += " and DIVISION IN ('" + _gDivision + "')";

        sql += "  ORDER BY EMPNAME ";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable getEmpDetailsNew(string _gID, string _gDivision, string _gCompany)    //16012020
    {
        string sql = " SELECT UPPER(EMP_NAME) EMPNAME, EMP_ID, (EMP_ID ||'-' || UPPER(EMP_NAME)) EMPLOYEE_NAME, ";
        sql += "  (SELECT COUNT(1) FROM MOBINT.MCR_SEAL_DETAILS WHERE seal_type is null AND CONSUM_SEAL_FLAG='Y' AND VENDOR_CODE=VENDOR_ID AND ALLOTED_TO=EMP_ID) SealAlloted ";
        sql += " FROM MOBINT.MCR_USER_DETAILS a WHERE a.EMP_TYPE='I' and a.ACTIVE_FLAG='Y'";
        if (_gID != "")
            sql += " AND  a.VENDOR_ID='" + _gID + "' ";

        if (_gDivision != "")
            sql += " and DIVISION IN ('" + _gDivision + "')";

        sql += "  ORDER BY EMPNAME ";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable getEmpDetailsNew_Gunny(string _gID, string _gDivision, string _gCompany)    //16012020
    {
        string sql = " SELECT UPPER(EMP_NAME) EMPNAME, EMP_ID, (EMP_ID ||'-' || UPPER(EMP_NAME)) EMPLOYEE_NAME,";
        sql += "  (SELECT COUNT(1) FROM MOBINT.MCR_SEAL_DETAILS WHERE SEAL_TYPE='GUNNY' AND CONSUM_SEAL_FLAG='Y' AND VENDOR_CODE=VENDOR_ID AND ALLOTED_TO=EMP_ID) SealAlloted ";
        sql += " FROM MOBINT.MCR_USER_DETAILS a WHERE a.EMP_TYPE='I' and a.ACTIVE_FLAG='Y'";
        if (_gID != "")
            sql += " AND  a.VENDOR_ID='" + _gID + "' ";

        if (_gDivision != "")
            sql += " and DIVISION IN ('" + _gDivision + "')";

        sql += "  ORDER BY EMPNAME ";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }   

    public DataTable getSealEmpDetails(string _gID, string _gDivision, string _gCompany)
    {
        string sql = " SELECT EMP_NAME EMPNAME, EMP_ID, (SELECT NAME FROM MOBINT.MCR_COR_SYS_MST WHERE COR_SYS_TYPE='ASSIGNEDAREA' AND ID=ASSIGNEDAREA AND ACTIVE='Y' ) ASSIGNEDAREA, (SELECT COUNT(1) FROM MOBINT.MCR_INPUT_DETAILS b, MOBINT.MCR_VEND_ORDER_INST_MAP c WHERE b.orderid=c.ORDER_NO AND b.FLAG='Y' AND INSTALLER_ID=a.EMP_ID) MeterAlloted, ";
        sql += "  (SELECT COUNT(1) FROM MOBINT.MCR_SEAL_DETAILS WHERE CONSUM_SEAL_FLAG='Y' AND VENDOR_CODE=VENDOR_ID AND ALLOTED_TO=EMP_ID) SealAlloted ";
        sql += " FROM MOBINT.MCR_USER_DETAILS a WHERE a.EMP_TYPE='I' and a.ACTIVE_FLAG='Y' AND a.VENDOR_ID IS NOT NULL ";

        if (_gID != "")
            sql += " AND a.VENDOR_ID='" + _gID + "'";

        if (_gDivision != "")
            sql += " and DIVISION IN ('" + _gDivision + "')";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable getEmpDetailsForDropDown(string _gID)
    {
        string sql = "SELECT EMP_NAME ||' ('|| EMP_ID||')' EMP_NAME ,EMP_ID  FROM MOBINT.MCR_USER_DETAILS a WHERE EMP_TYPE='I' AND VENDOR_ID='" + _gID + "' ORDER BY EMP_NAME";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable getDivisionDetails(string _gDivisionID)
    {
        string sql = "SELECT DIST_CD, DIVISION_NAME FROM MOBINT.mcr_division WHERE dist_cd IN ('" + _gDivisionID + "')  AND STATUS='Y' ORDER BY DIVISION_NAME ";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable getOrderTypeDetails(string _sVendorID, string _sDivision, string _sRole)   //16032018
    {
        string sql = "SELECT UNIQUE ORDER_TYPE,(ORDER_TYPE ||' - ' || ORDER_DESC) ORDER_DESCRIPTION FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y'";
        sql += " AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _sVendorID + "' AND DIVISION IN('" + _sDivision.Replace("'", "") + "') )"; //AND ROLE_TYPE = '" + _sRole + "'
        sql += " ORDER BY ORDER_DESCRIPTION";
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable getPM_Activity_OrderWise(string _sOrderType, string _sVendorID, string _sDivision, string _sRole)  //16032018
    {
        string sql = "SELECT DISTINCT PM_ACTIVTY,(PM_ACTIVTY ||' - ' ||PM_DESC)PM_DESCRIPTION FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE='" + _sOrderType + "'";
        sql += " AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _sVendorID + "' AND DIVISION IN('" + _sDivision.Replace("'", "") + "') )";    //AND ROLE_TYPE = '" + _sRole + "'
        sql += " ORDER BY PM_DESCRIPTION ";


        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable GetVendorList_DivWise(string _gDivisionID)
    {
        string sql = "SELECT VENDOR_ID,VENDOR_NAME FROM MOBINT.MCR_VENDOR_MST WHERE ADDRESS='" + _gDivisionID + "' and ACTIVE_FLAG='Y' ";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    

    public int Assign_OrderInstaller_InputData(string strOrder, string strVendorID, string strAllocateTo)
    {
        string sqlinsert = " UPDATE MOBINT.MCR_INPUT_DETAILS SET FLAG='Y', ALLOCATE_DATE = SYSDATE,ALLOCATE_TO='" + strAllocateTo + "',ALLOCATE_BY='" + strVendorID + "' WHERE ORDERID='" + strOrder + "' ";
        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int Revert_Assign_OrdInt_InputData(string strOrder)
    {
        string sqlinsert = " UPDATE MOBINT.MCR_INPUT_DETAILS SET FLAG='N', ALLOCATE_DATE = NULL,ALLOCATE_BY=NULL WHERE ORDERID='" + strOrder + "' ";
        return objUti.ExecuteNonQuery(sqlinsert);
    }


    public int Assign_OrderInstHappCode_InputData(string strOrder, string strHappyCode)
    {
        string sqlinsert = " UPDATE MOBINT.MCR_INPUT_DETAILS SET HAPPY_CODE_GEN='" + strHappyCode + "' WHERE ORDERID='" + strOrder + "' ";

        return objUti.ExecuteNonQuery(sqlinsert);
    }


    public DataTable getInstaller_EmpDetails(string _gVendorID, string _gEmpID)
    {
        string sql = " SELECT EMP_NAME EMPNAME, EMP_ID  FROM MOBINT.MCR_USER_DETAILS a WHERE a.EMP_TYPE='I' AND a.ACTIVE_FLAG='Y' ";
        sql += "  AND a.VENDOR_ID='" + _gVendorID + "' AND EMP_ID !='" + _gEmpID + "' ORDER BY EMP_NAME ";
        
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public int MapData_OrderInstaller_InputData(string strOrder, string strMeterNo, string strVendorID, string strInstallerID, string strOrdType)
    {
        string sqlinsert = " INSERT INTO MOBINT.MCR_VEND_ORDER_INST_MAP (ORDER_NO, METER_NO, VENDOR_CODE, INSTALLER_ID, ORDER_TYPE) ";
        sqlinsert = sqlinsert + "    VALUES  ('" + strOrder + "', '" + strMeterNo + "', '" + strVendorID + "', '" + strInstallerID + "', '" + strOrdType + "') ";
        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int Update_MCRSeal_Manual(string _sSeals,string _sUploadBy, string _sFileName, string _sFilePathAddress )
    {
        string sqlinsert = " UPDATE MOBINT.mcr_seal_details SET CONSUM_SEAL_FLAG='C',FILE_UPLOAD_BY='" + _sUploadBy + "',FILE_UPLOAD_DATE=SYSDATE,FILE_UPLOAD_REMARK='MANUAL', FILE_NAME='" + _sFileName + "', FILE_PATH_ADD='" + _sFilePathAddress + "' WHERE UPPER(SEAL)='" + _sSeals.ToUpper() + "'";
        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public DataTable GetUpload_SealFile()
    {
        string sql = "SELECT '0'SNO, FILE_NAME,FILE_UPLOAD_BY, FILE_UPLOAD_DATE,  FILE_PATH_ADD FROM MOBINT.MCR_SEAL_DETAILS WHERE FILE_NAME IS NOT NULL ";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public int InsertBySQL(string _sSql)
    {
        return objUti.ExecuteNonQuery(_sSql);
    }

    public int checkRole(string _sVendorID, string _sDivision, string _sRole)
    {
        int count = 0;
        string sql = "SELECT * FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE 1=1";      //VENDOR_CODE, DIVISION, ORDER_TYPE, PM_ACTIVITY, ROLE_TYPE
        
        //if (_sVendorID != "")
        sql += " AND VENDOR_CODE = '" + _sVendorID + "'";
        if (_sDivision != "")
            sql += " AND DIVISION IN ('" + _sDivision.Replace("'","") + "')";
        //if (_sRole != "")
        //    sql += " AND ROLE_TYPE = '" + _sRole + "'";

        DataTable dt = objUti.ExecuteReader(sql);
        count = dt.Rows.Count;

        return count;
    }

    #endregion
   

    #region Seal Allocation
    public DataTable Get_Seal_Details(string _gFlag, string _gDivision, string _gCompany, string _gVendorID, string _gDDlDivision, string _gPostingDate, string _gSealSerialNo)
    {
        string sql = string.Empty;

        sql = " SELECT DIVISION, to_char(POSTING_DATE, 'dd/MM/yyyy') POSTINGDATE, PLANNER_GROUP, SEAL, MATERIAL_CODE, SERIAL_NO, USER_RESPONSIBLE, SCHEME, ALLOTED_DATE, ALLOTED_TO ";
        sql += " FROM MOBINT.MCR_SEAL_DETAILS where";
        if (_gFlag != "")
            sql += " CONSUM_SEAL_FLAG='" + _gFlag + "' ";
        if (_gCompany != "")
            sql += " AND COMP_CODE='" + _gCompany + "' ";

        if (_gDDlDivision != "")
            sql += " AND division IN('" + _gDDlDivision + "') ";
        else
            sql += " AND division IN('" + _gDivision + "')  ";

        if (_gVendorID != "")
            sql += " AND VENDOR_CODE='" + _gVendorID + "' ";

        if (_gPostingDate != "")
            sql += " AND trunc(POSTING_DATE)='" + _gPostingDate + "' ";

        if (_gSealSerialNo != "")
            sql += " AND SERIAL_NO like '%" + _gSealSerialNo + "%' ";

        DataTable dt = new DataTable();
        dt = objUti.ExecuteReader(sql);

        return dt;

    }

    public int Assign_Seal_Allocation(string _gAllotedTO, string _gSeal, string _gSerialNO, string _gUserID)
    {
        string sqlinsert = " UPDATE MOBINT.MCR_SEAL_DETAILS SET ALLOTED_DATE=SYSDATE, ALLOTED_TO='" + _gAllotedTO + "', CONSUM_SEAL_FLAG='Y', ALLOTED_BY='" + _gUserID + "' WHERE seal='" + _gSerialNO + "' ";


        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public DataTable getSealDetails(string _gID, string _gSealNO, string _gFlag, string _gDivision)
    {
        string sql = " SELECT Seal, SERIAL_NO, CONSUM_SEAL_FLAG FROM MOBINT.MCR_SEAL_DETAILS WHERE SEAL_TYPE IS NULL AND VENDOR_CODE='" + _gID + "' ";

        if (_gDivision != "")
            sql += " AND DIVISION='" + _gDivision + "' ";

        if (_gFlag != "")
            sql += " AND CONSUM_SEAL_FLAG='" + _gFlag + "' ";

        if (_gSealNO != "")
            sql += " AND UPPER(SEAL)=UPPER('" + _gSealNO + "') ";

        sql += "ORDER BY serial_no ASC";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable getSeal_GunnyDetails(string _gID, string _gSealNO, string _gFlag, string _gDivision)
    {
        string sql = " SELECT Seal, SERIAL_NO, CONSUM_SEAL_FLAG FROM MOBINT.MCR_SEAL_DETAILS WHERE SEAL_TYPE ='GUNNY' AND VENDOR_CODE='" + _gID + "' ";

        if (_gDivision != "")
            sql += " AND DIVISION='" + _gDivision + "' ";

        if (_gFlag != "")
            sql += " AND CONSUM_SEAL_FLAG='" + _gFlag + "' ";

        if (_gSealNO != "")
            sql += " AND UPPER(SEAL)=UPPER('" + _gSealNO + "') ";

        sql += "ORDER BY serial_no ASC";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable getSealDetails_InstallerWise(string _sVendorID, string _sIstallerID, string _gDivision)
    {
        string sql = " SELECT SEAL FROM  MOBINT.MCR_SEAL_DETAILS WHERE VENDOR_code='" + _sVendorID + "' AND ALLOTED_TO='" + _sIstallerID + "' AND DIVISION  IN ('" + _gDivision + "') AND CONSUM_SEAL_FLAG='Y'";
        sql += " ORDER BY SEAL ASC";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable getSealDetailsCount(string _gID, string _gSealFromNO, string _gSealToNO, string _gDivision)
    {
        string sql = " SELECT count(1) SERIAL_NO FROM MOBINT.MCR_SEAL_DETAILS WHERE CONSUM_SEAL_FLAG='N' AND VENDOR_CODE='" + _gID + "' ";

        if (_gSealFromNO != "" && _gSealToNO != "")
            sql += " AND UPPER(Seal) between UPPER('" + _gSealFromNO + "') and UPPER('" + _gSealToNO + "') ";

        if (_gDivision != "")
            sql += " AND DIVISION='" + _gDivision + "' ";

        sql += "ORDER BY Seal ASC";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable getSeal_Detail_CheckBox(string _gID, string _gSealFromNO, string _gSealToNO, string _gDivision)
    {
        string sql = " SELECT Seal FROM MOBINT.MCR_SEAL_DETAILS WHERE CONSUM_SEAL_FLAG='N' AND VENDOR_CODE='" + _gID + "' ";

        if (_gSealFromNO != "" && _gSealToNO != "")
            sql += " AND UPPER(Seal) between UPPER('" + _gSealFromNO + "') and UPPER('" + _gSealToNO + "') ";

        if (_gDivision != "")
            sql += " AND DIVISION='" + _gDivision + "' ";

        sql += "ORDER BY Seal ASC";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }
    #endregion

    #region Menu
    public DataTable GetMenuData_RoleWise(string _sRole, string _sCompany)
    {
        StringBuilder _sQuery = new StringBuilder();

        _sQuery.Append("  SELECT PAGE_ID,PAGE_TITLE,NAVIGATE_URL,PARENT_ID FROM MOBINT.MCR_PAGE_MST WHERE PARENT_ID='106' AND PAGE_ID IN  ");
        _sQuery.Append(" (SELECT USER_MODULE_CODE FROM  MOBINT.MCR_ROLE_DETAILS WHERE ROLE_ID='" + _sRole + "' AND ROLE_AMQ !='N' AND company='" + _sCompany + "') OR (PARENT_ID='0' AND  NAVIGATE_URL IS NULL AND PAGE_TITLE='MMG Dashboard') ORDER BY PAGE_ID ");

        DataTable dt = new DataTable();
        dt = objUti.ExecuteReader(_sQuery.ToString());
        return dt;
    }
    #endregion

    #region Add User
    public DataTable getRoleDetails(string _gRoleID)
    {
        string sql = "SELECT ROLE_ID, ROLE_NAME FROM MOBINT.MCR_ROLE_MST WHERE ID>(SELECT ID FROM MOBINT.MCR_ROLE_MST WHERE ROLE_ID='" + _gRoleID + "')";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable getEmployeeDetails(string _gVendorID, string _gInstallerID, string _sType, string _sDivison)
    {
        string sql = string.Empty;

         if (_sType == "U")
             sql = "   SELECT a.EMP_NAME, '' VENDOR, a.EMP_ID, IMEI_NO, DIVISION, (SELECT ROLE_NAME FROM MOBINT.MCR_ROLE_MST WHERE ROLE_ID=EMP_TYPE) EMP_TYPE, ROLE, a.COMPANY, a.ACTIVE_FLAG, (CASE a.ACTIVE_FLAG WHEN 'Y' THEN 'Active' ELSE 'Inactive' END) Status, b.PASSWORD FROM MOBINT.MCR_USER_DETAILS a,	  MOBINT.MCR_LOGIN_MST b WHERE a.EMP_ID=b.LOGIN_ID    ";
        else  if (_sType == "A")
             sql = "   SELECT a.EMP_NAME, '' VENDOR, a.EMP_ID, IMEI_NO, DIVISION, (SELECT ROLE_NAME FROM MOBINT.MCR_ROLE_MST WHERE ROLE_ID=EMP_TYPE) EMP_TYPE, ROLE, a.COMPANY, a.ACTIVE_FLAG, (CASE a.ACTIVE_FLAG WHEN 'Y' THEN 'Active' ELSE 'Inactive' END) Status, b.PASSWORD FROM MOBINT.MCR_USER_DETAILS a,	  MOBINT.MCR_LOGIN_MST b WHERE a.EMP_ID=b.LOGIN_ID    ";
        else
             sql = "   SELECT a.EMP_NAME, vendor_NAME VENDOR, a.EMP_ID, IMEI_NO, DIVISION, (SELECT ROLE_NAME FROM MOBINT.MCR_ROLE_MST WHERE ROLE_ID=EMP_TYPE) EMP_TYPE, ROLE, a.COMPANY, a.ACTIVE_FLAG, (case a.ACTIVE_FLAG when 'Y' then 'Active' else 'Inactive' end) Status, b.PASSWORD FROM MOBINT.MCR_USER_DETAILS a, MOBINT.MCR_LOGIN_MST b,  MOBINT.MCR_VENDOR_MST  c WHERE a.EMP_ID=b.LOGIN_ID   AND a.vendor_id=c.vendor_id AND a.division=b.division_id AND a.division=c.ADDRESS ";

        if (_sType == "V")
            sql += " and EMP_TYPE='V' ";
        else if (_sType == "A")
            sql += " and EMP_TYPE='A' ";
        else if (_sType == "U")
            sql += " and EMP_TYPE='U' ";
        else
        {
            sql += " and EMP_TYPE='I' AND a.VENDOR_ID='" + _gVendorID + "' ";
            sql += "  AND c.ADDRESS IN ('" + _sDivison + "') ";

            if (_sDivison != "")
                sql += " and DIVISION IN ('" + _sDivison + "')";
        }             

        if (_gInstallerID != "")
            sql += " AND a.EMP_ID='" + _gInstallerID + "' ";
       

        sql += " ORDER BY EMP_NAME";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable Check_USER_DETAILS(string _gEMP_ID)
    {
        string sqlinsert = " select count(1) count from MOBINT.MCR_USER_DETAILS where EMP_ID='" + _gEMP_ID + "' ";

        return objUti.ExecuteReader(sqlinsert);
    }

    public int insert_USER_DETAILS(string _gEMP_NAME, string _gEMP_ID, string _gIMEI_NO, string _gDIVISION, string _gEMP_TYPE, string _gROLE, string _gVENDOR_ID, string _gCOMPANY, string _gMobNo, string _gDesgnation)
    {        
        string sqlinsert = " insert into MOBINT.MCR_USER_DETAILS (EMP_NAME, EMP_ID, IMEI_NO, DIVISION, EMP_TYPE, ROLE, VENDOR_ID, COMPANY,DESIGNATION,MOBILE_NO) values ('" + _gEMP_NAME + "', '" + _gEMP_ID + "','" + _gIMEI_NO + "','" + _gDIVISION + "','" + _gEMP_TYPE + "','" + _gROLE + "','" + _gVENDOR_ID + "','" + _gCOMPANY + "','" + _gDesgnation + "','" + _gMobNo + "') ";

        return objUti.ExecuteNonQuery(sqlinsert);
    }
    

    public int insert_LOGIN_MST(string _gLOGIN_ID, string _gPASSWORD, string _gLOGIN_TYPE, string _gName, string _gDivID)
    {
        string sqlinsert = " insert into MOBINT.MCR_LOGIN_MST (LOGIN_ID, PASSWORD, LOGIN_TYPE,LOGIN_NAME,DIVISION_ID,COMP_CODE) values ('" + _gLOGIN_ID + "','" + _gPASSWORD + "','" + _gLOGIN_TYPE + "','" + _gName + "','" + _gDivID + "','BRPL') ";

        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int Update_USER_DETAILS(string _gACTIVE_FLAG, string _gEMP_NAME, string _gEMP_ID, string _gIMEI_NO, string _gDIVISION, string _gEMP_TYPE, string _gROLE, string _gVENDOR_ID, string _gCOMPANY, string _gMobNo, string _gDesgnation)
    {
        string sqlinsert = " Update MOBINT.MCR_USER_DETAILS set ACTIVE_FLAG='" + _gACTIVE_FLAG + "', EMP_NAME='" + _gEMP_NAME + "', IMEI_NO='" + _gIMEI_NO + "', DIVISION='" + _gDIVISION + "', EMP_TYPE='" + _gEMP_TYPE + "', ROLE='" + _gROLE + "', VENDOR_ID='" + _gVENDOR_ID + "', COMPANY='" + _gCOMPANY + "',MOBILE_NO='" + _gMobNo + "',DESIGNATION='" + _gDesgnation + "' where EMP_ID='" + _gEMP_ID + "'";

        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int update_LOGIN_MST(string _gLOGIN_ID, string _gPASSWORD, string _gLOGIN_TYPE)
    {
        string sqlinsert = " update MOBINT.MCR_LOGIN_MST set PASSWORD='" + _gPASSWORD + "', LOGIN_TYPE='" + _gLOGIN_TYPE + "' where LOGIN_ID='" + _gLOGIN_ID + "' ";

        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int Reset_Password(string _gLOGIN_ID)
    {
        string sqlinsert = " update MOBINT.MCR_LOGIN_MST set PASSWORD='12345678' where LOGIN_ID='" + _gLOGIN_ID + "' ";

        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int insert_Insallter_DETAILS(string _gInstallID, string _gInstallName, string _gDIVISION, string _gMobile, string _gVENDOR_ID, string _gDesgnation)
    {
        string sqlinsert = " INSERT INTO mobint.MCR_INSTALLER_MST(INSTALLER_ID, INSTALLER_NAME, ADDRESS, TEL_NO, VENDOR_ID, COMPANY, DESIGNATION) ";
        sqlinsert += " VALUES ";
        sqlinsert += " ('" + _gInstallID + "','" + _gInstallName + "','" + _gDIVISION + "','" + _gMobile + "','" + _gVENDOR_ID + "','BRPL','" + _gDesgnation + "') ";

        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public DataTable GetVendorID_DIVWise(string _sDivision)
    {
        string sqlinsert = " select VENDOR_ID from mobint.mcr_vendor_mst where ACTIVE_FLAG='Y' and ADDRESS='" + _sDivision + "' ";
        return objUti.ExecuteReader(sqlinsert);
    }

    #endregion

    #region Meter Reconciliation Report
    public DataTable getMeterReconciliation(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gDivision, string _gddlDivision,
                                            string _gCompany, string _sOrderType, string _sActType, string _gID, string _sRole)
    {
        string sql = "SELECT (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION) DIVISION,  (SELECT VENDOR_NAME FROM MOBINT.MCR_VENDOR_MST WHERE VENDOR_ID=VENDOR_CODE  AND ADDRESS=a.DIVISION AND ACTIVE_FLAG='Y') Vender_name, VENDOR_CODE, COUNT(1) Meters_Issued_To_Vendor, ";
        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_INPUT_DETAILS WHERE ALLOCATE_BY IS NOT NULL AND PUNCH_BY IS NOT NULL AND FLAG='C' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION and comp_code='" + _gCompany + "' ";//AND SUBSTR(CA_NO,1,4) !='0003' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

        if (_sOrderType != "-ALL-" && _sOrderType != "")
            sql += " AND AUART ='" + _sOrderType + "'";
        else
            sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";
        if (_sActType != "-ALL-" && _sActType != "")
            sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
        else
            sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";  

        sql += " )  Meters_Installed, ";

        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_INPUT_DETAILS WHERE ALLOCATE_BY IS NOT NULL AND PUNCH_BY IS NULL AND FLAG='Y' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION and comp_code='" + _gCompany + "' ";// AND SUBSTR(CA_NO,1,4) !='0003' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

        if (_sOrderType != "-ALL-" && _sOrderType != "")
            sql += " AND AUART ='" + _sOrderType + "'";
        else
            sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";
        if (_sActType != "-ALL-" && _sActType != "")
            sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
        else
            sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";    

        sql += " ) Mtr_Pndng_Instlr_Side,  ";

        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_INPUT_DETAILS WHERE ALLOCATE_BY IS NOT NULL AND PUNCH_BY IS NOT NULL AND FLAG='E' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION and comp_code='" + _gCompany + "' ";// AND SUBSTR(CA_NO,1,4) !='0003' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        if (_sOrderType != "-ALL-" && _sOrderType != "")
            sql += " AND AUART ='" + _sOrderType + "'";
        else
            sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";
        if (_sActType != "-ALL-" && _sActType != "")
            sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
        else
            sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     

        sql += " )  Mtr_Cancelled, ";

        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_INPUT_DETAILS WHERE ALLOCATE_BY IS NULL AND PUNCH_BY IS NULL AND FLAG='N' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION and comp_code='" + _gCompany + "' ";//AND SUBSTR(CA_NO,1,4) !='0003' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        if (_sOrderType != "-ALL-" && _sOrderType != "")
            sql += " AND AUART ='" + _sOrderType + "'";
        else
            sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";
        if (_sActType != "-ALL-" && _sActType != "")
            sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
        else
            sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     

        sql += " )  Pending_Alottment_TO_Installer ";

        sql += " FROM MOBINT.MCR_INPUT_DETAILS a  ";
        sql += " WHERE 1=1  ";//AND SUBSTR(CA_NO,1,4) !='0003' ";

        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

        if (_gddlDivision != "")
            sql += " AND division='" + _gddlDivision + "' ";
        else
            sql += " AND division in ('" + _gDivision + "') ";

        sql += "  and division in (SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE STATUS='Y') ";

        if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
            sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'

      if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
          sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

        if (_gCompany != "")
            sql += " and comp_code='" + _gCompany + "'";

        sql += " GROUP BY DIVISION, VENDOR_CODE ";


        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    public DataTable getMeterReconciliationDetails(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gDivision, string _gddlDivision, string _gType,
                                                    string _gCompany, string _gVendor, string _sOrderType, string _sActType, string _gID, string _sRole)
    {
        string sql = "";
        if (_gType == "CASESCOUNT")
        {
            sql = "select COMP_CODE COMPANY, TO_CHAR(PSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, ORDERID, METER_NO, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=DIVISION AND ROWNUM<2) DIV_NAME, DIVISION, VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, POLE_NO, ( select EMP_NAME from MOBINT.MCR_USER_DETAILS where emp_id=(SELECT INSTALLER_ID FROM MOBINT.MCR_VEND_ORDER_INST_MAP WHERE order_no=a.ORDERID AND ROWNUM<2)) PUNCH_BY, TO_CHAR(PUNCH_DATE, 'dd-Mon-yyyy') PUNCHDATE, (SELECT reason FROM MOBINT.MCR_ORDER_CANCEL  WHERE a.orderid=ORDERID AND ROWNUM<2) Cancel_Reason, ";

            sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.AUART = B.ORDER_TYPE)) ORDER_TYPE,";
            sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.ILART_ACTIVITY_TYPE = B.PM_ACTIVTY)) PM_ACT,";
            sql += "'' OLD_M_READING,'' OLD_MR_KWH,'' OLD_MR_KW,'' MRKVAH_OLD,'' OLD_MR_KVA,'' INSTALLEDCABLE_OLD,'' CABLESIZE_OLD,     ";
            sql += "  '' DRUMSIZE_OLD,'' CABLEINSTALLTYPE_OLD,'' RUNNINGLENGTHFROM_OLD,'' RUNNINGLENGTHTO_OLD,'' CABLELENGTH_OLD,'' OUTPUTBUSLENGTH_OLD,'' REM_TERMINAL_SEAL, ";
            sql += " '' REM_OTHER_SEAL, '' REM_BOX_SEAL1,'' REM_BOX_SEAL2, '' REM_BUSBAR_SEAL1,'' REM_BUSBAR_SEAL2,'' BOX_OLD,'' GLANDS_OLD,'' TCOVER_OLD,'' BRASSSCREW_OLD,  ";
            sql += "  '' BUSBAR_OLD,'' THIMBLE_OLD,'' SADDLE_OLD,'' GUNNYBAG_OLD,'' GUNNYBAGSEAL_OLD,'' LABTESTING_DATE_OLD,'' METERRELOCATE_OLD ";

            sql += " from MOBINT.MCR_INPUT_DETAILS a ";
            sql += " WHERE 1=1   ";//AND SUBSTR(CA_NO,1,4) !='0003' ";

            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

            if (_gddlDivision != "")
                sql += " AND division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";
            if (_gVendor != "")
                sql += " and VENDOR_CODE='" + _gVendor + "'";

            if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
                sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";  //AND ROLE_TYPE = '" + _sRole + "'
        }

        if (_gType == "METERSCONSUMED")
        {
            sql = "select COMP_CODE COMPANY, TO_CHAR(PSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, a.ORDERID, METER_NO,(SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION AND ROWNUM<2) DIV_NAME, a.DIVISION, a.VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, POLE_NO, ( select EMP_NAME from MOBINT.MCR_USER_DETAILS where emp_id=(SELECT INSTALLER_ID FROM MOBINT.MCR_VEND_ORDER_INST_MAP WHERE order_no=a.ORDERID AND ROWNUM<2)) PUNCH_BY, TO_CHAR(PUNCH_DATE, 'dd-Mon-yyyy') PUNCHDATE, (SELECT reason FROM MOBINT.MCR_ORDER_CANCEL  WHERE a.orderid=ORDERID AND ROWNUM<2) Cancel_Reason, ";
            sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.AUART = B.ORDER_TYPE)) ORDER_TYPE,";
            sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.ILART_ACTIVITY_TYPE = B.PM_ACTIVTY)) PM_ACT,";
            sql += "OLD_M_READING,OLD_MR_KWH,OLD_MR_KW,MRKVAH_OLD,OLD_MR_KVA,INSTALLEDCABLE_OLD,CABLESIZE_OLD,     ";
            sql += "  DRUMSIZE_OLD,CABLEINSTALLTYPE_OLD,RUNNINGLENGTHFROM_OLD,RUNNINGLENGTHTO_OLD,CABLELENGTH_OLD,OUTPUTBUSLENGTH_OLD,REM_TERMINAL_SEAL, ";
            sql += "  REM_OTHER_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2, REM_BUSBAR_SEAL1,REM_BUSBAR_SEAL2,BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,  ";
            sql += "  BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD,GUNNYBAGSEAL_OLD,LABTESTING_DATE_OLD,METERRELOCATE_OLD ";
            sql += "  FROM MOBINT.MCR_INPUT_DETAILS a, MOBINT.MCR_DETAILS b WHERE a.ORDERID=b.ORDERID ";//AND  SUBSTR(CA_NO,1,4) !='0003'   ";
            
            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

            if (_gddlDivision != "")
                sql += " AND a.division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";
            sql += "AND ALLOCATE_BY IS NOT NULL AND PUNCH_BY IS NOT NULL AND FLAG='C'  ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";
            if (_gVendor != "")
                sql += " and a.VENDOR_CODE='" + _gVendor + "'";

            if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
                sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))"; //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";  //AND ROLE_TYPE = '" + _sRole + "'
        }

        if (_gType == "METERPENDING")
        {
            sql = "select COMP_CODE COMPANY, TO_CHAR(PSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, ORDERID, METER_NO, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=DIVISION AND ROWNUM<2) DIV_NAME, DIVISION, VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, POLE_NO, ( select EMP_NAME from MOBINT.MCR_USER_DETAILS where emp_id=(SELECT INSTALLER_ID FROM MOBINT.MCR_VEND_ORDER_INST_MAP WHERE order_no=a.ORDERID AND ROWNUM<2)) PUNCH_BY, TO_CHAR(PUNCH_DATE, 'dd-Mon-yyyy') PUNCHDATE, (SELECT reason FROM MOBINT.MCR_ORDER_CANCEL  WHERE a.orderid=ORDERID AND ROWNUM<2) Cancel_Reason, ";

            sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.AUART = B.ORDER_TYPE)) ORDER_TYPE,";
            sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.ILART_ACTIVITY_TYPE = B.PM_ACTIVTY)) PM_ACT,";
            sql += "'' OLD_M_READING,'' OLD_MR_KWH,'' OLD_MR_KW,'' MRKVAH_OLD,'' OLD_MR_KVA,'' INSTALLEDCABLE_OLD,'' CABLESIZE_OLD,     ";
            sql += "  '' DRUMSIZE_OLD,'' CABLEINSTALLTYPE_OLD,'' RUNNINGLENGTHFROM_OLD,'' RUNNINGLENGTHTO_OLD,'' CABLELENGTH_OLD,'' OUTPUTBUSLENGTH_OLD,'' REM_TERMINAL_SEAL, ";
            sql += " '' REM_OTHER_SEAL, '' REM_BOX_SEAL1,'' REM_BOX_SEAL2, '' REM_BUSBAR_SEAL1,'' REM_BUSBAR_SEAL2,'' BOX_OLD,'' GLANDS_OLD,'' TCOVER_OLD,'' BRASSSCREW_OLD,  ";
            sql += "  '' BUSBAR_OLD,'' THIMBLE_OLD,'' SADDLE_OLD,'' GUNNYBAG_OLD,'' GUNNYBAGSEAL_OLD,'' LABTESTING_DATE_OLD,'' METERRELOCATE_OLD ";

            sql += " from MOBINT.MCR_INPUT_DETAILS a ";
            sql += " WHERE 1=1  ";//AND SUBSTR(CA_NO,1,4) !='0003' ";
            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

            if (_gddlDivision != "")
                sql += " AND division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";
            sql += "AND ALLOCATE_BY IS NOT NULL AND PUNCH_BY IS NULL AND FLAG='Y' ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";
            if (_gVendor != "")
                sql += " and VENDOR_CODE='" + _gVendor + "'";

            if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
                sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))"; //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";  //AND ROLE_TYPE = '" + _sRole + "'
        }

        if (_gType == "METERSCANCEL")
        {
            sql = "select COMP_CODE COMPANY, TO_CHAR(PSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, ORDERID, METER_NO,(SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=DIVISION AND ROWNUM<2) DIV_NAME, DIVISION, VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, POLE_NO, ( select EMP_NAME from MOBINT.MCR_USER_DETAILS where emp_id=(SELECT INSTALLER_ID FROM MOBINT.MCR_VEND_ORDER_INST_MAP WHERE order_no=a.ORDERID AND ROWNUM<2)) PUNCH_BY, TO_CHAR(PUNCH_DATE, 'dd-Mon-yyyy') PUNCHDATE, (SELECT reason FROM MOBINT.MCR_ORDER_CANCEL  WHERE a.orderid=ORDERID AND ROWNUM<2) Cancel_Reason, ";

            sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.AUART = B.ORDER_TYPE)) ORDER_TYPE,";
            sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.ILART_ACTIVITY_TYPE = B.PM_ACTIVTY)) PM_ACT,";
            sql += "'' OLD_M_READING,'' OLD_MR_KWH,'' OLD_MR_KW,'' MRKVAH_OLD,'' OLD_MR_KVA,'' INSTALLEDCABLE_OLD,'' CABLESIZE_OLD,     ";
            sql += "  '' DRUMSIZE_OLD,'' CABLEINSTALLTYPE_OLD,'' RUNNINGLENGTHFROM_OLD,'' RUNNINGLENGTHTO_OLD,'' CABLELENGTH_OLD,'' OUTPUTBUSLENGTH_OLD,'' REM_TERMINAL_SEAL, ";
            sql += " '' REM_OTHER_SEAL, '' REM_BOX_SEAL1,'' REM_BOX_SEAL2, '' REM_BUSBAR_SEAL1,'' REM_BUSBAR_SEAL2,'' BOX_OLD,'' GLANDS_OLD,'' TCOVER_OLD,'' BRASSSCREW_OLD,  ";
            sql += "  '' BUSBAR_OLD,'' THIMBLE_OLD,'' SADDLE_OLD,'' GUNNYBAG_OLD,'' GUNNYBAGSEAL_OLD,'' LABTESTING_DATE_OLD,'' METERRELOCATE_OLD ";

            sql += " from MOBINT.MCR_INPUT_DETAILS a ";
            sql += " WHERE 1=1  ";//AND SUBSTR(CA_NO,1,4) !='0003' ";
            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

            if (_gddlDivision != "")
                sql += " AND division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";

            sql += "AND ALLOCATE_BY IS NOT NULL AND PUNCH_BY IS NOT NULL AND FLAG='E' ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";
            if (_gVendor != "")
                sql += " and VENDOR_CODE='" + _gVendor + "'";

            if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
                sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))"; //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";  //AND ROLE_TYPE = '" + _sRole + "'
        }

        if (_gType == "METERSNOTASSIGN")
        {
            sql = "select COMP_CODE COMPANY, TO_CHAR(PSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, ORDERID, METER_NO, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=DIVISION AND ROWNUM<2) DIV_NAME, DIVISION, VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, POLE_NO, ( select EMP_NAME from MOBINT.MCR_USER_DETAILS where emp_id=(SELECT INSTALLER_ID FROM MOBINT.MCR_VEND_ORDER_INST_MAP WHERE order_no=a.ORDERID AND ROWNUM<2)) PUNCH_BY, TO_CHAR(PUNCH_DATE, 'dd-Mon-yyyy') PUNCHDATE, (SELECT reason FROM MOBINT.MCR_ORDER_CANCEL  WHERE a.orderid=ORDERID AND ROWNUM<2) Cancel_Reason, ";

            sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.AUART = B.ORDER_TYPE)) ORDER_TYPE,";
            sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.ILART_ACTIVITY_TYPE = B.PM_ACTIVTY)) PM_ACT,";
            sql += "'' OLD_M_READING,'' OLD_MR_KWH,'' OLD_MR_KW,'' MRKVAH_OLD,'' OLD_MR_KVA,'' INSTALLEDCABLE_OLD,'' CABLESIZE_OLD,     ";
            sql += "  '' DRUMSIZE_OLD,'' CABLEINSTALLTYPE_OLD,'' RUNNINGLENGTHFROM_OLD,'' RUNNINGLENGTHTO_OLD,'' CABLELENGTH_OLD,'' OUTPUTBUSLENGTH_OLD,'' REM_TERMINAL_SEAL, ";
            sql += " '' REM_OTHER_SEAL, '' REM_BOX_SEAL1,'' REM_BOX_SEAL2, '' REM_BUSBAR_SEAL1,'' REM_BUSBAR_SEAL2,'' BOX_OLD,'' GLANDS_OLD,'' TCOVER_OLD,'' BRASSSCREW_OLD,  ";
            sql += "  '' BUSBAR_OLD,'' THIMBLE_OLD,'' SADDLE_OLD,'' GUNNYBAG_OLD,'' GUNNYBAGSEAL_OLD,'' LABTESTING_DATE_OLD,'' METERRELOCATE_OLD ";

            sql += " from MOBINT.MCR_INPUT_DETAILS a ";
            sql += " WHERE 1=1  ";//AND SUBSTR(CA_NO,1,4) !='0003' ";
            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

            if (_gddlDivision != "")
                sql += " AND division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";
            sql += " AND ALLOCATE_BY IS NULL AND PUNCH_BY IS NULL AND FLAG='N' ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";
            if (_gVendor != "")
                sql += " and VENDOR_CODE='" + _gVendor + "'";

            if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
                sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'               

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'
               
        }


        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    #endregion

    #region Seal Reconciliation Report
    public DataTable getSealReconciliation(string _sVendorID, string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gDivision, string _gddlDivision, string _gCompany)
    {
        string sql = "SELECT (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION) DIVISION, (SELECT VENDOR_NAME FROM MOBINT.MCR_VENDOR_MST WHERE VENDOR_ID=VENDOR_CODE  AND ADDRESS=a.DIVISION AND ACTIVE_FLAG='Y') Vender_name, VENDOR_CODE, COUNT(1) Seal_Issued_Vendor, ";
        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_SEAL_DETAILS WHERE SEAL_TYPE is null and PLANNER_GROUP in('MMG','REC','GCC') AND CONSUM_SEAL_FLAG='C' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION AND comp_code='" + _gCompany + "' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        sql += " )  SealConsumed, ";
        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_SEAL_DETAILS WHERE SEAL_TYPE is null and PLANNER_GROUP in('MMG','REC','GCC') AND CONSUM_SEAL_FLAG='Y' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION AND comp_code='" + _gCompany + "' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        sql += " ) Seal_with_Installer,    ";
        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_SEAL_DETAILS WHERE SEAL_TYPE is null and PLANNER_GROUP in('MMG','REC','GCC') AND CONSUM_SEAL_FLAG='N' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION AND comp_code='" + _gCompany + "' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        sql += ")  Seal_Pendding   ";
        sql += " FROM MOBINT.MCR_SEAL_DETAILS a  ";
        sql += " WHERE SEAL_TYPE is null AND PLANNER_GROUP in('MMG','REC','GCC') ";

        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

        if (_gddlDivision != "")
        {                        
            sql += " AND division='" + _gddlDivision + "' ";
        }
        else
        {                     
            sql += " AND division in ('" + _gDivision + "') ";
        }

        sql += " AND VENDOR_CODE in ('" + _sVendorID + "')";
        sql += "  and division in (SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE STATUS='Y') ";
        sql += " and VENDOR_CODE in (SELECT VENDOR_ID FROM MOBINT.MCR_VENDOR_MST WHERE ADDRESS=a.DIVISION) ";

        if (_gCompany != "")
            sql += " and comp_code='" + _gCompany + "'";

        sql += " GROUP BY DIVISION, VENDOR_CODE ";


        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    public DataTable getGunnySealReconciliation(string _sVendorID, string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gDivision, string _gddlDivision, string _gCompany)
    {
        string sql = "SELECT (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION) DIVISION, (SELECT VENDOR_NAME FROM MOBINT.MCR_VENDOR_MST WHERE VENDOR_ID=VENDOR_CODE  AND ADDRESS=a.DIVISION AND ACTIVE_FLAG='Y') Vender_name, VENDOR_CODE, COUNT(1) Seal_Issued_Vendor, ";
        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_SEAL_DETAILS WHERE SEAL_TYPE ='GUNNY' and PLANNER_GROUP in('MMG','REC','GCC')  AND CONSUM_SEAL_FLAG='C' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION AND comp_code='" + _gCompany + "' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        sql += " )  SealConsumed, ";
        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_SEAL_DETAILS WHERE SEAL_TYPE ='GUNNY' and PLANNER_GROUP in('MMG','REC','GCC')  AND CONSUM_SEAL_FLAG='Y' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION AND comp_code='" + _gCompany + "' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        sql += " ) Seal_with_Installer,    ";
        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_SEAL_DETAILS WHERE SEAL_TYPE ='GUNNY' and PLANNER_GROUP in('MMG','REC','GCC')  AND CONSUM_SEAL_FLAG='N' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION AND comp_code='" + _gCompany + "' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        sql += ")  Seal_Pendding   ";
        sql += " FROM MOBINT.MCR_SEAL_DETAILS a  ";
        sql += " WHERE  SEAL_TYPE ='GUNNY' AND PLANNER_GROUP in('MMG','REC','GCC')  ";

        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

        if (_gddlDivision != "")
        {
            sql += " AND division='" + _gddlDivision + "' ";
        }
        else
        {
            sql += " AND division in ('" + _gDivision + "') ";
        }

        sql += " AND VENDOR_CODE in ('" + _sVendorID + "')";
        sql += "  and division in (SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE STATUS='Y') ";
        sql += " and VENDOR_CODE in (SELECT VENDOR_ID FROM MOBINT.MCR_VENDOR_MST WHERE ADDRESS=a.DIVISION) ";

        if (_gCompany != "")
            sql += " and comp_code='" + _gCompany + "'";

        sql += " GROUP BY DIVISION, VENDOR_CODE ";


        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    public DataTable getSealReconciliationDetails(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gDivision, string _gddlDivision, string _gType,
                                                    string _gCompany, string _sVendorID)
    {
        string sql = "";
        if (_gType == "CASESCOUNT")
        {
            sql = "select COMP_CODE, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION)DIVISION, to_char(POSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, SEAL, MATERIAL_CODE, SERIAL_NO, VENDOR_NAME, ORDER_TYPE, to_char(ALLOTED_DATE, 'dd-Mon-yyyy') ALLOTED_DATE, (ALLOTED_TO ||'-' || (SELECT EMP_NAME FROM MOBINT.MCR_user_details WHERE EMP_ID=ALLOTED_TO AND ROWNUM<2))ALLOTED_TO,  to_char(PUNCH_DATE, 'dd-Mon-yyyy')PUNCH_DATE ";
            sql += " from MOBINT.MCR_SEAL_DETAILS a ";
            sql += " WHERE SEAL_TYPE is null and PLANNER_GROUP in('MMG','REC','GCC') ";

            if (_sVendorID != "")
                sql += " AND VENDOR_CODE in ('" + _sVendorID + "')";

            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

            if (_gddlDivision != "")
                sql += " AND division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";
        }

        if (_gType == "SEALCONSUMED")
        {
            sql = "select COMP_CODE, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION)DIVISION, to_char(POSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, SEAL, MATERIAL_CODE, SERIAL_NO, VENDOR_NAME, ORDER_TYPE, to_char(ALLOTED_DATE, 'dd-Mon-yyyy') ALLOTED_DATE, (ALLOTED_TO ||'-' || (SELECT EMP_NAME FROM MOBINT.MCR_user_details WHERE EMP_ID=ALLOTED_TO AND ROWNUM<2))ALLOTED_TO, to_char(PUNCH_DATE, 'dd-Mon-yyyy')PUNCH_DATE ";
            sql += " from MOBINT.MCR_SEAL_DETAILS a ";
            sql += " WHERE SEAL_TYPE is null and PLANNER_GROUP in('MMG','REC','GCC') ";

            if (_sVendorID != "")
                sql += " AND VENDOR_CODE in ('" + _sVendorID + "')";

            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

            if (_gddlDivision != "")
                sql += " AND division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";
            sql += " AND CONSUM_SEAL_FLAG='C' ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";
        }

        if (_gType == "SEALPENDING")
        {
            sql = "select COMP_CODE, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION)DIVISION, to_char(POSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, SEAL, MATERIAL_CODE, SERIAL_NO, VENDOR_NAME, ORDER_TYPE, to_char(ALLOTED_DATE, 'dd-Mon-yyyy') ALLOTED_DATE, (ALLOTED_TO ||'-' || (SELECT EMP_NAME FROM MOBINT.MCR_user_details WHERE EMP_ID=ALLOTED_TO AND ROWNUM<2))ALLOTED_TO, to_char(PUNCH_DATE, 'dd-Mon-yyyy')PUNCH_DATE ";
            sql += " from MOBINT.MCR_SEAL_DETAILS a ";
            sql += " WHERE SEAL_TYPE is null and PLANNER_GROUP in('MMG','REC','GCC') ";

            if (_sVendorID != "")
                sql += " AND VENDOR_CODE in ('" + _sVendorID + "')";

            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

            if (_gddlDivision != "")
                sql += " AND division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";
            sql += " AND CONSUM_SEAL_FLAG='Y' ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";
        }

        if (_gType == "SEALNOTASSIGN")
        {
            sql = "select COMP_CODE, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION)DIVISION, to_char(POSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, SEAL, MATERIAL_CODE, SERIAL_NO, VENDOR_NAME, ORDER_TYPE, to_char(ALLOTED_DATE, 'dd-Mon-yyyy') ALLOTED_DATE,(ALLOTED_TO ||'-' || (SELECT EMP_NAME FROM MOBINT.MCR_user_details WHERE EMP_ID=ALLOTED_TO AND ROWNUM<2))ALLOTED_TO, to_char(PUNCH_DATE, 'dd-Mon-yyyy')PUNCH_DATE ";
            sql += " from MOBINT.MCR_SEAL_DETAILS a ";
            sql += " WHERE SEAL_TYPE is null and PLANNER_GROUP in('MMG','REC','GCC') ";

            if (_sVendorID != "")
                sql += " AND VENDOR_CODE in ('" + _sVendorID + "')";

            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

            if (_gddlDivision != "")
                sql += " AND division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";
            sql += "AND CONSUM_SEAL_FLAG='N' ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";
        }

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    public DataTable getSealGunnyReconciliationDetails(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gDivision, string _gddlDivision, string _gType,
                                                  string _gCompany, string _sVendorID)
    {
        string sql = "";
        if (_gType == "CASESCOUNT")
        {
            sql = "select COMP_CODE, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION)DIVISION, to_char(POSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, SEAL, MATERIAL_CODE, SERIAL_NO, VENDOR_NAME, ORDER_TYPE, to_char(ALLOTED_DATE, 'dd-Mon-yyyy') ALLOTED_DATE, (ALLOTED_TO ||'-' || (SELECT EMP_NAME FROM MOBINT.MCR_user_details WHERE EMP_ID=ALLOTED_TO AND ROWNUM<2))ALLOTED_TO,  to_char(PUNCH_DATE, 'dd-Mon-yyyy')PUNCH_DATE ";
            sql += " from MOBINT.MCR_SEAL_DETAILS a ";
            sql += " WHERE SEAL_TYPE ='GUNNY' and PLANNER_GROUP in('MMG','REC','GCC')  ";

            if (_sVendorID != "")
                sql += " AND VENDOR_CODE in ('" + _sVendorID + "')";

            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

            if (_gddlDivision != "")
                sql += " AND division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";
        }

        if (_gType == "SEALCONSUMED")
        {
            sql = "select COMP_CODE, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION)DIVISION, to_char(POSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, SEAL, MATERIAL_CODE, SERIAL_NO, VENDOR_NAME, ORDER_TYPE, to_char(ALLOTED_DATE, 'dd-Mon-yyyy') ALLOTED_DATE, (ALLOTED_TO ||'-' || (SELECT EMP_NAME FROM MOBINT.MCR_user_details WHERE EMP_ID=ALLOTED_TO AND ROWNUM<2))ALLOTED_TO, to_char(PUNCH_DATE, 'dd-Mon-yyyy')PUNCH_DATE ";
            sql += " from MOBINT.MCR_SEAL_DETAILS a ";
            sql += " WHERE SEAL_TYPE ='GUNNY' and PLANNER_GROUP in('MMG','REC','GCC')  ";

            if (_sVendorID != "")
                sql += " AND VENDOR_CODE in ('" + _sVendorID + "')";

            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

            if (_gddlDivision != "")
                sql += " AND division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";
            sql += " AND CONSUM_SEAL_FLAG='C' ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";
        }

        if (_gType == "SEALPENDING")
        {
            sql = "select COMP_CODE, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION)DIVISION, to_char(POSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, SEAL, MATERIAL_CODE, SERIAL_NO, VENDOR_NAME, ORDER_TYPE, to_char(ALLOTED_DATE, 'dd-Mon-yyyy') ALLOTED_DATE, (ALLOTED_TO ||'-' || (SELECT EMP_NAME FROM MOBINT.MCR_user_details WHERE EMP_ID=ALLOTED_TO AND ROWNUM<2))ALLOTED_TO, to_char(PUNCH_DATE, 'dd-Mon-yyyy')PUNCH_DATE ";
            sql += " from MOBINT.MCR_SEAL_DETAILS a ";
            sql += " WHERE SEAL_TYPE ='GUNNY' and  PLANNER_GROUP in('MMG','REC','GCC')  ";

            if (_sVendorID != "")
                sql += " AND VENDOR_CODE in ('" + _sVendorID + "')";

            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

            if (_gddlDivision != "")
                sql += " AND division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";
            sql += " AND CONSUM_SEAL_FLAG='Y' ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";
        }

        if (_gType == "SEALNOTASSIGN")
        {
            sql = "select COMP_CODE, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION)DIVISION, to_char(POSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, SEAL, MATERIAL_CODE, SERIAL_NO, VENDOR_NAME, ORDER_TYPE, to_char(ALLOTED_DATE, 'dd-Mon-yyyy') ALLOTED_DATE,(ALLOTED_TO ||'-' || (SELECT EMP_NAME FROM MOBINT.MCR_user_details WHERE EMP_ID=ALLOTED_TO AND ROWNUM<2))ALLOTED_TO, to_char(PUNCH_DATE, 'dd-Mon-yyyy')PUNCH_DATE ";
            sql += " from MOBINT.MCR_SEAL_DETAILS a ";
            sql += " WHERE SEAL_TYPE ='GUNNY' and PLANNER_GROUP in('MMG','REC','GCC') ";

            if (_sVendorID != "")
                sql += " AND VENDOR_CODE in ('" + _sVendorID + "')";

            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

            if (_gddlDivision != "")
                sql += " AND division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";
            sql += "AND CONSUM_SEAL_FLAG='N' ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";
        }

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    public DataTable GetVendorID_DivisionWise(string _sDivision)
    {
        string sql = "";
        sql = "SELECT VENDOR_CODE FROM  MOBINT.MCR_DIVISION WHERE DIST_CD IN ('" + _sDivision + "') ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }


    #endregion

    #region TF Sticker Report
    public DataTable getTFStickerDetails(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gCANo, string _gMeterNo, string _gCompany,
                                string _gDivision, string _sOrderType, string _sActType, string _gID, string _sRole)
    {
        string sql = " SELECT (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=b.DIVISION)DIVISION, (SELECT VENDOR_NAME FROM MOBINT.MCR_VENDOR_MST WHERE VENDOR_ID=b.VENDOR_CODE  AND ADDRESS=b.DIVISION AND ACTIVE_FLAG='Y') VENDOR,b.VENDOR_CODE, b.CA_NO, b.METER_NO, UPPER(OTHERSTICKER) Sticker_NO, (CASE WHEN OTHERSTICKER IS NOT NULL THEN 'Y' ELSE 'N' END) Sticker_Found, ";

        //sql += "   (SELECT DISTINCT (order_type ||' - '||ORDER_DESC)ORD_DESC FROM MOBINT.MCR_ORDER_PM_MASTER WHERE order_type=AUART AND ACTIVE_FLAG='Y' AND ROWNUM<2) ORDER_TYPE, ";
        //sql += " (  SELECT DISTINCT (PM_ACTIVTY ||' - '||PM_DESC)PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER WHERE PM_ACTIVTY=ILART_ACTIVITY_TYPE AND ACTIVE_FLAG='Y' AND ROWNUM<2) PM_ACT,";
        sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER C WHERE B.AUART = C.ORDER_TYPE)) ORDER_TYPE,";
        sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER C WHERE B.ILART_ACTIVITY_TYPE = C.PM_ACTIVTY)) PM_ACT,";
        sql += "  OLD_M_READING,OLD_MR_KWH,OLD_MR_KW,MRKVAH_OLD,OLD_MR_KVA,INSTALLEDCABLE_OLD,CABLESIZE_OLD,     ";
        sql += "  DRUMSIZE_OLD,CABLEINSTALLTYPE_OLD,RUNNINGLENGTHFROM_OLD,RUNNINGLENGTHTO_OLD,CABLELENGTH_OLD,OUTPUTBUSLENGTH_OLD,REM_TERMINAL_SEAL, ";
        sql += "  REM_OTHER_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2, REM_BUSBAR_SEAL1,REM_BUSBAR_SEAL2,BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,  ";
        sql += "  BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD,GUNNYBAGSEAL_OLD,LABTESTING_DATE_OLD,METERRELOCATE_OLD ";

        sql += " FROM MOBINT.MCR_DETAILS a, MOBINT.MCR_INPUT_DETAILS b ";
        sql += " WHERE a.ORDERID=b.ORDERID ";

        if (_gCANo != "")
            sql += " AND b.ca_no LIKE '%" + _gCANo + "%' ";

        if (_gMeterNo != "")
            sql += " AND b.METER_NO LIKE '%" + _gMeterNo + "%' ";

        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

        sql += " AND b.comp_code='" + _gCompany + "' ";
        sql += " AND b.division in ('" + _gDivision + "') ";

        if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
            sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))"; //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'


        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    #endregion

    #region MCR_PDF_SAP
    public DataTable getMCR_PDF_SAP_Details(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gCANo, string _gOrdNo, string _gCompany,
                                string _gDivision, string _sOrderType, string _sActType, string _gID, string _sRole,string _sDiv)
    {
        string sql = " SELECT (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=b.DIVISION)DIVISION, (SELECT VENDOR_NAME FROM MOBINT.MCR_VENDOR_MST WHERE VENDOR_ID=b.VENDOR_CODE  AND ADDRESS=b.DIVISION AND ACTIVE_FLAG='Y') VENDOR,b.VENDOR_CODE, b.CA_NO, b.METER_NO,a.ORDERID,  ";        
        sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER C WHERE B.AUART = C.ORDER_TYPE)) ORDER_TYPE,";
        sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER C WHERE B.ILART_ACTIVITY_TYPE = C.PM_ACTIVTY)) PM_ACT,";
        sql += "  OLD_MR_KWH,MRKVAH_OLD,REM_TERMINAL_SEAL,b.NAME,i.MCR_PDF ";
        sql += " FROM MOBINT.MCR_DETAILS a, MOBINT.MCR_INPUT_DETAILS b,MOBINT.MCR_image_DETAILS i ";
        sql += " WHERE a.ORDERID=b.ORDERID  AND b.ORDERID=i.ORDERID  AND i.TRANSFER_FLAG='N' AND i.MCR_PDF is not null and i.MCR_PDF !='X' ";

        if (_gCANo != "")
            sql += " AND b.ca_no LIKE '%" + _gCANo + "%' ";

        if (_gOrdNo != "")
            sql += " AND b.ORDERID LIKE '%" + _gOrdNo + "%' ";

        if (_sDiv != "")
            sql += " AND b.division ='"+_sDiv+"' ";
        else
            sql += " AND b.division in ('" + _gDivision + "') ";

        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(a.ENTRY_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

        sql += " AND b.comp_code='" + _gCompany + "' ";       

        if (_sOrderType != "-ALL-" && _sOrderType != "")
            sql += " AND AUART ='" + _sOrderType + "'";
        else
            sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))"; 

        if (_sActType != "-ALL-" && _sActType != "")
            sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
        else
            sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";   


        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    public DataTable GetData_OrderSendSAP(string _sOrderNo)
    {
        StringBuilder _sQuery = new StringBuilder();

        _sQuery.Append("  SELECT A.ORDERID, A.IMAGE1, A.IMAGE2, A.IMAGE3, A.IMEAGE_MCR, A.IMAGE_METERTESTREPORT, A.IMAGE_LABTESTINGREPORT, A.IMAGE_SIGNATURE, A.IMAGE4,A.MCR_PDF, B.COMP_CODE, B.CA_NO,A.IMAGE1_OLD, A.IMAGE2_OLD  FROM  MCR_IMAGE_DETAILS A, MCR_INPUT_DETAILS B WHERE A.orderid=B.orderid AND A.orderid='" + _sOrderNo + "' AND A.TRANSFER_FLAG='N' AND B.COMP_CODE='BRPL'  ");        

        DataTable dt = new DataTable();
        dt = objUti.ExecuteReader(_sQuery.ToString());
        return dt;
    }

    public DataTable GetData_PDFOrderSAP(string _sOrderNo)
    {
        StringBuilder _sQuery = new StringBuilder();

        _sQuery.Append("  SELECT PDF_FLAG,MCR_PDF FROM MCR_IMAGE_DETAILS WHERE  ORDERID='" + _sOrderNo + "' ");

        DataTable dt = new DataTable();
        dt = objUti.ExecuteReader(_sQuery.ToString());
        return dt;
    }

    public int Update_MCR_PDF(string _sOrderNo)
    {
        string sqlUpdate = " UPDATE MCR_IMAGE_DETAILS set UPD_FLG='0',PDF_FLAG='Y' where ORDERID='" + _sOrderNo + "' ";
        return objUti.ExecuteNonQuery(sqlUpdate);
    }

    public int Update_TansferSAP_Flag(string _sOrderNo)
    {
        string sqlUpdate = " UPDATE MCR_IMAGE_DETAILS set TRANSFER_FLAG='Y', TRANSFER_DATE=sysdate where ORDERID='" + _sOrderNo + "' ";
        return objUti.ExecuteNonQuery(sqlUpdate);
    }

    public int Insert_MCR_PDF_Log(string _sOrderNo, string _gCANO, string _sDir, string _gImagePath)
    {
        string sqlinsert = " INSERT INTO MCR_IMAGE_DETAILS_LOG(ORDERID,CA_NUMBER,SAP_PATH,NAS_PATH,UPD_FLG,PDF_FLAG)VALUES('" + _sOrderNo + "','" + _gCANO + "','" + _sDir + "\\" + _sOrderNo + "_0.pdf" + "','" + _gImagePath + "','0','Y') ";
        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int Insert_MCR_jpeg_Log(string _gDeviceNo, string _gCANO, string _gCount, string _sDir, string _gImagePath)
    {
        string sqlinsert = " INSERT INTO MCR_IMAGE_DETAILS_LOG(ORDERID,DEVICENO,CA_NUMBER,SAP_PATH,NAS_PATH,UPD_FLG)VALUES('','" + _gDeviceNo + "','" + _gCANO + "','" + _sDir + "\\" + _gCANO + "_" + _gCount + ".jpeg" + "','" + _gImagePath + "','" + _gCount + "') ";
        return objUti.ExecuteNonQuery(sqlinsert);
    }

    #endregion

    #region Quality Check Report

    public DataTable getQualityCheckDetails(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gCANo, string _gMeterNo, string _gCompany, string _gDivision
                                            , string _sOrderType, string _sActType, string _gID, string _sRole)
    {
        string sql = " SELECT (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=b.DIVISION)DIVISION, b.VENDOR_CODE, b.CA_NO, b.METER_NO, INSTALLATION, a.ELCB_INSTALLED, a.METERDOWNLOAD, FLOWMADE,a.EARTHING, HEIGHTOFMETER, ANYJOINTS,DBLOCKED, EARTHINGPOLE, ADDITIONALACCESSORIES, OVERHEADCABLE, OVERHEADCABLEPOLE,";

        sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER C WHERE B.AUART = C.ORDER_TYPE)) ORDER_TYPE,";
        sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER C WHERE B.ILART_ACTIVITY_TYPE = C.PM_ACTIVTY)) PM_ACT,";
        sql += "OLD_M_READING,OLD_MR_KWH,OLD_MR_KW,MRKVAH_OLD,OLD_MR_KVA,INSTALLEDCABLE_OLD,CABLESIZE_OLD,     ";
        sql += "  DRUMSIZE_OLD,CABLEINSTALLTYPE_OLD,RUNNINGLENGTHFROM_OLD,RUNNINGLENGTHTO_OLD,CABLELENGTH_OLD,OUTPUTBUSLENGTH_OLD,REM_TERMINAL_SEAL, ";
        sql += "  REM_OTHER_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2, REM_BUSBAR_SEAL1,REM_BUSBAR_SEAL2,BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,  ";
        sql += "  BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD,GUNNYBAGSEAL_OLD,LABTESTING_DATE_OLD,METERRELOCATE_OLD ";

        sql += " FROM MOBINT.MCR_DETAILS a, MOBINT.MCR_INPUT_DETAILS b ";
        sql += " WHERE a.ORDERID=b.ORDERID ";
        if (_gCANo != "")
            sql += " AND b.ca_no LIKE '%" + _gCANo + "%' ";
        if (_gMeterNo != "")
            sql += " AND b.METER_NO LIKE '%" + _gMeterNo + "%' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

        sql += " AND b.comp_code='" + _gCompany + "' ";
        sql += " AND b.division in ('" + _gDivision + "') ";

        if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
            sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'
            DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    #endregion


    /// <summary>
    /// Developed by Sanjeev Ranjan on dt 28/01/2019
    /// </summary>
    /// 
    #region MMG_ISU
    public DataTable getMMG_ISU(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gDivision, string _gddlDivision, string _gCompany,
                                   string _gPunchFrmDate, string _gPunchToDate, string _sOrderType, string _sActType, string _gID, string _sRole)
    {
        string sql = "SELECT '0'Sno, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION) DIVISION, (SELECT VENDOR_NAME FROM MOBINT.MCR_VENDOR_MST WHERE VENDOR_ID=VENDOR_CODE  AND ADDRESS=a.DIVISION AND ACTIVE_FLAG='Y') VENDOR,VENDOR_CODE, COUNT(1) Cases_Given_Vendor, ";
        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_INPUT_DETAILS WHERE FLAG='C' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION and comp_code='" + _gCompany + "' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        if (_gPunchFrmDate != "" && _gPunchToDate != "")
            sql += " AND TRUNC(PUNCH_DATE) BETWEEN '" + _gPunchFrmDate + "' AND '" + _gPunchToDate + "' ";

        if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
            sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      ////AND ROLE_TYPE = '" + _sRole + "'

        sql += " )  Cases_Executed, ";

        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_INPUT_DETAILS WHERE FLAG in ('Y', 'N') AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION and comp_code='" + _gCompany + "' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        if (_gPunchFrmDate != "" && _gPunchToDate != "")
            sql += " AND TRUNC(PUNCH_DATE) BETWEEN '" + _gPunchFrmDate + "' AND '" + _gPunchToDate + "' ";

        if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
            sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'

        sql += " ) Cases_Pending,  ";

        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_INPUT_DETAILS WHERE FLAG='E' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION and comp_code='" + _gCompany + "' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        if (_gPunchFrmDate != "" && _gPunchToDate != "")
            sql += " AND TRUNC(PUNCH_DATE) BETWEEN '" + _gPunchFrmDate + "' AND '" + _gPunchToDate + "' ";

        if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
            sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'

        sql += " )  Cases_Required_Cancel ";

        sql += " FROM MOBINT.MCR_INPUT_DETAILS a  ";
        sql += " WHERE 1=1 ";

        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

        if (_gPunchFrmDate != "" && _gPunchToDate != "")
            sql += " AND TRUNC(PUNCH_DATE) BETWEEN '" + _gPunchFrmDate + "' AND '" + _gPunchToDate + "' ";

        if (_gddlDivision != "")
            sql += " AND division='" + _gddlDivision + "' ";
        else
            sql += " AND division in ('" + _gDivision + "') ";

        sql += "  and division in (SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE STATUS='Y') ";

        if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
            sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";  //AND ROLE_TYPE = '" + _sRole + "'


        if (_gCompany != "")
            sql += " and comp_code='" + _gCompany + "'";

        sql += " GROUP BY DIVISION, VENDOR_CODE ";



        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    public DataTable getMMG_ISU_Details(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gDivision, string _gddlDivision, string _gType, string _gCompany,
                                        string _gPunchFrmDate, string _gPunchToDate, string _sOrderType, string _sActType, string _gID, string _sRole)
    {
        string sql = "";
        if (_gType == "CASESCOUNT")
        {
            sql = "select COMP_CODE COMPANY, TO_CHAR(PSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, ORDERID, METER_NO, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=DIVISION)DIVISION, VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, POLE_NO, (SELECT INSTALLER_NAME FROM MOBINT.MCR_INSTALLER_MST WHERE INSTALLER_ID=PUNCH_BY AND ROWNUM<2)PUNCH_BY, PUNCH_DATE PUNCHDATE, (SELECT reason FROM MOBINT.MCR_ORDER_CANCEL  WHERE a.orderid=ORDERID AND ROWNUM<2) Cancel_Reason , ";

            sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.AUART = B.ORDER_TYPE)) ORDER_TYPE,";
            sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.ILART_ACTIVITY_TYPE = B.PM_ACTIVTY)) PM_ACT,";
            sql += "'' OLD_M_READING,'' OLD_MR_KWH,'' OLD_MR_KW,'' MRKVAH_OLD,'' OLD_MR_KVA,'' INSTALLEDCABLE_OLD,'' CABLESIZE_OLD,     ";
            sql += "  '' DRUMSIZE_OLD,'' CABLEINSTALLTYPE_OLD,'' RUNNINGLENGTHFROM_OLD,'' RUNNINGLENGTHTO_OLD,'' CABLELENGTH_OLD,'' OUTPUTBUSLENGTH_OLD,'' REM_TERMINAL_SEAL, ";
            sql += " '' REM_OTHER_SEAL, '' REM_BOX_SEAL1,'' REM_BOX_SEAL2, '' REM_BUSBAR_SEAL1,'' REM_BUSBAR_SEAL2,'' BOX_OLD,'' GLANDS_OLD,'' TCOVER_OLD,'' BRASSSCREW_OLD,  ";
            sql += "  '' BUSBAR_OLD,'' THIMBLE_OLD,'' SADDLE_OLD,'' GUNNYBAG_OLD,'' GUNNYBAGSEAL_OLD,'' LABTESTING_DATE_OLD,'' METERRELOCATE_OLD ";

            sql += " from MOBINT.MCR_INPUT_DETAILS a ";
            sql += " WHERE 1=1 ";
            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
            if (_gPunchFrmDate != "" && _gPunchToDate != "")
                sql += " AND TRUNC(PUNCH_DATE) BETWEEN '" + _gPunchFrmDate + "' AND '" + _gPunchToDate + "' ";

            if (_gddlDivision != "")
                sql += " AND division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";            

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";

            if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
                sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'

        }

        if (_gType == "METERSCONSUMED")
        {
            sql = "select COMP_CODE COMPANY, TO_CHAR(PSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, a.ORDERID, METER_NO, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION)DIVISION, a.VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, POLE_NO, (SELECT INSTALLER_NAME FROM MOBINT.MCR_INSTALLER_MST WHERE INSTALLER_ID=PUNCH_BY AND ROWNUM<2)PUNCH_BY, PUNCH_DATE PUNCHDATE, (SELECT reason FROM MOBINT.MCR_ORDER_CANCEL  WHERE a.orderid=ORDERID AND ROWNUM<2) Cancel_Reason,  ";

            sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.AUART = B.ORDER_TYPE)) ORDER_TYPE,";
            sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.ILART_ACTIVITY_TYPE = B.PM_ACTIVTY)) PM_ACT,";
            sql += "OLD_M_READING,OLD_MR_KWH,OLD_MR_KW,MRKVAH_OLD,OLD_MR_KVA,INSTALLEDCABLE_OLD,CABLESIZE_OLD,     ";
            sql += "  DRUMSIZE_OLD,CABLEINSTALLTYPE_OLD,RUNNINGLENGTHFROM_OLD,RUNNINGLENGTHTO_OLD,CABLELENGTH_OLD,OUTPUTBUSLENGTH_OLD,REM_TERMINAL_SEAL, ";
            sql += "  REM_OTHER_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2, REM_BUSBAR_SEAL1,REM_BUSBAR_SEAL2,BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,  ";
            sql += "  BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD,GUNNYBAGSEAL_OLD,LABTESTING_DATE_OLD,METERRELOCATE_OLD ";

            sql += "  FROM MOBINT.MCR_INPUT_DETAILS a, MOBINT.MCR_DETAILS b WHERE a.ORDERID=b.ORDERID  ";
            
            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
            if (_gPunchFrmDate != "" && _gPunchToDate != "")
                sql += " AND TRUNC(PUNCH_DATE) BETWEEN '" + _gPunchFrmDate + "' AND '" + _gPunchToDate + "' ";

            if (_gddlDivision != "")
                sql += " AND a.division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";
            sql += "AND FLAG='C'  ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";

            if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
                sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'

        }

        if (_gType == "METERPENDING")
        {
            sql = "select COMP_CODE COMPANY, TO_CHAR(PSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, ORDERID, METER_NO, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=DIVISION)DIVISION, VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, POLE_NO, (SELECT INSTALLER_NAME FROM MOBINT.MCR_INSTALLER_MST WHERE INSTALLER_ID=PUNCH_BY AND ROWNUM<2)PUNCH_BY, PUNCH_DATE PUNCHDATE, (SELECT reason FROM MOBINT.MCR_ORDER_CANCEL  WHERE a.orderid=ORDERID AND ROWNUM<2) Cancel_Reason , ";

            sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.AUART = B.ORDER_TYPE)) ORDER_TYPE,";
            sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.ILART_ACTIVITY_TYPE = B.PM_ACTIVTY)) PM_ACT,";
            sql += "'' OLD_M_READING,'' OLD_MR_KWH,'' OLD_MR_KW,'' MRKVAH_OLD,'' OLD_MR_KVA,'' INSTALLEDCABLE_OLD,'' CABLESIZE_OLD,     ";
            sql += "  '' DRUMSIZE_OLD,'' CABLEINSTALLTYPE_OLD,'' RUNNINGLENGTHFROM_OLD,'' RUNNINGLENGTHTO_OLD,'' CABLELENGTH_OLD,'' OUTPUTBUSLENGTH_OLD,'' REM_TERMINAL_SEAL, ";
            sql += " '' REM_OTHER_SEAL, '' REM_BOX_SEAL1,'' REM_BOX_SEAL2, '' REM_BUSBAR_SEAL1,'' REM_BUSBAR_SEAL2,'' BOX_OLD,'' GLANDS_OLD,'' TCOVER_OLD,'' BRASSSCREW_OLD,  ";
            sql += "  '' BUSBAR_OLD,'' THIMBLE_OLD,'' SADDLE_OLD,'' GUNNYBAG_OLD,'' GUNNYBAGSEAL_OLD,'' LABTESTING_DATE_OLD,'' METERRELOCATE_OLD ";

            sql += " from MOBINT.MCR_INPUT_DETAILS a ";
            sql += " WHERE 1=1 ";
            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
            if (_gPunchFrmDate != "" && _gPunchToDate != "")
                sql += " AND TRUNC(PUNCH_DATE) BETWEEN '" + _gPunchFrmDate + "' AND '" + _gPunchToDate + "' ";

            if (_gddlDivision != "")
                sql += " AND division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";
            sql += "AND FLAG in ('Y', 'N') ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";

            if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
                sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'

        }

        if (_gType == "METERSCANCEL")
        {
            sql = "select COMP_CODE COMPANY, TO_CHAR(PSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, ORDERID, METER_NO, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=DIVISION)DIVISION, VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, POLE_NO, (SELECT INSTALLER_NAME FROM MOBINT.MCR_INSTALLER_MST WHERE INSTALLER_ID=PUNCH_BY AND ROWNUM<2)PUNCH_BY, PUNCH_DATE PUNCHDATE, (SELECT reason FROM MOBINT.MCR_ORDER_CANCEL  WHERE a.orderid=ORDERID AND ROWNUM<2) Cancel_Reason , ";

            sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.AUART = B.ORDER_TYPE)) ORDER_TYPE,";
            sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.ILART_ACTIVITY_TYPE = B.PM_ACTIVTY)) PM_ACT,";
            sql += "'' OLD_M_READING,'' OLD_MR_KWH,'' OLD_MR_KW,'' MRKVAH_OLD,'' OLD_MR_KVA,'' INSTALLEDCABLE_OLD,'' CABLESIZE_OLD,     ";
            sql += "  '' DRUMSIZE_OLD,'' CABLEINSTALLTYPE_OLD,'' RUNNINGLENGTHFROM_OLD,'' RUNNINGLENGTHTO_OLD,'' CABLELENGTH_OLD,'' OUTPUTBUSLENGTH_OLD,'' REM_TERMINAL_SEAL, ";
            sql += " '' REM_OTHER_SEAL, '' REM_BOX_SEAL1,'' REM_BOX_SEAL2, '' REM_BUSBAR_SEAL1,'' REM_BUSBAR_SEAL2,'' BOX_OLD,'' GLANDS_OLD,'' TCOVER_OLD,'' BRASSSCREW_OLD,  ";
            sql += "  '' BUSBAR_OLD,'' THIMBLE_OLD,'' SADDLE_OLD,'' GUNNYBAG_OLD,'' GUNNYBAGSEAL_OLD,'' LABTESTING_DATE_OLD,'' METERRELOCATE_OLD ";

            sql += " from MOBINT.MCR_INPUT_DETAILS a ";
            sql += " WHERE 1=1 ";
            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
            if (_gPunchFrmDate != "" && _gPunchToDate != "")
                sql += " AND TRUNC(PUNCH_DATE) BETWEEN '" + _gPunchFrmDate + "' AND '" + _gPunchToDate + "' ";

            if (_gddlDivision != "")
                sql += " AND division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";
            sql += "AND FLAG='E' ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";

            if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
                sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'

        }


        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    #endregion

    #region MMG_Tab

    public DataTable getMMG_Tab(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gDivision, string _gddlDivision, string _gCompany,
                                string _gPunchFrmDate, string _gPunchToDate, string _sOrderType, string _sActType, string _gID, string _sRole)
    {
        string sql = " select distinct DIVISION, (SELECT VENDOR_NAME FROM MOBINT.MCR_VENDOR_MST WHERE VENDOR_ID=vendor_code  AND ADDRESS=b.DIV_ID AND ACTIVE_FLAG='Y') VENDOR, vendor_code, sum (CasesGiven) Total_Cases, sum( Cases_Attended_TAB) Cases_Attended_TAB,sum(Updated_ISU)Updated_ISU,sum(Not_Updated_ISU)Not_Updated_ISU, sum(Cases_Cancel)Cases_Cancel from ";
        sql += " (SELECT (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION) DIVISION, a.DIVISION DIV_ID, VENDOR_CODE,ORDERID, COUNT(1) CasesGiven,  (select count(1) from MOBINT.MCR_DETAILS b where A.ORDERID = B.ORDERID) Cases_Attended_TAB, (select count(1) from MOBINT.MCR_DETAILS b where A.ORDERID = B.ORDERID and SAP_FLAG='Y') Updated_ISU, ";
        sql += " (select count(1) from MOBINT.MCR_DETAILS b where A.ORDERID = B.ORDERID and SAP_FLAG='N') Not_Updated_ISU, ";
        sql += " (select count(1) from MOBINT.MCR_INPUT_DETAILS b where A.ORDERID = B.ORDERID and FLAG='E' ";
        sql += " ) Cases_Cancel ";
        sql += " FROM MOBINT.MCR_INPUT_DETAILS a  ";
        sql += " WHERE 1=1 ";

        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

        if (_gPunchFrmDate != "" && _gPunchToDate != "")
            sql += " AND TRUNC(PUNCH_DATE) BETWEEN '" + _gPunchFrmDate + "' AND '" + _gPunchToDate + "' ";

        if (_gddlDivision != "")
            sql += " AND division='" + _gddlDivision + "' ";
        else
            sql += " AND division in ('" + _gDivision + "') ";

        if (_gCompany != "")
            sql += " and comp_code='" + _gCompany + "'";

        if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
            sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'

        sql += "  and division in (SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE STATUS='Y') ";

        sql += " GROUP BY DIVISION, a.DIVISION , VENDOR_CODE,ORDERID)b ";
        sql += " group by DIVISION,b.DIV_ID,vendor_code ";


        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    public DataTable getMMG_TAB_Details(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gDivision, string _gddlDivision, string _gType, string _gCompany,
                                        string _gPunchFrmDate, string _gPunchToDate, string _sOrderType, string _sActType, string _gID, string _sRole)
    {
        string sql = "";

        if (_gType == "CASESGIVEN")
        {
            sql = "select COMP_CODE COMPANY, TO_CHAR(PSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, ORDERID, METER_NO, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION)DIVISION , VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, POLE_NO,  (SELECT INSTALLER_NAME FROM MOBINT.MCR_INSTALLER_MST WHERE INSTALLER_ID=PUNCH_BY AND ROWNUM<2)PUNCH_BY, PUNCH_DATE PUNCHDATE, (SELECT reason FROM MOBINT.MCR_ORDER_CANCEL  WHERE a.orderid=ORDERID AND ROWNUM<2) Cancel_Reason , ";

            sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.AUART = B.ORDER_TYPE)) ORDER_TYPE,";
            sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.ILART_ACTIVITY_TYPE = B.PM_ACTIVTY)) PM_ACT,";
            sql += "'' OLD_M_READING,'' OLD_MR_KWH,'' OLD_MR_KW,'' MRKVAH_OLD,'' OLD_MR_KVA,'' INSTALLEDCABLE_OLD,'' CABLESIZE_OLD,     ";
            sql += "  '' DRUMSIZE_OLD,'' CABLEINSTALLTYPE_OLD,'' RUNNINGLENGTHFROM_OLD,'' RUNNINGLENGTHTO_OLD,'' CABLELENGTH_OLD,'' OUTPUTBUSLENGTH_OLD,'' REM_TERMINAL_SEAL, ";
            sql += " '' REM_OTHER_SEAL, '' REM_BOX_SEAL1,'' REM_BOX_SEAL2, '' REM_BUSBAR_SEAL1,'' REM_BUSBAR_SEAL2,'' BOX_OLD,'' GLANDS_OLD,'' TCOVER_OLD,'' BRASSSCREW_OLD,  ";
            sql += "  '' BUSBAR_OLD,'' THIMBLE_OLD,'' SADDLE_OLD,'' GUNNYBAG_OLD,'' GUNNYBAGSEAL_OLD,'' LABTESTING_DATE_OLD,'' METERRELOCATE_OLD ";

            sql += " from MOBINT.MCR_INPUT_DETAILS a ";
            sql += " WHERE 1=1 ";

            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
            if (_gPunchFrmDate != "" && _gPunchToDate != "")
                sql += " AND TRUNC(PUNCH_DATE) BETWEEN '" + _gPunchFrmDate + "' AND '" + _gPunchToDate + "' ";
            if (_gddlDivision != "")
                sql += " AND a.division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";

            if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
                sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'
        }

        if (_gType == "CASES_ATTENDED_TAB")
        {
            sql = "select COMP_CODE COMPANY, TO_CHAR(PSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, a.ORDERID, METER_NO, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION)DIVISION, a.VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, POLE_NO,  (SELECT INSTALLER_NAME FROM MOBINT.MCR_INSTALLER_MST WHERE INSTALLER_ID=PUNCH_BY AND ROWNUM<2)PUNCH_BY, PUNCH_DATE PUNCHDATE, (SELECT reason FROM MOBINT.MCR_ORDER_CANCEL  WHERE a.orderid=ORDERID AND ROWNUM<2) Cancel_Reason , ";

            sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.AUART = B.ORDER_TYPE)) ORDER_TYPE,";
            sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.ILART_ACTIVITY_TYPE = B.PM_ACTIVTY)) PM_ACT,";
            sql += "OLD_M_READING,OLD_MR_KWH,OLD_MR_KW,MRKVAH_OLD,OLD_MR_KVA,INSTALLEDCABLE_OLD,CABLESIZE_OLD,     ";
            sql += "  DRUMSIZE_OLD,CABLEINSTALLTYPE_OLD,RUNNINGLENGTHFROM_OLD,RUNNINGLENGTHTO_OLD,CABLELENGTH_OLD,OUTPUTBUSLENGTH_OLD,REM_TERMINAL_SEAL, ";
            sql += "  REM_OTHER_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2, REM_BUSBAR_SEAL1,REM_BUSBAR_SEAL2,BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,  ";
            sql += "  BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD,GUNNYBAGSEAL_OLD,LABTESTING_DATE_OLD,METERRELOCATE_OLD ";

            sql += " FROM MOBINT.MCR_INPUT_DETAILS A, MOBINT.MCR_DETAILS b where A.ORDERID = B.ORDERID ";

            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
            if (_gPunchFrmDate != "" && _gPunchToDate != "")
                sql += " AND TRUNC(PUNCH_DATE) BETWEEN '" + _gPunchFrmDate + "' AND '" + _gPunchToDate + "' ";
            if (_gddlDivision != "")
                sql += " AND a.division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'";

            if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
                sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'
        }

        if (_gType == "UPDATED_ISU")
        {
            sql = "select COMP_CODE COMPANY, TO_CHAR(PSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, a.ORDERID, METER_NO, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION)DIVISION, a.VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, POLE_NO,  (SELECT INSTALLER_NAME FROM MOBINT.MCR_INSTALLER_MST WHERE INSTALLER_ID=PUNCH_BY AND ROWNUM<2)PUNCH_BY, PUNCH_DATE PUNCHDATE, (SELECT reason FROM MOBINT.MCR_ORDER_CANCEL  WHERE a.orderid=ORDERID AND ROWNUM<2) Cancel_Reason , ";

            sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.AUART = B.ORDER_TYPE)) ORDER_TYPE,";
            sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.ILART_ACTIVITY_TYPE = B.PM_ACTIVTY)) PM_ACT,";
            sql += "OLD_M_READING,OLD_MR_KWH,OLD_MR_KW,MRKVAH_OLD,OLD_MR_KVA,INSTALLEDCABLE_OLD,CABLESIZE_OLD,     ";
            sql += "  DRUMSIZE_OLD,CABLEINSTALLTYPE_OLD,RUNNINGLENGTHFROM_OLD,RUNNINGLENGTHTO_OLD,CABLELENGTH_OLD,OUTPUTBUSLENGTH_OLD,REM_TERMINAL_SEAL, ";
            sql += "  REM_OTHER_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2, REM_BUSBAR_SEAL1,REM_BUSBAR_SEAL2,BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,  ";
            sql += "  BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD,GUNNYBAGSEAL_OLD,LABTESTING_DATE_OLD,METERRELOCATE_OLD ";

            sql += " FROM MOBINT.MCR_INPUT_DETAILS A, MOBINT.MCR_DETAILS b where A.ORDERID = B.ORDERID ";

            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
            if (_gPunchFrmDate != "" && _gPunchToDate != "")
                sql += " AND TRUNC(PUNCH_DATE) BETWEEN '" + _gPunchFrmDate + "' AND '" + _gPunchToDate + "' ";
            if (_gddlDivision != "")
                sql += " AND a.division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "' and SAP_FLAG='Y'";

            if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
                sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'
        }

        if (_gType == "NOT_UPDATED_ISU")
        {
            sql = "select COMP_CODE COMPANY, TO_CHAR(PSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, a.ORDERID, METER_NO, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION)DIVISION, a.VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, POLE_NO,  (SELECT INSTALLER_NAME FROM MOBINT.MCR_INSTALLER_MST WHERE INSTALLER_ID=PUNCH_BY AND ROWNUM<2)PUNCH_BY, PUNCH_DATE PUNCHDATE, (SELECT reason FROM MOBINT.MCR_ORDER_CANCEL  WHERE a.orderid=ORDERID AND ROWNUM<2) Cancel_Reason , ";

            sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.AUART = B.ORDER_TYPE)) ORDER_TYPE,";
            sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.ILART_ACTIVITY_TYPE = B.PM_ACTIVTY)) PM_ACT,";
            sql += "OLD_M_READING,OLD_MR_KWH,OLD_MR_KW,MRKVAH_OLD,OLD_MR_KVA,INSTALLEDCABLE_OLD,CABLESIZE_OLD,     ";
            sql += "  DRUMSIZE_OLD,CABLEINSTALLTYPE_OLD,RUNNINGLENGTHFROM_OLD,RUNNINGLENGTHTO_OLD,CABLELENGTH_OLD,OUTPUTBUSLENGTH_OLD,REM_TERMINAL_SEAL, ";
            sql += "  REM_OTHER_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2, REM_BUSBAR_SEAL1,REM_BUSBAR_SEAL2,BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,  ";
            sql += "  BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD,GUNNYBAGSEAL_OLD,LABTESTING_DATE_OLD,METERRELOCATE_OLD ";

            sql += " FROM MOBINT.MCR_INPUT_DETAILS A, MOBINT.MCR_DETAILS b where A.ORDERID = B.ORDERID ";

            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
            if (_gPunchFrmDate != "" && _gPunchToDate != "")
                sql += " AND TRUNC(PUNCH_DATE) BETWEEN '" + _gPunchFrmDate + "' AND '" + _gPunchToDate + "' ";
            if (_gddlDivision != "")
                sql += " AND a.division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "' and SAP_FLAG='N'";

            if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
                sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'
        }

        if (_gType == "CASES_CANCEL")
        {
            sql = "select COMP_CODE COMPANY, TO_CHAR(PSTING_DATE, 'dd-Mon-yyyy') POSTING_DATE, ORDERID, METER_NO, (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION)DIVISION, VENDOR_CODE, BP_NO, CA_NO, NAME, ADDRESS, FATHER_NAME, TEL_NO, POLE_NO,  (SELECT INSTALLER_NAME FROM MOBINT.MCR_INSTALLER_MST WHERE INSTALLER_ID=PUNCH_BY AND ROWNUM<2)PUNCH_BY, PUNCH_DATE PUNCHDATE, (SELECT reason FROM MOBINT.MCR_ORDER_CANCEL  WHERE a.orderid=ORDERID AND ROWNUM<2) Cancel_Reason , ";

            sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.AUART = B.ORDER_TYPE)) ORDER_TYPE,";
            sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER B WHERE A.ILART_ACTIVITY_TYPE = B.PM_ACTIVTY)) PM_ACT,";
            sql += "'' OLD_M_READING,'' OLD_MR_KWH,'' OLD_MR_KW,'' MRKVAH_OLD,'' OLD_MR_KVA,'' INSTALLEDCABLE_OLD,'' CABLESIZE_OLD,     ";
            sql += "  '' DRUMSIZE_OLD,'' CABLEINSTALLTYPE_OLD,'' RUNNINGLENGTHFROM_OLD,'' RUNNINGLENGTHTO_OLD,'' CABLELENGTH_OLD,'' OUTPUTBUSLENGTH_OLD,'' REM_TERMINAL_SEAL, ";
            sql += " '' REM_OTHER_SEAL, '' REM_BOX_SEAL1,'' REM_BOX_SEAL2, '' REM_BUSBAR_SEAL1,'' REM_BUSBAR_SEAL2,'' BOX_OLD,'' GLANDS_OLD,'' TCOVER_OLD,'' BRASSSCREW_OLD,  ";
            sql += "  '' BUSBAR_OLD,'' THIMBLE_OLD,'' SADDLE_OLD,'' GUNNYBAG_OLD,'' GUNNYBAGSEAL_OLD,'' LABTESTING_DATE_OLD,'' METERRELOCATE_OLD ";

            sql += " FROM MOBINT.MCR_INPUT_DETAILS A where 1=1 ";

            if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
                sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
            if (_gPunchFrmDate != "" && _gPunchToDate != "")
                sql += " AND TRUNC(PUNCH_DATE) BETWEEN '" + _gPunchFrmDate + "' AND '" + _gPunchToDate + "' ";
            if (_gddlDivision != "")
                sql += " AND division=(SELECT DIST_CD FROM MOBINT.MCR_DIVISION WHERE DIVISION_NAME='" + _gddlDivision + "') ";

            if (_gCompany != "")
                sql += " and comp_code='" + _gCompany + "'  and FLAG='E'";

            if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
                sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";     //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'
        }

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    #endregion

    /// <summary>
    /// Developed by Sanjeev Ranjan on dt 06/02/2019
    /// </summary>
    /// 
    #region MCR_Summary_Rpt"

    public DataTable getMCR_SummaryRpt(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gddlDivision)
    {
        string sql = " SELECT '0' Sno, Total_Cases.division,NVL(Total_Cases.v_count,0) Total_Cases, NVL(Allocation_pending.v_count,0) Pending_for_Allocation, ";
        sql += " 	NVL(Total_Cases.v_count,0) - (NVL(Allocation_pending.v_count,0)+ NVL(Completed_Cases.v_count,0)+NVL(reverted_Cases.v_count,0)) Allotted_Cases, ";
        sql += " NVL(Completed_Cases.v_count,0) Completed_Cases,NVL(reverted_Cases.v_count,0) Reverted_Cases FROM  ";
        sql += " (SELECT division, COUNT(*) V_count FROM mobint.mcr_input_details WHERE PSTING_DATE BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' AND comp_code='BRPL' GROUP BY division) Total_Cases, ";
        sql += " (SELECT division, COUNT(*) V_count FROM mobint.mcr_input_details WHERE PSTING_DATE BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' AND comp_code='BRPL' AND flag='N' GROUP BY division) Allocation_pending,  ";
        sql += " (SELECT division, COUNT(*) V_count FROM mobint.mcr_input_details WHERE PSTING_DATE BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' AND comp_code='BRPL' AND flag='C' GROUP BY division) Completed_Cases, ";
        sql += " (SELECT division, COUNT(*) V_count FROM mobint.mcr_input_details WHERE PSTING_DATE BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' AND comp_code='BRPL' AND flag='E' GROUP BY division) reverted_Cases ";
        sql += " WHERE Total_Cases.division= Allocation_pending.division AND Total_Cases.division= Completed_Cases.division ";
                   
        if (_gddlDivision != "")
            sql += " AND  Total_Cases.division in ('" + _gddlDivision + "') ";

        sql += " AND Total_Cases.division= reverted_Cases.division ORDER BY division";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    #endregion

    /// <summary>
    /// Developed by Sanjeev Ranjan on dt 05/02/2019
    /// </summary>
    /// 
    #region MCR Upload
    public DataTable getMCR_Upload(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gCANo, string _gMeterNo, string _gDivision, string _gSessionDivision,
                                    string _gCompany, string _sOrderType, string _sActType, string _gID, string _sRole, string _sActFrmDT, string _sActToDT)
    {
        string sql = "  SELECT DISTINCT (SELECT division_name FROM MOBINT.MCR_DIVISION WHERE DIST_CD=b.division) division,  (SELECT VENDOR_NAME FROM MOBINT.MCR_VENDOR_MST WHERE VENDOR_ID=b.Vendor_Code  AND ADDRESS=b.DIVISION AND ACTIVE_FLAG='Y') VENDOR, b.Vendor_Code, b.CA_NO, b.bp_no, b.orderid, b.Meter_No, ";
        sql += " (CASE WHEN image1 IS NOT NULL OR image2 IS NOT NULL OR image3 IS NOT NULL THEN 'Y' ELSE 'N' END) PhotoUpload, ";
        sql += " (CASE WHEN IMEAGE_MCR IS NOT NULL THEN 'Y' ELSE 'N' END) MCR_IMAGE,   ";
        sql += " (CASE WHEN (image1 IS NOT NULL OR image2 IS NOT NULL OR image3 IS NOT NULL) AND IMEAGE_MCR IS NOT NULL THEN 'Y' ELSE 'N' END) Both,";
        sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER C WHERE B.AUART = C.ORDER_TYPE)) ORDER_TYPE,";
        sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER C WHERE B.ILART_ACTIVITY_TYPE = C.PM_ACTIVTY)) PM_ACT ";
        sql += " FROM MOBINT.MCR_IMAGE_DETAILS a, MOBINT.MCR_INPUT_DETAILS b   ";
        sql += " where a.orderid=b.orderid ";

        if (_gCANo != "")
            sql += " AND b.ca_no LIKE '%" + _gCANo + "%' ";
        if (_gMeterNo != "")
            sql += " AND b.METER_NO LIKE '%" + _gMeterNo + "%' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(b.PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

        if (_sActFrmDT != "" && _sActToDT != "")
            sql += " AND TRUNC(b.PUNCH_DATE) BETWEEN '" + _sActFrmDT + "' AND '" + _sActToDT + "' ";

        //sql += " AND b.comp_code='" + _gDivision + "' ";
        //sql += " AND b.division in ('" + _gCompany + "') ";

        sql += " AND b.comp_code='" + _gCompany + "' ";

        if(_gDivision !="")
            sql += " AND b.division in ('" + _gDivision + "') ";
        else
            sql += " AND b.division in ('" + _gSessionDivision + "') ";

        if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
            sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gSessionDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gSessionDivision.Replace("'", "") + "') ))";       //AND ROLE_TYPE = '" + _sRole + "'


        sql += " UNION ";

        sql += " SELECT DISTINCT (SELECT division_name FROM MOBINT.MCR_DIVISION WHERE DIST_CD=b.division) division,  (SELECT VENDOR_NAME FROM MOBINT.MCR_VENDOR_MST WHERE VENDOR_ID=b.Vendor_Code  AND ADDRESS=b.DIVISION AND ACTIVE_FLAG='Y') VENDOR, b.Vendor_Code, b.CA_NO, b.bp_no, b.orderid, b.Meter_No, ";
        sql += "  'Y' PhotoUpload, ";
        sql += " 'N' MCR_IMAGE,   ";
        sql += " 'N'  Both, ";
        sql += "UPPER(AUART || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER C WHERE B.AUART = C.ORDER_TYPE)) ORDER_TYPE,";
        sql += "UPPER(ILART_ACTIVITY_TYPE || '-' || (SELECT UNIQUE PM_DESC FROM MOBINT.MCR_ORDER_PM_MASTER C WHERE B.ILART_ACTIVITY_TYPE = C.PM_ACTIVTY)) PM_ACT ";        
        sql += " FROM MOBINT.MCR_order_cancel a, MOBINT.MCR_INPUT_DETAILS b   ";
        sql += " where a.orderid=b.orderid ";

        if (_gCANo != "")
            sql += " AND b.ca_no LIKE '%" + _gCANo + "%' ";
        if (_gMeterNo != "")
            sql += " AND b.METER_NO LIKE '%" + _gMeterNo + "%' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(b.PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

        if (_sActFrmDT != "" && _sActToDT != "")
            sql += " AND TRUNC(b.PUNCH_DATE) BETWEEN '" + _sActFrmDT + "' AND '" + _sActToDT + "' ";

        //sql += " AND b.comp_code='" + _gDivision + "' ";
        //sql += " AND b.division in ('" + _gCompany + "') ";

        sql += " AND b.comp_code='" + _gCompany + "' ";

        if (_gDivision != "")
            sql += " AND b.division in ('" + _gDivision + "') ";
        else
            sql += " AND b.division in ('" + _gSessionDivision + "') ";

        if (_sOrderType != "-ALL-" && _sOrderType != "")
                sql += " AND AUART ='" + _sOrderType + "'";
            else
            sql += " AND AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND ORDER_TYPE IN (SELECT UNIQUE ORDER_TYPE FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gSessionDivision.Replace("'", "") + "') ))";      //AND ROLE_TYPE = '" + _sRole + "'

            if (_sActType != "-ALL-" && _sActType != "")
                sql += " AND ILART_ACTIVITY_TYPE ='" + _sActType + "'";
            else
                sql += " AND ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y' AND PM_ACTIVTY IN (SELECT UNIQUE PM_ACTIVITY FROM MOBINT.MCR_V_D_OTYPE_PMACTMAP WHERE VENDOR_CODE = '" + _gID + "' AND DIVISION IN('" + _gSessionDivision.Replace("'", "") + "') ))";       //AND ROLE_TYPE = '" + _sRole + "'

            DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    public DataTable getMCR_LosseMtr_Upload(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gMeterNo, string _sOrderType, string _sActFrmDT,
                                           string _sActToDT, string _gddlDivision)
    {
        string sql = " SELECT DISTINCT (SELECT division_name FROM MOBINT.MCR_DIVISION WHERE DIST_CD=D.division AND ROWNUM=1) division,LIFNR_ACCOUNT_NO VENDOR_CODE,NAME1_NAME VENDOR, ";
        sql += " 	  D.ORDERID,SERNR_SERIAL_NO METER,(CASE WHEN image1 IS NOT NULL OR image2 IS NOT NULL OR image3 IS NOT NULL THEN 'Y' ELSE 'N' END) PhotoUpload, ";
        sql += " 	  (CASE WHEN IMEAGE_MCR IS NOT NULL THEN 'Y' ELSE 'N' END) MCR_IMAGE,  ";
        sql += " 	  (CASE WHEN (image1 IS NOT NULL OR image2 IS NOT NULL OR image3 IS NOT NULL) AND IMEAGE_MCR IS NOT NULL THEN 'Y' ELSE 'N' END) Both, ";
        sql += " 	  UPPER(ORDER_TYPE || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER C WHERE D.ORDER_TYPE = C.ORDER_TYPE))ORDER_TYPE, ";
        sql += "  PM_ACTIVITY,I.DEVICENO FROM mobint.MCR_LOOSE_METER_DETAILS A , mobint.MCR_DETAILS D, MOBINT.MCR_IMAGE_DETAILS I,MOBINT.MCR_user_details C";
        sql += "  WHERE A.SERNR_SERIAL_NO=D.DEVICENO AND  D.DEVICENO=I.DEVICENO AND c.EMP_ID=d.TAB_LOGIN_ID  AND LM_LOOSEFLAG='LOOSE'  ";

        if (_gMeterNo != "")
            sql += " AND SERNR_SERIAL_NO LIKE '%" + _gMeterNo + "%' ";

        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(A.BUDAT_POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

        if (_sActFrmDT != "" && _sActToDT != "")
            sql += " AND TRUNC(a.PUNCH_DATE) BETWEEN '" + _sActFrmDT + "' AND '" + _sActToDT + "' ";

        if (_gddlDivision != "")
            sql += " and c.DIVISION in ('" + _gddlDivision + "')  ";

        if (_sOrderType != "-ALL-" && _sOrderType != "")
            sql += " AND D.ORDER_TYPE ='" + _sOrderType + "'";

        sql += "  UNION ";

        sql += "  SELECT DISTINCT (SELECT division_name FROM MOBINT.MCR_DIVISION WHERE DIST_CD=D.division AND ROWNUM=1) division,d.VENDOR_CODE VENDOR_CODE, ";
        sql += "  d.VENDOR_NAME VENDOR, D.ORDERID,A.LM_CUSTOMERMETER METER, ";
        sql += "  (CASE WHEN image1 IS NOT NULL OR image2 IS NOT NULL OR image3 IS NOT NULL THEN 'Y' ELSE 'N' END) PhotoUpload, ";
        sql += "  (CASE WHEN IMEAGE_MCR IS NOT NULL THEN 'Y' ELSE 'N' END) MCR_IMAGE, ";
        sql += "  (CASE WHEN (image1 IS NOT NULL OR image2 IS NOT NULL OR image3 IS NOT NULL) AND IMEAGE_MCR IS NOT NULL THEN 'Y' ELSE 'N' END) Both, ";
        sql += "  UPPER(d.ORDER_TYPE || '-' || (SELECT UNIQUE ORDER_DESC FROM MOBINT.MCR_ORDER_PM_MASTER C WHERE D.ORDER_TYPE = C.ORDER_TYPE))ORDER_TYPE, ";
        sql += "  d.PM_ACTIVITY,I.DEVICENO FROM mobint.MCR_DETAILS A , mobint.MCR_DETAILS D, MOBINT.MCR_IMAGE_DETAILS I,MOBINT.MCR_user_details C ";
        sql += "  WHERE A.LM_CUSTOMERMETER=D.DEVICENO AND D.DEVICENO=I.DEVICENO AND c.EMP_ID=d.TAB_LOGIN_ID AND d.LM_LOOSEFLAG='LOOSE' ";

        if (_gMeterNo != "")
            sql += " AND DEVICENO LIKE '%" + _gMeterNo + "%' ";

        if (_sActFrmDT != "" && _sActToDT != "")
            sql += " AND TRUNC(d.MACHINE_ENTRY_DATE) BETWEEN '" + _sActFrmDT + "' AND '" + _sActToDT + "' ";

        if (_gddlDivision != "")
            sql += " and c.DIVISION in ('" + _gddlDivision + "')  ";

        if (_sOrderType != "-ALL-" && _sOrderType != "")
            sql += " AND D.ORDER_TYPE ='" + _sOrderType + "'";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }



    public DataTable getBindOrder_ImageUpload(string _gOrder_ID)
    {
        string sql = " select * from  MOBINT.MCR_IMAGE_DETAILS a where ORDERID='" + _gOrder_ID + "' ";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }


    public DataTable getBindImageUpload(string _gOrder_ID)
    {
        string sql = " select * from  MOBINT.MCR_IMAGE_DETAILS a where DEVICENO='" + _gOrder_ID + "' ";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable getBindCancel_ImageUpload(string _gOrder_ID)
    {
        string sql = " select IMAGE_PATH,PDF_PATH,SIGNATURE_PATH from  MOBINT.MCR_order_cancel a where orderid='" + _gOrder_ID + "' ";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    #endregion

    #region Role Rights

    public int Update_OrderType_PMActivity(string _sActFlag, string _sSAPSendFlag, string _sOrdType, string _sPMActivty)
    {
        string sqlinsert = " UPDATE MOBINT.MCR_ORDER_PM_MASTER SET ACTIVE_FLAG='" + _sActFlag + "',SAP_SENT_FLAG='" + _sSAPSendFlag + "' WHERE ORDER_TYPE='" + _sOrdType + "' and PM_ACTIVTY='" + _sPMActivty + "' ";
        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public DataTable getRoleMstDetails(string _sStatus)
    {
        string sql = "select '0' Role_id, '--Select One--' role_name from dual union select Role_id, role_name from MOBINT.MCR_ROLE_MST where active='Y' ";

        DataTable dt = objUti.ExecuteReader(sql.ToString());
        return dt;
    }

    public DataTable getRoleRightName_IDWise(string _sRoleTYpe)
    {
        string sql = "";
        sql = "SELECT ROLE_NAME FROM MOBINT.MCR_ROLE_MST WHERE ROLE_ID='" + _sRoleTYpe + "' ";

        DataTable dt = objUti.ExecuteReader(sql.ToString());
        return dt;
    }

    public DataTable getRoleRightMst()
    {
        string sql = "";
        sql = "select Page_id, Page_title from MOBINT.MCR_PAGE_MST WHERE NAVIGATE_URL IS NOT NULL ";

        DataTable dt = objUti.ExecuteReader(sql.ToString());
        return dt;
    }

    public DataTable GetOrderType_PMActivityData(string _sOrdType, string _sActType)
    {
        string sql = "";
        sql = "SELECT ORDER_TYPE, ORDER_DESC, PM_ACTIVTY, PM_DESC,ACTIVE_FLAG, SAP_SENT_FLAG FROM MOBINT.MCR_ORDER_PM_MASTER WHERE 1=1";
        if (_sOrdType != "")
            sql += " AND ORDER_TYPE = '" + _sOrdType + "'";
        if (_sActType != "")
            sql += " AND PM_ACTIVTY = '" + _sActType + "'";
        sql += " order by ORDER_TYPE, ORDER_DESC ";

        DataTable dt = objUti.ExecuteReader(sql.ToString());
        return dt;
    }

    public DataTable GetRoleRightData_RoleWise(string _sRole, string _sComapany)
    {
        StringBuilder _sQuery = new StringBuilder();
        _sQuery.Append("SELECT USER_MODULE_CODE,ROLE_AMQ FROM MOBINT.MCR_ROLE_DETAILS WHERE ROLE_ID='" + _sRole + "' and Company='" + _sComapany + "' ");

        DataTable dt = objUti.ExecuteReader(_sQuery.ToString());
        return dt;
    }

    public int DeleteRole_RightAssinMst(string _sRoleID, string _sComapany)
    {
        StringBuilder _sQuery = new StringBuilder();
        _sQuery.Append(" DELETE FROM MOBINT.MCR_ROLE_DETAILS WHERE ROLE_ID='" + _sRoleID + "' and Company='" + _sComapany + "' ");
        return (objUti.ExecuteNonQuery(_sQuery.ToString()));
    }

    public int Insert_RoleAssign(string _sRoleID, string _sUserModule, string _sAction, string _sComapany)
    {
        StringBuilder _sQuery = new StringBuilder();
        _sQuery.Append(" INSERT INTO MOBINT.MCR_ROLE_DETAILS (ROLE_ID, USER_MODULE_CODE, ROLE_AMQ, company) ");
        _sQuery.Append(" VALUES ");
        _sQuery.Append(" ('" + _sRoleID + "','" + _sUserModule + "','" + _sAction + "', '" + _sComapany + "')");

        return (objUti.ExecuteNonQuery(_sQuery.ToString()));
    }

    #endregion

    #region Revert Process

    public int Insert_RevertProcess(string _gORDERID, string _gRINTALLERID, string _gCINTALLERID, string _gREMARKS)
    {
        StringBuilder _sQuery = new StringBuilder();
        _sQuery.Append(" INSERT INTO MOBINT.MCR_revert_process(ORDERID, RINTALLERID, CINTALLERID, REMARKS) VALUES('" + _gORDERID + "', '" + _gRINTALLERID + "', '" + _gCINTALLERID + "', '" + _gREMARKS + "')");

        return (objUti.ExecuteNonQuery(_sQuery.ToString()));
    }

    public int Update_Revert_Process(string _sINSTALLER_ID, string _sORDER_NO)
    {
        StringBuilder _sQuery = new StringBuilder();
        _sQuery.Append("  UPDATE MOBINT.MCR_VEND_ORDER_INST_MAP SET INSTALLER_ID='" + _sINSTALLER_ID + "', ALLOTED_DATE=sysdate WHERE ORDER_NO='" + _sORDER_NO + "' ");

        return (objUti.ExecuteNonQuery(_sQuery.ToString()));
    }


    public int Insert_LooseMtr_RevertProcess(string strMeterNo, string strDocNo, string _gRINTALLERID, string _gCINTALLERID, string _gREMARKS)
    {
        StringBuilder _sQuery = new StringBuilder();
        _sQuery.Append(" INSERT INTO MOBINT.MCR_REVERT_LOSMTR_PROCESS(METER_NO, DOC_NO, RINTALLERID, CINTALLERID, REMARKS) VALUES('" + strMeterNo + "','" + strDocNo + "', '" + _gRINTALLERID + "', '" + _gCINTALLERID + "', '" + _gREMARKS + "')");

        return (objUti.ExecuteNonQuery(_sQuery.ToString()));
    }
    public int Update_Revert_LooseMeter_Process(string _sINSTALLER_ID, string strMeterNo, string strDocNo)
    {
        StringBuilder _sQuery = new StringBuilder();
        _sQuery.Append("  UPDATE MOBINT.MCR_VEND_LOSMTR_INST_MAP SET INSTALLER_ID='" + _sINSTALLER_ID + "', ALLOTED_DATE=sysdate WHERE METER_NO='" + strMeterNo + "'  AND DOC_NO='"+strDocNo+"'");

        return (objUti.ExecuteNonQuery(_sQuery.ToString()));
    }

    #endregion

    #region"Schedular Data"

    public DataTable SehdularData1()
    {
        string sql = " SELECT MAX(ENTRY_DATE)FROM MOBINT.MCR_INPUT_DETAILS ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    public DataTable SehdularData2()
    {
        string sql = " SELECT MAX(MACHINE_ENTRY_DATE)FROM MOBINT.MCR_SEAL_DETAILS ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    public DataTable SehdularData3()
    {
        string sql = " SELECT MAX(ALLOCATE_DATE)FROM MOBINT.MCR_INPUT_DETAILS ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    public DataTable SehdularData4()
    {
        string sql = " SELECT MAX(PUNCH_DATE)FROM MOBINT.MCR_INPUT_DETAILS ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    public DataTable SehdularData5()
    {
        string sql = " SELECT MAX(SAP_UPDATE_DATE)FROM MOBINT.MCR_DETAILS ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    public DataTable SehdularData()
    {
        string sql = " SELECT 'Kitting Details' SechName, MAX(ENTRY_DATE) DateTime,'0'COUNT FROM MOBINT.MCR_INPUT_DETAILS ";
        sql += " UNION ";
        sql += " 	  SELECT 'Seal Details' SechName, MAX(MACHINE_ENTRY_DATE) DateTime,'0'COUNT FROM  MOBINT.MCR_SEAL_DETAILS ";
        sql += " UNION ";
        sql += " 	  SELECT 'Allocated Cases to Installer' SechName,MAX(ALLOCATE_DATE) DateTime,'0'COUNT FROM  MOBINT.MCR_INPUT_DETAILS ";
        sql += " UNION ";
        sql += " 	  SELECT 'Punched Cases through TAB' SechName,MAX(PUNCH_DATE) DateTime,'0'COUNT FROM  MOBINT.MCR_INPUT_DETAILS ";
        sql += " UNION ";
        sql += " 	  SELECT 'Punch Details' SechName,MAX(SAP_UPDATE_DATE) DateTime,'0'COUNT FROM  MOBINT.MCR_DETAILS ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    public DataTable SehdularDataCount1()
    {
        string sql = " 	  SELECT COUNT(*) FROM MOBINT.MCR_INPUT_DETAILS WHERE ENTRY_DATE= (SELECT MAX(ENTRY_DATE) FROM MOBINT.MCR_INPUT_DETAILS) ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    public DataTable SehdularDataCount2()
    {
        string sql = " 	  SELECT COUNT(*) FROM MOBINT.MCR_SEAL_DETAILS WHERE MACHINE_ENTRY_DATE= (SELECT MAX(MACHINE_ENTRY_DATE) FROM MOBINT.MCR_SEAL_DETAILS) ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    public DataTable SehdularDataCount3()
    {
        string sql = " 	  SELECT COUNT(*) FROM MOBINT.MCR_INPUT_DETAILS WHERE ALLOCATE_DATE= (SELECT MAX(ALLOCATE_DATE) FROM MOBINT.MCR_INPUT_DETAILS) ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    public DataTable SehdularDataCount4()
    {
        string sql = " 	  SELECT COUNT(*) FROM MOBINT.MCR_INPUT_DETAILS WHERE PUNCH_DATE= (SELECT MAX(PUNCH_DATE) FROM MOBINT.MCR_INPUT_DETAILS) ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    public DataTable SehdularDataCount5()
    {
        string sql = " 	  SELECT COUNT(*) FROM MOBINT.MCR_DETAILS WHERE SAP_UPDATE_DATE= (SELECT MAX(SAP_UPDATE_DATE) FROM MOBINT.MCR_DETAILS) ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    public DataTable SehdularDataCount1_Details()
    {
        string sql = " 	  SELECT COMP_CODE, PSTING_DATE, ORDERID, METER_NO, DIVISION, VENDOR_CODE,CA_NO, NAME, PLANNER_GROUP,AUART, FLAG, ACCOUNT_CLASS ";
        sql += " 	  FROM MOBINT.MCR_INPUT_DETAILS WHERE ENTRY_DATE= (SELECT MAX(ENTRY_DATE) FROM MOBINT.MCR_INPUT_DETAILS) ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    public DataTable SehdularDataCount2_Details()
    {
        string sql = " 	  SELECT COMP_CODE, DIVISION,  POSTING_DATE, PLANNER_GROUP, SEAL, MATERIAL_CODE, USER_RESPONSIBLE, VENDOR_CODE, ";
        sql += " 	  VENDOR_NAME, CONSUM_SEAL_FLAG FROM MOBINT.MCR_SEAL_DETAILS WHERE MACHINE_ENTRY_DATE=(SELECT MAX(MACHINE_ENTRY_DATE)FROM MOBINT.MCR_SEAL_DETAILS) ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    public DataTable SehdularDataCount3_Details()
    {
        string sql = " 	  SELECT COMP_CODE, PSTING_DATE, ORDERID, METER_NO, DIVISION, VENDOR_CODE,CA_NO, NAME, PLANNER_GROUP,AUART, FLAG, ACCOUNT_CLASS ";
        sql += " 	  FROM MOBINT.MCR_INPUT_DETAILS WHERE ALLOCATE_DATE= (SELECT MAX(ALLOCATE_DATE) FROM MOBINT.MCR_INPUT_DETAILS) ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    public DataTable SehdularDataCount4_Details()
    {
        string sql = " 	  SELECT COMP_CODE, PSTING_DATE, ORDERID, METER_NO, DIVISION, VENDOR_CODE,CA_NO, NAME, PLANNER_GROUP,AUART, FLAG, ACCOUNT_CLASS ";
        sql += " 	  FROM MOBINT.MCR_INPUT_DETAILS WHERE PUNCH_DATE= (SELECT MAX(PUNCH_DATE) FROM MOBINT.MCR_INPUT_DETAILS) ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    public DataTable SehdularDataCount5_Details()
    {
        string sql = " 	  	  SELECT   I.COMP_CODE, I.PSTING_DATE, I.ORDERID, I.METER_NO, I.DIVISION, I.VENDOR_CODE,CA_NO, NAME, PLANNER_GROUP,I.AUART, I.FLAG, I.ACCOUNT_CLASS ";
        sql += " 	 	   FROM MOBINT.MCR_INPUT_DETAILS I, MOBINT.MCR_DETAILS D WHERE I.ORDERID=D.ORDERID AND D.SAP_UPDATE_DATE= (SELECT MAX(SAP_UPDATE_DATE) FROM MOBINT.MCR_DETAILS)  ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    public void SchMCR_Data(string strCircle, string strFrmDate)
    {
        objUti.ExcuteProceudre(strCircle, strFrmDate);
    }

    #endregion

    /// <summary>
    /// Developed by Sanjeev Ranjan on dt 01/02/2019
    /// </summary>
    /// <param name="_gDivisionID"></param>
    /// <returns></returns>
    /// 

    public DataTable getCompleteRpt(string _gCompany, string _gFrom, string _gTo, string _gddlDivision, string _gddlVendorName, string _sOrderType, string _sActType)
    {
        string sql = "  SELECT  UNIQUE COMP_CODE COMPANY_CODE,  A.DIVISION,B.SUB_DIVISION, A.VENDOR_CODE, CA_NO, A.ORDERID ORDER_NO, AUART ORDER_TYPE, A.ACCOUNT_CLASS,  ";
        sql += " START_DATE BASIC_START_DATE, fINISH_DATE bASIE_FINISH_DATE,NAME, FATHER_NAME, ADDRESS, TEL_NO MOBILE_NO, BP_NO, A.POLE_NO,  CABLE_SIZE,  ";
        sql += " CABLE_LENGTH, A.pLANNER_GROUP, B.ACTIVITY_REASON, B.ACTIVITY_DATE, METER_NO, B.OTHERSTICKER, ELCB_INSTALLED, INSTALLEDBUSBAR BUS_BAR_INSTALLED,BB_CABLE_USED,  ";
        sql += " BUSBARSIZE bus_bar_type, BUS_BAR_NO, B_BAR_CABLE_SIZE BUS_BAR_CABLE_SIZE, OUTPUTBUSLENGTH BUS_BAR_CABLE_LENG, DRUM_NUMBER_BB BUS_BAR_DRUM_NO,  ";
        sql += " PM_ACTIVITY,(SELECT NAME FROM MOBINT.MCR_COR_SYS_MST  m WHERE UPPER(m.PM_ACTIVITY)=UPPER(b.PM_ACTIVITY)  AND COR_SYS_TYPE='ORD_LST_MST' AND ROWNUM<2)ACTIVITY_DESC,  ";
        sql += " SANCTIONED_LOAD,TO_CHAR(PSTING_DATE,'dd-Mon-yyyy') Assigned_Vendor_DT, CABLEINSTALLTYPE CABLE_INSTALLED, CABLESIZE2 CABLE_SIZE1,B.CABLE_REQD,B.CABLE_LEN_USED CABLE_TYPE,CABLELENGTH CABLE_LENGTH1,  DURM_NO CABLE_DRUM_NO,  ";
        sql += " RUNNINGLENGTHFROM RUNNING_LENGTH_L1, RUNNINGLENGTHTO RUNNING_LENGTH_L2, OUTPUT_CABLE_LEN_USED Output_Cable_Type, OUTPUTCABLELENGTH  OUTPUT_CABLE_LENGTH,  ";
        sql += " OVERHEAD_UG CABLE_OH_ug,TERMINAL_SEAL Terminal_Seal_1,  OTHER_SEAL Terminal_Seal_2, METERBOXSEAL1 Box_Seal_1, METERBOXSEAL2 Box_Seal_2,  ";
        sql += " BUSBARSEAL1 Bus_Bar_Seal_1,BUSBARSEAL2 Bus_Bar_Seal_2, INSTALLATION METER_INSTALLED_LOCATION, POLENUMBER POLE_NO_TAB, GIS_LONG gis_longitude,  ";
        sql += " GIS_LAT GIS_LATITUDE,MR_KWH, MR_KW, MR_KVAH, MR_KVA,LM_CUSTOMERPOLE,  ";
        //sql += "    (CASE WHEN D.IMAGE1  IS NOT NULL THEN 'Y' ELSE'N'  END)  IMAGE1,  ";
        //sql += "     (CASE WHEN D.IMAGE2 IS NOT NULL THEN 'Y' ELSE'N'  END) IMAGE2,  ";
        //sql += "     (CASE WHEN D.IMAGE3   IS NOT NULL THEN 'Y' ELSE'N'  END) IMAGE3,  ";
        //sql += "       (CASE WHEN D.IMEAGE_MCR  IS NOT NULL THEN 'Y' ELSE'N'  END) IMEAGE_MCR,  ";
        //sql += "         (CASE WHEN D.IMAGE_METERTESTREPORT    IS NOT NULL THEN 'Y' ELSE'N'  END) IMAGE_METERTESTREPORT,  ";
        //sql += "           (CASE WHEN D.IMAGE_LABTESTINGREPORT  IS NOT NULL THEN 'Y' ELSE'N'  END) IMAGE_LABTESTINGREPORT,  ";
        //sql += "            (CASE WHEN D.IMAGE_SIGNATURE    IS NOT NULL THEN 'Y' ELSE'N'  END) IMAGE_SIGNATURE, d.IMAGE1_OLD,d.IMAGE2_OLD, ";
        // , SUBSTR(OVERHEADCABLE,3,2)L-ANGLE, SUBSTR(OVERHEADCABLE,5,2)I-ANGLE, ";

        sql += " SUBSTR(OVERHEADCABLE,1,2)ANCHOR_CONSUMER_END_QTY, SUBSTR(OVERHEADCABLE,3,2)L_ANGLE, SUBSTR(OVERHEADCABLE,5,2)I_ANGLE,  ";
        sql += " SUBSTR(OVERHEADCABLEPOLE,1,2)ANCHOR_POLE_END_QTY,  SUBSTR(OVERHEADCABLEPOLE,3,2)POLE_END, SUBSTR(OVERHEADCABLEPOLE,5,2)PIPE_POLE_END_QTY, ";
        sql += " SUBSTR(ADDITIONALACCESSORIES,1,2)ACCES_GLAND_QTY, SUBSTR(ADDITIONALACCESSORIES,3,2)ACCES_EARTH_NUT_QTY, SUBSTR(ADDITIONALACCESSORIES,5,2)ACCES_THIMBLE_QTY, ";
        sql += " SUBSTR(ADDITIONALACCESSORIES,7,2)ACCES_SADDLE_CLAMP_QTY, SUBSTR(ADDITIONALACCESSORIES,9,2)ACCES_SATELLITE,  SUBSTR(ADDITIONALACCESSORIES,11,2)ACCES_PIERCING,REM_TERMINAL_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2,REM_OTHER_SEAL,REM_BUSBAR_SEAL1, ";
        sql += " REM_BUSBAR_SEAL2,DRUM_NUMBER_BB,RUNNING_LENGTH_FROM_BB,RUNNING_LENGTH_TO_BB,VALINSTALL_TYPE_BB,MRKVAH_OLD, ";
        sql += " CABLESIZE_OLD,BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD, ";
        sql += " GUNNYBAGSEAL_OLD,LABTESTING_DATE_OLD,OLD_MR_KWH,OLD_MR_KVAH,LAB_TSTNG_NTC,OLD_MTR_IF_NOT_AVBL,OLD_MTR_IF_AVBL, ";
        sql += " OLD_MTR_STATUS,RMVD_MTR_BASE_PLT,MTR_LOC_SHIFT,NOTICE_DATE,METER_REMOVED_BY,CAB_REMOVE_FRM_SITE, B.CABLESIZE_OLD CABLE_REMOVE_SIZE,B.CABLELENGTH_OLD CABLE_REMOVE_LENGTH,CABLENOTINSTALLREASON,CAB_RMVD_FRM_SITE, ";
        sql += " IS_GNY_BAG_PREPD, MTR_READ_AVAIL,PUNCH_BY,B.SUBMIT_DATETIME TAB_SUBMIT_DATE, PUNCH_DATE,HAPPY_CODE_GEN HappyCode_BY_SYS, HAPPY_CODE HappyCode_BY_INSTALLER,PUNCH_REMARKS HAPPY_CODE_RMKS,  ";
        sql += " (CASE WHEN (NOHAPPYCODEREASON IS NULL OR NOHAPPYCODEREASON ='Select') THEN '' ELSE NOHAPPYCODEREASOn END) Happy_reason ,  ";
        sql += " LM_LOOSEFLAG, NOHAPPYCODEREASON, SMARTMETERBOOL, SMARTMETERSIMNO, SMARTMETERSIMCODE,  ";
        sql += " SMARTMETER_OLDNUM AS Smart_Old_SimNO, SMARTMETER_RSSICODE AS Smart_RSSI_Code, SMARTMETER_ERRORCODE AS Smart_Error_Code, OLD_METERNO_SERNR Old_Meter,B.TAKEPHOTOGRAPH METER_INS_TYPE, ";
        sql += "   A.ALLOCATE_DATE SMS_SEND_DATE,B.PUNCH_MODE ,B.CORD_INSTALLED,B.DB_TYPE,B.BOX_TYPE,B.EARTHING_CONNECTOR,B.JUBLIEE_CLAMPS,B.NYLON_TIE,B.FASTNER  ";
        sql += " FROM  ";
        sql += " MOBINT.MCR_INPUT_DETAILS A, ";
        sql += " MOBINT.MCR_DETAILS B, mobint.MCR_IMAGE_DETAILS D ";
        sql += " WHERE A.ORDERID=B.ORDERID  AND B.DEVICENO=D.DEVICENO AND A.FLAG = 'C' AND (LM_LOOSEFLAG !='LOOSE' OR LM_LOOSEFLAG IS NULL) AND MCR_PUNCH_FLAG IS NULL";
        if (_gCompany != "")
            sql += " and COMP_CODE='" + _gCompany + "'   ";
        if (_gddlDivision != "")
            sql += " and a.DIVISION in ('" + _gddlDivision + "')  ";
        if (_gddlVendorName != "")
            sql += " and a.vendor_code='" + _gddlVendorName + "'   ";

        if (_sOrderType != "-ALL-")
            sql += " AND a.AUART ='" + _sOrderType + "'";
        else
            sql += " AND a.AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        if (_sActType != "-ALL-")
            sql += " AND a.ILART_ACTIVITY_TYPE ='" + _sActType + "'";
        else
            sql += " AND a.ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        if (_gFrom != "" && _gTo != "")
            sql += " and trunc(Punch_DATE) between '" + _gFrom + "' and '" + _gTo + "'   ";


        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }


    //public DataTable getCompleteExceptRpt(string _gCompany, string _gFrom, string _gTo, string _gddlDivision, string _gddlVendorName, string _sOrderType, string _sActType)
    //{

    //    string sql = "  SELECT  UNIQUE COMP_CODE COMPANY_CODE,  A.DIVISION,B.SUB_DIVISION, A.VENDOR_CODE, CA_NO, A.ORDERID ORDER_NO, AUART ORDER_TYPE, A.ACCOUNT_CLASS,  ";
    //    sql += " START_DATE BASIC_START_DATE, fINISH_DATE bASIE_FINISH_DATE,NAME, FATHER_NAME, ADDRESS, TEL_NO MOBILE_NO, BP_NO, A.POLE_NO,  CABLE_SIZE,  ";
    //    sql += " CABLE_LENGTH, A.pLANNER_GROUP, B.ACTIVITY_REASON, B.ACTIVITY_DATE, METER_NO, B.OTHERSTICKER, ELCB_INSTALLED, INSTALLEDBUSBAR BUS_BAR_INSTALLED,BB_CABLE_USED,  ";
    //    sql += " BUSBARSIZE bus_bar_type, BUS_BAR_NO, B_BAR_CABLE_SIZE BUS_BAR_CABLE_SIZE, OUTPUTBUSLENGTH BUS_BAR_CABLE_LENG, DRUM_NUMBER_BB BUS_BAR_DRUM_NO,  ";
    //    sql += " PM_ACTIVITY,(SELECT PM_ACTIVITY FROM MOBINT.MCR_COR_SYS_MST WHERE UPPER(NAME)=UPPER(b.PM_ACTIVITY)  AND COR_SYS_TYPE='ORD_LST_MST' AND ROWNUM<2)ACTIVITY_TYPE,  ";
    //    sql += " SANCTIONED_LOAD,TO_CHAR(PSTING_DATE,'dd-Mon-yyyy') Assigned_Vendor_DT, CABLEINSTALLTYPE CABLE_INSTALLED, CABLESIZE2 CABLE_SIZE1,B.CABLE_REQD,B.CABLE_LEN_USED CABLE_TYPE,CABLELENGTH CABLE_LENGTH1,  DURM_NO CABLE_DRUM_NO,  ";
    //    sql += " RUNNINGLENGTHFROM RUNNING_LENGTH_L1, RUNNINGLENGTHTO RUNNING_LENGTH_L2, OUTPUT_CABLE_LEN_USED Output_Cable_Type, OUTPUTCABLELENGTH  OUTPUT_CABLE_LENGTH,  ";
    //    sql += " OVERHEAD_UG CABLE_OH_ug,TERMINAL_SEAL Terminal_Seal_1,  OTHER_SEAL Terminal_Seal_2, METERBOXSEAL1 Box_Seal_1, METERBOXSEAL2 Box_Seal_2,  ";
    //    sql += " BUSBARSEAL1 Bus_Bar_Seal_1,BUSBARSEAL2 Bus_Bar_Seal_2, INSTALLATION METER_INSTALLED_LOCATION, POLENUMBER POLE_NO_TAB, GIS_LONG gis_longitude,  ";
    //    sql += " GIS_LAT GIS_LATITUDE,MR_KWH, MR_KW, MR_KVAH, MR_KVA,LM_CUSTOMERPOLE,  ";

    //    sql += " SUBSTR(OVERHEADCABLE,1,2)ANCHOR_CONSUMER_END_QTY, SUBSTR(OVERHEADCABLE,3,2)L_ANGLE, SUBSTR(OVERHEADCABLE,5,2)I_ANGLE,  ";
    //    sql += " SUBSTR(OVERHEADCABLEPOLE,1,2)ANCHOR_POLE_END_QTY,  SUBSTR(OVERHEADCABLEPOLE,3,2)POLE_END, SUBSTR(OVERHEADCABLEPOLE,5,2)PIPE_POLE_END_QTY, ";
    //    sql += " SUBSTR(ADDITIONALACCESSORIES,1,2)ACCES_GLAND_QTY, SUBSTR(ADDITIONALACCESSORIES,3,2)ACCES_EARTH_NUT_QTY, SUBSTR(ADDITIONALACCESSORIES,5,2)ACCES_THIMBLE_QTY, ";
    //    sql += " SUBSTR(ADDITIONALACCESSORIES,7,2)ACCES_SADDLE_CLAMP_QTY, SUBSTR(ADDITIONALACCESSORIES,9,2)ACCES_SATELLITE,  SUBSTR(ADDITIONALACCESSORIES,11,2)ACCES_PIERCING,REM_TERMINAL_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2,REM_OTHER_SEAL,REM_BUSBAR_SEAL1, ";
    //    sql += " REM_BUSBAR_SEAL2,DRUM_NUMBER_BB,RUNNING_LENGTH_FROM_BB,RUNNING_LENGTH_TO_BB,VALINSTALL_TYPE_BB,MRKVAH_OLD, ";
    //    sql += " CABLESIZE_OLD,BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD, ";
    //    sql += " GUNNYBAGSEAL_OLD,LABTESTING_DATE_OLD,OLD_MR_KWH,OLD_MR_KVAH,LAB_TSTNG_NTC,OLD_MTR_IF_NOT_AVBL,OLD_MTR_IF_AVBL, ";
    //    sql += " OLD_MTR_STATUS,RMVD_MTR_BASE_PLT,MTR_LOC_SHIFT,NOTICE_DATE,METER_REMOVED_BY,CAB_REMOVE_FRM_SITE, B.CABLESIZE_OLD CABLE_REMOVE_SIZE,B.CABLELENGTH_OLD CABLE_REMOVE_LENGTH,CABLENOTINSTALLREASON,CAB_RMVD_FRM_SITE, ";
    //    sql += " IS_GNY_BAG_PREPD, MTR_READ_AVAIL,PUNCH_BY,B.SUBMIT_DATETIME TAB_SUBMIT_DATE, PUNCH_DATE,HAPPY_CODE_GEN HappyCode_BY_SYS, HAPPY_CODE HappyCode_BY_INSTALLER,PUNCH_REMARKS HAPPY_CODE_RMKS,  ";
    //    sql += " (CASE WHEN (NOHAPPYCODEREASON IS NULL OR NOHAPPYCODEREASON ='Select') THEN '' ELSE NOHAPPYCODEREASOn END) Happy_reason ,  ";
    //    sql += " LM_LOOSEFLAG, NOHAPPYCODEREASON, SMARTMETERBOOL, SMARTMETERSIMNO, SMARTMETERSIMCODE,  ";
    //    sql += " SMARTMETER_OLDNUM AS Smart_Old_SimNO, SMARTMETER_RSSICODE AS Smart_RSSI_Code, SMARTMETER_ERRORCODE AS Smart_Error_Code, OLD_METERNO_SERNR Old_Meter,B.TAKEPHOTOGRAPH METER_INS_TYPE, ";
    //    sql += "   A.ALLOCATE_DATE SMS_SEND_DATE,B.PUNCH_MODE ";

    //    //sql += " SUBSTR(OVERHEADCABLE,1,2)ANCHOR_CONSUMER_END_QTY, SUBSTR(OVERHEADCABLE,3,2)ANGLE_IRON_CONSUMER_END_QTY, SUBSTR(OVERHEADCABLE,5,2)PIPE_CONSUMER_END_QTY,  ";
    //    //sql += " SUBSTR(OVERHEADCABLEPOLE,1,2)ANCHOR_POLE_END_QTY,  SUBSTR(OVERHEADCABLEPOLE,3,2)ANGLE_IRON_POLE_END_QTY, SUBSTR(OVERHEADCABLEPOLE,5,2)PIPE_POLE_END_QTY, ";
    //    //sql += " SUBSTR(ADDITIONALACCESSORIES,1,2)ACCES_GLAND_QTY, SUBSTR(ADDITIONALACCESSORIES,3,2)ACCES_EARTH_NUT_QTY, SUBSTR(ADDITIONALACCESSORIES,5,2)ACCES_THIMBLE_QTY, ";
    //    //sql += " SUBSTR(ADDITIONALACCESSORIES,7,2)ACCES_SADDLE_CLAMP_QTY,REM_TERMINAL_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2,REM_OTHER_SEAL,REM_BUSBAR_SEAL1, ";
    //    //sql += " REM_BUSBAR_SEAL2,DRUM_NUMBER_BB,RUNNING_LENGTH_FROM_BB,RUNNING_LENGTH_TO_BB,VALINSTALL_TYPE_BB,MRKVAH_OLD, ";
    //    //sql += " CABLESIZE_OLD,BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD, ";
    //    //sql += " GUNNYBAGSEAL_OLD,LABTESTING_DATE_OLD,OLD_MR_KWH,OLD_MR_KVAH,PUNCH_REMARKS,LAB_TSTNG_NTC,OLD_MTR_IF_NOT_AVBL,OLD_MTR_IF_AVBL, ";
    //    //sql += " OLD_MTR_STATUS,RMVD_MTR_BASE_PLT,MTR_LOC_SHIFT,NOTICE_DATE,METER_REMOVED_BY,BB_CABLE_USED,CAB_REMOVE_FRM_SITE, B.CABLESIZE_OLD CABLE_REMOVE_SIZE, B.CABLELENGTH_OLD CABLE_REMOVE_LENGTH, CABLENOTINSTALLREASON,CAB_RMVD_FRM_SITE, ";
    //    //sql += " IS_GNY_BAG_PREPD, MTR_READ_AVAIL,PUNCH_BY,B.SUBMIT_DATETIME TAB_SUBMIT_DATE, PUNCH_DATE,HAPPY_CODE_GEN HappyCode_BY_SYS, HAPPY_CODE HappyCode_BY_INSTALLER,  ";
    //    //sql += " (CASE WHEN (NOHAPPYCODEREASON IS NULL OR NOHAPPYCODEREASON ='Select') THEN '' ELSE NOHAPPYCODEREASOn END) Happy_reason ,  ";
    //    //sql += " LM_LOOSEFLAG, NOHAPPYCODEREASON, SMARTMETERBOOL, SMARTMETERSIMNO, SMARTMETERSIMCODE, OLD_METERNO_SERNR Old_Meter, ";
    //    //sql += "  (CASE WHEN cable_reqd='Y' THEN 'New' ELSE 'Old' END) CABLE_TYPE, A.ALLOCATE_DATE SMS_SEND_DATE,B.PUNCH_MODE ";

    //    sql += " FROM  ";
    //    sql += " MOBINT.MCR_INPUT_DETAILS A, ";
    //    sql += " MOBINT.MCR_DETAILS_TMP B, mobint.MCR_IMAGE_DETAILS_TMP D ";
    //    sql += " WHERE A.ORDERID=B.ORDERID  AND B.DEVICENO=D.DEVICENO AND (LM_LOOSEFLAG !='LOOSE' OR LM_LOOSEFLAG IS NULL) AND MCR_PUNCH_FLAG IS NULL AND B.FLAG = 'U'";
    //    if (_gCompany != "")
    //        sql += " and COMP_CODE='" + _gCompany + "'   ";
    //    if (_gddlDivision != "")
    //        sql += " and a.DIVISION in ('" + _gddlDivision + "')  ";
    //    if (_gddlVendorName != "")
    //        sql += " and a.vendor_code='" + _gddlVendorName + "'   ";

    //    if (_sOrderType != "-ALL-")
    //        sql += " AND a.AUART ='" + _sOrderType + "'";
    //    else
    //        sql += " AND a.AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

    //    if (_sActType != "-ALL-")
    //        sql += " AND a.ILART_ACTIVITY_TYPE ='" + _sActType + "'";
    //    else
    //        sql += " AND a.ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

    //    if (_gFrom != "" && _gTo != "")
    //        sql += " and trunc(Punch_DATE) between '" + _gFrom + "' and '" + _gTo + "'   ";


    //    DataTable dt = objUti.ExecuteReaderMIS(sql);
    //    return dt;
    //}

    public DataTable getCompleteExceptRpt(string _gCompany, string _gFrom, string _gTo, string _gddlDivision, string _gddlVendorName, string _sOrderType, string _sActType)
    {

        string sql = "  SELECT  UNIQUE COMP_CODE COMPANY_CODE,  A.DIVISION,B.SUB_DIVISION, A.VENDOR_CODE, CA_NO, A.ORDERID ORDER_NO, AUART ORDER_TYPE, A.ACCOUNT_CLASS,  ";
        sql += " START_DATE BASIC_START_DATE, fINISH_DATE bASIE_FINISH_DATE,NAME, FATHER_NAME, ADDRESS, TEL_NO MOBILE_NO, BP_NO, A.POLE_NO,  CABLE_SIZE,  ";
        sql += " CABLE_LENGTH, A.pLANNER_GROUP, B.ACTIVITY_REASON, B.ACTIVITY_DATE, METER_NO, B.OTHERSTICKER, ELCB_INSTALLED, INSTALLEDBUSBAR BUS_BAR_INSTALLED,BB_CABLE_USED,  ";
        sql += " BUSBARSIZE bus_bar_type, BUS_BAR_NO, B_BAR_CABLE_SIZE BUS_BAR_CABLE_SIZE, OUTPUTBUSLENGTH BUS_BAR_CABLE_LENG, DRUM_NUMBER_BB BUS_BAR_DRUM_NO,  ";
        sql += " PM_ACTIVITY,(SELECT PM_ACTIVITY FROM MOBINT.MCR_COR_SYS_MST WHERE UPPER(NAME)=UPPER(b.PM_ACTIVITY)  AND COR_SYS_TYPE='ORD_LST_MST' AND ROWNUM<2)ACTIVITY_TYPE,  ";
        sql += " SANCTIONED_LOAD,TO_CHAR(PSTING_DATE,'dd-Mon-yyyy') Assigned_Vendor_DT, CABLEINSTALLTYPE CABLE_INSTALLED, CABLESIZE2 CABLE_SIZE1,B.CABLE_REQD,B.CABLE_LEN_USED CABLE_TYPE,CABLELENGTH CABLE_LENGTH1,  DURM_NO CABLE_DRUM_NO,  ";
        sql += " RUNNINGLENGTHFROM RUNNING_LENGTH_L1, RUNNINGLENGTHTO RUNNING_LENGTH_L2, OUTPUT_CABLE_LEN_USED Output_Cable_Type, OUTPUTCABLELENGTH  OUTPUT_CABLE_LENGTH,  ";
        sql += " OVERHEAD_UG CABLE_OH_ug,TERMINAL_SEAL Terminal_Seal_1,  OTHER_SEAL Terminal_Seal_2, METERBOXSEAL1 Box_Seal_1, METERBOXSEAL2 Box_Seal_2,  ";
        sql += " BUSBARSEAL1 Bus_Bar_Seal_1,BUSBARSEAL2 Bus_Bar_Seal_2, INSTALLATION METER_INSTALLED_LOCATION, POLENUMBER POLE_NO_TAB, GIS_LONG gis_longitude,  ";
        sql += " GIS_LAT GIS_LATITUDE,MR_KWH, MR_KW, MR_KVAH, MR_KVA,LM_CUSTOMERPOLE,  ";

        sql += " SUBSTR(OVERHEADCABLE,1,2)ANCHOR_CONSUMER_END_QTY, SUBSTR(OVERHEADCABLE,3,2)L_ANGLE, SUBSTR(OVERHEADCABLE,5,2)I_ANGLE,  ";
        sql += " SUBSTR(OVERHEADCABLEPOLE,1,2)ANCHOR_POLE_END_QTY,  SUBSTR(OVERHEADCABLEPOLE,3,2)POLE_END, SUBSTR(OVERHEADCABLEPOLE,5,2)PIPE_POLE_END_QTY, ";
        sql += " SUBSTR(ADDITIONALACCESSORIES,1,2)ACCES_GLAND_QTY, SUBSTR(ADDITIONALACCESSORIES,3,2)ACCES_EARTH_NUT_QTY, SUBSTR(ADDITIONALACCESSORIES,5,2)ACCES_THIMBLE_QTY, ";
        sql += " SUBSTR(ADDITIONALACCESSORIES,7,2)ACCES_SADDLE_CLAMP_QTY, SUBSTR(ADDITIONALACCESSORIES,9,2)ACCES_SATELLITE,  SUBSTR(ADDITIONALACCESSORIES,11,2)ACCES_PIERCING,REM_TERMINAL_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2,REM_OTHER_SEAL,REM_BUSBAR_SEAL1, ";
        sql += " REM_BUSBAR_SEAL2,DRUM_NUMBER_BB,RUNNING_LENGTH_FROM_BB,RUNNING_LENGTH_TO_BB,VALINSTALL_TYPE_BB,MRKVAH_OLD, ";
        sql += " CABLESIZE_OLD,BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD, ";
        sql += " GUNNYBAGSEAL_OLD,LABTESTING_DATE_OLD,OLD_MR_KWH,OLD_MR_KVAH,LAB_TSTNG_NTC,OLD_MTR_IF_NOT_AVBL,OLD_MTR_IF_AVBL, ";
        sql += " OLD_MTR_STATUS,RMVD_MTR_BASE_PLT,MTR_LOC_SHIFT,NOTICE_DATE,METER_REMOVED_BY,CAB_REMOVE_FRM_SITE, B.CABLESIZE_OLD CABLE_REMOVE_SIZE,B.CABLELENGTH_OLD CABLE_REMOVE_LENGTH,CABLENOTINSTALLREASON,CAB_RMVD_FRM_SITE, ";
        sql += " IS_GNY_BAG_PREPD, MTR_READ_AVAIL,PUNCH_BY,B.SUBMIT_DATETIME TAB_SUBMIT_DATE, PUNCH_DATE,HAPPY_CODE_GEN HappyCode_BY_SYS, HAPPY_CODE HappyCode_BY_INSTALLER,PUNCH_REMARKS HAPPY_CODE_RMKS,  ";
        sql += " (CASE WHEN (NOHAPPYCODEREASON IS NULL OR NOHAPPYCODEREASON ='Select') THEN '' ELSE NOHAPPYCODEREASOn END) Happy_reason ,  ";
        sql += " LM_LOOSEFLAG, NOHAPPYCODEREASON, SMARTMETERBOOL, SMARTMETERSIMNO, SMARTMETERSIMCODE,  ";
        sql += " SMARTMETER_OLDNUM AS Smart_Old_SimNO, SMARTMETER_RSSICODE AS Smart_RSSI_Code, SMARTMETER_ERRORCODE AS Smart_Error_Code, OLD_METERNO_SERNR Old_Meter,B.TAKEPHOTOGRAPH METER_INS_TYPE, ";
        sql += "   A.ALLOCATE_DATE SMS_SEND_DATE,B.PUNCH_MODE ";

        //sql += " SUBSTR(OVERHEADCABLE,1,2)ANCHOR_CONSUMER_END_QTY, SUBSTR(OVERHEADCABLE,3,2)ANGLE_IRON_CONSUMER_END_QTY, SUBSTR(OVERHEADCABLE,5,2)PIPE_CONSUMER_END_QTY,  ";
        //sql += " SUBSTR(OVERHEADCABLEPOLE,1,2)ANCHOR_POLE_END_QTY,  SUBSTR(OVERHEADCABLEPOLE,3,2)ANGLE_IRON_POLE_END_QTY, SUBSTR(OVERHEADCABLEPOLE,5,2)PIPE_POLE_END_QTY, ";
        //sql += " SUBSTR(ADDITIONALACCESSORIES,1,2)ACCES_GLAND_QTY, SUBSTR(ADDITIONALACCESSORIES,3,2)ACCES_EARTH_NUT_QTY, SUBSTR(ADDITIONALACCESSORIES,5,2)ACCES_THIMBLE_QTY, ";
        //sql += " SUBSTR(ADDITIONALACCESSORIES,7,2)ACCES_SADDLE_CLAMP_QTY,REM_TERMINAL_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2,REM_OTHER_SEAL,REM_BUSBAR_SEAL1, ";
        //sql += " REM_BUSBAR_SEAL2,DRUM_NUMBER_BB,RUNNING_LENGTH_FROM_BB,RUNNING_LENGTH_TO_BB,VALINSTALL_TYPE_BB,MRKVAH_OLD, ";
        //sql += " CABLESIZE_OLD,BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD, ";
        //sql += " GUNNYBAGSEAL_OLD,LABTESTING_DATE_OLD,OLD_MR_KWH,OLD_MR_KVAH,PUNCH_REMARKS,LAB_TSTNG_NTC,OLD_MTR_IF_NOT_AVBL,OLD_MTR_IF_AVBL, ";
        //sql += " OLD_MTR_STATUS,RMVD_MTR_BASE_PLT,MTR_LOC_SHIFT,NOTICE_DATE,METER_REMOVED_BY,BB_CABLE_USED,CAB_REMOVE_FRM_SITE, B.CABLESIZE_OLD CABLE_REMOVE_SIZE, B.CABLELENGTH_OLD CABLE_REMOVE_LENGTH, CABLENOTINSTALLREASON,CAB_RMVD_FRM_SITE, ";
        //sql += " IS_GNY_BAG_PREPD, MTR_READ_AVAIL,PUNCH_BY,B.SUBMIT_DATETIME TAB_SUBMIT_DATE, PUNCH_DATE,HAPPY_CODE_GEN HappyCode_BY_SYS, HAPPY_CODE HappyCode_BY_INSTALLER,  ";
        //sql += " (CASE WHEN (NOHAPPYCODEREASON IS NULL OR NOHAPPYCODEREASON ='Select') THEN '' ELSE NOHAPPYCODEREASOn END) Happy_reason ,  ";
        //sql += " LM_LOOSEFLAG, NOHAPPYCODEREASON, SMARTMETERBOOL, SMARTMETERSIMNO, SMARTMETERSIMCODE, OLD_METERNO_SERNR Old_Meter, ";
        //sql += "  (CASE WHEN cable_reqd='Y' THEN 'New' ELSE 'Old' END) CABLE_TYPE, A.ALLOCATE_DATE SMS_SEND_DATE,B.PUNCH_MODE ";

        sql += " FROM  ";
        sql += " MOBINT.MCR_INPUT_DETAILS A, ";
        sql += " MOBINT.MCR_DETAILS_TMP B, mobint.MCR_IMAGE_DETAILS_TMP D ";
        sql += " WHERE A.ORDERID=B.ORDERID  AND B.DEVICENO=D.DEVICENO AND (LM_LOOSEFLAG !='LOOSE' OR LM_LOOSEFLAG IS NULL) AND MCR_PUNCH_FLAG IS NULL AND A.FLAG = 'U'";
        if (_gCompany != "")
            sql += " and COMP_CODE='" + _gCompany + "'   ";
        if (_gddlDivision != "")
            sql += " and a.DIVISION in ('" + _gddlDivision + "')  ";
        if (_gddlVendorName != "")
            sql += " and a.vendor_code='" + _gddlVendorName + "'   ";

        if (_sOrderType != "-ALL-")
            sql += " AND a.AUART ='" + _sOrderType + "'";
        else
            sql += " AND a.AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        if (_sActType != "-ALL-")
            sql += " AND a.ILART_ACTIVITY_TYPE ='" + _sActType + "'";
        else
            sql += " AND a.ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        if (_gFrom != "" && _gTo != "")
            sql += " and trunc(Punch_DATE) between '" + _gFrom + "' and '" + _gTo + "'   ";


        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    #region Gourav InstrallorDetails

    public DataTable getInstrallorDetails(string _gDivisionID)
    {
        string sql = "SELECT VENDOR_CODE FROM MOBINT.MCR_division WHERE dist_cd IN ('" + _gDivisionID + "')  AND STATUS='Y' ORDER BY DIVISION_NAME ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    public DataTable getInstrallorFullDetails(string _gEmpName, string _gDivision)
    {
        string sql = "select * from MOBINT.MCR_vendor_mst where VENDOR_ID in ('" + _gEmpName + "') and  ADDRESS='" + _gDivision + "' ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    public DataTable getActivityDetails()
    {
        string sql = "select unique AUART from MOBINT.MCR_INPUT_DETAILS ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    public DataTable getNCRpt(string _gCompany, string _gFrom, string _gTo, string _gddlDivision, string _gddlVendorName, string _sOrderType, string _sActType)
    {
        string sql = "select (select division_name from MOBINT.MCR_division where dist_cd=b.division) division, VENDOR_CODE, (SELECT VENDOR_NAME FROM MOBINT.MCR_VENDOR_MST WHERE VENDOR_ID=VENDOR_CODE  AND ADDRESS=b.DIVISION  AND ACTIVE_FLAG='Y' AND ROWNUM=1 ) Vender_name, AUART,  ";
        sql += " (select count(1) from MOBINT.MCR_INPUT_DETAILS a where a.DIVISION=b.DIVISION and a.VENDOR_CODE=b.VENDOR_CODE and a.AUART=b.AUART    ";
        if (_gCompany != "")
            sql += " and COMP_CODE='" + _gCompany + "'   ";
        if (_gddlDivision != "")
            sql += " and DIVISION in ('" + _gddlDivision + "')  ";
        if (_gddlVendorName != "")
            sql += " and vendor_code='" + _gddlVendorName + "'   ";

        if (_sOrderType != "-ALL-")
            sql += " AND a.AUART ='" + _sOrderType + "'";
        else
            sql += " AND a.AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        if (_sActType != "-ALL-")
            sql += " AND a.ILART_ACTIVITY_TYPE ='" + _sActType + "'";
        else
            sql += " AND a.ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        if (_gFrom != "" && _gTo != "")
            sql += " and trunc(PSTING_DATE) between '" + _gFrom + "' and '" + _gTo + "'   ";
        sql += " and account_class='SLCC'  ";
        sql += " ) SLCC,  ";
        sql += " (select count(1) from MOBINT.MCR_INPUT_DETAILS a where a.DIVISION=b.DIVISION and a.VENDOR_CODE=b.VENDOR_CODE and a.AUART=b.AUART    ";
        if (_gCompany != "")
            sql += " and COMP_CODE='" + _gCompany + "'   ";
        if (_gddlDivision != "")
            sql += " and DIVISION in ('" + _gddlDivision + "')  ";
        if (_gddlVendorName != "")
            sql += " and vendor_code='" + _gddlVendorName + "'   ";

        if (_sOrderType != "-ALL-")
            sql += " AND a.AUART ='" + _sOrderType + "'";
        else
            sql += " AND a.AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        if (_sActType != "-ALL-")
            sql += " AND a.ILART_ACTIVITY_TYPE ='" + _sActType + "'";
        else
            sql += " AND a.ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        if (_gFrom != "" && _gTo != "")
            sql += " and trunc(PSTING_DATE) between '" + _gFrom + "' and '" + _gTo + "'   ";
        sql += " and account_class='MLCC'  ";
        sql += " ) MLCC,  ";
        sql += " (select count(1) from MOBINT.MCR_INPUT_DETAILS a where a.DIVISION=b.DIVISION and a.VENDOR_CODE=b.VENDOR_CODE and a.AUART=b.AUART    ";
        if (_gCompany != "")
            sql += " and COMP_CODE='" + _gCompany + "'   ";
        if (_gddlDivision != "")
            sql += " and DIVISION in ('" + _gddlDivision + "')  ";
        if (_gddlVendorName != "")
            sql += " and vendor_code='" + _gddlVendorName + "'   ";

        if (_sOrderType != "-ALL-")
            sql += " AND a.AUART ='" + _sOrderType + "'";
        else
            sql += " AND a.AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        if (_sActType != "-ALL-")
            sql += " AND a.ILART_ACTIVITY_TYPE ='" + _sActType + "'";
        else
            sql += " AND a.ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        if (_gFrom != "" && _gTo != "")
            sql += " and trunc(PSTING_DATE) between '" + _gFrom + "' and '" + _gTo + "'   ";
        sql += " ) TOTAL  ";
        sql += " from MOBINT.MCR_INPUT_DETAILS b where   ";
        if (_gCompany != "")
            sql += " COMP_CODE='" + _gCompany + "'   ";
        if (_gddlDivision != "")
            sql += " and DIVISION in ('" + _gddlDivision + "')  ";
        if (_gddlVendorName != "")
            sql += " and vendor_code='" + _gddlVendorName + "'   ";

        if (_sOrderType != "-ALL-")
            sql += " AND b.AUART ='" + _sOrderType + "'";
        else
            sql += " AND b.AUART IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        if (_sActType != "-ALL-")
            sql += " AND b.ILART_ACTIVITY_TYPE ='" + _sActType + "'";
        else
            sql += " AND b.ILART_ACTIVITY_TYPE IN (SELECT DISTINCT PM_ACTIVTY FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        if (_gFrom != "" && _gTo != "")
            sql += " and trunc(PSTING_DATE) between '" + _gFrom + "' and '" + _gTo + "'   ";
        sql += " group by division, VENDOR_CODE, AUART ";
        //sql += " group by division, VENDOR_CODE, AUART, PSTING_DATE ";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }
    #endregion

    #region Graph
    public DataTable getMeterReconciliation_graph(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gDivision, string _gddlDivision, string _gCompany, string _gUser_ID)
    {
        string sql = "SELECT (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION) DIVISION,  (SELECT EMP_NAME FROM MOBINT.MCR_USER_DETAILS WHERE emp_id=VENDOR_CODE) Vender_name, VENDOR_CODE, COUNT(1) Alloted_Order, ";
        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_INPUT_DETAILS WHERE ALLOCATE_BY IS NOT NULL AND PUNCH_BY IS NOT NULL AND FLAG='C' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION and comp_code='" + _gCompany + "' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        sql += " )  Installed_Order, ";
        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_INPUT_DETAILS WHERE ALLOCATE_BY IS NOT NULL AND PUNCH_BY IS NULL AND FLAG='Y' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION and comp_code='" + _gCompany + "' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        sql += " ) Pending_order_at_Installer,  ";
        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_INPUT_DETAILS WHERE ALLOCATE_BY IS NOT NULL AND PUNCH_BY IS NOT NULL AND FLAG='E' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION and comp_code='" + _gCompany + "' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        sql += " )  Cancel_Order_From_Field, ";
        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_INPUT_DETAILS WHERE ALLOCATE_BY IS NULL AND PUNCH_BY IS NULL AND FLAG='N' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION and comp_code='" + _gCompany + "' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        sql += " )  Not_Assigned_Order ";
        sql += " FROM MOBINT.MCR_INPUT_DETAILS a  ";
        sql += " WHERE 1=1 ";

        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(PSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

        if (_gddlDivision != "")
            sql += " AND division='" + _gddlDivision + "' ";
        else
            sql += " AND division in ('" + _gDivision + "') ";

        if (_gCompany != "")
            sql += " and comp_code='" + _gCompany + "'";
        sql += "  and vendor_code='" + _gUser_ID + "' ";
        sql += " GROUP BY DIVISION, VENDOR_CODE ";


        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable getSealReconciliation_Graph(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gDivision, string _gddlDivision, string _gCompany, string _gUser_ID)
    {
        string sql = "SELECT (SELECT DIVISION_NAME FROM MOBINT.MCR_DIVISION WHERE DIST_CD=a.DIVISION) DIVISION, (SELECT EMP_NAME FROM MOBINT.MCR_USER_DETAILS WHERE emp_id=VENDOR_CODE) Vender_name, VENDOR_CODE, COUNT(1) Alloted_Seals, ";
        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_SEAL_DETAILS WHERE CONSUM_SEAL_FLAG='C' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION AND comp_code='" + _gCompany + "' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        sql += " )  Installed_Seals, ";
        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_SEAL_DETAILS WHERE CONSUM_SEAL_FLAG='Y' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION AND comp_code='" + _gCompany + "' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        sql += " ) Pending_Seals_at_Installer,    ";
        sql += " (SELECT COUNT(1) FROM MOBINT.MCR_SEAL_DETAILS WHERE CONSUM_SEAL_FLAG='N' AND VENDOR_CODE=a.VENDOR_CODE AND DIVISION=a.DIVISION AND comp_code='" + _gCompany + "' ";
        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";
        sql += ")  Not_Assigned_Seals   ";
        sql += " FROM MOBINT.MCR_SEAL_DETAILS a  ";
        sql += " WHERE 1=1 ";

        if (_gPSTING_From_DATE != "" && _gPSTING_To_DATE != "")
            sql += " AND TRUNC(POSTING_DATE) BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' ";

        if (_gddlDivision != "")
            sql += " AND division='" + _gddlDivision + "' ";
        else
            sql += " AND division in ('" + _gDivision + "') ";

        if (_gCompany != "")
            sql += " and comp_code='" + _gCompany + "'";

        sql += " GROUP BY DIVISION, VENDOR_CODE ";


        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }
    #endregion

    #region Loose Meter Punching


    public DataTable getScheme_DivWise(string _gDivisionID)
    {
        string sql = "SELECT '-ALL-' WBS_SCHEME_NO FROM DUAL UNION SELECT WBS_SCHEME_NO FROM MOBINT.MCR_DIVISION_SCHEME_MST WHERE  STATUS='Y' AND  WBS_SCHEME_NO IS NOT NULL ";

        if (_gDivisionID != "")
            sql += "  AND   DIST_CD IN ('" + _gDivisionID + "')  ";

        sql +="  ORDER BY WBS_SCHEME_NO ";
        
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable GetLosseMeter_OrderData(string _sMeterNo )    //03042018
    {
        string sql = " SELECT ORDER_TYPE,PM_ACTIVITY, LM_CUSTOMERCA,DEVICENO,LM_CUSTOMERMETER ";
        sql += "    FROM MOBINT.MCR_DETAILS WHERE BWART_MOVEMENT_TYPE='941' ";

        if (_sMeterNo != "")
            sql += " AND  DEVICENO='" + _sMeterNo + "' ";    

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable GetLooseMeter_TypeMaster(string _sPMActType)
    {
        string sql = " 	SELECT PM_ACTIVITY FROM MOBINT.MCR_COR_SYS_MST WHERE COR_SYS_TYPE='ORD_LST_MST' and UPPER(NAME)=UPPER('" + _sPMActType + "') ";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable GetLosseMeter_DetailsData_CaseWise(string _sMeterNo, string _sMatDocNo)    //03042018
    {
        string sql = " SELECT DIVISION, DEVICENO, D.ORDERID,  INSTALLEDBUSBAR,  CA, INSTALLATION,  ORDER_TYPE,  ELCB_INSTALLED, ACTIVITY_DATE, BUS_BAR_NO, ";
        sql += "   ACTIVITY_REASON, MR_KWH, MR_KW, MR_KVAH, MR_KVA, DURM_NO, DRUMSIZE, RUNNINGLENGTHFROM, RUNNINGLENGTHTO, CABLELENGTH,  MCR_NO,  CABLESIZE2, ";
        sql += "  CABLEINSTALLTYPE,  TERMINAL_SEAL, METERBOXSEAL1, METERBOXSEAL2, BUSBARSEAL1, BUSBARSEAL2, OTHER_SEAL, REM_TERMINAL_SEA, REM_BOX_SEAL1, REM_BOX_SEAL2, ";
        sql += "   REM_BUSBAR_SEAL1, REM_BUSBAR_SEAL2, REM_OTHER_SEAL,  METERDOWNLOAD, DBLOCKED, EARTHING, HEIGHTOFMETER, ANYJOINTS, OVERHEADCABLE, OVERHEADCABLEPOLE,";
        sql += "   FLOWMADE, ADDITIONALACCESSORIES, TAKEPHOTOGRAPH, D.ENTRY_DATE, TAB_LOGIN_ID, TAB_LN_ID_NAME, OTHERSTICKER, SAP_FLAG,  OUTPUTCABLELENGTH, EARTHINGPOLE, ";
        sql += "   GIS_LAT, GIS_LONG, GIS_STATUS, IMEI_NO, SUBMIT_DATETIME,ELCB_SUBMIT_VAL, METER_SCANNED_VAL, CABLE_REQD, PM_ACTIVITY, OLD_M_READING,";
        sql += "   MRKVAH_OLD, INSTALLEDCABLE_OLD, CABLESIZE_OLD, DRUMSIZE_OLD, CABLEINSTALLTYPE_OLD, RUNNINGLENGTHFROM_OLD, RUNNINGLENGTHTO_OLD, CABLELENGTH_OLD,";
        sql += "   OUTPUTBUSLENGTH_OLD, BOX_OLD, GLANDS_OLD, TCOVER_OLD, BRASSSCREW_OLD, BUSBAR_OLD, THIMBLE_OLD, SADDLE_OLD, GUNNYBAG_OLD, GUNNYBAGSEAL_OLD, ";
        sql += "   LABTESTING_DATE_OLD, LAB_TSTNG_NTC, METERRELOCATE_OLD,OLD_MR_KWH, OLD_MR_KW, OLD_MR_KVAH, OLD_MR_KVA, OLD_MR_KVAH_P,LM_CUSTOMERNAME Cust_Name,  ";
        sql += "    OLD_MR_KVAH_OP, PUNCH_REMARKS,  PUNCH_MODE,PUNCH_BY,   (SELECT LOGIN_NAME FROM mobint.MCR_LOGIN_MST WHERE LOGIN_ID=A.PUNCH_BY AND ROWNUM=1)PUNCH_NAME ,  TF_STCKR_STUS, SMS_FLAG, HAPPY_CODE,  D.ORDER_GEN_DATE ";        
        sql += "    FROM mobint.MCR_LOOSE_METER_DETAILS A , mobint.MCR_DETAILS D  WHERE  D.BWART_MOVEMENT_TYPE='941' AND A.SERNR_SERIAL_NO=D.DEVICENO AND FLAG = 'C' AND A.ORDERID IS NOT NULL and SAP_FLAG='L'";

        if (_sMeterNo != "")
            sql += " AND  DEVICENO='" + _sMatDocNo + "' ";
        //    sql += " AND  MOBINT.MCR_NO='" + _sMeterNo + "' ";
        if (_sMatDocNo != "")
            sql += " AND  DEVICENO='" + _sMatDocNo + "' ";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable getEmpDetails_LosseMeter(string _gID, string _gCompany)    //03042018
    {
        string sql = " SELECT EMP_NAME EMPNAME, EMP_ID, (SELECT NAME FROM MOBINT.MCR_COR_SYS_MST WHERE COR_SYS_TYPE='ASSIGNEDAREA' AND ID=ASSIGNEDAREA AND ACTIVE='Y' ) ASSIGNEDAREA, (SELECT COUNT(1) FROM MOBINT.MCR_LOOSE_METER_DETAILS b WHERE b.FLAG='Y' AND ALLOCATE_TO=a.EMP_ID) MeterAlloted ";
        //sql += "  ,(SELECT COUNT(1) FROM MOBINT.MCR_SEAL_DETAILS WHERE CONSUM_SEAL_FLAG='Y' AND VENDOR_CODE=VENDOR_ID AND ALLOTED_TO=EMP_ID) SealAlloted ";
        sql += " FROM MOBINT.MCR_USER_DETAILS a WHERE a.EMP_TYPE='I' and a.ACTIVE_FLAG='Y'";
        if (_gID != "")
            sql += " AND  a.VENDOR_ID='" + _gID + "' ";

        sql += " ORDER BY EMPNAME ";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable Get_LooseMeter_Details(string _gCompany, string _gmaterialDocNo, string _gSchemeNo,
                            string _gMeterNoFrom, string _gMeterNoTo, string _gPostingDateFrom, string _sPostingDateTo, string _sSelect, string _sVendor)
    {
        string sql = string.Empty;

        sql = " SELECT MBLNR_MATERIAL_DOC_NO, MJAHR_MATERIAL_DOC_YEAR, ZEILE_ITEM_NO, BWART_MOVEMENT_TYPE, CHARG_BATCH_NO, BWTAR_VALUATION_TYPE, ";
        sql += " MATNR_MATERIAL_NO,MAKTX_MATERIAL_DESC, WERKS_PLANT, LGORT_STORAGE_LOC, SERNR_SERIAL_NO, MENGE_QUANTITY, MEINS_BASE_UNIT, ORDERID, ";
        sql += " DMBTR_AMOUNT, RSNUM_NUMBER_REQUIREMENT, LIFNR_ACCOUNT_NO, NAME1_NAME, TO_CHAR(BUDAT_POSTING_DATE,'dd-Mon-yyyy') BUDAT_POSTING_DT,BUDAT_POSTING_DATE,";
        sql += " TO_CHAR(CPUDT_DAY,'dd-Mon-yyyy') CPUDT_DATE,CPUDT_DAY, USNAM_USER_NAME, XBLNR_REFERENCE_DOC,BKTXT_DOC_HEADER_TEXT, BUKRS_COMP_CODE, ";
        sql += "(SELECT UNIQUE C.VENDOR_NAME FROM MOBINT.MCR_VENDOR_MST C WHERE LIFNR_ACCOUNT_NO = C.VENDOR_ID) VENDOR_NAME,PS_PSP_PNR_WBS_ELEMENT SCHEME_NUMBER, ";
        sql += " TO_CHAR(ALLOCATE_DATE,'dd-Mon-yyyy') ALLOTED_DATE, ";
        //sql += "  (SELECT EMP_NAME FROM MOBINT.MCR_USER_DETAILS WHERE EMP_ID=ALLOCATE_TO and rownum=1) ALLOTED_TO FROM MOBINT.MCR_LOOSE_METER_DETAILS ";
        sql += " (SELECT EMP_NAME FROM MOBINT.MCR_USER_DETAILS WHERE EMP_ID = (SELECT INSTALLER_ID FROM (SELECT * FROM MOBINT.MCR_VEND_LOSMTR_INST_MAP ORDER BY ENTRY_DATE DESC) WHERE METER_NO = A.SERNR_SERIAL_NO AND ROWNUM = 1)) ALLOTED_TO FROM MOBINT.MCR_LOOSE_METER_DETAILS A ";

        sql += " WHERE SERNR_SERIAL_NO IN( SELECT  SERNR_SERIAL_NO   FROM MOBINT.MCR_LOOSE_METER_DETAILS WHERE BWART_MOVEMENT_TYPE='941' ";
        sql += " MINUS SELECT  SERNR_SERIAL_NO   FROM MOBINT.MCR_LOOSE_METER_DETAILS WHERE BWART_MOVEMENT_TYPE='942') ";

        if (_sSelect == "1")    //Pending For Allocation
            sql += " AND FLAG = 'N'";
        else if (_sSelect == "2")    //Alloted Cases
        {
            sql += " AND FLAG = 'Y'";

            if (_sVendor != "")
                sql += " AND LIFNR_ACCOUNT_NO = '" + _sVendor + "'";
        }
        else if (_sSelect == "3")    //After Punching Cases
        {
            sql += " AND FLAG = 'C' AND ORDERID is NULL";

            if (_sVendor != "")
                sql += " AND LIFNR_ACCOUNT_NO = '" + _sVendor + "'";
        }
        else if (_sSelect == "4")    //Kitting Cases
        {
            sql += " AND FLAG = 'C' AND ORDERID is NOT NULL";

            if (_sVendor != "")
                sql += " AND LIFNR_ACCOUNT_NO = '" + _sVendor + "'";
        }

        if (_gCompany != "")
            sql += " AND BUKRS_COMP_CODE = '" + _gCompany + "' ";
        if (_gmaterialDocNo != "")
            sql += " AND MBLNR_MATERIAL_DOC_NO = '" + _gmaterialDocNo + "' ";

        if (_gSchemeNo != "")
            sql += " AND PS_PSP_PNR_WBS_ELEMENT in ('" + _gSchemeNo + "') ";

        if (_gMeterNoFrom != "" && _gMeterNoTo != "")
            sql += " AND SERNR_SERIAL_NO BETWEEN LPAD('" + _gMeterNoFrom + "',18,0) AND LPAD('" + _gMeterNoTo + "',18,0)";
        if (_gPostingDateFrom != "" && _sPostingDateTo != "")
            sql += " AND trunc(BUDAT_POSTING_DATE) BETWEEN '" + _gPostingDateFrom + "' AND '" + _sPostingDateTo + "'";
        

        DataTable dt = new DataTable();
        dt = objUti.ExecuteReader(sql);

        return dt;

    }

    public DataTable Get_LooseMeter_OrderDetails(string _gCompany, string _gmaterialDocNo, string _gSchemeNo,
                        string _gMeterNoFromTo, string _gCANoFrmTo, string _gPostingDateFrom, string _sPostingDateTo,
                        string _sSelect, string _sVendor, string _sDivision, string _sOrderType)
    {
        string sql = string.Empty;

        sql = " SELECT (SELECT DIVISION FROM mobint.mcr_user_details WHERE EMP_ID=D.TAB_LOGIN_ID AND ROWNUM=1)DIV_NAME, LIFNR_ACCOUNT_NO VEN_CODE,LM_CUSTOMERCA,LM_CUSTOMERMETER OLD_METER,SERNR_SERIAL_NO NEW_METER, ";
        sql += "  ORDER_TYPE,LM_ACCOUNTCLASS ACC_CLASS,LM_CUSTOMERNAME Cust_Name,LM_CUSTOMERADDRESS Address, ";
        sql += "  PS_PSP_PNR_WBS_ELEMENT SCHEME_NUMBER,TO_CHAR(D.ENTRY_DATE,'dd-Mon-yyyy') ACTIVITY_DATE  , ";
        sql += "  PM_ACTIVITY,MBLNR_MATERIAL_DOC_NO,  BUKRS_COMP_CODE ";
        sql += " FROM mobint.MCR_LOOSE_METER_DETAILS A , mobint.MCR_DETAILS D  WHERE A.SERNR_SERIAL_NO=D.DEVICENO AND FLAG = 'C' AND A.ORDERID IS NULL ";

        if (_sVendor != "")
            sql += " AND LIFNR_ACCOUNT_NO = '" + _sVendor + "'";

        if (_gCompany != "")
            sql += " AND BUKRS_COMP_CODE = '" + _gCompany + "' ";
        if (_gmaterialDocNo != "")
            sql += " AND MBLNR_MATERIAL_DOC_NO = '" + _gmaterialDocNo + "' ";

        if (_gSchemeNo != "")
            sql += " AND PS_PSP_PNR_WBS_ELEMENT in ('" + _gSchemeNo + "') ";

        //if (_gMeterNoFromTo != "")
          //  sql += " AND SUBSTR(SERNR_SERIAL_NO,11,8) in ('" + _gMeterNoFromTo + "') ";

        if (_gMeterNoFromTo != "")
            sql += " AND LM_CUSTOMERMETER in ('" + _gMeterNoFromTo + "') "; //OLD_METER

        if (_gCANoFrmTo != "")
            sql += " AND LM_CUSTOMERCA in ('" + _gCANoFrmTo + "') ";

        if (_gPostingDateFrom != "" && _sPostingDateTo != "")
            sql += " AND trunc(d.MACHINE_ENTRY_DATE) BETWEEN '" + _gPostingDateFrom + "' AND '" + _sPostingDateTo + "'";

        if (_sDivision != "")
            sql += " AND  (SELECT DIVISION FROM mobint.MCR_USER_DETAILS WHERE EMP_ID=D.TAB_LOGIN_ID AND ROWNUM=1)='" + _sDivision + "' ";

        if (_sOrderType != "-ALL-")
            sql += " AND D.ORDER_TYPE ='" + _sOrderType + "'";
        else
            sql += " AND D.ORDER_TYPE IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        sql += "  UNION ";

        sql += " SELECT (SELECT DIVISION FROM mobint.mcr_user_details WHERE EMP_ID=D.TAB_LOGIN_ID AND ROWNUM=1)DIV_NAME, d.vendor_code VEN_CODE, ";
        sql += "  D.LM_CUSTOMERCA,D.LM_CUSTOMERMETER OLD_METER, a.deviceno NEW_METER, d.ORDER_TYPE,d.LM_ACCOUNTCLASS ACC_CLASS,d.LM_CUSTOMERNAME Cust_Name, ";
        sql += "  d. LM_CUSTOMERADDRESS Address, '' SCHEME_NUMBER,TO_CHAR(D.ENTRY_DATE,'dd-Mon-yyyy') ACTIVITY_DATE ,d.PM_ACTIVITY,'', 'BRPL'  ";
        sql += "  FROM mobint.MCR_DETAILS A , mobint.MCR_DETAILS D WHERE a.deviceno=d.lm_customermeter AND D.SAP_FLAG = 'L' AND A.ORDERID IS NULL ";

        if (_gPostingDateFrom != "" && _sPostingDateTo != "")
            sql += " AND trunc(d.MACHINE_ENTRY_DATE) BETWEEN '" + _gPostingDateFrom + "' AND '" + _sPostingDateTo + "'";
        if (_sDivision != "")
            sql += " AND  (SELECT DIVISION FROM mobint.MCR_USER_DETAILS WHERE EMP_ID=D.TAB_LOGIN_ID AND ROWNUM=1)='" + _sDivision + "' ";

        if (_sOrderType != "-ALL-")
            sql += " AND D.ORDER_TYPE ='" + _sOrderType + "'";
        else
            sql += " AND D.ORDER_TYPE IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        //if (_gMeterNoFromTo != "")
          //  sql += " AND SUBSTR(D.deviceno,11,8) in ('" + _gMeterNoFromTo + "') ";
        if (_gMeterNoFromTo != "")
            sql += " AND D.LM_CUSTOMERMETER in ('" + _gMeterNoFromTo + "') "; //OLD_METER

        if (_gCANoFrmTo != "")
            sql += " AND D.LM_CUSTOMERCA in ('" + _gCANoFrmTo + "') ";

        DataTable dt = new DataTable();
        dt = objUti.ExecuteReader(sql);

        return dt;

    }



    public DataTable Get_LooseMeter_KittingOrderDetails(string _gCompany, string _gmaterialDocNo, string _gSchemeNo,
                     string _gMeterNoFrom, string _gMeterNoTo, string _gPostingDateFrom, string _sPostingDateTo, 
                    string _sSelect, string _sVendor, string _sDivision)
    {
        string sql = string.Empty;

        sql = " SELECT (SELECT DIVISION FROM mobint.mcr_user_details WHERE EMP_ID=D.TAB_LOGIN_ID AND ROWNUM=1)DIV_NAME,LIFNR_ACCOUNT_NO VEN_CODE,NAME1_NAME,LM_CUSTOMERCA,LM_CUSTOMERMETER OLD_METER,SERNR_SERIAL_NO NEW_METER, ";
        sql += "  ORDER_TYPE,LM_ACCOUNTCLASS ACC_CLASS,LM_CUSTOMERNAME Cust_Name,LM_CUSTOMERADDRESS Address,MATNR_MATERIAL_NO , ";
        sql += "  PS_PSP_PNR_WBS_ELEMENT SCHEME_NUMBER,TO_CHAR(D.ENTRY_DATE,'dd-Mon-yyyy') ACTIVITY_DATE  , ";
        sql += "  PM_ACTIVITY,MBLNR_MATERIAL_DOC_NO,  BUKRS_COMP_CODE,A.ORDERID,PUNCH_BY,  ";
        sql += "  (SELECT LOGIN_ID FROM mobint.MCR_LOGIN_MST WHERE DIVISION_ID=D.DIVISION AND LOGIN_TYPE='A' AND ACTIVE_FLAG='Y' AND ROWNUM=1) Usr_Resp_ID,  ";
        sql += "  (SELECT LOGIN_NAME FROM mobint.MCR_LOGIN_MST WHERE DIVISION_ID=D.DIVISION AND LOGIN_TYPE='A' AND ACTIVE_FLAG='Y' AND ROWNUM=1) Usr_Resp  ";
        //(SELECT LOGIN_NAME FROM mobint.MCR_LOGIN_MST WHERE LOGIN_ID=A.PUNCH_BY AND ROWNUM=1)PUNCH_NAME ";
        sql += " FROM mobint.MCR_LOOSE_METER_DETAILS A , mobint.MCR_DETAILS D  WHERE A.SERNR_SERIAL_NO=D.DEVICENO AND FLAG = 'C' AND A.ORDERID IS NOT NULL and SAP_FLAG='L'";

        if (_sVendor != "")
            sql += " AND LIFNR_ACCOUNT_NO = '" + _sVendor + "'";

        if (_gCompany != "")
            sql += " AND BUKRS_COMP_CODE = '" + _gCompany + "' ";
        if (_gmaterialDocNo != "")
            sql += " AND MBLNR_MATERIAL_DOC_NO = '" + _gmaterialDocNo + "' ";

        if (_gSchemeNo != "")
            sql += " AND PS_PSP_PNR_WBS_ELEMENT in ('" + _gSchemeNo + "') ";

        if (_gMeterNoFrom != "" && _gMeterNoTo != "")
            sql += " AND SERNR_SERIAL_NO BETWEEN LPAD('" + _gMeterNoFrom + "',18,0) AND LPAD('" + _gMeterNoTo + "',18,0)";

        if (_gPostingDateFrom != "" && _sPostingDateTo != "")
            //sql += " AND BUDAT_POSTING_DATE BETWEEN '" + _gPostingDateFrom + "' AND '" + _sPostingDateTo + "'";
            sql += " AND trunc(PUNCH_DATE) BETWEEN '" + _gPostingDateFrom + "' AND '" + _sPostingDateTo + "'";

        if (_sDivision != "")
            sql += " and division='"+_sDivision+"' ";

        sql += " UNION ";

        sql += " SELECT (SELECT DIVISION FROM mobint.mcr_user_details WHERE EMP_ID=D.TAB_LOGIN_ID AND ROWNUM=1)DIV_NAME,d.vendor_code VEN_CODE,d.vendor_name NAME, ";
        sql += " d.LM_CUSTOMERCA,d.LM_CUSTOMERMETER OLD_METER,A.lm_customermeter NEW_METER, d.ORDER_TYPE,d.LM_ACCOUNTCLASS ACC_CLASS,d.LM_CUSTOMERNAME Cust_Name, ";
        sql += " d.LM_CUSTOMERADDRESS Address,'' , '' SCHEME_NUMBER,TO_CHAR(D.ENTRY_DATE,'dd-Mon-yyyy') ACTIVITY_DATE , ";
        sql += " d. PM_ACTIVITY,'', 'BRPL',A.ORDERID,d.TAB_LN_ID_NAME, (SELECT LOGIN_ID FROM mobint.MCR_LOGIN_MST ";
        sql += " WHERE DIVISION_ID=D.DIVISION AND LOGIN_TYPE='A' AND ACTIVE_FLAG='Y' AND ROWNUM=1) Usr_Resp_ID, (SELECT LOGIN_NAME FROM ";
        sql += " mobint.MCR_LOGIN_MST WHERE DIVISION_ID=D.DIVISION AND LOGIN_TYPE='A' AND ACTIVE_FLAG='Y' AND ROWNUM=1) Usr_Resp ";
        sql += " FROM mobint.MCR_DETAILS A , mobint.MCR_DETAILS D WHERE A.lm_customermeter=D.DEVICENO AND A.ORDERID IS NOT NULL ";
        sql += " AND D.SAP_FLAG='L'  ";

        if (_gMeterNoFrom != "" && _gMeterNoTo != "")
            sql += " AND D.DEVICENO BETWEEN LPAD('" + _gMeterNoFrom + "',18,0) AND LPAD('" + _gMeterNoTo + "',18,0)";

        if (_gPostingDateFrom != "" && _sPostingDateTo != "")
            sql += " AND trunc(a.MACHINE_ENTRY_DATE) BETWEEN '" + _gPostingDateFrom + "' AND '" + _sPostingDateTo + "'";

        if (_sDivision != "")
            sql += " and a.division='" + _sDivision + "' ";

        DataTable dt = new DataTable();
        dt = objUti.ExecuteReader(sql);

        return dt;

    }


    public DataTable getVendorDetails(string _gDivision, string _gCompany)
    {
        string sql = "SELECT UNIQUE VENDOR_ID,VENDOR_NAME FROM MOBINT.MCR_VENDOR_MST WHERE ACTIVE_FLAG = 'Y'";

        if (_gDivision != "")
            sql += " AND ADDRESS IN ('" + _gDivision + "')";
        if (_gCompany != "")
            sql += " AND COMPANY IN ('" + _gCompany + "')";

        sql += " ORDER BY VENDOR_NAME";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public int Assign_Vendor(string _sMeterNo, string _sMatDocNo, string _sCompany, string _sAllocatedBy, string _sIPAddress)
    {
        string sqlinsert = " UPDATE MOBINT.MCR_LOOSE_METER_DETAILS SET ALLOCATED_TO_VENDOR = 'Y', ALLOCATE_DATE = SYSDATE, ALLOCATE_BY='" + _sAllocatedBy + "', ALLOCATE_IP = '" + _sIPAddress + "'";
        sqlinsert += " WHERE SERNR_SERIAL_NO = '" + _sMeterNo + "' AND MBLNR_MATERIAL_DOC_NO = '" + _sMatDocNo + "' AND BUKRS_COMP_CODE = '" + _sCompany + "'";

        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int MapData_Vendor(string _sMeterNo, string _sMatDocNo, string _sCompany, string _sVendorID, string _sAllocatedBy, string _sIPAddress)
    {
        string sqlinsert = " INSERT INTO MOBINT.MCR_LOOSEMTR_VEND_MAP (METER_NO, MATERIAL_DOCNO, COMPANY, VENDOR_ID, ALLOCATE_BY, IP_ADDRESS) ";
        sqlinsert = sqlinsert + "    VALUES  ('" + _sMeterNo + "', '" + _sMatDocNo + "', '" + _sCompany + "', '" + _sVendorID + "', '" + _sAllocatedBy + "', '" + _sIPAddress + "') ";
        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int Assign_LossMeter_OrdInstaller_InputData(string strAllocateTo, string strVendorID, string strMeterNo, string strDocNo)
    {
        string sqlinsert = " UPDATE MOBINT.MCR_LOOSE_METER_DETAILS SET FLAG='Y', ALLOCATE_DATE = SYSDATE,ALLOCATE_BY='" + strVendorID + "',ALLOCATE_TO='" + strAllocateTo + "' WHERE SERNR_SERIAL_NO='" + strMeterNo + "' and MBLNR_MATERIAL_DOC_NO ='" + strDocNo + "'";

        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int Update_OrderGen_Status(string strOrdID, string strMeterNo, string strMatDocNo)
    {
        string sqlinsert = " UPDATE MOBINT.MCR_LOOSE_METER_DETAILS SET ORDERID='" + strOrdID + "', ORDER_GEN_DATE=SYSDATE WHERE ORDERID IS NULL AND SERNR_SERIAL_NO='" + strMeterNo + "' AND MBLNR_MATERIAL_DOC_NO='" + strMatDocNo + "' ";

        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int Update_OrderGen_Details_Data(string strDivision, string strOrdID, string strComCode, string strVenCode, string strDevicNo, string strCANo)
    {
        string sqlinsert = " UPDATE mobint.MCR_DETAILS SET DIVISION='" + strDivision + "',ORDERID='" + strOrdID + "', ORDER_GEN_DATE=SYSDATE, COMPANY_CODE='" + strComCode + "',VENDOR_CODE='" + strVenCode + "' ";
        //sqlinsert = sqlinsert + "  WHERE LM_LOOSEFLAG='LOOSE' AND  DEVICENO='" + strDevicNo + "' AND LM_CUSTOMERCA='" + strCANo + "' ";
        sqlinsert = sqlinsert + "  WHERE LM_LOOSEFLAG='LOOSE' AND LM_CUSTOMERCA='" + strCANo + "' ";

        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int UpdateLoose_Ketting_Status(string strOrdID, string strMeterNo)
    {
        string sqlinsert = " UPDATE MOBINT.MCR_LOOSE_METER_DETAILS SET FLAG='K' WHERE ORDERID ='" + strOrdID + "' AND SERNR_SERIAL_NO='" + strMeterNo + "' ";

        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int UpdateLoose_MCR_Details(string strOrdID, string strMeterNo)
    {
        string sqlinsert = " UPDATE MOBINT.MCR_DETAILS SET SAP_FLAG='N',ORDERID='" + strOrdID + "' WHERE SAP_FLAG='L' AND BWART_MOVEMENT_TYPE='941' AND DEVICENO='"+strMeterNo+"'";
        return objUti.ExecuteNonQuery(sqlinsert);        
    }

    public int UpdateLoose_MCR_ImageDetails(string strOrdID, string strMeterNo)
    {
        string sqlinsert = " UPDATE MOBINT.MCR_IMAGE_DETAILS SET TRANSFER_FLAG='N', ORDERID='" + strOrdID + "' WHERE TRANSFER_FLAG='L'  AND DEVICENO='" + strMeterNo + "' ";
        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int MapData_OrderInst_LosseMeter_InputData(string strMeterNo, string strDocNo,string strVendorID, string strInstallerID)
    {
        string sqlinsert = " INSERT INTO MOBINT.MCR_VEND_LOSMTR_INST_MAP (METER_NO,DOC_NO,  VENDOR_CODE, INSTALLER_ID, ORDER_TYPE) ";
        sqlinsert = sqlinsert + "    VALUES  ('" + strMeterNo + "', '" + strDocNo + "', '" + strVendorID + "', '" + strInstallerID + "', 'LOS') ";
        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public int Insert_LooseMtrDT_InputDetails(string _sCompCode, string _sOrdID, string _sMeterNo)
    {
        string sqlinsert = " INSERT INTO  MOBINT.MCR_input_details (COMP_CODE,ORDERID,METER_NO,FLAG)VALUES('" + _sCompCode + "','"+_sOrdID+"','"+_sMeterNo+"','C') ";

        return objUti.ExecuteNonQuery(sqlinsert);
    }

    public DataTable getCompleteLooseRpt(string _gCompany, string _gFrom, string _gTo, string _gddlDivision, string _gddlVendorName, string _sOrderType,
                                                                                                                                         string _sActType)
    {
        //string sql = "   SELECT  BUKRS_COMP_CODE COMPANY_CODE, C.DIVISION,B.SUB_DIVISION, LIFNR_ACCOUNT_NO VENDOR_CODE, LM_CUSTOMERCA CA_NO, A.ORDERID ORDER_NO, ORDER_TYPE, ";
        //sql += " LM_ACCOUNTCLASS ACCOUNT_CLASS,'' BASIC_START_DATE, '' BASIE_FINISH_DATE, LM_CUSTOMERNAME NAME,'' FATHER_NAME,LM_CUSTOMERADDRESS ADDRESS,  ";
        //sql += "  MOBILE_NO,'' BP_NO, '' POLE_NO,  CABLESIZE2 CABLE_SIZE,CABLELENGTH CABLE_LENGTH,'' PLANNER_GROUP, B.ACTIVITY_REASON,B.ACTIVITY_DATE, SERNR_SERIAL_NO METER_NO,  ";
        //sql += " B.OTHERSTICKER, ELCB_INSTALLED, INSTALLEDBUSBAR BUS_BAR_INSTALLED, BUSBARSIZE bus_bar_type, BUS_BAR_NO, B_BAR_CABLE_SIZE BUS_BAR_CABLE_SIZE,  ";
        //sql += " OUTPUTBUSLENGTH BUS_BAR_CABLE_LENG, DRUM_NUMBER_BB BUS_BAR_DRUM_NO, PM_ACTIVITY,    ";
        //sql += " (SELECT PM_ACTIVITY FROM MOBINT.MCR_COR_SYS_MST WHERE UPPER(NAME)=UPPER(b.PM_ACTIVITY)  AND COR_SYS_TYPE='ORD_LST_MST' AND ROWNUM<2)ACTIVITY_TYPE, ";
        //sql += " '' SANCTIONED_LOAD,   TO_CHAR(ALLOCATE_DATE,'dd-Mon-yyyy') Assigned_Vendor_DT, CABLEINSTALLTYPE CABLE_INSTALLED,  ";
        //sql += " DURM_NO CABLE_DRUM_NO, RUNNINGLENGTHFROM RUNNING_LENGTH_L1, RUNNINGLENGTHTO RUNNING_LENGTH_L2, OUTPUTCABLELENGTH  OUTPUT_CABLE_LENGTH,  ";
        //sql += " B.OVERHEAD_UG CABLE_OH_UG,TERMINAL_SEAL Terminal_Seal_1,  OTHER_SEAL Terminal_Seal_2,METERBOXSEAL1 Box_Seal_1, METERBOXSEAL2 Box_Seal_2,  ";
        //sql += " BUSBARSEAL1 Bus_Bar_Seal_1, BUSBARSEAL2 Bus_Bar_Seal_2, INSTALLATION METER_INSTALLED_LOCATION,  ";
        //sql += " POLENUMBER POLE_NO_TAB, GIS_LONG gis_longitude, GIS_LAT GIS_LATITUDE,  MR_KWH, MR_KW, MR_KVAH, MR_KVA, LM_CUSTOMERPOLE,  ";
        //sql += " (CASE WHEN (SELECT IMAGE1 FROM mobint.MCR_IMAGE_DETAILS WHERE DEVICENO=A.SERNR_SERIAL_NO AND ROWNUM<2) IS NOT NULL THEN 'Y' ELSE 'N' END) IMAGE1,  ";
        //sql += " (CASE WHEN (SELECT IMAGE2 FROM mobint.MCR_IMAGE_DETAILS WHERE DEVICENO=A.SERNR_SERIAL_NO AND ROWNUM<2) IS NOT NULL THEN 'Y' ELSE 'N' END) IMAGE2,  ";
        //sql += " (CASE WHEN (SELECT IMAGE3 FROM mobint.MCR_IMAGE_DETAILS WHERE DEVICENO=A.SERNR_SERIAL_NO AND ROWNUM<2) IS NOT NULL THEN 'Y' ELSE 'N' END) IMAGE3,  ";
        //sql += " (CASE WHEN (SELECT IMEAGE_MCR FROM mobint.MCR_IMAGE_DETAILS WHERE DEVICENO=A.SERNR_SERIAL_NO AND ROWNUM<2) IS NOT NULL THEN 'Y' ELSE 'N' END) IMEAGE_MCR, ";
        //sql += " (CASE WHEN (SELECT IMAGE_METERTESTREPORT FROM mobint.MCR_IMAGE_DETAILS WHERE DEVICENO=A.SERNR_SERIAL_NO AND ROWNUM<2) IS NOT NULL THEN 'Y' ELSE 'N' END) IMAGE_METERTESTREPORT,  ";
        //sql += " (CASE WHEN (SELECT IMAGE_LABTESTINGREPORT FROM mobint.MCR_IMAGE_DETAILS WHERE DEVICENO=A.SERNR_SERIAL_NO AND ROWNUM<2) IS NOT NULL THEN 'Y' ELSE 'N' END) IMAGE_LABTESTINGREPORT,  ";
        //sql += " (CASE WHEN (SELECT IMAGE_SIGNATURE FROM mobint.MCR_IMAGE_DETAILS WHERE DEVICENO=A.SERNR_SERIAL_NO AND ROWNUM<2) IS NOT NULL THEN 'Y' ELSE 'N' END) IMAGE_SIGNATURE, 	 ";
        //sql += " SUBSTR(OVERHEADCABLE,1,2)ANCHOR_CONSUMER_END_QTY, SUBSTR(OVERHEADCABLE,3,2)ANGLE_IRON_CONSUMER_END_QTY, SUBSTR(OVERHEADCABLE,5,2)PIPE_CONSUMER_END_QTY,	   ";
        //sql += " B.ANCHOR_POLE_END_QTY, B.ANGLE_IRON_POLE_END_QTY,B.PIPE_POLE_END_QTY,SUBSTR(ADDITIONALACCESSORIES,1,2)ACCES_GLAND_QTY,  ";
        //sql += " SUBSTR(ADDITIONALACCESSORIES,3,2)ACCES_EARTH_NUT_QTY, SUBSTR(ADDITIONALACCESSORIES,5,2)ACCES_THIMBLE_QTY, 	 ";
        //sql += " SUBSTR(ADDITIONALACCESSORIES,7,2)ACCES_SADDLE_CLAMP_QTY,REM_TERMINAL_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2,REM_OTHER_SEAL,REM_BUSBAR_SEAL1,REM_BUSBAR_SEAL2, ";
        //sql += " DRUM_NUMBER_BB,RUNNING_LENGTH_FROM_BB,RUNNING_LENGTH_TO_BB,VALINSTALL_TYPE_BB,MRKVAH_OLD,IMAGE1_OLD,IMAGE2_OLD,CABLESIZE_OLD, 	 ";
        //sql += " BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD,GUNNYBAGSEAL_OLD,LABTESTING_DATE_OLD, ";
        //sql += " OLD_MR_KWH,OLD_MR_KVAH,PUNCH_REMARKS,LAB_TSTNG_NTC,OLD_MTR_IF_NOT_AVBL,OLD_MTR_IF_AVBL,OLD_MTR_STATUS,RMVD_MTR_BASE_PLT, ";
        //sql += " MTR_LOC_SHIFT,NOTICE_DATE,METER_REMOVED_BY,BB_CABLE_USED,CAB_REMOVE_FRM_SITE,CABLENOTINSTALLREASON,CAB_RMVD_FRM_SITE,IS_GNY_BAG_PREPD,  ";
        //sql += " MTR_READ_AVAIL,PUNCH_BY, PUNCH_DATE, '' HappyCode_BY_SYS, '' HAPPYCODE_BY_INSTALLER,'' HAPPY_REASON ,LM_LOOSEFLAG,  ";
        //sql += " NOHAPPYCODEREASON, SMARTMETERBOOL, SMARTMETERSIMNO, SMARTMETERSIMCODE,LM_CUSTOMERMETER Old_Meter, ";
        //sql += " (CASE WHEN cable_reqd='Y' THEN 'New' ELSE 'Old' END) CABLE_TYPE, '' SMS_SEND_DATE ";
        //sql += " FROM  ";
        //sql += " MOBINT.MCR_LOOSE_METER_DETAILS A, ";
        //sql += " MOBINT.MCR_DETAILS B, MOBINT.MCR_user_details C";
        //sql += " WHERE A.SERNR_SERIAL_NO=B.DEVICENO AND c.EMP_ID=b.TAB_LOGIN_ID";

        //if (_gCompany != "")
        //    sql += " and BUKRS_COMP_CODE='" + _gCompany + "'   ";
        //if (_gddlDivision != "")
        //    sql += " and c.DIVISION in ('" + _gddlDivision + "')  ";
        //if (_gddlVendorName != "")
        //    sql += " and a.LIFNR_ACCOUNT_NO='" + _gddlVendorName + "'   ";

        //if (_sOrderType != "-ALL-")
        //    sql += " AND b.ORDER_TYPE ='" + _sOrderType + "'";
        //else
        //    sql += " AND b.ORDER_TYPE IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        //if (_gFrom != "" && _gTo != "")
        //    sql += " and trunc(Punch_DATE) between '" + _gFrom + "' and '" + _gTo + "'   ";

        //sql += " UNION ";

        string _sql = " SELECT UNIQUE 'BRPL' COMPANY_CODE,C.DIVISION,B.SUB_DIVISION, VENDOR_CODE,LM_CUSTOMERCA CA_NO, B.ORDERID ORDER_NO,ORDER_TYPE, LM_ACCOUNTCLASS ACCOUNT_CLASS, ";
        _sql += " '' BASIC_START_DATE, '' BASIE_FINISH_DATE, LM_CUSTOMERNAME NAME,'' FATHER_NAME,LM_CUSTOMERADDRESS ADDRESS, MOBILE_NO,'' BP_NO,'' POLE_NO, ";
        _sql += "  CABLESIZE2 CABLE_SIZE,CABLELENGTH CABLE_LENGTH,'' PLANNER_GROUP,B.ACTIVITY_REASON,B.ACTIVITY_DATE, B.Deviceno METER_NO, B.OTHERSTICKER, ";
        _sql += " ELCB_INSTALLED, INSTALLEDBUSBAR BUS_BAR_INSTALLED,BB_CABLE_USED,BUSBARSIZE bus_bar_type, BUS_BAR_NO, B_BAR_CABLE_SIZE BUS_BAR_CABLE_SIZE, ";
        _sql += " OUTPUTBUSLENGTH BUS_BAR_CABLE_LENG, DRUM_NUMBER_BB BUS_BAR_DRUM_NO,   ";
        _sql += " (SELECT PM_ACTIVITY FROM MOBINT.MCR_COR_SYS_MST WHERE UPPER(NAME)=UPPER(b.PM_ACTIVITY) AND COR_SYS_TYPE='ORD_LST_MST' AND ROWNUM<2) PM_ACTIVITY,PM_ACTIVITY ACTIVITY_DESC, ";
        _sql += " '' SANCTIONED_LOAD, TO_CHAR(MACHINE_ENTRY_DATE,'dd-Mon-yyyy') Assigned_Vendor_DT,CABLEINSTALLTYPE CABLE_INSTALLED, CABLESIZE2 CABLE_SIZE1,B.CABLE_REQD,B.CABLE_LEN_USED CABLE_TYPE,CABLELENGTH CABLE_LENGTH1, DURM_NO CABLE_DRUM_NO, ";
        _sql += " RUNNINGLENGTHFROM RUNNING_LENGTH_L1, RUNNINGLENGTHTO RUNNING_LENGTH_L2,OUTPUT_CABLE_LEN_USED Output_Cable_Type,OUTPUTCABLELENGTH OUTPUT_CABLE_LENGTH, B.OVERHEAD_UG CABLE_OH_UG, ";
        _sql += " TERMINAL_SEAL Terminal_Seal_1, OTHER_SEAL Terminal_Seal_2,METERBOXSEAL1 Box_Seal_1, METERBOXSEAL2 Box_Seal_2, BUSBARSEAL1 Bus_Bar_Seal_1, ";
        _sql += " BUSBARSEAL2 Bus_Bar_Seal_2,INSTALLATION METER_INSTALLED_LOCATION, POLENUMBER POLE_NO_TAB, GIS_LONG gis_longitude, GIS_LAT GIS_LATITUDE, ";
        _sql += " MR_KWH, MR_KW, MR_KVAH, MR_KVA, LM_CUSTOMERPOLE, ";
        //_sql += "  (CASE WHEN D.IMAGE1  IS NOT NULL THEN 'Y' ELSE'N'  END)  IMAGE1, ";
        //_sql += " (CASE WHEN D.IMAGE2 IS NOT NULL THEN 'Y' ELSE'N'  END) IMAGE2, ";
        //_sql += "  (CASE WHEN D.IMAGE3   IS NOT NULL THEN 'Y' ELSE'N'  END) IMAGE3, ";
        //_sql += "  (CASE WHEN D.IMEAGE_MCR  IS NOT NULL THEN 'Y' ELSE'N'  END) IMEAGE_MCR, ";
        //_sql += " 			  (CASE WHEN D.IMAGE_METERTESTREPORT    IS NOT NULL THEN 'Y' ELSE'N'  END) IMAGE_METERTESTREPORT, ";
        //_sql += " 			    (CASE WHEN D.IMAGE_LABTESTINGREPORT  IS NOT NULL THEN 'Y' ELSE'N'  END) IMAGE_LABTESTINGREPORT, "; 11 22 33 44 55 66
        //_sql += " 				  (CASE WHEN D.IMAGE_SIGNATURE    IS NOT NULL THEN 'Y' ELSE'N'  END) IMAGE_SIGNATURE, B.IMAGE1_OLD,B.IMAGE2_OLD, ";
        _sql += " SUBSTR(OVERHEADCABLE,1,2)ANCHOR_CONSUMER_END_QTY, SUBSTR(OVERHEADCABLE,3,2)L_ANGLE, SUBSTR(OVERHEADCABLE,5,2)I_ANGLE, ";
        _sql += " B.ANCHOR_POLE_END_QTY, B.ANGLE_IRON_POLE_END_QTY POLE_END ,B.PIPE_POLE_END_QTY, SUBSTR(ADDITIONALACCESSORIES,1,2)ACCES_GLAND_QTY, SUBSTR(ADDITIONALACCESSORIES,3,2)ACCES_EARTH_NUT_QTY, ";
        _sql += " SUBSTR(ADDITIONALACCESSORIES,5,2)ACCES_THIMBLE_QTY, SUBSTR(ADDITIONALACCESSORIES,7,2)ACCES_SADDLE_CLAMP_QTY, SUBSTR(ADDITIONALACCESSORIES,9,2)ACCES_SATELLITE,  SUBSTR(ADDITIONALACCESSORIES,11,2)ACCES_PIERCING, REM_TERMINAL_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2,REM_OTHER_SEAL, ";
        _sql += " REM_BUSBAR_SEAL1,REM_BUSBAR_SEAL2, DRUM_NUMBER_BB, RUNNING_LENGTH_FROM_BB,RUNNING_LENGTH_TO_BB,VALINSTALL_TYPE_BB,MRKVAH_OLD,CABLESIZE_OLD, ";
        _sql += " BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD,GUNNYBAGSEAL_OLD, LABTESTING_DATE_OLD, OLD_MR_KWH,OLD_MR_KVAH, ";
        _sql += " LAB_TSTNG_NTC,OLD_MTR_IF_NOT_AVBL,OLD_MTR_IF_AVBL, OLD_MTR_STATUS,RMVD_MTR_BASE_PLT, MTR_LOC_SHIFT,NOTICE_DATE,METER_REMOVED_BY,CAB_REMOVE_FRM_SITE, CABLESIZE_OLD CABLE_REMOVE_SIZE,CABLELENGTH_OLD CABLE_REMOVE_LENGTH,";
        _sql += " CABLENOTINSTALLREASON,CAB_RMVD_FRM_SITE,IS_GNY_BAG_PREPD, MTR_READ_AVAIL,TAB_LOGIN_ID PUNCH_BY,SUBMIT_DATETIME TAB_SUBMIT_DATE, MACHINE_ENTRY_DATE PUNCH_DATE, '' HappyCode_BY_SYS, '' HAPPYCODE_BY_INSTALLER,PUNCH_REMARKS HAPPY_CODE_RMKS, ";
        _sql += " '' HAPPY_REASON ,LM_LOOSEFLAG, NOHAPPYCODEREASON, SMARTMETERBOOL, SMARTMETERSIMNO, SMARTMETERSIMCODE,  ";
        _sql += " SMARTMETER_OLDNUM AS Smart_Old_SimNO, SMARTMETER_RSSICODE AS Smart_RSSI_Code, SMARTMETER_ERRORCODE AS Smart_Error_Code, LM_CUSTOMERMETER Old_Meter,B.TAKEPHOTOGRAPH METER_INS_TYPE, ";
        _sql += "   '' SMS_SEND_DATE,punch_mode,B.CORD_INSTALLED,B.DB_TYPE,B.BOX_TYPE,B.EARTHING_CONNECTOR,B.JUBLIEE_CLAMPS,B.NYLON_TIE,B.FASTNER  ";
        _sql += " FROM MOBINT.MCR_DETAILS B, MOBINT.MCR_user_details C ,mobint.MCR_IMAGE_DETAILS D ";
        _sql += " WHERE c.EMP_ID=b.TAB_LOGIN_ID AND D.DEVICENO=B.DEVICENO  AND LM_LOOSEFLAG='LOOSE' ";

        //if (_gddlVendorName != "")
        //    _sql += " and VENDOR_CODE='" + _gddlVendorName + "' ";

        if (_gddlDivision != "")
            _sql += " and c.DIVISION in ('" + _gddlDivision + "') ";

        if (_sOrderType != "-ALL-")
            _sql += " AND b.ORDER_TYPE ='" + _sOrderType + "'";
        else
            _sql += " AND b.ORDER_TYPE IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        if (_gFrom != "" && _gTo != "")
            _sql += " and trunc(MACHINE_ENTRY_DATE) between '" + _gFrom + "' and '" + _gTo + "'   ";

        DataTable dt = objUti.ExecuteReaderMIS(_sql);
        return dt;
    }

    public DataTable getCompleteLooseExpectionRpt(string _gCompany, string _gFrom, string _gTo, string _gddlDivision, string _gddlVendorName, string _sOrderType,
                                                                                                                                     string _sActType)
    {
        //string sql = "   SELECT  BUKRS_COMP_CODE COMPANY_CODE, C.DIVISION,B.SUB_DIVISION, LIFNR_ACCOUNT_NO VENDOR_CODE, LM_CUSTOMERCA CA_NO, A.ORDERID ORDER_NO, ORDER_TYPE, ";
        //sql += " LM_ACCOUNTCLASS ACCOUNT_CLASS,'' BASIC_START_DATE, '' BASIE_FINISH_DATE, LM_CUSTOMERNAME NAME,'' FATHER_NAME,LM_CUSTOMERADDRESS ADDRESS,  ";
        //sql += "  MOBILE_NO,'' BP_NO, '' POLE_NO,  CABLESIZE2 CABLE_SIZE,CABLELENGTH CABLE_LENGTH,'' PLANNER_GROUP, B.ACTIVITY_REASON,B.ACTIVITY_DATE, SERNR_SERIAL_NO METER_NO,  ";
        //sql += " B.OTHERSTICKER, ELCB_INSTALLED, INSTALLEDBUSBAR BUS_BAR_INSTALLED, BUSBARSIZE bus_bar_type, BUS_BAR_NO, B_BAR_CABLE_SIZE BUS_BAR_CABLE_SIZE,  ";
        //sql += " OUTPUTBUSLENGTH BUS_BAR_CABLE_LENG, DRUM_NUMBER_BB BUS_BAR_DRUM_NO, PM_ACTIVITY,    ";
        //sql += " (SELECT PM_ACTIVITY FROM MOBINT.MCR_COR_SYS_MST WHERE UPPER(NAME)=UPPER(b.PM_ACTIVITY)  AND COR_SYS_TYPE='ORD_LST_MST' AND ROWNUM<2)ACTIVITY_TYPE, ";
        //sql += " '' SANCTIONED_LOAD,   TO_CHAR(ALLOCATE_DATE,'dd-Mon-yyyy') Assigned_Vendor_DT, CABLEINSTALLTYPE CABLE_INSTALLED,  ";
        //sql += " DURM_NO CABLE_DRUM_NO, RUNNINGLENGTHFROM RUNNING_LENGTH_L1, RUNNINGLENGTHTO RUNNING_LENGTH_L2, OUTPUTCABLELENGTH  OUTPUT_CABLE_LENGTH,  ";
        //sql += " B.OVERHEAD_UG CABLE_OH_UG,TERMINAL_SEAL Terminal_Seal_1,  OTHER_SEAL Terminal_Seal_2,METERBOXSEAL1 Box_Seal_1, METERBOXSEAL2 Box_Seal_2,  ";
        //sql += " BUSBARSEAL1 Bus_Bar_Seal_1, BUSBARSEAL2 Bus_Bar_Seal_2, INSTALLATION METER_INSTALLED_LOCATION,  ";
        //sql += " POLENUMBER POLE_NO_TAB, GIS_LONG gis_longitude, GIS_LAT GIS_LATITUDE,  MR_KWH, MR_KW, MR_KVAH, MR_KVA, LM_CUSTOMERPOLE,  ";
        //sql += " (CASE WHEN (SELECT IMAGE1 FROM mobint.MCR_IMAGE_DETAILS WHERE DEVICENO=A.SERNR_SERIAL_NO AND ROWNUM<2) IS NOT NULL THEN 'Y' ELSE 'N' END) IMAGE1,  ";
        //sql += " (CASE WHEN (SELECT IMAGE2 FROM mobint.MCR_IMAGE_DETAILS WHERE DEVICENO=A.SERNR_SERIAL_NO AND ROWNUM<2) IS NOT NULL THEN 'Y' ELSE 'N' END) IMAGE2,  ";
        //sql += " (CASE WHEN (SELECT IMAGE3 FROM mobint.MCR_IMAGE_DETAILS WHERE DEVICENO=A.SERNR_SERIAL_NO AND ROWNUM<2) IS NOT NULL THEN 'Y' ELSE 'N' END) IMAGE3,  ";
        //sql += " (CASE WHEN (SELECT IMEAGE_MCR FROM mobint.MCR_IMAGE_DETAILS WHERE DEVICENO=A.SERNR_SERIAL_NO AND ROWNUM<2) IS NOT NULL THEN 'Y' ELSE 'N' END) IMEAGE_MCR, ";
        //sql += " (CASE WHEN (SELECT IMAGE_METERTESTREPORT FROM mobint.MCR_IMAGE_DETAILS WHERE DEVICENO=A.SERNR_SERIAL_NO AND ROWNUM<2) IS NOT NULL THEN 'Y' ELSE 'N' END) IMAGE_METERTESTREPORT,  ";
        //sql += " (CASE WHEN (SELECT IMAGE_LABTESTINGREPORT FROM mobint.MCR_IMAGE_DETAILS WHERE DEVICENO=A.SERNR_SERIAL_NO AND ROWNUM<2) IS NOT NULL THEN 'Y' ELSE 'N' END) IMAGE_LABTESTINGREPORT,  ";
        //sql += " (CASE WHEN (SELECT IMAGE_SIGNATURE FROM mobint.MCR_IMAGE_DETAILS WHERE DEVICENO=A.SERNR_SERIAL_NO AND ROWNUM<2) IS NOT NULL THEN 'Y' ELSE 'N' END) IMAGE_SIGNATURE, 	 ";
        //sql += " SUBSTR(OVERHEADCABLE,1,2)ANCHOR_CONSUMER_END_QTY, SUBSTR(OVERHEADCABLE,3,2)ANGLE_IRON_CONSUMER_END_QTY, SUBSTR(OVERHEADCABLE,5,2)PIPE_CONSUMER_END_QTY,	   ";
        //sql += " B.ANCHOR_POLE_END_QTY, B.ANGLE_IRON_POLE_END_QTY,B.PIPE_POLE_END_QTY,SUBSTR(ADDITIONALACCESSORIES,1,2)ACCES_GLAND_QTY,  ";
        //sql += " SUBSTR(ADDITIONALACCESSORIES,3,2)ACCES_EARTH_NUT_QTY, SUBSTR(ADDITIONALACCESSORIES,5,2)ACCES_THIMBLE_QTY, 	 ";
        //sql += " SUBSTR(ADDITIONALACCESSORIES,7,2)ACCES_SADDLE_CLAMP_QTY,REM_TERMINAL_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2,REM_OTHER_SEAL,REM_BUSBAR_SEAL1,REM_BUSBAR_SEAL2, ";
        //sql += " DRUM_NUMBER_BB,RUNNING_LENGTH_FROM_BB,RUNNING_LENGTH_TO_BB,VALINSTALL_TYPE_BB,MRKVAH_OLD,IMAGE1_OLD,IMAGE2_OLD,CABLESIZE_OLD, 	 ";
        //sql += " BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD,GUNNYBAGSEAL_OLD,LABTESTING_DATE_OLD, ";
        //sql += " OLD_MR_KWH,OLD_MR_KVAH,PUNCH_REMARKS,LAB_TSTNG_NTC,OLD_MTR_IF_NOT_AVBL,OLD_MTR_IF_AVBL,OLD_MTR_STATUS,RMVD_MTR_BASE_PLT, ";
        //sql += " MTR_LOC_SHIFT,NOTICE_DATE,METER_REMOVED_BY,BB_CABLE_USED,CAB_REMOVE_FRM_SITE,CABLENOTINSTALLREASON,CAB_RMVD_FRM_SITE,IS_GNY_BAG_PREPD,  ";
        //sql += " MTR_READ_AVAIL,PUNCH_BY, PUNCH_DATE, '' HappyCode_BY_SYS, '' HAPPYCODE_BY_INSTALLER,'' HAPPY_REASON ,LM_LOOSEFLAG,  ";
        //sql += " NOHAPPYCODEREASON, SMARTMETERBOOL, SMARTMETERSIMNO, SMARTMETERSIMCODE,LM_CUSTOMERMETER Old_Meter, ";
        //sql += " (CASE WHEN cable_reqd='Y' THEN 'New' ELSE 'Old' END) CABLE_TYPE, '' SMS_SEND_DATE ";
        //sql += " FROM  ";
        //sql += " MOBINT.MCR_LOOSE_METER_DETAILS A, ";
        //sql += " MOBINT.MCR_DETAILS B, MOBINT.MCR_user_details C";
        //sql += " WHERE A.SERNR_SERIAL_NO=B.DEVICENO AND c.EMP_ID=b.TAB_LOGIN_ID";

        //if (_gCompany != "")
        //    sql += " and BUKRS_COMP_CODE='" + _gCompany + "'   ";
        //if (_gddlDivision != "")
        //    sql += " and c.DIVISION in ('" + _gddlDivision + "')  ";
        //if (_gddlVendorName != "")
        //    sql += " and a.LIFNR_ACCOUNT_NO='" + _gddlVendorName + "'   ";

        //if (_sOrderType != "-ALL-")
        //    sql += " AND b.ORDER_TYPE ='" + _sOrderType + "'";
        //else
        //    sql += " AND b.ORDER_TYPE IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        //if (_gFrom != "" && _gTo != "")
        //    sql += " and trunc(Punch_DATE) between '" + _gFrom + "' and '" + _gTo + "'   ";

        //sql += " UNION ";

        string _sql = " SELECT UNIQUE 'BRPL' COMPANY_CODE,C.DIVISION,B.SUB_DIVISION, VENDOR_CODE,LM_CUSTOMERCA CA_NO, B.ORDERID ORDER_NO,ORDER_TYPE, LM_ACCOUNTCLASS ACCOUNT_CLASS, ";
        _sql += " '' BASIC_START_DATE, '' BASIE_FINISH_DATE, LM_CUSTOMERNAME NAME,'' FATHER_NAME,LM_CUSTOMERADDRESS ADDRESS, MOBILE_NO,'' BP_NO,'' POLE_NO, ";
        _sql += "  CABLESIZE2 CABLE_SIZE,CABLELENGTH CABLE_LENGTH,'' PLANNER_GROUP,B.ACTIVITY_REASON,B.ACTIVITY_DATE, B.Deviceno METER_NO, B.OTHERSTICKER, ";
        _sql += " ELCB_INSTALLED, INSTALLEDBUSBAR BUS_BAR_INSTALLED,BB_CABLE_USED,BUSBARSIZE bus_bar_type, BUS_BAR_NO, B_BAR_CABLE_SIZE BUS_BAR_CABLE_SIZE, ";
        _sql += " OUTPUTBUSLENGTH BUS_BAR_CABLE_LENG, DRUM_NUMBER_BB BUS_BAR_DRUM_NO, PM_ACTIVITY,  ";
        _sql += " (SELECT PM_ACTIVITY FROM MOBINT.MCR_COR_SYS_MST WHERE UPPER(NAME)=UPPER(b.PM_ACTIVITY) AND COR_SYS_TYPE='ORD_LST_MST' AND ROWNUM<2)ACTIVITY_TYPE, ";
        _sql += " '' SANCTIONED_LOAD, TO_CHAR(MACHINE_ENTRY_DATE,'dd-Mon-yyyy') Assigned_Vendor_DT,CABLEINSTALLTYPE CABLE_INSTALLED, CABLESIZE2 CABLE_SIZE1,B.CABLE_REQD,B.CABLE_LEN_USED CABLE_TYPE,CABLELENGTH CABLE_LENGTH1, DURM_NO CABLE_DRUM_NO, ";
        _sql += " RUNNINGLENGTHFROM RUNNING_LENGTH_L1, RUNNINGLENGTHTO RUNNING_LENGTH_L2,OUTPUT_CABLE_LEN_USED Output_Cable_Type,OUTPUTCABLELENGTH OUTPUT_CABLE_LENGTH, B.OVERHEAD_UG CABLE_OH_UG, ";
        _sql += " TERMINAL_SEAL Terminal_Seal_1, OTHER_SEAL Terminal_Seal_2,METERBOXSEAL1 Box_Seal_1, METERBOXSEAL2 Box_Seal_2, BUSBARSEAL1 Bus_Bar_Seal_1, ";
        _sql += " BUSBARSEAL2 Bus_Bar_Seal_2,INSTALLATION METER_INSTALLED_LOCATION, POLENUMBER POLE_NO_TAB, GIS_LONG gis_longitude, GIS_LAT GIS_LATITUDE, ";
        _sql += " MR_KWH, MR_KW, MR_KVAH, MR_KVA, LM_CUSTOMERPOLE, ";

        _sql += " SUBSTR(OVERHEADCABLE,1,2)ANCHOR_CONSUMER_END_QTY, SUBSTR(OVERHEADCABLE,3,2)L_ANGLE, SUBSTR(OVERHEADCABLE,5,2)I_ANGLE, ";
        _sql += " B.ANCHOR_POLE_END_QTY, B.ANGLE_IRON_POLE_END_QTY POLE_END ,B.PIPE_POLE_END_QTY, SUBSTR(ADDITIONALACCESSORIES,1,2)ACCES_GLAND_QTY, SUBSTR(ADDITIONALACCESSORIES,3,2)ACCES_EARTH_NUT_QTY, ";
        _sql += " SUBSTR(ADDITIONALACCESSORIES,5,2)ACCES_THIMBLE_QTY, SUBSTR(ADDITIONALACCESSORIES,7,2)ACCES_SADDLE_CLAMP_QTY, SUBSTR(ADDITIONALACCESSORIES,9,2)ACCES_SATELLITE,  SUBSTR(ADDITIONALACCESSORIES,11,2)ACCES_PIERCING, REM_TERMINAL_SEAL, REM_BOX_SEAL1,REM_BOX_SEAL2,REM_OTHER_SEAL, ";
        _sql += " REM_BUSBAR_SEAL1,REM_BUSBAR_SEAL2, DRUM_NUMBER_BB, RUNNING_LENGTH_FROM_BB,RUNNING_LENGTH_TO_BB,VALINSTALL_TYPE_BB,MRKVAH_OLD,CABLESIZE_OLD, ";
        _sql += " BOX_OLD,GLANDS_OLD,TCOVER_OLD,BRASSSCREW_OLD,BUSBAR_OLD,THIMBLE_OLD,SADDLE_OLD,GUNNYBAG_OLD,GUNNYBAGSEAL_OLD, LABTESTING_DATE_OLD, OLD_MR_KWH,OLD_MR_KVAH, ";
        _sql += " LAB_TSTNG_NTC,OLD_MTR_IF_NOT_AVBL,OLD_MTR_IF_AVBL, OLD_MTR_STATUS,RMVD_MTR_BASE_PLT, MTR_LOC_SHIFT,NOTICE_DATE,METER_REMOVED_BY,CAB_REMOVE_FRM_SITE, CABLESIZE_OLD CABLE_REMOVE_SIZE,CABLELENGTH_OLD CABLE_REMOVE_LENGTH,";
        _sql += " CABLENOTINSTALLREASON,CAB_RMVD_FRM_SITE,IS_GNY_BAG_PREPD, MTR_READ_AVAIL,TAB_LOGIN_ID PUNCH_BY,SUBMIT_DATETIME TAB_SUBMIT_DATE, MACHINE_ENTRY_DATE PUNCH_DATE, '' HappyCode_BY_SYS, '' HAPPYCODE_BY_INSTALLER,PUNCH_REMARKS HAPPY_CODE_RMKS, ";
        _sql += " '' HAPPY_REASON ,LM_LOOSEFLAG, NOHAPPYCODEREASON, SMARTMETERBOOL, SMARTMETERSIMNO, SMARTMETERSIMCODE,  ";
        _sql += " SMARTMETER_OLDNUM AS Smart_Old_SimNO, SMARTMETER_RSSICODE AS Smart_RSSI_Code, SMARTMETER_ERRORCODE AS Smart_Error_Code, LM_CUSTOMERMETER Old_Meter,B.TAKEPHOTOGRAPH METER_INS_TYPE, ";
        _sql += "   '' SMS_SEND_DATE,punch_mode  ";

        _sql += " FROM mobint.MCR_LOOSE_METER_DETAILS A, MOBINT.MCR_DETAILS_TMP B, MOBINT.MCR_user_details C ,mobint.MCR_IMAGE_DETAILS_TMP D ";
        _sql += " WHERE A.SERNR_SERIAL_NO=B.DEVICENO AND c.EMP_ID=b.TAB_LOGIN_ID AND D.DEVICENO=B.DEVICENO  AND LM_LOOSEFLAG='LOOSE' AND A.FLAG = 'U'  ";


        //if (_gddlVendorName != "")
        //    _sql += " and VENDOR_CODE='" + _gddlVendorName + "' ";

        if (_gddlDivision != "")
            _sql += " and c.DIVISION in ('" + _gddlDivision + "') ";

        if (_sOrderType != "-ALL-")
            _sql += " AND b.ORDER_TYPE ='" + _sOrderType + "'";
        else
            _sql += " AND b.ORDER_TYPE IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y')";

        if (_gFrom != "" && _gTo != "")
            _sql += " and trunc(MACHINE_ENTRY_DATE) between '" + _gFrom + "' and '" + _gTo + "'   ";

        DataTable dt = objUti.ExecuteReaderMIS(_sql);
        return dt;
    }

    public int UpdateLoose_AutoKittingCase()
    {
        string sqlinsert = " UPDATE mobint.mcr_details SET SAP_Flag='N' WHERE orderid IS NOT NULL AND  SAP_flag='L' and order_type in ('ZDIV','ZMSO','ZDRM') ";

        return objUti.ExecuteNonQuery(sqlinsert);
    }



    #endregion
   
    #region"Generate MCR"
    public DataTable GetMCR_OrderNoWise(string _sCANo)
    {
        string sql = " SELECT M.ORDERID,CA_NO,M.DIVISION, TO_CHAR(ACTIVITY_DATE,'dd-Mon-yyyy') ACT_DATE ,m.ACCOUNT_CLASS, AUART ,SANCTIONED_LOAD ,NAME, father_NAME,TEL_NO, ADDRESS, ";
        sql += " m.METER_NO ,MR_KWH ,MR_KVAH ,TERMINAL_SEAL , OTHER_SEAL,METERBOXSEAL1,METERBOXSEAL2 ,CABLESIZE2,CABLELENGTH,OLD_MR_KWH ,MRKVAH_OLD, ";
        sql += " REM_TERMINAL_SEAL ,REM_OTHER_SEAL,REM_BOX_SEAL1,REM_BOX_SEAL2 ,REM_CABLE_SIZE,REM_CABLE_LEN,GUNNYBAG_OLD,GUNNYBAGSEAL_OLD,  ";
        sql += " LABTESTING_DATE_OLD, CABLELENGTH_OLD, CABLESIZE_OLD, ILART_ACTIVITY_TYPE , PUNCH_BY, OLD_METERNO_SERNR,OUTPUTCABLELENGTH FROM MOBINT.MCR_input_DETAILS M, MOBINT.MCR_details d ";
        sql += " WHERE m.ORDERID=d.ORDERID ";

        if (_sCANo != "")
            sql += " and CA_NO='" + _sCANo + "' ";
        
        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable GetMCRLogin_CANoWise(string _sCANo, string _sMeterNo, string _sOrderNo)
    {
       // string sql = " select CA_NO from MOBINT.MCR_input_details where HAPPY_CODE_GEN='" + _sHappyCode + "' and CA_NO='" + _sCANo + "' ";
        string sql = " select CA_NO from MOBINT.MCR_input_details where METER_NO ='" + _sMeterNo + "'  and CA_NO='" + _sCANo + "' ";
        if (_sOrderNo != "")
            sql += " and ORDERID='" + _sOrderNo + "' ";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    public DataTable GetSubDivision_DivWise(string _sDivCode)
    {
        string sql = string.Empty;

        sql = " SELECT '0' SUB_DIVISION,'-ALL-' SUB_DIV_DESC FROM dual UNION ";
        sql += " SELECT SUB_DIVISION, (SUB_DIVISION ||' - ' ||SUB_DIVISION_DESC)SUB_DIV_DESC ";
        sql += " FROM MOBINT.MCR_SUBDIVISION_MST WHERE DIVISION='" + _sDivCode + "' ORDER BY SUB_DIV_DESC  ";

        DataTable dt = objUti.ExecuteReader(sql);
        return dt;
    }

    #endregion


    /// Developed by Arvinder on dt 02/04/2019

    #region "New Connection Request Through App Report"

    public DataTable getNewConAppReport_Main(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gddlDivision, string _sCompany)
    {
        string sql = "SELECT UNIQUE A.DIST_CD,(SELECT UNIQUE INITCAP(DIV.DIVISION_NAME) FROM MOBINT.MCR_DIVISION DIV WHERE DIV.DIST_CD = A.DIST_CD) DIVISION,";
        sql += "NVL(SUM(CASE WHEN FLAG IN ('Y','C','N','E') THEN 1 END),0) AS TOTAL,";
        sql += "NVL(SUM(CASE WHEN FLAG IN ('C','E','Y') THEN 1 END),0) AS ALLOCATED_IN_APP,";
        sql += "NVL(SUM(CASE WHEN FLAG = 'C' THEN 1 END),0) AS METER_INSTALLED_APP,";
        sql += "NVL(SUM(CASE WHEN FLAG = 'E' THEN 1 END),0) AS METER_NOT_INSTALLED_APP";
        sql += " FROM MOBINT.MCR_DIVISION A, MOBINT.MCR_INPUT_DETAILS B, MOBINT.MCR_DETAILS C, MOBINT.MCR_ORDER_CANCEL D";
        sql += " WHERE B.PSTING_DATE BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' AND B.AUART = 'ZDIN' AND B.COMP_CODE = '" + _sCompany + "'";
        sql += " and A.DIST_CD IN ('" + _gddlDivision + "')";
        sql += " AND UPPER(A.DIST_CD) = UPPER(B.DIVISION(+)) AND UPPER(A.DIST_CD) = UPPER(C.DIVISION(+)) AND B.ORDERID = C.ORDERID(+) AND B.METER_NO = C.DEVICENO(+)";
        sql += " AND B.ORDERID = D.ORDERID(+)";
        sql += " GROUP BY A.DIST_CD ORDER BY A.DIST_CD";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    public DataTable getNewConAppReport_Drill1(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gDivision, string _gCompany)
    {
        string sql = "SELECT A.DIVISION DIST_CD,";
        sql += " NVL(SUM(CASE WHEN UPPER(TRIM(B.REASON)) = UPPER('PL / PPL / MCD sealed') THEN 1 END),0) AS PL_PPL_MCDSEALED,";
        //sql += "--NVL(SUM(CASE WHEN UPPER(TRIM(B.REASON)) = UPPER('ANT') THEN 1 END),0) AS ANT,";
        sql += "NVL(SUM(CASE WHEN UPPER(TRIM(B.REASON)) = UPPER('Consumer out of station / wants time /  No responsible person available at site') THEN 1 END),0) AS CONSUMER_OUT_STATION,";
        //sql += "--NVL(SUM(CASE WHEN UPPER(TRIM(B.REASON)) = UPPER('INCORRECT_INFORMATION') THEN 1 END),0) AS INCORRECT_INFORMATION,";
        //sql += "--NVL(SUM(CASE WHEN UPPER(TRIM(B.REASON)) = UPPER('CABLE_NOT_PROVIDED') THEN 1 END),0) AS CABLE_NOT_PROVIDED,";
        sql += "NVL(SUM(CASE WHEN UPPER(TRIM(B.REASON)) = UPPER('ELCB / MCB not installed') THEN 1 END),0) AS NO_ELCB,";
        sql += "NVL(SUM(CASE WHEN UPPER(TRIM(B.REASON)) IN (UPPER('Court Case / Ownership Dispute / Any Other Dispute'),UPPER('Dispute Case (To be deleted ( Merged in Sl. No. 3)')) THEN 1 END),0) AS Disputed,";
        //sql += "--NVL(SUM(CASE WHEN UPPER(TRIM(B.REASON)) = UPPER('DB_FULL') THEN 1 END),0) AS DB_FULL,";
        sql += "NVL(SUM(CASE WHEN UPPER(TRIM(B.REASON)) = UPPER('Insufficient Space for Meter Installation') THEN 1 END),0) AS NO_SPACE,";
        sql += "NVL(SUM(CASE WHEN UPPER(TRIM(B.REASON)) = UPPER('Non-conforming, NX w/o SDM clearance') THEN 1 END),0) AS NON_CONFORMING_AREA,";
        //sql += "--NVL(SUM(CASE WHEN UPPER(TRIM(B.REASON)) = UPPER('CRA') THEN 1 END),0) AS CRA,";
        sql += "NVL(SUM(CASE WHEN UPPER(TRIM(B.REASON)) = UPPER('Road cutting permission required') THEN 1 END),0) AS RCP_Required,";
        sql += "NVL(SUM(CASE WHEN UPPER(TRIM(B.REASON)) = UPPER('Serparate line request') THEN 1 END),0) AS SEPARATE_LINE_REQUEST,";
        sql += "NVL(SUM(CASE WHEN UPPER(TRIM(B.REASON)) = UPPER('Under construction / Vacant plot') THEN 1 END),0) AS UNDER_CONSTRUCTION,";
        sql += "NVL(SUM(CASE WHEN UPPER(TRIM(B.REASON)) = UPPER('Wiring incomplete / Panel not installed') THEN 1 END),0) AS WIRING_INCOMPLETE,";
        sql += "NVL(SUM(CASE WHEN UPPER(TRIM(B.REASON)) = UPPER('Consumer wants Meter Installation inside his/her premises') THEN 1 END),0) AS INSIDE_PREMISES,";
        sql += "NVL(SUM(CASE WHEN (UPPER(TRIM(B.REASON)) = UPPER('If Others, Specify:')";
        sql += " OR UPPER(TRIM(B.REASON)) NOT IN (UPPER('PL / PPL / MCD sealed'),UPPER('Consumer out of station / wants time /  No responsible person available at site'),";
        sql += "UPPER('ELCB / MCB not installed'),UPPER('Court Case / Ownership Dispute / Any Other Dispute'),UPPER('Dispute Case (To be deleted ( Merged in Sl. No. 3)'),";
        sql += "UPPER('Insufficient Space for Meter Installation'),UPPER('Non-conforming, NX w/o SDM clearance'),UPPER('Road cutting permission required'),";
        sql += "UPPER('Serparate line request'),UPPER('Under construction / Vacant plot'),UPPER('Wiring incomplete / Panel not installed'),";
        sql += "UPPER('Consumer wants Meter Installation inside his/her premises'))) THEN 1 END),0) AS OTHER";
        sql += " FROM MOBINT.MCR_INPUT_DETAILS A, MOBINT.MCR_ORDER_CANCEL B";
        sql += " WHERE A.PSTING_DATE BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' AND A.AUART = 'ZDIN' AND A.COMP_CODE = '" + _gCompany + "'";
        sql += " AND A.FLAG = 'E' AND A.ORDERID = B.ORDERID AND A.DIVISION = '" + _gDivision + "'";
        sql += " GROUP BY A.DIVISION";

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    public DataTable getNewConAppReport_Drill2(string _gPSTING_From_DATE, string _gPSTING_To_DATE, string _gDivision, string _gCompany, string _sFlag, string _sDivName)
    {
        string sql = "SELECT UNIQUE TO_CHAR(A.PSTING_DATE,'dd-Mon-yyyy') PSTING_DATE,A.ORDERID,A.METER_NO,'" + _sDivName + "' DIVISION,A.CA_NO,INITCAP(A.NAME) NAME,INITCAP(A.ADDRESS) ADDRESS,";
        sql += "A.TEL_NO, A.ACCOUNT_CLASS,INITCAP(TRIM(B.REASON)) REASON,";
        sql += "(SELECT UNIQUE INITCAP(EMP_NAME) FROM MOBINT.MCR_USER_DETAILS C WHERE B.INSTALLER_ID = C.EMP_ID AND ROWNUM = 1) INSTALLER,";
        sql += "INITCAP(B.REMARKS) REMARKS,TO_CHAR(B.ENTRY_DATE,'dd-Mon-yyyy') PUNCHED_DATE";
        sql += " FROM MOBINT.MCR_INPUT_DETAILS A, MOBINT.MCR_ORDER_CANCEL B";
        sql += " WHERE A.PSTING_DATE BETWEEN '" + _gPSTING_From_DATE + "' AND '" + _gPSTING_To_DATE + "' AND A.AUART = 'ZDIN' AND A.COMP_CODE = '" + _gCompany + "'";
        sql += " AND A.FLAG = 'E' AND A.ORDERID = B.ORDERID AND A.DIVISION = '" + _gDivision + "'";
        if (_sFlag == "PL_PPL_MCDSEALED")
            sql += " AND UPPER(TRIM(B.REASON)) = UPPER('PL / PPL / MCD sealed')";
        else if (_sFlag == "CONSUMER_OUT_STATION")
            sql += " AND UPPER(TRIM(B.REASON)) = UPPER('Consumer out of station / wants time /  No responsible person available at site')";
        else if (_sFlag == "NO_ELCB")
            sql += " AND UPPER(TRIM(B.REASON)) = UPPER('ELCB / MCB not installed')";
        else if (_sFlag == "Disputed")
            sql += " AND UPPER(TRIM(B.REASON)) IN (UPPER('Court Case / Ownership Dispute / Any Other Dispute'),UPPER('Dispute Case (To be deleted ( Merged in Sl. No. 3)'))";
        else if (_sFlag == "NO_SPACE")
            sql += " AND UPPER(TRIM(B.REASON)) = UPPER('Insufficient Space for Meter Installation')";
        else if (_sFlag == "NON_CONFORMING_AREA")
            sql += " AND UPPER(TRIM(B.REASON)) = UPPER('Non-conforming, NX w/o SDM clearance')";
        else if (_sFlag == "RCP_Required")
            sql += " AND UPPER(TRIM(B.REASON)) = UPPER('Road cutting permission required')";
        else if (_sFlag == "SEPARATE_LINE_REQUEST")
            sql += " AND UPPER(TRIM(B.REASON)) = UPPER('Serparate line request')";
        else if (_sFlag == "UNDER_CONSTRUCTION")
            sql += " AND UPPER(TRIM(B.REASON)) = UPPER('Under construction / Vacant plot')";
        else if (_sFlag == "WIRING_INCOMPLETE")
            sql += " AND UPPER(TRIM(B.REASON)) = UPPER('Wiring incomplete / Panel not installed')";
        else if (_sFlag == "INSIDE_PREMISES")
            sql += " AND UPPER(TRIM(B.REASON)) = UPPER('Consumer wants Meter Installation inside his/her premises')";
        else if (_sFlag == "OTHER")
        {
            sql += " AND (UPPER(TRIM(B.REASON)) = UPPER('If Others, Specify:') OR UPPER(TRIM(B.REASON)) NOT IN (UPPER('PL / PPL / MCD sealed'),UPPER('Consumer out of station / wants time /  No responsible person available at site'),";
            sql += "UPPER('ELCB / MCB not installed'),UPPER('Court Case / Ownership Dispute / Any Other Dispute'),UPPER('Dispute Case (To be deleted ( Merged in Sl. No. 3)'),UPPER('Insufficient Space for Meter Installation'),";
            sql += "UPPER('Non-conforming, NX w/o SDM clearance'),UPPER('Road cutting permission required'),UPPER('Serparate line request'),UPPER('Under construction / Vacant plot'),UPPER('Wiring incomplete / Panel not installed'),";
            sql += "UPPER('Consumer wants Meter Installation inside his/her premises')))";
        }

        DataTable dt = objUti.ExecuteReaderMIS(sql);
        return dt;
    }

    #endregion

    #region"New MIS"

    public DataTable GetMISummaryReport(string division, string orderType, string fromDate, string toDate)
    {
        DataTable dt = new DataTable();
        string query = "select e.division,e.vendor_code, nvl(e.c_orderbasedCompleted,0) as c_orderbasedCompleted ,";
        query += " nvl(e.e_orderbasedCancellCases,0) as e_orderbasedCancellCases, nvl(f.c_looseCompleted,0) as c_looseCompleted  from";
        query += "   (select c.division,c.vendor_code,c.c_orderbasedCompleted, d.e_orderbasedCancellCases from  ";
        query += "   (select a.division,a.vendor_code,c_orderbasedCompleted   from";
        query += "   (select mid.division, mid.vendor_code  ";
        query += "    from  mobint.mcr_input_details mid, mobint.mcr_details mcr where mid.METER_NO=mcr.DEVICENO  AND mid.COMP_CODE='BRPL' AND mid.VENDOR_CODE IN(SELECT VENDOR_ID FROM mobint.mcr_vendor_mst WHERE VENDOR_ID=mid.VENDOR_CODE AND active_flag='Y' AND COMPANY='BRPL')";
        if (division != "" && division != null)
            query += "and mid.division ='" + division + "'";
        if (orderType != "" && orderType != null)
            query += "and mid.AUART ='" + orderType + "'";
        if (fromDate != "" && toDate != "")
            query += "and TRUNC(mid.PUNCH_DATE) between '" + fromDate + "' AND '" + toDate + "'";
        query += " and mid.division is not null  group by  mid.division, mid.vendor_code) a left outer join ";
        query += " (select mid.division, mid.vendor_code, count(1) as c_orderbasedCompleted  ";
        query += "  from  mobint.mcr_input_details mid, mobint.mcr_details mcr where mid.METER_NO=mcr.DEVICENO AND mid.VENDOR_CODE IN(SELECT VENDOR_ID FROM mobint.mcr_vendor_mst WHERE VENDOR_ID=mid.VENDOR_CODE AND active_flag='Y' AND COMPANY='BRPL')";
        if (division != "" && division != null)
            query += "and mid.division ='" + division + "'";
        if (orderType != "" && orderType != null)
            query += "and mid.AUART ='" + orderType + "'";
        if (fromDate != "" && toDate != "")
            query += "and TRUNC(mid.PUNCH_DATE) between '" + fromDate + "' AND '" + toDate + "'";
        query += "  and mid.flag='C' and mcr.LM_LOOSEFLAG='ORDERBASED' AND mid.COMP_CODE='BRPL'  group by  mid.division, mid.vendor_code) b   on a.division = b.division";
        query += " and a.vendor_code  = b.vendor_code) c left outer join";
        query += " (select mid.division, mid.vendor_code, count(1) as e_orderbasedCancellCases from  mobint.mcr_input_details mid  ";
        query += "  where mid.flag='E' AND mid.COMP_CODE='BRPL' AND mid.VENDOR_CODE IN(SELECT VENDOR_ID FROM mobint.mcr_vendor_mst WHERE VENDOR_ID=mid.VENDOR_CODE AND active_flag='Y' AND COMPANY='BRPL')  ";
        if (division != "" && division != null)
            query += "and mid.division ='" + division + "'";
        if (orderType != "" && orderType != null)
            query += "and mid.AUART ='" + orderType + "'";
        if (fromDate != "" && toDate != "")
            query += "and TRUNC(mid.PUNCH_DATE) between '" + fromDate + "' AND '" + toDate + "'";
        query += " group by  mid.division, mid.vendor_code) d";
        query += " on c.division = d.division";
        query += " and c.vendor_code  = d.vendor_code) e left outer join";
        query += "  (SELECT  l.division, COUNT(1) AS c_looseCompleted,   ";
        query += "   (SELECT VENDOR_ID FROM mobint.MCR_VENDOR_MST mvm  WHERE l.division= mvm.address AND active_flag='Y' AND COMPANY='BRPL' AND ROWNUM=1) AS VENDOR_ID ";
        //  query += "  active_flag='Y' AND COMPANY='BRPL' AND ROWNUM=1) AS VENDOR_ID  		 ";
        query += "  FROM	(SELECT DISTINCT 'BRPL' COMPANY_CODE,C.DIVISION,B.SUB_DIVISION, VENDOR_CODE,LM_CUSTOMERCA CA_NO ";
        query += " FROM  mobint.MCR_DETAILS b,MOBINT.MCR_USER_DETAILS c,mobint.MCR_IMAGE_DETAILS D ";
        query += " WHERE b.TAB_LOGIN_ID=c.EMP_ID AND b.DEVICENO=d.DEVICENO ";
        query += " AND b.ORDER_TYPE IN (SELECT DISTINCT ORDER_TYPE FROM MOBINT.MCR_ORDER_PM_MASTER WHERE ACTIVE_FLAG='Y') ";
        //   query += "AND TRUNC(b.MACHINE_ENTRY_DATE) BETWEEN '01-Jun-2019' AND '30-Jun-2019'  AND b.LM_LOOSEFLAG='LOOSE' ";
        // query += " AND c.division IS NOT NULL)l";
        //query += "GROUP BY  DIVISION) f  ON e.division = f.division";
        if (division != "" && division != null)
            query += "and c.division ='" + division + "'";
        if (orderType != "" && orderType != null)
            query += "and ORDER_TYPE ='" + orderType + "'";
        if (fromDate != "" && toDate != "")
            query += "and TRUNC(b.MACHINE_ENTRY_DATE) between '" + fromDate + "' AND '" + toDate + "'";
        query += "  AND b.LM_LOOSEFLAG='LOOSE'  AND c.division IS NOT NULL)l  GROUP BY  division) f ";
        query += " on e.division = f.division";
        // query += " and e.vendor_code  = f.vendor_code";
        dt = objUti.ExecuteReaderMIS(query);
        return dt;
    }

    public DataTable getMISCancelCases(string division, string orderType, string fromDate, string endDate)
    {
        DataTable dt = new DataTable();
        string query = "select mid.division, MID.CA_NO, mid.vendor_code, mid.OrderID, mid.AUART as ORDER_TYPE, mid.START_DATE as BASIC_START_DATE, ";
        query += "mid.FINISH_DATE as BASIE_FINISH_DATE, mid.NAME,mid.FATHER_NAME, mid.ADDRESS, mid.TEL_NO as MOBILE_NO, moc.REASON as CANCEL_REASON";
        query += "  from mobint.mcr_input_details mid, mobint.mcr_order_cancel moc where mid.ORDERID=moc.ORDERID AND mid.COMP_CODE='BRPL' AND mid.Flag='E' AND  moc.IMAGE_PATH !='X'";

        if (division != "" && division != null)
            query += " and mid.division='" + division + "'";

        if (orderType != "" && orderType != null)
            query += " and mid.AUART='" + orderType + "'";

        if (fromDate != "" && endDate != null)
            query += " and TRUNC(mid.PUNCH_DATE) between '" + fromDate + "' and '" + endDate + "'";

        dt = objUti.ExecuteReaderMIS(query);
        return dt;
    }

    #endregion
}