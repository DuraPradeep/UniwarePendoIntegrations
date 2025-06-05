using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Uniware_PandoIntegration.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Formatting;
using System.Security.Policy;
using Serilog;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Data;
using System.Configuration;
using System.IO;
using Superpower.Parsers;
using Superpower.Model;
using static Umbraco.Core.Constants.Conventions;

namespace Uniware_PandoIntegration.APIs
{
    public class BearerToken
    {
        //private static readonly ILogger log = LogManager.GetLogger(typeof(SendEmail));
        //  string ServerType = ConfigurationManager.AppSettings["ServerType"];//; AppSettings("ServerType");

        public void CreateLog(string message)
        {
            Log.Information(message);
        }
        public async Task<ServiceResponse<string>> GetTokens(string servertype, string Instance)
        {
            //PandoUniwariToken rootobject;
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                string URL = string.Empty;
                CreateLog(" Token api Started");
                if (servertype.ToLower() == "qa")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        URL = "https://stgsleepyhead2.unicommerce.com/oauth/token?grant_type=password&client_id=my-trusted-client&username=analytics@mysleepyhead.com&password=Unisleepy@123";
                    }
                    else if (Instance.ToLower() == "dfx")
                    {
                        URL = "https://stgmyduroflexworld.unicommerce.com/oauth/token?grant_type=password&client_id=my-trusted-client&username=analytics@mysleepyhead.com&password=Unisleepy@123";
                    }
                }
                else if (servertype.ToLower() == "prod")
                {

                    if (Instance.ToLower() == "sh")
                    {
                        URL = "https://sleepyhead.unicommerce.com/oauth/token?grant_type=password&client_id=my-trusted-client&username=analytics@mysleepyhead.com&password=Unisleepy@123";
                    }
                    else if (Instance.ToLower() == "dfx")
                    {
                        URL = "https://myduroflexworld.unicommerce.co.in/oauth/token?grant_type=password&client_id=my-trusted-client&username=analytics@mysleepyhead.com&password=Unisleepy@123";

                    }
                }
                using (HttpClient _client = new HttpClient())
                {
                    _client.BaseAddress = new Uri(URL);


                    _client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = _client.GetAsync(URL).Result;
                    //var responses = response.Content.ReadAsStringAsync().Result;
                    var responses = response.Content.ReadAsStreamAsync().Result;
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();

                    CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Response:{responses}");
                    if (response.IsSuccessStatusCode)
                    {
                        serviceResponse.Errcode = ((int)response.StatusCode);
                        return serviceResponse;

                    }
                    else
                    {
                        serviceResponse.Errdesc = await response.Content.ReadAsStringAsync();
                        serviceResponse.Errcode = ((int)response.StatusCode);
                        //GetCode(Details, Token);
                    }
                }

            }
            catch (Exception ex)
            {
                CreateLog($" Error: {ex.Message}");
                throw ex;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> GetCode(string Details, string Token, string servertype, string instance)
        {

            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {

                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Api saleOrder Search Started" + Details);
                var client = new HttpClient();
                var request = new HttpRequestMessage();
                if (servertype.ToLower() == "qa")
                {
                    if (instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgsleepyhead2.unicommerce.com/services/rest/v1/oms/saleOrder/search");
                    }
                    else if (instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgmyduroflexworld.unicommerce.com/services/rest/v1/oms/saleOrder/search");

                    }
                }
                else if (servertype.ToLower() == "prod")
                {
                    if (instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://sleepyhead.unicommerce.com/services/rest/v1/oms/saleOrder/search");
                    }
                    else if (instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://myduroflexworld.unicommerce.co.in/services/rest/v1/oms/saleOrder/search");

                    }
                }
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(Details, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                serviceResponse.Errcode = ((int)response.StatusCode);
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync(); ;
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Response:{JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                //return responses;
                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errdesc = await response.Content.ReadAsStringAsync();
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    //GetCode(Details, Token);
                }
            }
            catch (Exception ex)
            {
                CreateLog($" Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> GetCodeDetails(string Code, string Token, string Servertype, string Instance)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();

            try
            {
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Api saleorder get:- " + Code + ": " + Token);
                var client = new HttpClient();
                var request = new HttpRequestMessage();
                if (Servertype.ToLower() == "qa")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgsleepyhead2.unicommerce.com/services/rest/v1/oms/saleorder/get");
                    }
                    else if (Instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgmyduroflexworld.unicommerce.com/services/rest/v1/oms/saleorder/get");
                    }
                }
                else if (Servertype.ToLower() == "prod")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://sleepyhead.unicommerce.com/services/rest/v1/oms/saleorder/get");
                    }
                    else if (Instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://myduroflexworld.unicommerce.co.in/services/rest/v1/oms/saleorder/get");
                    }
                }
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(Code, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
                serviceResponse.ObjectParam = response.Content.ReadAsStringAsync().Result;
                //HttpResponseMessage data = response.Content.ReadAsStringAsync().Result;
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Response: {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                //return responses;
                dynamic data = JsonConvert.DeserializeObject(serviceResponse.ObjectParam);

                if (response.IsSuccessStatusCode)
                {
                    //var ErrorDesc = data.errors[0].description;
                    var datas = (bool)data.successful;

                    //var errors = data.errors;                    
                    if (datas)
                    {
                        serviceResponse.Errcode = ((int)response.StatusCode);
                        serviceResponse.IsSuccess = true;
                        return serviceResponse;
                    }
                    else
                    {
                        serviceResponse.Errcode = 400;
                        serviceResponse.IsSuccess = false;
                        serviceResponse.Errdesc = data.errors[0].description;
                        return serviceResponse;
                    }


                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    serviceResponse.IsSuccess = false;
                    serviceResponse.Errdesc = data.errors[0].description;
                    //GetCodeDetails(Code, Token);
                }
            }
            catch (Exception ex)
            {
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<string>> GetSkuDetails(string SkuCode, string Token, string Servertype, string Instance)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage();
                if (Servertype.ToLower() == "qa")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgsleepyhead2.unicommerce.com/services/rest/v1/catalog/itemType/get");
                    }
                    else if (Instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgmyduroflexworld.unicommerce.com/services/rest/v1/catalog/itemType/get");

                    }
                }
                else if (Servertype.ToLower() == "prod")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://sleepyhead.unicommerce.com/services/rest/v1/catalog/itemType/get");
                    }
                    else if (Instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://myduroflexworld.unicommerce.co.in/services/rest/v1/catalog/itemType/get");

                    }
                }
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(SkuCode, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Response: {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                dynamic data = JsonConvert.DeserializeObject(serviceResponse.ObjectParam);

                if (response.IsSuccessStatusCode)
                {
                    var datas = (bool)data.successful;
                    if (datas)
                    {
                        serviceResponse.Errcode = ((int)response.StatusCode);
                        serviceResponse.IsSuccess = true;
                        return serviceResponse;
                    }
                    else
                    {
                        serviceResponse.Errcode = 400;
                        serviceResponse.IsSuccess = false;
                        serviceResponse.Errdesc = data.errors[0].description;
                        return serviceResponse;
                    }
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    serviceResponse.IsSuccess = false;
                    serviceResponse.Errdesc = data.errors[0].description;
                }
            }
            catch (Exception ex)
            {
                CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<string>> PostDataToDeliverypackList(string jsonre, string ServerType)//List<Data> data
        {
            var responses = "";
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                var credentials = new byte[100];
                var request = new HttpRequestMessage();
                var client = new HttpClient();
                if (ServerType.ToLower() == "qa")
                {
                    string _credentials = "system+demoduro@pando.ai:Pandowelcome@123";
                    credentials = Encoding.ASCII.GetBytes(_credentials);
                    request = new HttpRequestMessage(HttpMethod.Post, "https://duroflex-mitm.gopando.in/inbound/api/transactions/optima/delivery-picklist");

                }
                else if (ServerType.ToLower() == "prod")
                {
                    string _credentials = "system+duroflex@pando.ai:Password@123";
                    credentials = Encoding.ASCII.GetBytes(_credentials);
                    request = new HttpRequestMessage(HttpMethod.Post, "https://duroflex-mitm.pando.in/inbound/api/transactions/optima/delivery-picklist");

                }
                //var credentials = Encoding.ASCII.GetBytes(_credentials);
                //var request = new HttpRequestMessage(HttpMethod.Post, "https://duroflex-mitm.gopando.in/inbound/api/transactions/order/material-invoice");

                //var request = new HttpRequestMessage(HttpMethod.Post, "https://duroflex-mitm.gopando.in/inbound/api/transactions/optima/delivery-picklist");

                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(credentials));
                var content = new StringContent(jsonre, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Response- : {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Error: " + ex.Message);
                throw ex;
            }
            return serviceResponse;

        }

        public async Task<ServiceResponse<string>> PostDataTomaterialinvoice(string data, string ServerType, string Deliveryno)
        {

            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            //var jsonre = JsonConvert.SerializeObject(new { data = data });

            try
            {
                //var data = JsonConvert.SerializeObject(new { data = AllData });
                Log.Information($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Waybill Post Data : {data}");

                var client = new HttpClient();
                var credentials = new byte[100];
                var request = new HttpRequestMessage();
                if (ServerType.ToLower() == "qa")
                {
                    string _credentials = "system+demoduro@pando.ai:Pandowelcome@123";

                    credentials = Encoding.ASCII.GetBytes(_credentials);
                    request = new HttpRequestMessage(HttpMethod.Post, "https://duroflex-mitm.gopando.in/inbound/api/transactions/order/material-invoice");
                }
                else if (ServerType.ToLower() == "prod")
                {
                    string _credentials = "system+duroflex@pando.ai:Password@123";
                    credentials = Encoding.ASCII.GetBytes(_credentials);
                    request = new HttpRequestMessage(HttpMethod.Post, "https://duroflex-mitm.pando.in/inbound/api/transactions/order/material-invoice");

                }
                //var credentials = Encoding.ASCII.GetBytes(_credentials);
                //var request = new HttpRequestMessage(HttpMethod.Post, "https://duroflex-mitm.gopando.in/inbound/api/transactions/order/material-invoice ");

                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(credentials));
                var content = new StringContent(data, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);

                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()},Waybill DeliveryNo:- {Deliveryno}, Response Material-invoice- : {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");

                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    serviceResponse.IsSuccess = true;
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    serviceResponse.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Error:{ex.Message}");
                throw ex;
            }
            return serviceResponse;

        }

        public async Task<ServiceResponse<string>> ReturnOrderGetCode(string Details, string Token, string Servertype, string Instance, string FacilityCode)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Return Order API Search Request" + Details + ": " + Token);
                var client = new HttpClient();
                var request = new HttpRequestMessage();

                if (Servertype.ToLower() == "qa")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgsleepyhead2.unicommerce.com/services/rest/v1/oms/return/search");
                    }
                    else if (Instance.ToLower() == "dfx")
                    {

                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgmyduroflexworld.unicommerce.com/services/rest/v1/oms/return/search");

                    }
                }
                else if (Servertype.ToLower() == "prod")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://sleepyhead.unicommerce.com/services/rest/v1/oms/return/search");
                    }
                    else if (Instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, " https://myduroflexworld.unicommerce.co.in/services/rest/v1/oms/return/search");

                    }
                }
                //if (ServerType.ToLower() == "qa")
                //{
                //    request = new HttpRequestMessage(HttpMethod.Post, "https://stgsleepyhead2.unicommerce.com/services/rest/v1/oms/return/search");
                //}
                //else if (ServerType.ToLower() == "prod")
                //{
                //    request = new HttpRequestMessage(HttpMethod.Post, "https://sleepyhead.unicommerce.com/services/rest/v1/oms/return/search");
                //}

                request.Headers.Add("Facility", FacilityCode);
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(Details, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                serviceResponse.Errcode = ((int)response.StatusCode);
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                CreateLog($"DateTime:-   {DateTime.Now.ToLongTimeString()} , Response:{JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    //GetCode(Details, Token);
                }
            }
            catch (Exception ex)
            {
                CreateLog($" Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> ReturnOrderGet(string Details, string Token, string Servertype, string FacilityCode, string Instance)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Return Order API Get Request" + Details + ": " + Token);
                var client = new HttpClient();
                var request = new HttpRequestMessage();

                if (Servertype.ToLower() == "qa")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgsleepyhead2.unicommerce.com/services/rest/v1/oms/return/get");
                    }
                    else if (Instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgmyduroflexworld.unicommerce.com/services/rest/v1/oms/return/get");

                    }
                }
                else if (Servertype.ToLower() == "prod")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://sleepyhead.unicommerce.com/services/rest/v1/oms/return/get");
                    }
                    else if (Instance.ToLower() == "dfx")
                    {

                        request = new HttpRequestMessage(HttpMethod.Post, "https://myduroflexworld.unicommerce.co.in/services/rest/v1/oms/return/get");

                    }
                }


                //if (ServerType.ToLower() == "qa")
                //{
                //    request = new HttpRequestMessage(HttpMethod.Post, "https://stgsleepyhead2.unicommerce.com/services/rest/v1/oms/return/get");

                //}
                //else if (ServerType.ToLower() == "prod")
                //{
                //    request = new HttpRequestMessage(HttpMethod.Post, "https://sleepyhead.unicommerce.com/services/rest/v1/oms/return/get");

                //}
                request.Headers.Add("Facility", FacilityCode);
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(Details, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                serviceResponse.Errcode = ((int)response.StatusCode);
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()},Return Order Response:{JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);

                }
            }
            catch (Exception ex)
            {
                CreateLog($" Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }
        //public async Task<ServiceResponse<string>> PostDataReturnOrderAPI(string jsonre)
        //{
        //    var responses = "";
        //    //var jsondeserial = JsonSerializer.Deserialize<List<sendRoot>>(data);
        //    ServiceResponse<string> serviceResponse = new ServiceResponse<string>();

        //    //var jsonre = JsonConvert.SerializeObject(new { data = data });
        //    string _credentials = "system+demoduro@pando.ai:Pandowelcome@123";
        //    CreateLog($" Post Data of Return Order API:-  {jsonre}");
        //    //var dejson=JsonConvert.DeserializeObject<Datum>(jsonre);
        //    try
        //    {
        //        var client = new HttpClient();
        //        var credentials = Encoding.ASCII.GetBytes(_credentials);
        //        var request = new HttpRequestMessage(HttpMethod.Post, "https://duroflex-mitm.gopando.in/inbound/api/transactions/optima/delivery-picklist");
        //        //request.Headers.Add("Authorization", "Bearer" + Token);
        //        request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(credentials));
        //        var content = new StringContent(jsonre, null, "application/json");
        //        request.Content = content;
        //        var response = await client.SendAsync(request);
        //        //response.EnsureSuccessStatusCode();
        //        serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
        //        CreateLog($" Response- : {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
        //        //return responses;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            serviceResponse.Errcode = ((int)response.StatusCode);
        //            return serviceResponse;
        //        }
        //        else
        //        {
        //            serviceResponse.Errcode = ((int)response.StatusCode);
        //            //PostDataToDeliverypaclList(data);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CreateLog("$ Error: " + ex.Message);
        //        throw ex;
        //    }
        //    return serviceResponse;

        //}
        //public async Task<ServiceResponse<string>> GetTokensSTO()
        //{
        //    //PandoUniwariToken rootobject;
        //    ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
        //    try
        //    {
        //        CreateLog(" GetToken For STOWaybill");
        //        string URL = "https://stgsleepyhead.unicommerce.com/oauth/token?grant_type=password&client_id=my-trusted-client&username=analytics@mysleepyhead.com&password=Unisleepy@123";

        //        HttpClient _client = new HttpClient()
        //        {
        //            BaseAddress = new Uri(URL)
        //        };
        //        _client.DefaultRequestHeaders.Accept.Add(
        //            new MediaTypeWithQualityHeaderValue("application/json"));
        //        var response = _client.GetAsync(URL).Result;
        //        var responses = response.Content.ReadAsStringAsync().Result;
        //        serviceResponse.Errcode = ((int)response.StatusCode);
        //        serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
        //        CreateLog($" Response:{responses}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return serviceResponse;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CreateLog($" Error: {ex.Message}");
        //        throw ex;
        //    }

        //    return serviceResponse;
        //}

        public async Task<ServiceResponse<string>> FetchingGetPassCode(string Details, string Token, string Servertype, string FacilityCode, string Instance)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                //CreateLog("STO WaybillGetPass Code" + Details + ": " + Token);
                var client = new HttpClient();
                var request = new HttpRequestMessage();
                if (Servertype.ToLower() == "qa")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "http://stgsleepyhead2.unicommerce.com/services/rest/v1/purchase/gatepass/search");
                    }
                    else if (Instance.ToLower() == "dfx")
                    {

                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgmyduroflexworld.unicommerce.com/services/rest/v1/purchase/gatepass/search");

                    }
                }
                else if (Servertype.ToLower() == "prod")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "http://sleepyhead.unicommerce.com/services/rest/v1/purchase/gatepass/search");
                    }
                    else if (Instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://myduroflexworld.unicommerce.co.in/services/rest/v1/purchase/gatepass/search");

                    }
                }
                //if (ServerType.ToLower() == "qa")
                //{
                //    request = new HttpRequestMessage(HttpMethod.Post, "http://stgsleepyhead2.unicommerce.com/services/rest/v1/purchase/gatepass/search");
                //}
                //else if (ServerType.ToLower() == "prod")
                //{
                //    request = new HttpRequestMessage(HttpMethod.Post, "http://sleepyhead.unicommerce.com/services/rest/v1/purchase/gatepass/search");
                //}
                //var request = new HttpRequestMessage(HttpMethod.Post, "http://stgsleepyhead.unicommerce.com/services/rest/v1/purchase/gatepass/search");
                request.Headers.Add("Facility", FacilityCode);
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(Details, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                serviceResponse.Errcode = ((int)response.StatusCode);
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<string>> FetchingGetPassElements(string Details, string Token, string Servertype, string FacilityCode, string Instance)
        {

            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                //CreateLog("STO WaybillGetPass Code" + Details + ": " + Token);
                var client = new HttpClient();
                var request = new HttpRequestMessage();

                if (Servertype.ToLower() == "qa")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "http://stgsleepyhead2.unicommerce.com/services/rest/v1/purchase/gatepass/get");
                    }
                    else if (Instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgmyduroflexworld.unicommerce.com/services/rest/v1/purchase/gatepass/get");

                    }
                }
                else if (Servertype.ToLower() == "prod")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "http://sleepyhead.unicommerce.com/services/rest/v1/purchase/gatepass/get");
                    }
                    else if (Instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://myduroflexworld.unicommerce.co.in/services/rest/v1/purchase/gatepass/get");

                    }
                }
                //if (ServerType.ToLower() == "qa")
                //{
                //    request = new HttpRequestMessage(HttpMethod.Post, "http://stgsleepyhead2.unicommerce.com/services/rest/v1/purchase/gatepass/get");
                //}
                //else if (ServerType.ToLower() == "prod")
                //{
                //    request = new HttpRequestMessage(HttpMethod.Post, "http://sleepyhead.unicommerce.com/services/rest/v1/purchase/gatepass/get");
                //}
                //var request = new HttpRequestMessage(HttpMethod.Post, "http://stgsleepyhead.unicommerce.com/services/rest/v1/purchase/gatepass/get");
                request.Headers.Add("Facility", FacilityCode);
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(Details, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                serviceResponse.Errcode = ((int)response.StatusCode);
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync(); ;
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Response:{JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                //return responses;
                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    //GetCode(Details, Token);
                }
            }
            catch (Exception ex)
            {
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }
        //public async Task<ServiceResponse<string>> GetSTOSkuDetails(string SkuCode, string Token)
        //{
        //    ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
        //    try
        //    {
        //        CreateLog("STO itemType_Get -" + SkuCode + ": " + Token);
        //        var client = new HttpClient();
        //        var request = new HttpRequestMessage(HttpMethod.Post, "https://stgsleepyhead.unicommerce.com/services/rest/v1/catalog/itemType/get");
        //        request.Headers.Add("Authorization", "Bearer" + Token);
        //        var content = new StringContent(SkuCode, null, "application/json");
        //        request.Content = content;
        //        var response = await client.SendAsync(request);
        //        //response.EnsureSuccessStatusCode();
        //        serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
        //        CreateLog($" Response: {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
        //        //return responses;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            serviceResponse.Errcode = ((int)response.StatusCode);
        //            return serviceResponse;
        //        }
        //        else
        //        {
        //            serviceResponse.Errcode = ((int)response.StatusCode);
        //            //GetSkuDetails(SkuCode, Token);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CreateLog($"Error: {ex.Message}");
        //        throw ex;
        //    }
        //    return serviceResponse;
        //}
        //public async Task<ServiceResponse<string>> WaybillSTOPostDataDeliverypackList(string jsonre)//List<PostDataSTOWaybill> data
        //{

        //    ServiceResponse<string> serviceResponse = new ServiceResponse<string>();

        //    //var jsonre = JsonConvert.SerializeObject(new { data = data });
        //    string _credentials = "system+demoduro@pando.ai:Pandowelcome@123";
        //    CreateLog($" Post Data to Pando waybill STO:-  {jsonre}");
        //    //var dejson=JsonConvert.DeserializeObject<Datum>(jsonre);
        //    try
        //    {
        //        var client = new HttpClient();
        //        var credentials = Encoding.ASCII.GetBytes(_credentials);
        //        var request = new HttpRequestMessage(HttpMethod.Post, "https://duroflex-mitm.gopando.in/inbound/api/transactions/optima/delivery-picklist");

        //        request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(credentials));
        //        var content = new StringContent(jsonre, null, "application/json");
        //        request.Content = content;
        //        var response = await client.SendAsync(request);
        //        serviceResponse.Errcode = ((int)response.StatusCode);
        //        serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
        //        CreateLog($" Response- : {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            serviceResponse.Errcode = ((int)response.StatusCode);
        //            return serviceResponse;
        //        }
        //        else
        //        {
        //            serviceResponse.Errcode = ((int)response.StatusCode);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CreateLog("$ Error: " + ex.Message);
        //        throw ex;
        //    }
        //    return serviceResponse;

        //}

        //public async Task<ServiceResponse<string>> STOPApiostDataDeliverypackList(string jsonre)
        //{

        //    ServiceResponse<string> serviceResponse = new ServiceResponse<string>();

        //    //var jsonre = JsonConvert.SerializeObject(new { data = data.ObjectParam });
        //    string _credentials = "system+demoduro@pando.ai:Pandowelcome@123";
        //    CreateLog($" Post Data to Pando waybill STO:-  {jsonre}");
        //    //var dejson=JsonConvert.DeserializeObject<Datum>(jsonre);
        //    try
        //    {
        //        var client = new HttpClient();
        //        var credentials = Encoding.ASCII.GetBytes(_credentials);
        //        var request = new HttpRequestMessage(HttpMethod.Post, "https://duroflex-mitm.gopando.in/inbound/api/transactions/optima/delivery-picklist");

        //        request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(credentials));
        //        var content = new StringContent(jsonre, null, "application/json");
        //        request.Content = content;
        //        var response = await client.SendAsync(request);
        //        serviceResponse.Errcode = ((int)response.StatusCode);
        //        serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
        //        CreateLog($" Response- : {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            serviceResponse.Errcode = ((int)response.StatusCode);
        //            return serviceResponse;
        //        }
        //        else
        //        {
        //            serviceResponse.Errcode = ((int)response.StatusCode);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CreateLog("$ Error: " + ex.Message);
        //        throw ex;
        //    }
        //    return serviceResponse;

        //}

        public async Task<ServiceResponse<string>> PostUpdateShippingpckg(UpdateShippingpackage data, string Token, string FacilityCode, string Servertype, string Instance)
        {

            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();

            //var jsonre = JsonConvert.SerializeObject(new { data = data });
            var jsonre = JsonConvert.SerializeObject(data);
            CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Update Shipping package Post Data:-  {jsonre}");
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage();
                //if (Servertype.ToLower() == "qa")
                //{
                //    request = new HttpRequestMessage(HttpMethod.Post, "https://stgsleepyhead2.unicommerce.com/services/rest/v1/oms/shippingPackage/edit");
                //}
                //else if (Servertype.ToLower() == "prod")
                //{
                //    request = new HttpRequestMessage(HttpMethod.Post, "https://sleepyhead.unicommerce.com/services/rest/v1/oms/shippingPackage/edit");
                //}

                if (Servertype.ToLower() == "qa")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgsleepyhead2.unicommerce.com/services/rest/v1/oms/shippingPackage/edit");
                    }
                    else if (Instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgmyduroflexworld.unicommerce.com/services/rest/v1/oms/shippingPackage/edit");

                    }
                }
                else if (Servertype.ToLower() == "prod")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://sleepyhead.unicommerce.com/services/rest/v1/oms/shippingPackage/edit");
                    }
                    else if (Instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://myduroflexworld.unicommerce.co.in/services/rest/v1/oms/shippingPackage/edit");

                    }
                }
                //var request = new HttpRequestMessage(HttpMethod.Post, "https://stgsleepyhead.unicommerce.com/services/rest/v1/oms/shippingPackage/edit");
                request.Headers.Add("Facility", FacilityCode);
                request.Headers.Add("Authorization", "Bearer " + Token);
                var content = new StringContent(jsonre, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                serviceResponse.Errcode = ((int)response.StatusCode);
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, shippingPackageCode {data.shippingPackageCode}, Update Shipping Response- : {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                dynamic datas = JsonConvert.DeserializeObject(serviceResponse.ObjectParam);

                if (response.IsSuccessStatusCode)
                {
                    //serviceResponse.Errcode = ((int)response.StatusCode);
                    //return serviceResponse;
                    var datastatus = (bool)datas.successful;
                    if (datastatus)
                    {
                        serviceResponse.Errcode = ((int)response.StatusCode);
                        serviceResponse.IsSuccess = true;
                        return serviceResponse;
                    }
                    else
                    {
                        serviceResponse.Errcode = ((int)response.StatusCode);
                        serviceResponse.IsSuccess = false;
                        serviceResponse.Errdesc = datas.errors[0].description;
                        return serviceResponse;
                    }
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    serviceResponse.IsSuccess = false;
                    serviceResponse.Errdesc = datas.errors[0].description;

                }
            }
            catch (Exception ex)
            {
                CreateLog($" Error:{ex.Message}");
                throw ex;
            }
            return serviceResponse;

        }
        public async Task<ServiceResponse<string>> PostAllocateShipping(Allocateshipping data, string Token, string FacilityCode, string ServerType, string Instance)
        {

            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();

            //var jsonre = JsonConvert.SerializeObject(new { data = data });
            var jsonre = JsonConvert.SerializeObject(data);
            //string _credentials = "system+demoduro@pando.ai:Pandowelcome@123";
            CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Allocate Shipping Post Data:-  {jsonre}");
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage();
                //if (ServerType.ToLower() == "qa")
                //{
                //    request = new HttpRequestMessage(HttpMethod.Post, "https://stgsleepyhead2.unicommerce.com/services/rest/v1/oms/shippingPackage/allocateShippingProvider");
                //}
                //else if (ServerType.ToLower() == "prod")
                //{
                //    request = new HttpRequestMessage(HttpMethod.Post, "https://sleepyhead.unicommerce.com/services/rest/v1/oms/shippingPackage/allocateShippingProvider");
                //}

                if (ServerType.ToLower() == "qa")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgsleepyhead2.unicommerce.com/services/rest/v1/oms/shippingPackage/allocateShippingProvider");
                    }
                    else if (Instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgmyduroflexworld.unicommerce.com/services/rest/v1/oms/shippingPackage/allocateShippingProvider");

                    }
                }
                else if (ServerType.ToLower() == "prod")
                {
                    if (Instance.ToLower() == "sh")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://sleepyhead.unicommerce.com/services/rest/v1/oms/shippingPackage/allocateShippingProvider");
                    }
                    else if (Instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://myduroflexworld.unicommerce.co.in/services/rest/v1/oms/shippingPackage/allocateShippingProvider");

                    }
                }

                request.Headers.Add("Facility", FacilityCode);
                //request.Headers.Add("Facility", "Hosur_Avigna");
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(jsonre, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                serviceResponse.Errcode = ((int)response.StatusCode);
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                dynamic datas = JsonConvert.DeserializeObject(serviceResponse.ObjectParam);

                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()},shippingPackageCode:- {data.shippingPackageCode} ,Allocate Shipping Response- : {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                //if (response.IsSuccessStatusCode)
                //{
                //    serviceResponse.Errcode = ((int)response.StatusCode);
                //    serviceResponse.IsSuccess=true;
                //    return serviceResponse;
                //}
                //else
                //{
                //    serviceResponse.Errcode = ((int)response.StatusCode);
                //    serviceResponse.IsSuccess = false;
                //}

                if (response.IsSuccessStatusCode)
                {
                    var datastatus = (bool)datas.successful;
                    if (datastatus)
                    {
                        serviceResponse.Errcode = ((int)response.StatusCode);
                        serviceResponse.IsSuccess = true;
                        return serviceResponse;
                    }
                    else
                    {
                        serviceResponse.Errcode = ((int)response.StatusCode);
                        serviceResponse.IsSuccess = false;
                        serviceResponse.Errdesc = datas.errors[0].description;
                        return serviceResponse;
                    }
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    serviceResponse.IsSuccess = false;
                    serviceResponse.Errdesc = datas.errors[0].description;

                }
            }
            catch (Exception ex)
            {
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Error:{ex.Message}");
                throw ex;
            }
            return serviceResponse;

        }
        public async Task<ServiceResponse<string>> DeleteDataTomaterialinvoice(string data)
        {

            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            //var jsonre = JsonConvert.SerializeObject(new { data = data });
            string _credentials = "system+demoduro@pando.ai:Pandowelcome@123";
            CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()},DeleteMaterial Invoice Request- : {data}");

            try
            {
                var client = new HttpClient();
                var credentials = Encoding.ASCII.GetBytes(_credentials);
                var request = new HttpRequestMessage(HttpMethod.Delete, "https://duroflex-mitm.gopando.in/inbound/api/transactions/order/material-invoice ");

                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(credentials));
                var content = new StringContent(data, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);

                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                CreateLog($"DateTime:-   {DateTime.Now.ToLongTimeString()} , Response- : {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");

                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);

                }
            }
            catch (Exception ex)
            {
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Error:{ex.Message}");
                throw ex;
            }
            return serviceResponse;

        }

        public async Task<ServiceResponse<string>> ReversePickUp(string Details, string Token, string Facility, string ServerType, string Instance)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage();


                if (ServerType.ToLower() == "qa")
                {
                    if (Instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://stgmyduroflexworld.unicommerce.com/services/rest/v1/oms/reversePickup/edit");

                    }
                }
                else if (ServerType.ToLower() == "prod")
                {
                    if (Instance.ToLower() == "dfx")
                    {
                        request = new HttpRequestMessage(HttpMethod.Post, "https://myduroflexworld.unicommerce.co.in/services/rest/v1/oms/reversePickup/edit");

                    }
                }




                request.Headers.Add("Facility", Facility);
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(Details, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                serviceResponse.Errcode = ((int)response.StatusCode);
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                CreateLog($" DateTime:-  {DateTime.Now.ToLongTimeString()}, Response:-  {serviceResponse.ObjectParam}");

                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    //GetCode(Details, Token);
                }
            }
            catch (Exception ex)
            {
                CreateLog($" Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<string>> TrackingStatus(TrackingStatus AllData, string Facility, string ServerType, string Instance)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                var Token = GetTokens(ServerType, Instance).Result;
                var accesstoken = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(Token.ObjectParam).access_token;
                CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()}, Tracking Status Post Data:-  {JsonConvert.SerializeObject(AllData)} , FacilityCode:- {Facility}");

                using (HttpClient client = new HttpClient())
                {
                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, GetAPIURL(ServerType, Instance)))
                    {
                        request.Headers.Add("Facility", Facility);
                        request.Headers.Add("Authorization", "Bearer" + accesstoken);
                        request.Content = new StringContent(JsonConvert.SerializeObject(AllData), null, "application/json");
                        var response = await client.SendAsync(request);
                        serviceResponse.Errcode = ((int)response.StatusCode);
                        serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                        CreateLog($"DateTime:-  {DateTime.Now.ToLongTimeString()},Tracking Number:- {AllData.trackingNumber}, Tracking Status Response:-  {serviceResponse.ObjectParam}");
                        if (((int)response.StatusCode) == 200)
                        {
                            dynamic datas = JsonConvert.DeserializeObject(serviceResponse.ObjectParam);

                            if (response.IsSuccessStatusCode)
                            {
                                var datastatus = (bool)datas.successful;
                                if (datastatus)
                                {
                                    serviceResponse.Errcode = ((int)response.StatusCode);
                                    serviceResponse.IsSuccess = true;
                                    //serviceResponse.Errdesc = datas.errors[0].message;
                                    return serviceResponse;
                                }
                                else
                                {
                                    serviceResponse.Errcode = ((int)response.StatusCode);
                                    serviceResponse.IsSuccess = false;
                                    serviceResponse.Errdesc = datas.errors[0].description;
                                    return serviceResponse;
                                }
                            }
                            else
                            {
                                serviceResponse.Errcode = ((int)response.StatusCode);
                                serviceResponse.IsSuccess = false;
                                serviceResponse.Errdesc = datas.errors[0].description;

                            }
                        }
                        else
                        {
                            serviceResponse.Errcode = ((int)response.StatusCode);
                            serviceResponse.IsSuccess = false;
                            serviceResponse.Errdesc = serviceResponse.ObjectParam;

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                CreateLog($" Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }


        public string GetAPIURL(string ServerType, string Instance)
        {

            if (ServerType.ToLower() == "qa")
            {
                if (Instance.ToLower() == "sh")
                {
                    return "https://stgsleepyhead2.unicommerce.com/services/rest/v1/oms/updateShipmentTrackingStatus";
                }
                else if (Instance.ToLower() == "dfx")
                {
                    return "https://stgmyduroflexworld.unicommerce.com/services/rest/v1/oms/updateShipmentTrackingStatus";

                }
                else { return ""; }
            }
            else if (ServerType.ToLower() == "prod")
            {
                if (Instance.ToLower() == "sh")
                {
                    return "https://sleepyhead.unicommerce.com/services/rest/v1/oms/updateShipmentTrackingStatus";
                }
                else if (Instance.ToLower() == "dfx")
                {
                    return "https://myduroflexworld.unicommerce.co.in/services/rest/v1/oms/updateShipmentTrackingStatus";

                }
                else { return ""; }
            }
            else { return ""; }

        }
        public async Task<ServiceResponse<string>> PostReversePickUp(string Details, string Token, string Facility, string ServerType)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage();

                CreateLog($" DateTime:-  {DateTime.Now.ToLongTimeString()}, Reverse PickUp SendDate:-  {serviceResponse.ObjectParam}");

                if (ServerType.ToLower() == "qa")
                {
                    request = new HttpRequestMessage(HttpMethod.Post, "https://stgmyduroflexworld.unicommerce.com/services/rest/v1/oms/reversePickup/edit");
                }
                else if (ServerType.ToLower() == "prod")
                {
                    request = new HttpRequestMessage(HttpMethod.Post, "https://myduroflexworld.unicommerce.co.in/services/rest/v1/oms/reversePickup/edit");
                }
                request.Headers.Add("Facility", Facility);
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(Details, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                serviceResponse.Errcode = ((int)response.StatusCode);
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                dynamic StatuaDta = JsonConvert.DeserializeObject(serviceResponse.ObjectParam);
                CreateLog($" DateTime:-  {DateTime.Now.ToLongTimeString()}, Reverse PickUp Response:-  {serviceResponse.ObjectParam}");
                if (response.IsSuccessStatusCode)
                {
                    var datastatus = (bool)StatuaDta.successful;
                    if (datastatus)
                    {
                        serviceResponse.Errcode = ((int)response.StatusCode);
                        serviceResponse.IsSuccess = true;
                        return serviceResponse;
                    }
                    else
                    {
                        serviceResponse.Errcode = ((int)response.StatusCode);
                        serviceResponse.IsSuccess = false;
                        serviceResponse.Errdesc = StatuaDta.errors[0].description;
                        return serviceResponse;
                    }
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    serviceResponse.IsSuccess = false;
                    serviceResponse.Errdesc = StatuaDta.errors[0].description;

                }




                //if (response.IsSuccessStatusCode)
                //{
                //    serviceResponse.Errcode = ((int)response.StatusCode);
                //    return serviceResponse;
                //}
                //else
                //{
                //    serviceResponse.Errcode = ((int)response.StatusCode);
                //    //GetCode(Details, Token);
                //}
            }
            catch (Exception ex)
            {
                serviceResponse.Errdesc += ex.ToString();
                serviceResponse.Errcode = 500;
                serviceResponse.IsSuccess = false;
                CreateLog($"Reverse PickUp  Error: {JsonConvert.SerializeObject(serviceResponse)}");

            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> PostReturnAllocateShipping(string Details, string Token, string Facility, string ServerType)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage();

                CreateLog($" DateTime:-  {DateTime.Now.ToLongTimeString()}, Return Allocate SendDate:-  {serviceResponse.ObjectParam}");

                if (ServerType.ToLower() == "qa")
                {
                    request = new HttpRequestMessage(HttpMethod.Post, "https://stgmyduroflexworld.unicommerce.com/services/rest/v1/oms/reversePickup/edit");
                }
                else if (ServerType.ToLower() == "prod")
                {
                    request = new HttpRequestMessage(HttpMethod.Post, "https://myduroflexworld.unicommerce.co.in/services/rest/v1/oms/reversePickup/edit");
                }
                request.Headers.Add("Facility", Facility);
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(Details, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                serviceResponse.Errcode = ((int)response.StatusCode);
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                dynamic StatuaDta = JsonConvert.DeserializeObject(serviceResponse.ObjectParam);
                CreateLog($" DateTime:-  {DateTime.Now.ToLongTimeString()}, Return Allocate Response:-  {serviceResponse.ObjectParam}");
                if (response.IsSuccessStatusCode)
                {
                    var datastatus = (bool)StatuaDta.successful;
                    if (datastatus)
                    {
                        serviceResponse.Errcode = ((int)response.StatusCode);
                        serviceResponse.IsSuccess = true;
                        return serviceResponse;
                    }
                    else
                    {
                        serviceResponse.Errcode = ((int)response.StatusCode);
                        serviceResponse.IsSuccess = false;
                        serviceResponse.Errdesc = StatuaDta.errors[0].description;
                        return serviceResponse;
                    }
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    serviceResponse.IsSuccess = false;
                    serviceResponse.Errdesc = StatuaDta.errors[0].description;

                }
            }
            catch (Exception ex)
            {
                serviceResponse.Errdesc += ex.ToString();
                serviceResponse.Errcode = 500;
                serviceResponse.IsSuccess = false;
                CreateLog($"Reverse PickUp  Error: {JsonConvert.SerializeObject(serviceResponse)}");

            }
            return serviceResponse;
        }
    }
}
