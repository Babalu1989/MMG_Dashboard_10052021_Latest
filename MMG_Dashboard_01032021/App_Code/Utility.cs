using System;
using System.Globalization;
using System.Threading;
using System.Web;

/// <summary>
/// Summary description for Utility
/// </summary>
/// 

namespace common
{
    public class Utility
    {
        public static string GetMMDDYYYY(string strMMDDYYYY)
        {
            if (strMMDDYYYY != "")
            {
                Array arrDate;
                arrDate = strMMDDYYYY.Split(Convert.ToChar("/"));
                return ((arrDate.GetValue(1).ToString().Trim()) + "/" + (arrDate.GetValue(0).ToString().Trim()) + "/" + (arrDate.GetValue(2).ToString().Trim()));

            }
            return null;
        }

        public static string GetDate()
        {
            DateTime dt = DateTime.Now;
            string strTmpDate;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            strTmpDate = dt.ToString();

            return (strTmpDate);
        }

        public static string GetIpAddress()
        {
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

        }


        public static string GetErrorDesc(int intErrorNumber)
        {
            switch (intErrorNumber)
            {
                case 1: return ("Record Saved Successfully");
                case 2: return ("Record Modified Successfully");
                case 3: return ("Record Deleted Successfully");
                case 4: return ("Duplicate Record Found");
                case 10: return ("Period From Cannot Be Less Than Last Period From");
                case 11: return ("No Record Found");

                case 0: return ("Error Occurred");
                case -1: return ("Error While Saving Record");
                case -2: return ("Error While Modifying Record");
                case -3: return ("Error While Deleting Record");
                default: return ("Please specified correct Error Number");

            }
        }
    }
}