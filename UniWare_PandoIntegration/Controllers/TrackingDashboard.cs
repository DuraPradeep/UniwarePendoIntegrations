using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public Microsoft.AspNetCore.Mvc.JsonResult GettrackingLink(string SearchBy, string trackingNo)
        {
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            var DashboardDetails = ApiControl.Get<List<TDashboardDetails>, string, string, string>(Enviornment, SearchBy, trackingNo, "Enviornment", "SearchBy", "trackingNo", "api/Calling/GetTrackingLink");
            return Json(DashboardDetails);

        }

        public IActionResult HistoryExceldownload(string FromDate, string ToDate)
        {
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            ApiControl = new ApiOperation(Apibase);
            var DashboardDetails = ApiControl.Get<List<TDashboardDetails>, string, string, string>(Enviornment, FromDate, ToDate, "Enviornment", "FromDate", "ToDate", "api/Calling/DashboardHistoryDataDownload");

            try
            {
                if (DashboardDetails.Count > 0)
                {
                    HttpContext.Session.SetString("DashboardRecords", JsonConvert.SerializeObject(DashboardDetails) );

                    return Json(new { Data = 1 });
                }
                else
                {
                    return Json(new { Data = 0 });
                }
            }
            catch (Exception ex)
            {
                return Json("Failed", System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }

        public IActionResult Downloaddata()
        {
            try
            {
                var DashboardRecords = HttpContext.Session.GetString("DashboardRecords").ToString();
                var DashboardDetails = JsonConvert.DeserializeObject<List<TDashboardDetails>>(DashboardRecords);
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "Dashboard Records.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Dashboard Records");
                    worksheet.Cell(1, 1).Value = "Tracking No.";
                    worksheet.Cell(1, 2).Value = "Display Order Code";
                    worksheet.Cell(1, 3).Value = "Shipment Id";
                    worksheet.Cell(1, 4).Value = "Latest Staus";
                    worksheet.Cell(1, 5).Value = "MileStone";
                    worksheet.Cell(1, 6).Value = "Courier Name";
                    worksheet.Cell(1, 7).Value = "Tracking Link";
                    worksheet.Cell(1, 8).Value = "Customer Name";
                    worksheet.Cell(1, 9).Value = "Customer Phone";
                    worksheet.Cell(1, 10).Value = "Facility Code";
                    worksheet.Cell(1, 11).Value = "Customer City";
                    worksheet.Cell(1, 12).Value = "Invoice Date";
                    worksheet.Cell(1, 13).Value = "Material Code";
                    worksheet.Cell(1, 14).Value = "Quantity";
                    worksheet.Cell(1, 15).Value = "UOM";
                    worksheet.Cell(1, 16).Value = "Indent ID";
                    worksheet.Cell(1, 17).Value = "Pincode";
                    worksheet.Cell(1, 18).Value = "State";
                    worksheet.Cell(1, 19).Value = "Region";
                    worksheet.ShowGridLines = true;

                    for (int index = 1; index <= DashboardDetails.Count; index++)
                    {

                        worksheet.Cell(index + 1, 1).Value = DashboardDetails[index - 1].TrackingNumber;
                        worksheet.Cell(index + 1, 2).Value = DashboardDetails[index - 1].DisplayOrder;
                        worksheet.Cell(index + 1, 3).Value = DashboardDetails[index - 1].ShipmentID;
                        worksheet.Cell(index + 1, 4).Value = DashboardDetails[index - 1].LatestStatus;
                        worksheet.Cell(index + 1, 5).Value = DashboardDetails[index - 1].MileStone;
                        worksheet.Cell(index + 1, 6).Value = DashboardDetails[index - 1].CourierName;
                        worksheet.Cell(index + 1, 7).Value = DashboardDetails[index - 1].trackingLink;
                        worksheet.Cell(index + 1, 8).Value = DashboardDetails[index - 1].CustomerName;
                        worksheet.Cell(index + 1, 9).Value = DashboardDetails[index - 1].CustomerPhone;
                        worksheet.Cell(index + 1, 10).Value = DashboardDetails[index - 1].FacilityCode;
                        worksheet.Cell(index + 1, 11).Value = DashboardDetails[index - 1].CustomerCity;
                        worksheet.Cell(index + 1, 12).Value = DashboardDetails[index - 1].InvoiceDate;
                        worksheet.Cell(index + 1, 13).Value = DashboardDetails[index - 1].MaterialCode;
                        worksheet.Cell(index + 1, 14).Value = DashboardDetails[index - 1].Quantity;
                        worksheet.Cell(index + 1, 15).Value = DashboardDetails[index - 1].UOM;
                        worksheet.Cell(index + 1, 16).Value = DashboardDetails[index - 1].IndentID;
                        worksheet.Cell(index + 1, 17).Value = DashboardDetails[index - 1].Pincode;
                        worksheet.Cell(index + 1, 18).Value = DashboardDetails[index - 1].state;
                        worksheet.Cell(index + 1, 19).Value = DashboardDetails[index - 1].Region;

                        worksheet.Cell(index + 1, index).Style.Font.FontColor = XLColor.Black;

                        worksheet.Cell(1, index).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cell(1, index).Style.Font.FontColor = XLColor.Black;
                        worksheet.Cell(1, index).Style.Font.Bold = true;
                        worksheet.Cell(1, index).Style.Fill.BackgroundColor = XLColor.LightBlue;
                        //worksheet.Cell(index + 1, 2).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 3).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 4).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 5).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 6).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 7).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 8).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 9).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 10).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 11).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 12).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 13).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 14).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 15).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 16).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 17).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 18).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 19).Style.Font.FontColor = XLColor.Black;
                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception)
            {

                return Json("Failed", System.Web.Mvc.JsonRequestBehavior.AllowGet);

            }

        }

    }
}
