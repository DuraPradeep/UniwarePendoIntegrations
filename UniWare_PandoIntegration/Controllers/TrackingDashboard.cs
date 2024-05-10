using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.Entities;

namespace UniWare_PandoIntegration.Controllers
{
    public class TrackingDashboard : Microsoft.AspNetCore.Mvc.Controller
    {
        ApiOperation ApiControl;
        private readonly IConfiguration iconfiguration;
        private readonly string Apibase;
        List<TrackingDetails> trackingDetails;
        public TrackingDashboard(IConfiguration configuration)
        {
            Apibase = configuration.GetSection("baseaddress:Url").Value;
        }
        public IActionResult Index()
        {
            return View();
        }
        public Microsoft.AspNetCore.Mvc.ActionResult DashBoard()
        {
            return View();
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public Microsoft.AspNetCore.Mvc.JsonResult Getalldetails()
        {
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            List<TDashboardDetails> dashboardDetail = new List<TDashboardDetails>();
            trackingDetails = new List<TrackingDetails>();

            var DashboardDetails = ApiControl.Get<ServiceResponse<DashboardsLists>, string>(Enviornment, "Enviornment", "api/UniwarePando/GetDashboardDetails");
            DashboardsLists dashboardsLists = new DashboardsLists();
            dashboardsLists.dashboardDetails = DashboardDetails.ObjectParam.dashboardDetails;
            dashboardsLists.trackingDetails = DashboardDetails.ObjectParam.trackingDetails;
            return Json(dashboardsLists);
        }

        public Microsoft.AspNetCore.Mvc.JsonResult GetStatusDetailsByName(string Name)
        {
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            List<TDashboardDetails> dashboardDetail = new List<TDashboardDetails>();
            trackingDetails = new List<TrackingDetails>();
            var DashboardDetails = ApiControl.Get<List<TDashboardDetails>, string, string>(Enviornment, Name, "Enviornment", "Name", "api/UniwarePando/GetDashboardDetailsByName");
            return Json(DashboardDetails);

        }
        public Microsoft.AspNetCore.Mvc.ActionResult TrackingStatus()
        {
            return View();
        }
        public Microsoft.AspNetCore.Mvc.JsonResult GettrackingLink(string SearchBy,string trackingNo)
        {
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            var DashboardDetails = ApiControl.Get<List<TDashboardDetails>, string, string,string>(Enviornment, SearchBy, trackingNo, "Enviornment", "SearchBy", "trackingNo", "api/UniwarePando/GetTrackingLink");
            return Json(DashboardDetails);

        }

    }
}
