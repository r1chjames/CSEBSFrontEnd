<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Error.aspx.cs" Inherits="Business_Home" MasterPageFile="~/Site.Master"%>
<asp:Content id="myContent" runat="server" ContentPlaceholderId="ContentPlaceHolder1">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"/>

         <h1>Error</h1>


        <div class="error_block">

            <img id="Img1" src="~/Styles/images/cross.png" alt="Back" title="Back" runat="server" style="float: right; cursor: pointer;" onclick="JavaScript: window.history.back(1); return false;" />

            <h2>Page failed to load</h2>

            <p>Please try again in a few minutes.</p>

            <p><a href="Help.aspx"><u>Help</u></a></p>

        </div>

         </form>
        </body>
</html>
</asp:Content>
