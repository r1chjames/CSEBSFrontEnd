using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
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
using System.Net.Sockets;

public partial class Goods_In_2 : System.Web.UI.Page
{
    private OracleConnection Connect = new OracleConnection();
    private string sql, EBSDM13A, ponum, itemnum, subinv, locator, lotnum, lpnnum, errbuff, retcode, recnum;
    private int poid, polineid, supp;
    private decimal qty;
    
    public Goods_In_2()
    {
        EBSDM13A = "Data Source=(DESCRIPTION="
               + "(ADDRESS=(PROTOCOL=TCP)(HOST=tebvm73.unix.company.net) (PORT=1521))"
               + "(CONNECT_DATA=(SERVICE_NAME=EBSDM13A)));"
               + "User Id=XXPOC;Password=oragang12;";
    }

    public void Page_Load(object sender, EventArgs e)
    {
        init_fields(null, null);
        init_title(null, null);
        init_PO_lines_table(null, null);
        if (Session["SE_polinerowindex"] != null)
        {
            GridView1_RowPostBack(null, null);
        }
    }

    protected void init_fields(object sender, EventArgs e)
    {
        ponum = (string)(Session["SE_ponum"]);
        subinv = (string)(Session["SE_subinv"]);
        locator = (string)(Session["SE_locator"]);
        itemnum = (string)(Session["SE_itemnum"]);
        txtBoxWeight.Text = (string)(Session["SE_weight"]);
        txtBoxLPN.Text = (string)ViewState["VS_lpnnum"];
        txtBoxLot.Text = (string)ViewState["VS_lotnum"];

        if (subinv == null)
        {
            txtBoxSubInv.Text = "GOODS-IN";
        }
        if (subinv != null)
        {
            txtBoxSubInv.Text = subinv;
        }
        if (locator != "")
        {
            txtBoxLocator.Text = locator;
        }
    }

    protected void init_title(object sender, EventArgs e)
    {
        string LC_supp;
        sql = "SELECT SUPPLIER FROM XXPOC_PO_DETAILS WHERE PO_NUM = '" + ponum + "'";
        using (OracleConnection conn = new OracleConnection())
        {
            try
            {
                conn.ConnectionString = EBSDM13A;
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    LC_supp = dr.GetString(0);
                    lblTitle.Text = ponum + " - " + LC_supp;
                }
            }
            catch (OracleException ex)
            {
                Debug.WriteLine("Exception Message: " + ex.Message);
                Debug.WriteLine("Exception Source: " + ex.Source);
            }
        }
    }
  
    protected void generate_lot(string org_id, string item_id)
    {
        using (OracleConnection conn = new OracleConnection())
        {
            try
            {
                conn.ConnectionString = EBSDM13A;
                conn.Open();
                string stored_procedure = "DECLARE "
                    + "p_lot_number VARCHAR2(2000); "
                    + "BEGIN "
                    + "DBMS_OUTPUT.ENABLE; "
                    + "xxpoc_goods_in.create_lot_num(" + org_id + ", " + item_id + ", p_lot_number); "
                    + "DBMS_OUTPUT.PUT_LINE(p_lot_number); "
                    + "END;";
                string anonymous_block = "BEGIN "
                    + "DBMS_OUTPUT.GET_LINES(:1, :2); "
                    + "END;";
                // used to indicate number of lines to get during each fetch
                const int NUM_TO_FETCH = 3;

                // used to determine number of rows fetched in anonymous pl/sql block
                int numLinesFetched = 0;

                // simple loop counter used below
                int i = 0;

                // create command and execute the stored procedure
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = stored_procedure;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                // create parameter objects for the anonymous pl/sql block
                OracleParameter p_lines = new OracleParameter("", OracleDbType.Varchar2, NUM_TO_FETCH, "", ParameterDirection.Output);

                p_lines.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                p_lines.ArrayBindSize = new int[NUM_TO_FETCH];

                // set the bind size value for each element
                for (i = 0; i < NUM_TO_FETCH; i++)
                {
                    p_lines.ArrayBindSize[i] = 32000;
                }

                // this is an input output parameter...
                // on input it holds the number of lines requested to be fetched from the buffer
                // on output it holds the number of lines actually fetched from the buffer
                OracleParameter p_numlines = new OracleParameter("", OracleDbType.Decimal, "", ParameterDirection.InputOutput);

                // set the number of lines to fetch
                p_numlines.Value = NUM_TO_FETCH;

                // set up command object and execute anonymous pl/sql block
                cmd.CommandText = anonymous_block;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(p_lines);
                cmd.Parameters.Add(p_numlines);
                cmd.ExecuteNonQuery();

                // get the number of lines that were fetched (0 = no more lines in buffer)
                numLinesFetched = ((OracleDecimal)p_numlines.Value).ToInt32();

                // as long as lines were fetched from the buffer...
                while (numLinesFetched > 0)
                {
                    // write the text returned for each element in the pl/sql
                    // associative array to the console window
                    for (i = 2; i < 3; i++)
                    {
                        lotnum = (p_lines.Value as OracleString[])[2].ToString();
                        txtBoxLot.Text = lotnum;
                        ViewState["VS_lotnum"] = lotnum;
                    }

                    // re-execute the command to fetch more lines (if any remain)
                    cmd.ExecuteNonQuery();

                    // get the number of lines that were fetched (0 = no more lines in buffer)
                    numLinesFetched = ((OracleDecimal)p_numlines.Value).ToInt32();
                }

                // clean up
                p_numlines.Dispose();
                p_lines.Dispose();
                cmd.Dispose();
            }
            catch (OracleException ex)
            {
                Debug.WriteLine("Exception Message: " + ex.Message);
                Debug.WriteLine("Exception Source: " + ex.Source);
            }
        }
    }

    protected void generate_LPN(string org_id)
    {
        using (OracleConnection conn = new OracleConnection())
        {
            try
            {
                conn.ConnectionString = EBSDM13A;
                conn.Open();
                string stored_procedure = "DECLARE "
                    + "p_lpn_id NUMBER; "
                    + "p_lpn_number VARCHAR2(50); "
                    + "BEGIN "
                    + "DBMS_OUTPUT.ENABLE; "
                    + "xxpoc_goods_in.create_lpn(" + org_id + ", p_lpn_id, p_lpn_number); "
                    + "DBMS_OUTPUT.PUT_LINE(p_lpn_number); "
                    + "END;";
                string anonymous_block = "BEGIN "
                    + "DBMS_OUTPUT.GET_LINES(:1, :2); "
                    + "END;";
                // used to indicate number of lines to get during each fetch
                const int NUM_TO_FETCH = 1;

                // used to determine number of rows fetched in anonymous pl/sql block
                int numLinesFetched = 0;

                // simple loop counter used below
                int i = 0;

                // create command and execute the stored procedure
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = stored_procedure;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                // create parameter objects for the anonymous pl/sql block
                OracleParameter p_lines = new OracleParameter("", OracleDbType.Varchar2, NUM_TO_FETCH, "", ParameterDirection.Output);

                p_lines.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                p_lines.ArrayBindSize = new int[NUM_TO_FETCH];

                // set the bind size value for each element
                for (i = 0; i < NUM_TO_FETCH; i++)
                {
                    p_lines.ArrayBindSize[i] = 32000;
                }

                // this is an input output parameter...
                // on input it holds the number of lines requested to be fetched from the buffer
                // on output it holds the number of lines actually fetched from the buffer
                OracleParameter p_numlines = new OracleParameter("", OracleDbType.Decimal, "", ParameterDirection.InputOutput);

                // set the number of lines to fetch
                p_numlines.Value = NUM_TO_FETCH;

                // set up command object and execute anonymous pl/sql block
                cmd.CommandText = anonymous_block;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(p_lines);
                cmd.Parameters.Add(p_numlines);
                cmd.ExecuteNonQuery();

                // get the number of lines that were fetched (0 = no more lines in buffer)
                numLinesFetched = ((OracleDecimal)p_numlines.Value).ToInt32();

                // as long as lines were fetched from the buffer...
                while (numLinesFetched > 0)
                {
                    // write the text returned for each element in the pl/sql
                    // associative array to the console window
                    for (i = 0; i < 1; i++)
                    {
                        lpnnum = (p_lines.Value as OracleString[])[0].ToString();
                        txtBoxLPN.Text = lpnnum;
                        ViewState["VS_lpnnum"] = lpnnum;
                    }

                    // re-execute the command to fetch more lines (if any remain)
                    cmd.ExecuteNonQuery();

                    // get the number of lines that were fetched (0 = no more lines in buffer)
                    numLinesFetched = ((OracleDecimal)p_numlines.Value).ToInt32();
                }

                // clean up
                p_numlines.Dispose();
                p_lines.Dispose();
                cmd.Dispose();
            }
            catch (OracleException ex)
            {
                Debug.WriteLine("Exception Message: " + ex.Message);
                Debug.WriteLine("Exception Source: " + ex.Source);
            }
        }
    }

    protected void init_PO_lines_table(object sender, EventArgs e)
    {
        sql = "SELECT LINE_NUM AS \"LINE\", ITEM_NUM AS \"ITEM NO\", ITEM_DESCRIPTION AS \"ITEM DESC\", QUANTITY AS \"QTY\", BALANCE AS \"BAL\" FROM XXPOC_PO_LINE_DETAILS WHERE SEGMENT1 = '" + ponum + "'";
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

    protected void GridView1_RowPostBack(object sender, GridViewRowEventArgs e)
    {
        int rowindex = Convert.ToInt32(Session["SE_polinerowindex"]);
        foreach (GridViewRow row in GridView1.Rows)
        {
            if (row.RowIndex == rowindex)
            {
                row.BackColor = ColorTranslator.FromHtml("#d8d8d8");
            }
        }
    }

    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to select this row";

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
                itemnum = GridView1.SelectedRow.Cells[1].Text;
                Session["SE_itemnum"] = itemnum;
                Session["SE_polinerowindex"] = row.RowIndex;
            }
            else
            {
                row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                row.ToolTip = "Click to select this row";
            }
        }
    }

    protected void btnReadScale_Click(object sender, EventArgs e)
    {
        string weight;
        try
        {
            Int32 port = 1365;
            TcpClient client = new TcpClient("10.3.109.247", port);
            Byte[] data = System.Text.Encoding.ASCII.GetBytes("%q");
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
            data = new Byte[256];
            String responseData = String.Empty;
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            stream.Close();
            client.Close();
            weight = responseData.Substring(5).Replace(",", ".").Replace("+", "").Replace("!", "");
            //weight = responseData.Substring(6).Replace(",", ".");
            txtBoxWeight.Text = weight;
            Session["SE_weight"] = weight;
        }
        catch (ArgumentNullException ex)
        {
            Debug.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException ex)
        {
            Debug.WriteLine("SocketException: {0}", e);
        }
    }

    protected void BoxProdKillDateEnter_Click(object sender, EventArgs e)
    {

    }

    protected void btnSuppLotEnter_Click(object sender, EventArgs e)
    {

    }

    protected void btnCOOEnter_Click(object sender, EventArgs e)
    {

    }

    public void showmsg(string str)
    {
        string prompt =
        "<script>$(document).ready(function(){{$.prompt('{0}',{1});}});</script>";
        string message = string.Format(prompt, str, "{ prefix: 'jqi' }");
        this.Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", message);
    }

    protected void imgSubInv_Click(object sender, EventArgs e)
    {
        Response.Redirect("Goods_In_SubInv.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }

    protected void imgLocator_Click(object sender, ImageClickEventArgs e)
    {
        Session["SE_subinv"] = txtBoxSubInv.Text;
        Response.Redirect("Goods_In_Locator.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }

    protected void imgBack_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("Goods_In_1.aspx");
        Context.ApplicationInstance.CompleteRequest();
    }

    protected void get_data(object sender, EventArgs e)
    {
        string LC_pohdsql;
        string LC_polineidsql;
        string LC_suppsql;
        //Get the PO line from the datagrid
        try
        {
            //gridviewvalue = GridView1.SelectedRow.Cells[1].Text;
            //itemnum = gridviewvalue;
            ViewState["VS_itemnum"] = itemnum;
        }
        catch
        {
            showmsg("NO PO LINE SELECTED");
        }

        try
        {
            qty = Convert.ToDecimal(txtBoxWeight.Text.Replace("k", "").Replace("g", ""));
        }
        catch
        {
            showmsg("NO WEIGHT CAPTURED");
        }
        generate_lot("140", itemnum);
        generate_LPN("140");
        LC_pohdsql = "SELECT PO_HEADER_ID FROM XXPOC_PO_LINE_DETAILS WHERE rownum <= 1 AND segment1 = '" + ponum + "'";
        LC_polineidsql = "SELECT PO_LINE_ID FROM XXPOC_PO_LINE_DETAILS WHERE SEGMENT1 = '" + ponum + "' AND ITEM_NUM = '" + itemnum + "'";
        LC_suppsql = "SELECT SUPPLIER_ID FROM xxpoc_po_details WHERE PO_NUM = '" + ponum + "'";
        lotnum = txtBoxLot.Text;
        lpnnum = txtBoxLPN.Text;
        //Get the PO Header ID and put into variable poHd
        using (OracleConnection conn = new OracleConnection())
        {
            try
            {
                conn.ConnectionString = EBSDM13A;
                conn.Open();
                OracleCommand cmd = new OracleCommand(LC_pohdsql, conn);
                cmd.CommandType = CommandType.Text;

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    poid = dr.GetInt32(0);
                }
            }
            catch (OracleException ex)
            {
                Debug.WriteLine("Exception Message: " + ex.Message);
                Debug.WriteLine("Exception Source: " + ex.Source);
            }
        }
        //Get the PO Line ID and put into variable poHd
        using (OracleConnection conn = new OracleConnection())
        {
            try
            {
                conn.ConnectionString = EBSDM13A;
                conn.Open();
                OracleCommand cmd = new OracleCommand(LC_polineidsql, conn);
                cmd.CommandType = CommandType.Text;

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    polineid = dr.GetInt32(0);
                }
            }
            catch (OracleException ex)
            {
                Debug.WriteLine("Exception Message: " + ex.Message);
                Debug.WriteLine("Exception Source: " + ex.Source);
            }
        }
        //Get the supplier and put into variable supp
        using (OracleConnection conn = new OracleConnection())
        {
            try
            {
                conn.ConnectionString = EBSDM13A;
                conn.Open();
                OracleCommand cmd = new OracleCommand(LC_suppsql, conn);
                cmd.CommandType = CommandType.Text;

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    supp = dr.GetInt32(0);
                }
            }
            catch (OracleException ex)
            {
                Debug.WriteLine("Exception Message: " + ex.Message);
                Debug.WriteLine("Exception Source: " + ex.Source);
            }
        }
    }

    protected void imgReceive_Click(object sender, ImageClickEventArgs e)
    {
        get_data(null, null);
        int LC_orgid = 140;

        using (OracleConnection conn = new OracleConnection())
        {
            try
            {
                conn.ConnectionString = EBSDM13A;
                conn.Open();
                //OracleCommand cmd = new OracleCommand(sql, conn);
                string stored_procedure = "DECLARE "
                    + "l_errbuf VARCHAR2(1000); "
                    + "l_retcode VARCHAR2(10); "
                    + "l_receipt_num NUMBER; "
                    + "BEGIN "
                    + "DBMS_OUTPUT.ENABLE; "
                    + "xxpoc_goods_in.main "
                    + "(l_errbuf, "
                    + "l_retcode, "
                    + "L_RECEIPT_NUM, "
                    + LC_orgid + ", "
                    + supp + ", "
                    + poid + ", "
                    + polineid + ", "
                    + qty + ", "
                    + "'" + lotnum + "'" + ", "
                    + "'" + lpnnum + "'"
                    + ");"
                    + " DBMS_OUTPUT.PUT_LINE('Error Buf is '        || L_ERRBUF);"
                    + " DBMS_OUTPUT.PUT_LINE('Return Code is ' || L_RETCODE);"
                    + " DBMS_OUTPUT.PUT_LINE('Receipt Num ' || L_RECEIPT_NUM);"
                    + "END;";
                Debug.Write(stored_procedure);
                string anonymous_block = "BEGIN "
                    + "DBMS_OUTPUT.GET_LINES(:1, :2); "
                    + "END;";
                // used to indicate number of lines to get during each fetch
                const int NUM_TO_FETCH = 4;

                // used to determine number of rows fetched in anonymous pl/sql block
                int numLinesFetched = 0;

                // simple loop counter used below
                int i = 0;

                // create command and execute the stored procedure
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = stored_procedure;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                // create parameter objects for the anonymous pl/sql block
                OracleParameter p_lines = new OracleParameter("", OracleDbType.Varchar2, NUM_TO_FETCH, "", ParameterDirection.Output);

                p_lines.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                p_lines.ArrayBindSize = new int[NUM_TO_FETCH];

                // set the bind size value for each element
                for (i = 0; i < NUM_TO_FETCH; i++)
                {
                    p_lines.ArrayBindSize[i] = 32000;
                }

                // this is an input output parameter...
                // on input it holds the number of lines requested to be fetched from the buffer
                // on output it holds the number of lines actually fetched from the buffer
                OracleParameter p_numlines = new OracleParameter("", OracleDbType.Decimal, "", ParameterDirection.InputOutput);

                // set the number of lines to fetch
                p_numlines.Value = NUM_TO_FETCH;

                // set up command object and execute anonymous pl/sql block
                cmd.CommandText = anonymous_block;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(p_lines);
                cmd.Parameters.Add(p_numlines);
                cmd.ExecuteNonQuery();

                // get the number of lines that were fetched (0 = no more lines in buffer)
                numLinesFetched = ((OracleDecimal)p_numlines.Value).ToInt32();

                // as long as lines were fetched from the buffer...
                while (numLinesFetched > 0)
                {
                    // write the text returned for each element in the pl/sql
                    // associative array to the console window
                    for (i = 0; i < 1; i++)
                    {
                        Debug.WriteLine(i);
                        errbuff = (p_lines.Value as OracleString[])[1].ToString();
                        retcode = (p_lines.Value as OracleString[])[2].ToString();
                        recnum = (p_lines.Value as OracleString[])[3].ToString().Remove(0,12);
                    }

                    // re-execute the command to fetch more lines (if any remain)
                    cmd.ExecuteNonQuery();

                    // get the number of lines that were fetched (0 = no more lines in buffer)
                    numLinesFetched = ((OracleDecimal)p_numlines.Value).ToInt32();
                }

                // clean up
                p_numlines.Dispose();
                p_lines.Dispose();
                cmd.Dispose();
                //MessageBox.Show("RECEIPT SUCCESSFUL. RECEIPT NUMBER IS: " + recNum);
                showmsg("TRANSACTION SUCCESSFUL<br /><br />RECEIPT NUMBER <strong>" + recnum + "</strong>");
            }
            catch (OracleException ex)
            {
                Debug.WriteLine("Exception Message: " + ex.Message);
                Debug.WriteLine("Exception Source: " + ex.Source);
                //MessageBox.Show("RECEIPT UNSUCCESSFUL. RETURN CODE IS: " + retcode + " AND ERROR IS: " + errbuff);
                showmsg("TRANSACTION FAILED<br /><br />RETURN CODE <strong>" + retcode + "</strong> ERROR <strong>" + errbuff + "</strong>");
            }
        }

        Page_Load(null, null);

    }
}



