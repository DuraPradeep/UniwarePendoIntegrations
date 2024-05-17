using ClosedXML.Excel;
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

            var DashboardDetails = ApiControl.Get<ServiceResponse<DashboardsLists>, string>(Enviornment, "Enviornment", "api/Calling/GetDashboardDetails");
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
            var DashboardDetails = ApiControl.Get<List<TDashboardDetails>, string, string>(Enviornment, Name, "Enviornment", "Name", "api/Calling/GetDashboardDetailsByName");
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
            var DashboardDetails = ApiControl.Get<List<TDashboardDetails>, string, string,string>(Enviornment, SearchBy, trackingNo, "Enviornment", "SearchBy", "trackingNo", "api/Calling/GetTrackingLink");
            return Json(DashboardDetails);

        }

        public Microsoft.AspNetCore.Mvc.ActionResult HistoryExceldownload(string fromdate,string todate)
        {
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();

            //ApiControl = new ApiOperation(Apibase);
            //var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            //ListcustomerModels = ApiControl.Get<List<FacilityMaintain>, string>(Enviornment, "Enviornment", "api/Calling/GetFacilityMaster_Details");


            ////ListcustomerModels = ApiControl.Get<List<FacilityMaintain>>("api/Calling/GetFacilityMaster_Details");

            //var listdata = ListcustomerModels;
            //string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //string fileName = "Facility Master.xlsx";
            //try
            //{
            //    using (var workbook = new XLWorkbook())
            //    {
            //        IXLWorksheet worksheet =
            //        workbook.Worksheets.Add("FacilityDetails");
            //        worksheet.Cell(1, 1).Value = "Facility Code";
            //        worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //        worksheet.Cell(1, 1).Style.Font.FontColor = XLColor.Black;
            //        worksheet.Cell(1, 1).Style.Font.Bold = true;
            //        worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;

            //        worksheet.Cell(1, 2).Value = "Facility Name";
            //        worksheet.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //        worksheet.Cell(1, 2).Style.Font.FontColor = XLColor.Black;
            //        worksheet.Cell(1, 2).Style.Font.Bold = true;
            //        worksheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;

            //        worksheet.Cell(1, 3).Value = "Address";
            //        worksheet.Cell(1, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //        worksheet.Cell(1, 3).Style.Font.FontColor = XLColor.Black;
            //        worksheet.Cell(1, 3).Style.Font.Bold = true;
            //        worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.LightBlue;

            //        worksheet.Cell(1, 4).Value = "City";
            //        worksheet.Cell(1, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //        worksheet.Cell(1, 4).Style.Font.FontColor = XLColor.Black;
            //        worksheet.Cell(1, 4).Style.Fill.BackgroundColor = XLColor.LightBlue;
            //        worksheet.Cell(1, 4).Style.Font.Bold = true;

            //        worksheet.Cell(1, 5).Value = "State";
            //        worksheet.Cell(1, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //        worksheet.Cell(1, 5).Style.Font.FontColor = XLColor.Black;
            //        worksheet.Cell(1, 5).Style.Font.Bold = true;
            //        worksheet.Cell(1, 5).Style.Fill.BackgroundColor = XLColor.LightBlue;

            //        worksheet.Cell(1, 6).Value = "Pin Code";
            //        worksheet.Cell(1, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //        worksheet.Cell(1, 6).Style.Font.FontColor = XLColor.Black;
            //        worksheet.Cell(1, 6).Style.Font.Bold = true;
            //        worksheet.Cell(1, 6).Style.Fill.BackgroundColor = XLColor.LightBlue;

            //        worksheet.Cell(1, 7).Value = "Mobile";
            //        worksheet.Cell(1, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //        worksheet.Cell(1, 7).Style.Font.FontColor = XLColor.Black;
            //        worksheet.Cell(1, 7).Style.Font.Bold = true;
            //        worksheet.Cell(1, 7).Style.Fill.BackgroundColor = XLColor.LightBlue;

            //        worksheet.Cell(1, 8).Value = "Region";
            //        worksheet.Cell(1, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //        worksheet.Cell(1, 8).Style.Font.FontColor = XLColor.Black;
            //        worksheet.Cell(1, 8).Style.Font.Bold = true;
            //        worksheet.Cell(1, 8).Style.Fill.BackgroundColor = XLColor.LightBlue;

            //        worksheet.Cell(1, 9).Value = "Email";
            //        worksheet.Cell(1, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //        worksheet.Cell(1, 9).Style.Font.FontColor = XLColor.Black;
            //        worksheet.Cell(1, 9).Style.Font.Bold = true;
            //        worksheet.Cell(1, 9).Style.Fill.BackgroundColor = XLColor.LightBlue;

            //        worksheet.Cell(1, 10).Value = "Instance";
            //        worksheet.Cell(1, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //        worksheet.Cell(1, 10).Style.Font.FontColor = XLColor.Black;
            //        worksheet.Cell(1, 10).Style.Font.Bold = true;
            //        worksheet.Cell(1, 10).Style.Fill.BackgroundColor = XLColor.LightBlue;

            //        worksheet.ShowGridLines = true;
            //        for (int index = 1; index <= listdata.Count; index++)
            //        {
            //            worksheet.Cell(index + 1, 1).Value = listdata[index - 1].FacilityCode;
            //            worksheet.Cell(index + 1, 2).Value = listdata[index - 1].FacilityName;
            //            worksheet.Cell(index + 1, 3).Value = listdata[index - 1].Address;
            //            worksheet.Cell(index + 1, 4).Value = listdata[index - 1].City;
            //            worksheet.Cell(index + 1, 5).Value = listdata[index - 1].State;
            //            worksheet.Cell(index + 1, 6).Value = listdata[index - 1].Pincode;
            //            worksheet.Cell(index + 1, 7).Value = listdata[index - 1].Mobile;
            //            worksheet.Cell(index + 1, 8).Value = listdata[index - 1].Region;
            //            worksheet.Cell(index + 1, 9).Value = listdata[index - 1].Email;
            //            worksheet.Cell(index + 1, 10).Value = listdata[index - 1].Instance;

            //            worksheet.Cell(index + 1, 1).Style.Font.FontColor = XLColor.Black;
            //            worksheet.Cell(index + 1, 2).Style.Font.FontColor = XLColor.Black;
            //            worksheet.Cell(index + 1, 3).Style.Font.FontColor = XLColor.Black;
            //            worksheet.Cell(index + 1, 4).Style.Font.FontColor = XLColor.Black;
            //            worksheet.Cell(index + 1, 5).Style.Font.FontColor = XLColor.Black;
            //            worksheet.Cell(index + 1, 6).Style.Font.FontColor = XLColor.Black;
            //            worksheet.Cell(index + 1, 7).Style.Font.FontColor = XLColor.Black;
            //            worksheet.Cell(index + 1, 8).Style.Font.FontColor = XLColor.Black;
            //            worksheet.Cell(index + 1, 9).Style.Font.FontColor = XLColor.Black;
            //            worksheet.Cell(index + 1, 10).Style.Font.FontColor = XLColor.Black;
            //        }
            //        using (var stream = new MemoryStream())
            //        {
            //            workbook.SaveAs(stream);
            //            var content = stream.ToArray();
            //            return File(content, contentType, fileName);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return Json("Failed", System.Web.Mvc.JsonRequestBehavior.AllowGet);
            //}
            var DashboardDetails = new TDashboardDetails();// ApiControl.Get<List<TDashboardDetails>, string, string, string>(Enviornment, SearchBy, trackingNo, "Enviornment", "SearchBy", "trackingNo", "api/Calling/GetTrackingLink");
            return Json(DashboardDetails);
        }

    }
}
