using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.Entities;

namespace UniWare_PandoIntegration.Controllers
{
    public class UploadExcel : Controller
    {
        ApiOperation ApiControl;
        private readonly IConfiguration iconfiguration;
        private readonly string Apibase;
        private static List<FacilityMaintain> ListcustomerModels;

        public UploadExcel(IConfiguration configuration)
        {
            //this.iconfiguration = iconfiguration;
            Apibase = configuration.GetSection("baseaddress:Url").Value;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UploadExcelData(IFormFile upload)
        {
            if (upload != null)
            {
                try
                {


                    Stream stream = upload.OpenReadStream();
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    IExcelDataReader reader = null;
                    if (upload.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateReader(stream);
                    }
                    else if (upload.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateReader(stream);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        return View("~/Views/UploadExcel/Uploads.cshtml");
                    }

                    DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true,
                            FilterRow = rowReader =>
                            {
                                var hasData = false;
                                for (var i = 0; i < rowReader.FieldCount; i++)
                                {
                                    if (rowReader[i] == null || string.IsNullOrEmpty(rowReader[i].ToString()))
                                    {
                                        continue;
                                    }
                                    hasData = true;
                                    break;
                                }
                                return hasData;
                            },
                        }
                    });
                    reader.Close();
                    DataTable cloned = result.Tables[0].Clone();
                    for (var i = 0; i < cloned.Columns.Count; i++)
                    {
                        cloned.Columns[i].DataType = typeof(string);
                    }
                    foreach (DataRow row in result.Tables[0].Rows)
                    {
                        cloned.ImportRow(row);
                    }

                    List<UploadExcels> empList = new List<UploadExcels>();
                    //int table = Convert.ToInt32(dataSet.Tables.Count);
                    //for (int i = 0; i < table; i++)
                    //{
                    foreach (DataRow dr in cloned.Rows)
                    {
                        empList.Add(new UploadExcels { Code = Convert.ToString(dr["OrderCode"]).Trim(), Type = Convert.ToString(dr["FilterType"]).Trim(), Instance = Convert.ToString(dr["Instance"]).Trim(), Facility = Convert.ToString(dr["FacilityCode"]).Trim() });
                    }
                    //}
                    string Environment = HttpContext.Session.GetString("Environment").ToString();
                    string userid = HttpContext.Session.GetString("LoginId").ToString();

                    ApiControl = new ApiOperation(Apibase);
                    MainClass mainClass = new MainClass();
                    mainClass.UploadExcels = empList;
                    mainClass.Enviornment = Environment;
                    mainClass.Userid = userid;
                    var response = ApiControl.Post1<ServiceResponse<string>, MainClass>(mainClass, "api/Calling/UploadExcel");
                    TempData["Success"] = response.Remove(0, 1).Remove(response.Length - 2, 1);
                }
                catch (Exception ex)
                {

                    TempData["Success"] = ex.Message.Replace("'"," ");
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            //return PartialView("~/Views/Home/Dashboard.cshtml");
            return RedirectToAction("Dashboard", "Home");
        }
        public ActionResult Uploads()
        {
            return View();
        }
        public ActionResult DispatchUploadTemplate()
        {
            try
            {
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "Dispatch Upload.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Code");
                    worksheet.Cell(1, 1).Value = "OrderCode";
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 1).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 2).Value = "FilterType";
                    worksheet.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 2).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 2).Style.Font.Bold = true;
                    worksheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 3).Value = "Instance";
                    worksheet.Cell(1, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 3).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 3).Style.Font.Bold = true;
                    worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 4).Value = "FacilityCode";
                    worksheet.Cell(1, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 4).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 4).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    worksheet.Cell(1, 4).Style.Font.Bold = true;
                    worksheet.ShowGridLines = true;
                   
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed", System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UploadMaster()
        {
            return View("~/Views/UploadExcel/UploadMaster.cshtml");
        }
        public IActionResult UploadMasterData(IFormFile Upload)
        {
            if (Upload != null)
            {
                Stream stream = Upload.OpenReadStream();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                IExcelDataReader reader = null;
                if (Upload.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else if (Upload.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else
                {
                    ModelState.AddModelError("File", "This file format is not supported");
                    return View("~/Views/UploadExcel/UploadMaster.cshtml");
                }
                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true,
                        FilterRow = rowReader =>
                        {
                            var hasData = false;
                            for (var i = 0; i < rowReader.FieldCount; i++)
                            {
                                if (rowReader[i] == null || string.IsNullOrEmpty(rowReader[i].ToString()))
                                {
                                    continue;
                                }
                                hasData = true;
                                break;
                            }
                            return hasData;
                        },
                    }
                });
                reader.Close();
                DataTable cloned = result.Tables[0].Clone();
                for (var i = 0; i < cloned.Columns.Count; i++)
                {
                    cloned.Columns[i].DataType = typeof(string);
                }
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    cloned.ImportRow(row);
                }

                //DataTable Facility = new DataTable();

                //Facility.Columns.Add("FacilityCode");
                //Facility.Columns.Add("FacilityName");
                //Facility.Columns.Add("Address");
                //Facility.Columns.Add("City");
                //Facility.Columns.Add("State");
                //Facility.Columns.Add("Pincode");
                //Facility.Columns.Add("Region");
                //Facility.Columns.Add("Mobile_number");
                //Facility.Columns.Add("email");
                //Facility.Columns.Add("Instance");
                //for (var i = 0; i < cloned.Rows.Count; i++)
                //{
                //    DataRow SOrow = Facility.NewRow();
                //    SOrow["FacilityCode"] = cloned.Rows[i]["Facility Code"];
                //    SOrow["FacilityName"] = cloned.Rows[i]["Facility Name"];
                //    SOrow["Address"] = cloned.Rows[i]["Address"];
                //    SOrow["City"] = cloned.Rows[i]["City"];
                //    SOrow["State"] = cloned.Rows[i]["State"];
                //    SOrow["Pincode"] = cloned.Rows[i]["Pin Code"];
                //    SOrow["Region"] = cloned.Rows[i]["Region"];
                //    SOrow["Mobile_number"] = cloned.Rows[i]["Mobile"];
                //    SOrow["email"] = cloned.Rows[i]["Email"];
                //    SOrow["Instance"] = cloned.Rows[i]["Instance"];
                //    Facility.Rows.Add(SOrow);
                //}
                //DataTable dataSet = new DataTable();
                //dataSet = Facility;
                List<FacilityMaintain> FacList = new List<FacilityMaintain>();
                foreach (DataRow dr in cloned.Rows)
                {
                    FacList.Add(new FacilityMaintain
                    {
                        FacilityCode = Convert.ToString(dr["Facility Code"]),
                        FacilityName = Convert.ToString(dr["Facility Name"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Pincode = Convert.ToString(dr["Pin Code"]),
                        Region = Convert.ToString(dr["Region"]),
                        Mobile = Convert.ToString(dr["Mobile"]),
                        Email = Convert.ToString(dr["email"]),
                        Instance = Convert.ToString(dr["Instance"])
                    });
                }
                ApiControl = new ApiOperation(Apibase);
                FacilityList facilitylist = new FacilityList();
                facilitylist.Listoffacility = FacList;
                facilitylist.Enviornment = HttpContext.Session.GetString("Environment").ToString();
                facilitylist.UserId = HttpContext.Session.GetString("LoginId").ToString();


                var response = ApiControl.Post1<ServiceResponse<string>, FacilityList>(facilitylist, "Api/Calling/FacilityMasterUploads").Trim();

                TempData["Success"] = response.Remove(0, 1).Remove(response.Length - 2, 1);

            }
            //return View("~/Views/Home/Dashboard.cshtml");
            return RedirectToAction("Dashboard", "Home");

        }
        public ActionResult FacilityMasterDownload()
        {

            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            ListcustomerModels = ApiControl.Get<List<FacilityMaintain>, string>(Enviornment, "Enviornment", "api/Calling/GetFacilityMaster_Details");


            //ListcustomerModels = ApiControl.Get<List<FacilityMaintain>>("api/Calling/GetFacilityMaster_Details");

            var listdata = ListcustomerModels;
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Facility Master.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("FacilityDetails");
                    worksheet.Cell(1, 1).Value = "Facility Code";
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 1).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 2).Value = "Facility Name";
                    worksheet.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 2).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 2).Style.Font.Bold = true;
                    worksheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 3).Value = "Address";
                    worksheet.Cell(1, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 3).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 3).Style.Font.Bold = true;
                    worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 4).Value = "City";
                    worksheet.Cell(1, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 4).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 4).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    worksheet.Cell(1, 4).Style.Font.Bold = true;

                    worksheet.Cell(1, 5).Value = "State";
                    worksheet.Cell(1, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 5).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 5).Style.Font.Bold = true;
                    worksheet.Cell(1, 5).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 6).Value = "Pin Code";
                    worksheet.Cell(1, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 6).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 6).Style.Font.Bold = true;
                    worksheet.Cell(1, 6).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 7).Value = "Mobile";
                    worksheet.Cell(1, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 7).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 7).Style.Font.Bold = true;
                    worksheet.Cell(1, 7).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 8).Value = "Region";
                    worksheet.Cell(1, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 8).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 8).Style.Font.Bold = true;
                    worksheet.Cell(1, 8).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 9).Value = "Email";
                    worksheet.Cell(1, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 9).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 9).Style.Font.Bold = true;
                    worksheet.Cell(1, 9).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 10).Value = "Instance";
                    worksheet.Cell(1, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 10).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 10).Style.Font.Bold = true;
                    worksheet.Cell(1, 10).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.ShowGridLines = true;
                    for (int index = 1; index <= listdata.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = listdata[index - 1].FacilityCode;
                        worksheet.Cell(index + 1, 2).Value = listdata[index - 1].FacilityName;
                        worksheet.Cell(index + 1, 3).Value = listdata[index - 1].Address;
                        worksheet.Cell(index + 1, 4).Value = listdata[index - 1].City;
                        worksheet.Cell(index + 1, 5).Value = listdata[index - 1].State;
                        worksheet.Cell(index + 1, 6).Value = listdata[index - 1].Pincode;
                        worksheet.Cell(index + 1, 7).Value = listdata[index - 1].Mobile;
                        worksheet.Cell(index + 1, 8).Value = listdata[index - 1].Region;
                        worksheet.Cell(index + 1, 9).Value = listdata[index - 1].Email;
                        worksheet.Cell(index + 1, 10).Value = listdata[index - 1].Instance;

                        worksheet.Cell(index + 1, 1).Style.Font.FontColor = XLColor.Black;
                        worksheet.Cell(index + 1, 2).Style.Font.FontColor = XLColor.Black;
                        worksheet.Cell(index + 1, 3).Style.Font.FontColor = XLColor.Black;
                        worksheet.Cell(index + 1, 4).Style.Font.FontColor = XLColor.Black;
                        worksheet.Cell(index + 1, 5).Style.Font.FontColor = XLColor.Black;
                        worksheet.Cell(index + 1, 6).Style.Font.FontColor = XLColor.Black;
                        worksheet.Cell(index + 1, 7).Style.Font.FontColor = XLColor.Black;
                        worksheet.Cell(index + 1, 8).Style.Font.FontColor = XLColor.Black;
                        worksheet.Cell(index + 1, 9).Style.Font.FontColor = XLColor.Black;
                        worksheet.Cell(index + 1, 10).Style.Font.FontColor = XLColor.Black;
                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed", System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UploadTruckDetails()
        {
            return View("~/Views/UploadExcel/TruckDetailsUpload.cshtml");
        }
        public ActionResult TruckDetailsMasterDownload()
        {

            ApiControl = new ApiOperation(Apibase);
            List<TruckDetails> truckDetails = new List<TruckDetails>();


            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            var listdata = ApiControl.Get<List<TruckDetails>, string>(Enviornment, "Enviornment", "api/Calling/GetTruckMaster_Details");
            //var listdata = ApiControl.Get<List<TruckDetails>>("api/Calling/GetTruckMaster_Details");

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Truck Detals Master.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("TruckDetails");
                    worksheet.Cell(1, 1).Value = "Truck Details";
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 1).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 2).Value = "Instance";
                    worksheet.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 2).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 2).Style.Font.Bold = true;
                    worksheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.ShowGridLines = true;
                    for (int index = 1; index <= listdata.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = listdata[index - 1].Details;
                        worksheet.Cell(index + 1, 2).Value = listdata[index - 1].Instance;


                        worksheet.Cell(index + 1, 1).Style.Font.FontColor = XLColor.Black;
                        worksheet.Cell(index + 1, 2).Style.Font.FontColor = XLColor.Black;

                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed", System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }
        public IActionResult UploadTruckMaster(IFormFile Upload)
        {
            if (Upload != null)
            {
                Stream stream = Upload.OpenReadStream();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                IExcelDataReader reader = null;
                if (Upload.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else if (Upload.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else
                {
                    ModelState.AddModelError("File", "This file format is not supported");
                    return View("~/Views/UploadExcel/UploadTruckDetails.cshtml");
                }
                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true,
                        FilterRow = rowReader =>
                        {
                            var hasData = false;
                            for (var i = 0; i < rowReader.FieldCount; i++)
                            {
                                if (rowReader[i] == null || string.IsNullOrEmpty(rowReader[i].ToString()))
                                {
                                    continue;
                                }
                                hasData = true;
                                break;
                            }
                            return hasData;
                        },
                    }
                });
                reader.Close();
                DataTable cloned = result.Tables[0].Clone();
                for (var i = 0; i < cloned.Columns.Count; i++)
                {
                    cloned.Columns[i].DataType = typeof(string);
                }
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    cloned.ImportRow(row);
                }

                //DataTable truckdetails = new DataTable();

                //truckdetails.Columns.Add("Details");
                //truckdetails.Columns.Add("Instance");

                //for (var i = 0; i < cloned.Rows.Count; i++)
                //{
                //    DataRow SOrow = truckdetails.NewRow();
                //    SOrow["Details"] = cloned.Rows[i]["Truck Details"];
                //    SOrow["Instance"] = cloned.Rows[i]["Instance"];
                //    truckdetails.Rows.Add(SOrow);
                //}
                //DataTable dataSet = new DataTable();
                //dataSet = truckdetails;
                List<TruckDetails> FacList = new List<TruckDetails>();
                foreach (DataRow dr in cloned.Rows)
                {
                    FacList.Add(new TruckDetails
                    {
                        Details = Convert.ToString(dr["Truck Details"]),
                        Instance = Convert.ToString(dr["Instance"])
                    });
                }
                ApiControl = new ApiOperation(Apibase);

                TruckdetailsMap mainClass = new TruckdetailsMap();
                mainClass.Enviornment = HttpContext.Session.GetString("Environment").ToString();
                mainClass.TruckDetails = FacList;
                mainClass.Userid = HttpContext.Session.GetString("LoginId").ToString();



                var response = ApiControl.Post1<ServiceResponse<string>, TruckdetailsMap>(mainClass, "Api/Calling/TruckDetailsUpdate").Trim();
                TempData["Success"] = response.Remove(0, 1).Remove(response.Length - 2, 1);
            }
            //return View("~/Views/Home/Dashboard.cshtml");
            return RedirectToAction("Dashboard", "Home");

        }
        public ActionResult STOUpload()
        {
            return View("~/Views/UploadExcel/ExcelUploadForSTO.cshtml");
        }
        public IActionResult STOUploadExcel(IFormFile Upload)
        {
            if (Upload != null)
            {
                Stream stream = Upload.OpenReadStream();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                IExcelDataReader reader = null;
                if (Upload.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else if (Upload.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else
                {
                    ModelState.AddModelError("File", "This file format is not supported");
                    return View("~/Views/UploadExcel/STOUpload.cshtml");
                }
                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true,
                        FilterRow = rowReader =>
                        {
                            var hasData = false;
                            for (var i = 0; i < rowReader.FieldCount; i++)
                            {
                                if (rowReader[i] == null || string.IsNullOrEmpty(rowReader[i].ToString()))
                                {
                                    continue;
                                }
                                hasData = true;
                                break;
                            }
                            return hasData;
                        },
                    }
                });
                reader.Close();
                DataTable cloned = result.Tables[0].Clone();
                for (var i = 0; i < cloned.Columns.Count; i++)
                {
                    cloned.Columns[i].DataType = typeof(string);
                }
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    cloned.ImportRow(row);
                }
                //DataTable STO = new DataTable();

                //STO.Columns.Add("Code");
                //STO.Columns.Add("Type");
                //STO.Columns.Add("Instance");

                ////for (var i = 0; i < cloned.Rows.Count; i++)
                ////{
                ////    if (cloned.Rows[i]["FilterType"].Equals("SO"))
                ////    {
                ////        DataRow SOrow = STO.NewRow();
                ////        SOrow["Code"] = cloned.Rows[i]["Code"];
                ////        SOrow["Type"] = cloned.Rows[i]["FilterType"];
                ////        STO.Rows.Add(SOrow);
                ////    } 

                ////}
                //for (var i = 0; i < cloned.Rows.Count; i++)
                //{
                //    DataRow SOrow = STO.NewRow();
                //    SOrow["Code"] = cloned.Rows[i]["Code"];
                //    SOrow["Type"] = cloned.Rows[i]["FilterType"];
                //    SOrow["Instance"] = cloned.Rows[i]["Instance"];
                //    STO.Rows.Add(SOrow);
                //}


                //DataSet dataSet = new DataSet();
                //dataSet.Tables.Add(STO);
                List<UploadExcels> empList = new List<UploadExcels>();
                //int table = Convert.ToInt32(dataSet.Tables.Count);
                //for (int i = 0; i < table; i++)
                //{
                foreach (DataRow dr in cloned.Rows)
                {
                    empList.Add(new UploadExcels { Code = Convert.ToString(dr["Code"]), Type = Convert.ToString(dr["FilterType"]), Instance = Convert.ToString(dr["Instance"]) });
                }
                //}
                ApiControl = new ApiOperation(Apibase);
                MainClass mainClass = new MainClass();
                mainClass.Enviornment = HttpContext.Session.GetString("Environment").ToString();
                mainClass.UploadExcels = empList;
                mainClass.Userid = HttpContext.Session.GetString("LoginId").ToString();

                var response = ApiControl.Post1<ServiceResponse<string>, MainClass>(mainClass, "Api/Calling/STOUpload").Trim();



                TempData["Success"] = response.Remove(0, 1).Remove(response.Length - 2, 1);

            }
            //return View("~/Views/Home/Dashboard.cshtml");
            return RedirectToAction("Dashboard", "Home");

        }
        public ActionResult RegionUpload()
        {
            return View("~/Views/UploadExcel/ReagionMaster.cshtml");
        }
        public IActionResult UploadReagionMaster(IFormFile Upload)
        {
            if (Upload != null)
            {
                Stream stream = Upload.OpenReadStream();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                IExcelDataReader reader = null;
                if (Upload.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else if (Upload.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else
                {
                    ModelState.AddModelError("File", "This file format is not supported");
                    return View("~/Views/UploadExcel/ReagionMaster.cshtml");
                }
                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true,
                        FilterRow = rowReader =>
                        {
                            var hasData = false;
                            for (var i = 0; i < rowReader.FieldCount; i++)
                            {
                                if (rowReader[i] == null || string.IsNullOrEmpty(rowReader[i].ToString()))
                                {
                                    continue;
                                }
                                hasData = true;
                                break;
                            }
                            return hasData;
                        },
                    }
                });
                reader.Close();
                DataTable cloned = result.Tables[0].Clone();
                for (var i = 0; i < cloned.Columns.Count; i++)
                {
                    cloned.Columns[i].DataType = typeof(string);
                }
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    cloned.ImportRow(row);
                }

                //DataTable truckdetails = new DataTable();

                //truckdetails.Columns.Add("State");
                //truckdetails.Columns.Add("Region");

                //for (var i = 0; i < cloned.Rows.Count; i++)
                //{
                //    DataRow SOrow = truckdetails.NewRow();
                //    SOrow["State"] = cloned.Rows[i]["State"];
                //    SOrow["Region"] = cloned.Rows[i]["Region"];
                //    truckdetails.Rows.Add(SOrow);
                //}
                //DataTable dataSet = new DataTable();
                //dataSet = truckdetails;
                List<RegionMaster> FacList = new List<RegionMaster>();
                foreach (DataRow dr in cloned.Rows)
                {
                    FacList.Add(new RegionMaster
                    {
                        State = Convert.ToString(dr["State"]),
                        Region = Convert.ToString(dr["Region"])
                    });
                }
                ApiControl = new ApiOperation(Apibase);
                RegionMasterMap regionMasterMap = new RegionMasterMap();
                regionMasterMap.RegionMasters = FacList;
                regionMasterMap.Enviornment = HttpContext.Session.GetString("Environment").ToString();
                regionMasterMap.Userid = HttpContext.Session.GetString("LoginId").ToString();


                var response = ApiControl.Post1<ServiceResponse<string>, RegionMasterMap>(regionMasterMap, "Api/Calling/RegionMasterUpdate").Trim();
                TempData["Success"] = response.Remove(0, 1).Remove(response.Length - 2, 1);
            }
            //return View("~/Views/Home/Dashboard.cshtml");
            return RedirectToAction("Dashboard", "Home");

        }
        public ActionResult RegionDetailsMasterDownload()
        {

            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            var listdata = ApiControl.Get<List<RegionMaster>, string>(Enviornment, "Enviornment", "api/Calling/GetReagonMaster_Details");
            //var listdata = ApiControl.Get<List<RegionMaster>>("api/Calling/GetReagonMaster_Details");

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "RegionMaster.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("ReagionDetails");
                    worksheet.Cell(1, 1).Value = "State";
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 1).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 2).Value = "Region";
                    worksheet.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 2).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 2).Style.Font.Bold = true;
                    worksheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.ShowGridLines = true;
                    for (int index = 1; index <= listdata.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = listdata[index - 1].State;
                        worksheet.Cell(index + 1, 2).Value = listdata[index - 1].Region;


                        worksheet.Cell(index + 1, 1).Style.Font.FontColor = XLColor.Black;
                        worksheet.Cell(index + 1, 2).Style.Font.FontColor = XLColor.Black;

                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed", System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult TrackingStatusUpload()
        {
            return View("~/Views/UploadExcel/TrackingStatusUpload.cshtml");
        }
        public ActionResult TrackingMasterDownload()
        {

            ApiControl = new ApiOperation(Apibase);

            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            var listdata = ApiControl.Get<List<TrackingMaster>, string>(Enviornment, "Enviornment", "api/Calling/GetTrackingMasterDetails");
            //var listdata = ApiControl.Get<List<TrackingMaster>>("api/Calling/GetTrackingMasterDetails");

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "TrackingStatusMaster.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("TrackingMaster");
                    worksheet.Cell(1, 1).Value = "Pando Status";
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 1).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 2).Value = "Uniware Status";
                    worksheet.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 2).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 2).Style.Font.Bold = true;
                    worksheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;


                    worksheet.Cell(1, 3).Value = "Courier Name";
                    worksheet.Cell(1, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 3).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 3).Style.Font.Bold = true;
                    worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.ShowGridLines = true;
                    for (int index = 1; index <= listdata.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = listdata[index - 1].PandoStatus;
                        worksheet.Cell(index + 1, 2).Value = listdata[index - 1].UniwareStatus;
                        worksheet.Cell(index + 1, 3).Value = listdata[index - 1].CourierName;


                        worksheet.Cell(index + 1, 1).Style.Font.FontColor = XLColor.Black;
                        worksheet.Cell(index + 1, 2).Style.Font.FontColor = XLColor.Black;
                        worksheet.Cell(index + 1, 3).Style.Font.FontColor = XLColor.Black;

                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed", System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }
        public IActionResult UploadTrackingStatusMaster(IFormFile Upload)
        {
            if (Upload != null)
            {
                Stream stream = Upload.OpenReadStream();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                IExcelDataReader reader = null;
                if (Upload.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else if (Upload.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else
                {
                    ModelState.AddModelError("File", "This file format is not supported");
                    return View("~/Views/UploadExcel/TrackingStatusUpload.cshtml");
                }
                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true,
                        FilterRow = rowReader =>
                        {
                            var hasData = false;
                            for (var i = 0; i < rowReader.FieldCount; i++)
                            {
                                if (rowReader[i] == null || string.IsNullOrEmpty(rowReader[i].ToString()))
                                {
                                    continue;
                                }
                                hasData = true;
                                break;
                            }
                            return hasData;
                        },
                    }
                });
                reader.Close();
                DataTable cloned = result.Tables[0].Clone();
                for (var i = 0; i < cloned.Columns.Count; i++)
                {
                    cloned.Columns[i].DataType = typeof(string);
                }
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    cloned.ImportRow(row);
                }

                //DataTable truckdetails = new DataTable();
                //truckdetails.Columns.Add("PandoStatus");
                //truckdetails.Columns.Add("UniwareStatus");
                //truckdetails.Columns.Add("CourierName");

                //for (var i = 0; i < cloned.Rows.Count; i++)
                //{
                //    DataRow SOrow = truckdetails.NewRow();
                //    SOrow["PandoStatus"] = cloned.Rows[i]["Pando Status"];
                //    SOrow["UniwareStatus"] = cloned.Rows[i]["Uniware Status"];
                //    SOrow["CourierName"] = cloned.Rows[i]["Courier Name"];
                //    truckdetails.Rows.Add(SOrow);
                //}
                //DataTable dataSet = new DataTable();
                //dataSet = truckdetails;
                List<TrackingMaster> FacList = new List<TrackingMaster>();
                foreach (DataRow dr in cloned.Rows)
                {
                    FacList.Add(new TrackingMaster
                    {
                        PandoStatus = Convert.ToString(dr["Pando Status"]),
                        UniwareStatus = Convert.ToString(dr["Uniware Status"]),
                        CourierName = Convert.ToString(dr["Courier Name"])
                    });
                }
                TrackingMasterMapping trackingMasterMapping = new TrackingMasterMapping();
                trackingMasterMapping.TrackingMasters = FacList;
                trackingMasterMapping.Enviornment = HttpContext.Session.GetString("Environment").ToString();
                trackingMasterMapping.Userid = HttpContext.Session.GetString("LoginId").ToString();

                ApiControl = new ApiOperation(Apibase);
                var response = ApiControl.Post1<ServiceResponse<string>, TrackingMasterMapping>(trackingMasterMapping, "Api/Calling/TrackingStatusMasterUpload").Trim();
                TempData["Success"] = response.Remove(0, 1).Remove(response.Length - 2, 1);
            }
            //return View("~/Views/Home/Dashboard.cshtml");
            return RedirectToAction("Dashboard", "Home");

        }
        public ActionResult CourierNameUpload()
        {
            return View();
        }
        public ActionResult CourierListDownload()
        {

            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            var listdata = ApiControl.Get<List<TrackingMaster>, string>(Enviornment, "Enviornment", "api/Calling/GetCourierNameDetails");

            //var listdata = ApiControl.Get<List<TrackingMaster>>("api/Calling/GetCourierNameDetails");

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Courier List.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Courier List");


                    worksheet.Cell(1, 1).Value = "Courier Name";
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 1).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.ShowGridLines = true;
                    for (int index = 1; index <= listdata.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = listdata[index - 1].CourierName;


                        worksheet.Cell(index + 1, 1).Style.Font.FontColor = XLColor.Black;

                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed", System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }
        public IActionResult CourierListUpload(IFormFile Upload)
        {
            if (Upload != null)
            {
                Stream stream = Upload.OpenReadStream();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                IExcelDataReader reader = null;
                if (Upload.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else if (Upload.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else
                {
                    ModelState.AddModelError("File", "This file format is not supported");
                    return View("~/Views/UploadExcel/CourierNameUpload.cshtml");
                }
                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true,
                        FilterRow = rowReader =>
                        {
                            var hasData = false;
                            for (var i = 0; i < rowReader.FieldCount; i++)
                            {
                                if (rowReader[i] == null || string.IsNullOrEmpty(rowReader[i].ToString()))
                                {
                                    continue;
                                }
                                hasData = true;
                                break;
                            }
                            return hasData;
                        },
                    }
                });
                reader.Close();
                DataTable cloned = result.Tables[0].Clone();
                for (var i = 0; i < cloned.Columns.Count; i++)
                {
                    cloned.Columns[i].DataType = typeof(string);
                }
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    cloned.ImportRow(row);
                }
                //truckdetails.Columns.Add("CourierName");

                //for (var i = 0; i < cloned.Rows.Count; i++)
                //{
                //    DataRow SOrow = truckdetails.NewRow();
                //    SOrow["CourierName"] = cloned.Rows[i]["Courier Name"];
                //    truckdetails.Rows.Add(SOrow);
                //}
                //DataTable dataSet = new DataTable();
                //dataSet = truckdetails;
                List<TrackingMaster> FacList = new List<TrackingMaster>();
                foreach (DataRow dr in cloned.Rows)
                {
                    FacList.Add(new TrackingMaster
                    {
                        CourierName = Convert.ToString(dr["Courier Name"])
                    });
                }
                TrackingMasterMapping trackingMasterMapping = new TrackingMasterMapping();
                trackingMasterMapping.TrackingMasters = FacList;
                trackingMasterMapping.Enviornment = HttpContext.Session.GetString("Environment").ToString();
                trackingMasterMapping.Userid = HttpContext.Session.GetString("LoginId").ToString();

                ApiControl = new ApiOperation(Apibase);
                var response = ApiControl.Post1<ServiceResponse<string>, TrackingMasterMapping>(trackingMasterMapping, "Api/Calling/CourierListUpload").Trim();
                TempData["Success"] = response.Remove(0, 1).Remove(response.Length - 2, 1);
            }
            //return View("~/Views/Home/Dashboard.cshtml");
            return RedirectToAction("Dashboard", "Home");

        }
        public ActionResult TrackingLinkUpload()
        {
            return View();
        }
        public ActionResult TrackingLinkDownload()
        {

            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            var listdata = ApiControl.Get<List<TrackingLinkMapping>, string>(Enviornment, "Enviornment", "api/Calling/GetTrackingLinkList");

            //var listdata = ApiControl.Get<List<TrackingLinkMapping>>("api/Calling/GetTrackingLinkList");

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Tracking Mapping List.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Tracking Mapping List");


                    worksheet.Cell(1, 1).Value = "Courier Name";
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 1).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 2).Value = "Tracking Link";
                    worksheet.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 2).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 2).Style.Font.Bold = true;
                    worksheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.ShowGridLines = true;
                    for (int index = 1; index <= listdata.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = listdata[index - 1].CourierName;
                        worksheet.Cell(index + 1, 2).Value = listdata[index - 1].TrackingLink;


                        worksheet.Cell(index + 1, 1).Style.Font.FontColor = XLColor.Black;
                        worksheet.Cell(index + 1, 2).Style.Font.FontColor = XLColor.Black;

                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed", System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }
        public IActionResult TrackingMappingUpload(IFormFile Upload)
        {
            if (Upload != null)
            {
                Stream stream = Upload.OpenReadStream();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                IExcelDataReader reader = null;
                if (Upload.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else if (Upload.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else
                {
                    ModelState.AddModelError("File", "This file format is not supported");
                    return View("~/Views/UploadExcel/TrackingLinkUpload.cshtml");
                }
                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true,
                        FilterRow = rowReader =>
                        {
                            var hasData = false;
                            for (var i = 0; i < rowReader.FieldCount; i++)
                            {
                                if (rowReader[i] == null || string.IsNullOrEmpty(rowReader[i].ToString()))
                                {
                                    continue;
                                }
                                hasData = true;
                                break;
                            }
                            return hasData;
                        },
                    }
                });
                reader.Close();
                DataTable cloned = result.Tables[0].Clone();
                for (var i = 0; i < cloned.Columns.Count; i++)
                {
                    cloned.Columns[i].DataType = typeof(string);
                }
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    cloned.ImportRow(row);
                }
                List<TrackingLinkMapping> trackingLinkMappings = new List<TrackingLinkMapping>();

                foreach (DataRow dr in cloned.Rows)
                {
                    trackingLinkMappings.Add(new TrackingLinkMapping
                    {
                        CourierName = Convert.ToString(dr["Courier Name"]),
                        TrackingLink = Convert.ToString(dr["Tracking Link"])
                    });
                }
                //string Environment = HttpContext.Session.GetString("Environment").ToString();
                TrackingLinkMappingMap trackingLinkMappingMap = new TrackingLinkMappingMap();
                trackingLinkMappingMap.TrackingLinkMappings = trackingLinkMappings;
                trackingLinkMappingMap.Enviornment = HttpContext.Session.GetString("Environment").ToString();
                trackingLinkMappingMap.Userid = HttpContext.Session.GetString("LoginId").ToString();

                ApiControl = new ApiOperation(Apibase);
                var response = ApiControl.Post1<ServiceResponse<string>, TrackingLinkMappingMap>(trackingLinkMappingMap, "Api/Calling/BulkUploadtrackingMapping").Trim();
                TempData["Success"] = response.Remove(0, 1).Remove(response.Length - 2, 1);
            }
            //return View("~/Views/UploadExcel/TrackingLinkUpload.cshtml");
            return RedirectToAction("Dashboard", "Home");

        }
        public ActionResult ShippingStatusMaster()
        {
            return View();
        }
        public ActionResult ShippingStatus()
        {
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            var listdata = ApiControl.Get<List<ShippingStatus>, string>(Enviornment, "Enviornment", "api/Calling/GetShippingStatus");

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Shipping Status Master.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Shipping Status");


                    worksheet.Cell(1, 1).Value = "Status Name";
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 1).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    //worksheet.Cell(1, 2).Value = "Tracking Link";
                    //worksheet.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //worksheet.Cell(1, 2).Style.Font.FontColor = XLColor.Black;
                    //worksheet.Cell(1, 2).Style.Font.Bold = true;
                    //worksheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.ShowGridLines = true;
                    for (int index = 1; index <= listdata.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = listdata[index - 1].StatusName;
                        //worksheet.Cell(index + 1, 2).Value = listdata[index - 1].TrackingLink;


                        worksheet.Cell(index + 1, 1).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 2).Style.Font.FontColor = XLColor.Black;

                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed", System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }
        public IActionResult ShippingStatusMasterUpload(IFormFile Upload)
        {
            if (Upload != null)
            {
                Stream stream = Upload.OpenReadStream();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                IExcelDataReader reader = null;
                if (Upload.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else if (Upload.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else
                {
                    ModelState.AddModelError("File", "This file format is not supported");
                    return View("~/Views/UploadExcel/TrackingLinkUpload.cshtml");
                }
                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true,
                        FilterRow = rowReader =>
                        {
                            var hasData = false;
                            for (var i = 0; i < rowReader.FieldCount; i++)
                            {
                                if (rowReader[i] == null || string.IsNullOrEmpty(rowReader[i].ToString()))
                                {
                                    continue;
                                }
                                hasData = true;
                                break;
                            }
                            return hasData;
                        },
                    }
                });
                reader.Close();
                DataTable cloned = result.Tables[0].Clone();
                for (var i = 0; i < cloned.Columns.Count; i++)
                {
                    cloned.Columns[i].DataType = typeof(string);
                }
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    cloned.ImportRow(row);
                }
                List<ShippingStatus> trackingLinkMappings = new List<ShippingStatus>();

                foreach (DataRow dr in cloned.Rows)
                {
                    trackingLinkMappings.Add(new ShippingStatus
                    {
                        StatusName = Convert.ToString(dr["Status Name"]),
                        //TrackingLink = Convert.ToString(dr["Tracking Link"])
                    });
                }
                //string Environment = HttpContext.Session.GetString("Environment").ToString();
                ShippingStatusList trackingLinkMappingMap = new ShippingStatusList();
                trackingLinkMappingMap.ShippingStatus = trackingLinkMappings;
                trackingLinkMappingMap.Enviornment = HttpContext.Session.GetString("Environment").ToString();
                trackingLinkMappingMap.UserId = HttpContext.Session.GetString("LoginId").ToString();

                ApiControl = new ApiOperation(Apibase);
                var response = ApiControl.Post1<ServiceResponse<string>, ShippingStatusList>(trackingLinkMappingMap, "Api/Calling/UpdateShippingStatus").Trim();
                TempData["Success"] = response.Remove(0, 1).Remove(response.Length - 2, 1);
            }
            //return View("~/Views/UploadExcel/TrackingLinkUpload.cshtml");
            return RedirectToAction("Dashboard", "Home");
        }

        public ActionResult SpecialCharactermaster()
        {
            return View();
        }

        public ActionResult SpecialCharacterData()
        {
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Shipping Status Master.xlsx";
            try
            {
                var listdata = ApiControl.Get<string, string>(Enviornment, "Enviornment", "api/Calling/GetSpecialCharacters");

                byte[] bytes = Encoding.UTF8.GetBytes(listdata);
                return File(bytes, "text/plain", "SpecialCharacter.txt");
            }
            catch (Exception ex)
            {
                return Json("Failed", System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }
        public IActionResult SpecialCharacterMasterUpload(IFormFile Upload)
        {
            ApiControl = new ApiOperation(Apibase);
            //string text = System.IO.File.ReadAllText(Upload);
            SpecialCharacterEntity specialCharacterEntity = new SpecialCharacterEntity();
            Upload.OpenReadStream().Position = 0;
            specialCharacterEntity.Enviornment = HttpContext.Session.GetString("Environment").ToString();
            specialCharacterEntity.userid = HttpContext.Session.GetString("LoginId").ToString();
            specialCharacterEntity.Contents = new StreamReader(Upload.OpenReadStream()).ReadToEnd();
            var response = ApiControl.Post1<ServiceResponse<string>, SpecialCharacterEntity>(specialCharacterEntity, "api/Calling/UpdateSpecialCharacters");
            TempData["Success"] = response.Remove(0, 1).Remove(response.Length - 2, 1);
            return RedirectToAction("Dashboard", "Home");

        }
        public ActionResult CityMaster()
        {
            return View();
        }
        public ActionResult CityMasterDownload()
        {
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            var listdata = ApiControl.Get<List<CityMasterEntity>, string>(Enviornment, "Enviornment", "api/Calling/GetCityMaster");

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "City Master.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Citys");


                    worksheet.Cell(1, 1).Value = "Reference City Name";
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 1).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 2).Value = "Actual Name";
                    worksheet.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 2).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 2).Style.Font.Bold = true;
                    worksheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.ShowGridLines = true;
                    for (int index = 1; index <= listdata.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = listdata[index - 1].ReferenceName;
                        worksheet.Cell(index + 1, 2).Value = listdata[index - 1].ActualName;
                        worksheet.Cell(index + 1, index).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 2).Style.Font.FontColor = XLColor.Black;

                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed", System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }
        public IActionResult CityMasterUpload(IFormFile Upload)
        {
            if (Upload != null)
            {
                Stream stream = Upload.OpenReadStream();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                IExcelDataReader reader = null;
                if (Upload.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else if (Upload.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else
                {
                    ModelState.AddModelError("File", "This file format is not supported");
                    return View("~/Views/UploadExcel/CityMaster.cshtml");
                }
                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true,
                        FilterRow = rowReader =>
                        {
                            var hasData = false;
                            for (var i = 0; i < rowReader.FieldCount; i++)
                            {
                                if (rowReader[i] == null || string.IsNullOrEmpty(rowReader[i].ToString()))
                                {
                                    continue;
                                }
                                hasData = true;
                                break;
                            }
                            return hasData;
                        },
                    }
                });
                reader.Close();
                DataTable cloned = result.Tables[0].Clone();
                for (var i = 0; i < cloned.Columns.Count; i++)
                {
                    cloned.Columns[i].DataType = typeof(string);
                }
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    cloned.ImportRow(row);
                }
                List<CityMasterEntity> cityList = new List<CityMasterEntity>();

                foreach (DataRow dr in cloned.Rows)
                {
                    cityList.Add(new CityMasterEntity
                    {
                        ReferenceName = Convert.ToString(dr["Reference City Name"]),
                        ActualName = Convert.ToString(dr["Actual Name"]),
                        //TrackingLink = Convert.ToString(dr["Tracking Link"])
                    });
                }
                //string Environment = HttpContext.Session.GetString("Environment").ToString();
                CityMasterEntitymap cityMasterEntitymap = new CityMasterEntitymap();
                cityMasterEntitymap.cityMasterEntities = cityList;
                cityMasterEntitymap.Enviornment = HttpContext.Session.GetString("Environment").ToString();
                cityMasterEntitymap.Userid = HttpContext.Session.GetString("LoginId").ToString();

                ApiControl = new ApiOperation(Apibase);
                var response = ApiControl.Post1<ServiceResponse<string>, CityMasterEntitymap>(cityMasterEntitymap, "Api/Calling/CityMasterUpload").Trim();
                TempData["Success"] = response.Remove(0, 1).Remove(response.Length - 2, 1);
            }
            //return View("~/Views/UploadExcel/TrackingLinkUpload.cshtml");
            return RedirectToAction("Dashboard", "Home");
        }

        public ActionResult DashboardStatusMaster()
        {
            return View();
        }
        public ActionResult DashboardStatusMasterDownload()
        {
            ApiControl = new ApiOperation(Apibase);
            var Enviornment = HttpContext.Session.GetString("Environment").ToString();
            var listdata = ApiControl.Get<List<DashboardStatusMasterEntity>, string>(Enviornment, "Enviornment", "api/Calling/GetDashboardStatus");

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Dashboard Status Master.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Dashboard");


                    worksheet.Cell(1, 1).Value = "Tracking Status";
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 1).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.Cell(1, 2).Value = "Dashboard Status";
                    worksheet.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(1, 2).Style.Font.FontColor = XLColor.Black;
                    worksheet.Cell(1, 2).Style.Font.Bold = true;
                    worksheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    worksheet.ShowGridLines = true;
                    for (int index = 1; index <= listdata.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = listdata[index - 1].TrackingStatus;
                        worksheet.Cell(index + 1, 2).Value = listdata[index - 1].DashboardStatus;
                        worksheet.Cell(index + 1, index).Style.Font.FontColor = XLColor.Black;
                        //worksheet.Cell(index + 1, 2).Style.Font.FontColor = XLColor.Black;

                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Failed", System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }
        public IActionResult DashboardStatusMasterUpload(IFormFile Upload)
        {
            if (Upload != null)
            {
                Stream stream = Upload.OpenReadStream();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                IExcelDataReader reader = null;
                if (Upload.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else if (Upload.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateReader(stream);
                }
                else
                {
                    ModelState.AddModelError("File", "This file format is not supported");
                    return View("~/Views/UploadExcel/DashboardStatusMaster.cshtml");
                }
                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true,
                        FilterRow = rowReader =>
                        {
                            var hasData = false;
                            for (var i = 0; i < rowReader.FieldCount; i++)
                            {
                                if (rowReader[i] == null || string.IsNullOrEmpty(rowReader[i].ToString()))
                                {
                                    continue;
                                }
                                hasData = true;
                                break;
                            }
                            return hasData;
                        },
                    }
                });
                reader.Close();
                DataTable cloned = result.Tables[0].Clone();
                for (var i = 0; i < cloned.Columns.Count; i++)
                {
                    cloned.Columns[i].DataType = typeof(string);
                }
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    cloned.ImportRow(row);
                }
                List<DashboardStatusMasterEntity> dashboard = new List<DashboardStatusMasterEntity>();

                foreach (DataRow dr in cloned.Rows)
                {
                    dashboard.Add(new DashboardStatusMasterEntity
                    {
                        TrackingStatus = Convert.ToString(dr["Tracking Status"]),
                        DashboardStatus = Convert.ToString(dr["Dashboard Status"]),
                        //TrackingLink = Convert.ToString(dr["Tracking Link"])
                    });
                }
                //string Environment = HttpContext.Session.GetString("Environment").ToString();
                DashboardStatusMasterEntityMap dashboardMaster = new DashboardStatusMasterEntityMap();
                dashboardMaster.dashboardStatusMasterEntities = dashboard;
                dashboardMaster.Enviornment = HttpContext.Session.GetString("Environment").ToString();
                dashboardMaster.Userid = HttpContext.Session.GetString("LoginId").ToString();

                ApiControl = new ApiOperation(Apibase);
                var response = ApiControl.Post1<ServiceResponse<string>, DashboardStatusMasterEntityMap>(dashboardMaster, "Api/Calling/DashboardStatusUpload").Trim();
                TempData["Success"] = response.Remove(0, 1).Remove(response.Length - 2, 1);
            }
            //return View("~/Views/UploadExcel/TrackingLinkUpload.cshtml");
            return RedirectToAction("Dashboard", "Home");
        }

    }
}
