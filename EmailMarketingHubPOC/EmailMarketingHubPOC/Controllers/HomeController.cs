using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace EmailMarketingHubPOC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            //var user = System.Web.HttpContext.Current.User.Identity.Name.Replace("mmreibc\\", "");
            return View();
        }



        public ActionResult Authorize()
        {
            var APIKey = "mdyvurwe4m3etfvkga8nzrp6";
            var secret = "C3TqYbYxYzb3RC9udfcRW8AV";
            var redirect = "http://emailmarketinghub.mmreibc.prv/Home/AuthorizeResponse";
            var url = "https://oauth2.constantcontact.com/oauth2/oauth/siteowner/authorize?response_type=code&client_id=" + APIKey + "&redirect_uri=" + redirect;
            return Redirect(url);
        }
        //
        public ActionResult AuthorizeResponse(string code, string username)
        {
            ViewBag.Message = "Your contact page.";

            var APIKey = "mdyvurwe4m3etfvkga8nzrp6";
            var secret = "C3TqYbYxYzb3RC9udfcRW8AV";
            var redirect = "http://emailmarketinghub.mmreibc.prv/Home/AuthorizeResponse";

            //POST
            var url = "https://oauth2.constantcontact.com/oauth2/oauth/token?grant_type=authorization_code&client_id=" + APIKey + "&client_secret=" + secret + "&code=" + code + "&redirect_uri=" + redirect;

            string token = "";
            using (WebClient wc = new WebClient())
            {
                token = wc.UploadString(url, "");
            }

            //var token = "{\"access_token\": \"c9be2bad-ba98-4129-a942-215011e4e215\",\"expires_in\": 315359873,\"token_type\": \"Bearer\"}";

            //var token = "{\"access_token\": \"c9be2bad-ba98-4129-a942-215011e4e215\",\"expires_in\": 315359873,\"token_type\": \"Bearer\"}";
            var auth = "Bearer " + token.Split(',')[0].Substring(21, 36);

            var data = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/bloomberg.json"));
            data = data.Replace("{{NAME}}", "Test Script at:" + ToUnixTime(DateTime.Now).ToString()).Replace("\r", "").Replace("\n", "");

            var uploadURL = "https://api.constantcontact.com/v2/emailmarketing/campaigns?api_key=" + APIKey;
            string result = "";

            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(HttpRequestHeader.Authorization, auth);
                wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");

                result = wc.UploadString(uploadURL, data);
            }

            return RedirectToAction("Success");
        }

        public ActionResult Success()
        {
            return View();
        }


        private long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
    }
}