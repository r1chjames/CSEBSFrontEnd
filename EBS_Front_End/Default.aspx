<%@ Page Language="C#" AutoEventWireup="true" Inherits="Default" MasterPageFile="~/Site.Master" Codebehind="Default.aspx.cs" %>
<asp:Content id="Content1" runat="server" ContentPlaceholderId="ContentPlaceHolder1">EBS PoC</asp:Content>
<asp:Content id="Content2" runat="server" ContentPlaceholderId="ContentPlaceHolder2">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>EBS PoC</title>
    <style type="text/css">
            .modal { display: none; position: fixed; z-index: 1000; top: 0; left: 0; height: 100%; width: 100%; background: #777 url('Styles/Images/loading.gif') 50% 50% no-repeat; opacity: 0.6; filter: alpha(opacity=60); }
            body.loading { overflow: hidden; }
            body.loading .modal { display: block; }
        </style>
        <script type="text/javascript">
            $(document).ready(function () {
                $body = $("body");
                $("[id$=Goods_In]").click(function () { $body.addClass("loading"); });
            });
        </script>
</head>
<body>
<form id="DEFAULT" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"/>

    <div id="home_container">
        <a id="Goods_In" href="Goods_In/Goods_In_1.aspx" title="Goods In" class="home_button">Goods In</a>
        <a href="Default.aspx" title="Finished Goods In" class="home_button"><s>Finished Goods In</s></a>
        <a href="Default.aspx" title="Bacon" class="home_button"><s>Bacon</s></a>
    </div>
</form>
</body>
</html>
</asp:Content>
