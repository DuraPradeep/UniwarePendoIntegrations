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
        //[HttpGet]
        //public ActionResult UploadExcels()
        //{
        //    return PartialView("~/Views/Home/pv_ExcelUploads.cshtml");
        //}

        public IActionResult UploadExcelData(IFormFile upload)
        {
            if (upload != null)
            {
                // ExcelDataReader works with the binary Excel file, so it needs a FileStream
                // to get started. This is how we avoid dependencies on ACE or Interop:
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
                RO.Columns.Add("Code");
                RO.Columns.Add("Type");
                STO.Columns.Add("Code");
                STO.Columns.Add("Type");

                for (var i = 0; i < cloned.Rows.Count; i++)
                {
                    if (cloned.Rows[i]["FilterType"].Equals("SO"))
                    {
                        DataRow SOrow = SO.NewRow();
                        SOrow["Code"] = cloned.Rows[i]["Code"];
                        SOrow["Type"] = cloned.Rows[i]["FilterType"];
                        SO.Rows.Add(SOrow);
                    }
                    else if (cloned.Rows[i]["FilterType"].Equals("RO"))
                    {
                        DataRow ROrow = RO.NewRow();
                        ROrow["Code"] = cloned.Rows[i]["Code"];
                        ROrow["Type"] = cloned.Rows[i]["FilterType"];
                        RO.Rows.Add(ROrow);
                    }
                    else if (cloned.Rows[i]["FilterType"].Equals("STO"))
                    {
                        DataRow STOrow = STO.NewRow();
                        STOrow["Code"] = cloned.Rows[i]["Code"];
                        STOrow["Type"] = cloned.Rows[i]["FilterType"];
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
                        empList.Add(new UploadExcels { Code = Convert.ToString(dr["Code"]), Type = Convert.ToString(dr["Type"]) });
                    }
                }
                ApiControl = new ApiOperation(Apibase);                
                var response = ApiControl.Post1<ServiceResponse<string>, List<UploadExcels>>(empList, "Api/UniwarePando/UploadExcel");

                ViewBag.Message = response;

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
                for (var i = 0; i < cloned.Rows.Count; i++)
                {
                    DataRow SOrow = Facility.NewRow();
                    SOrow["FacilityCode"] = cloned.Rows[i]["FacilityCode"];
                    SOrow["FacilityName"] = cloned.Rows[i]["FacilityName"];
                    SOrow["Address"] = cloned.Rows[i]["Address"];
                    SOrow["City"] = cloned.Rows[i]["City"];
                    SOrow["State"] = cloned.Rows[i]["State"];
                    SOrow["Pincode"] = cloned.Rows[i]["Pincode"];
                    SOrow["Region"] = cloned.Rows[i]["Region"];
                    SOrow["Mobile_number"] = cloned.Rows[i]["Mobile_number"];
                    SOrow["email"] = cloned.Rows[i]["email"];
                    Facility.Rows.Add(SOrow);
                }
                DataTable dataSet = new DataTable();
                dataSet=Facility;
                List<FacilityMaintain> FacList = new List<FacilityMaintain>();
                //int table = Convert.ToInt32(dataSet.);
                //for (int i = 0; i < table; i++)
                //{
                    foreach (DataRow dr in dataSet.Rows)
                    {
                        FacList.Add(new FacilityMaintain { FacilityCode = Convert.ToString(dr["Code"]), FacilityName = Convert.ToString(dr["Type"]) });
                    }
                //}
                ApiControl = new ApiOperation(Apibase);
                var response = ApiControl.Post1<ServiceResponse<string>, List<FacilityMaintain>>(FacList, "Api/UniwarePando/UploadExcel");

                ViewBag.Message = response;

            }
            return View("~/Views/UploadExcel/UploadMaster.cshtml");
        }
        public void FacilityMasterDownload()
        {

            ApiControl = new ApiOperation(Apibase);
            ListcustomerModels = ApiControl.Get<List<FacilityMaintain>>("api/UniwarePando/GetFacilityMaster_Details");
            ListtoDataTableConverter listtoDataTableConverter = new ListtoDataTableConverter();
            var dt = listtoDataTableConverter.ToDataTable(ListcustomerModels);
            string attachment = "attachment; filename=FacilityMaster.xls";
            Response.Clear();
            Response.Headers.Add("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                Response.WriteAsync(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.WriteAsync("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    Response.WriteAsync(tab + dr[i].ToString());
                    tab = "\t";
                }
                Response.WriteAsync("\n");
            }


        }
    }
}
