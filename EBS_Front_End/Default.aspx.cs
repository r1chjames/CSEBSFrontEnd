using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            clear_session_fields(null, null);
        }

        protected void clear_session_fields(object sender, EventArgs e)
        {
            Session["SE_ponum"] = null;
        }

}


