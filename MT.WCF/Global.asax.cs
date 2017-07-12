using System;
using System.IO;

namespace MT.WCF
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Startup.AutofacConfiguration();
            Startup.UseLog4Net(new FileInfo(Server.MapPath("/logger.config")));
        }
    }
}