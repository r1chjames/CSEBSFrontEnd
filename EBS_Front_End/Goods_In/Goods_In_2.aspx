<%@ Page Language="C#" AutoEventWireup="True" Inherits="Goods_In_2" MasterPageFile="~/Site.Master" CodeBehind="Goods_In_2.aspx.cs" EnableEventValidation="false" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">Goods In</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder2">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html>
    <head>
        <title>Goods In</title>
        <style type="text/css">
            .modal {
                display: none;
                position: fixed;
                z-index: 1000;
                top: 0;
                left: 0;
                height: 100%;
                width: 100%;
                background: #777 url('../Styles/Images/loading.gif') 50% 50% no-repeat;
                opacity: 0.6;
                filter: alpha(opacity=60);
            }

            body.loading {
                overflow: hidden;
            }

                body.loading .modal {
                    display: block;
                }
        </style>
        <script type="text/javascript">
            $(document).ready(function () {
                $("#goods_grid_2").scroller();
                $("[id$=imgExp]").click(function () {
                    $("[id$=expDate]").datepicker('show');
                });
                $("[id$=imgPro]").click(function () {
                    $("[id$=proDate]").datepicker('show');
                });
                $("[id$=expDate]").datepicker({
                    constrainInput: "true",
                    prevText: "PREVIOUS",
                    nextText: "NEXT",
                    dayNamesMin: ["SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"],
                    minDate: "0",
                    maxDate: "+1y",
                    dateFormat: "dd-M-y"
                });
                $("[id$=proDate]").datepicker({
                    constrainInput: "true",
                    prevText: "PREVIOUS",
                    nextText: "NEXT",
                    dayNamesMin: ["SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"],
                    minDate: "-1y",
                    maxDate: "+1y",
                    dateFormat: "dd-M-y"
                });
                $("[id$=imgSuppLot]").click(function () {
                    $("[id$=txtSuppLot]").getkeyboard().reveal();
                });
                $("[id$=txtSuppLot]").keyboard({
                    layout: 'custom',
                    customLayout: { 'default': ['1 2 3 4 5 6 7 8 9 0 - {bksp}', 'Q W E R T Y U I O P', 'A S D F G H J K L', 'Z X C V B N M .', '{cancel} {left} {space} {right} {accept}'] },
                    display: { 'bksp': 'Backspace:Backspace' },
                    appendTo: 'body',
                    maxLength: '20',
                });
                $("[id$=imgOrig]").click(function () {
                    $("[id$=txtOrg]").getkeyboard().reveal();
                });
                $("[id$=txtOrg]").keyboard({
                    layout: 'custom',
                    customLayout: { 'default': ['1 2 3 4 5 6 7 8 9 0 - {bksp}', 'Q W E R T Y U I O P', 'A S D F G H J K L', 'Z X C V B N M .', '{cancel} {left} {space} {right} {accept}'] },
                    display: { 'bksp': 'Backspace:Backspace' },
                    appendTo: 'body',
                    maxLength: '20',
                });
                $body = $("body");
                $("[id$=imgReceive]").click(function () { $body.addClass("loading"); });
                $("[id$=GridView1]").click(function () { $body.addClass("loading"); });
                $("[id$=imageReadScale]").click(function () { $body.addClass("loading"); });
            });
        </script>
    </head>
    <body>
        <form id="form1" runat="server">
            <div id="main_container">
                <table style="width: 100%; margin-left: auto; margin-right: auto;">
                    <tr>
                        <td style="width: 84px; height: 74px;"></td>
                        <td class="Date_title">
                            <asp:Label ID="lblTitle" runat="server" />
                        </td>
                        <td style="width: 84px; height: 74px;"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>

                            <div id="grid_header">
                                <table id="grid_header_2">
                                    <tr>
                                        <td style="width: 75px;">LINE</td>
                                        <td style="width: 126px;">ITEM NO</td>
                                        <td>DESCRIPTION</td>
                                        <td style="width: 125px;">QUANTITY</td>
                                        <td style="width: 105px;">BALANCE</td>
                                    </tr>
                                </table>
                            </div>
                            <div id="goods_grid_2">
                                <asp:GridView ID="GridView1" runat="server" CssClass="Grid" OnRowDataBound="GridView1_OnRowDataBound" ShowHeader="false" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="LINE" HeaderText="LINE" ItemStyle-Width="50px" />
                                        <asp:BoundField DataField="ITEM NO" HeaderText="ITEM NO" ItemStyle-Width="100px" />
                                        <asp:BoundField DataField="ITEM DESC" HeaderText="ITEM DESC" />
                                        <asp:BoundField DataField="QTY" HeaderText="QTY" ItemStyle-Width="100px" />
                                        <asp:BoundField DataField="BAL" HeaderText="BAL" ItemStyle-Width="100px" />
                                    </Columns>
                                </asp:GridView>
                            </div>
            </div>

            </td>
                <td></td>
            </tr>
            <tr>

                <td></td>
                <td>
                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 26%;" class="goods_box">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="white-space: pre;">WEIGHT</td>
                                        <td>
                                            <asp:TextBox ID="txtBoxWeight" runat="server" ReadOnly="True" onclick="btnReadScale_Click" CausesValidation="true" class="text_box_short"></asp:TextBox></td>
                                        <td>&nbsp;<asp:ImageButton Style="cursor: pointer;" ID="imageReadScale" runat="server" OnClick="btnReadScale_Click" ImageUrl="~/Styles/Images/calculator.png" title="Read Weight" CausesValidation="true" /></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 20px;"></td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: pre;">LPN</td>
                                        <td>
                                            <asp:TextBox ID="txtBoxLPN" runat="server" ReadOnly="True" class="text_box_disabled"></asp:TextBox></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 20px;"></td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: pre;">LOT</td>
                                        <td>
                                            <asp:TextBox ID="txtBoxLot" runat="server" ReadOnly="True" class="text_box_disabled"></asp:TextBox></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 2%;">&nbsp;</td>
                            <td style="width: 35%;" class="goods_box">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="white-space: pre;">SUB INVENTORY</td>
                                        <td>
                                            <asp:TextBox ID="txtBoxSubInv" runat="server" class="text_box"></asp:TextBox></td>
                                        <td>&nbsp;<asp:ImageButton ID="imgSubInv" OnClick="imgSubInv_Click" runat="server" ImageUrl="~/Styles/Images/search_small.png" title="Select" CausesValidation="true" /></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 20px;"></td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: pre;">LOCATOR</td>
                                        <td>
                                            <asp:TextBox ID="txtBoxLocator" runat="server" class="text_box"></asp:TextBox></td>
                                        <td>&nbsp;<asp:ImageButton ID="imgLocator" OnClick="imgLocator_Click" runat="server" ImageUrl="~/Styles/Images/search_small.png" title="Select" CausesValidation="true" /></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 20px;"></td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: pre;">SUPPLIER LOT</td>
                                        <td>
                                            <asp:TextBox ID="txtSuppLot" runat="server" class="text_box" Style="border: 1px solid #d8d8d8;"></asp:TextBox></td>
                                        <td>&nbsp;<asp:Image ID="imgSuppLot" runat="server" ImageUrl="~/Styles/Images/keyboard_small.png" title="Select" CausesValidation="true" /></td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 2%;">&nbsp;</td>
                            <td style="width: 35%;" class="goods_box">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="white-space: pre;">EXPIRY DATE</td>
                                        <td>
                                            <asp:TextBox ID="expDate" runat="server" class="text_box"></asp:TextBox></td>
                                        <td>&nbsp;<asp:Image ID="imgExp" runat="server" ImageUrl="~/Styles/Images/calendar_small.png" title="Select" CausesValidation="true" /></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 20px;"></td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: pre;">PROD/KILL DATE</td>
                                        <td>
                                            <asp:TextBox ID="proDate" runat="server" class="text_box"></asp:TextBox></td>
                                        <td>&nbsp;<asp:Image ID="imgPro" runat="server" ImageUrl="~/Styles/Images/calendar_small.png" title="Select" CausesValidation="true" /></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 20px;"></td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: pre;">ORIGIN</td>
                                        <td>
                                            <asp:TextBox ID="txtOrg" runat="server" class="text_box" Style="border: 1px solid #d8d8d8;"></asp:TextBox></td>
                                        <td>&nbsp;<asp:Image ID="imgOrig" runat="server" ImageUrl="~/Styles/Images/keyboard_small.png" title="Select" CausesValidation="true" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td></td>
            </tr>
            <tr>
                <td style="width: 84px; height: 84px; text-align: center; vertical-align: bottom;">
                    <asp:ImageButton Style="cursor: pointer;" ID="imgBack" runat="server" ImageUrl="~/Styles/Images/back.png" title="Back" OnClick="imgBack_Click" />
                </td>
                <td style="text-align: right; padding-right: 40px; vertical-align: bottom;">
                </td>
                <td style="width: 84px; height: 84px; text-align: center; vertical-align: bottom;">
                    <asp:ImageButton Style="cursor: pointer;" ID="imgReceive" runat="server" ImageUrl="~/Styles/Images/tick.png" OnClick="imgReceive_Click" title="Receive" />
                </td>
            </tr>
            </table>
    </div>

            <div class="modal">
            </div>

        </form>
    </body>
    </html>
</asp:Content>
