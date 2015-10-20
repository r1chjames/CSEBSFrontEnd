using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Diagnostics;
using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Windows.Forms;

public partial class Goods_In_Locator : System.Web.UI.Page
{
    private OracleConnection Connect = new OracleConnection();
    private string EBSDM13A, subinv, sql;

    public Goods_In_Locator()
    {
        EBSDM13A = "Data Source=(DESCRIPTION="
               + "(ADDRESS=(PROTOCOL=TCP)(HOST=tebvm73.unix.company.net) (PORT=1521))"
               + "(CONNECT_DATA=(SERVICE_NAME=EBSDM13A)));"
               + "User Id=XXPOC;Password=oragang12;";
    }  

    protected void Page_Load(object sender, EventArgs e)
    {
        init_table(null, null);
    }

    protected void init_table(object sender, EventArgs e)
    {        
        subinv = (string)(Session["SE_subinv"]);
        sql = "SELECT LOCAT AS LOCATOR FROM XXPOC_STOCK_LOCATORS WHERE SUBINVENTORY_CODE = '" + subinv + "'";
        using (OracleConnection conn = new OracleConnection())
        {
            try
            {
                conn.ConnectionString = EBSDM13A;
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;

                OracleDataReader dr = cmd.ExecuteReader();
                GridView1.DataSource = dr;
                GridView1.DataBind();
            }
            catch (OracleException ex)
            {
                Debug.WriteLine("Exception Message: " + ex.Message);
                Debug.WriteLine("Exception Source: " + ex.Source);
            }
        }
    }

    public void showmsg(string str)
    {
        string prompt =
        "<script>$(document).ready(function(){{$.prompt('{0}',{1});}});</script>";
        string message = string.Format(prompt, str, "{ prefix: 'jqi' }");
        this.Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", message);
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            string gridviewvalue = GridView1.SelectedRow.Cells[0].Text;
            Session["SE_locator"] = gridviewvalue;
            Response.Redirect("Goods_In_2.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        catch (System.Exception)
        {
            showmsg("NO LOCATOR SELECTED");
        }      
    }

    protected void GridView1_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
        }
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            if (row.RowIndex == GridView1.SelectedIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#d8d8d8");
                row.ToolTip = string.Empty;
            }
            else
            {
                row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
            }
        }
    }



}



