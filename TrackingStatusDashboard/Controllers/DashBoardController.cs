using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.Entities;

namespace TrackingStatusDashboard.Controllers
{
    public class DashBoardController : Controller
    {
        ApiOperation ApiControl;
        private readonly string Apibase;

        public DashBoardController(IConfiguration configuration)
        {
            Apibase = configuration.GetSection("baseaddress:Url").Value;
        }
        public IActionResult Index()
        {
            return View();
        }
        public Microsoft.AspNetCore.Mvc.ActionResult TrackingStatus()
        {
            return View("TrackingLinkStatus");
        }
        public Microsoft.AspNetCore.Mvc.JsonResult GettrackingLink(string SearchBy, string trackingNo)
        {
            ApiControl = new ApiOperation(Apibase);
            //var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            var Enviornment = "Prod";
            var DashboardDetails = ApiControl.Get<List<TDashboardDetails>, string, string, string>(Enviornment, SearchBy, trackingNo, "Enviornment", "SearchBy", "trackingNo", "api/Calling/GetTrackingLink");
            return Json(DashboardDetails);

        }
    }
}
