<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Help.aspx.cs" Inherits="Business_Home" MasterPageFile="~/Site.Master"%>
<asp:Content id="myContent" runat="server" ContentPlaceholderId="ContentPlaceHolder1">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"/>

         <h1>Help</h1>

         <hr />

         <div class="form_block" style="float: right; margin-right: 0px;">
         Morrisons IT Service Desk
         <br /><br />
         <strong>0845 611 6500</strong>
         <br /><br />
         <em>Please ask the Service Desk to follow <strong>KBE1196</strong> when logging the call.</em>
         <br /><br />
         Supported by: <strong>DEVMAN</strong>
         <br /><br />
         </div>
         <div style="display: inline-block; vertical-align: top; width: 70%; background: #fff;">
         <h2>Common queries</h2>
         <h3>Pages appear to be loading slowly or crashing</h3>
         Many queries fetch large amounts of data across the internet from the Niche servers at Colne, Spalding, and Turriff and return it to this site located at Hilmore House. This can take some time, especially when other site wide activities are going on, such as sending large e-mails or accessing central network drives.<br />
         <br />
         <h3>I keep being taken to an error page</h3>
         Sometimes when there is a large load on the network or the Niche database, there will be a restriction on how many users can be connected at once. There may also be temporary network connectivity issues between Hilmore House and one of the servers which may be causing these errors. In any instance, wait a few minutes and try again.<br /><br />If it persists for more than one hour you can contact the Morrisons IT Service Desk, detailing what you were doing at the time, and the page you were attempting to access.<br />
         <br />
         <h3>I keep being asked to enter a username and password</h3>
         This means that you&#39;re attempting to access a part of the site that is outside of the permissions level set against your account. If you need to have access to a specific area, contact the Morrisons IT Service Desk, specifying which part of the site you&#39;re attempting to access and the reasons for requiring this and we&#39;ll look at amending your access.<br />
         <br />
         <h3>I had a page bookmarked / in my favourites that now shows an error message</h3>
         Some pages have been moved to streamline the directories that the site runs in. This has meant that some of the old addresses no longer work. Manually browse for the page by pressing on the home icon (top left of the page), and locating it through the updated menu structure.<br />
         </div>

         </form>
        </body>
</html>
</asp:Content>
