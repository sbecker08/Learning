using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmailMarketingHubPOC.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Index()
        {
            //if query string param "q" exists it is a post back
            if (Request.QueryString["q"] != null)
            {
                MNet_Common.Auth.ProcessAuthResponse(ConfigurationManager.AppSettings["EncryptionKey"], Request.QueryString["q"]);
            }
            else
            {
                MNet_Common.Auth.SendAuthRequest(ConfigurationManager.AppSettings["EncryptionKey"]);
            }
            //it shouldn't return anything as all the auth controller should do is handle the authentication redirects (either to MNet or back to the originating page)
            return View();
        }
    }
}