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

public partial class Goods_In_1 : System.Web.UI.Page
{
    private OracleConnection Connect = new OracleConnection();
    private string EBSDM13A, ponum, sql;

    public Goods_In_1()
    {
        EBSDM13A = "Data Source=(DESCRIPTION="
               + "(ADDRESS=(PROTOCOL=TCP)(HOST=tebvm73.unix.company.net) (PORT=1521))"
               + "(CONNECT_DATA=(SERVICE_NAME=EBSDM13A)));"
               + "User Id=XXPOC;Password=oragang12;";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        init_fields(null, null);
        if (txtDate.Text == "")
        {
            txtDate.Text = DateTime.Today.ToString("dd-MMM-yy").ToUpper();
        }

        if (ponum != null)
        {
            sql = "SELECT PO_NUM AS \"PO NUMBER\", SUPPLIER, REQ_DATETIME AS \"DATE\" FROM XXPOC_PO_DETAILS WHERE PO_NUM = '" + ponum + "'";
        }
        else
        {
            sql = "SELECT PO_NUM AS \"PO NUMBER\", SUPPLIER, REQ_DATETIME AS \"DATE\" FROM XXPOC_PO_DETAILS WHERE REQ_DATETIME >= to_date('" + txtDate.Text + "','dd-MON-yy') AND REQ_DATETIME < to_date('" + txtDate.Text + "','dd-MON-yy') +1";
        }
        init_table(null, null, sql);
        if (Session["SE_poheadrowindex"] != null)
        {
            GridView1_RowPostBack(null, null);
        }
    }

    protected void init_fields(object sender, EventArgs e)
    {
        ponum = (string)(Session["SE_ponum"]);
        clear_session_fields(null, null);
    }

    protected void clear_session_fields(object sender, EventArgs e)
    {
        Session["SE_rowindex"] = null;
        Session["SE_subinv"] = null;
        Session["SE_locator"] = null;
    }

    protected void init_title(object sender, EventArgs e)
    {
        string LC_titlesql, LC_todate;
        if (ponum != null)
        {
            sql = "SELECT PO_NUM AS \"PO NUMBER\", SUPPLIER, REQ_DATETIME AS \"DATE\" FROM XXPOC_PO_DETAILS WHERE PO_NUM = '" + ponum + "'";
            LC_titlesql = "SELECT REQ_DATETIME FROM XXPOC_PO_DETAILS WHERE PO_NUM = '" + ponum + "'";
            using (OracleConnection conn = new OracleConnection())
            {
                try
                {
                    conn.ConnectionString = EBSDM13A;
                    conn.Open();
                    OracleCommand cmd = new OracleCommand(LC_titlesql, conn);
                    cmd.CommandType = CommandType.Text;

                    OracleDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Debug.WriteLine(dr.GetFieldType(0));
                        LC_todate = dr.GetOracleDate(0).ToString();
                        txtDate.Text = LC_todate;
                        lblDate.Text = txtDate.Text;
                    }
                }
                catch (OracleException ex)
                {
                    Debug.WriteLine("Exception Message: " + ex.Message);
                    Debug.WriteLine("Exception Source: " + ex.Source);
                }
            }
            Session["SE_ponum"] = null;
        }
        else
        {
            lblDate.Text = txtDate.Text;
        }
    }

    protected void init_date_scroll(object sender, EventArgs e)
    {
        if ((DateTime.Parse(txtDate.Text) - DateTime.Today).TotalDays <= -50)
        {
            imgDatePrev.Visible = false;
        }
        else if ((DateTime.Parse(txtDate.Text) - DateTime.Today).TotalDays >= 50)
        {
            imgDateNext.Visible = false;
        }
        else
        {
            imgDatePrev.Visible = true;
            imgDateNext.Visible = true;
        }
    }

    //protected void btnSearch_Click(object sender, EventArgs e)
    //{
    //    MessageBox.Show("PO NUM ENTERED IS: " + txtSearch.Text);
    //}

    //protected void po_search(object sender, EventArgs e)
    //{
    //    Session["SE_ponum"] = txtSearch.Text;

    //    Page_Load(null, null);
    //}

    protected void init_table(object sender, EventArgs e, string sql)
    {        
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
            init_title(null, null);
            init_date_scroll(null, null);
    }

    protected void ImgDatePrev_Click(object sender, EventArgs e)
    {
        DateTime dateTime = Convert.ToDateTime(this.txtDate.Text.Trim(), new CultureInfo("en-GB"));
        txtDate.Text = dateTime.AddDays(-1).ToString("dd-MMM-yy").ToUpper();
        sql = "SELECT PO_NUM AS \"PO NUMBER\", SUPPLIER, REQ_DATETIME AS \"DATE\" FROM XXPOC_PO_DETAILS WHERE REQ_DATETIME >= to_date('" + txtDate.Text + "','dd-MON-yy') AND REQ_DATETIME < to_date('" + txtDate.Text + "','dd-MON-yy') +1";
        init_table(null, null, sql);
    }

    protected void ImgDateNext_Click(object sender, EventArgs e)
    {
        DateTime dateTime = Convert.ToDateTime(this.txtDate.Text.Trim(), new CultureInfo("en-GB"));
        txtDate.Text = dateTime.AddDays(1).ToString("dd-MMM-yy").ToUpper();
        sql = "SELECT PO_NUM AS \"PO NUMBER\", SUPPLIER, REQ_DATETIME AS \"DATE\" FROM XXPOC_PO_DETAILS WHERE REQ_DATETIME >= to_date('" + txtDate.Text + "','dd-MON-yy') AND REQ_DATETIME < to_date('" + txtDate.Text + "','dd-MON-yy') +1";
        init_table(null,null,sql);

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
            string gridviewValue = GridView1.SelectedRow.Cells[0].Text;
            Session["SE_ponum"] = gridviewValue;
            Response.Redirect("Goods_In_2.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        catch (System.Exception)
        {
            showmsg("NO PURCHASE ORDER SELECTED");
            init_title(null, null);
        }      
    }

    protected void GridView1_RowPostBack(object sender, GridViewRowEventArgs e)
    {
        int rowindex = Convert.ToInt32(Session["SE_poheadrowindex"]);
        foreach (GridViewRow row in GridView1.Rows)
        {
            if (row.RowIndex == rowindex)
            {
                row.BackColor = ColorTranslator.FromHtml("#d8d8d8");
            }
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
                Session["SE_poheadrowindex"] = row.RowIndex;
                init_title(null, null);
            }
            else
            {
                row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
            }
        }
    }

}



