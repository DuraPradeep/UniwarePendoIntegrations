using ClosedXML.Excel;
using ExcelDataReader;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.CodeAnalysis;
using System.Data;
using System.Drawing.Printing;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
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
                DataTable SO = new DataTable();
                DataTable RO = new DataTable();
                DataTable STO = new DataTable();
                SO.Columns.Add("Code");
                SO.Columns.Add("Type");
                SO.Columns.Add("Instance");
                RO.Columns.Add("Code");
                RO.Columns.Add("Type");
                RO.Columns.Add("Instance");
                STO.Columns.Add("Code");
                STO.Columns.Add("Type");
                STO.Columns.Add("Instance");

                for (var i = 0; i < cloned.Rows.Count; i++)
                {
                    if (cloned.Rows[i]["FilterType"].Equals("SO"))
                    {
                        DataRow SOrow = SO.NewRow();
                        SOrow["Code"] = cloned.Rows[i]["Code"];
                        SOrow["Type"] = cloned.Rows[i]["FilterType"];
                        SOrow["Instance"] = cloned.Rows[i]["Instance"];

                        SO.Rows.Add(SOrow);
                    }
                    else if (cloned.Rows[i]["FilterType"].Equals("RO"))
                    {
                        DataRow ROrow = RO.NewRow();
                        ROrow["Code"] = cloned.Rows[i]["Code"];
                        ROrow["Type"] = cloned.Rows[i]["FilterType"];
                        ROrow["Instance"] = cloned.Rows[i]["Instance"];
                        RO.Rows.Add(ROrow);
                    }
                    else if (cloned.Rows[i]["FilterType"].Equals("STO"))
                    {
                        DataRow STOrow = STO.NewRow();
                        STOrow["Code"] = cloned.Rows[i]["Code"];
                        STOrow["Type"] = cloned.Rows[i]["FilterType"];
                        STOrow["Instance"] = cloned.Rows[i]["Instance"];
                        STO.Rows.Add(STOrow);
                    }
                }
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(STO);
                dataSet.Tables.Add(SO);
                dataSet.Tables.Add(RO);

                //DataSet ds = new DataSet();
                //UploadExcels objEmp = new UploadExcels();
                List<UploadExcels> empList = new List<UploadExcels>();
                int table = Convert.ToInt32(dataSet.Tables.Count);
                for (int i = 0; i < table; i++)
                {
                    foreach (DataRow dr in dataSet.Tables[i].Rows)
                    {
                        empList.Add(new UploadExcels { Code = Convert.ToString(dr["Code"]), Type = Convert.ToString(dr["Type"]), Instance = Convert.ToString(dr["Instance"]) });
                    }
                }
                ApiControl = new ApiOperation(Apibase);
                var response = ApiControl.Post1<ServiceResponse<string>, List<UploadExcels>>(empList, "Api/UniwarePando/UploadExcel");

                ViewBag.Message = response.Remove(0, 1).Remove(response.Length - 2, 1);

            }
            return PartialView("~/Views/Home/Dashboard.cshtml");
        }
        public ActionResult Uploads()
        {
            return View();
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

                DataTable Facility = new DataTable();

                Facility.Columns.Add("FacilityCode");
                Facility.Columns.Add("FacilityName");
                Facility.Columns.Add("Address");
                Facility.Columns.Add("City");
                Facility.Columns.Add("State");
                Facility.Columns.Add("Pincode");
                Facility.Columns.Add("Region");
                Facility.Columns.Add("Mobile_number");
                Facility.Columns.Add("email");
                Facility.Columns.Add("Instance");
                for (var i = 0; i < cloned.Rows.Count; i++)
                {
                    DataRow SOrow = Facility.NewRow();
                    SOrow["FacilityCode"] = cloned.Rows[i]["Facility Code"];
                    SOrow["FacilityName"] = cloned.Rows[i]["Facility Name"];
                    SOrow["Address"] = cloned.Rows[i]["Address"];
                    SOrow["City"] = cloned.Rows[i]["City"];
                    SOrow["State"] = cloned.Rows[i]["State"];
                    SOrow["Pincode"] = cloned.Rows[i]["Pin Code"];
                    SOrow["Region"] = cloned.Rows[i]["Region"];
                    SOrow["Mobile_number"] = cloned.Rows[i]["Mobile"];
                    SOrow["email"] = cloned.Rows[i]["Email"];
                    SOrow["Instance"] = cloned.Rows[i]["Instance"];
                    Facility.Rows.Add(SOrow);
                }
                DataTable dataSet = new DataTable();
                dataSet = Facility;
                List<FacilityMaintain> FacList = new List<FacilityMaintain>();
                foreach (DataRow dr in dataSet.Rows)
                {
                    FacList.Add(new FacilityMaintain
                    {
                        FacilityCode = Convert.ToString(dr["FacilityCode"]),
                        FacilityName = Convert.ToString(dr["FacilityName"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Pincode = Convert.ToString(dr["Pincode"]),
                        Region = Convert.ToString(dr["Region"]),
                        Mobile = Convert.ToString(dr["Mobile_number"]),
                        Email = Convert.ToString(dr["email"]),
                        Instance = Convert.ToString(dr["Instance"])
                    });
                }
                ApiControl = new ApiOperation(Apibase);
                var response = ApiControl.Post1<ServiceResponse<string>, List<FacilityMaintain>>(FacList, "Api/UniwarePando/FacilityMasterUploads").Trim();

                ViewBag.Message = response.Remove(0, 1).Remove(response.Length - 2, 1);

            }
            return View("~/Views/Home/Dashboard.cshtml");
        }
        public ActionResult FacilityMasterDownload()
        {

            ApiControl = new ApiOperation(Apibase);
            ListcustomerModels = ApiControl.Get<List<FacilityMaintain>>("api/UniwarePando/GetFacilityMaster_Details");

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
            var listdata = ApiControl.Get<List<TruckDetails>>("api/UniwarePando/GetTruckMaster_Details");

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

                DataTable truckdetails = new DataTable();

                truckdetails.Columns.Add("Details");
                truckdetails.Columns.Add("Instance");

                for (var i = 0; i < cloned.Rows.Count; i++)
                {
                    DataRow SOrow = truckdetails.NewRow();
                    SOrow["Details"] = cloned.Rows[i]["Truck Details"];
                    SOrow["Instance"] = cloned.Rows[i]["Instance"];
                    truckdetails.Rows.Add(SOrow);
                }
                DataTable dataSet = new DataTable();
                dataSet = truckdetails;
                List<TruckDetails> FacList = new List<TruckDetails>();
                foreach (DataRow dr in dataSet.Rows)
                {
                    FacList.Add(new TruckDetails
                    {
                        Details = Convert.ToString(dr["Details"]),
                        Instance = Convert.ToString(dr["Instance"])
                    });
                }
                ApiControl = new ApiOperation(Apibase);
                var response = ApiControl.Post1<ServiceResponse<string>, List<TruckDetails>>(FacList, "Api/UniwarePando/TruckDetailsUpdate").Trim();
                ViewBag.Message = response.Remove(0, 1).Remove(response.Length - 2, 1);
            }
            return View("~/Views/Home/Dashboard.cshtml");
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
                DataTable STO = new DataTable();

                STO.Columns.Add("Code");
                STO.Columns.Add("Type");
                STO.Columns.Add("Instance");

                //for (var i = 0; i < cloned.Rows.Count; i++)
                //{
                //    if (cloned.Rows[i]["FilterType"].Equals("SO"))
                //    {
                //        DataRow SOrow = STO.NewRow();
                //        SOrow["Code"] = cloned.Rows[i]["Code"];
                //        SOrow["Type"] = cloned.Rows[i]["FilterType"];
                //        STO.Rows.Add(SOrow);
                //    } 

                //}
                for (var i = 0; i < cloned.Rows.Count; i++)
                {
                    DataRow SOrow = STO.NewRow();
                    SOrow["Code"] = cloned.Rows[i]["Code"];
                    SOrow["Type"] = cloned.Rows[i]["FilterType"];
                    SOrow["Instance"] = cloned.Rows[i]["Instance"];
                    STO.Rows.Add(SOrow);
                }


                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(STO);
                List<UploadExcels> empList = new List<UploadExcels>();
                int table = Convert.ToInt32(dataSet.Tables.Count);
                for (int i = 0; i < table; i++)
                {
                    foreach (DataRow dr in dataSet.Tables[i].Rows)
                    {
                        empList.Add(new UploadExcels { Code = Convert.ToString(dr["Code"]), Type = Convert.ToString(dr["Type"]),Instance= Convert.ToString(dr["Instance"]) });
                    }
                }
                ApiControl = new ApiOperation(Apibase);
                var response = ApiControl.Post1<ServiceResponse<string>, List<UploadExcels>>(empList, "Api/UniwarePando/STOUpload").Trim();

                ViewBag.Message = response.Remove(0, 1).Remove(response.Length - 2, 1);

            }
            return View("~/Views/Home/Dashboard.cshtml");
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

                DataTable truckdetails = new DataTable();

                truckdetails.Columns.Add("State");
                truckdetails.Columns.Add("Region");

                for (var i = 0; i < cloned.Rows.Count; i++)
                {
                    DataRow SOrow = truckdetails.NewRow();
                    SOrow["State"] = cloned.Rows[i]["State"];
                    SOrow["Region"] = cloned.Rows[i]["Region"];
                    truckdetails.Rows.Add(SOrow);
                }
                DataTable dataSet = new DataTable();
                dataSet = truckdetails;
                List<RegionMaster> FacList = new List<RegionMaster>();
                foreach (DataRow dr in dataSet.Rows)
                {
                    FacList.Add(new RegionMaster
                    {
                        State = Convert.ToString(dr["State"]),
                        Region = Convert.ToString(dr["Region"])
                    });
                }
                ApiControl = new ApiOperation(Apibase);
                var response = ApiControl.Post1<ServiceResponse<string>, List<RegionMaster>>(FacList, "Api/UniwarePando/RegionMasterUpdate").Trim();
                ViewBag.Message = response.Remove(0, 1).Remove(response.Length - 2, 1);
            }
            return View("~/Views/Home/Dashboard.cshtml");
        }
        public ActionResult RegionDetailsMasterDownload()
        {

            ApiControl = new ApiOperation(Apibase);

            var listdata = ApiControl.Get<List<RegionMaster>>("api/UniwarePando/GetReagonMaster_Details");

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

            var listdata = ApiControl.Get<List<TrackingMaster>>("api/UniwarePando/GetTrackingMasterDetails");

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

                DataTable truckdetails = new DataTable();

                truckdetails.Columns.Add("PandoStatus");
                truckdetails.Columns.Add("UniwareStatus");
                truckdetails.Columns.Add("CourierName");

                for (var i = 0; i < cloned.Rows.Count; i++)
                {
                    DataRow SOrow = truckdetails.NewRow();
                    SOrow["PandoStatus"] = cloned.Rows[i]["Pando Status"];
                    SOrow["UniwareStatus"] = cloned.Rows[i]["Uniware Status"];
                    SOrow["CourierName"] = cloned.Rows[i]["Courier Name"];
                    truckdetails.Rows.Add(SOrow);
                }
                DataTable dataSet = new DataTable();
                dataSet = truckdetails;
                List<TrackingMaster> FacList = new List<TrackingMaster>();
                foreach (DataRow dr in dataSet.Rows)
                {
                    FacList.Add(new TrackingMaster
                    {
                        PandoStatus = Convert.ToString(dr["PandoStatus"]),
                        UniwareStatus = Convert.ToString(dr["UniwareStatus"]),
                        CourierName= Convert.ToString(dr["CourierName"])
                    });
                }
                ApiControl = new ApiOperation(Apibase);
                var response = ApiControl.Post1<ServiceResponse<string>, List<TrackingMaster>>(FacList, "Api/UniwarePando/TrackingStatusMasterUpload").Trim();
                ViewBag.Message = response.Remove(0, 1).Remove(response.Length - 2, 1);
            }
            return View("~/Views/Home/Dashboard.cshtml");
        }
    }
}
