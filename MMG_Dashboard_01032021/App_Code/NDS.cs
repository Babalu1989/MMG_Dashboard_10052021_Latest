using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using System.Text;

public class NDS
{
    public NDS()
    {

    }
    public static bool IsNumeric(object value)
    {
        try
        {
            Double i = Convert.ToDouble(value.ToString());
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
    public static void ClearControls(Control control)
    {
        for (int i = control.Controls.Count - 1; i >= 0; i--)
        {
            ClearControls(control.Controls[i]);
        }

        if (!(control is TableCell))
        {
            if (control.GetType().GetProperty("SelectedItem") != null)
            {
                LiteralControl literal = new LiteralControl();
                control.Parent.Controls.Add(literal);
                try
                {
                    literal.Text = (string)control.GetType().GetProperty("SelectedItem").GetValue(control, null);
                }
                catch
                {
                }
                control.Parent.Controls.Remove(control);
            }
            else
                if (control.GetType().GetProperty("Text") != null)
                {
                    LiteralControl literal = new LiteralControl();
                    control.Parent.Controls.Add(literal);
                    literal.Text = (string)control.GetType().GetProperty("Text").GetValue(control, null);
                    control.Parent.Controls.Remove(control);
                }
        }
        return;
    }
    //public string con()
    //{
    //    //string database = "";
    //    //string user_id = "";
    //    //string pass = "";

    //    //Cryptograph crp = new Cryptograph();
    //    //HttpServerUtility myserver = HttpContext.Current.Server;

    //    //string vs = AppDomain.CurrentDomain.BaseDirectory + "prm.ini";

    //    //string pw_key = "o8??^am(*)";

    //    //user_id = crp.Decrypt(NDSINI.GetINI(vs, "itpms", crp.Encrypt("dbuserid", pw_key), "?"), pw_key);
    //    //pass = crp.Decrypt(NDSINI.GetINI(vs, "itpms", crp.Encrypt("dbuserpwd", pw_key), "?"), pw_key);
    //    //database = crp.Decrypt(NDSINI.GetINI(vs, "itpms", crp.Encrypt("dbconn", pw_key), "?"), pw_key);// put user code to initialize the page here

    //    //string str = "provider=msdaora.1; user id=" + user_id + "; password=" + pass + "; data source=" + database + ";";
    //    string str = "Provider=MSDAORA.1; User ID=MOBAPP; Password=MOBAPP; Data Source=ebstest;";
    //    return str;
    //}
    public string DcrepCon()
    {
        string database = "";
        string user_id = "";
        string pass = "";
        //Cryptograph crp = new Cryptograph();
        ////HttpServerUtility myserver = HttpContext.Current.Server;
        //string vs = AppDomain.CurrentDomain.BaseDirectory + "bsesmobint.INI";
        //string PW_KEY = "o8??^am(*)"; // Enter Key Below Function Also...
        //user_id = crp.Decrypt(NDSINI.GetINI(vs, "mobint", crp.Encrypt("dbuserid", PW_KEY), "?"), PW_KEY);
        //pass = crp.Decrypt(NDSINI.GetINI(vs, "mobint", crp.Encrypt("dbuserpwd", PW_KEY), "?"), PW_KEY);
        //database = crp.Decrypt(NDSINI.GetINI(vs, "mobint", crp.Encrypt("dbconn", PW_KEY), "?"), PW_KEY);// Put user code to initialize the page here
        //string str = "Provider=MSDAORA.1; User ID=" + user_id + "; Password=" + pass + "; Data Source=" + database + ";";
        //  string str = "Provider=OraOLEDB.Oracle; User ID=mobint; Password=mobint; Data Source=localhost/orcl;";
        string str = "Provider=MSDAORA.1; User ID=mobint; Password=mobint; Data Source=ebstestold";
        return str;
    }
    public string MISCon()
    {
        string database = "";
        string user_id = "";
        string pass = "";
        //Cryptograph crp = new Cryptograph();
        //HttpServerUtility myserver = HttpContext.Current.Server;
        //string vs = AppDomain.CurrentDomain.BaseDirectory + "bses.ini";
        //string pw_key = "o8??^am(*)";
        //user_id = crp.Decrypt(NDSINI.GetINI(vs, "dcrep", crp.Encrypt("dbuserid", pw_key), "?"), pw_key);
        //pass = crp.Decrypt(NDSINI.GetINI(vs, "dcrep", crp.Encrypt("dbuserpwd", pw_key), "?"), pw_key);
        //database = crp.Decrypt(NDSINI.GetINI(vs, "dcrep", crp.Encrypt("dbconn", pw_key), "?"), pw_key);// put user code to initialize the page here
        //string str = "Provider=MSDAORA.1; User ID=" + user_id + "; Password=" + pass + "; Data Source=" + database + ";";
          string str = "Provider=MSDAORA.1; User ID=mobint; Password=mobint; Data Source=ebstestold";
        return str;
    }
    public string DcrepConTest()
    {
        string database = "";
        string user_id = "";
        string pass = "";
        Cryptograph crp = new Cryptograph();
        HttpServerUtility myserver = HttpContext.Current.Server;
        string vs = AppDomain.CurrentDomain.BaseDirectory + "bses.ini";
        string pw_key = "o8??^am(*)";
        user_id = crp.Decrypt(NDSINI.GetINI(vs, "pcc", crp.Encrypt("dbuserid", pw_key), "?"), pw_key);
        pass = crp.Decrypt(NDSINI.GetINI(vs, "pcc", crp.Encrypt("dbuserpwd", pw_key), "?"), pw_key);
        database = crp.Decrypt(NDSINI.GetINI(vs, "pcc", crp.Encrypt("dbconn", pw_key), "?"), pw_key);// put user code to initialize the page here
        string str = "Provider=MSDAORA.1; User ID=" + user_id + "; Password=" + pass + "; Data Source=" + database + ";";
        //string str = "Provider=MSDAORA.1; User ID=dcrep; Password=dcrep; Data Source=ebstest;";
        //string str = "Provider=MSDAORA.1; User ID=MOBAPP; Password=mobileservice; Data Source=ebsdbstd;";//64.86
        //string str = "Provider=MSDAORA.1; User ID=piyush; Password=piyush; Data Source=ebsdbstd;";
        return str;
    }
}




