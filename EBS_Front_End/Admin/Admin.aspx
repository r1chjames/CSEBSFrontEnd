<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Admin.aspx.cs" Inherits="Admin" MasterPageFile="~/Site.Master"%>
<asp:Content id="myContent" runat="server" ContentPlaceholderId="ContentPlaceHolder1">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"/>

            <h1>Administration</h1>
    <hr />   

            <div class="block_wrapper">

                <a href="~/elmah.axd" runat="server" class="block_grey_large" title="ELMAH Error Log">ELMAH <br /> Error Log</a>

           </div>

      </form>
</body>
</html>
</asp:Content>
