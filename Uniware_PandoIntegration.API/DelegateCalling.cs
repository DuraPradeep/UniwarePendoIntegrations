using Newtonsoft.Json;
using Serilog;
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.BusinessLayer;
using Uniware_PandoIntegration.Entities;

namespace Uniware_PandoIntegration.API
{
    public class DelegateCalling
    {
        BearerToken _Token = new BearerToken();
        private UniwareBL ObjBusinessLayer = new();
        MethodWrapper _MethodWrapper = new MethodWrapper();

        // public delegate void DelegateTrackingStatus(string Servertype, String Instance, List<TrackingStatusDb> trackingStatusDbs);
        public void CallingTrackingStatus(string Servertype, String Instance, List<TrackingStatusDb> trackingStatusDbs)
        {
            CreateLog("Execution start");
            var resu = _Token.GetTokens(Servertype, Instance).Result;
            var accesstoken = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(resu.ObjectParam);
            string token = accesstoken.access_token;
            string responsmessage = string.Empty;
            if (!string.IsNullOrEmpty(token))
            {
                var TrackingList = ObjBusinessLayer.GetTrackingDetails(Servertype, trackingStatusDbs);
                ObjBusinessLayer.InsertTrackingStatusPostdata(TrackingList, Servertype);

                if (TrackingList.Count > 0)
                {
                    for (int i = 0; i < TrackingList.Count; i++)
                    {

                        TrackingStatus trackingStatus = new TrackingStatus();
                        trackingStatus.providerCode = TrackingList[i].providerCode;
                        trackingStatus.trackingStatus = TrackingList[i].trackingStatus;
                        trackingStatus.trackingNumber = TrackingList[i].trackingNumber;
                        trackingStatus.shipmentTrackingStatusName = TrackingList[i].shipmentTrackingStatusName;
                        trackingStatus.statusDate = TrackingList[i].statusDate;
                        //ObjBusinessLayer.InsertTrackingStatusPostdata(trackingStatus, TrackingList[i].facilitycode, Servertype);
                        var res = _MethodWrapper.TrackingStatus(trackingStatus, 0, token, TrackingList[i].facilitycode, Servertype, Instance);
                        CreateLog("Execution end");

                        if (res.IsSuccess)
                        {
                            responsmessage = res.ObjectParam.ToString();
                        }
                        else
                        {
                            responsmessage = res.ObjectParam.ToString();
                        }
                    }
                    TrackingResponse reversePickupResponse = new TrackingResponse();
                    reversePickupResponse.successful = true;
                    reversePickupResponse.message = responsmessage;
                    reversePickupResponse.errors = "";
                    reversePickupResponse.warnings = "";
                    CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Success {JsonConvert.SerializeObject(reversePickupResponse)}");

                    //return new JsonResult(reversePickupResponse);
                }
                else
                {
                    TrackingResponse reversePickupResponse = new TrackingResponse();
                    reversePickupResponse.successful = false;
                    reversePickupResponse.message = responsmessage;
                    reversePickupResponse.errors = "There Is not Data For Tacking";
                    reversePickupResponse.warnings = "";
                    CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Error {JsonConvert.SerializeObject(reversePickupResponse)}");

                    //return new JsonResult(reversePickupResponse);
                }

            }
            else
            {
                TrackingResponse reversePickupResponse = new TrackingResponse();
                reversePickupResponse.successful = false;
                reversePickupResponse.message = "Token Not Generated";
                reversePickupResponse.errors = "";
                reversePickupResponse.warnings = "";
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Error {JsonConvert.SerializeObject(reversePickupResponse)}");

                //return new JsonResult(reversePickupResponse);
            }
        }

        public void CreateLog(string message)
        {
            Log.Information(message);
        }
    }
}
