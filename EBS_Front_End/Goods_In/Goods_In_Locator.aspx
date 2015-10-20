<%@ Page Language="C#" AutoEventWireup="True" Inherits="Goods_In_Locator" MasterPageFile="~/Site.Master" Codebehind="Goods_In_Locator.aspx.cs" EnableEventValidation="false" %>
<asp:Content id="Content1" runat="server" ContentPlaceholderId="ContentPlaceHolder1">Goods In</asp:Content>
<asp:Content id="Content2" runat="server" ContentPlaceholderId="ContentPlaceHolder2">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>Goods In</title>
    <script type="text/javascript">
        $(document).ready(function()
        {
            $("#goods_grid").scroller();
        });
    </script>
</head>
<body>
<form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"/>
    <div id="main_container">
        <table style="width: 100%; margin-left: auto; margin-right: auto;">
            <tr>
                <td style="width: 84px; height: 64px; text-align: center; padding-top: 10px;">
                    <span class="form_block">
                </td>
                <td class="Date_title">
                </td>
                <td style="width: 84px; height: 64px; text-align: center; padding-top: 10px;">
                    <span class="form_block"><asp:TextBox ID="txtDate" runat="server" AutoPostBack="true" style="display: none; text-transform: uppercase;" /></span>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: middle; text-align: center;">
                </td>
                <td>
                <div id="grid_header">
                    <table id="grid_header_2">
                        <tr>
                            <td>LOCATOR</td>
                        </tr>
                    </table>
                </div>
                <div id="goods_grid">   
                    <asp:GridView ID="GridView1" runat="server" CssClass="Grid" OnRowDataBound="GridView1_OnRowDataBound" ShowHeader="false" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" >
                        <Columns>
                            <asp:BoundField DataField="LOCATOR" HeaderText="LOCATOR" ItemStyle-Width="130px" />
                        </Columns>
                    </asp:GridView>
                </div>
                </td>
                <td style="vertical-align: middle; text-align: center;">
                </td>
            </tr>
            <tr>
                <td style="width: 84px; height: 74px; text-align: center;"></td>
                <td></td>
                <td style="width: 84px; height: 74px; text-align: center;">
                    <asp:ImageButton style="cursor: pointer;" ID="imgContinue" runat="server" ImageUrl="~/Styles/Images/tick.png" title="Continue" onclick="btnNext_Click" Text="Next" CausesValidation="true" />
                </td>
            </tr>
        </table>
    </div>  

</form>
</body>
</html>
</asp:Content>