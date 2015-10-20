<%@ Page Language="C#" AutoEventWireup="True" Inherits="Goods_In_1" MasterPageFile="~/Site.Master" Codebehind="Goods_In_1.aspx.cs" EnableEventValidation="false" %>
<asp:Content id="Content1" runat="server" ContentPlaceholderId="ContentPlaceHolder1">Goods In</asp:Content>
<asp:Content id="Content2" runat="server" ContentPlaceholderId="ContentPlaceHolder2">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>Goods In</title>
        <style type="text/css">
            .modal { display: none; position: fixed; z-index: 1000; top: 0; left: 0; height: 100%; width: 100%; background: #777 url('Styles/Images/loading.gif') 50% 50% no-repeat; opacity: 0.6; filter: alpha(opacity=60); }
            body.loading { overflow: hidden; }
            body.loading .modal { display: block; }
        </style>
    <script type="text/javascript">
        $(document).ready(function()
        {
            $("#goods_grid").scroller();
            $("[id$=txtDate]").datepicker({
                constrainInput: "true",
                prevText: "PREVIOUS",
                nextText: "NEXT",
                dayNamesMin: [ "SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT" ],
                minDate: "-50",
                maxDate: "+50",
                dateFormat: "dd-M-y",
                showOn: "button",
                buttonImageOnly: "true",
                buttonImage: "<%= ResolveUrl("~/Styles/Images/calendar.png") %>"
            });
            $("[id$=poSearch]").click(function () {
                $("[id$=txtSearch]").getkeyboard().reveal();
            });
            $("[id$=txtSearch]").keyboard({
                layout: 'custom',
                customLayout: { 'default': ['1 2 3 4 5 6 7 8 9 0 - {bksp}', 'Q W E R T Y U I O P', 'A S D F G H J K L', 'Z X C V B N M .', '{cancel} {left} {space} {right} {accept}'] },
                display: { 'bksp': 'Backspace:Backspace' },
                appendTo: 'body',
                maxLength: '20',
            });
            $body = $("body");
            $("[id$=imgContinue]").click(function () { $body.addClass("loading"); });
            $("[id$=GridView1]").click(function () { $body.addClass("loading"); });
        });
    </script>
</head>
<body>
<form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div id="main_container">
        <table style="width: 100%; margin-left: auto; margin-right: auto;">
            <tr>
                <td style="width: 84px; height: 64px; text-align: center; padding-top: 10px;">
                    <%--<asp:Image ID="poSearch" runat="server" ImageUrl="~/Styles/Images/search.png" title="Search" />--%>
                    <%--<span class="form_block"><asp:TextBox ID="txtSearch" class="text_box" runat="server" AutoPostBack="false" style="border: 1px solid #d8d8d8; border-radius: 4px; display: none; text-transform: uppercase;" /></span>--%>
                </td>
                <td class="Date_title">
                    <asp:Label ID="lblDate" runat="server" />
                </td>
                <td style="width: 84px; height: 64px; text-align: center; padding-top: 10px;">
                    <span class="form_block"><asp:TextBox ID="txtDate" runat="server" AutoPostBack="true" style="display: none; text-transform: uppercase;" /></span>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: middle; text-align: center;">
                    <asp:ImageButton Style="cursor: pointer;" ID="imgDatePrev" runat="server" ImageUrl="~/Styles/Images/left_arrow.png" OnClick="ImgDatePrev_Click" title="Previous" />
                </td>
                <td>
                <div id="grid_header">
                    <table id="grid_header_2">
                        <tr>
                            <td style="width: 152px;">PO NUMBER</td><td>SUPPLIER</td><td style="width: 104px;">DATE</td>
                        </tr>
                    </table>
                </div>
                <div id="goods_grid">   
                    <asp:GridView ID="GridView1" runat="server" CssClass="Grid" OnRowDataBound="GridView1_OnRowDataBound" ShowHeader="false" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" >
                        <Columns>
                            <asp:BoundField DataField="PO NUMBER" HeaderText="PO NUMBER" ItemStyle-Width="130px" />
                            <asp:BoundField DataField="SUPPLIER" HeaderText="SUPPLIER" />
                            <asp:BoundField DataField="DATE" HeaderText="DATE" DataFormatString="{0:dd-MMM-yy}" ItemStyle-Width="100px" />
                        </Columns>
                    </asp:GridView>
                </div>
                </td>
                <td style="vertical-align: middle; text-align: center;">
                    <asp:ImageButton style="cursor: pointer;" ID="imgDateNext" runat="server" ImageUrl="~/Styles/Images/right_arrow.png" onclick="ImgDateNext_Click" title="Next" />
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